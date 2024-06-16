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
 *  Copyright (c) 2012 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: IUserApiQueryProvider.cs
 *
 *  Purpose:  Interface used to query user secret
 *
 *  Date        Name                Reason
 *  18/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace Middleware
{
	/// <summary>
	/// Interface used to query a users api details in order to validate requests
	/// </summary>
	public interface IUserApiQueryProvider
	{
		/// <summary>
		/// Retrieves the Api Secret associated with the merchant and api key
		/// </summary>
		/// <param name="merchantId">Unique Merchant Id</param>
		/// <param name="apiKey">Unique Merchant Api Key</param>
		/// <param name="secret">The secret associated with the merchanes Api Key</param>
		/// <returns>bool</returns>
		bool ApiSecret(string merchantId, string apiKey, out string secret);
	}
}
