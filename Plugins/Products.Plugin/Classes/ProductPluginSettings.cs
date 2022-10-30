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
 *  Product:  Products.Plugin
 *  
 *  File: ProductPluginSettings.cs
 *
 *  Purpose:  Product Plugin Settings
 *
 *  Date        Name                Reason
 *  31/01/2019  Simon Carter        Initially Created
 *  03/06/2021  Simon Carter        Renamed
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AppSettings;

namespace ProductPlugin
{
    /// <summary>
    /// Products which affect how ProductsPlugin is configured.
    /// </summary>
    public sealed class ProductPluginSettings
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
        /// <value>string</value>
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
        /// <value>bool</value>
        [SettingDefault(true)]
        public bool ShowProductCounts { get; set; }

        /// <summary>
        /// Indicates that images that are uploaded will be resized to standard sizes
        /// </summary>
        /// <value>bool</value>
        [SettingDefault(true)]
        public bool ResizeImages { get; set; }

        /// <summary>
        /// Indicates the size of new images (widthxheight) from the original, for instance a web page could display images as thumbnails, within a product list etc.  Sizes 
        /// are automatically created based on the original size.  Therefor it is important that the new sizes are relative to original image size
        /// 
        /// Numbers less than 1 will be ignored, duplicate numbers will be ignored. 
        /// </summary>
        /// <value>string</value>
        [SettingDelimitedString(';', 1u, 8u)]
        [SettingDefault("178x128;148x114;200x145;89x64;288x268")]
        public string ResizeWidths { get; set; }

        /// <summary>
        /// Hexadecimal color value that will be used as a backfill when resizing images
        /// </summary>
        /// <value>string</value>
        [SettingDefault("#FFFFFF")]
        public string ResizeBackfillColor { get; set; }
    }
}
