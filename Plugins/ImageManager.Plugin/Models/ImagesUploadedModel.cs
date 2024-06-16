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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Image Manager Plugin
 *  
 *  File: ImagesUploadedModel.cs
 *
 *  Date        Name                Reason
 *  20/05/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace ImageManager.Plugin.Models
{
	/// <summary>
	/// Cached image upload data
	/// </summary>
	public sealed class ImagesUploadedModel
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <exception cref="ArgumentNullException">Thrown if groupName is null or empty.</exception>
		public ImagesUploadedModel(string groupName, string subgroupName, string memoryCacheName)
		{
			if (String.IsNullOrEmpty(groupName))
				throw new ArgumentNullException(nameof(groupName));

			if (String.IsNullOrEmpty(memoryCacheName))
				throw new ArgumentNullException(nameof(memoryCacheName));

			GroupName = groupName;
			SubgroupName = subgroupName;
			MemoryCacheName = memoryCacheName;
		}

		/// <summary>
		/// Name of group images are being uploaded to
		/// </summary>
		/// <value>string</value>
		public string GroupName { get; }

		/// <summary>
		/// Name of subgroup images are being uploaded to
		/// </summary>
		/// <value>string</value>
		public string SubgroupName { get; }

		/// <summary>
		/// Name of memory cache item for uploaded images
		/// </summary>
		public string MemoryCacheName { get; set; }
	}
}
