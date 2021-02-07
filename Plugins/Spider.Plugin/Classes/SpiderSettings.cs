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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Spider.Plugin
 *  
 *  File: SpiderSettings.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  29/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AppSettings;

namespace Spider.Plugin
{
    /// <summary>
    /// Contains setting values that determine how Spider.Plugin is configured.
    /// </summary>
    public class SpiderSettings
    {
        #region Properties

        /// <summary>
        /// Determines whether static files are ignored when determining whether a connection is allowed to connect to the resource.
        /// </summary>
        /// <value>string</value>
        public bool ProcessStaticFiles { get; set; }

        /// <summary>
        /// Delimited list of file extensions to ignore
        /// </summary>
        /// <value>string</value>
        [SettingDefault(SharedPluginFeatures.Constants.StaticFileExtensions)]
        [SettingString(false)]
        [SettingDelimitedString(';', 1)]
        public string StaticFileExtensions { get; set; }

        /// <summary>
        /// Provides an opportunity to add a non existant route to sitemap which is dissalowed, if a bot goes to the route
        /// data is logged about the none behaving bot who is abusing sitemap.
        /// 
        /// The value must be a valid root, i.e. /api/v19.x/ that is not in use elsewhere, this route is added to the sitemap
        /// and any visits to the route are notified.
        /// </summary>
        /// <value>string</value>
        [SettingDefault("/identities/reveal")]
        [SettingOptional]
        [SettingUri(false, System.UriKind.Relative)]
        public string BotTrapRoute { get; set; }

        #endregion Properties
    }
}
