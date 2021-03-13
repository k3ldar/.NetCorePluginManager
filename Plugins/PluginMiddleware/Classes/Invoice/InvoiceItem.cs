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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: InvoiceItem.cs
 *
 *  Purpose:  Order Item
 *
 *  Date        Name                Reason
 *   04/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using static Shared.Utilities;

namespace Middleware.Accounts.Invoices
{
    /// <summary>
    /// Represents an individual invoice item within an Invoice used by IAccountProvider and UserAccount.Plugin module.
    /// </summary>
    public sealed class InvoiceItem
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Unique id of the invoice item.</param>
        /// <param name="description">Description of the item within the invoice.</param>
        /// <param name="cost">Cost of the item in the invoice.</param>
        /// <param name="taxRate">Tax rate applied to the invoiceable item.</param>
        /// <param name="quantity">Quantity of the item within the invoice.</param>
        /// <param name="status">Current status of the item within the invoice.</param>
        /// <param name="discountType">Type of discount, if appliccable, for the item within the invoice.</param>
        /// <param name="discount">Discount applied to the item within the invoice.</param>
        public InvoiceItem(in long id, in string description, in decimal cost,
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

        /// <summary>
        /// Unique id of the invoice item.
        /// </summary>
        /// <value>long</value>
        public long Id { get; private set; }

        /// <summary>
        /// Invoice to which the item belongs.
        /// </summary>
        /// <value>Invoice</value>
        public Invoice Invoice { get; internal set; }

        /// <summary>
        /// Description of the item within the invoice.
        /// </summary>
        /// <value>string</value>
        public string Description { get; private set; }

        /// <summary>
        /// Cost of the item in the invoice.
        /// </summary>
        /// <value>decimal</value>
        public decimal Price { get; private set; }

        /// <summary>
        /// Tax rate applied to the invoiceable item.
        /// </summary>
        /// <value>int</value>
        public int TaxRate { get; private set; }

        /// <summary>
        /// Quantity of the item within the invoice.
        /// </summary>
        /// <value>decimal</value>
        public decimal Quantity { get; private set; }

        /// <summary>
        /// Discount applied to the item within the invoice.
        /// </summary>
        /// <value>decimal</value>
        public decimal Discount { get; private set; }

        /// <summary>
        /// Type of discount, if appliccable, for the item within the invoice.
        /// </summary>
        /// <value>DiscountType</value>
        public DiscountType DiscountType { get; private set; }

        /// <summary>
        /// Current status of the item within the invoice.
        /// </summary>
        /// <value>ItemStatus</value>
        public ItemStatus Status { get; private set; }

        /// <summary>
        /// Total + Tax
        /// </summary>
        /// <value>decimal</value>
        public decimal Cost
        {
            get
            {
                decimal taxRate = 1 + ((decimal)TaxRate / 100);
                decimal totalCost = BankersRounding(Price * Quantity, Invoice.Culture.NumberFormat.NumberDecimalDigits);

                return BankersRounding(taxRate * totalCost, Invoice.Culture.NumberFormat.NumberDecimalDigits) - TotalDiscount;
            }
        }

        /// <summary>
        /// Total Tax amount
        /// </summary>
        /// <value>decimal</value>
        public decimal TotalTax
        {
            get
            {
                decimal taxRate = 1 + ((decimal)TaxRate / 100);
                decimal totalCost = BankersRounding(Price * Quantity, Invoice.Culture.NumberFormat.NumberDecimalDigits);

                return BankersRounding(taxRate * totalCost, Invoice.Culture.NumberFormat.NumberDecimalDigits) - totalCost;
            }
        }

        /// <summary>
        /// Subtotal of this item within the invoice.
        /// </summary>
        /// <value>decimal</value>
        public decimal SubTotal
        {
            get
            {
                return BankersRounding(Price * Quantity, Invoice.Culture.NumberFormat.NumberDecimalDigits);
            }
        }


        /// <summary>
        /// Total discount for the item within the invoice.
        /// </summary>
        /// <value>decimal</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0047:Remove unnecessary parentheses", Justification = "For clarity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Intended for developers not end users")]
        public decimal TotalDiscount
        {
            get
            {
                if (Discount == 0)
                    return Discount;

                return DiscountType switch
                {
                    DiscountType.None => 0,
                    DiscountType.PercentageSubTotal => BankersRounding((SubTotal / 100) * Discount, Invoice.Culture.NumberFormat.NumberDecimalDigits),
                    DiscountType.PercentageTotal => BankersRounding((SubTotal + TotalTax) / 100 * Discount, Invoice.Culture.NumberFormat.NumberDecimalDigits),
                    DiscountType.Value => BankersRounding(Discount, Invoice.Culture.NumberFormat.NumberDecimalDigits),
                    _ => throw new InvalidOperationException("Invalid Discount Type"),
                };
            }
        }

        #endregion Properties
    }
}
