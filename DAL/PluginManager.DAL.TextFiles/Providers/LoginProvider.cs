/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: LoginProvider.cs
 *
 *  Purpose:  ILoginProvider for text based storage
 *
 *  Date        Name                Reason
 *  25/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Middleware;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Tables;
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Providers
{
    internal class LoginProvider : ILoginProvider
    {
        private readonly ISimpleDBOperations<UserDataRow> _users;
        private readonly ISimpleDBOperations<ExternalUsersDataRow> _externalUsers;
        private readonly string _encryptionKey;

        public LoginProvider(ISimpleDBOperations<UserDataRow> users, ISimpleDBOperations<ExternalUsersDataRow> externalUsers, ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            SimpleDBSettings settings = settingsProvider.GetSettings<SimpleDBSettings>(nameof(SimpleDBSettings));

            if (settings == null)
                throw new InvalidOperationException();

            _encryptionKey = settings.EnycryptionKey ?? throw new InvalidOperationException("Encryption key missing from settings!");
            _users = users ?? throw new ArgumentNullException(nameof(users));
            _externalUsers = externalUsers ?? throw new ArgumentNullException(nameof(externalUsers));
        }

        public LoginResult Login(in string username, in string password, in string ipAddress,
            in byte attempts, ref UserLoginDetails loginDetails)
        {
            ExternalUsersDataRow externalUser = _externalUsers.Select(loginDetails.UserId);

            if (externalUser != null)
            {
                loginDetails.Username = externalUser.UserName;
                loginDetails.Email = externalUser.Email;
                return LoginResult.Remembered;
            }

            UserDataRow tableUser = _users.Select(loginDetails.UserId);

            if (loginDetails.RememberMe && tableUser != null)
            {

                loginDetails.Username = tableUser.FullName;
                loginDetails.Email = tableUser.Email;
                loginDetails.UserId = tableUser.Id;
                return LoginResult.Remembered;
            }

            string loginName = username;
            string encryptedPassword = Shared.Utilities.Encrypt(password, _encryptionKey);

            tableUser = _users.Select().FirstOrDefault(u => u.Email.Equals(loginName, StringComparison.OrdinalIgnoreCase));

            if (tableUser != null && tableUser.Password.Equals(encryptedPassword))
            {
                if (tableUser.Locked)
                {
                    return LoginResult.AccountLocked;
                }

                loginDetails.Username = tableUser.FullName;
                loginDetails.Email = tableUser.Email;
                loginDetails.UserId = tableUser.Id;

                if (tableUser.PasswordExpire < DateTime.Now)
                    return LoginResult.PasswordChangeRequired;

                return LoginResult.Success;
            }


            if (tableUser != null && attempts > 4)
            {
                tableUser.UnlockCode = Shared.Utilities.GetRandomPassword(8);
                _users.Update(tableUser);

                return LoginResult.AccountLocked;
            }

            return LoginResult.InvalidCredentials;
        }

        public LoginResult Login(in ITokenUserDetails tokenUserDetails, ref UserLoginDetails loginDetails)
        {
            if (tokenUserDetails == null)
                throw new ArgumentNullException(nameof(tokenUserDetails));

            if (String.IsNullOrEmpty(tokenUserDetails.Email))
                throw new ArgumentNullException(nameof(tokenUserDetails));

            if (String.IsNullOrEmpty(tokenUserDetails.Provider))
                throw new ArgumentNullException(nameof(tokenUserDetails));

            if (tokenUserDetails.Verify)
            {
                return VerifyExternalUser(tokenUserDetails);
            }
            else
            {
                return LoginExternalUser(tokenUserDetails, ref loginDetails);
            }
        }

        public bool UnlockAccount(in string username, in string unlockCode)
        {
            if (String.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            if (String.IsNullOrEmpty(unlockCode))
                throw new ArgumentNullException(nameof(unlockCode));

            string loginName = username;

            UserDataRow tableUser = _users.Select()
                .FirstOrDefault(u => u.Email.Equals(loginName, StringComparison.OrdinalIgnoreCase));

            if (tableUser == null)
                return false;

            if (!tableUser.Locked)
                return false;

            if (tableUser.UnlockCode.Equals(unlockCode))
            {
                tableUser.UnlockCode = String.Empty;
                _users.Update(tableUser);

                return true;
            }

            return false;
        }

        public bool ForgottenPassword(in string username)
        {
            throw new NotImplementedException();
        }

        public void RemoveExternalUser(ITokenUserDetails tokenUserDetails)
        {
            if (tokenUserDetails == null)
                throw new ArgumentNullException(nameof(tokenUserDetails));

            ExternalUsersDataRow externalUser = _externalUsers.Select()
                .FirstOrDefault(eu => eu.Provider.Equals(tokenUserDetails.Provider, StringComparison.OrdinalIgnoreCase) &&
                    eu.Token.Equals(tokenUserDetails.Id));

            if (externalUser != null)
                _externalUsers.Delete(externalUser);
        }

        #region Private Methods

        private LoginResult LoginExternalUser(ITokenUserDetails tokenUserDetails, ref UserLoginDetails loginDetails)
        {
            ExternalUsersDataRow externalUser = new ExternalUsersDataRow()
            {
                Email = tokenUserDetails.Email,
                UserName = tokenUserDetails.Name ?? tokenUserDetails.Email,
                Provider = tokenUserDetails.Provider,
                Token = tokenUserDetails.Id
            };

            _externalUsers.Insert(externalUser);

            loginDetails = new UserLoginDetails()
            {
                UserId = externalUser.Id,
                Email = externalUser.Email,
                Username = externalUser.UserName,
                RememberMe = true
            };

            return LoginResult.Success;
        }

        private LoginResult VerifyExternalUser(ITokenUserDetails tokenUserDetails)
        {
            ExternalUsersDataRow externalUser = _externalUsers.Select()
                .FirstOrDefault(eu => eu.Provider.Equals(tokenUserDetails.Provider, StringComparison.OrdinalIgnoreCase) &&
                    eu.Token.Equals(tokenUserDetails.Id));

            if (externalUser == null)
                return LoginResult.InvalidCredentials;
            else
                return LoginResult.Success;
        }

        #endregion Private Methods
    }
}
