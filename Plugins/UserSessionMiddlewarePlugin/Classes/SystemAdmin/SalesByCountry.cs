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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatures
 *  
 *  File: SalesByCountry.cs
 *
 *  Purpose:  Class for containing User session sales by country statistics
 *
 *  Date        Name                Reason
 *  29/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace UserSessionMiddleware.Plugin.Classes.SystemAdmin
{
    /// <summary>
    /// Returns a list of all sales from the active user sessions and can 
    /// be viewed within SystemAdmin.Plugin.  
    /// 
    /// This class descends from SystemAdminSubMenu.
    /// </summary>
    public sealed class SalesByCountry : SystemAdminSubMenu
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

        public override Enums.SystemAdminMenuType MenuType()
        {
            return Enums.SystemAdminMenuType.Grid;
        }

        public override string Data()
        {
            StringBuilder Result = new StringBuilder("Country|Total Sales|Value\r");
            List<UserSession> sessions = UserSessionManager.Clone;
            List<SessionStatistics> statistics = new List<SessionStatistics>();


            foreach (UserSession session in sessions)
            {
                if (session.CurrentSale <= 0)
                    continue;

                string countryCode = String.IsNullOrEmpty(session.CountryCode) ? "ZZ" : session.CountryCode;
                SessionStatistics stats = statistics.Where(s => s.IsBot == session.IsBot &&
                    s.CountryCode.Equals(countryCode)).FirstOrDefault();

                if (stats == null)
                {
                    stats = new SessionStatistics(countryCode);
                    statistics.Add(stats);
                }

                stats.Count++;
                stats.Value += session.CurrentSale;
            }

            foreach (SessionStatistics stats in statistics)
            {
                Result.Append(stats.CountryCode + "|");
                Result.Append(stats.Count.ToString() + "|");
                Result.Append(stats.Value.ToString("G") + "\r");
            }

            return Result.ToString().Trim();
        }

        public override string Name()
        {
            return "Sales by Country";
        }

        public override string ParentMenuName()
        {
            return "User Sessions";
        }

        public override int SortOrder()
        {
            return 0;
        }

        public override string Image()
        {
            return String.Empty;
        }

        public override string BackColor()
        {
            return "#3498DB";
        }
    }
}

#pragma warning restore CS1591