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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Demo Website
 *  
 *  File: MockAccountProvider.cs
 *
 *  Purpose:  Mock IAccountProvider for tesing purpose
 *
 *  Date        Name                Reason
 *  09/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Middleware;
using Middleware.Accounts;
using Middleware.Accounts.Invoices;
using Middleware.Accounts.Orders;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
    [ExcludeFromCodeCoverage(Justification = "Code coverage not required for mock classes")]
    public class MockAccountProvider : IAccountProvider
    {
        #region Private Static Members

        private static List<DeliveryAddress> _deliveryAddresses;

        private static Marketing _marketing;

        private static List<Order> _orders;

        private static List<Invoice> _invoices;

        private static int InvoiceItemId = 50;

        private static int InvoiceId = 50;

        #endregion Private Static Members

        #region Change Password

        public bool ChangePassword(in long userId, in string newPassword)
        {
            if (String.IsNullOrEmpty(newPassword))
                throw new ArgumentNullException(nameof(newPassword));

            return newPassword.Equals("Pa$$w0rd");
        }

        #endregion Change Password

        #region Address Options

        public AddressOptions GetAddressOptions(in AddressOption addressOption)
        {
            return AddressOptions.AddressLine1Mandatory | AddressOptions.AddressLine1Show |
                AddressOptions.AddressLine2Show |
                AddressOptions.CityMandatory | AddressOptions.CityShow |
                AddressOptions.PostCodeMandatory | AddressOptions.PostCodeShow |
                AddressOptions.TelephoneShow;
        }

        #endregion Address Options

        #region User Contact Details

        public bool GetUserAccountDetails(in Int64 userId, out string firstName, out string lastName, out string email, out bool emailConfirmed,
            out string telephone, out bool telephoneConfirmed)
        {
            firstName = "Fred";
            lastName = "Bloggs";
            email = "fred@bloggs.com";
            emailConfirmed = true;
            telephone = "0044 121 345 6789";
            telephoneConfirmed = false;

            return true;
        }

        public bool SetUserAccountDetails(in Int64 userId, in string firstName, in string lastName, in string email, in string telephone)
        {
            return firstName == "Fred" && lastName == "Bloggs";
        }

        public bool ConfirmEmailAddress(in Int64 userId, in string confirmationCode)
        {
            if (String.IsNullOrEmpty(confirmationCode))
                throw new ArgumentNullException(nameof(confirmationCode));

            return confirmationCode.Equals("NewEmail");
        }

        public bool ConfirmTelephoneNumber(in Int64 userId, in string confirmationCode)
        {
            if (String.IsNullOrEmpty(confirmationCode))
                throw new ArgumentNullException(nameof(confirmationCode));

            return confirmationCode.Equals("NewTelephone");
        }

        #endregion User Contact Details

        #region Account Creation/Deletion

        public bool CreateAccount(in string email, in string firstName, in string surname, in string password,
            in string telephone, in string businessName, in string addressLine1, in string addressLine2,
            in string addressLine3, in string city, in string county, in string postcode, in string countryCode,
            out long userId)
        {
            userId = 2;


            return true;
        }

        /// <summary>
        /// Delete's a user account
        /// </summary>
        /// <param name="userId">Unique user id for the new user account.</param>
        /// <returns>bool.  True if the account was deleted, otherwise false.</returns>
        public bool DeleteAccount(in Int64 userId)
        {
            return true;
        }

        #endregion Account Creation/Deletion

        #region Account Lock/Unlock

        /// <summary>
        /// Locks a user account, preventing access to the system
        /// </summary>
        /// <param name="userId">Unique user id for the new user account.</param>
        /// <returns>bool.  True if the account was locked, otherwise false.</returns>
        public bool AccountLock(in Int64 userId)
        {
            return true;
        }

        /// <summary>
        /// Unlocks a user account enabling access to the system
        /// </summary>
        /// <param name="userId">Unique user id for the new user account.</param>
        /// <returns>bool.  True if the account was unlocked, otherwise false.</returns>
        public bool AccountUnlock(in Int64 userId)
        {
            return true;
        }

        #endregion Account Lock/Unlock

        #region Billing Address

        public bool SetBillingAddress(in long userId, in Address billingAddress)
        {
            if (billingAddress == null)
                throw new ArgumentNullException(nameof(billingAddress));

            return billingAddress.AddressLine1 == "Mike St";
        }

        public Address GetBillingAddress(in long userId)
        {
            return new Address(1, String.Empty, "Mike St", String.Empty, String.Empty, "London", String.Empty, "L1 1AA", "GB");
        }

        #endregion Billing Address

        #region Delivery Address

        public bool SetDeliveryAddress(in long userId, in DeliveryAddress deliveryAddress)
        {
            return true;
        }

        public List<DeliveryAddress> GetDeliveryAddresses(in long userId)
        {
            if (_deliveryAddresses == null)
                _deliveryAddresses = new List<DeliveryAddress>()
                    {
                        new DeliveryAddress(1, String.Empty, "1 Mike St", String.Empty, String.Empty, "London", String.Empty, "L1 1AA", "GB", 5.99m),
                        new DeliveryAddress(2, String.Empty, "29 5th Avenue", String.Empty, String.Empty, "New York", String.Empty, "49210", "US", 5.99m),
                    };

            return _deliveryAddresses;
        }

        public bool AddDeliveryAddress(in Int64 userId, in DeliveryAddress deliveryAddress)
        {
            _deliveryAddresses.Add(deliveryAddress);
            return true;
        }

        public DeliveryAddress GetDeliveryAddress(in Int64 userId, in long deliveryAddressId)
        {
            foreach (DeliveryAddress address in GetDeliveryAddresses(userId))
            {
                if (address.Id == deliveryAddressId)
                    return address;
            }

            return null;
        }

        public bool DeleteDeliveryAddress(in long userId, in DeliveryAddress deliveryAddress)
        {
            if (deliveryAddress == null || deliveryAddress.Id == 1)
                return false;

            _deliveryAddresses.Remove(deliveryAddress);

            return true;
        }

        #endregion Delivery Address

        #region Marketing Preferences

        public MarketingOptions GetMarketingOptions()
        {
            return MarketingOptions.ShowEmail |
                MarketingOptions.ShowPostal |
                MarketingOptions.ShowSMS |
                MarketingOptions.ShowTelephone;
        }

        public Marketing GetMarketingPreferences(in Int64 userId)
        {
            if (_marketing == null)
                _marketing = new Marketing(true, true, false, false);

            return _marketing;
        }

        public bool SetMarketingPreferences(in Int64 userId, in Marketing marketing)
        {
            _marketing = marketing;
            return true;
        }

        #endregion Marketing Preferences

        #region Orders

        public List<Order> OrdersGet(in Int64 userId)
        {
            if (_orders == null)
            {
                _orders = new List<Order>()
                {
                    new Order(1, DateTime.Now.AddDays(-10), 4.99m, new CultureInfo("en-US"), ProcessStatus.Dispatched,
                        GetDeliveryAddresses(userId)[0], new List<OrderItem>()
                        {
                            new OrderItem(1, "The shining ones by David Eddings", 14.99m, 20, 1m, ItemStatus.Dispatched, DiscountType.Value, 0),
                            new OrderItem(2, "Domes of Fire by David Eddings", 12.99m, 20, 2m, ItemStatus.BackOrder, DiscountType.PercentageSubTotal, 10),
                            new OrderItem(3, "The hidden city by David Eddings", 12.99m, 20, 1m, ItemStatus.OnHold, DiscountType.PercentageTotal, 10),
                            new OrderItem(4, "Bookmark", 0.99m, 20, 1m, ItemStatus.Dispatched, DiscountType.None, 0)
                        }),

                    new Order(2, DateTime.Now.AddDays(-8), 6.99m, new CultureInfo("en-GB"), ProcessStatus.Dispatched,
                        GetDeliveryAddresses(userId)[0], new List<OrderItem>()
                        {
                            new OrderItem(5, "Mug, shiny white", 6.99m, 20, 6m, ItemStatus.Dispatched, DiscountType.Value, 15),
                            new OrderItem(6, "Dinner Plate", 7.99m, 20, 6m, ItemStatus.Dispatched, DiscountType.PercentageSubTotal, 10),
                            new OrderItem(7, "Cereal bowl", 5.99m, 20, 6m, ItemStatus.Dispatched, DiscountType.PercentageTotal, 10)
                        })
                };
            }

            return _orders;
        }

        public void OrderPaid(in Order order, in PaymentStatus paymentStatus, in string message)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (String.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            List<InvoiceItem> items = new List<InvoiceItem>();

            foreach (OrderItem item in order.OrderItems)
            {
                items.Add(new InvoiceItem(++InvoiceItemId, item.Description, item.Cost, item.TaxRate,
                    item.Quantity, ItemStatus.Received, item.DiscountType, item.Discount));
            }

            Invoice invoice = new Invoice(++InvoiceId, DateTime.Now, order.Postage, order.Culture,
                ProcessStatus.PaymentCheck, paymentStatus, order.DeliveryAddress, items);
        }

        #endregion Orders

        #region Invoices

        public List<Invoice> InvoicesGet(in Int64 userId)
        {
            if (_invoices == null)
            {
                _invoices = new List<Invoice>()
                {
                    new Invoice(123, DateTime.Now.AddDays(-10), 4.99m, new CultureInfo("en-US"), ProcessStatus.Dispatched,
                        PaymentStatus.PaidMixed, GetDeliveryAddresses(userId)[0], new List<InvoiceItem>()
                        {
                            new InvoiceItem(1, "The shining ones by David Eddings", 14.99m, 20, 1m, ItemStatus.Dispatched, DiscountType.Value, 0),
                            new InvoiceItem(4, "Bookmark", 0.99m, 20, 1m, ItemStatus.Dispatched, DiscountType.None, 0)
                        }),

                    new Invoice(234, DateTime.Now.AddDays(-8), 6.99m, new CultureInfo("en-GB"), ProcessStatus.Dispatched,
                        PaymentStatus.PaidCash, GetDeliveryAddresses(userId)[0], new List<InvoiceItem>()
                        {
                            new InvoiceItem(5, "Mug, shiny white", 6.99m, 20, 6m, ItemStatus.Dispatched, DiscountType.Value, 15),
                            new InvoiceItem(6, "Dinner Plate", 7.99m, 20, 6m, ItemStatus.Dispatched, DiscountType.PercentageSubTotal, 10),
                            new InvoiceItem(7, "Cereal bowl", 5.99m, 20, 6m, ItemStatus.Dispatched, DiscountType.PercentageTotal, 10)
                        })
                };
            }

            return _invoices;
        }

        #endregion Invoices
    }
}
