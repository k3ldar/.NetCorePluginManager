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
 *  Copyright (c) 2012 - 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: ShoppingCartSummary.cs
 *
 *  Purpose:  Provide summary for display on any page
 *
 *  Date        Name                Reason
 *  07/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Globalization;

namespace SharedPluginFeatures
{
    public class ShoppingCartSummary
    {
        #region Constructors

        public ShoppingCartSummary(in long id, in int totalItems, in decimal subTotal,
            in decimal discountRate, in decimal shipping, in decimal taxRate, in CultureInfo culture)
        {
            Id = id;

            if (totalItems < 0)
                throw new ArgumentOutOfRangeException(nameof(totalItems));

            if (subTotal < 0)
                throw new ArgumentOutOfRangeException(nameof(subTotal));

            if (discountRate < 0 || discountRate > 100)
                throw new ArgumentOutOfRangeException(nameof(discountRate));

            TotalItems = totalItems;
            SubTotal = subTotal;
            DiscountRate = discountRate;
            Shipping = shipping;
            TaxRate = taxRate;
            Culture = culture ?? throw new ArgumentNullException(nameof(culture));

            ResetTotalCost(subTotal);
        }

        #endregion Constructors

        #region Public Methods

        public void ResetShoppingCartId(in long id)
        {
            if (Id != 0)
                throw new InvalidOperationException();

            Id = id;
        }

        #endregion Public Methods

        #region Protected Methods

        protected void ResetTotalItems(in int totalItems)
        {
            if (totalItems < 0)
                throw new InvalidOperationException();

            TotalItems = totalItems;
        }

        protected void ResetTotalCost(in decimal cost, in CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
                throw new ArgumentNullException(nameof(cultureInfo));

            ResetTotalCost(cost);
        }

        protected void ResetTotalCost(in decimal cost)
        {
            if (cost < 0)
                throw new InvalidOperationException();

            SubTotal = cost;

            decimal total = SubTotal + Shipping;

            if (DiscountRate > 0 && total > 0)
                Discount = Shared.Utilities.BankersRounding((total / 100) * DiscountRate, 2);

            Total = total - Discount;
            Tax = Shared.Utilities.BankersRounding(Shared.Utilities.VATCalculatePaid(Total, TaxRate), 2);
        }

        #endregion Protected Methods

        #region Properties

        public long Id { get; private set; }

        public int TotalItems { get; private set; }

        public decimal SubTotal { get; private set; }

        public decimal DiscountRate { get; private set; }

        public decimal Discount { get; private set; }

        public decimal TaxRate { get; private set; }

        public decimal Tax { get; private set; }

        public decimal Shipping { get; private set; }

        public decimal Total { get; private set; }

        public CultureInfo Culture { get; private set; }

        #endregion Properties
    }
}
