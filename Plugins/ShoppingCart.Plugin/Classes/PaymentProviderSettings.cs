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
 *  Product:  Shopping Cart Plugin
 *  
 *  File: PaymentProviderSettings.cs
 *
 *  Purpose:  Settings for payment providers
 *
 *  Date        Name                Reason
 *  24/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AppSettings;

namespace ShoppingCartPlugin.Classes
{
	/// <summary>
	/// Settings which affect how the payment providers are configured.  
	/// 
	/// Each payment provider has its own settings, which determine how it is used.
	/// </summary>
	public class PaymentProviderSettings
	{
		/// <summary>
		/// Determines whether the payment provider can be used or not.
		/// </summary>
		/// <value>bool.  If true the payment provider is enabled.</value>
		public bool Enabled { get; set; }

		/// <summary>
		/// Unique guid which identifies the payment provider.
		/// </summary>
		/// <value>Guid</value>
		[SettingGuid]
		public string UniqueId { get; set; }

		/// <summary>
		/// Delimited list of supported currencies for the payment provider.
		/// 
		/// Default:    GBP;USD;EUR
		/// </summary>
		/// <value>string</value>
		[SettingDefault("GBP;USD;EUR")]
		[SettingDelimitedString(';', 1)]
		public string Currencies { get; set; }
	}
}
