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
 *  Product:  Plugin Middleware
 *  
 *  File: ResourceCategory.cs
 *
 *  Purpose:  Resource category
 *
 *  Date        Name                Reason
 *  29/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

namespace Middleware.Resources
{
	/// <summary>
	/// Resource class
	/// </summary>
	public sealed class ResourceCategory
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id">Unique id for resource category</param>
		/// <param name="parentId">Parent id</param>
		/// <param name="name">Resource name</param>
		/// <param name="description">Resource description</param>
		/// <param name="foreColor">Resource text color</param>
		/// <param name="backColor">Resource back color</param>
		/// <param name="image">Image to be used for background of resource</param>
		/// <param name="routeName">Route friendly name for resource</param>
		/// <param name="resourceItems">List of resource items belonging to the category</param>
		/// <param name="isVisible">Indicates whether the category is visible to users or not</param>
		public ResourceCategory(long id, long parentId, string name, string description, string foreColor, string backColor, string image, string routeName, bool isVisible, List<ResourceItem> resourceItems)
		{
			Id = id;
			ParentId = parentId;
			ResourceItems = resourceItems ?? throw new ArgumentNullException(nameof(resourceItems));
			Name = name;
			Description = description;
			ForeColor = foreColor;
			BackColor = backColor;
			Image = image;
			RouteName = routeName;
			IsVisible = isVisible;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id">Unique id for resource category</param>
		/// <param name="parentId">Id of parent category or zero</param>
		/// <param name="name">Resource name</param>
		/// <param name="description">Resource description</param>
		/// <param name="foreColor">Resource text color</param>
		/// <param name="backColor">Resource back color</param>
		/// <param name="image">Image to be used for background of resource</param>
		/// <param name="routeName">Route friendly name for resource</param>
		/// <param name="isVisible">Indicates whether the category is visible to users or not</param>
		public ResourceCategory(long id, long parentId, string name, string description, string foreColor, string backColor, string image, string routeName, bool isVisible)
			: this(id, parentId, name, description, foreColor, backColor, image, routeName, isVisible, new List<ResourceItem>())
		{
		}

		/// <summary>
		/// Unique id for resource category
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Id of parent category or zero
		/// </summary>
		public long ParentId { get; set; }

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
		/// Resource forecolor
		/// </summary>
		/// <value>string</value>
		public string ForeColor { get; set; } = "#000";
		/// <summary>
		/// Resource forecolor
		/// </summary>
		/// <value>string</value>
		public string BackColor { get; set; } = "#FFFFFF";

		/// <summary>
		/// Image to be displayed in the background for the resource
		/// </summary>
		public string Image { get; set; }

		/// <summary>
		/// Route name for resource
		/// </summary>
		public string RouteName { get; }

		/// <summary>
		/// Indicates whether the category is visible to users or not
		/// </summary>
		public bool IsVisible { get; }

		/// <summary>
		/// List of resource items
		/// </summary>
		public List<ResourceItem> ResourceItems { get; }
	}
}
