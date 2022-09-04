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
 *  Product:  Plugin Middleware
 *  
 *  File: ResourceItem.cs
 *
 *  Purpose:  Resource item class
 *
 *  Date        Name                Reason
 *  31/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Resources
{
	/// <summary>
	/// Represents an individual resource
	/// </summary>
	public sealed class ResourceItem
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		public ResourceItem()
		{

		}

		/// <summary>
		/// Constructor
		/// </summary>
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
		/// <param name="approved">Approved for public viewing</param>
		public ResourceItem(long id, long categoryId, ResourceType resourceType, long userId, string userName, string name, string description, string value, int likes, int dislikes, bool approved)
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
		/// Id of user who created the item
		/// </summary>
		public long UserId { get; set; }

		/// <summary>
		/// Name of user creating the resource
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Name of resource item
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Description of resource item
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Value of resource item
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// Number of likes
		/// </summary>
		public int Likes { get; set; }

		/// <summary>
		/// Number of dislikes
		/// </summary>
		public int Dislikes { get; set; }

		/// <summary>
		/// Approved for public viewing
		/// </summary>
		public bool Approved { get; set; }

	}
}
