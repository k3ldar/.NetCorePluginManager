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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.DemoWebsite
 *  
 *  File: UserPermissionsMenu.cs
 *
 *  Purpose:  Menu to show user permissions
 *
 *  Date        Name                Reason
 *  08/08/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using PluginManager.Abstractions;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace SystemAdmin.Plugin.Classes.MenuItems
{
    public class UserPermissionsMenu : SystemAdminSubMenu
    {
        #region Private Members

        private readonly ISettingsProvider _settingsProvider;

        #endregion Private Members

        #region Constructors

        public UserPermissionsMenu(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
        }

        #endregion Constructors

        #region SystemAdminSubMenu Methods

        public override string Action()
        {
            return nameof(Controllers.SystemAdminController.Permissions);
        }

        public override string Area()
        {
            return String.Empty;
        }

        public override string Controller()
        {
            return Controllers.SystemAdminController.Name;
        }

        public override Enums.SystemAdminMenuType MenuType()
        {
            return Enums.SystemAdminMenuType.View;
        }

        public override string Data()
        {
            return String.Empty;
        }

        public override string Name()
        {
            return nameof(Languages.LanguageStrings.UserPermissions);
        }

        public override string ParentMenuName()
        {
            return nameof(Languages.LanguageStrings.Permissions);
        }

        public override int SortOrder()
        {
            return 0;
        }

        public override string Image()
        {
            return String.Empty;
        }

        #endregion SystemAdminSubMenu Methods
    }
}

#pragma warning restore CS1591