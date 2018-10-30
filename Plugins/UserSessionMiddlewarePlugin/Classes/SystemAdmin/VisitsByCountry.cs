using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Shared.Classes;

using SharedPluginFeatures;

namespace UserSessionMiddleware.Plugin.Classes.SystemAdmin
{
    public sealed class VisitsByCountry : SystemAdminSubMenu
    {
        public override string Action()
        {
            return (String.Empty);
        }

        public override string Area()
        {
            return (String.Empty);
        }

        public override string Controller()
        {
            return (String.Empty);
        }

        public override Enums.SystemAdminMenuType MenuType()
        {
            return (Enums.SystemAdminMenuType.Grid);
        }

        public override string Data()
        {
            StringBuilder Result = new StringBuilder("Country|Total Visitors|Is Bot\r");
            List<UserSession> sessions = UserSessionManager.Clone;
            List<SessionStatistics> statistics = new List<SessionStatistics>();


            foreach (UserSession session in sessions)
            {
                string countryCode = String.IsNullOrEmpty(session.CountryCode) ? "ZZ" : session.CountryCode;
                SessionStatistics stats = statistics.Where(s => s.IsBot == session.IsBot &&
                    s.CountryCode.Equals(countryCode)).FirstOrDefault();

                if (stats == null)
                {
                    stats = new SessionStatistics(countryCode);
                    statistics.Add(stats);
                }

                stats.Count++;
            }

            foreach(SessionStatistics stats in statistics)
            { 
                Result.Append(stats.CountryCode + "|");
                Result.Append(stats.Count.ToString() + "|");
                //Result.Append(cpu.Substring(cpu.IndexOf("/") + 1) + "|");
                //Result.Append(SplitText(parts[2], ':') + "|");
                //Result.Append(SplitText(parts[3], ':') + "|");
                //Result.Append(SplitText(parts[4], ':') + "|");
                Result.Append(stats.IsBot ? "Yes" : "No" + "\r");
            }

            return (Result.ToString().Trim());
        }

        public override string Name()
        {
            return ("Visits by Country");
        }

        public override string ParentMenuName()
        {
            return ("User Sessions");
        }

        public override int SortOrder()
        {
            return (0);
        }

        public override string Image()
        {
            return (String.Empty);
        }

        public override string BackColor()
        {
            if (ParentMenu != null)
                return (ParentMenu.BackColor());

            return ("#3498DB");
        }
    }
}
