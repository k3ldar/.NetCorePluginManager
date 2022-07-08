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
 *  File: OrderItemsDataRow.cs
 *
 *  Purpose:  Table definition for user orders items
 *
 *  Date        Name                Reason
 *  06/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using PluginManager.DAL.TextFiles.Internal;

namespace PluginManager.DAL.TextFiles.Tables
{
    [Table(Constants.TableNameOrderItems)]
    internal class OrderItemsDataRow : TableRowDefinition
    {
        private long _orderId;
        private int _discountType;
        private int _status;
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
                _discountType = value;
                Update();
            }
        }

        public int Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
                Update();
            }
        }
    }
}
