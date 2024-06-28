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
	/// <summary>
	/// Shopping cart summary, used if the website is implementing a shopping cart and data needs to be displayed on each page.
	/// </summary>
	public class ShoppingCartSummary
	{
		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id">Unique id for shopping cart.</param>
		/// <param name="totalItems">Total number of items in the shopping cart.</param>
		/// <param name="subTotal">Sub total value of the shopping cart.</param>
		/// <param name="discountRate">Discount applied to the shopping cart.</param>
		/// <param name="shipping">Shipping rate applied to the shopping cart.</param>
		/// <param name="taxRate">Tax rate applied to the shopping cart.</param>
		/// <param name="culture">Culture used by the shopping cart.</param>
		/// <param name="currencyCode">Currency code used by the shopping cart.</param>
		public ShoppingCartSummary(in long id, in int totalItems, in decimal subTotal,
			in decimal discountRate, in decimal shipping, in decimal taxRate,
			in CultureInfo culture, in string currencyCode)
		{
			Id = id;

			ArgumentOutOfRangeException.ThrowIfNegative(totalItems);

			ArgumentOutOfRangeException.ThrowIfNegative(subTotal);

			if (discountRate < 0 || discountRate > 100)
				throw new ArgumentOutOfRangeException(nameof(discountRate));

			if (String.IsNullOrEmpty(currencyCode) || currencyCode.Length != 3)
				throw new ArgumentOutOfRangeException(nameof(currencyCode));

			TotalItems = totalItems;
			SubTotal = subTotal;
			DiscountRate = discountRate;
			Shipping = shipping;
			TaxRate = taxRate;
			Culture = culture ?? throw new ArgumentNullException(nameof(culture));
			CurrencyCode = currencyCode;

			ResetTotalCost(subTotal);
		}

		#endregion Constructors

		#region Public Methods

		/// <summary>
		/// Resets the unique id associated with the shopping cart.
		/// </summary>
		/// <param name="id">Unique id for the cart.</param>
		public void ResetShoppingCartId(in long id)
		{
			if (Id != 0)
				throw new InvalidOperationException();

			Id = id;
		}

		#endregion Public Methods

		#region Protected Methods

		/// <summary>
		/// Resets the total number of items within the cart.
		/// </summary>
		/// <param name="totalItems">Total number of items within the cart.</param>
		protected void ResetTotalItems(in int totalItems)
		{
			if (totalItems < 0)
				throw new InvalidOperationException();

			TotalItems = totalItems;
		}

		/// <summary>
		/// Forces the shipping value to be reset within the shopping cart.
		/// </summary>
		/// <param name="shipping">Shipping charges which will apply.</param>
		protected void ResetShipping(in decimal shipping)
		{
			Shipping = shipping;
			ResetTotalCost(SubTotal);
		}

		/// <summary>
		/// Forces the total costs for the cart to be reset.
		/// </summary>
		/// <param name="cost">New cost to be applied to the shopping cart.</param>
		protected void ResetTotalCost(in decimal cost)
		{
			if (cost < 0)
				throw new InvalidOperationException();

			SubTotal = cost;

			decimal total = SubTotal + Shipping;

			if (DiscountRate > 0 && total > 0)
			{
				switch (DiscountType)
				{
					case DiscountType.None:
						Discount = 0;
						break;
					case DiscountType.PercentageTotal:
						Discount = Shared.Utilities.BankersRounding(total / 100 * DiscountRate, 2);
						break;
				}
			}

			Total = total - Discount;
			Tax = Shared.Utilities.BankersRounding(Shared.Utilities.VATCalculatePaid(Total, TaxRate), 2);
		}

		/// <summary>
		/// Forces the total costs for the cart to be reset.
		/// </summary>
		/// <param name="cost">New cost to be applied to the shopping cart.</param>
		/// <param name="cultureInfo">Culture to be applied to the shopping cart.</param>
		protected void ResetTotalCost(in decimal cost, in CultureInfo cultureInfo)
		{
			if (cultureInfo == null)
				throw new ArgumentNullException(nameof(cultureInfo));

			ResetTotalCost(cost);
		}

		#endregion Protected Methods

		#region Properties

		/// <summary>
		/// Unique id representing the shopping cart.
		/// </summary>
		public long Id { get; private set; }

		/// <summary>
		/// Total number of items within the shopping cart.
		/// </summary>
		public int TotalItems { get; private set; }

		/// <summary>
		/// Sub total of the shopping cart.
		/// </summary>
		public decimal SubTotal { get; private set; }

		/// <summary>
		/// Rate at which discount has been applied to the shopping cart.
		/// </summary>
		public decimal DiscountRate { get; protected set; }

		/// <summary>
		/// Type of discount applied to the shopping cart
		/// </summary>
		public DiscountType DiscountType { get; protected set; }

		/// <summary>
		/// Total discount value applied to the shopping cart.
		/// </summary>
		public decimal Discount { get; private set; }

		/// <summary>
		/// Rate at which tax has been applied to the shopping cart.
		/// </summary>
		public decimal TaxRate { get; private set; }

		/// <summary>
		/// Total tax applied to the shopping cart.
		/// </summary>
		public decimal Tax { get; private set; }

		/// <summary>
		/// Total cost of shipping for the cart.
		/// </summary>
		public decimal Shipping { get; private set; }

		/// <summary>
		/// Total value of the shopping cart.
		/// </summary>
		public decimal Total { get; private set; }

		/// <summary>
		/// The culture to be used for the shopping cart.
		/// </summary>
		public CultureInfo Culture { get; private set; }

		/// <summary>
		/// Three letter code depicting the currency used for the cart.
		/// </summary>
		public string CurrencyCode { get; private set; }

		#endregion Properties
	}
}
