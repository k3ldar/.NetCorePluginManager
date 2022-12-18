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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Helpdesk Plugin
 *  
 *  File: Pop3ClientFactory.cs
 *
 *  Purpose:  Factory used to create pop3 clients
 *
 *  Date        Name                Reason
 *  17/12/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Shared.Communication;

using SharedPluginFeatures;

#if NET6_0_OR_GREATER

namespace HelpdeskPlugin.Classes
{
	/// <summary>
	/// Factory for creating IPop3Clients
	/// </summary>
	public sealed class Pop3ClientFactory : IPop3ClientFactory
	{
		/// <summary>
		/// Create new instance of pop 3 client
		/// </summary>
		/// <returns>IPop3Client</returns>
		public IPop3Client Create()
		{
			return new Pop3Client();
		}
	}
}

#endif
