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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
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

using Shared.Classes;

using SharedPluginFeatures;

namespace SystemAdmin.Plugin
{
    /// <summary>
    /// This class acts as a wrapper arund the elements you extend through plugin manager
    /// </summary>
    public sealed class SystemAdminHelper : ISystemAdminHelperService
    {
        #region Constants

        private const string SystemAdminMainMenuCache = "System Admin Main Menu Cache";
        private const string SystemAdminMainMenu = "System Admin Main Menu {0}";
        private const string SystemAdminSubMenu = "System Admin Sub Menu {0}";

        #endregion Constants

        #region Private Methods

        private readonly IMemoryCache _memoryCache;
        private readonly IPluginClassesService _pluginClassesService;

        #endregion Private Methods

        #region Constructors

        public SystemAdminHelper(IMemoryCache memoryCache, IPluginClassesService pluginClassesService, IBreadcrumbService breadcrumbService)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _pluginClassesService = pluginClassesService ?? throw new ArgumentNullException(nameof(pluginClassesService));

            RegisterBreadcrumbs(breadcrumbService);
        }

        #endregion Constructors

        #region ISystemAdminHelperService Methods

        public List<SystemAdminMainMenu> GetSystemAdminMainMenu()
        {
            CacheItem cache = _memoryCache.GetCache().Get(SystemAdminMainMenuCache);

            if (cache == null)
            {
                int uniqueId = 0;

                //should loop through all child items, add to a parent (create if not found) and then finally sort all parent and parent menu items

                List<SystemAdminMainMenu> menuItems = new List<SystemAdminMainMenu>();
                List<SystemAdminSubMenu> allSubMenuItems = _pluginClassesService.GetPluginClasses<SystemAdminSubMenu>();

                // get sub menu items
                foreach (SystemAdminSubMenu menu in allSubMenuItems)
                {
                    menu.UniqueId = ++uniqueId;

                    // get parent menu
                    SystemAdminMainMenu parent = menuItems.Where(p => p.Name.Equals(menu.ParentMenuName())).FirstOrDefault();

                    if (parent == null)
                    {
                        parent = new SystemAdminMainMenu(menu.ParentMenuName(), ++uniqueId);
                        menuItems.Add(parent);
                    }

                    menu.ParentMenu = parent;
                    parent.ChildMenuItems.Add(menu);

                    _memoryCache.GetCache().Add(String.Format(SystemAdminMainMenu, menu.UniqueId), 
                        new CacheItem(String.Format(SystemAdminMainMenu, menu.UniqueId), menu));

                    menu.UniqueId = ++uniqueId;

                    _memoryCache.GetCache().Add(String.Format(SystemAdminSubMenu, menu.UniqueId),
                        new CacheItem(String.Format(SystemAdminSubMenu, menu.UniqueId), menu));

                    parent.ChildMenuItems.Sort();
                }

                menuItems.Sort();

                cache = new CacheItem(SystemAdminMainMenuCache, menuItems);
                _memoryCache.GetCache().Add(SystemAdminMainMenuCache, cache);
            }

            return ((List<SystemAdminMainMenu>)cache.Value);
        }

        public SystemAdminMainMenu GetSystemAdminDefaultMainMenu()
        {
            return (GetSystemAdminMainMenu()[0]);
        }

        public SystemAdminMainMenu GetSystemAdminMainMenu(in int id)
        {
            // get from memory if available
            CacheItem cacheItem = _memoryCache.GetCache().Get(String.Format(SystemAdminMainMenu, id));

            if (cacheItem != null)
                return ((SystemAdminMainMenu)cacheItem.Value);

            // not in memory try looping all items
            foreach (SystemAdminMainMenu menu in GetSystemAdminMainMenu())
            {
                if (menu.UniqueId == id)
                    return (menu);
            }

            return (null);
        }

        public List<SystemAdminSubMenu> GetSubMenuItems()
        {
            List<SystemAdminMainMenu> allMenuItems = GetSystemAdminMainMenu();

            if (allMenuItems.Count > 0)
                return (allMenuItems[0].ChildMenuItems);

            return (new List<SystemAdminSubMenu>());
        }

        public List<SystemAdminSubMenu> GetSubMenuItems(in string mainMenuName)
        {
            List<SystemAdminMainMenu> allMenuItems = GetSystemAdminMainMenu();

            foreach (SystemAdminMainMenu menuItem in allMenuItems)
            {
                if (menuItem.Name.Equals(mainMenuName))
                    return (menuItem.ChildMenuItems);
            }

            return (new List<SystemAdminSubMenu>());
        }

        public SystemAdminSubMenu GetSubMenuItem(in int id)
        {
            // get from memory if available
            CacheItem cacheItem = _memoryCache.GetCache().Get(String.Format(SystemAdminSubMenu, id));

            if (cacheItem != null)
                return ((SystemAdminSubMenu)cacheItem.Value);

            return (null);
        }

        #endregion ISystemAdminHelperService Methods

        #region Private Methods

        private void RegisterBreadcrumbs(in IBreadcrumbService breadcrumbService)
        {
            string parentRoute = "/SystemAdmin";
            breadcrumbService.AddBreadcrumb(nameof(Languages.LanguageStrings.SystemAdmin), parentRoute, false);

            foreach (SystemAdminMainMenu item in GetSystemAdminMainMenu())
            {
                string route = $"/SystemAdmin/Index/{item.UniqueId}";
                breadcrumbService.AddBreadcrumb(item.Name, route, parentRoute, false);

                foreach (SystemAdminSubMenu childItem in item.ChildMenuItems)
                {
                    switch (childItem.MenuType())
                    {
                        case Enums.SystemAdminMenuType.Grid:
                            breadcrumbService.AddBreadcrumb(childItem.Name(), $"/SystemAdmin/Grid/{childItem.UniqueId}", route, false);
                            break;
                        case Enums.SystemAdminMenuType.Text:
                            breadcrumbService.AddBreadcrumb(childItem.Name(), $"/SystemAdmin/Text/{childItem.UniqueId}", route, false);
                            break;
                        case Enums.SystemAdminMenuType.PartialView:
                            breadcrumbService.AddBreadcrumb(childItem.Name(), $"/SystemAdmin/View/{childItem.UniqueId}", route, false);
                            break;
                        case Enums.SystemAdminMenuType.Map:
                            breadcrumbService.AddBreadcrumb(childItem.Name(), $"/SystemAdmin/Map/{childItem.UniqueId}", route, false);
                            break;
                        case Enums.SystemAdminMenuType.FormattedText:
                            breadcrumbService.AddBreadcrumb(childItem.Name(), $"/SystemAdmin/TextEx/{childItem.UniqueId}", route, false);
                            break;
                    }

                }
            }
        }

        #endregion Private Methods
    }
}
