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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Login Plugin
 *  
 *  File: MockLoginProvider.cs
 *
 *  Purpose:  Mock ILoginProvider for tesing purpose
 *
 *  Date        Name                Reason
 *  21/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Middleware;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
    public class MockLoginProvider : ILoginProvider
    {
        public LoginResult Login(in string username, in string password, in string ipAddress, 
            in byte attempts, ref UserLoginDetails loginDetails)
        {
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

            if (attempts > 4)
                return LoginResult.AccountLocked;

            return LoginResult.InvalidCredentials;
        }

        public bool UnlockAccount(in string username, in string unlockCode)
        {
            return (unlockCode == "123456");
        }

        public bool ForgottenPassword(in string username)
        {
            return (username == "admin");
        }
    }
}
