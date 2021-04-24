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
 *  Copyright (c) 2012 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: IImageProvider.cs
 *
 *  Purpose:  Image provider
 *
 *  Date        Name                Reason
 *  17/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System.Collections.Generic;

using Middleware.Images;

namespace Middleware.Interfaces
{
    /// <summary>
    /// Image provider interface, provides methods used to manage images within the system
    /// </summary>
    public interface IImageProvider
    {
        /// <summary>
        /// Creates an image group, an image group will logically co-locate images which are naturally grouped.
        /// </summary>
        /// <param name="groupName">Name of group to create</param>
        /// <exception cref="ArgumentNullException">Thrown if groupName is null or an empty string.</exception>
        /// <returns>bool</returns>
        bool CreateGroup(string groupName);

        /// <summary>
        /// Deletes an image group.
        /// </summary>
        /// <param name="groupName">Name of group to delete.</param>
        /// <exception cref="ArgumentNullException">Thrown if groupName is null or an empty string.</exception>
        /// <returns>bool</returns>
        bool DeleteGroup(string groupName);

        /// <summary>
        /// Retrieves a list of available image groups
        /// </summary>
        /// <returns>List&lt;string&gt;</returns>
        List<string> Groups();

        /// <summary>
        /// Retrieves a list of all images within an image group
        /// </summary>
        /// <param name="groupName">Name of group where images will be retrieved from</param>
        /// <exception cref="ArgumentNullException">Thrown if groupName is null or an empty string.</exception>
        /// <returns>List&lt;ImageFile&gt;</returns>
        List<ImageFile> Images(string groupName);
    }
}
