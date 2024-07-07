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
 *  Product:  AspNetCore.PluginManager.DemoWebsite
 *  
 *  File: SharedPluginHelper.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

using SystemAdmin.Plugin.Classes.MenuItems;

#pragma warning disable CS1591

namespace SystemAdmin.Plugin
{
	/// <summary>
	/// This class acts as a wrapper around the elements you extend through plugin manager
	/// </summary>
	public sealed class SystemAdminHelper : ISystemAdminHelperService
	{
		#region Constants

		private const string SystemAdminMainMenuCache = "System Admin Main Menu Cache";
		private const string SystemAdminMainMenu = "System Admin Main Menu {0}";
		private const string SystemAdminSubMenu = "System Admin Sub Menu {0}";

		#endregion Constants

		#region Private Members

		private readonly IMemoryCache _memoryCache;
		private readonly IPluginClassesService _pluginClassesService;

		#endregion Private Members

		#region Constructors

		public SystemAdminHelper(IMemoryCache memoryCache, IPluginClassesService pluginClassesService)
		{
			_memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
			_pluginClassesService = pluginClassesService ?? throw new ArgumentNullException(nameof(pluginClassesService));
		}

		#endregion Constructors

		#region ISystemAdminHelperService Methods

		public List<SystemAdminMainMenu> GetSystemAdminMainMenu()
		{
			CacheItem cache = _memoryCache.GetCache().Get(SystemAdminMainMenuCache);

			if (cache == null)
			{
				int uniqueId = 0;
				List<SystemAdminMainMenu> menuItems = _pluginClassesService.GetPluginClasses<SystemAdminMainMenu>();

				uniqueId = BuildDefaultSystemAdminMenuItems(uniqueId, menuItems);

				_ = BuildSystemSettingsMenuItems(uniqueId, menuItems);

				menuItems.Sort();

				cache = new CacheItem(SystemAdminMainMenuCache, menuItems);
				_memoryCache.GetCache().Add(SystemAdminMainMenuCache, cache);
			}

			return (List<SystemAdminMainMenu>)cache.Value;
		}

		public SystemAdminMainMenu GetSystemAdminMainMenu(in int id)
		{
			// get from memory if available
			CacheItem cacheItem = _memoryCache.GetCache().Get(String.Format(SystemAdminMainMenu, id));

			if (cacheItem != null)
				return (SystemAdminMainMenu)cacheItem.Value;

			// not in memory try looping all items
			foreach (SystemAdminMainMenu menu in GetSystemAdminMainMenu())
			{
				if (menu.UniqueId == id)
					return menu;
			}

			return null;
		}

		public SystemAdminMainMenu GetSystemAdminDefaultMainMenu()
		{
			return GetSystemAdminMainMenu()[0];
		}

		private int BuildDefaultSystemAdminMenuItems(int uniqueId, List<SystemAdminMainMenu> menuItems)
		{
			//should loop through all child items, add to a parent (create if not found) and then finally sort all parent and parent menu items

			menuItems.ForEach(mi => mi.UniqueId = ++uniqueId);
			List<SystemAdminSubMenu> allSubMenuItems = _pluginClassesService.GetPluginClasses<SystemAdminSubMenu>();

			// get sub menu items
			foreach (SystemAdminSubMenu menu in allSubMenuItems.Where(sm => sm.Enabled()).ToList())
			{
				// get parent menu
				SystemAdminMainMenu parent = menuItems.Find(p => p.Name.Equals(menu.ParentMenuName()));

				if (parent == null)
				{
					parent = new SystemAdminMainMenu(menu.ParentMenuName(), ++uniqueId);
					menuItems.Add(parent);
					_memoryCache.GetCache().Add(String.Format(SystemAdminMainMenu, parent.UniqueId),
						new CacheItem(String.Format(SystemAdminMainMenu, parent.UniqueId), parent));
				}

				menu.UniqueId = ++uniqueId;

				menu.ParentMenu = parent;
				parent.ChildMenuItems.Add(menu);

				_memoryCache.GetCache().Add(String.Format(SystemAdminSubMenu, menu.UniqueId),
					new CacheItem(String.Format(SystemAdminSubMenu, menu.UniqueId), menu));

				parent.ChildMenuItems.Sort();
			}

			return uniqueId;
		}

		private int BuildSystemSettingsMenuItems(int uniqueId, List<SystemAdminMainMenu> menuItems)
		{
			List<IPluginSettings> settings = _pluginClassesService.GetPluginClasses<IPluginSettings>();

			if (settings.Count > 0)
			{
				SystemAdminMainMenu settingsParent = new(Languages.LanguageStrings.Settings, ++uniqueId);
				menuItems.Add(settingsParent);

				_memoryCache.GetCache().Add(String.Format(SystemAdminMainMenu, settingsParent.UniqueId),
					new CacheItem(String.Format(SystemAdminMainMenu, settingsParent.UniqueId), settingsParent));

				foreach (IPluginSettings pluginSettings in settings)
				{
					SettingsMenuItem settingsMenuItem = new(pluginSettings)
					{
						UniqueId = ++uniqueId
					};
					settingsParent.ChildMenuItems.Add(settingsMenuItem);

					_memoryCache.GetCache().Add(String.Format(SystemAdminSubMenu, settingsMenuItem.UniqueId),
						new CacheItem(String.Format(SystemAdminSubMenu, settingsMenuItem.UniqueId), settingsMenuItem));
				}

				settingsParent.ChildMenuItems.Sort();
			}

			return uniqueId;
		}

		public List<SystemAdminSubMenu> GetSubMenuItems()
		{
			List<SystemAdminMainMenu> allMenuItems = GetSystemAdminMainMenu();

			if (allMenuItems.Count > 0)
				return allMenuItems[0].ChildMenuItems;

			return [];
		}

		public List<SystemAdminSubMenu> GetSubMenuItems(in string mainMenuName)
		{
			List<SystemAdminMainMenu> allMenuItems = GetSystemAdminMainMenu();

			foreach (SystemAdminMainMenu menuItem in allMenuItems)
			{
				if (menuItem.Name.Equals(mainMenuName))
					return menuItem.ChildMenuItems;
			}

			return [];
		}

		public SystemAdminSubMenu GetSubMenuItem(in int id)
		{
			// get from memory if available
			CacheItem cacheItem = _memoryCache.GetCache().Get(String.Format(SystemAdminSubMenu, id));

			if (cacheItem != null)
				return (SystemAdminSubMenu)cacheItem.Value;

			return null;
		}

		#endregion ISystemAdminHelperService Methods
	}
}

#pragma warning restore CS1591