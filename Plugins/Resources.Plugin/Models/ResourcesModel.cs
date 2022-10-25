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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Resources.Plugin
 *  
 *  File: ResourcesModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  04/09/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

namespace Resources.Plugin.Models
{
	/// <summary>
	/// Resources model, contains list of resource categories
	/// </summary>
	public sealed class ResourcesModel : BaseModel
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="modelData"></param>
		/// <param name="resourceCategories"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public ResourcesModel(BaseModelData modelData, List<ResourceCategoryModel> resourceCategories)
			: base(modelData)
		{
			ResourceCategories = resourceCategories ?? throw new ArgumentNullException(nameof(resourceCategories));
		}

		/// <summary>
		/// List of resource categories
		/// </summary>
		public List<ResourceCategoryModel> ResourceCategories { get; }
	}
}
