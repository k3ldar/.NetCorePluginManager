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

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Hosting;

using Middleware.Images;
using Middleware.Interfaces;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

namespace ImageManager.Plugin.Classes
{
	/// <summary>
	/// Default image provider, used by default when no other image provider is registered.
	/// 
	/// The default provider is designed to work with physical folders and files
	/// </summary>
	public class DefaultImageProvider : BaseCoreClass, IImageProvider
	{
		#region Private Members

		private const string TempPathName = "Temp";
		private readonly string _rootPath;
		private static readonly CacheManager _imageProviderCache = new(nameof(DefaultImageProvider), new TimeSpan(12, 0, 0), false, true);

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
			string CacheNameGroups = "Default Image Provider - Groups";
			CacheItem groupsCache = _imageProviderCache.Get(CacheNameGroups);

			if (groupsCache == null)
			{
				Dictionary<string, List<string>> Result = new();

				foreach (string group in Directory.GetDirectories(_rootPath, Constants.Asterix.ToString(), SearchOption.TopDirectoryOnly))
				{
					List<string> subGroups = new();

					foreach (string subGroup in Directory.GetDirectories(group, Constants.Asterix.ToString(), SearchOption.TopDirectoryOnly))
					{
						subGroups.Add(Path.GetFileName(subGroup));
					}

					Result.Add(Path.GetFileName(group), subGroups);
				}

				groupsCache = new CacheItem(CacheNameGroups, Result);
				_imageProviderCache.Add(CacheNameGroups, groupsCache);
			}

			return (Dictionary<string, List<string>>)groupsCache.Value;
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

			string cacheNameImageGroup = $"Default Image Provider Image Group - {groupName}";
			CacheItem imageCache = _imageProviderCache.Get(cacheNameImageGroup);

			if (imageCache == null)
			{
				List<ImageFile> Result = new();

				foreach (string file in Directory.GetFiles(groupPath, "*", SearchOption.TopDirectoryOnly))
				{
					string uriFile = file.Substring(_rootPath.Length + 1).Replace("\\", "/");

					Uri uri = new($"/images/{uriFile}", UriKind.RelativeOrAbsolute);
					Result.Add(new ImageFile(uri, file));
				}

				imageCache = new CacheItem(cacheNameImageGroup, Result);
				_imageProviderCache.Add(cacheNameImageGroup, imageCache);
			}

			return (List<ImageFile>)imageCache.Value;
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

			string groupPath = Path.Combine(_rootPath, ValidateUserInput(groupName, ValidationType.Path), ValidateUserInput(subgroupName, ValidationType.Path));

			if (!Directory.Exists(groupPath))
				return new List<ImageFile>();

			string cacheNameImageGroup = $"Default Image Provider Image Subgroup - {groupName} {subgroupName}";
			CacheItem imageCache = _imageProviderCache.Get(cacheNameImageGroup);

			if (imageCache == null)
			{
				List<ImageFile> Result = new();

				foreach (string file in Directory.GetFiles(groupPath, "*", SearchOption.TopDirectoryOnly))
				{
					string uriFile = file.Substring(_rootPath.Length + 1).Replace("\\", "/");

					Uri uri = new($"/images/{uriFile}", UriKind.RelativeOrAbsolute);
					Result.Add(new ImageFile(uri, file));
				}

				imageCache = new CacheItem(cacheNameImageGroup, Result);
				_imageProviderCache.Add(cacheNameImageGroup, imageCache);
			}

			return (List<ImageFile>)imageCache.Value;
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
			_imageProviderCache.Clear();

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

			_imageProviderCache.Clear();

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

			string groupPath = Path.Combine(_rootPath, ValidateUserInput(groupName, ValidationType.Path));

			string cacheNameGroup = $"Default Image Provider Image Group Exists - {groupName}";
			CacheItem imageCache = _imageProviderCache.Get(cacheNameGroup);

			if (imageCache == null)
			{
				imageCache = new CacheItem(cacheNameGroup, Directory.Exists(groupPath));
				_imageProviderCache.Add(cacheNameGroup, imageCache);
			}

			return (bool)imageCache.Value;
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

			_imageProviderCache.Clear();

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

			_imageProviderCache.Clear();

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

			string groupPath = Path.Combine(_rootPath, ValidateUserInput(groupName, ValidationType.Path), ValidateUserInput(subgroupName, ValidationType.Path));

			string cacheNameGroup = $"Default Image Provider Image Subgroup Exists - {groupName} {subgroupName}";
			CacheItem imageCache = _imageProviderCache.Get(cacheNameGroup);

			if (imageCache == null)
			{
				imageCache = new CacheItem(cacheNameGroup, Directory.Exists(groupPath));
				_imageProviderCache.Add(cacheNameGroup, imageCache);
			}

			return (bool)imageCache.Value;
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

			string cacheNameImage = $"Default Image Provider ImageExists - {groupName} {imageName}";
			CacheItem imageCache = _imageProviderCache.Get(cacheNameImage);

			if (imageCache == null)
			{
				imageCache = new CacheItem(cacheNameImage, File.Exists(fileName));
				_imageProviderCache.Add(cacheNameImage, imageCache);
			}

			return (bool)imageCache.Value;
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

			_imageProviderCache.Clear();

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

			_imageProviderCache.Clear();

			File.Delete(fileName);

			return !File.Exists(fileName);
		}

		/// <summary>
		/// Retreives the name of a file which can be used for temporary storage of image files
		/// </summary>
		/// <returns>string</returns>
		public string TemporaryImageFile(string fileExtension)
		{
			if (String.IsNullOrEmpty(fileExtension))
			{
				throw new ArgumentNullException(nameof(fileExtension));
			}

			if (fileExtension[0] != '.')
				throw new InvalidOperationException();

			string path = Path.Combine(_rootPath, TempPathName);

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			string Result;

			do
			{
				Result = Path.Combine(path, Path.GetRandomFileName());
				Result = Path.ChangeExtension(Result, fileExtension.ToLower());
			}
			while (File.Exists(Result));

			File.Create(Result).Dispose();

			return Result;
		}

		/// <summary>
		/// Adds a file to the specific group or subgroup
		/// </summary>
		/// <param name="groupName">Name of group</param>
		/// <param name="subgroupName">Name of subgroup or null if not applicable</param>
		/// <param name="fileName">Name of file to be saved</param>
		/// <param name="fileContents">Contents of file</param>
		/// <exception cref="ArgumentNullException">Thrown if groupName is null or empty.</exception>
		/// <exception cref="ArgumentNullException">Thrown if fileName is null or empty.</exception>
		/// <exception cref="ArgumentException">Thrown if fileContents length is 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if the file already exists.</exception>
		public void AddFile(string groupName, string subgroupName, string fileName, byte[] fileContents)
		{
			if (String.IsNullOrEmpty(groupName))
				throw new ArgumentNullException(nameof(groupName));

			if (String.IsNullOrEmpty(fileName))
				throw new ArgumentNullException(nameof(fileName));

			if (fileContents.Length < 1)
				throw new InvalidOperationException();

			string newFilePath = Path.Combine(_rootPath, groupName);

			if (!String.IsNullOrEmpty(subgroupName))
				newFilePath = Path.Combine(newFilePath, subgroupName);

			if (!Directory.Exists(newFilePath))
				Directory.CreateDirectory(newFilePath);

			string newFileName = Path.Combine(newFilePath, fileName);

			if (File.Exists(newFileName))
				throw new InvalidOperationException();

			_imageProviderCache.Clear();

			File.WriteAllBytes(newFileName, fileContents);
		}

		#endregion IImageProvider Methods
	}
}
