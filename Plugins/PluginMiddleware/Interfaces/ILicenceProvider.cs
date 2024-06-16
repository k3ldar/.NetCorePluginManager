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
 *  File: ILicenceProvider.cs
 *
 *  Purpose:  Licence provider
 *
 *  Date        Name                Reason
 *  05/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Middleware.Accounts.Licences;

namespace Middleware
{
	/// <summary>
	/// Licence provider for managing user licences.
	/// 
	/// This item must be implemented by the host application and made available via DI.
	/// </summary>
	public interface ILicenceProvider
	{
		#region Public Interface Methods

		/// <summary>
		/// Retrieves a list of licence types.
		/// </summary>
		/// <returns>List&lt;LicenceType&gt;</returns>
		List<LicenceType> LicenceTypesGet();

		/// <summary>
		/// Retrieve all licences for a specific user.
		/// </summary>
		/// <param name="userId">Unique id of user whos licence details are being requested.</param>
		/// <returns>List&lt;Licence&gt;</returns>
		List<Licence> LicencesGet(in Int64 userId);

		/// <summary>
		/// Updates the domain or Ip details for a licence that is linked to a specific server.
		/// </summary>
		/// <param name="userId">Unique id of the user who is updating the Licence.</param>
		/// <param name="licence">Licence being updated.</param>
		/// <param name="domain">Domain or Ip address to be applied against the Licence.</param>
		/// <returns>bool</returns>
		bool LicenceUpdateDomain(in Int64 userId, in Licence licence, in string domain);

		/// <summary>
		/// Requests for an email to be sent, with details of an individual licence for a user.
		/// </summary>
		/// <param name="userId">Unique id of the user makeing the request.</param>
		/// <param name="licenceId">Unique id of the Licence who's email details are being requested.</param>
		/// <returns>bool</returns>
		bool LicenceSendEmail(in Int64 userId, in int licenceId);

		/// <summary>
		/// Request to create a trial licence for a product.
		/// </summary>
		/// <param name="userId">Unique id of the user requesting a trial licence.</param>
		/// <param name="licenceType">Type of licence a trial key is being requested for.</param>
		/// <returns>LicenceCreate</returns>
		LicenceCreate LicenceTrialCreate(in Int64 userId, in LicenceType licenceType);

		#endregion Public Interface Methods
	}
}
