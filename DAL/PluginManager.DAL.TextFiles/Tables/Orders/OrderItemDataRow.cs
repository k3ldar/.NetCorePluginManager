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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: OrderItemsDataRow.cs
 *
 *  Purpose:  Table definition for user orders items
 *
 *  Date        Name                Reason
 *  06/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.TableNameOrderItems)]
    internal class OrderItemDataRow : TableRowDefinition
    {
        private long _orderId;
        private int _discountType;
        private int _itemStatus;
        private string _description;
        private decimal _taxRate;
        private decimal _price;
        private decimal _discount;
        private decimal _quantity;

        [ForeignKey(Constants.TableNameOrders)]
        public long OrderId
        {
            get
            {
                return _orderId;
            }

            set
            {
                if (_orderId == value)
                    return;

                _orderId = value;
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

        public decimal Price
        {
            get
            {
                return _price;
            }

            set
            {
                if (_price == value)
                    return;

                _price = value;
                Update();
            }
        }

        public decimal Quantity
        {
            get
            {
                return _quantity;
            }

            set
            {
                if (_quantity == value)
                    return;

                _quantity = value;
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
                if (_discount == value)
                    return;

                _discount = value;
                Update();
            }
        }

        public int DiscountType
        {
            get
            {
                return _discountType;
            }

            set
            {
                if (_discountType == value)
                    return;

                _discountType = value;
                Update();
            }
        }

        public int ItemStatus
        {
            get
            {
                return _itemStatus;
            }

            set
            {
                if (_itemStatus == value)
                    return;

                _itemStatus = value;
                Update();
            }
        }
    }
}
