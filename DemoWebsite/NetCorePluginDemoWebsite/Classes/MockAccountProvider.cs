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
 *  Product:  Login Plugin
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
using System.Linq;

using static Middleware.Enums;

using Middleware;
using System.Collections.Generic;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
    public class MockAccountProvider : IAccountProvider
    {
        #region Change Password

        public bool ChangePassword(in long userId, in string newPassword)
        {
            return newPassword.Equals("Pa$$w0rd");
        }

        #endregion Change Password

        #region Address Options

        public AddressOptions GetAddressOptions()
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
            return confirmationCode.Equals("NewEmail");
        }

        public bool ConfirmTelephoneNumber(in Int64 userId, in string confirmationCode)
        {
            return confirmationCode.Equals("NewTelephone");
        }

        #endregion User Contact Details

        #region Create Account

        public bool CreateAccount(in string email, in string firstName, in string surname, in string password, 
            in string telephone, in string businessName, in string addressLine1, in string addressLine2, 
            in string addressLine3, in string city, in string county, in string postcode, in string countryCode, 
            out long userId)
        {
            userId = 2;


            return true;
        }

        #endregion Create Account

        #region Billing Address

        public bool SetBillingAddress(in long userId, in Address billingAddress)
        {
            return billingAddress.AddressLine1 == "Mike St";
        }

        public Address GetBillingAddress(in long userId)
        {
            return new Address(String.Empty, "Mike St", String.Empty, String.Empty, "London", String.Empty, "L1 1AA", "GB");
        }

        #endregion Billing Address

        #region Delivery Address

        public bool SetDeliveryAddress(in long userId, in DeliveryAddress deliveryAddress)
        {
            return true;
        }

        public List<DeliveryAddress> GetDeliveryAddresses(in long userId)
        {
            return new List<DeliveryAddress>()
            {
                new DeliveryAddress(1, String.Empty, "1 Mike St", String.Empty, String.Empty, "London", String.Empty, "L1 1AA", "GB", 5.99m),
                new DeliveryAddress(2, String.Empty, "29 5th Avenue", String.Empty, String.Empty, "New York", String.Empty, "49210", "US", 5.99m),
            };
        }

        public DeliveryAddress GetDeliveryAddress(in Int64 userId, in int deliveryAddressId)
        {
            foreach (DeliveryAddress address in GetDeliveryAddresses(userId))
            {
                if (address.AddressId == deliveryAddressId)
                    return address;
            }

            return null;
        }

        public bool DeleteDeliveryAddress(in long userId, in DeliveryAddress deliveryAddress)
        {
            return true;
        }

        #endregion Delivery Address
    }
}
