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
 *  File: ShoppingCartDetail.cs
 *
 *  Purpose:  User Account provider
 *
 *  Date        Name                Reason
 *  07/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Globalization;

using SharedPluginFeatures;

namespace Middleware.ShoppingCart
{
    public sealed class ShoppingCartDetail : ShoppingCartSummary
    {
        #region Constructors

        public ShoppingCartDetail(in long id, in int totalItems, in decimal totalCost, 
            in CultureInfo culture, in decimal tax, in decimal subTotal, 
            in decimal discount, in decimal shipping,
            in string couponCode, in List<ShoppingCartItem> items)
            : base(id, totalItems, totalCost, culture)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));

            if (tax < 0 || (tax > 0 && items.Count < 1))
                throw new ArgumentOutOfRangeException(nameof(tax));

            if (subTotal < 0 || (subTotal > 0 && items.Count < 1))
                throw new ArgumentOutOfRangeException(nameof(subTotal));

            if (discount < 0 || (discount > 0 && items.Count < 1) || (discount > totalCost))
                throw new ArgumentOutOfRangeException(nameof(discount));

            if (shipping < 0 || (shipping > 0 && items.Count < 1) || (shipping > 0 && totalCost == 0))
                throw new ArgumentOutOfRangeException(nameof(shipping));


            Tax = tax;
            SubTotal = subTotal;
            Discount = discount;
            Shipping = shipping;
            CouponCode = couponCode ?? String.Empty;
        }

        #endregion Constructors

        #region Properties

        public decimal Tax { get; private set; }

        public decimal SubTotal { get; private set; }

        public decimal Discount { get; private set; }

        public decimal Shipping { get; private set; }

        public string CouponCode { get; private set; }

        public List<ShoppingCartItem> Items { get; private set; }

        #endregion Properties
    }
}
