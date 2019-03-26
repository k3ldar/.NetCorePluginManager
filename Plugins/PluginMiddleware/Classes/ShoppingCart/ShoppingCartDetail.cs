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
using System.Linq;

using SharedPluginFeatures;

using Middleware.Products;

namespace Middleware.ShoppingCart
{
    public sealed class ShoppingCartDetail : ShoppingCartSummary
    {
        #region Constructors

        public ShoppingCartDetail(in long id, in int totalItems, in decimal totalCost, 
            in decimal taxRate, in decimal shipping, in decimal discount, in CultureInfo culture, 
            in string couponCode, in List<ShoppingCartItem> items, in bool requiresShipping,
            in string currencyCode)
            : base(id, totalItems, totalCost, discount, shipping, taxRate, culture, currencyCode)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            CouponCode = couponCode ?? String.Empty;
            RequiresShipping = requiresShipping;
        }

        #endregion Constructors

        #region Properties

        public string CouponCode { get; private set; }

        public List<ShoppingCartItem> Items { get; private set; }

        public bool RequiresShipping { get; private set; }

        #endregion Properties

        #region Public Methods

        public void Add(in Product product, in int count)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (!RequiresShipping && !product.IsDownload)
                RequiresShipping = true;

            int existingId = product.Id;
            ShoppingCartItem existingItem = Items.Where(e => e.Id == existingId).FirstOrDefault();

            if (existingItem == null)
            {
                Items.Add(new ShoppingCartItem(product.Id, count, product.RetailPrice, product.Name, 
                    product.Description.Substring(0, Shared.Utilities.CheckMinMax(product.Description.Length, 0, 49)), 
                    product.Sku, product.Images, product.IsDownload, product.AllowBackorder, String.Empty));
            }
            else
            {
                existingItem.UpdateCount(count);
            }

            // reset totals for summary
            ResetTotalItems(TotalItems + count);
            ResetTotalCost(base.SubTotal + (product.RetailPrice * count));
        }

        public void Update(int productId, int quantity)
        {
            ShoppingCartItem existingItem = Items.Where(e => e.Id == productId).FirstOrDefault();

            if (existingItem == null)
                throw new ArgumentException(nameof(productId));

            existingItem.ResetCount(quantity);

            Reset();
        }

        public void Delete(int productId)
        {
            ShoppingCartItem item = Items.Where(i => i.Id == productId).FirstOrDefault();

            if (item != null)
            {
                ResetTotalItems(TotalItems - (int)item.ItemCount);
                ResetTotalCost(base.SubTotal - (item.ItemCost * item.ItemCount));
                Items.Remove(item);
            }
        }

        public void Reset()
        {
            ResetTotalItems((int)Items.Sum(s => s.ItemCount));
            ResetTotalCost(Items.Sum(s => s.ItemCost * s.ItemCount));
        }

        #endregion Public Methods
    }
}
