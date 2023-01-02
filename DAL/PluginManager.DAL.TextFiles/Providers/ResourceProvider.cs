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
using PluginManager.DAL.TextFiles.Tables.Resources;

#pragma warning disable CA1826

namespace PluginManager.DAL.TextFiles.Providers
{
	internal sealed class ResourceProvider : IResourceProvider
	{
		private const int MaximumBookmarksPerUser = 30;
		private readonly ISimpleDBOperations<UserDataRow> _users;
		private readonly ISimpleDBOperations<ResourceCategoryDataRow> _resourceCategories;
		private readonly ISimpleDBOperations<ResourceItemDataRow> _resourceItems;
		private readonly ISimpleDBOperations<ResourceItemUserResponseDataRow> _resourceResponses;
		private readonly ISimpleDBOperations<ResourceBookmarkDataRow> _resourceBookmarks;

		public ResourceProvider(ISimpleDBOperations<UserDataRow> users,
			ISimpleDBOperations<ResourceCategoryDataRow> resources,
			ISimpleDBOperations<ResourceItemDataRow> resourceItems,
			ISimpleDBOperations<ResourceItemUserResponseDataRow> resourceResponses,
			ISimpleDBOperations<ResourceBookmarkDataRow> resourceBookmarks)
		{
			_users = users ?? throw new ArgumentNullException(nameof(users));
			_resourceCategories = resources ?? throw new ArgumentNullException(nameof(resources));
			_resourceItems = resourceItems ?? throw new ArgumentNullException(nameof(resourceItems));
			_resourceResponses = resourceResponses ?? throw new ArgumentNullException(nameof(resourceResponses));
			_resourceBookmarks = resourceBookmarks ?? throw new ArgumentNullException(nameof(resourceBookmarks));
		}

		public List<ResourceCategory> GetAllResources()
		{
			return ConvertResourceDataRowsToResourceList(_resourceCategories.Select().Where(r => r.ParentCategoryId.Equals(0) && r.IsVisible).ToList());
		}

		public List<ResourceCategory> GetAllResources(long parentId)
		{
			return ConvertResourceDataRowsToResourceList(_resourceCategories.Select().Where(r => r.ParentCategoryId.Equals(parentId) && r.IsVisible).ToList());
		}

		public ResourceCategory GetResourceCategory(long categoryId)
		{
			return ConvertResourceCategoryDataRowToResourceCategory(_resourceCategories.Select(categoryId));
		}

		public ResourceItem GetResourceItem(long id)
		{
			return ConvertResourceItemDataRowToResourceItem(_resourceItems.Select(id));
		}

		public List<ResourceCategory> RetrieveAllCategories()
		{
			return ConvertResourceDataRowsToResourceList(_resourceCategories.Select().ToList());
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

		public void IncrementViewCount(long resourceId)
		{
			ResourceItemDataRow resourceItem = _resourceItems.Select(resourceId);

			if (resourceItem == null)
				return;

			resourceItem.ViewCount++;
			_resourceItems.Update(resourceItem);
		}

		public ResourceCategory AddResourceCategory(long userId, long parent, string name, string description)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			if (string.IsNullOrEmpty(description))
				throw new ArgumentNullException(nameof(description));

			UserDataRow userDataRow = _users.Select(userId);

			if (userDataRow == null && userId > 0)
				throw new ArgumentNullException(nameof(userId));

			ResourceCategoryDataRow newCategoryRow = new ResourceCategoryDataRow()
			{ 
				ParentCategoryId = parent,
				Name = name,
				Description = description,
				UserId = userDataRow == null ? 0 : userDataRow.Id,
			};

			_resourceCategories.Insert(newCategoryRow);

			return ConvertResourceCategoryDataRowToResourceCategory(newCategoryRow);
		}

		public ResourceCategory UpdateResourceCategory(long userId, ResourceCategory category)
		{
			if (category == null)
				throw new ArgumentNullException(nameof(category));

			ResourceCategoryDataRow categoryRow = _resourceCategories.Select(category.Id);

			if (categoryRow == null)
				throw new ArgumentOutOfRangeException(nameof(category));

			UserDataRow userDataRow = _users.Select(userId);

			if (userDataRow == null && userId != 0)
				throw new ArgumentNullException(nameof(userId));

			categoryRow.UserId = userId;
			categoryRow.Name = category.Name;
			categoryRow.RouteName = category.RouteName;
			categoryRow.BackColor = category.BackColor;
			categoryRow.ForeColor = category.ForeColor;
			categoryRow.Image = category.Image;
			categoryRow.Description = category.Description;
			categoryRow.IsVisible = category.IsVisible;
			categoryRow.ParentCategoryId = category.ParentId;

			_resourceCategories.Update(categoryRow);

			return ConvertResourceCategoryDataRowToResourceCategory(categoryRow);
		}

		public ResourceItem AddResourceItem(long categoryId, ResourceType resourceType, long userId, 
			string userName, string name, string description, string value, bool approved, List<string> tags)
		{
			if (String.IsNullOrEmpty(userName)) 
				throw new ArgumentNullException(nameof(userName));

			if (String.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			if (String.IsNullOrEmpty(description))
				throw new ArgumentNullException(nameof(description));

			if (String.IsNullOrEmpty(value))
				throw new ArgumentNullException(nameof(value));

			if (tags == null)
				throw new ArgumentNullException(nameof(tags));

			UserDataRow user = _users.Select(userId);

			ResourceItemDataRow resourceItem = new ResourceItemDataRow()
			{
				Approved = approved,
				CategoryId = categoryId,
				Description = description,
				Name = name,
				ResourceType = (int)resourceType,
				UserId = user?.Id ?? 0,
				UserName = userName,
				Value = value,
			};

			tags.ForEach(t => resourceItem.Tags.Add(t));

			_resourceItems.Insert(resourceItem);

			return ConvertResourceItemDataRowToResourceItem(resourceItem);
		}

		public List<ResourceItem> RetrieveAllResourceItems()
		{
			List<ResourceItem> resources = new List<ResourceItem>();

			foreach (ResourceItemDataRow resourceItemDataRow in _resourceItems.Select())
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
					resourceItemDataRow.ViewCount,
					resourceItemDataRow.Approved,
					resourceItemDataRow.Tags));
			}

			return resources;
		}

		public ResourceItem UpdateResourceItem(long userId, ResourceItem resourceItem)
		{
			if (resourceItem == null) throw
				new ArgumentNullException(nameof(resourceItem));

			ResourceItemDataRow resourceItemRow = _resourceItems.Select(resourceItem.Id);

			if (resourceItemRow == null)
				throw new ArgumentOutOfRangeException(nameof(resourceItem));

			UserDataRow userDataRow = _users.Select(userId);

			if (userDataRow == null && userId != 0)
				throw new ArgumentNullException(nameof(userId));

			ResourceCategoryDataRow resourceCategoryDataRow = _resourceCategories.Select(resourceItem.CategoryId);

			if (resourceCategoryDataRow == null)
				throw new ArgumentException(nameof(resourceCategoryDataRow));

			resourceItemRow.CategoryId = resourceItem.CategoryId;
			resourceItemRow.Name = resourceItem.Name;
			resourceItemRow.Description = resourceItem.Description;
			resourceItemRow.Approved = resourceItem.Approved;
			resourceItemRow.ResourceType = (int)resourceItem.ResourceType;
			resourceItemRow.Value = resourceItem.Value;
			resourceItemRow.UserName = resourceItem.UserName;

			_resourceItems.Update(resourceItemRow);

			return ConvertResourceItemDataRowToResourceItem(resourceItemRow);
		}

		public BookmarkActionResult ToggleResourceBookmark(long userId, ResourceItem resourceItem)
		{
			if (resourceItem == null)
				throw new ArgumentNullException(nameof(resourceItem));
			
			if (_users.Select(userId) == null)
				return BookmarkActionResult.Unknown;

			if (_resourceItems.Select(resourceItem.Id) == null)
				return BookmarkActionResult.Unknown;

			ResourceBookmarkDataRow resourceBookmarkDataRow = _resourceBookmarks.Select(rb => rb.UserId.Equals(userId) && rb.ResourceId.Equals(resourceItem.Id)).FirstOrDefault();

			if (resourceBookmarkDataRow == null)
			{
				if (_resourceBookmarks.Select(r => r.UserId.Equals(userId)).Count >= MaximumBookmarksPerUser)
					return BookmarkActionResult.QuotaExceeded;

				_resourceBookmarks.Insert(new ResourceBookmarkDataRow()
				{ 
					UserId = userId,
					ResourceId = resourceItem.Id,
				});

				return BookmarkActionResult.Added;
			}

			_resourceBookmarks.Delete(resourceBookmarkDataRow);
			return BookmarkActionResult.Removed;
		}

		public List<ResourceItem> RetrieveUserBookmarks(long userId)
		{
			if (_users.Select(userId) != null)
			{
				IReadOnlyList<ResourceBookmarkDataRow> userBookmarks = _resourceBookmarks.Select(rb => rb.UserId.Equals(userId));
				IReadOnlyList<ResourceItemDataRow> resourceBookmarks = _resourceItems.Select(ri => userBookmarks.Any(ub => ub.Id.Equals(ri.Id)));
				
				return ConvertResourceItemDataRowsToResourceItemList(resourceBookmarks);
			}

			return new List<ResourceItem>();
		}

		#region Private Methods

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static List<ResourceItem> ConvertResourceItemDataRowsToResourceItemList(IReadOnlyList<ResourceItemDataRow> resourceItemList)
		{
			if (resourceItemList == null)
				return null;

			List<ResourceItem> result = new List<ResourceItem>();

			foreach (ResourceItemDataRow resourceItemDataRow in resourceItemList)
			{
				result.Add(new ResourceItem(
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
					resourceItemDataRow.ViewCount,
					resourceItemDataRow.Approved,
					resourceItemDataRow.Tags));
			}

			return result;
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
					resourceItemDataRow.ViewCount,
					resourceItemDataRow.Approved,
					resourceItemDataRow.Tags);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ResourceCategory ConvertResourceCategoryDataRowToResourceCategory(ResourceCategoryDataRow resourceRow)
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
					resourceItemDataRow.ViewCount,
					resourceItemDataRow.Approved,
					resourceItemDataRow.Tags));
			}

			return new ResourceCategory(resourceRow.Id, resourceRow.ParentCategoryId, resourceRow.Name, resourceRow.Description, resourceRow.ForeColor,
				resourceRow.BackColor, resourceRow.Image, resourceRow.RouteName, resourceRow.IsVisible, resources);
		}

		private static List<ResourceCategory> ConvertResourceDataRowsToResourceList(IReadOnlyList<ResourceCategoryDataRow> resources)
		{
			List<ResourceCategory> result = new List<ResourceCategory>();

			foreach (ResourceCategoryDataRow row in resources)
			{
				result.Add(new ResourceCategory(row.Id, 0, row.Name, row.Description, row.ForeColor, row.BackColor, row.Image, row.RouteName, row.IsVisible));
			}

			return result;
		}

		#endregion Private Methods
	}
}

#pragma warning restore CA1826 
