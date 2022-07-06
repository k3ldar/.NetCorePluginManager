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
        string _sKU;
        bool _isDownload;
        int _weight;
        string _customerReference;
        bool _canBackOrder;
        string _size;
        uint _stockAvailability;

        [ForeignKey(Constants.TableNameShoppingCart)]
        public long ShoppingCartId
        {
            get
            {
                return _shoppingCartId;
            }
        
            set
            {
                _shoppingCartId = value;
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
                _itemCount = value;
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
                _itemCost = value;
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
                _taxRate = value;
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
            }
        }

        public string SKU
        {
            get
            {
                return _sKU;
            }
        
            set
            {
                _sKU = value;
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
                _weight = value;
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
                _customerReference = value;
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
                _canBackOrder = value;
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
                _size = value;
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
            }
        }
    }
}
