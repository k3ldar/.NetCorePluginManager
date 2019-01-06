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
 *  File: UserLoginDetails.cs
 *
 *  Purpose:  User login details
 *
 *  Date        Name                Reason
 *  10/11/2018  Simon Carter        Initially Created
 *  16/12/2018  Simon Carter        Moved to PluginMiddleware library
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace Middleware
{
    public sealed class UserLoginDetails
    {
        #region Constructors

        public UserLoginDetails()
        {

        }

        public UserLoginDetails(long userId)
        {
            UserId = userId;
        }

        public UserLoginDetails(in long userId, in bool rememberMe)
            : this (userId)
        {
            RememberMe = rememberMe;
        }

        #endregion Constructors

        #region Properties

        public long UserId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool RememberMe { get; set; }

        #endregion Properties
    }
}
