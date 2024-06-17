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
 *  File: DefaultEmailSenderSettings.cs
 *
 *  Purpose:  Default email sender
 *
 *  Date        Name                Reason
 *  17/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using AppSettings;

namespace Middleware.Classes
{
	/// <summary>
	/// Default email sender details
	/// </summary>
	public class DefaultEmailSenderSettings
	{
		/// <summary>
		/// Email host server
		/// </summary>
		[SettingDefault("%EmailHost%")]
		public string Host { get; set; }

		/// <summary>
		/// Email server password
		/// </summary>
		[SettingDefault("%EmailUserPassword%")]
		public string Password { get; set; }

		/// <summary>
		/// Email server port
		/// </summary>
		[SettingDefault("%EmailPort%")]
		public string Port { get; set; }

		/// <summary>
		/// Email uses ssl for sending emails
		/// </summary>
		[SettingDefault("%EmailSSL%")]
		public string SSL { get; set; }

		/// <summary>
		/// Email account username
		/// </summary>
		[SettingDefault("%EmailUserName%")]
		public string UserName { get; set; }
	}
}
