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
    /// <summary>
    /// Extended address information.  Extends Address class.
    /// </summary>
    public sealed class DeliveryAddress : Address
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public DeliveryAddress()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="addressId">Address id.</param>
        /// <param name="businessName">Name of business if applicable.</param>
        /// <param name="addressLine1">Address line 1.</param>
        /// <param name="addressLine2">Address line 2.</param>
        /// <param name="addressLine3">Address line 3.</param>
        /// <param name="city">Name of city.</param>
        /// <param name="county">County or State name.</param>
        /// <param name="postcode">Postal or zip code.</param>
        /// <param name="country">Country name.</param>
        /// <param name="postageCost">Postage costs.</param>
        public DeliveryAddress(in int addressId, in string businessName, in string addressLine1,
            in string addressLine2, in string addressLine3, in string city, in string county,
            in string postcode, in string country, in decimal postageCost)
            : base(addressId, postageCost, businessName, addressLine1,
                  addressLine2, addressLine3, city, county, postcode, country)
        {
            AddressId = addressId;
            PostageCost = postageCost;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Address id
        /// </summary>
        /// <value>int</value>
        public int AddressId { get; set; }

        /// <summary>
        /// Postage cost for the address.
        /// </summary>
        /// <value>decimal</value>
        public decimal PostageCost { get; set; }

        #endregion Properties
    }
}
