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
 *  Product:  Demo Website
 *  
 *  File: MockResourceProvider.cs
 *
 *  Purpose:  Mock IResourceProvider for tesing purpose
 *
 *  Date        Name                Reason
 *  29/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;

using Middleware;
using Middleware.Resources;

namespace AspNetCore.PluginManager.DemoWebsite.Classes.Mocks
{
	public class MockResourceProvider : IResourceProvider
	{
		private List<ResourceCategory> _resources;
		private readonly List<ResourceItem> _items = new List<ResourceItem>();

		public List<ResourceCategory> GetAllResources()
		{
			if (_resources == null)
			{
				_resources = new List<ResourceCategory>()
				{
					new ResourceCategory(1, null, "Resource 1", "Resource desc 1", "black", "rgba(0,0,0,.03)", "/Images/Download/download.jpg", "resource-1"),
					new ResourceCategory(2, null, "Resource 2", "Resource desc 2", "white", "red", null, "resource-2"),
					new ResourceCategory(21, 2, "Resource 2 Child 1", "Resource desc 2 (1)", "black", "white", null, "resource-2-child-1"),
					new ResourceCategory(22, 2, "Resource 2 Child 2", "Resource desc 2 (2)", "black", "white", null, "resource-2-child-2"),
					new ResourceCategory(23, 2, "Resource 2 Child 3", "Resource desc 2 (3)", "black", "white", null, "resource-2-child-3"),
					new ResourceCategory(3, null, "Resource 3", "Resource desc 2", "white", "black", null, "resource-3"),
					new ResourceCategory(4, null, "Resource 4", "Resource desc 2", "black", "grey", null, "resource-4"),
					new ResourceCategory(5, null, "Resource 5", "Resource desc 2", "white", "blue", null, "resource-5"),
				};

				foreach (ResourceCategory category in _resources)
				{
					for (int i = 0; i < 10; i++)
					{
						ResourceItem item = new ResourceItem()
						{
							Id = (category.Id * 100) + i,
							CategoryId = category.Id,
							Approved = true,
							Description = $"Description of {category.Name} {i}",
							Name = $"Child {i} for {category.Name}",
							ResourceType = ResourceType.Text,
							Value = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit..."
						};

						if (item.Id == 101)
						{
							item.ResourceType = ResourceType.YouTube;
							item.Value = "OP1tBC6dBW0";
						}
						else if (item.Id == 202)
						{
							item.ResourceType = ResourceType.TikTok;
							item.Value = $"visualstudio;7026423558041537839";
						}
						else if (item.Id == 303)
						{
							item.ResourceType = ResourceType.Uri;
							item.Value = "https://www.pluginmanager.website/";
						}
						else if (item.Id == 404)
						{
							item.ResourceType = ResourceType.Image;
							item.Value = "https://www.pluginmanager.website/images/PluginTechnology.png";
						}

						category.ResourceItems.Add(item);
						_items.Add(item);
					}
				}
			}

			return _resources.Where(r => r.ParentId == null).ToList();
		}

		public List<ResourceCategory> GetAllResources(long parentId)
		{
			if (_resources == null)
				GetAllResources();

			return _resources.Where(r => r.ParentId.HasValue && r.ParentId.Value.Equals(parentId)).ToList();
		}

		public ResourceCategory GetResourceCategory(long categoryId)
		{
			if (_resources == null)
				GetAllResources();

			return _resources.Where(r => r.Id.Equals(categoryId)).FirstOrDefault();
		}

		public ResourceItem GetResourceItem(long id)
		{
			if (_resources == null)
				GetAllResources();

			return _items.Where(i => i.Id.Equals(id)).FirstOrDefault();
		}

		public ResourceItem IncrementResourceItemResponse(long id, long userId, bool like)
		{
			ResourceItem item = _items.FirstOrDefault(i => i.Id.Equals(id));

			if (item == null)
				return null;

			if (like)
				item.Likes++;
			else
				item.Dislikes++;

			return item;
		}

		public void IncrementViewCount(long resourceId)
		{
			ResourceItem item = _items.FirstOrDefault(i => i.Id.Equals(resourceId));

			item.ViewCount++;
		}

		public ResourceCategory AddResourceCategory(long userId, long? parent, string name, string description)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			return new ResourceCategory(1, parent, name, description, null, null, null, name);
		}

		public ResourceCategory UpdateResourceCategory(long userId, ResourceCategory category)
		{
			if (category == null)
				throw new ArgumentNullException(nameof(category));

			return category;
		}
	}
}
