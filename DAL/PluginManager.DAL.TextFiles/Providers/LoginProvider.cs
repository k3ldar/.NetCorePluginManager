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
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Middleware;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Internal;
using PluginManager.DAL.TextFiles.Tables;

namespace PluginManager.DAL.TextFiles.Providers
{
    internal class LoginProvider : ILoginProvider
    {
        private readonly ITextTableOperations<TableUser> _users;
        private readonly ITextTableOperations<TableExternalUsers> _externalUsers;
        private readonly string _encryptionKey;

        public LoginProvider(ITextTableOperations<TableUser> users, ITextTableOperations<TableExternalUsers> externalUsers, ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            TextFileSettings settings = settingsProvider.GetSettings<TextFileSettings>(nameof(TextFileSettings));

            if (settings == null)
                throw new InvalidOperationException();

            _encryptionKey = settings.EnycryptionKey ?? throw new InvalidOperationException("Encryption key missing from settings!");
            _users = users ?? throw new ArgumentNullException(nameof(users));
            _externalUsers = externalUsers ?? throw new ArgumentNullException(nameof(externalUsers));
        }

        public LoginResult Login(in string username, in string password, in string ipAddress,
            in byte attempts, ref UserLoginDetails loginDetails)
        {
            TableExternalUsers externalUser = _externalUsers.Select(loginDetails.UserId);

            if (externalUser != null)
            {
                loginDetails.Username = externalUser.UserName;
                loginDetails.Email = externalUser.Email;
                return LoginResult.Remembered;
            }

            TableUser tableUser = _users.Select(loginDetails.UserId);

            if (loginDetails.RememberMe && tableUser != null)
            {

                loginDetails.Username = tableUser.FullName;
                loginDetails.Email = tableUser.Email;
                loginDetails.UserId = tableUser.Id;
                return LoginResult.Remembered;
            }

            string loginName = username;
            string encryptedPassword = Shared.Utilities.Encrypt(password, _encryptionKey);

            tableUser = _users.Select()
                .Where(u => u.Email.Equals(loginName, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

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
                tableUser.Locked = true;
                _users.Update(tableUser);

                return LoginResult.AccountLocked;
            }

            return LoginResult.InvalidCredentials;
        }

        public bool UnlockAccount(in string username, in string unlockCode)
        {
            throw new NotImplementedException();
            //return unlockCode == "123456";

        }

        public bool ForgottenPassword(in string username)
        {
            throw new NotImplementedException();
            //return username == "admin";
        }

        public LoginResult Login(in ITokenUserDetails tokenUserDetails, ref UserLoginDetails loginDetails)
        {
            //if (tokenUserDetails == null)
            //    throw new ArgumentNullException(nameof(tokenUserDetails));

            //if (String.IsNullOrEmpty(tokenUserDetails.Email))
            //    throw new ArgumentException(nameof(tokenUserDetails));

            //if (String.IsNullOrEmpty(tokenUserDetails.Provider))
            //    throw new ArgumentNullException(nameof(tokenUserDetails));

            //// in the real world use a proper method for getting id, this is ok as only a mock
            //string stringId = tokenUserDetails.Provider + tokenUserDetails.Id;

            //long id = stringId.GetHashCode();

            //if (tokenUserDetails.Verify)
            //{
            //    if (_externalUsers.ContainsKey(id))
            //        return LoginResult.Success;
            //    else
            //        return LoginResult.InvalidCredentials;
            //}
            //else
            //{
            //    loginDetails = new UserLoginDetails();
            //    loginDetails.UserId = id;
            //    loginDetails.Username = tokenUserDetails.Name ?? tokenUserDetails.Email;
            //    loginDetails.Email = tokenUserDetails.Email;

            //    //_externalUsers[id] = tokenUserDetails.Email;

            //    return LoginResult.Success;
            //}

            throw new NotImplementedException();
        }

        public void RemoveExternalUser(ITokenUserDetails tokenUserDetails)
        {
            throw new NotImplementedException();
        }
    }
}
