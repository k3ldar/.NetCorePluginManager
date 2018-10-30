using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Shared.Classes;

using SharedPluginFeatures;

namespace UserSessionMiddleware.Plugin.Classes.SystemAdmin
{
    public class SalesByCountry : SystemAdminSubMenu
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
            StringBuilder Result = new StringBuilder("Country|Total Sales|Value\r");
            List<UserSession> sessions = UserSessionManager.Clone;
            List<SessionStatistics> statistics = new List<SessionStatistics>();


            foreach (UserSession session in sessions)
            {
                session.CurrentSale = 9.87m;
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

            return (Result.ToString().Trim());
        }

        public override string Name()
        {
            return ("Sales by Country");
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
