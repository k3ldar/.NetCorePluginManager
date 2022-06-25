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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: CountriesDataRow.cs
 *
 *  Purpose:  Row definition for Table for countries
 *
 *  Date        Name                Reason
 *  31/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using PluginManager.DAL.TextFiles.Internal;

namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.TableNameCountries, CompressionType.None, CachingStrategy.Memory)]
    internal sealed class CountryDataRow : TableRowDefinition
    {
        /// <summary>
        /// Name of country.
        /// </summary>
        /// <value>string</value>
        public string Name { get; set; }

        /// <summary>
        /// Country code.
        /// </summary>
        /// <value>string</value>
        [UniqueIndex(IndexType.Ascending)]
        public string Code { get; set; }

        /// <summary>
        /// Indicates whether the country is visible or not.
        /// </summary>
        /// <value>bool</value>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Sort order
        /// </summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// Indicates whether prices are shown for a country or not
        /// </summary>
        public bool ShowPriceData { get; set; } = true;

        /// <summary>
        /// Indicates that vat/tax should be removed for this country
        /// </summary>
        public bool AllowVatRemoval { get; set; } = false;

        /// <summary>
        /// Tax rate applied to the country
        /// </summary>
        public decimal TaxRate { get; set; } = decimal.Zero;

        /// <summary>
        /// Cost multiplier to enable charging different costs for different countries
        /// </summary>
        public decimal CostMultiplier { get; set; } = 1.0m;
    }
}
