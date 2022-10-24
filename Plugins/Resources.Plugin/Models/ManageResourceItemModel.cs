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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Resources.Plugin
 *  
 *  File: ManageResourceItemModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  21/10/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;
using System;

using SharedPluginFeatures;

namespace Resources.Plugin.Models
{
	/// <summary>
	/// Manage resource item model
	/// </summary>
	public class ManageResourceItemModel : BaseModel
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="baseModelData"></param>
		/// <param name="growlMessage"></param>
		/// <param name="resourceItems"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public ManageResourceItemModel(BaseModelData baseModelData, string growlMessage, List<ResourceItemModel> resourceItems)
			: base(baseModelData)
		{
			GrowlMessage = growlMessage;
			ResourceItems = resourceItems ?? throw new ArgumentNullException(nameof(resourceItems));
		}

		/// <summary>
		/// GrowlMessage
		/// </summary>
		public string GrowlMessage { get; }

		/// <summary>
		/// List of all categories
		/// </summary>
		public List<ResourceItemModel> ResourceItems { get; }
	}
}
