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
 *  File: ShoppingCartDataRow.cs
 *
 *  Purpose:  Table definition for shopping cart
 *
 *  Date        Name                Reason
 *  02/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Globalization;
using System.Text.Json.Serialization;

using PluginManager.DAL.TextFiles.Internal;

namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.TableNameShoppingCart)]
    internal class ShoppingCartDataRow : TableRowDefinition
    {
        int _totalItems;
        decimal _subTotal;
        decimal _discountRate;
        decimal _discount;
        decimal _taxRate;
        decimal _tax;
        decimal _shipping;
        decimal _total;
        string _culture;
        string _currencyCode;
        string _couponCode;
        bool _requiresShipping;
        int _deliveryAddressId;

        public int TotalItems
        {
            get
            {
                return _totalItems;
            }
        
            set
            {
                _totalItems = value;
                Update();
            }
        }

        public decimal SubTotal
        {
            get
            {
                return _subTotal;
            }
        
            set
            {
                _subTotal = value;
                Update();
            }
        }

        public decimal DiscountRate
        {
            get
            {
                return _discountRate;
            }
        
            set
            {
                _discountRate = value;
                Update();
            }
        }

        public decimal Discount
        {
            get
            {
                return _discount;
            }
        
            set
            {
                _discount = value;
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
                _taxRate = value;
                Update();
            }
        }

        public decimal Tax
        {
            get
            {
                return _tax;
            }
        
            set
            {
                _tax = value;
                Update();
            }
        }

        public decimal Shipping
        {
            get
            {
                return _shipping;
            }
        
            set
            {
                _shipping = value;
                Update();
            }
        }

        public decimal Total
        {
            get
            {
                return _total;
            }
        
            set
            {
                _total = value;
                Update();
            }
        }

        public string Culture
        {
            get
            {
                return _culture;
            }
        
            set
            {
                _culture = value;
                Update();
            }
        }

        public string CurrencyCode
        {
            get
            {
                return _currencyCode;
            }
        
            set
            {
                _currencyCode = value;
                Update();
            }
        }

        public string CouponCode
        {
            get
            {
                return _couponCode;
            }
        
            set
            {
                _couponCode = value;
                Update();
            }
        }

        public bool RequiresShipping
        {
            get
            {
                return _requiresShipping;
            }
        
            set
            {
                _requiresShipping = value;
                Update();
            }
        }

        public int DeliveryAddressId
        {
            get
            {
                return _deliveryAddressId;
            }
        
            set
            {
                _deliveryAddressId = value;
                Update();
            }
        }
    }
}
