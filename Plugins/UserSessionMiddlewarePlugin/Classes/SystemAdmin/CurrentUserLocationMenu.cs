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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
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
using System.Text;

using Shared.Classes;

using SharedPluginFeatures;

namespace UserSessionMiddleware.Plugin.Classes.SystemAdmin
{
    public sealed class CurrentUserLocationMenu : SystemAdminSubMenu
    {
        #region Overridden Methods

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
            return (Enums.SystemAdminMenuType.Map);
        }

        public override string Data()
        {
            return (GetUserMapData());
        }

        public override string Name()
        {
            return ("Map of Visitors");
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

        #endregion Overridden Methods

        #region Private Methods

        private string GetUserMapData()
        {
            string Result = String.Empty;

            foreach (UserSession session in UserSessionManager.Clone)
            {
                Result += String.Format("['{0}\\rUser: {3}\\rConverted: {4}\\rMobile: {5}\\rReferrer: {6}" + 
                    "\\rCountry: {9}\\rCity: {10}\\rTotal Pages: {11}\\rTotal Time: {12} (s)', {1}, {2}, {7}, {8}, {13}, '{14}'],",
                    session.IPAddress,
                    session.Latitude,
                    session.Longitude,
                    String.IsNullOrEmpty(session.UserName) ? "Unknown" : session.UserName,
                    session.CurrentSale > 0.00m ? "Yes" : "No",
                    session.IsMobileDevice ? "Yes" : "No",
                    session.Referal.ToString(),
                    session.IsBot ? 1 : 2, // 7 bot
                    session.CurrentSale > 0.00m ? 1 : 2, // 8 sale
                    session.CountryCode,
                    session.CityName,
                    session.Pages.Count,
                    session.TotalTime,
                    session.Bounced ? 1 : 2,
                    GetImageName(session));
            }

            if (Result.EndsWith(","))
                Result = Result.Substring(0, Result.Length - 1);

            return (Result);
        }

        private string GetImageName(UserSession session)
        {
            if (session.IsBot)
                return ("orange");

            if (session.Bounced)
                return ("yellow");

            if (session.CurrentSale > 0.00m)
            {
                if (session.IsMobileDevice)
                    return ("grn-pushpin");
                else
                    return ("blue-pushpin");
            }

            if (!String.IsNullOrEmpty(session.UserEmail))
            {
                if (session.IsMobileDevice)
                    return ("green-dot");
                else
                    return ("green");
            }

            return ("blue");
        }

        #endregion Private Methods
    }
}
