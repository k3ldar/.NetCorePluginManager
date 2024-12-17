/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
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

using SharedPluginFeatures;

using static Shared.Utilities;

#pragma warning disable IDE0047

namespace Middleware.Accounts.Orders
{
	/// <summary>
	/// Represents an item within an Order created by a user.  This is primarily used by IAccountProvider and the UserAccount.Plugin module.
	/// </summary>
	public sealed class OrderItem
	{
		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id">Unique id of order item.</param>
		/// <param name="description">Description of item.</param>
		/// <param name="cost">Price of the item.</param>
		/// <param name="taxRate">Rate of tax applied to the item within an order.</param>
		/// <param name="quantity">Quantity of items within an order.</param>
		/// <param name="status">Current status of item within the order.</param>
		/// <param name="discountType">Type of discount applied to the item.</param>
		/// <param name="discount">Discount amount applied to the item.</param>
		public OrderItem(in long id, in string description, in decimal cost,
			in decimal taxRate, in decimal quantity, in ItemStatus status, in DiscountType discountType,
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
		/// Unique id of order item.
		/// </summary>
		/// <value>long</value>
		public long Id { get; private set; }

		/// <summary>
		/// Order the item belongs to.
		/// </summary>
		/// <value>Order</value>
		public Order Order { get; internal set; }

		/// <summary>
		/// Description of item.
		/// </summary>
		/// <value>string</value>
		public string Description { get; private set; }

		/// <summary>
		/// Price of the item.
		/// </summary>
		/// <value>decimal</value>
		public decimal Price { get; private set; }

		/// <summary>
		/// Rate of tax applied to the item within an order.
		/// </summary>
		/// <value>int</value>
		public decimal TaxRate { get; private set; }

		/// <summary>
		/// Quantity of items within an order.
		/// </summary>
		/// <value>decimal</value>
		public decimal Quantity { get; private set; }

		/// <summary>
		/// Discount amount applied to the item.
		/// </summary>
		/// <value>decimal</value>
		public decimal Discount { get; private set; }

		/// <summary>
		/// Type of discount applied to the item.
		/// </summary>
		/// <value>DiscountType</value>
		public DiscountType DiscountType { get; private set; }

		/// <summary>
		/// Current status of item within the order.
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
				decimal taxRate = 1 + (TaxRate / 100);
				decimal totalCost = BankersRounding(Price * Quantity, Order.Culture.NumberFormat.NumberDecimalDigits);

				return BankersRounding(taxRate * totalCost, Order.Culture.NumberFormat.NumberDecimalDigits) - TotalDiscount;
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
				decimal taxRate = 1 + (TaxRate / 100);
				decimal totalCost = BankersRounding(Price * Quantity, Order.Culture.NumberFormat.NumberDecimalDigits);

				return BankersRounding(taxRate * totalCost, Order.Culture.NumberFormat.NumberDecimalDigits) - totalCost;
			}
		}

		/// <summary>
		/// Sub total value for the item.
		/// </summary>
		/// <value>decimal</value>
		public decimal SubTotal
		{
			get
			{
				return BankersRounding(Price * Quantity, Order.Culture.NumberFormat.NumberDecimalDigits);
			}
		}

		/// <summary>
		/// Total discount for the item within the Order.
		/// </summary>
		/// <value>decimal</value>
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
					DiscountType.PercentageSubTotal => BankersRounding((SubTotal / 100) * Discount, Order.Culture.NumberFormat.NumberDecimalDigits),
					DiscountType.PercentageTotal => BankersRounding(((SubTotal + TotalTax) / 100) * Discount, Order.Culture.NumberFormat.NumberDecimalDigits),
					DiscountType.Value => BankersRounding(Discount, Order.Culture.NumberFormat.NumberDecimalDigits),
					_ => throw new InvalidOperationException("Invalid Discount Type"),
				};
			}
		}

		#endregion Properties
	}
}

#pragma warning restore IDE0047