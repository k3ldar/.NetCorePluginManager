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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  UserAccount.Plugin
 *  
 *  File: InvoiceViewModel.cs
 *
 *  Purpose:  Invoice view model
 *
 *  Date        Name                Reason
 *  04/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

using Middleware;
using Middleware.Accounts.Invoices;

using SharedPluginFeatures;

namespace UserAccount.Plugin.Models
{
#pragma warning disable CS1591

    public class InvoiceViewModel : BaseModel
    {
        #region Constructors

        public InvoiceViewModel(in BaseModelData baseModelData, Invoice invoice)
            : base(baseModelData)
        {
            if (invoice == null)
                throw new ArgumentNullException(nameof(invoice));

            InvoiceId = invoice.Id;
            Culture = invoice.Culture ?? throw new ArgumentNullException(nameof(invoice.Culture));
            Date = invoice.Date;
            SubTotal = invoice.SubTotal;
            Postage = invoice.Postage;
            Tax = invoice.Tax;
            Total = invoice.Total;
            Status = invoice.Status;
            InvoiceItems = invoice.InvoiceItems ?? throw new ArgumentNullException(nameof(invoice.InvoiceItems));
            DeliveryAddress = String.Empty;

            if (invoice.DeliveryAddress != null)
            {
                DeliveryAddress += String.IsNullOrEmpty(invoice.DeliveryAddress.BusinessName) ?
                    String.Empty : invoice.DeliveryAddress.BusinessName + "<br />";
                DeliveryAddress += String.IsNullOrEmpty(invoice.DeliveryAddress.AddressLine1) ?
                    String.Empty : invoice.DeliveryAddress.AddressLine1 + "<br />";
                DeliveryAddress += String.IsNullOrEmpty(invoice.DeliveryAddress.AddressLine2) ?
                    String.Empty : invoice.DeliveryAddress.AddressLine2 + "<br />";
                DeliveryAddress += String.IsNullOrEmpty(invoice.DeliveryAddress.AddressLine3) ?
                    String.Empty : invoice.DeliveryAddress.AddressLine3 + "<br />";
                DeliveryAddress += String.IsNullOrEmpty(invoice.DeliveryAddress.City) ?
                    String.Empty : invoice.DeliveryAddress.City + "<br />";
                DeliveryAddress += String.IsNullOrEmpty(invoice.DeliveryAddress.County) ?
                    String.Empty : invoice.DeliveryAddress.County + "<br />";
                DeliveryAddress += String.IsNullOrEmpty(invoice.DeliveryAddress.Country) ?
                    String.Empty : invoice.DeliveryAddress.Country + "<br />";
            }
        }

        #endregion Constructors

        #region Properties

        public int InvoiceId { get; private set; }

        public CultureInfo Culture { get; private set; }

        public string DeliveryAddress { get; private set; }

        [Display(Name = nameof(Languages.LanguageStrings.InvoiceDate))]
        public DateTime Date { get; private set; }

        public decimal SubTotal { get; private set; }

        public decimal Postage { get; private set; }

        public decimal Tax { get; private set; }

        public decimal Total { get; private set; }

        public ProcessStatus Status { get; private set; }

        public List<InvoiceItem> InvoiceItems { get; private set; }

        #endregion Properties
    }

#pragma warning restore CS1591
}
