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
 *  Product:  SystemAdmin.Plugin
 *  
 *  File: AvailableIconViewModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  28/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

namespace SystemAdmin.Plugin.Models
{
    public class AvailableIconViewModel
    {
        #region Constructors

        public AvailableIconViewModel()
        {
            BreadCrumb = String.Empty;
        }

        public AvailableIconViewModel(in List<SystemAdminMainMenu> homeMenuItems)
            : this ()
        {
            HomeIcons = homeMenuItems ?? throw new ArgumentNullException(nameof(homeMenuItems));
        }

        public AvailableIconViewModel(in SystemAdminMainMenu mainMenu)
        {
            if (mainMenu == null)
                throw new ArgumentNullException(nameof(mainMenu));

            Title = mainMenu.Name();
            MenuItems = mainMenu.ChildMenuItems ?? throw new ArgumentNullException(nameof(mainMenu.ChildMenuItems));

            BreadCrumb = $"<ul><li><a href=\"/SystemAdmin/\">System Admin</a></li>&nbsp;&gt;&nbsp;<li>{Title}</li></ul>";
        }

        #endregion Constructors

        #region Public Methods

        public AvailableIconViewModel ClearBreadCrumb()
        {
            BreadCrumb = String.Empty;
            return (this);
        }

        public string ProcessImage(in string imageName)
        {
            if (String.IsNullOrEmpty(imageName))
                return ("/images/setting-icon.png");

            return (imageName);
        }

        public string GetMenuLink(in SystemAdminSubMenu menu)
        {
            if (menu == null)
                throw new ArgumentNullException(nameof(menu));

            switch (menu.MenuType())
            {
                case Enums.SystemAdminMenuType.Grid:
                    return ($"/SystemAdmin/Grid/{menu.UniqueId}");
                case Enums.SystemAdminMenuType.Text:
                    return ($"/SystemAdmin/Text/{menu.UniqueId}");
                case Enums.SystemAdminMenuType.PartialView:
                    return ($"/SystemAdmin/View/{menu.UniqueId}");
            }

            throw new InvalidOperationException();
        }

        #endregion Public Methods

        #region Properties

        public string Title { get; set; }

        public List<SystemAdminSubMenu> MenuItems { get; set; }

        public List<SystemAdminMainMenu> HomeIcons { get; set; }

        public string BreadCrumb { get; private set; }

        #endregion Properties
    }
}
