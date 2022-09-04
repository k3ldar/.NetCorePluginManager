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
		/// Retrieves a resource using route name
		/// </summary>
		/// <param name="routeName">Name of route to find</param>
		/// <returns>Resource</returns>
		ResourceCategory GetResourceFromRouteName(string routeName);

		/// <summary>
		/// Retrieves a resource item based on it's id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		ResourceItem GetResourceItemFromId(long id);

		/// <summary>
		/// Increments the like count for a resource
		/// </summary>
		/// <param name="id">id of resource item</param>
		/// <param name="userId">User liking post</param>
		/// <param name="like">Indicates whether the user liked/disliked the item</param>
		ResourceItem IncrementResourceItemResponse(long id, long userId, bool like);
	}
}
