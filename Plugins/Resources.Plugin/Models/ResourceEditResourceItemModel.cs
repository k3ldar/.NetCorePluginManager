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
 *  Product:  Resources.Plugin
 *  
 *  File: ResourceEditResourceItemModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  21/10/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Middleware;

using Resources.Plugin.Controllers;

using SharedPluginFeatures;

namespace Resources.Plugin.Models
{
	/// <summary>
	/// Model for editing a resource item
	/// </summary>
	public class ResourceEditResourceItemModel : BaseResourceItemModel
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public ResourceEditResourceItemModel()
		{

		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="modelData">Generic model data</param>
		/// <param name="id">Unique id for resource item</param>
		/// <param name="categoryId">Category id</param>
		/// <param name="resourceType">Resource type</param>
		/// <param name="userId">Id of user who created the resource</param>
		/// <param name="userName">Name of user creating the resource</param>
		/// <param name="name">Name of Resource Item</param>
		/// <param name="description">Description</param>
		/// <param name="value">Resource item value</param>
		/// <param name="approved">Approved for public viewing</param>
		/// <param name="tags">Resource item tags</param>
		/// <param name="allCategories">List of all categories except current category</param>
		public ResourceEditResourceItemModel(BaseModelData modelData, long id, long categoryId, ResourceType resourceType, 
			long userId, string userName, string name, string description, string value, bool approved,
			string tags, List<NameIdModel> allCategories)
			: base(modelData)
		{
			Id = id;
			CategoryId = categoryId;
			ResourceType = resourceType;
			UserId = userId;
			UserName = userName;
			Name = name;
			Description = description;
			Value = value;
			Approved = approved;
			Tags = tags;
			AllCategories = allCategories;
		}

		/// <summary>
		/// Unique id for resource item
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Category Id
		/// </summary>
		public long CategoryId { get; set; }

		/// <summary>
		/// Resource type
		/// </summary>
		public ResourceType ResourceType { get; set; }

		/// <summary>
		/// Id of user creating the resource item
		/// </summary>
		public long UserId { get; set; }

		/// <summary>
		/// Name of user creating the resource
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Name of resource item
		/// </summary>
		[Required]
		[Display(Name = nameof(Languages.LanguageStrings.Name))]
		[StringLength(ResourcesController.MaximumNameLength, MinimumLength = ResourcesController.MinimumNameLength)]
		public string Name { get; set; }

		/// <summary>
		/// Description of resource item
		/// </summary>
		[Required]
		[Display(Name = nameof(Languages.LanguageStrings.Description))]
		[StringLength(ResourcesController.MaximumDescriptionLength, MinimumLength = ResourcesController.MinimumDescriptionLength)]
		public string Description { get; set; }

		/// <summary>
		/// Value of resource item
		/// </summary>
		[Required]
		public string Value { get; set; }

		/// <summary>
		/// Approved for public viewing
		/// </summary>
		public bool Approved { get; set; }

		/// <summary>
		/// List of all categories
		/// </summary>
		public List<NameIdModel> AllCategories { get; }

		/// <summary>
		/// Resource item tags
		/// </summary>
		public string Tags { get; set; }
	}
}
