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
 *  Copyright (c) 2012 - 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatues
 *  
 *  File: IClaimsProvider.cs
 *
 *  Purpose: Provides ClaimsPrincipal and Claims for an individual user
 *
 *  Date        Name                Reason
 *  02/08/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;
using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;

namespace SharedPluginFeatures
{
    /// <summary>
    /// IClaimsProvider returns individual claims and claims principals for a user
    /// </summary>
    public interface IClaimsProvider
    {
        /// <summary>
        /// Retrieves a dictionary of ClaimsPrincipal, along with a dictionary of principal name/value pairs.
        /// </summary>
        /// <returns>IReadOnlyDictionary&lt;string, IReadOnlyDictionary&lt;string, string&gt;&gt;</returns>
        List<ClaimsIdentity> GetUserClaims(in long userId);

        /// <summary>
        /// Retrieves default properties to be used with IAuthenticationService interface
        /// </summary>
        /// <returns>AuthenticationProperties</returns>
        AuthenticationProperties GetAuthenticationProperties();
    }
}
