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
 *  File: ProductDataRow.cs
 *
 *  Purpose:  Table definition for products
 *
 *  Date        Name                Reason
 *  26/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using PluginManager.DAL.TextFiles.Internal;

namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.TableNameProducts, CompressionType.None, CachingStrategy.None)]
    internal class ProductDataRow : TableRowDefinition
    {
        private int _productGroupId;
        private string _name;
        private string _description;
        private string _features;
        private string _videoLink;
        private bool _newProduct;
        private bool _bestSeller;
        private decimal _retailPrice;
        private string _sku;
        private bool _isDownload;
        private bool _allowBackorder;
        private uint _stockAvailability;

        [ForeignKey(Constants.TableNameProductGroups)]
        public int ProductGroupId
        {
            get
            {
                return _productGroupId;
            }

            set
            {
                _productGroupId = value;
                Update();
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                Update();
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
                Update();
            }
        }

        public string Features
        {
            get
            {
                return _features;
            }

            set
            {
                _features = value;
                Update();
            }
        }

        public string VideoLink
        {
            get
            {
                return _videoLink;
            }

            set
            {
                _videoLink = value;
                Update();
            }
        }

        public bool NewProduct
        {
            get
            {
                return _newProduct;
            }

            set
            {
                _newProduct = value;
                Update();
            }
        }

        public bool BestSeller
        {
            get
            {
                return _bestSeller;
            }

            set
            {
                _bestSeller = value;
                Update();
            }
        }

        public decimal RetailPrice
        {
            get
            {
                return _retailPrice;
            }

            set
            {
                _retailPrice = value;
                Update();
            }
        }

        [UniqueIndex]
        public string Sku
        {
            get
            {
                return _sku;
            }

            set
            {
                _sku = value;
                Update();
            }
        }

        public bool IsDownload
        {
            get
            {
                return _isDownload;
            }

            set
            {
                _isDownload = value;
                Update();
            }
        }

        public bool AllowBackorder
        {
            get
            {
                return _allowBackorder;
            }

            set
            {
                _allowBackorder = value;
                Update();
            }
        }

        public uint StockAvailability
        {
            get
            {
                return _stockAvailability;
            }

            set
            {
                _stockAvailability = value;
                Update();
            }
        }
    }
}
