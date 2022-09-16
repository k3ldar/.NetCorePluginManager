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
 *  File: ResourceViewItemModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  16/09/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Middleware;

using SharedPluginFeatures;

namespace Resources.Plugin.Models
{
	/// <summary>
	/// Resource item model
	/// </summary>
	public sealed class ResourceViewItemModel : BaseModel
	{

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="baseModelData">Base model data</param>
		/// <param name="id">Unique id for resource item</param>
		/// <param name="categoryId">Category id</param>
		/// <param name="resourceType">Resource type</param>
		/// <param name="userId">User Id</param>
		/// <param name="userName">Name of user creating the resource</param>
		/// <param name="name">Name of Resource Item</param>
		/// <param name="description">Description</param>
		/// <param name="value">Resource item value</param>
		/// <param name="likes">Number of likes</param>
		/// <param name="dislikes">Number of dislikes</param>
		/// <param name="viewCount">Number of views for resource item</param>
		/// <param name="approved">Approved for public viewing</param>
		public ResourceViewItemModel(BaseModelData baseModelData, long id, long categoryId, ResourceType resourceType, 
			long userId, string userName, string name, string description, string value, int likes, int dislikes, int viewCount, bool approved)
			: base (baseModelData)
		{
			Id = id;
			CategoryId = categoryId;
			ResourceType = resourceType;
			UserId = userId;
			UserName = userName;
			Name = name;
			Description = description;
			Value = value;
			Likes = likes;
			Dislikes = dislikes;
			Approved = approved;
			ViewCount = viewCount;
		}

		/// <summary>
		/// Unique id for resource item
		/// </summary>
		public long Id { get; }

		/// <summary>
		/// Category Id
		/// </summary>
		public long CategoryId { get; }

		/// <summary>
		/// Resource type
		/// </summary>
		public ResourceType ResourceType { get; }

		/// <summary>
		/// Id of user who created the item
		/// </summary>
		public long UserId { get; }

		/// <summary>
		/// Name of user creating the resource
		/// </summary>
		public string UserName { get; }

		/// <summary>
		/// Name of resource item
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Description of resource item
		/// </summary>
		public string Description { get; }

		/// <summary>
		/// Value of resource item
		/// </summary>
		public string Value { get; }

		/// <summary>
		/// Number of likes
		/// </summary>
		public int Likes { get; }

		/// <summary>
		/// Number of dislikes
		/// </summary>
		public int Dislikes { get;  }

		/// <summary>
		/// Number of views for resource item
		/// </summary>
		public int ViewCount { get; }

		/// <summary>
		/// Approved for public viewing
		/// </summary>
		public bool Approved { get; }
	}
}
