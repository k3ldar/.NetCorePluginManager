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
 *  File: ManageCategoryModel.cs
 *
 *  Purpose:  Model used for managing categories
 *
 *  Date        Name                Reason
 *  22/09/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

namespace Resources.Plugin.Models
{
	/// <summary>
	/// Model for managing classes
	/// </summary>
	public sealed class ManageCategoryModel : BaseModel
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="baseModelData"></param>
		/// <param name="growlMessage"></param>
		/// <param name="categories"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public ManageCategoryModel(BaseModelData baseModelData, string growlMessage, List<ResourceCategoryModel> categories)
			: base (baseModelData)
		{
			GrowlMessage = growlMessage;
			Categories = categories ?? throw new ArgumentNullException(nameof(categories));
		}

		/// <summary>
		/// GrowlMessage
		/// </summary>
		public string GrowlMessage { get; }

		/// <summary>
		/// List of all categories
		/// </summary>
		public List<ResourceCategoryModel> Categories { get; }
	}
}
