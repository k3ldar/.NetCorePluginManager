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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
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
using System.Text;
using System.Globalization;

namespace Middleware.Orders
{
    public sealed class Order
    {
        #region Constructors

        public Order(in int id, in DateTime date, in decimal postage, in CultureInfo culture, 
            in ProcessStatus processStatus, List<OrderItem> orderItems)
        {
            if (postage < 0)
                throw new ArgumentOutOfRangeException(nameof(postage));

            Date = date;
            Postage = postage;
            Culture = culture;
            Status = processStatus;
            OrderItems = orderItems ?? throw new ArgumentNullException(nameof(orderItems));
        }

        #endregion Constructors

        #region Properties

        public int Id { get; private set; }

        public DateTime Date { get; private set; }

        public decimal Postage { get; private set; }

        public CultureInfo Culture { get; private set; }

        public ProcessStatus Status { get; private set; }

        public List<OrderItem> OrderItems { get; private set; }

        /// <summary>
        /// Total discount amount
        /// </summary>
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
        public decimal SubTotal
        {
            get
            {
                decimal Result = 0;

                foreach (OrderItem item in OrderItems)
                    Result += item.SubTotal;

                return Result;
            }
        }

        /// <summary>
        /// Total Tax amount
        /// </summary>
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
        public decimal Total
        {
            get
            {
                decimal Result = 0;

                foreach (OrderItem item in OrderItems)
                    Result += item.Total;

                return Result;

            }
        }

        #endregion Properties
    }
}
