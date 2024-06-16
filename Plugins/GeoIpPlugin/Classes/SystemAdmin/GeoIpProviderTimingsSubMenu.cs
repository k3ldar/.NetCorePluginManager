﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Product:  GeoIpPlugin
 *  
 *  File: GeoIpDatabaseTimingsSubMenu.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  14/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace GeoIp.Plugin.Classes.SystemAdmin
{
	/// <summary>
	/// Returns Timings information for all time spent processing Geo Ip requests and can 
	/// be viewed within SystemAdmin.Plugin.  
	/// 
	/// This class descends from SystemAdminSubMenu.
	/// </summary>
	public class GeoIpProviderTimingsSubMenu : SystemAdminSubMenu
	{
		public override string Action()
		{
			return String.Empty;
		}

		public override string Area()
		{
			return String.Empty;
		}

		public override string Controller()
		{
			return String.Empty;
		}

		/// <summary>
		/// Returns Timings data in milliseconds for time spent processing by GeoIpPlugin requests.
		/// </summary>
		/// <returns>string</returns>
		public override string Data()
		{
			string Result = "Setting|Value";

			Result += $"\rTotal Requests|{GeoIpService._timingsIpProvider.Requests}";
			Result += $"\rFastest ms|{GeoIpService._timingsIpProvider.Fastest}";
			Result += $"\rSlowest ms|{GeoIpService._timingsIpProvider.Slowest}";
			Result += $"\rAverage ms|{GeoIpService._timingsIpProvider.Average}";
			Result += $"\rTrimmed Avg ms|{GeoIpService._timingsIpProvider.TrimmedAverage}";
			Result += $"\rTotal ms|{GeoIpService._timingsIpProvider.Total}";

			return Result;
		}

		public override string Image()
		{
			return Constants.SystemImageStopWatch;
		}

		public override Enums.SystemAdminMenuType MenuType()
		{
			return Enums.SystemAdminMenuType.Grid;
		}

		public override string Name()
		{
			return "GeoIp Provider";
		}

		public override string ParentMenuName()
		{
			return "Timings";
		}

		public override int SortOrder()
		{
			return 0;
		}
	}
}

#pragma warning restore CS1591