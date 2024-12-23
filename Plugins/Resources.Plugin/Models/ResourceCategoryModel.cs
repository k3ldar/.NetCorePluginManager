﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
		/// <param name="isVisible">Indicates whether the category is visible or not</param>
		/// <param name="parentId">Parent id of category or zero if no parent found</param>
		public ResourceCategoryModel(long id, string name, string description, string foreColor,
			string backColor, string image, string routeName, bool isVisible, long parentId)
		{
			Id = id;
			Name = name;
			Description = description;
			ForeColor = foreColor;
			BackColor = backColor;
			Image = image;
			RouteName = routeName;
			IsVisible = isVisible;
			ParentId = parentId;
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
		/// <param name="categories">List of sub categories for the resource</param>
		/// <param name="resourceItems">List of resource items</param>
		/// <param name="isVisible">Indicates whether the category is visible or not</param>
		/// <param name="parentId">Parent id of category or zero if no parent found</param>
		public ResourceCategoryModel(BaseModelData modelData, long id, string name, string description, string foreColor,
			string backColor, string image, string routeName, bool isVisible, long parentId,
			List<ResourceCategoryModel> categories, List<ResourceItemModel> resourceItems)
			: base(modelData)
		{
			Id = id;
			Name = name;
			Description = description;
			ForeColor = foreColor;
			BackColor = backColor;
			Image = image;
			RouteName = routeName;
			IsVisible = isVisible;
			ParentId = parentId;
			Categories = categories ?? throw new ArgumentNullException(nameof(categories));
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
		/// Indicates whether the category is visible or not
		/// </summary>
		public bool IsVisible { get; }

		/// <summary>
		/// Parent id or zero if no parent
		/// </summary>
		public long ParentId { get; }

		/// <summary>
		/// List of resource sub categories
		/// </summary>
		public List<ResourceCategoryModel> Categories { get; }

		/// <summary>
		/// List of resource items
		/// </summary>
		public List<ResourceItemModel> ResourceItems { get; }
	}
}
