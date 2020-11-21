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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  UserSessionMiddleware.Plugin
 *  
 *  File: DefaultSessionTimingsSubMenu.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  12/09/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Newtonsoft.Json;

using PluginManager.Abstractions;

using SharedPluginFeatures;

using UserSessionMiddleware.Plugin.Classes.SessionData;

#pragma warning disable CS1591

namespace UserSessionMiddleware.Plugin.Classes.SystemAdmin
{
    /// <summary>
    /// Returns data for daily visits to be shown in a chart.  
    /// 
    /// This class descends from SystemAdminSubMenu.
    /// </summary>
    public sealed class VisitsDailySubMenu : SystemAdminSubMenu
    {
        private readonly bool _enabled;

        public VisitsDailySubMenu(ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            UserSessionSettings settings = settingsProvider.GetSettings<UserSessionSettings>(Constants.UserSessionConfiguration);

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
        /// Returns last 30 days of daily user sessions.
        /// </summary>
        /// <returns>string</returns>
        public override string Data()
        {
            ChartModel Result = new ChartModel();

            Result.ChartTitle = "Daily Visitor Statistics";

            List<SessionDaily> sessionData = DefaultUserSessionService.GetDailyData(false)
                .OrderBy(o => o.Date)
                .Take(30)
                .ToList();

            if (sessionData == null)
                return String.Empty;

            Result.DataNames.Add(new KeyValuePair<ChartDataType, string>(ChartDataType.String, "Day"));
            Result.DataNames.Add(new KeyValuePair<ChartDataType, string>(ChartDataType.Number, "Visits"));
            Result.DataNames.Add(new KeyValuePair<ChartDataType, string>(ChartDataType.Number, "Mobile Visits"));
            Result.DataNames.Add(new KeyValuePair<ChartDataType, string>(ChartDataType.Number, "Bounced"));

            foreach (SessionDaily day in sessionData)
            {
                List<Decimal> datavalues = new List<decimal>();
                Result.DataValues.Add(
                    day.Date.ToString(Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern),
                    datavalues);

                datavalues.Add(day.HumanVisits);
                datavalues.Add(day.MobileVisits);
                datavalues.Add(day.Bounced);
            }

            return JsonConvert.SerializeObject(Result);
        }

        public override string Image()
        {
            return Constants.SystemImageChart;
        }

        public override Enums.SystemAdminMenuType MenuType()
        {
            return Enums.SystemAdminMenuType.Chart;
        }

        public override string Name()
        {
            return "Visits - Daily";
        }

        public override string ParentMenuName()
        {
            return "User Sessions";
        }

        public override int SortOrder()
        {
            return 460;
        }

        public override Boolean Enabled()
        {
            return _enabled;
        }
    }
}

#pragma warning restore CS1591
