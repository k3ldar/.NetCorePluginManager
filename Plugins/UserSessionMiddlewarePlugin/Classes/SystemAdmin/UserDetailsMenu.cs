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
 *  Product:  SharedPluginFeatures
 *  
 *  File: UserDetailsMenu.cs
 *
 *  Purpose:  Class for containing User session sales by country statistics
 *
 *  Date        Name                Reason
 *  29/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Text;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace UserSessionMiddleware.Plugin.Classes.SystemAdmin
{
    /// <summary>
    /// Returns a list of detailed information of all active sessions currently 
    /// active within the website and can be viewed within SystemAdmin.Plugin.  
    /// 
    /// This class descends from SystemAdminSubMenu.
    /// </summary>
    public sealed class UserDetailsMenu : SystemAdminSubMenu
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
            return Enums.SystemAdminMenuType.FormattedText;
        }

        public override string Data()
        {
            StringBuilder Result = new StringBuilder();

            foreach (UserSession session in UserSessionManager.Clone)
            {
                Result.Append($"<h2>{session.SessionID}</h2>");
                string countryCode = String.IsNullOrEmpty(session.CountryCode) ? "ZZ" : session.CountryCode;

                Result.Append($"<p>Time Created: {session.Created}<br />");
                Result.Append($"Country: {countryCode}<br />");
                Result.Append($"City Name: {session.CityName}<br />");
                Result.Append($"Bounced: {session.Bounced}<br />");
                Result.Append($"Sale: {session.CurrentSaleCurrency}{session.CurrentSale}<br />");
                Result.Append($"Bot: {session.IsBot}<br />");
                Result.Append($"User Agent: {session.UserAgent}");
                Result.Append($"Referrer: {session.InitialReferrer}<br />");
                Result.Append($"IP Address: {session.IPAddress}<br />");

                if (!String.IsNullOrEmpty(session.UserEmail))
                {
                    Result.Append($"Username: {session.UserName}<br />");
                    Result.Append($"Email: {session.UserEmail}<br />");
                }

                Result.Append($"Total Page Views: {session.Pages.Count}<br />");
                Result.Append($"Total Time: {session.TotalTime}</p>");
                Result.Append($"<h3>Pages Visited</h3>");
                Result.Append("<table><tr><th>Total time</th><th>Page</th></tr>");

                foreach (var page in session.Pages)
                {
                    Result.Append($"<tr><td>{page.TotalTime}</td>");
                    Result.Append($"<td>{page.URL}</td></tr>");
                }

                Result.Append("</table>");
            }

            return Result.ToString().Trim();
        }

        public override string Name()
        {
            return "User Session Details";
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