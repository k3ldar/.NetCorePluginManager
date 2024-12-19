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
 *  Copyright (c) 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Subdomain.Plugin
 *  
 *  File: SubdomainTimingsSubMenu.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  13/02/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace Subdomain.Plugin.Classes.SystemAdmin
{
	/// <summary>
	/// Returns Timings information for all time spent processing subdomain requests and can 
	/// be viewed within SystemAdmin.Plugin.  
	/// 
	/// This class descends from SystemAdminSubMenu.
	/// </summary>
	public sealed class SubdomainTimingsSubMenu : SystemAdminSubMenu
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
		/// Returns Timings data in milliseconds for time spent processing by Subdomain.Plugin requests.
		/// </summary>
		/// <returns>string</returns>
		public override string Data()
		{
			string Result = "Setting|Value";

			Result += $"\rTotal Requests|{SubdomainMiddleware._timings.Requests}";
			Result += $"\rFastest ms|{SubdomainMiddleware._timings.Fastest}";
			Result += $"\rSlowest ms|{SubdomainMiddleware._timings.Slowest}";
			Result += $"\rAverage ms|{SubdomainMiddleware._timings.Average}";
			Result += $"\rTrimmed Avg ms|{SubdomainMiddleware._timings.TrimmedAverage}";
			Result += $"\rTotal ms|{SubdomainMiddleware._timings.Total}";

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
			return "Subdomain";
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