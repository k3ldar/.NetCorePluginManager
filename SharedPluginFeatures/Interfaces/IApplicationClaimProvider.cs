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
 *  Product:  SharedPluginFeatues
 *  
 *  File: IApplicationClaimProvider.cs
 *
 *  Purpose: Provides an application with an opportunity to add additional 
 *			 claims for a user, if it is supported by the claims provider
 *
 *  Date        Name                Reason
 *  29/11/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;
using System.Security.Claims;

namespace SharedPluginFeatures
{
	/// <summary>
	/// Interface that can be used by an application to provide additional claims for a user
	/// </summary>
	public interface IApplicationClaimProvider
	{
		/// <summary>
		/// Provide a list of claims for the user
		/// </summary>
		/// <param name="userId">Id of user who's claims are being created</param>
		/// <returns>List&lt;Claim&gt;</returns>
		List<Claim> AdditionalUserClaims(in long userId);
	}
}
