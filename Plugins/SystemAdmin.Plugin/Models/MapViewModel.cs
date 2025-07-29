/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
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

using PluginManager.Abstractions;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace SystemAdmin.Plugin.Models
{
	public sealed class MapViewModel : BaseModel
	{
		#region Constructors

		public MapViewModel(in IBaseModelData modelData,
			in ISettingsProvider settingsProvider, in SystemAdminSubMenu subMenu)
			: base(modelData)
		{
			if (settingsProvider == null)
				throw new ArgumentNullException(nameof(settingsProvider));

			if (subMenu == null)
				throw new ArgumentNullException(nameof(subMenu));

			Title = subMenu.Name();

			MapLocationData = subMenu.Data();

			if (String.IsNullOrWhiteSpace(MapLocationData))
				throw new InvalidOperationException("map data not found");

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

#pragma warning restore CS1591