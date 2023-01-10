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
 *  Copyright (c) 2012 - 2022 Simon Carter.  All Rights Reserved.
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

using Microsoft.Extensions.Primitives;

using Middleware.Resources;

using SharedPluginFeatures;

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
		/// <param name="isVisible">Indicates whether the category is visible or not</param>
		ResourceCategory AddResourceCategory(long userId, long parent, string name, string description, bool isVisible);

		/// <summary>
		/// Updates an existing resource category
		/// </summary>
		/// <param name="userId">Id of user</param>
		/// <param name="category">Category to update</param>
		/// <returns>ResourceCategory</returns>
		ResourceCategory UpdateResourceCategory(long userId, ResourceCategory category);

		/// <summary>
		/// Retrieves all resource categorys for administration purposes
		/// </summary>
		/// <returns></returns>
		List<ResourceCategory> RetrieveAllCategories();

		/// <summary>
		/// Adds a new resource item
		/// </summary>
		/// <param name="categoryId"></param>
		/// <param name="resourceType"></param>
		/// <param name="userId"></param>
		/// <param name="userName"></param>
		/// <param name="name"></param>
		/// <param name="description"></param>
		/// <param name="value"></param>
		/// <param name="approved"></param>
		/// <param name="tags"></param>
		/// <returns></returns>
		ResourceItem AddResourceItem(long categoryId, ResourceType resourceType, long userId, string userName, 
			string name, string description, string value, bool approved, List<string> tags);

		/// <summary>
		/// Retrieves all resource items for administrative purposes
		/// </summary>
		/// <returns></returns>
		List<ResourceItem> RetrieveAllResourceItems();

		/// <summary>
		/// Updates a resource item
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="resourceItem"></param>
		/// <returns></returns>
		ResourceItem UpdateResourceItem(long userId, ResourceItem resourceItem);

		/// <summary>
		/// Updates a user bookmark for a resource item, if not present the bookmark is added, if present the bookmark is removed
		/// </summary>
		/// <param name="userId">Id of user</param>
		/// <param name="resourceItem">Resource item whose bookmark is being toggled</param>
		/// <returns>BookmarkActionResult</returns>
		BookmarkActionResult ToggleResourceBookmark(long userId, ResourceItem resourceItem);

		/// <summary>
		/// Retrieves a list of all user
		/// </summary>
		/// <param name="userId">Id of user who's bookmarked items are being retrieved</param>
		/// <returns>List&lt;ResourceItem&gt;</returns>
		List<ResourceItem> RetrieveUserBookmarks(long userId);
	}
}
