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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Demo Website
 *  
 *  File: MockLoginProvider.cs
 *
 *  Purpose:  Mock ILoginProvider for tesing purpose
 *
 *  Date        Name                Reason
 *  21/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Middleware;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
    [ExcludeFromCodeCoverage(Justification = "Code coverage not required for mock classes")]
    public class MockLoginProvider : ILoginProvider
    {
        private readonly Dictionary<long, string> _externalUsers = new Dictionary<long, string>();

        public LoginResult Login(in string username, in string password, in string ipAddress,
            in byte attempts, ref UserLoginDetails loginDetails)
        {
            if (loginDetails == null)
                throw new ArgumentNullException(nameof(loginDetails));

            if (_externalUsers.ContainsKey(loginDetails.UserId))
            {
                loginDetails.Username = _externalUsers[loginDetails.UserId];
                loginDetails.Email = _externalUsers[loginDetails.UserId];
                return LoginResult.Remembered;
            }

            if (loginDetails.RememberMe && loginDetails.UserId == 123)
            {
                loginDetails.Username = "Administrator";
                loginDetails.Email = "admin@nowhere.com";
                loginDetails.UserId = 123;
                return LoginResult.Remembered;
            }

            if (username == "admin" && password == "password")
            {
                loginDetails.Username = "Administrator";
                loginDetails.Email = "admin@nowhere.com";
                loginDetails.UserId = 123;
                return LoginResult.Success;
            }

            if (username == "admin" && password == "changepassword")
            {
                loginDetails.Username = "Administrator";
                loginDetails.Email = "admin@nowhere.com";
                loginDetails.UserId = 124;
                return LoginResult.PasswordChangeRequired;
            }

            if (attempts > 4)
                return LoginResult.AccountLocked;

            return LoginResult.InvalidCredentials;
        }

        public bool UnlockAccount(in string username, in string unlockCode)
        {
            return unlockCode == "123456";
        }

        public bool ForgottenPassword(in string username)
        {
            return username == "admin";
        }

        public LoginResult Login(in ITokenUserDetails tokenUserDetails, ref UserLoginDetails loginDetails)
        {
            if (tokenUserDetails == null)
                throw new ArgumentNullException(nameof(tokenUserDetails));

            if (String.IsNullOrEmpty(tokenUserDetails.Email))
                throw new ArgumentNullException(nameof(tokenUserDetails));

            if (String.IsNullOrEmpty(tokenUserDetails.Provider))
                throw new ArgumentNullException(nameof(tokenUserDetails));

            // in the real world use a proper method for getting id, this is ok as only a mock
            string stringId = tokenUserDetails.Provider + tokenUserDetails.Id;

            long id = stringId.GetHashCode();

            if (tokenUserDetails.Verify)
            {
                if (_externalUsers.ContainsKey(id))
                    return LoginResult.Success;
                else
                    return LoginResult.InvalidCredentials;
            }
            else
            {
                loginDetails = new UserLoginDetails
                {
                    UserId = id,
                    Username = tokenUserDetails.Name ?? tokenUserDetails.Email,
                    Email = tokenUserDetails.Email
                };

                _externalUsers[id] = tokenUserDetails.Email;

                return LoginResult.Success;
            }
        }

        public void RemoveExternalUser(ITokenUserDetails tokenUserDetails)
        {
            throw new NotImplementedException();
        }
    }
}
