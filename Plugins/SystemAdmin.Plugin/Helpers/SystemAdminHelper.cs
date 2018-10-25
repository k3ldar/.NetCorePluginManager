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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
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
        private const string SystemAdminSubMenuCache = "System Admin Sub Menu Cache - {0}";

        #endregion Constants

        #region Private Methods

        private readonly IMemoryCache _memoryCache;
        private readonly IPluginClassesService _pluginClassesService;

        #endregion Private Methods

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
                cache = new CacheItem(SystemAdminMainMenuCache, _pluginClassesService.GetPluginClasses<SystemAdminMainMenu>());
                _memoryCache.GetCache().Add(SystemAdminMainMenuCache, cache);
            }

            return ((List<SystemAdminMainMenu>)cache.Value);
        }

        public List<SystemAdminSubMenu> GetSystemAdminSubMenuItems(in SystemAdminMainMenu mainMenu)
        {
            if (mainMenu == null)
                throw new ArgumentNullException(nameof(mainMenu));

            string cacheName = String.Format(SystemAdminSubMenuCache, mainMenu.Name());

            CacheItem cache = _memoryCache.GetCache().Get(cacheName);

            if (cache == null)
            {
                List<SystemAdminSubMenu> subMenuItems = _pluginClassesService.GetPluginClasses<SystemAdminSubMenu>();

                for (int i = subMenuItems.Count -1; i >= 0; i--)
                {
                    if (!subMenuItems[i].ParentMenuName().Equals(mainMenu.Name()))
                        subMenuItems.RemoveAt(i);
                }

                cache = new CacheItem(cacheName, subMenuItems);
                _memoryCache.GetCache().Add(cacheName, cache);
            }

            return ((List<SystemAdminSubMenu>)cache.Value);
        }

        #endregion ISystemAdminHelperService Methods
    }
}
