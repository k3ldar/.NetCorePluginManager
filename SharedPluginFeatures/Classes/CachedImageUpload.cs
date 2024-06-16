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
 *  Product:  SharedPluginFeatues
 *  
 *  File: CachedImageUpload.cs
 *
 *  Purpose:  Contains data for file held in memory for uploading
 *
 *  Date        Name                Reason
 *  14/05/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

namespace SharedPluginFeatures
{
	/// <summary>
	/// Cached image upload data
	/// </summary>
	public sealed class CachedImageUpload
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <exception cref="ArgumentNullException">Thrown if groupName is null or empty.</exception>
		public CachedImageUpload(string groupName, string subgroupName)
		{
			if (String.IsNullOrEmpty(groupName))
				throw new ArgumentNullException(nameof(groupName));

			GroupName = groupName;
			SubgroupName = subgroupName;

			Files = new List<string>();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <exception cref="ArgumentNullException">Thrown if groupName is null or empty.</exception>
		public CachedImageUpload(string groupName)
			: this(groupName, null)
		{

		}

		/// <summary>
		/// List of files that have been uploaded
		/// </summary>
		/// <value>List&lt;string&gt;</value>
		public List<string> Files { get; }

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
