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
 *  Copyright (c) 2024 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Cron.Plugin
 *  
 *  File: CronSettings.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  08/08/2024  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SharedPluginFeatures;

namespace Cron.Plugin
{
	/// <summary>
	/// Settings which affect how Cron is configured and used.
	/// </summary>
	public class CronSettings : IPluginSettings
	{
		#region Properties

		/// <summary>
		/// Name of settings
		/// </summary>
		public string SettingsName => Constants.CronSettingsName;

		#endregion Properties
	}
}
