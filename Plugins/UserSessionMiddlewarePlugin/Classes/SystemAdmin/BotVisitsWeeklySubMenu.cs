﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Product:  UserSessionMiddleware.Plugin
 *  
 *  File: DefaultSessionTimingsSubMenu.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  17/11/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;

using Middleware;
using Middleware.SessionData;

using PluginManager.Abstractions;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace UserSessionMiddleware.Plugin.Classes.SystemAdmin
{
	/// <summary>
	/// Returns data for weekly bot visits to be shown in a chart.  
	/// 
	/// This class descends from SystemAdminSubMenu.
	/// </summary>
	public sealed class BotVisitsWeeklySubMenu : SystemAdminSubMenu
	{
		#region Private Members

		private readonly ISessionStatisticsProvider _sessionStatisticsProvider;
		private readonly bool _enabled;

		#endregion Private Members

		public BotVisitsWeeklySubMenu(ISettingsProvider settingsProvider, ISessionStatisticsProvider sessionStatisticsProvider)
		{
			_sessionStatisticsProvider = sessionStatisticsProvider ?? throw new ArgumentNullException(nameof(sessionStatisticsProvider));

			if (settingsProvider == null)
				throw new ArgumentNullException(nameof(settingsProvider));

			UserSessionSettings settings = settingsProvider.GetSettings<UserSessionSettings>(SharedPluginFeatures.Constants.UserSessionConfiguration);

			_enabled = settings.EnableDefaultSessionService;
		}

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
		/// Returns last 26 weeks of bot sessions by week.
		/// </summary>
		/// <returns>string</returns>
		public override string Data()
		{
			ChartModel Result = new()
			{
				ChartTitle = "Weekly Bot Visitor Statistics"
			};

			List<SessionWeekly> sessionData = _sessionStatisticsProvider.GetWeeklyData(false)
				.OrderBy(o => o.Year)
				.ThenBy(o => o.Week)
				.Take(26)
				.ToList();

			if (sessionData.Count == 0)
				return String.Empty;

			Result.DataNames.Add(new KeyValuePair<ChartDataType, string>(ChartDataType.String, "Week"));
			Result.DataNames.Add(new KeyValuePair<ChartDataType, string>(ChartDataType.Number, "Bot Visits"));
			Result.DataNames.Add(new KeyValuePair<ChartDataType, string>(ChartDataType.Number, "Bounced"));

			foreach (SessionWeekly week in sessionData)
			{
				List<Decimal> datavalues = [];
				Result.DataValues[week.Week.ToString(Thread.CurrentThread.CurrentUICulture)] = datavalues;

				datavalues.Add(week.BotVisits);
				datavalues.Add(week.Bounced);
			}

			return JsonSerializer.Serialize(Result);
		}

		public override string Image()
		{
			return SharedPluginFeatures.Constants.SystemImageChart;
		}

		public override Enums.SystemAdminMenuType MenuType()
		{
			return Enums.SystemAdminMenuType.Chart;
		}

		public override string Name()
		{
			return "Bot Visits - Weekly";
		}

		public override string ParentMenuName()
		{
			return "User Sessions";
		}

		public override int SortOrder()
		{
			return 670;
		}

		public override Boolean Enabled()
		{
			return _enabled;
		}
	}
}

#pragma warning restore CS1591