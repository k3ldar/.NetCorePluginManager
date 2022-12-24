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
 *  Product:  Breadcrumb.Plugin
 *  
 *  File: BreadcrumbSettings.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  20/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AppSettings;

using SharedPluginFeatures;

namespace Breadcrumb.Plugin
{
    /// <summary>
    /// Settings which affect how breadcrumb data is served.
    /// </summary>
    public class BreadcrumbSettings : IPluginSettings
	{
		#region Properties

		/// <summary>
		/// Name
		/// </summary>
		public string SettingsName => nameof(Breadcrumb);

        /// <summary>
        /// Determines whether breadcrumb data is applied to static filed, .css, .js etc
        /// </summary>
        /// <value>bool</value>
        public bool ProcessStaticFiles { get; set; }

        /// <summary>
        /// Delimited list of file extensions to ignore
        /// </summary>
        /// <value>string</value>
        [SettingDefault(Constants.StaticFileExtensions)]
        [SettingString(false)]
        [SettingDelimitedString(';', 1)]
        public string StaticFileExtensions { get; set; }

        /// <summary>
        /// Name of home, usually Home or similar.  If Localization is enabled then this will be the 
        /// value looked up from resource strings.
        /// </summary>
        /// <value>string</value>
        [SettingDefault(nameof(Languages.LanguageStrings.Home))]
        [SettingString(false)]
        public string HomeName { get; set; }

        /// <summary>
        /// Name of home controller, without the Controller appendage, i.e. Home
        /// </summary>
        /// <value>string</value>
        [SettingDefault("Home")]
        [SettingString(false)]
        public string HomeController { get; set; }

        /// <summary>
        /// Default action name for home route.
        /// </summary>
        /// <value>string</value>
        [SettingDefault("Index")]
        [SettingString(false)]
        public string DefaultAction { get; set; }

        #endregion Properties
    }
}
