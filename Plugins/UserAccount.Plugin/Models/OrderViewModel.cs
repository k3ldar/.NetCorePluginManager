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
 *  Product:  UserAccount.Plugin
 *  
 *  File: OrderViewModel.cs
 *
 *  Purpose:  Orders view model
 *
 *  Date        Name                Reason
 *  31/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json.Serialization;

using Middleware;
using Middleware.Accounts.Orders;

using SharedPluginFeatures;

namespace UserAccount.Plugin.Models
{
#pragma warning disable CS1591

    public sealed class OrderViewModel : BaseModel
    {
        #region Constructors

        public OrderViewModel(in BaseModelData baseModelData, Order order)
            : base(baseModelData)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            OrderId = order.Id;
            Culture = order.Culture ?? throw new ArgumentOutOfRangeException(nameof(order.Culture));
            Date = order.Date;
            SubTotal = order.SubTotal;
            Postage = order.Postage;
            Tax = order.Tax;
            Total = order.Total;
            Status = order.Status;
            OrderItems = order.OrderItems ?? throw new ArgumentNullException(nameof(order.OrderItems));
            DeliveryAddress = String.Empty;

            if (order.DeliveryAddress != null)
            {
                DeliveryAddress += String.IsNullOrEmpty(order.DeliveryAddress.BusinessName) ?
                    String.Empty : order.DeliveryAddress.BusinessName + "<br />";
                DeliveryAddress += String.IsNullOrEmpty(order.DeliveryAddress.AddressLine1) ?
                    String.Empty : order.DeliveryAddress.AddressLine1 + "<br />";
                DeliveryAddress += String.IsNullOrEmpty(order.DeliveryAddress.AddressLine2) ?
                    String.Empty : order.DeliveryAddress.AddressLine2 + "<br />";
                DeliveryAddress += String.IsNullOrEmpty(order.DeliveryAddress.AddressLine3) ?
                    String.Empty : order.DeliveryAddress.AddressLine3 + "<br />";
                DeliveryAddress += String.IsNullOrEmpty(order.DeliveryAddress.City) ?
                    String.Empty : order.DeliveryAddress.City + "<br />";
                DeliveryAddress += String.IsNullOrEmpty(order.DeliveryAddress.County) ?
                    String.Empty : order.DeliveryAddress.County + "<br />";
                DeliveryAddress += String.IsNullOrEmpty(order.DeliveryAddress.Country) ?
                    String.Empty : order.DeliveryAddress.Country + "<br />";
            }
        }

		#endregion Constructors

		#region Properties

		[JsonRequired]
		public long OrderId { get; private set; }

        public CultureInfo Culture { get; private set; }

        public string DeliveryAddress { get; private set; }

        [Display(Name = nameof(Languages.LanguageStrings.OrderDate))]
        public DateTime Date { get; private set; }

        public decimal SubTotal { get; private set; }

        public decimal Postage { get; private set; }

        public decimal Tax { get; private set; }

        public decimal Total { get; private set; }

        public ProcessStatus Status { get; private set; }

        public List<OrderItem> OrderItems { get; private set; }

        #endregion Properties
    }

#pragma warning restore CS1591
}
