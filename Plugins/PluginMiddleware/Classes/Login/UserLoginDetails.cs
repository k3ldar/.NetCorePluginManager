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
    /// <summary>
    /// User login details, used by ILoginProvider interface.
    /// </summary>
    public sealed class UserLoginDetails
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserLoginDetails()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="userId">Unique user id.</param>
        public UserLoginDetails(long userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userId">Unique user id.</param>
        /// <param name="rememberMe">Indicates whether the login should be remembered or not (can include adding a cookie).</param>
        public UserLoginDetails(in long userId, in bool rememberMe)
            : this (userId)
        {
            RememberMe = rememberMe;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Unique user id.
        /// </summary>
        /// <value>long</value>
        public long UserId { get; set; }

        /// <summary>
        /// Name of user.
        /// </summary>
        /// <value>string</value>
        public string Username { get; set; }

        /// <summary>
        /// Email address for the user
        /// </summary>
        /// <value>string</value>
        public string Email { get; set; }

        /// <summary>
        /// Indicates that the login details should be remembered.
        /// </summary>
        /// <value>bool</value>
        public bool RememberMe { get; set; }

        #endregion Properties
    }
}
