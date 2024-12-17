/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Login Plugin
 *  
 *  File: TokenUserDetails.cs
 *
 *  Purpose:  Concrete implementation of ITokenUserDetails
 *
 *  Date        Name                Reason
 *  05/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Text.Json.Serialization;

using Middleware;

#pragma warning disable CS1591

namespace LoginPlugin.Classes
{
	public class TokenUserDetails : ITokenUserDetails
	{
		public TokenUserDetails()
		{

		}

		public TokenUserDetails(FacebookRemoveUser facebookRemoveUser)
		{
			if (facebookRemoveUser == null)
				throw new ArgumentNullException(nameof(facebookRemoveUser));

			Id = facebookRemoveUser.UserId;
			Provider = "Facebook";
		}

		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("given_name")]
		public string GivenName { get; set; }

		[JsonPropertyName("email")]
		public string Email { get; set; }

		[JsonPropertyName("picture")]
		public string Picture { get; set; }

		public string Provider { get; set; }

		public bool Verify { get; set; }
	}
}

#pragma warning restore CS1591