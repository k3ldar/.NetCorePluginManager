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
 *  Product:  AspNetCore.PluginManager.DemoWebsite
 *  
 *  File: SettingsMenuItem.cs
 *
 *  Purpose:  Placeholder menu for system settings
 *
 *  Date        Name                Reason
 *  01/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace SystemAdmin.Plugin.Classes.MenuItems
{
	/// <summary>
	/// Settings menu item
	/// </summary>
	public sealed class SettingsMenuItem: SystemAdminSubMenu
	{
		private readonly IPluginSettings _pluginSettings;

		public SettingsMenuItem()
		{

		}

		public SettingsMenuItem(IPluginSettings pluginSettings)
		{
			_pluginSettings = pluginSettings ?? throw new ArgumentNullException(nameof(pluginSettings));
		}

		public override string Action()
		{
			return null;
		}

		public override string Area()
		{
			return null;
		}

		public override string Controller()
		{
			return nameof(Controllers.SystemAdminController.Name);
		}

		public override string Data()
		{
			return string.Empty;
		}

		public override string Image()
		{
			return null;
		}

		public override Enums.SystemAdminMenuType MenuType()
		{
			return Enums.SystemAdminMenuType.Settings;
		}

		public override string Name()
		{
			return _pluginSettings.SettingsName;
		}

		public override string ParentMenuName()
		{
			return Languages.LanguageStrings.Settings;
		}

		public override int SortOrder()
		{
			return 0;
		}

		public override bool Enabled()
		{
			return _pluginSettings != null;
		}

		public IPluginSettings PluginSettings => _pluginSettings;
	}
}

#pragma warning restore CS1591