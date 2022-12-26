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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Company.Plugin
 *  
 *  File: CompanySettings.cs
 *
 *  Purpose:  Settings
 *
 *  Date        Name                Reason
 *  07/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AppSettings;

using SharedPluginFeatures;

namespace Company.Plugin.Classes
{
	/// <summary>
	/// Settings for the Company plugin module which determine which pages are visible or not.
	/// </summary>
	public sealed class CompanySettings : IPluginSettings
	{
		#region Properties

		/// <summary>
		/// Name
		/// </summary>
		public string SettingsName => nameof(CompanySettings);

		/// <summary>
		/// Show the about page, provide information about the website and/or company operating it.
		/// </summary>
		/// <value>bool</value>
		[SettingDefault(true)]
		public bool ShowAbout { get; set; }


		/// <summary>
		/// Show the careers page, if the company has job openings, details can be shown on the careers page.
		/// </summary>
		/// <value>bool</value>
		[SettingDefault(false)]
		public bool ShowCareers { get; set; }


		/// <summary>
		/// Show the contact us page, provide visitors with company contact details.
		/// </summary>
		/// <value>bool</value>
		[SettingDefault(true)]
		public bool ShowContact { get; set; }


		/// <summary>
		/// Show the cookie page, provide visitors with details information regarding the site cookie policy.
		/// </summary>
		/// <value>bool</value>
		[SettingDefault(false)]
		public bool ShowCookies { get; set; }


		/// <summary>
		/// Show the delivery page, detailing information on delivery options etc.
		/// </summary>
		/// <value>bool</value>
		[SettingDefault(false)]
		public bool ShowDelivery { get; set; }


		/// <summary>
		/// Show the newsletter page, giving visitors details of any newsletters that may be provided.
		/// </summary>
		/// <value>bool</value>
		[SettingDefault(false)]
		public bool ShowNewsletter { get; set; }


		/// <summary>
		/// Show the privacy page, giving visitors details of the privacy policy.
		/// </summary>
		/// <value>bool</value>
		[SettingDefault(false)]
		public bool ShowPrivacy { get; set; }


		/// <summary>
		/// Show the returns page, if the company allows users to return items, details of the procedure/policy can be shown here.
		/// </summary>
		/// <value>bool</value>
		[SettingDefault(false)]
		public bool ShowReturns { get; set; }


		/// <summary>
		/// Show the Terms and conditions page, provide visitors with terms and conditions on using the website.
		/// </summary>
		/// <value>bool</value>
		[SettingDefault(false)]
		public bool ShowTerms { get; set; }


		/// <summary>
		/// Show the affiliates page, if an affiliate program is provided, details on its useage con be provided here.
		/// </summary>
		/// <value>bool</value>
		[SettingDefault(false)]
		public bool ShowAffiliates { get; set; }

		#endregion Properties
	}
}
