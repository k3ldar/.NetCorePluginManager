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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: DeliveryAddress.cs
 *
 *  Purpose:  Delivery Address
 *
 *  Date        Name                Reason
 *  16/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace Middleware.Accounts
{
    public sealed class DeliveryAddress : Address
    {
        #region Constructors

        public DeliveryAddress()
        {
        }

        public DeliveryAddress(in int addressId, in string businessName, in string addressLine1, 
            in string addressLine2, in string addressLine3, in string city, in string county, 
            in string postcode, in string country, in decimal postageCost)
            : base (businessName, addressLine1, addressLine2, addressLine3, city, county, postcode, country)
        {
            AddressId = addressId;
            PostageCost = postageCost;
        }

        #endregion Constructors

        #region Properties

        public int AddressId { get; set; }

        public decimal PostageCost { get; set; }

        #endregion Properties
    }
}
