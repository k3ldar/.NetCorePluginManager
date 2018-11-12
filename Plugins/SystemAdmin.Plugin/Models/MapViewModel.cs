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
 *  File: MapViewModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  02/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SharedPluginFeatures;

namespace SystemAdmin.Plugin.Models
{
    public class MapViewModel : BaseCoreClass
    {
        #region Constructors

        public MapViewModel(SystemAdminSubMenu subMenu)
        {
            if (subMenu == null)
                throw new ArgumentNullException(nameof(subMenu));

            Title = subMenu.Name();

            MapLocationData = subMenu.Data();

            SystemAdminSettings settings = GetSettings<SystemAdminSettings>("SystemAdmin");

            GoogleMapApiKey = settings.GoogleMapApiKey;

            BreadCrumb = $"<ul><li><a href=\"/SystemAdmin/\">System Admin</a></li><li><a href=\"/SystemAdmin/Index/" +
                $"{subMenu.ParentMenu.UniqueId}\">{subMenu.ParentMenu.Name()}</a></li><li>{Title}</li></ul>";
        }

        #endregion Constructors

        #region Public Properties

        public string GoogleMapApiKey { get; set; }

        public string MapLocationData { get; set; }

        public string Title { get; set; }

        public string BreadCrumb { get; set; }

        #endregion Public Properties
    }
}
