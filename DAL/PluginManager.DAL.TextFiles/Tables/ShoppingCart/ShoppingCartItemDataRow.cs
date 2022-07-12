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
 *  File: ShoppingCartItemDataRow.cs
 *
 *  Purpose:  Table definition for shopping cart items
 *
 *  Date        Name                Reason
 *  02/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using PluginManager.DAL.TextFiles.Internal;

namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.TableNameShoppingCartItems)]
    internal class ShoppingCartItemDataRow : TableRowDefinition
    {
        long _shoppingCartId;
        decimal _itemCount;
        decimal _itemCost;
        decimal _taxRate;
        string _name;
        string _description;
        string _sku;
        bool _isDownload;
        int _weight;
        string _customerReference;
        bool _canBackOrder;
        string _size;
        uint _stockAvailability;
        private decimal _discount;
        private int _discountType;
        private long _itemId;

        [ForeignKey(Constants.TableNameShoppingCart)]
        public long ShoppingCartId
        {
            get
            {
                return _shoppingCartId;
            }

            set
            {
                if (_shoppingCartId == value)
                    return;

                _shoppingCartId = value;
                Update();
            }
        }

        [ForeignKey(Constants.TableNameProducts)]
        public long ProductId
        {
            get
            {
                return _itemId;
            }

            set
            {
                if (_itemId == value)
                    return;

                _itemId = value;
                Update();
            }
        }

        public decimal ItemCount
        {
            get
            {
                return _itemCount;
            }

            set
            {
                if (_itemCount == value)
                    return;

                _itemCount = value;
                Update();
            }
        }

        public decimal ItemCost
        {
            get
            {
                return _itemCost;
            }

            set
            {
                if (_itemCost == value)
                    return;

                _itemCost = value;
                Update();
            }
        }

        public decimal TaxRate
        {
            get
            {
                return _taxRate;
            }

            set
            {
                if (_taxRate == value)
                    return;

                _taxRate = value;
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
                if (_name == value)
                    return;

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
                if (_description == value)
                    return;

                _description = value;
                Update();
            }
        }

        public string SKU
        {
            get
            {
                return _sku;
            }

            set
            {
                if (_sku == value)
                    return;

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
                if (_isDownload == value)
                    return;

                _isDownload = value;
                Update();
            }
        }

        public int Weight
        {
            get
            {
                return _weight;
            }

            set
            {
                if (_weight == value)
                    return;

                _weight = value;
                Update();
            }
        }

        public string CustomerReference
        {
            get
            {
                return _customerReference;
            }

            set
            {
                if (_customerReference == value)
                    return;

                _customerReference = value;
                Update();
            }
        }

        public bool CanBackOrder
        {
            get
            {
                return _canBackOrder;
            }

            set
            {
                if (_canBackOrder == value)
                    return;

                _canBackOrder = value;
                Update();
            }
        }

        public string Size
        {
            get
            {
                return _size;
            }

            set
            {
                if (_size == value)
                    return;

                _size = value;
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
                if (_stockAvailability == value)
                    return;

                _stockAvailability = value;
                Update();
            }
        }

        public decimal DiscountRate
        {
            get
            {
                return _discount;
            }

            set
            {
                if (value == _discount)
                    return;

                _discount = value;
                Update();
            }
        }

        public int DiscountType
        {
            get => _discountType;

            set
            {
                if (value == _discountType)
                    return;

                _discountType = value;
                Update();
            }
        }
    }
}
