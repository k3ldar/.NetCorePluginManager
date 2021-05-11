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
 *  Product:  ImageManager Plugin
 *  
 *  File: DefaultImageProvider.cs
 *
 *  Purpose:  Image provider
 *
 *  Date        Name                Reason
 *  17/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Extensions.Hosting;

using Middleware.Images;
using Middleware.Interfaces;

using PluginManager.Abstractions;

using SharedPluginFeatures;

namespace ImageManager.Plugin.Classes
{
    /// <summary>
    /// Default image provider, used by default when no other image provider is registered.
    /// 
    /// The default provider is designed to work with physical folders and files
    /// </summary>
    public class DefaultImageProvider : IImageProvider
    {
        #region Private Members

        private readonly string _rootPath;

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="hostEnvironment">IHostEnvironment instance</param>
        /// <param name="settingsProvider">ISettingsProvider instance</param>
        public DefaultImageProvider(IHostEnvironment hostEnvironment, ISettingsProvider settingsProvider)
        {
            if (hostEnvironment == null)
                throw new ArgumentNullException(nameof(hostEnvironment));

            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            ImageManagerSettings imageManagerSettings = settingsProvider.GetSettings<ImageManagerSettings>(nameof(ImageManager));

            _rootPath = imageManagerSettings.ImagePath;

            if (String.IsNullOrEmpty(_rootPath))
            {
                _rootPath = Path.Combine(hostEnvironment.ContentRootPath, "wwwroot", "images");
            }
        }

        #endregion Constructors

        #region IImageProvider Methods

        /// <summary>
        /// Retrieves a list of available image groups
        /// </summary>
        /// <returns>List&lt;string&gt;</returns>
        public Dictionary<string, List<string>> Groups()
        {
            Dictionary<string, List<string>> Result = new Dictionary<string, List<string>>();

            foreach (string group in Directory.GetDirectories(_rootPath, Constants.Asterix.ToString(), SearchOption.TopDirectoryOnly))
            {
                List<string> subGroups = new List<string>();

                foreach (string subGroup in Directory.GetDirectories(group, Constants.Asterix.ToString(), SearchOption.TopDirectoryOnly))
                {
                    subGroups.Add(Path.GetFileName(subGroup));
                }

                Result.Add(Path.GetFileName(group), subGroups);
            }

            return Result;
        }

        /// <summary>
        /// Retrieves a list of all images within an image group
        /// </summary>
        /// <param name="groupName">Name of group where images will be retrieved from</param>
        /// <exception cref="ArgumentNullException">Thrown if groupName is null or an empty string.</exception>
        /// <returns>List&lt;ImageFile&gt;</returns>
        public List<ImageFile> Images(string groupName)
        {
            if (groupName == null)
                throw new ArgumentNullException(nameof(groupName));

            string groupPath = String.IsNullOrEmpty(groupName) ? _rootPath : Path.Combine(_rootPath, groupName);

            if (!Directory.Exists(groupPath))
                throw new ArgumentException($"{groupName} does not exist");

            List<ImageFile> Result = new List<ImageFile>();

            foreach (string file in Directory.GetFiles(groupPath, "*", SearchOption.TopDirectoryOnly))
            {
                string uriFile = file.Substring(_rootPath.Length + 1).Replace("\\", "/");

                Uri uri = new Uri($"/images/{uriFile}", UriKind.RelativeOrAbsolute);
                Result.Add(new ImageFile(uri, file));
            }

            return Result;
        }

        /// <summary>
        /// Retrieves a list of all images within an image group
        /// </summary>
        /// <param name="groupName">Name of group where images will be retrieved from</param>
        /// <param name="subgroupName">Name of subgroup where images reside, or null if only require group images.</param>
        /// <exception cref="ArgumentNullException">Thrown if groupName is null or an empty string.</exception>
        /// <returns>List&lt;ImageFile&gt;</returns>
        public List<ImageFile> Images(string groupName, string subgroupName)
        {
            if (String.IsNullOrEmpty(groupName))
                throw new ArgumentNullException(nameof(groupName));

            if (String.IsNullOrEmpty(subgroupName))
                throw new ArgumentNullException(nameof(subgroupName));

            string groupPath = Path.Combine(_rootPath, groupName, subgroupName);

            if (!Directory.Exists(groupPath))
                throw new ArgumentException($"{groupName} does not exist");

            List<ImageFile> Result = new List<ImageFile>();

            foreach (string file in Directory.GetFiles(groupPath, "*", SearchOption.TopDirectoryOnly))
            {
                string uriFile = file.Substring(_rootPath.Length + 1).Replace("\\", "/");

                Uri uri = new Uri($"/images/{uriFile}", UriKind.RelativeOrAbsolute);
                Result.Add(new ImageFile(uri, file));
            }

            return Result;
        }

        /// <summary>
        /// Creates an image group, an image group will logically co-locate images which are naturally grouped.
        /// </summary>
        /// <param name="groupName">Name of group to create</param>
        /// <exception cref="ArgumentNullException">Thrown if groupName is null or an empty string.</exception>
        /// <exception cref="InvalidOperationException">Thrown if groupName already exists</exception>
        /// <returns>bool</returns>
        public bool CreateGroup(string groupName)
        {
            if (String.IsNullOrEmpty(groupName))
                throw new ArgumentNullException(nameof(groupName));

            string groupPath = Path.Combine(_rootPath, groupName);

            if (Directory.Exists(groupPath))
                return false;

            Directory.CreateDirectory(groupPath);

            return Directory.Exists(groupPath);
        }

        /// <summary>
        /// Deletes an image group.
        /// </summary>
        /// <param name="groupName">Name of group to delete.</param>
        /// <exception cref="ArgumentNullException">Thrown if groupName is null or an empty string.</exception>
        /// <returns>bool</returns>
        public bool DeleteGroup(string groupName)
        {
            if (String.IsNullOrEmpty(groupName))
                throw new ArgumentNullException(nameof(groupName));

            string groupPath = Path.Combine(_rootPath, groupName);

            if (!Directory.Exists(groupPath))
                return false;

            Directory.Delete(groupPath, true);

            return !Directory.Exists(groupPath);
        }

        /// <summary>
        /// Determines whether a group exists or not
        /// </summary>
        /// <param name="groupName">Name of group to find if exists</param>
        /// <returns>bool</returns>
        public bool GroupExists(string groupName)
        {
            if (String.IsNullOrEmpty(groupName))
                throw new ArgumentNullException(nameof(groupName));

            string groupPath = Path.Combine(_rootPath, groupName);

            return Directory.Exists(groupPath);
        }

        /// <summary>
        /// Adds a new subgroup to an existing image group
        /// </summary>
        /// <param name="groupName">Name of group under which the subgroup will be added.</param>
        /// <param name="subgroupName">Name of subgroup to add.</param>
        /// <returns>bool</returns>
        public bool AddSubgroup(string groupName, string subgroupName)
        {
            if (String.IsNullOrEmpty(groupName))
                throw new ArgumentNullException(nameof(groupName));

            if (String.IsNullOrEmpty(subgroupName))
                throw new ArgumentNullException(nameof(subgroupName));

            if (SubgroupExists(groupName, subgroupName))
                return false;

            string groupPath = Path.Combine(_rootPath, groupName, subgroupName);

            Directory.CreateDirectory(groupPath);

            return SubgroupExists(groupName, subgroupName);
        }

        /// <summary>
        /// Deletes a subgroup and all image files contained within the subgroup.
        /// </summary>
        /// <param name="groupName">Name of group where the subgroup resides.</param>
        /// <param name="subgroupName">Name of subgroup to be deleted.</param>
        /// <returns>bool</returns>
        public bool DeleteSubgroup(string groupName, string subgroupName)
        {
            if (String.IsNullOrEmpty(groupName))
                throw new ArgumentNullException(nameof(groupName));

            if (String.IsNullOrEmpty(subgroupName))
                throw new ArgumentNullException(nameof(subgroupName));

            if (!SubgroupExists(groupName, subgroupName))
                return false;

            string groupPath = Path.Combine(_rootPath, groupName, subgroupName);

            Directory.Delete(groupPath, true);

            return !SubgroupExists(groupName, subgroupName);
        }

        /// <summary>
        /// Determines whether a subgroup exists or not
        /// </summary>
        /// <param name="groupName">Name of group that should contain subgroup.</param>
        /// <param name="subgroupName">Name of subgroup whose existence is being verified.</param>
        /// <returns>bool</returns>
        public bool SubgroupExists(string groupName, string subgroupName)
        {
            if (String.IsNullOrEmpty(groupName))
                throw new ArgumentNullException(nameof(groupName));

            if (String.IsNullOrEmpty(subgroupName))
                throw new ArgumentNullException(nameof(subgroupName));

            string groupPath = Path.Combine(_rootPath, groupName, subgroupName);

            return Directory.Exists(groupPath);
        }

        /// <summary>
        /// Determines whether an image exists within a group
        /// </summary>
        /// <param name="groupName">Name of group where images will be found.</param>
        /// <param name="imageName">Name of image</param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentNullException">Thrown if groupName is null or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if subgroupName is null or an empty string.</exception>
        public bool ImageExists(string groupName, string imageName)
        {
            if (String.IsNullOrEmpty(groupName))
                throw new ArgumentNullException(nameof(groupName));

            if (String.IsNullOrEmpty(imageName))
                throw new ArgumentNullException(nameof(imageName));

            string fileName = Path.Combine(_rootPath, groupName, imageName);

            return File.Exists(fileName);
        }

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
        public bool ImageExists(string groupName, string subgroupName, string imageName)
        {
            if (String.IsNullOrEmpty(groupName))
                throw new ArgumentNullException(nameof(groupName));

            if (String.IsNullOrEmpty(subgroupName))
                throw new ArgumentNullException(nameof(subgroupName));

            if (String.IsNullOrEmpty(imageName))
                throw new ArgumentNullException(nameof(imageName));

            string fileName = Path.Combine(_rootPath, groupName, subgroupName, imageName);

            return File.Exists(fileName);
        }

        /// <summary>
        /// Deletes an image from within a group
        /// </summary>
        /// <param name="groupName">Name of group where images will be found.</param>
        /// <param name="imageName">Name of image</param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentNullException">Thrown if groupName is null or an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown if imageName is null or an empty string.</exception>
        public bool ImageDelete(string groupName, string imageName)
        {
            if (String.IsNullOrEmpty(groupName))
                throw new ArgumentNullException(nameof(groupName));

            if (String.IsNullOrEmpty(imageName))
                throw new ArgumentNullException(nameof(imageName));

            string fileName = Path.Combine(_rootPath, groupName, imageName);

            if (!File.Exists(fileName))
                return false;

            File.Delete(fileName);

            return !File.Exists(fileName);
        }

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
        public bool ImageDelete(string groupName, string subgroupName, string imageName)
        {
            if (String.IsNullOrEmpty(groupName))
                throw new ArgumentNullException(nameof(groupName));

            if (String.IsNullOrEmpty(subgroupName))
                throw new ArgumentNullException(nameof(subgroupName));

            if (String.IsNullOrEmpty(imageName))
                throw new ArgumentNullException(nameof(imageName));

            string fileName = Path.Combine(_rootPath, groupName, subgroupName, imageName);

            if (!File.Exists(fileName))
                return false;

            File.Delete(fileName);

            return !File.Exists(fileName);
        }

        #endregion IImageProvider Methods
    }
}
