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
 *  Copyright (c) 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Shopping Cart Plugin
 *  
 *  File: ShippingAddressModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  23/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Runtime.CompilerServices;

namespace ShoppingCartPlugin.Models
{
    public sealed class ShippingAddressModel
    {
        #region Constructors

        public ShippingAddressModel(in int id, in string businessName, in string addressLine1,
            in string addressLine2, in string addressLine3, in string city, in string county,
            in string postcode, in string country, in decimal shippingCost)
        {
            Id = id;
            Name = businessName;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            AddressLine3 = addressLine3;
            City = city;
            County = county;
            Postcode = postcode;
            Country = country;
            ShippingCost = shippingCost;
        }

        #endregion Constructors

        #region Properties

        public int Id { get; private set; }

        public string Name { get; private set; }

        public string AddressLine1 { get; private set; }

        public string AddressLine2 { get; private set; }

        public string AddressLine3 { get; private set; }

        public string City { get; private set; }

        public string County { get; private set; }

        public string Postcode { get; private set; }

        public string Country { get; private set; }

        public decimal ShippingCost { get; private set; }

        #endregion Properties

        #region Public Methods

        public string ShippingAddress
        {
            get
            {
                string Result = AddLine(Name);
                Result += AddLine(AddressLine1);
                Result += AddLine(AddressLine2);
                Result += AddLine(AddressLine3);
                Result += AddLine(City);
                Result += AddLine(County);
                Result += AddLine(Postcode);
                Result += AddLine(Country);

                return Result;
            }
        }

        #endregion Public Methods

        #region Private Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string AddLine(string s)
        {
            if (String.IsNullOrEmpty(s))
                return String.Empty;

            return $"{s}<br />";
        }

        #endregion Private Methods
    }
}
