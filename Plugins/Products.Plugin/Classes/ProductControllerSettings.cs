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

        /// <summary>
        /// Price groups that users can search for, this is a delimited list that must contain decimal values
        /// 
        /// e.g.
        /// 0;5.00;10.00;20.00;35.00;50.00
        /// 
        /// This would be displayed as 
        /// Free
        /// Under 5
        /// 5 to 10
        /// 10to 20
        /// 20 to 35
        /// 35 to 50
        /// Over 50
        /// </summary>
        [SettingDelimitedString(';', 1u, 8u)]
        [SettingDefault("0;5.00;10.00;20.00;35.00;50.00")]
        public string PriceGroups { get; set; }

        /// <summary>
        /// If true, the number of products that match the search item will be displayed in brackets next to the value.
        /// 
        /// For instance, if 3 products are valued at 3.99, given the default PriceGroups you would see
        /// 
        /// Under 5 (3)
        /// </summary>
        [SettingDefault(true)]
        public bool ShowProductCounts { get; set; }
    }
}
