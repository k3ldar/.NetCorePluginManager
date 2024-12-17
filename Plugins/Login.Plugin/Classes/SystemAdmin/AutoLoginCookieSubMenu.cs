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
 *  Product:  Login Plugin
 *  
 *  File: AutoLoginCookieSubMenu.cs
 *
 *  Purpose:  Auto Login Timings for cookies Sub Menu
 *
 *  Date        Name                Reason
 *  17/02/2019  Simon Carter        Initially Created
 *  23/04/2020  Simon Carter        Renamed for cookie login
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace LoginPlugin.Classes.SystemAdmin
{
	public class AutoLoginCookieSubMenu : SystemAdminSubMenu
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

		public override string Data()
		{
			string Result = "Setting|Value";

			Result += $"\rTotal Requests|{LoginMiddleware._autoLoginCookieTimings.Requests}";
			Result += $"\rFastest ms|{LoginMiddleware._autoLoginCookieTimings.Fastest}";
			Result += $"\rSlowest ms|{LoginMiddleware._autoLoginCookieTimings.Slowest}";
			Result += $"\rAverage ms|{LoginMiddleware._autoLoginCookieTimings.Average}";
			Result += $"\rTrimmed Avg ms|{LoginMiddleware._autoLoginCookieTimings.TrimmedAverage}";
			Result += $"\rTotal ms|{LoginMiddleware._autoLoginCookieTimings.Total}";

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
			return Languages.LanguageStrings.AutoLoginCookies;
		}

		public override string ParentMenuName()
		{
			return Languages.LanguageStrings.Timings;
		}

		public override int SortOrder()
		{
			return 0;
		}
	}
}

#pragma warning restore CS1591