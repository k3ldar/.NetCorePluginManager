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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: AccountProvider.cs
 *
 *  Purpose:  IAccountProvider for text based storage
 *
 *  Date        Name                Reason
 *  25/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Globalization;

using Middleware;
using Middleware.Accounts;
using Middleware.Accounts.Invoices;
using Middleware.Accounts.Orders;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Tables;
using PluginManager.SimpleDB;

using SharedPluginFeatures;

namespace PluginManager.DAL.TextFiles.Providers
{
    internal class AccountProvider : IAccountProvider
    {
        #region Private Members

        private readonly ITextTableOperations<UserDataRow> _users;
        private readonly ITextTableOperations<AddressDataRow> _addresses;
        private readonly ITextTableOperations<OrderDataRow> _orders;
        private readonly ITextTableOperations<OrderItemDataRow> _ordersItems;
        private readonly ITextTableOperations<InvoiceDataRow> _invoices;
        private readonly ITextTableOperations<InvoiceItemDataRow> _invoiceItems;
        private readonly string _encryptionKey;


        #endregion Private Members

        #region Constructors

        public AccountProvider(ITextTableOperations<UserDataRow> users, 
            ITextTableOperations<AddressDataRow> addresses, 
            ITextTableOperations<OrderDataRow> orders,
            ITextTableOperations<OrderItemDataRow> orderItems,
            ITextTableOperations<InvoiceDataRow> invoices,
            ITextTableOperations<InvoiceItemDataRow> invoiceItems,
            ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            TextFileSettings settings = settingsProvider.GetSettings<TextFileSettings>(nameof(TextFileSettings));

            if (settings == null)
                throw new InvalidOperationException();

            _encryptionKey = settings.EnycryptionKey;
            _users = users ?? throw new ArgumentNullException(nameof(users));
            _addresses = addresses ?? throw new ArgumentNullException(nameof(addresses));
            _orders = orders ?? throw new ArgumentNullException(nameof(orders));
            _ordersItems = orderItems ?? throw new ArgumentNullException(nameof(orderItems));
            _invoices = invoices ?? throw new ArgumentNullException(nameof(invoices));
            _invoiceItems = invoiceItems ?? throw new ArgumentNullException(nameof(invoiceItems));
        }

        #endregion Constructors

        #region Change Password

        public bool ChangePassword(in long userId, in string newPassword)
        {
            if (String.IsNullOrEmpty(newPassword))
                throw new ArgumentNullException(nameof(newPassword));

            UserDataRow user = _users.Select(userId);

            if (user == null)
                throw new ArgumentException("user not found", nameof(userId));

            user.Password = Shared.Utilities.Encrypt(newPassword, _encryptionKey);
            _users.Update(user);
            return true;
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
            UserDataRow user = _users.Select(userId);

            firstName = user?.FirstName;
            lastName = user?.Surname;
            email = user?.Email;
            emailConfirmed = user != null && user.EmailConfirmed;
            telephone = user?.Telephone;
            telephoneConfirmed = user != null && user.TelephoneConfirmed;

            return user != null;
        }

        public bool SetUserAccountDetails(in Int64 userId, in string firstName, in string lastName, in string email, in string telephone)
        {
            UserDataRow user = _users.Select(userId);

            if (user == null)
                return false;

            user.FirstName = firstName;
            user.Surname = lastName;
            user.Email = email;
            user.EmailConfirmed = false;
            user.EmailConfirmCode = GenerateRandomNumber().ToString();
            user.Telephone = telephone;
            user.TelephoneConfirmCode = GenerateRandomNumber().ToString();
            user.TelephoneConfirmed = false;

            _users.Update(user);
            return true;
        }

        public bool ConfirmEmailAddress(in Int64 userId, in string confirmationCode)
        {
            if (String.IsNullOrEmpty(confirmationCode))
                throw new ArgumentNullException(nameof(confirmationCode));

            UserDataRow user = _users.Select(userId);

            if (user == null)
                return false;

            user.EmailConfirmed = user.EmailConfirmCode.Equals(confirmationCode);

            if (user.EmailConfirmed)
            {
                user.EmailConfirmCode = String.Empty;
                _users.Update(user);
            }

            return user.EmailConfirmed;
        }

        public bool ConfirmTelephoneNumber(in Int64 userId, in string confirmationCode)
        {
            if (String.IsNullOrEmpty(confirmationCode))
                throw new ArgumentNullException(nameof(confirmationCode));

            UserDataRow user = _users.Select(userId);

            if (user == null)
                return false;

            user.TelephoneConfirmed = user.TelephoneConfirmCode.Equals(confirmationCode);

            if (user.TelephoneConfirmed)
            {
                user.TelephoneConfirmCode = String.Empty;
                _users.Update(user);
            }

            return user.TelephoneConfirmed;
        }

        #endregion User Contact Details

        #region Account Creation/Deletion

        public bool CreateAccount(in string email, in string firstName, in string surname, in string password,
            in string telephone, in string businessName, in string addressLine1, in string addressLine2,
            in string addressLine3, in string city, in string county, in string postcode, in string countryCode,
            out long userId)
        {
            UserDataRow newUser = new UserDataRow
            {
                Email = email,
                FirstName = firstName,
                Surname = surname,
                Password = Shared.Utilities.Encrypt(password, _encryptionKey),
                Telephone = telephone,
                BusinessName = businessName,
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                AddressLine3 = addressLine3,
                City = city,
                County = county,
                Postcode = postcode,
                CountryCode = countryCode,
                EmailConfirmCode = GenerateRandomNumber().ToString(),
                TelephoneConfirmCode = GenerateRandomNumber().ToString()
            };

            _users.Insert(newUser);

            userId = newUser.Id;
            return true;
        }

        /// <summary>
        /// Delete's a user account
        /// </summary>
        /// <param name="userId">Unique user id for the new user account.</param>
        /// <returns>bool.  True if the account was deleted, otherwise false.</returns>
        public bool DeleteAccount(in Int64 userId)
        {
            UserDataRow user = _users.Select(userId);

            if (user == null)
                return false;

            _users.Delete(user);
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
            UserDataRow user = _users.Select(userId);

            if (user == null)
                return false;

            if (user.Locked)
                return false;

            user.UnlockCode = Shared.Utilities.GetRandomPassword(8);
            _users.Update(user);

            return true;
        }

        /// <summary>
        /// Unlocks a user account enabling access to the system
        /// </summary>
        /// <param name="userId">Unique user id for the new user account.</param>
        /// <returns>bool.  True if the account was unlocked, otherwise false.</returns>
        public bool AccountUnlock(in Int64 userId)
        {
            UserDataRow user = _users.Select(userId);

            if (user == null)
                return false;

            if (!user.Locked)
                return false;

            user.UnlockCode = String.Empty;
            _users.Update(user);

            return true;
        }

        #endregion Account Lock/Unlock

        #region Billing Address

        public bool SetBillingAddress(in long userId, in Address billingAddress)
        {
            if (billingAddress == null)
                throw new ArgumentNullException(nameof(billingAddress));

            UserDataRow user = _users.Select(userId);

            if (user == null)
                return false;

            long billingAddressId = billingAddress.Id;

            AddressDataRow userAddresses = _addresses.Select().Where(a => a.UserId == user.Id && a.Id.Equals(billingAddressId)).FirstOrDefault();

            if (userAddresses == null)
            {
                userAddresses = new AddressDataRow()
                {
                    AddressLine1 = billingAddress.AddressLine1,
                    AddressLine2 = billingAddress.AddressLine2,
                    AddressLine3 = billingAddress.AddressLine3,
                    BusinessName = billingAddress.BusinessName,
                    City = billingAddress.City,
                    Country = billingAddress.Country,
                    County = billingAddress.County,
                    Postcode = billingAddress.Postcode,
                    IsDelivery = false,
                    UserId = userId,
                };

                _addresses.Insert(userAddresses);
            }
            else
            {
                userAddresses.AddressLine1 = billingAddress.AddressLine1;
                userAddresses.AddressLine2 = billingAddress.AddressLine2;
                userAddresses.AddressLine3 = billingAddress.AddressLine3;
                userAddresses.BusinessName = billingAddress.BusinessName;
                userAddresses.City = billingAddress.City;
                userAddresses.Country = billingAddress.Country;
                userAddresses.County = billingAddress.County;
                userAddresses.Postcode = billingAddress.Postcode;
                userAddresses.IsDelivery = false;
                userAddresses.UserId = userId;
                _addresses.Update(userAddresses);
            }

            return true;
        }

        public Address GetBillingAddress(in long userId)
        {
            UserDataRow user = _users.Select(userId);

            if (user == null)
                return null;

            AddressDataRow userAddresses = _addresses.Select().Where(a => a.UserId == user.Id && !a.IsDelivery).FirstOrDefault();

            return new Address(Convert.ToInt32(userAddresses.Id), userAddresses.BusinessName,
                userAddresses.AddressLine1, userAddresses.AddressLine2, userAddresses.AddressLine3,
                userAddresses.City, userAddresses.County, userAddresses.Postcode, userAddresses.Country);
        }

        #endregion Billing Address

        #region Delivery Address

        public bool SetDeliveryAddress(in long userId, in DeliveryAddress deliveryAddress)
        {
            UserDataRow user = _users.Select(userId);

            if (user == null)
                return false;

            AddressDataRow addressDataRow = _addresses.Select(deliveryAddress.Id);

            if (addressDataRow == null)
                return false;

            addressDataRow.BusinessName = deliveryAddress.BusinessName;
            addressDataRow.AddressLine1 = deliveryAddress.AddressLine1;
            addressDataRow.AddressLine2 = deliveryAddress.AddressLine2;
            addressDataRow.AddressLine3 = deliveryAddress.AddressLine3;
            addressDataRow.City = deliveryAddress.City;
            addressDataRow.County = deliveryAddress.County;
            addressDataRow.Postcode = deliveryAddress.Postcode;
            addressDataRow.Country = deliveryAddress.Country;
            addressDataRow.PostageCost = deliveryAddress.PostageCost;
            addressDataRow.IsDelivery = true;

            if (!addressDataRow.HasChanged)
                return false;

            _addresses.Update(addressDataRow);
            return true;
        }

        public List<DeliveryAddress> GetDeliveryAddresses(in long userId)
        {
            List<DeliveryAddress> Result = new List<DeliveryAddress>();
            UserDataRow user = _users.Select(userId);

            if (user == null)
                return Result;

            long userAddressId = userId;
            List<AddressDataRow> userAddresses = _addresses.Select().Where(a => a.UserId.Equals(userAddressId)).ToList();

            userAddresses.ForEach(ua => Result.Add(ConvertToDeliveryAddress(ua)));

            return Result;
        }

        public bool AddDeliveryAddress(in Int64 userId, in DeliveryAddress deliveryAddress)
        {
            UserDataRow user = _users.Select(userId);

            if (user == null)
                return false;

            AddressDataRow newAddress = new AddressDataRow()
            {
                AddressLine1 = deliveryAddress.AddressLine1,
                AddressLine2 = deliveryAddress.AddressLine2,
                AddressLine3 = deliveryAddress.AddressLine3,
                BusinessName = deliveryAddress.BusinessName,
                City = deliveryAddress.City,
                Country = deliveryAddress.Country,
                County = deliveryAddress.County,
                IsDelivery = true,
                PostageCost = deliveryAddress.PostageCost,
                Postcode = deliveryAddress.Postcode,
                UserId = userId,
            };

            _addresses.Insert(newAddress);

            deliveryAddress.Id = newAddress.Id;
            return true;
        }

        public DeliveryAddress GetDeliveryAddress(in Int64 userId, in long deliveryAddressId)
        {
            UserDataRow user = _users.Select(userId);

            if (user == null)
                return null;

            return ConvertToDeliveryAddress(_addresses.Select(deliveryAddressId));
        }

        public bool DeleteDeliveryAddress(in long userId, in DeliveryAddress deliveryAddress)
        {
            if (deliveryAddress == null)
                return false;

            UserDataRow user = _users.Select(userId);

            if (user == null)
                return false;

            AddressDataRow addressDataRow = _addresses.Select(deliveryAddress.Id);

            if (addressDataRow == null)
                return false;

            _addresses.Delete(addressDataRow);

            return !_addresses.IdExists(deliveryAddress.Id);
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
            UserDataRow user = _users.Select(userId);

            if (user == null)
                return null;

            return new Marketing(user.MarketingEmail, user.MarketingTelephone, user.MarketingSms, user.MarketingPostal);
        }

        public bool SetMarketingPreferences(in Int64 userId, in Marketing marketing)
        {
            UserDataRow user = _users.Select(userId);

            if (user == null)
                return false;

            user.MarketingEmail = marketing.EmailOffers;
            user.MarketingPostal = marketing.PostalOffers;
            user.MarketingSms = marketing.SMSOffers;
            user.MarketingTelephone = marketing.TelephoneOffers;

            _users.Update(user);
            return true;
        }

        #endregion Marketing Preferences

        #region Orders

        public List<Order> OrdersGet(in Int64 userId)
        {
            List<Order> Result = new List<Order>();

            UserDataRow user = _users.Select(userId);

            if (user == null)
                return Result;

            List<OrderDataRow> userOrders = _orders.Select().Where(o => o.UserId == user.Id).ToList();

            userOrders.ForEach(o =>
            {
                List<OrderItem> orderItems = new List<OrderItem>();

                List<OrderItemDataRow> userOrderItems = _ordersItems.Select().Where(oi => oi.OrderId.Equals(o.Id)).ToList();

                userOrderItems.ForEach(oi => orderItems.Add(new OrderItem(oi.Id, oi.Description, oi.Price, oi.TaxRate, oi.Quantity, (ItemStatus)oi.ItemStatus, (DiscountType)oi.DiscountType, oi.Discount)));

                Result.Add(new Order(o.Id, o.Created, o.Postage, new CultureInfo(o.Culture), (ProcessStatus)o.ProcessStatus, GetDeliveryAddress(user.Id, o.DeliveryAddress), orderItems));
            });

            return Result;
        }

        public void OrderPaid(in Order order, in PaymentStatus paymentStatus, in string message)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (String.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            if (paymentStatus == PaymentStatus.Unpaid)
                throw new ArgumentOutOfRangeException(nameof(paymentStatus));

            OrderDataRow orderDataRow = _orders.Select(order.Id);

            if (orderDataRow == null)
                throw new InvalidOperationException("Order could not be found!");

            long userId = orderDataRow.UserId;

            InvoiceDataRow newInvoice = new InvoiceDataRow()
            {
                UserId = userId,
                DeliveryAddress = orderDataRow.DeliveryAddress,
                OrderId = orderDataRow.Id,
                Postage = orderDataRow.Postage,
                Culture = orderDataRow.Culture,
                PaymentStatus = (int)paymentStatus,
                ProcessStatus = orderDataRow.ProcessStatus,
            };

            _invoices.Insert(newInvoice);

            List<InvoiceItemDataRow> invoiceItems = new List<InvoiceItemDataRow>();
            List<OrderItemDataRow> orderItems = _ordersItems.Select().Where(oi => oi.OrderId.Equals(orderDataRow.Id)).ToList();

            orderItems.ForEach(oi =>
            {
                invoiceItems.Add(new InvoiceItemDataRow()
                {
                    InvoiceId = newInvoice.Id,
                    OrderItemId = oi.Id,
                    Description = oi.Description,
                    TaxRate = oi.TaxRate,
                    Price = oi.Price,
                    Quantity = oi.Quantity,
                    Discount = oi.Discount,
                    DiscountType = oi.DiscountType,
                    ItemStatus = oi.ItemStatus,
                });
            });

            _invoiceItems.Insert(invoiceItems);
        }

        #endregion Orders

        #region Invoices

        public List<Invoice> InvoicesGet(in Int64 userId)
        {
            List<Invoice> Result = new List<Invoice>();
            UserDataRow user = _users.Select(userId);

            if (user == null)
                return Result;

            List<InvoiceDataRow> userInvoices = _invoices.Select().Where(o => o.UserId == user.Id).ToList();

            userInvoices.ForEach(i =>
            {
                List<InvoiceItem> invoiceItems = new List<InvoiceItem>();

                List<InvoiceItemDataRow> userOrderItems = _invoiceItems.Select().Where(ii => ii.InvoiceId.Equals(i.Id)).ToList();

                userOrderItems.ForEach(ii => invoiceItems.Add(new InvoiceItem(ii.Id, ii.Description, ii.Price, ii.TaxRate, ii.Quantity, (ItemStatus)ii.ItemStatus, (DiscountType)ii.DiscountType, ii.Discount)));

                Result.Add(new Invoice(i.Id, i.Created, i.Postage, new CultureInfo(i.Culture), (ProcessStatus)i.ProcessStatus, (PaymentStatus)i.PaymentStatus, GetDeliveryAddress(user.Id, i.DeliveryAddress), invoiceItems));
            });

            return Result;
        }

        #endregion Invoices

        #region Private Methods

        private static DeliveryAddress ConvertToDeliveryAddress(AddressDataRow address)
        {
            if (address == null)
                return null;

            return new DeliveryAddress(address.Id, address.BusinessName, address.AddressLine1, address.AddressLine2, 
                address.AddressLine3, address.City, address.County, address.Country, address.Postcode, address.PostageCost);
        }

        private static int GenerateRandomNumber()
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }

        #endregion Private Methods
    }
}
