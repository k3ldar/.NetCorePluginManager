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
 *  Product:  SharedPluginFeatues
 *  
 *  File: IApiAuthorizationService.cs
 *
 *  Purpose:  Interface providing api authorization
 *
 *  Date        Name                Reason
 *  18/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.AspNetCore.Http;

namespace SharedPluginFeatures
{
	/// <summary>
	/// Api authorization service interface
	/// </summary>
	public interface IApiAuthorizationService
	{
		/// <summary>
		/// Validates a request against the api service
		/// </summary>
		/// <param name="httpRequest">Request making the call</param>
		/// <param name="policyName">Name of the policy</param>
		/// <param name="responseCode">Response code</param>
		/// <returns>bool</returns>
		bool ValidateApiRequest(HttpRequest httpRequest, string policyName, out int responseCode);
	}
}
