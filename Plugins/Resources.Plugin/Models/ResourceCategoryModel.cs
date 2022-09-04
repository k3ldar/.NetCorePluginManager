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
 *  File: ResourceModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  28/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

namespace Resources.Plugin.Models
{
	/// <summary>
	/// Resource model
	/// </summary>
	public sealed class ResourceCategoryModel : BaseModel
	{
		/// <summary>
		/// Standard Constructor
		/// </summary>
		public ResourceCategoryModel()
		{

		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id">Unique id for resource</param>
		/// <param name="name">Resource name</param>
		/// <param name="description">Resource description</param>
		/// <param name="foreColor">Resource text color</param>
		/// <param name="backColor">Back color for resource</param>
		/// <param name="image">Image to be displayed in background for the resource</param>
		/// <param name="routeName">Route friendly name</param>
		public ResourceCategoryModel(long id, string name, string description, string foreColor, string backColor, string image, string routeName)
		{
			Id = id;
			Name = name;
			Description = description;
			ForeColor = foreColor;
			BackColor = backColor;
			Image = image;
			RouteName = routeName;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="modelData">Base model data</param>
		/// <param name="id">Unique id for resource</param>
		/// <param name="name">Resource name</param>
		/// <param name="description">Resource description</param>
		/// <param name="foreColor">Resource text color</param>
		/// <param name="backColor">Back color for resource</param>
		/// <param name="image">Image to be displayed in background for the resource</param>
		/// <param name="routeName">Route friendly name</param>
		/// <param name="resourceItems">List of resource items</param>
		public ResourceCategoryModel(BaseModelData modelData, long id, string name, string description, string foreColor, string backColor, string image, string routeName, List<ResourceItemModel> resourceItems)
			: base(modelData)
		{
			Id = id;
			Name = name;
			Description = description;
			ForeColor = foreColor;
			BackColor = backColor;
			Image = image;
			RouteName = routeName;
			ResourceItems = resourceItems ?? throw new ArgumentNullException(nameof(resourceItems));
		}

		/// <summary>
		/// Unique Id for resource
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Name of resource
		/// </summary>
		/// <value>string</value>
		public string Name { get; set; }

		/// <summary>
		/// Resource description
		/// </summary>
		/// <value>string</value>
		public string Description { get; set; }

		/// <summary>
		/// Resource color
		/// </summary>
		/// <value>string</value>
		public string ForeColor { get; set; } = "#000";

		/// <summary>
		/// Resource color
		/// </summary>
		/// <value>string</value>
		public string BackColor { get; set; } = "#FFFFFF";

		/// <summary>
		/// Image to be displayed in the background for the resource
		/// </summary>
		public string Image { get; set; }

		/// <summary>
		/// Route friendly name for resource
		/// </summary>
		public string RouteName { get; }

		/// <summary>
		/// List of resource items
		/// </summary>
		public List<ResourceItemModel> ResourceItems { get; }
	}
}
