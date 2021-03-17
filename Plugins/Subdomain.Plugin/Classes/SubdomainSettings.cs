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
 *  Copyright (c) 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Subdomain.Plugin
 *  
 *  File: SubdomainSettings.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  21/02/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using AppSettings;

using SharedPluginFeatures;

namespace Subdomain.Plugin
{
    /// <summary>
    /// Settings for all subdomains using the <seealso cref="SubdomainAttribute"/>
    /// </summary>
    public class SubdomainSettings
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public SubdomainSettings()
        {
            Subdomains = new Dictionary<string, SubdomainSetting>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Dictionary of subdomain settings for each individual subdomain
        /// </summary>
        /// <value>Dictionary&lt;string, SubdomainSetting&gt;</value>
        public Dictionary<string, SubdomainSetting> Subdomains { get; private set; }

        /// <summary>
        /// Indicates whether subdomain validation is enabled or not, by default this value 
        /// is false and must be specifically enabled.
        /// </summary>
        /// <value>bool</value>
        public bool Enabled { get; set; }

        /// <summary>
        /// If disabled, auto redirect from subdomain for route that has no subdomain does not
        /// redirect to www, instead it will redirect to domain without subdomain, i.e.
        /// http://pluginmanager.website instead of http://www.pluginmanger.website
        /// </summary>
        /// <value>bool</value>
        public bool DisableRedirectWww { get; set; }

        /// <summary>
        /// Represents the full domain name of the domain to be used for subdomains, i.e.
        /// 
        /// pluginmanager.website
        /// 
        /// This value must include the toplevel domain name but no subdomains including www
        /// </summary>
        /// <value>string</value>
        [SettingString(false)]
        public string DomainName { get; set; }

        /// <summary>
        /// Custom semicolon seperated list of static file extensions
        /// 
        /// Default value is: .less;.ico;.css;.js;.svg;.jpg;.jpeg;.gif;.png;.eot;.map;
        /// </summary>
        /// <value>string</value>
        [SettingDefault(Constants.StaticFileExtensions)]
        [SettingString(false)]
        [SettingDelimitedString(';', 1)]
        public string StaticFileExtensions { get; set; }

        /// <summary>
        /// Indicates whether the middleware processes static files, .js, .css etc or not
        /// </summary>
        /// <value>bool</value>
        public bool ProcessStaticFiles { get; set; }

        #endregion Properties
    }
}
