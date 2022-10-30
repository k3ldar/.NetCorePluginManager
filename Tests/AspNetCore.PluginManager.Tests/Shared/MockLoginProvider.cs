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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockLoginProvider.cs
 *
 *  Purpose:  Mock ILoginProvider class
 *
 *  Date        Name                Reason
 *  21/08/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using Middleware;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class MockLoginProvider : ILoginProvider
    {
        public bool ForgottenPassword(in string username)
        {
            if (username == "user not found")
                return false;

            return true;
        }

        public LoginResult Login(in string username, in string password, in string ipAddress, in byte attempts, ref UserLoginDetails loginDetails)
        {
            if (loginDetails.UserId == 999)
                throw new Exception("An Error");

            if (username == "Account" && password == "Locked")
                return LoginResult.AccountLocked;

            if (username == "password" && password == "change required")
                return LoginResult.PasswordChangeRequired;

            if (username == "login" && password == "success")
                return LoginResult.Success;

            if (loginDetails.RememberMe && loginDetails.UserId == 123)
            {
                loginDetails.Email = "joe@bloggs.com";
                loginDetails.Username = "Joe Bloggs";
                return LoginResult.Remembered;
            }

            return LoginResult.InvalidCredentials;
        }

        public LoginResult Login(in ITokenUserDetails tokenUserDetails, ref UserLoginDetails loginDetails)
        {
            throw new NotImplementedException();
        }

        public void RemoveExternalUser(ITokenUserDetails tokenUserDetails)
        {
            throw new NotImplementedException();
        }

        public bool UnlockAccount(in string username, in string unlockCode)
        {
            if (username == "unlock me" && unlockCode == "123")
                return true;

            return false;
        }
    }
}
