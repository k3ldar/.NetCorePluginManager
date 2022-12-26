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
 *  Product:  MemoryCachePlugin
 *  
 *  File: MemoryClassPluginSettings.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AppSettings;

using SharedPluginFeatures;

namespace MemoryCache.Plugin
{
    /// <summary>
    /// Provides settings loaded by ISettingsProvider that determine how MemoryCachePlugin is configures.
    /// </summary>
    public class MemoryClassPluginSettings : IPluginSettings
	{
        #region Properties

		/// <summary>
		/// Name
		/// </summary>
		public string SettingsName => "MemoryCachePluginConfiguration";

        /// <summary>
        /// Default number of minutes the default cache stores items.
        /// 
        /// Default: 120 minutes.
        /// Minimum: 30 minutes.
        /// Maximum: 2880 minutes.
        /// </summary>
        /// <value>int</value>
        [SettingDefault(120)]
        [SettingRange(30, 480)]
        public int DefaultCacheDuration { get; set; }

        /// <summary>
        /// Default number of minutes the short cache stores items.
        /// 
        /// Default: 5 minutes.
        /// Minimum: 1 minutes.
        /// Maximum: 60 minutes.
        /// </summary>
        /// <value>int</value>
        [SettingDefault(5)]
        [SettingRange(1, 60)]
        public int ShortCacheDuration { get; set; }

		#endregion Properties
	}
}
