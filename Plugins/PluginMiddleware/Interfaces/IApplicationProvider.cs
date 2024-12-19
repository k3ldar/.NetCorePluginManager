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
 *  Product:  PluginMiddleware
 *  
 *  File: IApplicationProvider.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  31/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace Middleware
{
	/// <summary>
	/// Application provider interface provides application wide methods that can be used from
	/// any plugin module within the website.
	/// 
	/// This item must be implemented by the host application and made available via DI.
	/// </summary>
	public interface IApplicationProvider
	{
		/// <summary>
		/// Send an email to site administrators.
		/// </summary>
		/// <param name="subject">Subject of email.</param>
		/// <param name="message">Message</param>
		void Email(in string subject, in string message);
	}
}
