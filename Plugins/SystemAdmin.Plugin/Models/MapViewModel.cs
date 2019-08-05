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

using SharedPluginFeatures;

namespace SystemAdmin.Plugin.Models
{
    public sealed class MapViewModel : BaseModel
    {
        #region Private Members

        private readonly ISettingsProvider _settingsProvider;

        #endregion Private Members

        #region Constructors

        public MapViewModel(in BaseModelData modelData,
            in ISettingsProvider settingsProvider, in SystemAdminSubMenu subMenu)
            : base(modelData)
        {
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));

            if (subMenu == null)
                throw new ArgumentNullException(nameof(subMenu));

            Title = subMenu.Name();

            MapLocationData = subMenu.Data();
            SystemAdminSettings settings = settingsProvider.GetSettings<SystemAdminSettings>("SystemAdmin");

            GoogleMapApiKey = settings.GoogleMapApiKey;
        }

        #endregion Constructors

        #region Public Properties

        public string GoogleMapApiKey { get; set; }

        public string MapLocationData { get; set; }

        public string Title { get; set; }

        #endregion Public Properties
    }
}
