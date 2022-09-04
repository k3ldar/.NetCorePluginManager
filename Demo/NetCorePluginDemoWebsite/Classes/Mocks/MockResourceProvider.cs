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
					new ResourceCategory(1, "Resource 1", "Resource desc 1", "black", "rgba(0,0,0,.03)", "/Images/Download/download.jpg", "resource-1"),
					new ResourceCategory(2, "Resource 2", "Resource desc 2", "white", "red", null, "resource-2"),
					new ResourceCategory(3, "Resource 3", "Resource desc 2", "white", "black", null, "resource-3"),
					new ResourceCategory(4, "Resource 4", "Resource desc 2", "black", "grey", null, "resource-4"),
					new ResourceCategory(5, "Resource 5", "Resource desc 2", "white", "blue", null, "resource-5"),
				};

				foreach (ResourceCategory category in _resources)
				{
					for (int i = 0; i < 10; i++)
					{
						ResourceItem item = new ResourceItem()
						{
							Id = (category.Id * 100) + i,
							CategoryId = i,
							Approved = true,
							Description = $"Description of {category.Name} {i}",
							Name = $"Child {i} for {category.Name}",
							ResourceType = ResourceType.Text,
							Value = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit..."
						};

						category.ResourceItems.Add(item);
						_items.Add(item);
					}
				}
			}

			return _resources;
		}

		public ResourceCategory GetResourceFromRouteName(string routeName)
		{
			return GetAllResources().Where(r => r.RouteName.Equals(routeName)).FirstOrDefault();
		}

		public ResourceItem GetResourceItemFromId(long id)
		{
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
	}
}
