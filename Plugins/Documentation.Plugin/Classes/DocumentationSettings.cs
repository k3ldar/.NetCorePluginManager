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
 *  Product:  Documentation Plugin
 *  
 *  File: DefaultDocumentationService.cs
 *
 *  Purpose:  Provides default implementation of documentation service
 *
 *  Date        Name                Reason
 *  19/05/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using AppSettings;

using SharedPluginFeatures;

namespace DocumentationPlugin.Classes
{
    /// <summary>
    /// Settings which affect how the Documentation Plugin is configured.
    /// </summary>
    public sealed class DocumentationSettings : IPluginSettings
	{
        /// <summary>
        /// Default path where documentation files are located.
        /// 
        /// Default value: %AppPath%\\Plugins
        /// </summary>
        /// <value>string</value>
        [SettingDefault("%AppPath%\\Plugins")]
        public string Path { get; set; }

		/// <summary>
		/// Name
		/// </summary>
		public string SettingsName => Controllers.DocsController.Name;
	}
}
