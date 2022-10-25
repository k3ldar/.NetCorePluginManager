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
using System.ComponentModel.DataAnnotations;

using SharedPluginFeatures;

namespace Resources.Plugin.Models
{
	/// <summary>
	/// Resource model
	/// </summary>
	public sealed class ResourceEditCategoryModel : BaseModel
	{
		/// <summary>
		/// Standard Constructor
		/// </summary>
		public ResourceEditCategoryModel()
		{

		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="modelData">Generic model data</param>
		/// <param name="id">Unique id for resource</param>
		/// <param name="name">Resource name</param>
		/// <param name="description">Resource description</param>
		/// <param name="foreColor">Resource text color</param>
		/// <param name="backColor">Back color for resource</param>
		/// <param name="image">Image to be displayed in background for the resource</param>
		/// <param name="routeName">Route friendly name</param>
		/// <param name="isVisible">Indicates whether the category is visible or not</param>
		/// <param name="parentId">Parent id of category or zero if no parent found</param>
		/// <param name="allCategories">List of all categories except current category</param>
		public ResourceEditCategoryModel(BaseModelData modelData, long id, string name, string description, string foreColor,
			string backColor, string image, string routeName, bool isVisible, long parentId,
			List<NameIdModel> allCategories)
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
			AllCategories = allCategories ?? throw new ArgumentNullException(nameof(allCategories));
		}

		/// <summary>
		/// Unique Id for resource
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Name of resource
		/// </summary>
		/// <value>string</value>
		[Required]
		[Display(Name = nameof(Languages.LanguageStrings.Name))]
		[StringLength(30, MinimumLength = 5)]
		public string Name { get; set; }

		/// <summary>
		/// Resource description
		/// </summary>
		/// <value>string</value>
		[Required]
		[Display(Name = nameof(Languages.LanguageStrings.Description))]
		[StringLength(100, MinimumLength = 15)]
		public string Description { get; set; }

		/// <summary>
		/// Resource color
		/// </summary>
		/// <value>string</value>
		[Required]
		public string ForeColor { get; set; } = "#000";

		/// <summary>
		/// Resource color
		/// </summary>
		/// <value>string</value>
		[Required]
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
		public bool IsVisible { get; set; }

		/// <summary>
		/// Parent id or zero if no parent
		/// </summary>
		[Display(Name = nameof(Languages.LanguageStrings.Parent))]
		public long ParentId { get; set; }

		/// <summary>
		/// List of all categories
		/// </summary>
		public List<NameIdModel> AllCategories { get; }
	}
}
