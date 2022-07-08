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
        private decimal _costMultiplier = 1.0m;
        private decimal _taxRate = decimal.Zero;
        private bool _allowVatRemoval = false;
        private bool _showPriceData = true;
        private int _sortOrder = 0;
        private bool _visible = true;
        private string _code;
        private string _name;

        /// <summary>
        /// Name of country.
        /// </summary>
        /// <value>string</value>
        public string Name
        {
            get => _name;

            set
            {
                _name = value;
                Update();
            }
        }

        /// <summary>
        /// Country code.
        /// </summary>
        /// <value>string</value>
        [UniqueIndex(IndexType.Ascending)]
        public string Code
        {
            get => _code;

            set
            {
                if (_code == value)
                    return;

                _code = value;
                Update();
            }
        }

        /// <summary>
        /// Indicates whether the country is visible or not.
        /// </summary>
        /// <value>bool</value>
        public bool Visible
        {
            get => _visible;

            set
            {
                if (_visible == value)
                    return;

                _visible = value;
                Update();
            }
        }

        /// <summary>
        /// Sort order
        /// </summary>
        public int SortOrder
        {
            get => _sortOrder;

            set
            {
                if (_sortOrder == value)
                    return;

                _sortOrder = value;
                Update();
            }
        }

        /// <summary>
        /// Indicates whether prices are shown for a country or not
        /// </summary>
        public bool ShowPriceData
        {
            get => _showPriceData;

            set
            {
                if (_showPriceData == value)
                    return;

                _showPriceData = value;
                Update();
            }
        }

        /// <summary>
        /// Indicates that vat/tax should be removed for this country
        /// </summary>
        public bool AllowVatRemoval
        {
            get => _allowVatRemoval;

            set
            {
                if (_allowVatRemoval == value)
                    return;

                _allowVatRemoval = value;
                Update();
            }
        }

        /// <summary>
        /// Tax rate applied to the country
        /// </summary>
        public decimal TaxRate
        {
            get => _taxRate;

            set
            {
                if (_taxRate == value)
                    return;

                _taxRate = value;
                Update();
            }
        }

        /// <summary>
        /// Cost multiplier to enable charging different costs for different countries
        /// </summary>
        public decimal CostMultiplier
        {
            get => _costMultiplier;

            set
            {
                if (_costMultiplier == value)
                    return;

                _costMultiplier = value;
                Update();
            }
        }
    }
}
