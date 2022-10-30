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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockSystemAdminHelperService.cs
 *
 *  Purpose:  Mocks ISystemAdminHelperService for unit testing
 *
 *  Date        Name                Reason
 *  07/10/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class MockSystemAdminHelperService : ISystemAdminHelperService
    {
        private readonly List<SystemAdminMainMenu> _systemAdminMenus;

        public MockSystemAdminHelperService()
        {
            _systemAdminMenus = new List<SystemAdminMainMenu>();
        }

        public MockSystemAdminHelperService(List<SystemAdminMainMenu> systemAdminMenus)
        {
            _systemAdminMenus = systemAdminMenus ?? throw new ArgumentNullException(nameof(systemAdminMenus));
        }

        public SystemAdminSubMenu GetSubMenuItem(in int id)
        {
            foreach (SystemAdminMainMenu mainMenu in _systemAdminMenus)
            {
                foreach (SystemAdminSubMenu subMenu in mainMenu.ChildMenuItems)
                {
                    if (subMenu.UniqueId.Equals(id))
                        return subMenu;
                }
            }

            return null;
        }

        public List<SystemAdminSubMenu> GetSubMenuItems()
        {
            throw new NotImplementedException();
        }

        public List<SystemAdminSubMenu> GetSubMenuItems(in string mainMenuName)
        {
            throw new NotImplementedException();
        }

        public SystemAdminMainMenu GetSystemAdminDefaultMainMenu()
        {
            throw new NotImplementedException();
        }

        public List<SystemAdminMainMenu> GetSystemAdminMainMenu()
        {
            return _systemAdminMenus;
        }

        public SystemAdminMainMenu GetSystemAdminMainMenu(in int id)
        {
            int menuId = id;
            return _systemAdminMenus.Where(sam => sam.UniqueId.Equals(menuId)).FirstOrDefault();
        }
    }
}
