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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: Order.cs
 *
 *  Purpose:  Contains Order Details
 *
 *  Date        Name                Reason
 *  31/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Middleware.Accounts.Orders
{
	/// <summary>
	/// Represents a users order, primarily used by IAccountProvider within the UserAccount.Plugin module.
	/// </summary>
	public sealed class Order
	{
		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id">Unique id for the order.</param>
		/// <param name="date">Date and time the order was created.</param>
		/// <param name="postage">Cost of postage for the order.</param>
		/// <param name="culture">Culture used for the order.</param>
		/// <param name="processStatus">Current process status for the order.</param>
		/// <param name="deliveryAddress">Delivery address where the order will be dispatched to.</param>
		/// <param name="orderItems">List of items within the order.</param>
		public Order(in long id, in DateTime date, in decimal postage, in CultureInfo culture,
			in ProcessStatus processStatus, DeliveryAddress deliveryAddress,
			List<OrderItem> orderItems)
		{
			if (postage < 0)
				throw new ArgumentOutOfRangeException(nameof(postage));

			Id = id;
			Date = date;
			Postage = postage;
			Culture = culture;
			Status = processStatus;
			OrderItems = orderItems ?? throw new ArgumentNullException(nameof(orderItems));
			DeliveryAddress = deliveryAddress;

			foreach (OrderItem item in OrderItems)
				item.Order = this;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Unique id for the order.
		/// </summary>
		/// <value>int</value>
		public long Id { get; private set; }

		/// <summary>
		/// Date and time the order was created.
		/// </summary>
		/// <value>DateTime</value>
		public DateTime Date { get; private set; }

		/// <summary>
		/// Delivery address where the order will be dispatched to.
		/// </summary>
		/// <value>DeliveryAddress</value>
		public DeliveryAddress DeliveryAddress { get; private set; }

		/// <summary>
		/// Cost of postage for the order.
		/// </summary>
		/// <value>decimal</value>
		public decimal Postage { get; private set; }

		/// <summary>
		/// Culture used for the order.
		/// </summary>
		/// <value>CultureInfo</value>
		public CultureInfo Culture { get; private set; }

		/// <summary>
		/// Current process status for the order.
		/// </summary>
		/// <value>ProcessStatus</value>
		public ProcessStatus Status { get; private set; }

		/// <summary>
		/// List of items within the order.
		/// </summary>
		/// <value>List&lt;OrderItem&gt;</value>
		public List<OrderItem> OrderItems { get; private set; }

		/// <summary>
		/// Total discount amount
		/// </summary>
		/// <value>decimal</value>
		public decimal Discount
		{
			get
			{
				decimal Result = 0;

				foreach (OrderItem item in OrderItems)
					Result += item.TotalDiscount;

				return Result;
			}
		}

		/// <summary>
		/// Total without tax
		/// </summary>
		/// <value>decimal</value>
		public decimal SubTotal
		{
			get
			{
				decimal Result = 0;

				foreach (OrderItem item in OrderItems)
					Result += item.Cost;

				return Result;
			}
		}

		/// <summary>
		/// Total Tax amount
		/// </summary>
		/// <value>decimal</value>
		public decimal Tax
		{
			get
			{
				decimal Result = 0;

				foreach (OrderItem item in OrderItems)
					Result += item.TotalTax;

				return Result;
			}
		}

		/// <summary>
		/// Total cost + Tax
		/// </summary>
		/// <value>decimal</value>
		public decimal Total
		{
			get
			{
				decimal Result = 0;

				foreach (OrderItem item in OrderItems)
					Result += item.Cost;

				return SubTotal + Postage;
			}
		}

		/// <summary>
		/// Count of items within the order.
		/// </summary>
		/// <value>decimal</value>
		public decimal ItemCount
		{
			get
			{
				decimal Result = 0;

				foreach (OrderItem item in OrderItems)
					Result += item.Quantity;

				return Result;
			}
		}

		#endregion Properties
	}
}
