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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: Address.cs
 *
 *  Purpose:  Country Details
 *
 *  Date        Name                Reason
 *  16/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace Middleware
{
	/// <summary>
	/// Contains address details that can also be used for DeliveryAddress in IAccountProvider interface.
	/// </summary>
	public class Address
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public Address()
		{

		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id">Shipping costs for the address.</param>
		/// <param name="businessName">Business name if applicable.</param>
		/// <param name="addressLine1">Address line 1.</param>
		/// <param name="addressLine2">Address line 2.</param>
		/// <param name="addressLine3">Address line 3.</param>
		/// <param name="city">City name.</param>
		/// <param name="county">County/state name.</param>
		/// <param name="postcode">Postal or zip code.</param>
		/// <param name="country">Postal or zip code.</param>
		public Address(in long id, in string businessName, in string addressLine1,
			in string addressLine2, in string addressLine3, in string city, in string county, in string postcode, in string country)
		{
			Id = id;
			BusinessName = businessName;
			AddressLine1 = addressLine1;
			AddressLine2 = addressLine2;
			AddressLine3 = addressLine3;
			City = city;
			County = county;
			Postcode = postcode;
			Country = country;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Unique id for the address.
		/// </summary>
		/// <value>int</value>
		public long Id { get; set; }

		/// <summary>
		/// Business name if applicable.
		/// </summary>
		/// <value>string</value>
		public string BusinessName { get; set; }

		/// <summary>
		/// Address line 1.
		/// </summary>
		/// <value>string</value>
		public string AddressLine1 { get; set; }

		/// <summary>
		/// Address line 2.
		/// </summary>
		/// <value>string</value>
		public string AddressLine2 { get; set; }

		/// <summary>
		/// Address line 3.
		/// </summary>
		/// <value>string</value>
		public string AddressLine3 { get; set; }

		/// <summary>
		/// City name.
		/// </summary>
		/// <value>string</value>
		public string City { get; set; }

		/// <summary>
		/// County/state name.
		/// </summary>
		/// <value>string</value>
		public string County { get; set; }

		/// <summary>
		/// Postal or zip code.
		/// </summary>
		/// <value>string</value>
		public string Postcode { get; set; }

		/// <summary>
		/// Country name.
		/// </summary>
		/// <value>string</value>
		public string Country { get; set; }

		#endregion Properties
	}
}
