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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: OrderItem.cs
 *
 *  Purpose:  Order Item
 *
 *  Date        Name                Reason
 *  31/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using static Shared.Utilities;

namespace Middleware.Accounts.Orders
{
    public sealed class OrderItem
    {
        #region Constructors

        public OrderItem(in Int64 id, in string description, in decimal cost, 
            in int taxRate, in decimal quantity, in ItemStatus status, in DiscountType discountType, 
            in decimal discount)
        {
            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));

            if (cost < 0)
                throw new ArgumentOutOfRangeException(nameof(cost));

            if (taxRate < 0 || taxRate > 100)
                throw new ArgumentOutOfRangeException(nameof(taxRate));

            if (quantity < 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            if (discount < 0)
                throw new ArgumentOutOfRangeException(nameof(discount));

            Id = id;
            Description = description;
            Price = cost;
            TaxRate = taxRate;
            Quantity = quantity;
            Status = status;
            DiscountType = discountType;
            Discount = discount;
        }

        #endregion Constructors

        #region Properties

        public Int64 Id { get; private set; }

        public Order Order { get; internal set; }

        public string Description { get; private set; }

        public decimal Price { get; private set; }

        public int TaxRate { get; private set; }

        public decimal Quantity { get; private set; }

        public decimal Discount { get; private set; }

        public DiscountType DiscountType { get; private set; }

        public ItemStatus Status { get; private set; }

        /// <summary>
        /// Total + Tax
        /// </summary>
        public decimal Cost
        {
            get
            {
                decimal taxRate = 1 + ((decimal)TaxRate / 100);
                decimal totalCost = BankersRounding(Price * Quantity, Order.Culture.NumberFormat.NumberDecimalDigits);

                return BankersRounding(taxRate * totalCost, Order.Culture.NumberFormat.NumberDecimalDigits) - TotalDiscount;
            }
        }

        /// <summary>
        /// Total Tax amount
        /// </summary>
        public decimal TotalTax
        {
            get
            {
                decimal taxRate = 1 + ((decimal)TaxRate / 100);
                decimal totalCost = BankersRounding(Price * Quantity, Order.Culture.NumberFormat.NumberDecimalDigits);

                return BankersRounding(taxRate * totalCost, Order.Culture.NumberFormat.NumberDecimalDigits) - totalCost;
            }
        }

        public decimal SubTotal
        {
            get
            {
                return BankersRounding(Price * Quantity, Order.Culture.NumberFormat.NumberDecimalDigits);
            }
        }

        public decimal TotalDiscount
        {
            get
            {
                if (Discount == 0)
                    return Discount;

                switch (DiscountType)
                {
                    case DiscountType.None:
                        return 0;

                    case DiscountType.PercentageSubTotal:
                        return BankersRounding((SubTotal / 100) * Discount, Order.Culture.NumberFormat.NumberDecimalDigits);

                    case DiscountType.PercentageTotal:
                        return BankersRounding(((SubTotal + TotalTax) / 100) * Discount, Order.Culture.NumberFormat.NumberDecimalDigits);

                    case DiscountType.Value:
                        return BankersRounding(Discount, Order.Culture.NumberFormat.NumberDecimalDigits);

                    default:
                        throw new InvalidOperationException("Invalid Discount Type");
                }
            }
        }

        #endregion Properties
    }
}
