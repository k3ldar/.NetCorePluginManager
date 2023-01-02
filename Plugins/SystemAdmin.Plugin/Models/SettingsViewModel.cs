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
 *  File: SettingsViewModel.cs
 *
 *  Purpose:  View model for settings
 *
 *  Date        Name                Reason
 *  19/12/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

namespace SystemAdmin.Plugin.Models
{
	/// <summary>
	/// View model for system admin settings
	/// </summary>
	public sealed class SettingsViewModel : BaseModel
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SettingsViewModel()
		{
			Settings = new();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="data">Base model data</param>
		/// <param name="settingId">Unique id for setting</param>
		/// <param name="settingsName">Name of settings</param>
		public SettingsViewModel(BaseModelData data, int settingId, string settingsName)
			: base(data)
		{
			if (String.IsNullOrEmpty(settingsName))
				throw new ArgumentNullException(nameof(settingsName));

			SettingId = settingId;
			SettingsName = settingsName;
			Settings = new();
		}

		/// <summary>
		/// Id of the individual setting
		/// </summary>
		public int SettingId { get; set; }

		/// <summary>
		/// Name of settings
		/// </summary>
		public string SettingsName { get; set; }

		/// <summary>
		/// List of individual application settings
		/// </summary>
		public List<ApplicationSettingViewModel> Settings { get; set; }
	}
}
