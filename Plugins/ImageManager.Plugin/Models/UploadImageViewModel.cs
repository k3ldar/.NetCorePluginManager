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
 *  File: UploadImageViewModel.cs
 *
 *  Date        Name                Reason
 *  13/05/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

using SharedPluginFeatures;

namespace ImageManager.Plugin.Models
{
	/// <summary>
	/// View model for uploading images
	/// </summary>
	public class UploadImageViewModel : BaseModel
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public UploadImageViewModel()
			: base()
		{
			Files = [];
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="modelData">Valid BaseModelData instance</param>
		/// <exception cref="ArgumentNullException">Thrown if modelData is null</exception>
		public UploadImageViewModel(BaseModelData modelData)
			: base(modelData)
		{
			Files = [];
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="modelData">Valid BaseModelData instance</param>
		/// <param name="groupName">Name of group for uploaded images</param>
		/// <param name="subgroupName">Name of subgroup for uploaded images</param>
		/// <exception cref="ArgumentNullException">Thrown if modelData is null</exception>
		/// <exception cref="ArgumentNullException">Thrown if groupName is null or empty</exception>
		public UploadImageViewModel(BaseModelData modelData, string groupName, string subgroupName)
			: this(modelData)
		{
			if (String.IsNullOrEmpty(groupName))
				throw new ArgumentNullException(nameof(groupName));

			GroupName = groupName;
			SubgroupName = subgroupName;
		}

		/// <summary>
		/// List of files to be uploaded
		/// </summary>
		/// <value>List&lt;IFormFile&gt;</value>
		public List<IFormFile> Files { get; set; }

		/// <summary>
		/// Name of group where image is being uploaded to
		/// </summary>
		/// <value>string</value>
		public string GroupName { get; set; }

		/// <summary>
		/// Name of subgroup where image is being uploaded to
		/// </summary>
		/// <value>string</value>
		public string SubgroupName { get; set; }
	}
}
