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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatures
 *  
 *  File: SessionHelper.cs
 *
 *  Purpose:  Provides interface for Managing sessions
 *
 *  Date        Name                Reason
 *  29/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

using Shared;
using Shared.Classes;

using SharedPluginFeatures;

using AspNetCore.PluginManager;

namespace UserSessionMiddleware.Plugin
{
    /// <summary>
    /// The purpose of this static class is to provide a conduit to integrating with the user session
    /// and obtaining geo ip data, if required
    /// </summary>
    internal static class SessionHelper
    {
        #region Private Static Members

        private static byte _loadGeoIpServiceAttempts = 0;
        private static byte _loadUserSessionServiceAttempts = 0;
        private const byte _maximumLoadInterfaceAttempts = 10;
        private static IGeoIpDataService _geoIpInstance;
        private static IUserSessionService _userSessionService;

        #endregion Private Static Members

        #region Internal Static Methods

        internal static void InitSessionHelper()
        {
            UserSessionManager.Instance.OnSessionCreated += UserSession_OnSessionCreated;
            UserSessionManager.Instance.OnSavePage += UserSession_OnSavePage;
            UserSessionManager.Instance.OnSessionClosing += UserSession_OnSessionClosing;
            UserSessionManager.Instance.OnSessionRetrieve += UserSession_OnSessionRetrieve;
            UserSessionManager.Instance.OnSessionSave += UserSession_OnSessionSave;
            UserSessionManager.Instance.IPAddressDetails += UserSession_IPAddressDetails;
        }

        internal static void UserSession_IPAddressDetails(object sender, IpAddressArgs e)
        {
            if (LoadGeoIpService())
            {
                if (_geoIpInstance.GetIPAddressDetails(e.IPAddress, out string countryCode, out string region, 
                    out string cityName, out decimal latitude, out decimal longitude, out long ipUniqueId))
                {
                    e.IPUniqueID = ipUniqueId;
                    e.Latitude = latitude;
                    e.Longitude = longitude;
                    e.CityName = cityName;
                    e.CountryCode = countryCode;
                    e.Region = region;
                }
            }
            else
            {
                e.CountryCode = "ZZ";
                e.CityName = "Unknown";
                e.Region = String.Empty;
            }
        }

        internal static void UserSession_OnSessionSave(object sender, UserSessionArgs e)
        {
            if (LoadUserSessionService())
            {
                _userSessionService.Save(e.Session);
            }
            else
            {
                // if there is no service to save the page, indicate it has been saved so
                // that this method isn't repeatedly called multiple times for each page
                e.Session.SaveStatus = SaveStatus.Saved;
            }
        }

        internal static void UserSession_OnSessionRetrieve(object sender, UserSessionRequiredArgs e)
        {
            if (LoadUserSessionService())
            {
                UserSession userSession = null;
                _userSessionService.Retrieve(e.SessionID, ref userSession);

                if (userSession != null)
                    e.Session = userSession;
            }
        }

        internal static void UserSession_OnSessionClosing(object sender, UserSessionArgs e)
        {
            if (LoadUserSessionService())
            {
                _userSessionService.Closing(e.Session);
            }
        }

        internal static void UserSession_OnSavePage(object sender, UserPageViewArgs e)
        {
            if (LoadUserSessionService())
            {
                _userSessionService.SavePage(e.Session);
            }
            else
            {
                // if there is no interface to save the page, indicate it has been saved so
                // that this method isn't repeatedly called multiple times for each page
                if (e.Session.SaveStatus != SaveStatus.Saved)
                    e.Session.SaveStatus = SaveStatus.Saved;

                foreach (PageViewData item in e.Session.Pages)
                {
                    if (item.SaveStatus != SaveStatus.Saved)
                        item.SaveStatus = SaveStatus.Saved;
                }
            }
        }

        internal static void UserSession_OnSessionCreated(object sender, UserSessionArgs e)
        {
            if (LoadUserSessionService())
            {
                _userSessionService.Created(e.Session);
            }
        }

        #endregion Internal Static Methods

        #region Private Static Methods

        private static bool LoadGeoIpService()
        {
            if (_geoIpInstance != null)
                return (true);

            if (_loadGeoIpServiceAttempts > _maximumLoadInterfaceAttempts)
                return (false);

            _loadGeoIpServiceAttempts++;

            try
            {
                _geoIpInstance = Initialisation.GetServiceProvider.GetRequiredService<IGeoIpDataService>();
            }
            catch (InvalidOperationException)
            {

            }

            if (_geoIpInstance != null)
                return (true);

            List<IGeoIpDataService> geoIpList = PluginManagerService.GetPluginClasses<IGeoIpDataService>();

            if (geoIpList.Count == 0)
                return (false);

            _geoIpInstance = geoIpList[0];

            return (true);
        }

        private static bool LoadUserSessionService()
        {
            if (_userSessionService != null)
                return (true);

            if (_loadUserSessionServiceAttempts > _maximumLoadInterfaceAttempts)
                return (false);

            _loadUserSessionServiceAttempts++;

            try
            {
                _userSessionService = Initialisation.GetServiceProvider.GetRequiredService<IUserSessionService>();
            }
            catch (InvalidOperationException)
            {

            }

            if (_userSessionService != null)
                return (true);

            List<IUserSessionService> userSessions = PluginManagerService.GetPluginClasses<IUserSessionService>();

            if (userSessions.Count == 0)
                return (false);

            _userSessionService = userSessions[0];

            return (true);
        }

        #endregion Private Static Methods
    }
}
