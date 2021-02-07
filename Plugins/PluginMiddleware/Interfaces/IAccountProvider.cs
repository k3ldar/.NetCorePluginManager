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
 *  Copyright (c) 2012 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: IAccountProvider.cs
 *
 *  Purpose:  User Account provider
 *
 *  Date        Name                Reason
 *  16/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Middleware.Accounts.Invoices;
using Middleware.Accounts.Orders;

namespace Middleware.Accounts
{
    /// <summary>
    /// Provides user account services predominantly used by UserAccount.Plugin module.
    /// 
    /// This item must be implemented by the host application and made available via DI.
    /// </summary>
    public interface IAccountProvider
    {
        #region Change Password

        /// <summary>
        /// Change a password for a specific user.
        /// </summary>
        /// <param name="userId">Unique id of user.</param>
        /// <param name="newPassword">New password that is to be set for the user.</param>
        /// <returns></returns>
        bool ChangePassword(in Int64 userId, in string newPassword);

        #endregion Change Password

        #region Address Options

        /// <summary>
        /// Retrieves the appropriate address options that are available for user addresses.
        /// </summary>
        /// <param name="addressOption">Address options to be shown.</param>
        /// <returns>AddressOptions</returns>
        AddressOptions GetAddressOptions(in AddressOption addressOption);

        #endregion Address Options

        #region User Contact Details

        /// <summary>
        /// Retrieves default user account details.
        /// </summary>
        /// <param name="userId">Unique user id.</param>
        /// <param name="firstName">Users first name.</param>
        /// <param name="lastName">Users last name.</param>
        /// <param name="email">Users email address.</param>
        /// <param name="emailConfirmed">Indicates whether the email address has been confirmed or not.</param>
        /// <param name="telephone">Users telephone number.</param>
        /// <param name="telephoneConfirmed">Indicates whether the telephone number has been confirmed or not.</param>
        /// <returns></returns>
        bool GetUserAccountDetails(in Int64 userId, out string firstName, out string lastName,
            out string email, out bool emailConfirmed, out string telephone, out bool telephoneConfirmed);

        /// <summary>
        /// Updates default user account details.
        /// 
        /// The values provided during this request will depend on the AddressOptions rules that are in place.
        /// </summary>
        /// <param name="userId">Unique user id.</param>
        /// <param name="firstName">Users first name.</param>
        /// <param name="lastName">Users last name.</param>
        /// <param name="email">Users email address.</param>
        /// <param name="telephone">Users telephone number.</param>
        /// <returns></returns>
        bool SetUserAccountDetails(in Int64 userId, in string firstName, in string lastName,
            in string email, in string telephone);

        /// <summary>
        /// Provides a mechanism whereby the website can confirm the users telephone number.
        /// 
        /// Primarily the site sends a unique code associated with the account, the user then enters
        /// the code for verification.
        /// </summary>
        /// <param name="userId">Unique Id of the user.</param>
        /// <param name="confirmationCode">User supplied confirmation code.</param>
        /// <returns>bool.  True if the confirmation code was correct.</returns>
        bool ConfirmEmailAddress(in Int64 userId, in string confirmationCode);

        /// <summary>
        /// Provides a mechanism whereby the website can confirm the users telephone number.
        /// 
        /// Primarily the site sends a unique code associated with the account, the user then enters
        /// the code for verification.
        /// </summary>
        /// <param name="userId">Unique Id of the user.</param>
        /// <param name="confirmationCode">User supplied confirmation code.</param>
        /// <returns>bool.  True if the confirmation code was correct.</returns>
        bool ConfirmTelephoneNumber(in Int64 userId, in string confirmationCode);

        #endregion User Contact Details

        #region Account Creation/Deletion

        /// <summary>
        /// Creates a new user account.
        /// 
        /// The values provided during this request will depend on the AddressOptions rules that are in place.
        /// </summary>
        /// <param name="email">Email address for the user.</param>
        /// <param name="firstName">Users first name.</param>
        /// <param name="surname">Users last name.</param>
        /// <param name="password">Password supplied by the user.</param>
        /// <param name="telephone">Telephone number provided by the user.</param>
        /// <param name="businessName">Business name supplied by the user.</param>
        /// <param name="addressLine1">Street and number for the address.</param>
        /// <param name="addressLine2">Second line of the address.</param>
        /// <param name="addressLine3">Third line of the address.</param>
        /// <param name="city">City wher the user lives.</param>
        /// <param name="county">County/State where the user lives.</param>
        /// <param name="postcode">Post/Zip code for the user.</param>
        /// <param name="countryCode">Country.</param>
        /// <param name="userId">out.  Unique user id for the new user account.</param>
        /// <returns>bool.  True if the account was created.</returns>
        bool CreateAccount(in string email, in string firstName, in string surname, in string password,
            in string telephone, in string businessName,
            in string addressLine1, in string addressLine2, in string addressLine3, in string city,
            in string county, in string postcode, in string countryCode, out Int64 userId);

        /// <summary>
        /// Delete's a user account
        /// </summary>
        /// <param name="userId">Unique user id for the new user account.</param>
        /// <returns>bool.  True if the account was deleted, otherwise false.</returns>
        bool DeleteAccount(in Int64 userId);

        #endregion Account Creation/Deletion

        #region Account Lock/Unlock

        /// <summary>
        /// Locks a user account, preventing access to the system
        /// </summary>
        /// <param name="userId">Unique user id for the new user account.</param>
        /// <returns>bool.  True if the account was locked, otherwise false.</returns>
        bool AccountLock(in Int64 userId);

        /// <summary>
        /// Unlocks a user account enabling access to the system
        /// </summary>
        /// <param name="userId">Unique user id for the new user account.</param>
        /// <returns>bool.  True if the account was unlocked, otherwise false.</returns>
        bool AccountUnlock(in Int64 userId);

        #endregion Account Lock/Unlock

        #region Billing Address

        /// <summary>
        /// Updates a billing address for a specific user.
        /// </summary>
        /// <param name="userId">Unique user id.</param>
        /// <param name="billingAddress">Billing address to be updated.</param>
        /// <returns>bool.  True if the billing address was updated.</returns>
        bool SetBillingAddress(in Int64 userId, in Address billingAddress);

        /// <summary>
        /// Retrieves the billing address for a specific user.
        /// </summary>
        /// <param name="userId">Unique user id.</param>
        /// <returns>Address</returns>
        Address GetBillingAddress(in Int64 userId);

        #endregion Billing Address

        #region Delivery Address

        /// <summary>
        /// Updates a delivery address for a user.
        /// </summary>
        /// <param name="userId">Unique user id.</param>
        /// <param name="deliveryAddress">Delivery address to be updated.</param>
        /// <returns>bool.  True if the address was updated.</returns>
        bool SetDeliveryAddress(in Int64 userId, in DeliveryAddress deliveryAddress);

        /// <summary>
        /// Retrieve all delivery addresses for a specific user.
        /// </summary>
        /// <param name="userId">Unique user id.</param>
        /// <returns>List&lt;<see cref="DeliveryAddress"/>&gt;</returns>
        List<DeliveryAddress> GetDeliveryAddresses(in Int64 userId);

        /// <summary>
        /// Retrieves a specific delivery address for a user.
        /// </summary>
        /// <param name="userId">Unique user id.</param>
        /// <param name="deliveryAddressId">Unique id of the delivery address associated with the user.</param>
        /// <returns>DeliveryAddress</returns>
        DeliveryAddress GetDeliveryAddress(in Int64 userId, in int deliveryAddressId);

        /// <summary>
        /// Deletes a delivery address for a user.
        /// </summary>
        /// <param name="userId">Unique user id.</param>
        /// <param name="deliveryAddress">Delivery address to be deleted.</param>
        /// <returns>bool.  True if the address was deleted.</returns>
        bool DeleteDeliveryAddress(in Int64 userId, in DeliveryAddress deliveryAddress);

        /// <summary>
        /// Adds a new delivery address for the user.
        /// </summary>
        /// <param name="userId">Unique user id.</param>
        /// <param name="deliveryAddress">Delivery address for the user.</param>
        /// <returns>bool.  True if the address was created.</returns>
        bool AddDeliveryAddress(in Int64 userId, in DeliveryAddress deliveryAddress);

        #endregion Delivery Address

        #region Marketing Preferences

        /// <summary>
        /// Retrieves marketing preferences available to be viewed on the website.
        /// </summary>
        /// <returns>MarketingOptions</returns>
        MarketingOptions GetMarketingOptions();

        /// <summary>
        /// Retrieves marketing preferences for a specific user.
        /// </summary>
        /// <param name="userId">Unique user id.</param>
        /// <returns>Marketing</returns>
        Marketing GetMarketingPreferences(in Int64 userId);

        /// <summary>
        /// Updates marketing preferences for a specific user.
        /// </summary>
        /// <param name="userId">Unique user id.</param>
        /// <param name="marketing">New marketing options selected by the user.</param>
        /// <returns></returns>
        bool SetMarketingPreferences(in Int64 userId, in Marketing marketing);

        #endregion Marketing Preferences

        #region Orders

        /// <summary>
        /// Retrieves all orders for the user.
        /// </summary>
        /// <param name="userId">Unique user id.</param>
        /// <returns>List&lt;Order&gt;</returns>
        List<Order> OrdersGet(in Int64 userId);

        /// <summary>
        /// Indicates that payment was successful for an order.  Implemntors would typically need to 
        /// create an Invoice for the Order item that has been paid for.
        /// </summary>
        /// <param name="order">Order being paid.</param>
        /// <param name="paymentStatus">Current PaymentStatus.  This could vary depending on the IPaymentProvider instance that was used.</param>
        /// <param name="message">A message provided during payment as part of the Order, could for instance delivery instructions.</param>
        void OrderPaid(in Order order, in PaymentStatus paymentStatus, in string message);

        #endregion Orders

        #region Invoices

        /// <summary>
        /// Retrieve all Invoice items for the user.
        /// </summary>
        /// <param name="userId">Unique user id.</param>
        /// <returns>List&lt;Invoice&gt;</returns>
        List<Invoice> InvoicesGet(in Int64 userId);

        #endregion Invoices
    }
}
