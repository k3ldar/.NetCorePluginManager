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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: IResourceProvider.cs
 *
 *  Purpose:  IResourceProvider for text based storage
 *
 *  Date        Name                Reason
 *  27/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SharedPluginFeatures;

using PluginManager.DAL.TextFiles.Tables;
using SimpleDB;

using Middleware;
using Middleware.Resources;
using System.Runtime.CompilerServices;

#pragma warning disable CA1826

namespace PluginManager.DAL.TextFiles.Providers
{
	internal sealed class ResourceProvider : IResourceProvider
	{
		private readonly ISimpleDBOperations<UserDataRow> _users;
		private readonly ISimpleDBOperations<ResourceCategoryDataRow> _resourceCategories;
		private readonly ISimpleDBOperations<ResourceItemDataRow> _resourceItems;
		private readonly ISimpleDBOperations<ResourceItemUserResponseDataRow> _resourceResponses;

		public ResourceProvider(ISimpleDBOperations<UserDataRow> users,
			ISimpleDBOperations<ResourceCategoryDataRow> resources,
			ISimpleDBOperations<ResourceItemDataRow> resourceItems,
			ISimpleDBOperations<ResourceItemUserResponseDataRow> resourceResponses)
		{
			_users = users ?? throw new ArgumentNullException(nameof(users));
			_resourceCategories = resources ?? throw new ArgumentNullException(nameof(resources));
			_resourceItems = resourceItems ?? throw new ArgumentNullException(nameof(resourceItems));
			_resourceResponses = resourceResponses ?? throw new ArgumentNullException(nameof(resourceResponses));
		}

		public List<ResourceCategory> GetAllResources()
		{
			return ConvertResourceDataRowsToResourceList(_resourceCategories.Select());
		}

		public ResourceCategory GetResourceFromRouteName(string routeName)
		{
			return ConvertResourceCategoryDataRowToResourceItem(_resourceCategories.Select().Where(r => r.RouteName.Equals(routeName)).FirstOrDefault());
		}

		public ResourceItem GetResourceItemFromId(long id)
		{
			return ConvertResourceItemDataRowToResourceItem(_resourceItems.Select(id));
		}

		public ResourceItem IncrementResourceItemResponse(long id, long userId, bool like)
		{
			ResourceItemDataRow resourceItemDataRow = _resourceItems.Select(id);

			if (resourceItemDataRow == null)
				return null;

			UserDataRow user = _users.Select(userId);

			if (user == null)
				return null;

			ResourceItemUserResponseDataRow userResponse = _resourceResponses
				.Select(ur => 
					ur.UserId.Equals(user.Id) && 
					ur.ResourceItemId.Equals(resourceItemDataRow.Id)
				).FirstOrDefault();

			if (userResponse == null)
			{
				userResponse = new ResourceItemUserResponseDataRow() 
				{ 
					UserId = user.Id,
					ResourceItemId = resourceItemDataRow.Id,
					Like = like 
				};

				if (like)
					resourceItemDataRow.Likes++;
				else
					resourceItemDataRow.Dislikes++;
			}
			else
			{ 
				if (userResponse.Like != like)
				{
					if (like)
					{
						resourceItemDataRow.Likes++;
						resourceItemDataRow.Dislikes--;
					}
					else
					{
						resourceItemDataRow.Likes--;
						resourceItemDataRow.Dislikes++;
					}
				}

				userResponse.Like = like;
			}

			_resourceItems.Update(resourceItemDataRow);
			_resourceResponses.InsertOrUpdate(userResponse);

			return ConvertResourceItemDataRowToResourceItem(resourceItemDataRow);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ResourceItem ConvertResourceItemDataRowToResourceItem(ResourceItemDataRow resourceItemDataRow)
		{
			if (resourceItemDataRow == null)
				return null;

			return new ResourceItem(
					resourceItemDataRow.Id,
					resourceItemDataRow.CategoryId,
					(ResourceType)resourceItemDataRow.ResourceType,
					resourceItemDataRow.UserId,
					resourceItemDataRow.UserName,
					resourceItemDataRow.Name,
					resourceItemDataRow.Description,
					resourceItemDataRow.Value,
					resourceItemDataRow.Likes,
					resourceItemDataRow.Dislikes,
					resourceItemDataRow.Approved);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ResourceCategory ConvertResourceCategoryDataRowToResourceItem(ResourceCategoryDataRow resourceRow)
		{
			if (resourceRow == null)
				return null;

			List<ResourceItem> resources = new List<ResourceItem>();

			foreach (ResourceItemDataRow resourceItemDataRow in _resourceItems.Select(ri => ri.CategoryId.Equals(resourceRow.Id)))
			{
				resources.Add(new ResourceItem(
					resourceItemDataRow.Id,
					resourceItemDataRow.CategoryId,
					(ResourceType)resourceItemDataRow.ResourceType,
					resourceItemDataRow.UserId,
					resourceItemDataRow.UserName,
					resourceItemDataRow.Name,
					resourceItemDataRow.Description,
					resourceItemDataRow.Value,
					resourceItemDataRow.Likes,
					resourceItemDataRow.Dislikes,
					resourceItemDataRow.Approved));
			}

			return new ResourceCategory(resourceRow.Id, resourceRow.Name, resourceRow.Description, resourceRow.ForeColor,
				resourceRow.BackColor, resourceRow.Image, resourceRow.RouteName, resources);
		}

		private static List<ResourceCategory> ConvertResourceDataRowsToResourceList(IReadOnlyList<ResourceCategoryDataRow> resources)
		{
			List<ResourceCategory> result = new List<ResourceCategory>();

			foreach (ResourceCategoryDataRow row in resources)
			{
				result.Add(new ResourceCategory(row.Id, row.Name, row.Description, row.ForeColor, row.BackColor, row.Image, row.RouteName));
			}

			return result;
		}
	}
}

#pragma warning restore CA1826 
