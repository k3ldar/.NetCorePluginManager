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
 *  Copyright (c) 2019 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Shopping Cart Plugin
 *  
 *  File: SunTech24PaymentSettings.cs
 *
 *  Purpose:  Settings for sun tech 24
 *
 *  Date        Name                Reason
 *  24/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using AppSettings;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace ShoppingCartPlugin.Classes
{
    public sealed class SunTech24PaymentSettings : PaymentProviderSettings, IPluginSettings
    {
		/// <summary>
		/// Name
		/// </summary>
		public string SettingsName => "SunTech24";

        [SettingString(false, 1, 100)]
        public string MerchantPassword { get; set; }

        [SettingString(false, 1, 100)]
        public string MerchantId { get; set; }

        public bool TestMode { get; set; }

        [SettingRange(1, 500)]
        public int DueDateDays { get; set; }
	}
}

#pragma warning restore CS1591