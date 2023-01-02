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
    /// <summary>
    /// Represents an invoice that is used for display within the website.  This is used by IAccountProvider and the UserAccount.Plugin module.
    /// </summary>
    public sealed class Invoice
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Unique invoice id.</param>
        /// <param name="date">Date and time the invoice was created.</param>
        /// <param name="postage">The cost of postage for the invoice.</param>
        /// <param name="culture">Invoice culture</param>
        /// <param name="processStatus">Current process status for the invoice.</param>
        /// <param name="paymentStatus">Current payment status for the invoice.</param>
        /// <param name="deliveryAddress">The address where the invoice has been dispatched to.</param>
        /// <param name="invoiceItems">List of all invoice items.</param>
        public Invoice(in long id, in DateTime date, in decimal postage, in CultureInfo culture,
            in ProcessStatus processStatus, in PaymentStatus paymentStatus,
            DeliveryAddress deliveryAddress, List<InvoiceItem> invoiceItems)
        {
            if (postage < 0)
                throw new ArgumentOutOfRangeException(nameof(postage));

            Id = id;
            Date = date;
            Postage = postage;
            Culture = culture;
            Status = processStatus;
            PaymentStatus = paymentStatus;
            InvoiceItems = invoiceItems ?? throw new ArgumentNullException(nameof(invoiceItems));
            DeliveryAddress = deliveryAddress;

            foreach (InvoiceItem item in InvoiceItems)
                item.Invoice = this;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Unique invoice id.
        /// </summary>
        /// <value>int</value>
        public long Id { get; private set; }

        /// <summary>
        /// Date and time the invoice was created.
        /// </summary>
        /// <value>DateTime</value>
        public DateTime Date { get; private set; }

        /// <summary>
        /// The address where the invoice has been dispatched to.
        /// </summary>
        /// <value>DeliveryAddress</value>
        public DeliveryAddress DeliveryAddress { get; private set; }

        /// <summary>
        /// The cost of postage for the invoice.
        /// </summary>
        /// <value>decimal</value>
        public decimal Postage { get; private set; }

        /// <summary>
        /// Invoice culture.
        /// </summary>
        /// <value>CultureInfo</value>
        public CultureInfo Culture { get; private set; }

        /// <summary>
        /// Current process status for the invoice.
        /// </summary>
        /// <value>ProcessStatus</value>
        public ProcessStatus Status { get; private set; }

        /// <summary>
        /// Current payment status for the invoice.
        /// </summary>
        /// <value>PaymentStatus</value>
        public PaymentStatus PaymentStatus { get; private set; }

        /// <summary>
        /// List of all invoice items.
        /// </summary>
        /// <value>List&lt;InvoiceItem&gt;</value>
        public List<InvoiceItem> InvoiceItems { get; private set; }

        /// <summary>
        /// Total discount amount
        /// </summary>
        /// <value>decimal</value>
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
        /// <value>decimal</value>
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
        /// <value>decimal</value>
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
        /// <value>decimal</value>
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

        /// <summary>
        /// Total number of items within the invoice.
        /// </summary>
        /// <value>decimal</value>
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
