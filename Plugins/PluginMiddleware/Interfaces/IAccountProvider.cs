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
 *  Copyright (c) 2012 - 2018 Simon Carter.  All Rights Reserved.
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
    public interface IAccountProvider
    {
        #region Change Password

        bool ChangePassword(in Int64 userId, in string newPassword);

        #endregion Change Password

        #region Address Options

        AddressOptions GetAddressOptions(in AddressOption addressOption);

        #endregion Address Options

        #region User Contact Details

        bool GetUserAccountDetails(in Int64 userId, out string firstName, out string lastName, 
            out string email, out bool emailConfirmed, out string telephone, out bool telephoneConfirmed);

        bool SetUserAccountDetails(in Int64 userId, in string firstName, in string lastName, 
            in string email, in string telephone);

        bool ConfirmEmailAddress(in Int64 userId, in string confirmationCode);

        bool ConfirmTelephoneNumber(in Int64 userId, in string confirmationCode);

        #endregion User Contact Details

        #region Create Account

        bool CreateAccount(in string email, in string firstName, in string surname, in string password,
            in string telephone, in string businessName,
            in string addressLine1, in string addressLine2, in string addressLine3, in string city,
            in string county, in string postcode, in string countryCode, out Int64 userId);

        #endregion Create Account

        #region Billing Address

        bool SetBillingAddress(in Int64 userId, in Address billingAddress);

        Address GetBillingAddress(in Int64 userId);

        #endregion Billing Address

        #region Delivery Address

        bool SetDeliveryAddress(in Int64 userId, in DeliveryAddress deliveryAddress);

        List<DeliveryAddress> GetDeliveryAddresses(in Int64 userId);

        DeliveryAddress GetDeliveryAddress(in Int64 userId, in int deliveryAddressId);

        bool DeleteDeliveryAddress(in Int64 userId, in DeliveryAddress deliveryAddress);

        bool AddDeliveryAddress(in Int64 userId, in DeliveryAddress deliveryAddress);

        #endregion Delivery Address

        #region Marketing Preferences

        MarketingOptions GetMarketingOptions();

        Marketing GetMarketingPreferences(in Int64 userId);

        bool SetMarketingPreferences(in Int64 userId, in Marketing marketing);

        #endregion Marketing Preferences

        #region Orders

        List<Order> OrdersGet(in Int64 userId);

        #endregion Orders

        #region Invoices

        List<Invoice> InvoicesGet(in Int64 userId);

        #endregion Invoices
    }
}
