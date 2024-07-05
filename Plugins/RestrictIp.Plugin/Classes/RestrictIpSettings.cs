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
 *  Product:  RestrictIp.Plugin
 *  
 *  File: SpiderSettings.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  19/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

using SharedPluginFeatures;

namespace RestrictIp.Plugin
{
	/// <summary>
	/// Loads settings us ISettingsProvider to configure the routes that are restricted to
	/// specific Ip addresses etc using RestrictedIpRouteAttribute.
	/// </summary>
	public class RestrictIpSettings : IPluginSettings
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public RestrictIpSettings()
		{
			RouteRestrictions = [];
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Name
		/// </summary>
		public string SettingsName => "RestrictedIpRoutes.Plugin";

		/// <summary>
		/// Determines whether the middleware router is disabled or not.
		/// </summary>
		/// <value>bool</value>
		public bool Disabled { get; set; }

		/// <summary>
		/// Dictionary of restricted routes
		/// </summary>
		/// <value>Dictionary&lt;string, string&gt;</value>
		public Dictionary<string, string> RouteRestrictions { get; set; }

		#endregion Properties
	}
}
