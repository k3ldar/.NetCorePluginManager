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
 *  File: ShoppingCartItem.cs
 *
 *  Purpose:  User Account provider
 *
 *  Date        Name                Reason
 *  07/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

namespace Middleware.ShoppingCart
{
	/// <summary>
	/// Provides details shopping cart item data.
	/// </summary>
	public sealed class ShoppingCartItem
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		private ShoppingCartItem()
		{
			Weight = 0;
			TaxRate = 0;
			CustomerReference = String.Empty;
			Size = String.Empty;
			DiscountRate = 0;
			DiscountType = DiscountType.None;
		}

		/// <summary>
		/// Constructor containing detailed items information.
		/// </summary>
		/// <param name="id">Unique id of shopping cart.</param>
		/// <param name="itemCount">Number of items in the cart.</param>
		/// <param name="itemId"></param>
		/// <param name="itemCost">Cost of the items within shopping cart.</param>
		/// <param name="name">Name of the item within the shopping cart.</param>
		/// <param name="description">Description of item within the cart</param>
		/// <param name="sku">SKU of item.</param>
		/// <param name="images">Images associated with the item.</param>
		/// <param name="isDownload">Indicates that the item is downloadable or not.</param>
		/// <param name="canBackOrder">Indicates that the item is on back order.</param>
		/// <param name="size">Size of item.</param>
		/// <param name="discountType">Type of discount applied to the item</param>
		/// <param name="discount">Discount amount</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Intended for developers not end users")]
		public ShoppingCartItem(in int id, in decimal itemCount, in long itemId, in decimal itemCost, in string name,
			in string description, in string sku, in string[] images, in bool isDownload,
			in bool canBackOrder, in string size, in DiscountType discountType, in decimal discount)
			: this()
		{
			if (String.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			if (String.IsNullOrEmpty(description))
				throw new ArgumentNullException(nameof(description));

			if (images == null)
				throw new ArgumentNullException(nameof(images));

			if (images.Length < 1)
				throw new ArgumentException("Value must be greater than zero", nameof(images));

			if (itemCount <= 0)
				throw new ArgumentOutOfRangeException(nameof(itemCount));

			if (itemCost < 0)
				throw new ArgumentOutOfRangeException(nameof(itemCost));

			Id = id;
			ItemId = itemId;
			ItemCount = itemCount;
			ItemCost = itemCost;
			Name = name;
			Description = description;
			SKU = sku ?? String.Empty;
			Images = images;
			IsDownload = isDownload;
			CanBackOrder = canBackOrder;
			Size = size ?? String.Empty;
			DiscountRate = discount;
			DiscountType = discountType;
		}

		/// <summary>
		/// Constructor containing all detailed item information.
		/// </summary>
		/// <param name="id">Unique id of shopping cart.</param>
		/// <param name="itemId">Id of the product item</param>
		/// <param name="itemCount">Number of items in the cart.</param>
		/// <param name="itemCost">Cost of the items within shopping cart.</param>
		/// <param name="taxRate">Tax rate applied to the item.</param>
		/// <param name="name">Name of the item within the shopping cart.</param>
		/// <param name="description">Description of item within the cart</param>
		/// <param name="sku">SKU of item.</param>
		/// <param name="images">Images associated with the item.</param>
		/// <param name="isDownload">Indicates that the item is downloadable or not.</param>
		/// <param name="weight">Weight of item in grams.</param>
		/// <param name="customerReference"></param>
		/// <param name="canBackOrder">Indicates that the item is on back order.</param>
		/// <param name="size">Size of item.</param>
		public ShoppingCartItem(in int id, in long itemId, in decimal itemCount, in decimal itemCost, in decimal taxRate,
			in string name, in string description, in string sku, in string[] images, in bool isDownload,
			in int weight, in string customerReference, in bool canBackOrder, in string size)
			: this(id, itemCount, itemId, itemCost, name, description, sku, images, isDownload, canBackOrder, size, DiscountType.None, 0)
		{
			if (taxRate < 0)
				throw new ArgumentOutOfRangeException(nameof(taxRate));

			if (weight < 0)
				throw new ArgumentOutOfRangeException(nameof(weight));

			TaxRate = taxRate;
			Weight = weight;
			CustomerReference = customerReference ?? String.Empty;
		}

		#endregion Constructors

		#region Public Methods

		/// <summary>
		/// Update the count of the individual item by adding more.
		/// </summary>
		/// <param name="count">Count to be added to existing count.</param>
		public void UpdateCount(in int count)
		{
			if (count < 1)
				throw new ArgumentOutOfRangeException(nameof(count));

			ItemCount += count;
		}

		/// <summary>
		/// Resets the total count to a new value
		/// </summary>
		/// <param name="count">Count of item within cart.</param>
		public void ResetCount(in int count)
		{
			if (count < 1)
				throw new ArgumentOutOfRangeException(nameof(count));

			ItemCount = count;
		}

		/// <summary>
		/// Sets the stock availability for an item within the shopping cart.
		/// </summary>
		/// <param name="stockavailability">Current stock availability for the item.</param>
		public void SetCurrentStockLevel(in uint stockavailability)
		{
			StockAvailability = stockavailability;
		}

		/// <summary>
		/// Retrieves the cost with any discounts applied
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public decimal CostWithDiscountApplied()
		{
			if (DiscountType == DiscountType.None || DiscountRate.Equals(0))
				return ItemCost;

			switch (DiscountType)
			{
				case DiscountType.Value:
					return ItemCost - DiscountRate;
				default:
					throw new InvalidOperationException("Invalid discount type");
			}
		}

		#endregion Public Methods

		#region Properties

		/// <summary>
		/// Unique id of the shopping cart item.
		/// </summary>
		/// <value>int</value>
		public int Id { get; private set; }

		/// <summary>
		/// Id of the product item
		/// </summary>
		/// <value>long</value>
		public long ItemId { get; private set; }

		/// <summary>
		/// Number of items.
		/// </summary>
		/// <value>decimal</value>
		public decimal ItemCount { get; private set; }

		/// <summary>
		/// Cost of the item.
		/// </summary>
		/// <value></value>
		public decimal ItemCost { get; private set; }

		/// <summary>
		/// Rate of tax applied to the item.
		/// </summary>
		/// <value>decimal</value>
		public decimal TaxRate { get; private set; }

		/// <summary>
		/// Name of the item.
		/// </summary>
		/// <string>string</string>
		public string Name { get; private set; }

		/// <summary>
		/// Description of the item.
		/// </summary>
		/// <value>string</value>
		public string Description { get; private set; }

		/// <summary>
		/// Item SKU
		/// </summary>
		/// <value>string</value>
		public string SKU { get; private set; }

		/// <summary>
		/// Images for the item
		/// </summary>
		/// <value>string[]</value>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "ok on this occasion")]
		public string[] Images { get; private set; }

		/// <summary>
		/// Indicates that the item is downloadable or not.
		/// </summary>
		/// <value>bool</value>
		public bool IsDownload { get; private set; }

		/// <summary>
		/// Weight of item in grams.
		/// </summary>
		/// <value>int</value>
		public int Weight { get; private set; }

		/// <summary>
		/// Customer reference for the item in the cart.
		/// </summary>
		/// <value>string</value>
		public string CustomerReference { get; private set; }

		/// <summary>
		/// Indicates that the item can be backordered.
		/// </summary>
		/// <value>bool</value>
		public bool CanBackOrder { get; private set; }

		/// <summary>
		/// Size or dimensions of the item.
		/// </summary>
		/// <value>string</value>
		public string Size { get; private set; }

		/// <summary>
		/// Availability of stock for the item in the shopping cart.
		/// </summary>
		public uint StockAvailability { get; private set; }

		/// <summary>
		/// Discount amount for specific product item
		/// </summary>
		public decimal DiscountRate { get; private set; }

		/// <summary>
		/// Type of discount
		/// </summary>
		public DiscountType DiscountType { get; private set; }

		/// <summary>
		/// Updates the shopping cart with a shopping cart, if it exists
		/// </summary>
		/// <param name="voucherCode"></param>
		/// <param name="discountType"></param>
		/// <param name="discountRate"></param>
		/// <param name="discountProductCount"></param>
		/// <exception cref="NotImplementedException"></exception>
		public void UpdateDiscountCode(string voucherCode, DiscountType discountType, decimal discountRate, int discountProductCount)
		{
			throw new NotImplementedException();
		}

		#endregion Properties
	}
}
