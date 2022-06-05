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
using System.Linq;

using Middleware;
using Middleware.Accounts;
using Middleware.Accounts.Invoices;
using Middleware.Accounts.Orders;

using PluginManager.DAL.TextFiles.Tables;

namespace PluginManager.DAL.TextFiles.Providers
{
    internal class AccountProvider : IAccountProvider
    {
        #region Private Members

        private readonly ITextTableOperations<TableUserRowDefinition> _users;
        private readonly ITextTableOperations<TableAddressRowDefinition> _addresses;

        #endregion Private Members

        #region Constructors

        public AccountProvider(ITextTableOperations<TableUserRowDefinition> users, ITextTableOperations<TableAddressRowDefinition> addresses)
        {
            _users = users ?? throw new ArgumentNullException(nameof(users));
            _addresses = addresses ?? throw new ArgumentNullException(nameof(addresses));
        }

        #endregion Constructors

        #region Change Password

        public bool ChangePassword(in long userId, in string newPassword)
        {
            if (String.IsNullOrEmpty(newPassword))
                throw new ArgumentNullException(nameof(newPassword));

            TableUserRowDefinition user = _users.Select(userId);

            if (user == null)
                throw new ArgumentException("user not found", nameof(userId));

            user.Password = newPassword;
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
            TableUserRowDefinition user = _users.Select(userId);

            firstName = user?.FirstName;
            lastName = user?.Surname;
            email = user?.Email;
            emailConfirmed = user == null ? false : user.EmailConfirmed;
            telephone = user?.Telephone;
            telephoneConfirmed = user == null ? false : user.TelephoneConfirmed;

            return user != null;
        }

        public bool SetUserAccountDetails(in Int64 userId, in string firstName, in string lastName, in string email, in string telephone)
        {
            TableUserRowDefinition user = _users.Select(userId);

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

            TableUserRowDefinition user = _users.Select(userId);

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

            TableUserRowDefinition user = _users.Select(userId);

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
            TableUserRowDefinition newUser = new TableUserRowDefinition();
            newUser.Email = email;
            newUser.FirstName = firstName;
            newUser.Surname = surname;
            newUser.Password = password;
            newUser.Telephone = telephone;
            newUser.BusinessName = businessName;
            newUser.AddressLine1 = addressLine1;
            newUser.AddressLine2 = addressLine2;
            newUser.AddressLine3 = addressLine3;
            newUser.City = city;
            newUser.County = county;
            newUser.Postcode = postcode;
            newUser.CountryCode = countryCode;
            newUser.EmailConfirmCode = GenerateRandomNumber().ToString();
            newUser.TelephoneConfirmCode = GenerateRandomNumber().ToString();

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
            TableUserRowDefinition user = _users.Select(userId);

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
            TableUserRowDefinition user = _users.Select(userId);

            if (user == null)
                return false;

            if (user.Locked)
                return false;

            user.Locked = true;
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
            TableUserRowDefinition user = _users.Select(userId);

            if (user == null)
                return false;

            if (!user.Locked)
                return false;

            user.Locked = false;
            _users.Update(user);

            return true;
        }

        #endregion Account Lock/Unlock

        #region Billing Address

        public bool SetBillingAddress(in long userId, in Address billingAddress)
        {
            if (billingAddress == null)
                throw new ArgumentNullException(nameof(billingAddress));

            TableUserRowDefinition user = _users.Select(userId);

            if (user == null)
                return false;

            int billingAddressId = billingAddress.Id;

            TableAddressRowDefinition userAddresses = _addresses.Select().Where(a => a.UserId == user.Id && a.Id.Equals(billingAddressId)).FirstOrDefault();

            if (userAddresses == null)
            {
                userAddresses = new TableAddressRowDefinition()
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
                    Shipping = billingAddress.Shipping,
                    PostageCost = billingAddress.Shipping,
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
                userAddresses.Shipping = billingAddress.Shipping;
                userAddresses.PostageCost = billingAddress.Shipping;
                userAddresses.UserId = userId;
                _addresses.Update(userAddresses);
            }

            return true;
        }

        public Address GetBillingAddress(in long userId)
        {
            TableUserRowDefinition user = _users.Select(userId);

            if (user == null)
                return null;

            TableAddressRowDefinition userAddresses = _addresses.Select().Where(a => a.UserId == user.Id && !a.IsDelivery).FirstOrDefault();

            return new Address(Convert.ToInt32(userAddresses.Id), userAddresses.Shipping, userAddresses.BusinessName,
                userAddresses.AddressLine1, userAddresses.AddressLine2, userAddresses.AddressLine3,
                userAddresses.City, userAddresses.County, userAddresses.Postcode, userAddresses.Country);
        }

        #endregion Billing Address

        #region Delivery Address

        public bool SetDeliveryAddress(in long userId, in DeliveryAddress deliveryAddress)
        {
            TableUserRowDefinition user = _users.Select(userId);

            if (user == null)
                return false;

            throw new NotImplementedException();
        }

        public List<DeliveryAddress> GetDeliveryAddresses(in long userId)
        {
            TableUserRowDefinition user = _users.Select(userId);

            if (user == null)
                return new List<DeliveryAddress>();

            throw new NotImplementedException();
        }

        public bool AddDeliveryAddress(in Int64 userId, in DeliveryAddress deliveryAddress)
        {
            TableUserRowDefinition user = _users.Select(userId);

            if (user == null)
                return false;

            throw new NotImplementedException();
        }

        public DeliveryAddress GetDeliveryAddress(in Int64 userId, in int deliveryAddressId)
        {
            TableUserRowDefinition user = _users.Select(userId);

            if (user == null)
                return null;

            throw new NotImplementedException();
        }

        public bool DeleteDeliveryAddress(in long userId, in DeliveryAddress deliveryAddress)
        {
            if (deliveryAddress == null || deliveryAddress.AddressId == 1)
                return false;

            TableUserRowDefinition user = _users.Select(userId);

            if (user == null)
                return false;

            throw new NotImplementedException();
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
            TableUserRowDefinition user = _users.Select(userId);

            if (user == null)
                return null;

            return new Marketing(user.MarketingEmail, user.MarketingTelephone, user.MarketingSms, user.MarketingPostal);
        }

        public bool SetMarketingPreferences(in Int64 userId, in Marketing marketing)
        {
            TableUserRowDefinition user = _users.Select(userId);

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
            TableUserRowDefinition user = _users.Select(userId);

            if (user == null)
                return new List<Order>();

            throw new NotImplementedException();
        }

        public void OrderPaid(in Order order, in PaymentStatus paymentStatus, in string message)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (String.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            //TableUserRow user = _users.Select(userId);

            //if (user == null)
            //    return;
            throw new NotImplementedException();
        }

        #endregion Orders

        #region Invoices

        public List<Invoice> InvoicesGet(in Int64 userId)
        {
            TableUserRowDefinition user = _users.Select(userId);

            if (user == null)
                return new List<Invoice>();

            throw new NotImplementedException();
        }

        #endregion Invoices

        #region Private Methods

        private int GenerateRandomNumber()
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }

        #endregion Private Methods
    }
}
