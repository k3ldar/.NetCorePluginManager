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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Products.Plugin
 *  
 *  File: ProductControllerSettings.cs
 *
 *  Purpose:  Product Controller Settings
 *
 *  Date        Name                Reason
 *  31/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AppSettings;

namespace ProductPlugin
{
    /// <summary>
    /// Products which affect how ProductsPlugin is configured.
    /// </summary>
    public sealed class ProductControllerSettings
    {
        /// <summary>
        /// Number of products to display on each page.
        /// 
        /// Default: 12
        /// Minimum: 1
        /// Maximum: 500
        /// </summary>
        /// <value>uint</value>
        [SettingDefault(12u)]
        [SettingRange(1u, 500u)]
        public uint ProductsPerPage { get; set; }
    }
}
