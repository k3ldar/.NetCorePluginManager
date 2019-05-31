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
 *  Product:  PluginMiddleware
 *  
 *  File: ILoginProvider.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  16/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace Middleware
{
    /// <summary>
    /// Login provider used to manage user logons for the website.
    /// 
    /// This item must be implemented by the host application and made available via DI.
    /// </summary>
    public interface ILoginProvider
    {
        /// <summary>
        /// Login attempt by a user.
        /// </summary>
        /// <param name="username">Name or email address of user attempting to login.</param>
        /// <param name="password">Password for user attempting to login.</param>
        /// <param name="ipAddress">Ip Address of user attempting to login.</param>
        /// <param name="attempts">Number of previous login attempts.</param>
        /// <param name="loginDetails">out.  Login details for the user.</param>
        /// <returns>LoginResult</returns>
        LoginResult Login(in string username, in string password, in string ipAddress, 
            in byte attempts, ref UserLoginDetails loginDetails);

        /// <summary>
        /// Instruction to unlock the account for a user.
        /// </summary>
        /// <param name="username">Name or email address of user whos account needs unlocking.</param>
        /// <param name="unlockCode">Unlock code provided by the user.</param>
        /// <returns>bool.  True if the account was unlocked.</returns>
        bool UnlockAccount(in string username, in string unlockCode);

        /// <summary>
        /// Forgotten password request for a user.
        /// </summary>
        /// <param name="username">Name or email address of user who is requesting a new password.</param>
        /// <returns>bool.  True if a reminder or change password was sent to the user.</returns>
        bool ForgottenPassword(in string username);
    }
}
