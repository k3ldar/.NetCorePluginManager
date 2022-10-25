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
 *  File: CreateResourceItemModel.cs
 *
 *  Purpose:  Model used for creating a new resource item
 *
 *  Date        Name                Reason
 *  27/09/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

using Middleware;

using Resources.Plugin.Controllers;

using SharedPluginFeatures;

namespace Resources.Plugin.Models
{
	/// <summary>
	/// Model used to create resource items
	/// </summary>
	public class CreateResourceItemModel : BaseModel
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="baseModelData"></param>
		/// <param name="parentId">Id of parent category or zero</param>
		public CreateResourceItemModel(BaseModelData baseModelData, long parentId)
			: base(baseModelData)
		{
			ParentId = parentId;
			ResourceType = 3;
			Value = String.Empty;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="baseModelData"></param>
		/// <param name="parentId">Parent category id</param>
		/// <param name="name">Category name</param>
		/// <param name="description">Category description</param>
		public CreateResourceItemModel(BaseModelData baseModelData, long parentId, string name, string description)
			: base(baseModelData)
		{
			ParentId = parentId;
			Name = name;
			Description = description;
		}

		/// <summary>
		/// Default Constructor
		/// </summary>
		public CreateResourceItemModel()
			: base()
		{
		}

		/// <summary>
		/// Name of category / Item
		/// </summary>
		[Required]
		[Display(Name = nameof(Languages.LanguageStrings.Name))]
		[StringLength(ResourcesController.MaximumNameLength, MinimumLength = ResourcesController.MinimumNameLength)]
		public string Name { get; set; }

		/// <summary>
		/// Description for category
		/// </summary>
		[Required]
		[Display(Name = nameof(Languages.LanguageStrings.Description))]
		[StringLength(ResourcesController.MaximumDescriptionLength, MinimumLength = ResourcesController.MinimumDescriptionLength)]
		public string Description { get; set; }

		/// <summary>
		/// Resource item value
		/// </summary>
		[Display(Name = nameof(Languages.LanguageStrings.Value))]
		public string Value { get; set; }

		/// <summary>
		/// Parent category id
		/// </summary>
		public long ParentId { get; set; }

		/// <summary>
		/// Type of resource
		/// </summary>
		[Required]
		[Display(Name = nameof(Languages.LanguageStrings.ResourceType))]
		public int ResourceType { get; set; }

		/// <summary>
		/// All resource item types
		/// </summary>
		public List<NameIdModel> AllResourceTypes
		{ 
			get
			{
				List<NameIdModel> Result = new List<NameIdModel>();

				foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
				{
					Result.Add(new NameIdModel((long)resourceType, resourceType.ToString()));
				}

				return Result;
			}
		}
	}
}
