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
 *  Product:  SharedPluginFeatures
 *  
 *  File: IUserCultureChangeProvider.cs
 *
 *  Purpose:  Provides interface for Managing user culture changes
 *
 *  Date        Name                Reason
 *  14/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Globalization;

using Microsoft.AspNetCore.Http;

using Shared.Classes;

namespace SharedPluginFeatures
{
    /// <summary>
    /// Provides a mechanism for updating the current user session with updated culture information.
    /// 
    /// This interface is registered within the DI container and is implemented by UserSessionMiddleware.Plugin module.
    /// </summary>
    public interface IUserCultureChangeProvider
    {
        /// <summary>
        /// Indicates the current culture has been changed for the user.
        /// </summary>
        /// <param name="httpContext">Valid HttpContext</param>
        /// <param name="userSession">UserSession whos culture has changed.</param>
        /// <param name="cultureInfo">Culture being used by the user.</param>
        void CultureChanged(in HttpContext httpContext, in UserSession userSession, in CultureInfo cultureInfo);
    }
}
