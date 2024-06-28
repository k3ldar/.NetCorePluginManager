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
 *  Copyright (c) 2012 - 2022 Simon Carter.  All Rights Reserved.
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

using Middleware.Accounts;
using Middleware.Products;

using SharedPluginFeatures;

namespace Middleware.ShoppingCart
{
	/// <summary>
	/// Provides detailed information about shopping a users shopping cart.  Descends from ShoppingCartSummary
	/// and is used with IShoppingCartProvider and ShoppingCart.Plugin module.
	/// </summary>
	public sealed class ShoppingCartDetail : ShoppingCartSummary
	{
		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id">Unique id of the shopping cart.</param>
		/// <param name="totalItems">Total number of items within the shopping cart.</param>
		/// <param name="totalCost">Total cost of the shopping cart.</param>
		/// <param name="taxRate">Tax rate applied to the shopping cart.</param>
		/// <param name="shipping">Shipping rate for the shopping cart.</param>
		/// <param name="discount">Discount that has been applied to the shopping cart.</param>
		/// <param name="culture">Culture that has been used for the shopping cart.</param>
		/// <param name="couponCode">Coupon code that has been applied to the shopping cart.</param>
		/// <param name="items">List of items within the shopping cart.</param>
		/// <param name="requiresShipping">Indicates that the cart has items that need shipping.</param>
		/// <param name="currencyCode">Currency code for the shopping cart.</param>
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

		/// <summary>
		/// Coupon code that has been applied to the shopping cart.
		/// </summary>
		/// <value>string</value>
		public string CouponCode { get; private set; }

		/// <summary>
		/// List of all items within the shopping cart.
		/// </summary>
		/// <value>List&lt;ShoppingCartItems&gt;</value>
		public List<ShoppingCartItem> Items { get; private set; }

		/// <summary>
		/// Determines whether the cart contains items that require shipping.
		/// </summary>
		/// <value>bool.  True if items require shipping.</value>
		public bool RequiresShipping { get; private set; }

		/// <summary>
		/// Unique delivery address for the users, where the products will be shipped to.
		/// </summary>
		/// <value>int</value>
		public long DeliveryAddressId { get; private set; }

		#endregion Properties

		#region Public Methods

		/// <summary>
		/// Adds a new product to the shopping cart.
		/// </summary>
		/// <param name="product">Product to be added to the cart.</param>
		/// <param name="count">Quantity to be added to the cart.</param>
		public void Add(in Product product, in int count)
		{
			if (product == null)
				throw new ArgumentNullException(nameof(product));

			ArgumentOutOfRangeException.ThrowIfLessThan(count, 1);

			if (!RequiresShipping && !product.IsDownload)
				RequiresShipping = true;

			int existingId = product.Id;
			ShoppingCartItem existingItem = Items.Find(e => e.Id == existingId);

			if (existingItem == null)
			{
				Items.Add(new ShoppingCartItem(product.Id, count, product.Id, product.RetailPrice, product.Name,
					product.Description[..Shared.Utilities.CheckMinMax(product.Description.Length, 0, 49)],
					product.Sku, product.Images, product.IsDownload, product.AllowBackorder, String.Empty, DiscountType.None, 0));
			}
			else
			{
				existingItem.UpdateCount(count);
			}

			// reset totals for summary
			ResetTotalItems(TotalItems + count);
			ResetTotalCost(base.SubTotal + (product.RetailPrice * count));
		}


		/// <summary>
		/// Updates the quantity for a specific product within the shopping cart.
		/// </summary>
		/// <param name="productId">Unique id of the product to be updated.</param>
		/// <param name="count">New quantity to be applied to the Product.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
		public void Update(int productId, int count)
		{
			ShoppingCartItem existingItem = Items.Find(e => e.Id == productId) ?? throw new ArgumentException("invalid product id", nameof(productId));
			existingItem.ResetCount(count);

			Reset();
		}

		/// <summary>
		/// Deletes a product from the shopping cart.
		/// </summary>
		/// <param name="productId">Unique id of the product to be deleted.</param>
		public void Delete(int productId)
		{
			ShoppingCartItem item = Items.Find(i => i.Id == productId);

			if (item != null)
			{
				ResetTotalItems(TotalItems - (int)item.ItemCount);
				ResetTotalCost(base.SubTotal - (item.ItemCost * item.ItemCount));
				Items.Remove(item);
			}
		}

		/// <summary>
		/// Resets the total cost and item count of all items within the cart.
		/// </summary>
		public void Reset()
		{
			ResetTotalItems((int)Items.Sum(s => s.ItemCount));
			ResetTotalCost(Items.Sum(s => s.CostWithDiscountApplied() * s.ItemCount));
		}

		/// <summary>
		/// Clears any voucher data associated with the shopping cart
		/// </summary>
		public void ClearVoucherData()
		{
			DiscountRate = 0;
			DiscountType = DiscountType.None;
			Reset();
		}

		/// <summary>
		/// Sets the delivery address for the shopping cart, this is typically completed during the checkout phase.
		/// </summary>
		/// <param name="address">Address the user wants the cart shipping to.</param>
		public void SetDeliveryAddress(in DeliveryAddress address)
		{
			if (address == null)
				throw new ArgumentNullException(nameof(address));

			DeliveryAddressId = address.Id;
			ResetShipping(address.PostageCost);
		}

		/// <summary>
		/// Clears and resets shopping cart
		/// </summary>
		public void Clear()
		{
			Items.Clear();
			ResetShipping(0);
			Reset();
		}

		/// <summary>
		/// Updates the discount for a shopping cart
		/// </summary>
		/// <param name="couponCode"></param>
		/// <param name="discountType"></param>
		/// <param name="discount"></param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public void UpdateDiscount(string couponCode, DiscountType discountType, decimal discount)
		{
			if (String.IsNullOrEmpty(couponCode))
				throw new ArgumentNullException(nameof(couponCode));

			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(discount);

			CouponCode = couponCode;
			DiscountRate = discount;
			DiscountType = discountType;
			Reset();
		}

		#endregion Public Methods
	}
}
