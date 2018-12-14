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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SharedPluginFeatures;
using static SharedPluginFeatures.Enums;

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
    }
}
