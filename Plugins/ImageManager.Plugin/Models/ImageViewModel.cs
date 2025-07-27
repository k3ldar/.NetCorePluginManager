/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Image Manager Plugin
 *  
 *  File: ImageViewModel.cs
 *
 *  Date        Name                Reason
 *  21/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Middleware.Images;

using SharedPluginFeatures;

namespace ImageManager.Plugin.Models
{
	/// <summary>
	/// View model used when viewing images using <see cref="Controllers.ImageManagerController"/>
	/// </summary>
	public sealed class ImageViewModel : BaseModel
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public ImageViewModel()
		{

		}

		/// <summary>
		/// Constructor for use when displaying data within controller
		/// </summary>
		/// <param name="modelData"></param>
		/// <param name="canManageImages">Determines whether the user can manage image data or not (add, delete etc)</param>
		/// <param name="selectedGroupName">Name of group, or empty string if root path</param>
		/// <param name="selectedSubgroupName">Name of subgroup, or empty string if no subgroup is being highlighted</param>
		/// <param name="selectedImageFile">The selected image file if applicable, otherwise null</param>
		/// <param name="groups">List of all groups</param>
		/// <param name="imageFiles">List of images that belong to the group</param>
		public ImageViewModel(in IBaseModelData modelData,
			bool canManageImages,
			string selectedGroupName,
			string selectedSubgroupName,
			ImageFile selectedImageFile,
			Dictionary<string,
			List<string>> groups,
			List<ImageFile> imageFiles)
			: base(modelData)
		{
			CanManageImages = canManageImages;
			SelectedGroupName = selectedGroupName ?? throw new ArgumentNullException(nameof(selectedGroupName));
			SelectedSubgroupName = selectedSubgroupName ?? throw new ArgumentNullException(nameof(selectedSubgroupName));
			SelectedImageFile = selectedImageFile;
			Groups = groups ?? throw new ArgumentNullException(nameof(groups));
			ImageFiles = imageFiles ?? throw new ArgumentNullException(nameof(imageFiles));
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Name of group
		/// </summary>
		/// <value>string</value>
		public string SelectedGroupName { get; set; }

		/// <summary>
		/// Name of subgroup
		/// </summary>
		/// <value>string</value>
		public string SelectedSubgroupName { get; set; }

		/// <summary>
		/// The currently selected image file, if applicable
		/// </summary>
		public ImageFile SelectedImageFile { get; set; }

		/// <summary>
		/// List of all image groups
		/// </summary>
		/// <value>List&lt;string&gt;</value>
		public Dictionary<string, List<string>> Groups { get; set; }

		/// <summary>
		/// List of images that belong to the group
		/// </summary>
		/// <value>List&lt;ImageFile&gt;</value>
		public List<ImageFile> ImageFiles { get; }

		/// <summary>
		/// Determines whether images can be managed by the current user
		/// </summary>
		public bool CanManageImages { get; }

		#endregion Properties
	}
}
