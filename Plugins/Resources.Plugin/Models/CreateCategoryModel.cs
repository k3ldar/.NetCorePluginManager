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
 *  File: CreateCategoryModel.cs
 *
 *  Purpose:  Model used for creating a new category
 *
 *  Date        Name                Reason
 *  12/09/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

using SharedPluginFeatures;

namespace Resources.Plugin.Models
{
	/// <summary>
	/// Model used to create categories
	/// </summary>
	public sealed class CreateCategoryModel : BaseModel
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="baseModelData"></param>
		/// <param name="parentId">Id of parent category or zero</param>
		public CreateCategoryModel(BaseModelData baseModelData, long parentId)
			: base(baseModelData)
		{
			ParentId = parentId;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="baseModelData"></param>
		/// <param name="parentId">Parent category id</param>
		/// <param name="name">Category name</param>
		/// <param name="description">Category description</param>
		public CreateCategoryModel(BaseModelData baseModelData, long parentId, string name, string description)
			: base(baseModelData)
		{
			ParentId = parentId;
			Name = name;
			Description = description;
		}

		/// <summary>
		/// Default Constructor
		/// </summary>
		public CreateCategoryModel()
			: base()
		{
		}

		/// <summary>
		/// Name of category
		/// </summary>
		[Required]
		[Display(Name = nameof(Languages.LanguageStrings.Name))]
		[StringLength(30, MinimumLength = 5)]
		public string Name { get; set; }

		/// <summary>
		/// Description for category
		/// </summary>
		[Required]
		[Display(Name = nameof(Languages.LanguageStrings.Description))]
		[StringLength(100, MinimumLength = 15)]
		public string Description { get; set; }

		/// <summary>
		/// Parent category id
		/// </summary>
		public long ParentId { get; set; }
	}
}
