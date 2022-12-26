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
 *  Copyright (c) 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Subdomain.Plugin
 *  
 *  File: SubdomainSettings.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  13/02/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using AppSettings;

namespace Subdomain.Plugin
{
	/// <summary>
	/// Loads settings to configure the routes that are subdomains.
	/// </summary>
	public class SubdomainSetting
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public SubdomainSetting()
		{

		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Determines whether subdomain routing is disabled or not.
		/// </summary>
		/// <value>bool</value>
		public bool Disabled { get; set; }

		/// <summary>
		/// Route that the configuration will be redirected to, this must be a relative route.
		/// </summary>
		/// <value>string</value>
		[SettingUri(false, System.UriKind.Relative)]
		public string RedirectedRoute { get; set; }

		/// <summary>
		/// Indicates whether the route is permanently redirected (301) or temporarily directed (302)
		/// </summary>
		/// <value>bool</value>
		public bool PermanentRedirect { get; set; }

		#endregion Properties
	}
}
