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
 *  Product:  PluginMiddleware
 *  
 *  File: ITokenUserDetails.cs
 *
 *  Purpose:  Contains basic user details obtained via an oauth call for a user
 *
 *  Date        Name                Reason
 *  05/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace Middleware
{
    /// <summary>
    /// User details obtained via OAuth
    /// </summary>
    public interface ITokenUserDetails
    {
        /// <summary>
        /// User Id
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Give name
        /// </summary>
        string GivenName { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// Picture
        /// </summary>
        string Picture { get; set; }

        /// <summary>
        /// Provider of the current credentials
        /// </summary>
        string Provider { get; set; }

        /// <summary>
        /// Indicates whether we are just verifying the details or performing a login
        /// </summary>
        bool Verify { get; set; }
    }
}
