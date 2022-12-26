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
 *  Product:  CacheControl Plugin
 *  
 *  File: CacheControlSettings.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  14/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

using SharedPluginFeatures;

namespace CacheControl.Plugin
{
	/// <summary>
	/// Dictionary of CacheControlRoute values loaded from settings using ISettingsProvider
	/// </summary>
	public class CacheControlSettings : IPluginSettings
	{
		#region Constructors

		/// <summary>
		/// Default Constructor
		/// </summary>
		public CacheControlSettings()
		{
			CacheControlRoutes = new Dictionary<string, CacheControlRoute>();
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Name
		/// </summary>
		public string SettingsName => "CacheControlRoute";

		/// <summary>
		/// Indicates that caching is disabled or enabled.
		/// 
		/// If disabled then no values will be applied to any header.
		/// </summary>
		/// <value>bool</value>
		public bool Disabled { get; set; }

		/// <summary>
		/// Dictionary of CacheControlRoute values, definining routes cache values.
		/// </summary>
		/// <value>Dictionary&lt;string, CacheControlRoute&gt;</value>
		public Dictionary<string, CacheControlRoute> CacheControlRoutes { get; set; }

		#endregion Properties
	}
}
