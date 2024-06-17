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
 *  Product:  SharedPluginFeatures
 *  
 *  File: IIpValidation.cs
 *
 *  Purpose:  Provides interface for Managing Ip connections
 *
 *  Date        Name                Reason
 *  05/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using static SharedPluginFeatures.Enums;

namespace SharedPluginFeatures
{
	/// <summary>
	/// This interface should be implemented by the host application and provides a conduit for 
	/// notifications events for Ip addresses being monitored within BadEgg.Plugin module.
	/// 
	/// This interface, when implemented can be used by applications to evaluate how Ip
	/// addresses are behaving whilst navigating their website.
	/// 
	/// See BadEgg.Plugin and BadEggAttribute for information on how to validate routes 
	/// within an application.
	/// </summary>
	public interface IIpValidation
	{
		/// <summary>
		/// Indicates a new connection has been made by a client Ip.
		/// </summary>
		/// <param name="ipAddress">Ip Address that has connected.</param>
		void ConnectionAdd(in string ipAddress);

		/// <summary>
		/// Indicates that a connection has expired and is about to be removed.  Host applications
		/// can evaluate how the Ip used the website and 
		/// </summary>
		/// <param name="ipAddress">Ip address being monitored.</param>
		/// <param name="hits">Average number of hits per minute.</param>
		/// <param name="requests">Total number of requests made.</param>
		/// <param name="duration">Total time the client was active within the website.</param>
		void ConnectionRemove(in string ipAddress, in double hits, in ulong requests, in TimeSpan duration);

		/// <summary>
		/// Provides a report on the connection after it has been validated.
		/// </summary>
		/// <param name="ipAddress">Ip address being monitored.</param>
		/// <param name="queryString">Query and form data that was validated.</param>
		/// <param name="validation">Result of validation.</param>
		void ConnectionReport(in string ipAddress, in string queryString, in ValidateRequestResult validation);

		/// <summary>
		/// Indicates that a connection has failed validation and that a request to ban the Ip address
		/// is made.
		/// </summary>
		/// <param name="ipAddress">Ip address being monitored.</param>
		/// <param name="hits">Average number of hits per minute.</param>
		/// <param name="requests">Total number of requests made.</param>
		/// <param name="duration">Total time the client was active within the website.</param>
		/// <returns>bool.  If the implementing class returns true, the Ip address will be included
		/// on the blacklist and further requests will be denied.  If false is returned then
		/// no further action will be taken.</returns>
		bool ConnectionBan(in string ipAddress, in double hits, in ulong requests, in TimeSpan duration);
	}
}
