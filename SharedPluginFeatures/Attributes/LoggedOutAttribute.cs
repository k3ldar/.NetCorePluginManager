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
 *  Product:  SharedPluginFeatures
 *  
 *  File: LoggedOutAttribute.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  08/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
    /// <summary>
    /// This attribute is used in conjunction with the UserSessionMiddleware.Plugin module and indicates that a user
    /// must be logged out when attempting to gain access to the route.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class LoggedOutAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// 
        /// If a logged in user attempts to enter the route, they will be sent to the site home page (/)
        /// </summary>
        public LoggedOutAttribute()
        {
            RedirectPage = "/";
        }

        /// <summary>
        /// Constructor
        /// 
        /// Allows the developer to specify a page that the user will be redirected to if the attempt to navigate to the route whilst logged in.
        /// </summary>
        /// <param name="redirectPage">string.  Url of route the user will be redirected to.</param>
        public LoggedOutAttribute(in string redirectPage)
        {
            if (String.IsNullOrEmpty(redirectPage))
                throw new ArgumentNullException(nameof(redirectPage));

            RedirectPage = redirectPage;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Url of the page the user will be redirected to, should they be logged into the system and attempt to access the route.
        /// </summary>
        public string RedirectPage { get; private set; }

        #endregion Properties
    }
}
