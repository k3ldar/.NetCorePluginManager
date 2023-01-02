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

#pragma warning disable CS1591

namespace SystemAdmin.Plugin.Models
{
    public class AvailableIconViewModel : BaseModel
    {
        #region Constructors

        public AvailableIconViewModel(in BaseModelData modelData)
            : base(modelData)
        {

        }

        public AvailableIconViewModel(in BaseModelData modelData,
            in List<SystemAdminMainMenu> homeMenuItems)
            : this(modelData)
        {
            HomeIcons = homeMenuItems ?? throw new ArgumentNullException(nameof(homeMenuItems));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Valid in this context")]
        public AvailableIconViewModel(in BaseModelData modelData,
            in SystemAdminMainMenu mainMenu)
            : this(modelData)
        {
            if (mainMenu == null)
                throw new ArgumentNullException(nameof(mainMenu));

            Title = mainMenu.Name;
            MenuItems = mainMenu.ChildMenuItems;
        }

        #endregion Constructors

        #region Public Methods

        public string ProcessImage(in string imageName)
        {
            switch (imageName)
            {
                case Constants.SystemImageBadEgg:
                    return "/images/SystemAdmin/badegg.png";

                case Constants.SystemImageStopWatch:
                    return "/images/SystemAdmin/stopwatch.png";

                case Constants.SystemImageChart:
                    return "/images/SystemAdmin/chart.png";

                case Constants.SystemImageUptime:
                    return "/images/SystemAdmin/uptime.png";
            }

            if (String.IsNullOrEmpty(imageName))
                return "/images/SystemAdmin/setting-icon.png";

            return imageName;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0066:Convert switch statement to expression", Justification = "Like it how it is thanks")]
        public string GetMenuLink(in SystemAdminSubMenu menu)
        {
            if (menu == null)
                throw new ArgumentNullException(nameof(menu));

            switch (menu.MenuType())
            {
                case Enums.SystemAdminMenuType.Grid:
                    return $"/SystemAdmin/Grid/{menu.UniqueId}";

                case Enums.SystemAdminMenuType.Text:
                    return $"/SystemAdmin/Text/{menu.UniqueId}";

                case Enums.SystemAdminMenuType.PartialView:
                    return $"/SystemAdmin/View/{menu.UniqueId}";

                case Enums.SystemAdminMenuType.Map:
                    return $"/SystemAdmin/Map/{menu.UniqueId}";

                case Enums.SystemAdminMenuType.FormattedText:
                    return $"/SystemAdmin/TextEx/{menu.UniqueId}";

                case Enums.SystemAdminMenuType.View:
                    return $"/{menu.Controller()}/{menu.Action()}/";

                case Enums.SystemAdminMenuType.Chart:
                    return $"/SystemAdmin/Chart/{menu.UniqueId}";

				case Enums.SystemAdminMenuType.Settings:
					return $"/SystemAdmin/Settings/{menu.UniqueId}";
            }

            throw new InvalidOperationException();
        }

        #endregion Public Methods

        #region Properties

        public string Title { get; set; }

        public List<SystemAdminSubMenu> MenuItems { get; set; }

        public List<SystemAdminMainMenu> HomeIcons { get; set; }

        #endregion Properties
    }
}

#pragma warning restore CS1591