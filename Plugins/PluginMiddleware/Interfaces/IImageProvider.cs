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
 *  Copyright (c) 2012 - 2022 Simon Carter.  All Rights Reserved.
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
using System;
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
        /// Determines whether a group exists or not
        /// </summary>
        /// <param name="groupName">Name of group to find if exists</param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentNullException">Thrown if groupName is null or an empty string.</exception>
        bool GroupExists(string groupName);

        /// <summary>
        /// Retrieves a list of available image groups with subgroups
        /// </summary>
        /// <returns>Dictionary&lt;string, List&lt;string&gt;&gt;</returns>
        Dictionary<string, List<string>> Groups();

        /// <summary>
        /// Retrieves a list of all images within an image group
        /// </summary>
        /// <param name="groupName">Name of group where images will be retrieved from.</param>
        /// <exception cref="ArgumentNullException">Thrown if groupName is null or an empty string.</exception>
        /// <returns>List&lt;ImageFile&gt;</returns>
        /// <exception cref="ArgumentNullException">Thrown if groupName is null or an empty string.</exception>
        List<ImageFile> Images(string groupName);

        /// <summary>
        /// Retrieves a list of all images within an image subgroup
        /// </summary>
        /// <param name="groupName">Name of group where images will be retrieved from.</param>
        /// <param name="subgroupName">Name of subgroup where images will be retrieved from.</param>
        /// <exception cref="ArgumentNullException">Thrown if groupName is null or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if subgroupName is null or an empty string.</exception>
        /// <returns>List&lt;ImageFile&gt;</returns>
        List<ImageFile> Images(string groupName, string subgroupName);

        /// <summary>
        /// Determines whether an image exists within a group
        /// </summary>
        /// <param name="groupName">Name of group where images will be found.</param>
        /// <param name="imageName">Name of image</param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentNullException">Thrown if groupName is null or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if subgroupName is null or an empty string.</exception>
        bool ImageExists(string groupName, string imageName);

        /// <summary>
        /// Determines whether an image exists within a subgroup
        /// </summary>
        /// <param name="groupName">Name of group where images will be found.</param>
        /// <param name="subgroupName">Name of subgroup where image will be found.</param>
        /// <param name="imageName">Name of image</param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentNullException">Thrown if groupName is null or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if subgroupName is null or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if imageName is null or an empty string.</exception>
        bool ImageExists(string groupName, string subgroupName, string imageName);

        /// <summary>
        /// Deletes an image from within a group
        /// </summary>
        /// <param name="groupName">Name of group where images will be found.</param>
        /// <param name="imageName">Name of image</param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentNullException">Thrown if groupName is null or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if imageName is null or an empty string.</exception>
        bool ImageDelete(string groupName, string imageName);

        /// <summary>
        /// Deletes an image from within a subgroup
        /// </summary>
        /// <param name="groupName">Name of group where images will be found.</param>
        /// <param name="subgroupName">Name of subgroup where image will be found.</param>
        /// <param name="imageName">Name of image</param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentNullException">Thrown if groupName is null or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if subgroupName is null or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if imageName is null or an empty string.</exception>
        bool ImageDelete(string groupName, string subgroupName, string imageName);

        /// <summary>
        /// Determines whether a subgroup exists or not
        /// </summary>
        /// <param name="groupName">Name of group that should contain subgroup.</param>
        /// <param name="subgroupName">Name of subgroup whose existence is being verified.</param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentNullException">Thrown if groupName is null or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if subgroupName is null or an empty string.</exception>
        bool SubgroupExists(string groupName, string subgroupName);

        /// <summary>
        /// Adds a new subgroup to an existing image group
        /// </summary>
        /// <param name="groupName">Name of group under which the subgroup will be added.</param>
        /// <param name="subgroupName">Name of subgroup to add.</param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentNullException">Thrown if groupName is null or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if subgroupName is null or an empty string.</exception>
        bool AddSubgroup(string groupName, string subgroupName);

        /// <summary>
        /// Deletes a subgroup and all image files contained within the subgroup.
        /// </summary>
        /// <param name="groupName">Name of group where the subgroup resides.</param>
        /// <param name="subgroupName">Name of subgroup to be deleted.</param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentNullException">Thrown if groupName is null or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if subgroupName is null or an empty string.</exception>
        bool DeleteSubgroup(string groupName, string subgroupName);

        /// <summary>
        /// Retreives the name of a file which can be used for temporary storage of image files
        /// </summary>
        /// <param name="fileExtension">File extension to be used for new file</param>
        /// <returns>string</returns>
        /// <exception cref="ArgumentNullException">Thrown if fileExtension is null or empty</exception>
        /// <exception cref="ArgumentException">Thrown if fileExtension does not start with a period (.)</exception>
        string TemporaryImageFile(string fileExtension);

        /// <summary>
        /// Adds a file to the specific group or subgroup
        /// </summary>
        /// <param name="groupName">Name of group</param>
        /// <param name="subgroupName">Name of subgroup or null if not applicable</param>
        /// <param name="fileName">Name of file to be saved</param>
        /// <param name="fileContents">Contents of file</param>
        /// <exception cref="ArgumentNullException">Thrown if groupName is null or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if fileName is null or empty.</exception>
        /// <exception cref="ArgumentException">Thrown if fileContents length is 0</exception>
        void AddFile(string groupName, string subgroupName, string fileName, byte[] fileContents);
    }
}
