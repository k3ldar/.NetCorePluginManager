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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: Invoice.cs
 *
 *  Purpose:  Contains Order Details
 *
 *  Date        Name                Reason
 *  04/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Middleware.Accounts.Invoices
{
    public sealed class Invoice
    {
        #region Constructors

        public Invoice(in int id, in DateTime date, in decimal postage, in CultureInfo culture,
            in ProcessStatus processStatus, DeliveryAddress deliveryAddress,
            List<InvoiceItem> invoiceItems)
        {
            if (postage < 0)
                throw new ArgumentOutOfRangeException(nameof(postage));

            Id = id;
            Date = date;
            Postage = postage;
            Culture = culture;
            Status = processStatus;
            InvoiceItems = invoiceItems ?? throw new ArgumentNullException(nameof(invoiceItems));
            DeliveryAddress = deliveryAddress;

            foreach (InvoiceItem item in InvoiceItems)
                item.Invoice = this;
        }

        #endregion Constructors

        #region Properties

        public int Id { get; private set; }

        public DateTime Date { get; private set; }

        public DeliveryAddress DeliveryAddress { get; private set; }

        public decimal Postage { get; private set; }

        public CultureInfo Culture { get; private set; }

        public ProcessStatus Status { get; private set; }

        public List<InvoiceItem> InvoiceItems { get; private set; }

        /// <summary>
        /// Total discount amount
        /// </summary>
        public decimal Discount
        {
            get
            {
                decimal Result = 0;

                foreach (InvoiceItem item in InvoiceItems)
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

                foreach (InvoiceItem item in InvoiceItems)
                    Result += item.Cost;

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

                foreach (InvoiceItem item in InvoiceItems)
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

                foreach (InvoiceItem item in InvoiceItems)
                    Result += item.Cost;

                return SubTotal + Postage;
            }
        }

        public decimal ItemCount
        {
            get
            {
                decimal Result = 0;

                foreach (InvoiceItem item in InvoiceItems)
                    Result += item.Quantity;

                return Result;
            }
        }

        #endregion Properties
    }
}
