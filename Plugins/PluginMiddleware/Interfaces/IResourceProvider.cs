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
 *  Copyright (c) 2012 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: IResourceProvider.cs
 *
 *  Purpose:  Resource provider
 *
 *  Date        Name                Reason
 *  16/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Middleware.Resources;

namespace Middleware
{
	/// <summary>
	/// Resource provider interface
	/// </summary>
	public interface IResourceProvider
	{
		/// <summary>
		/// Retrieves a list of all resources
		/// </summary>
		/// <returns>List&lt;Resource&gt;</returns>
		List<ResourceCategory> GetAllResources();

		/// <summary>
		/// Retrieves a list of all resources with a specific parent id
		/// </summary>
		/// <param name="parentId"></param>
		/// <returns>List&lt;Resource&gt;</returns>
		List<ResourceCategory> GetAllResources(long parentId);

		/// <summary>
		/// Retrieves a resource category with a specific id
		/// </summary>
		/// <param name="categoryId">The id of the category</param>
		/// <returns>Resource</returns>
		ResourceCategory GetResourceCategory(long categoryId);

		/// <summary>
		/// Retrieves a resource item based on it's id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		ResourceItem GetResourceItem(long id);

		/// <summary>
		/// Increments the like count for a resource
		/// </summary>
		/// <param name="id">id of resource item</param>
		/// <param name="userId">User liking post</param>
		/// <param name="like">Indicates whether the user liked/disliked the item</param>
		ResourceItem IncrementResourceItemResponse(long id, long userId, bool like);

		/// <summary>
		/// Increments the view count for a resource item
		/// </summary>
		/// <param name="resourceId"></param>
		void IncrementViewCount(long resourceId);

		/// <summary>
		/// Creates a new resource category
		/// </summary>
		/// <param name="userId">User creating the category</param>
		/// <param name="parent">Parent category id</param>
		/// <param name="name">Name of the new category</param>
		/// <param name="description">Description of the category</param>
		ResourceCategory AddResourceCategory(long userId, long? parent, string name, string description);

		/// <summary>
		/// Updates an existing resource category
		/// </summary>
		/// <param name="userId">Id of user</param>
		/// <param name="category">Category to update</param>
		/// <returns>ResourceCategory</returns>
		ResourceCategory UpdateResourceCategory(long userId, ResourceCategory category);
	}
}
