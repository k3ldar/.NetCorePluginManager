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
 *  File: CartSettings.cs
 *
 *  Purpose:  Settings for paypoint provider
 *
 *  Date        Name                Reason
 *  24/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AppSettings;

using SharedPluginFeatures;

namespace ShoppingCartPlugin.Classes
{
    /// <summary>
    /// Settings that configure how the Shopping cart is configured.
    /// </summary>
    public sealed class CartSettings : IPluginSettings
	{
		/// <summary>
		/// Name
		/// </summary>
		public string SettingsName => Controllers.CartController.Name;

        /// <summary>
        /// Default currency to be used.
        /// </summary>
        [SettingString(false, 3, 3)]
        [SettingDefault("GBP")]
        public string DefaultCurrency { get; set; }

        /// <summary>
        /// Default tax rate to be applied.  This can later be overridden depending on shipping address.
        /// </summary>
        public decimal DefaultTaxRate { get; set; }
    }
}
