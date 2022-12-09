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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  UserSessionMiddleware.Plugin
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

using Microsoft.Extensions.DependencyInjection;

using PluginManager.Abstractions;

using Shared;
using Shared.Classes;

using SharedPluginFeatures;

namespace UserSessionMiddleware.Plugin
{
    /// <summary>
    /// The purpose of this static class is to provide a conduit to integrating with the user session
    /// and obtaining customer user session data or geo ip data, if required
    /// </summary>
    internal static class SessionHelper
    {
        #region Private Static Members

        private static byte _loadGeoIpServiceAttempts = 0;
        private static byte _loadUserSessionServiceAttempts = 0;
        private const byte _maximumLoadInterfaceAttempts = 10;
        private static IGeoIpProvider _geoIpInstance;
        private static IUserSessionService _userSessionService;
        private static IPluginClassesService _pluginClasses { get; set; }
        private static IServiceProvider _serviceProvider;

        #endregion Private Static Members

        #region Internal Static Methods

        internal static void InitSessionHelper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            UserSessionManager.Instance.OnSessionCreated += UserSession_OnSessionCreated;
            UserSessionManager.Instance.OnSavePage += UserSession_OnSavePage;
            UserSessionManager.Instance.OnSessionClosing += UserSession_OnSessionClosing;
            UserSessionManager.Instance.OnSessionRetrieve += UserSession_OnSessionRetrieve;
            UserSessionManager.Instance.OnSessionSave += UserSession_OnSessionSave;
            UserSessionManager.Instance.IPAddressDetails += UserSession_IPAddressDetails;

            _pluginClasses = _serviceProvider.GetRequiredService<IPluginClassesService>();
        }

        internal static void UserSession_IPAddressDetails(object sender, IpAddressArgs e)
        {
            if (LoadGeoIpService() &&
                _geoIpInstance.GetIpAddressDetails(e.IPAddress, out string countryCode, out string region,
                    out string cityName, out decimal latitude, out decimal longitude, out long ipUniqueId, out long _, out long _))
            {
                e.IPUniqueID = ipUniqueId;
                e.Latitude = latitude;
                e.Longitude = longitude;
                e.CityName = cityName;
                e.CountryCode = countryCode;
                e.Region = region;
            }
            else
            {
                e.CountryCode = "ZZ";
                e.CityName = "Unknown";
                e.Region = String.Empty;
                e.Latitude = 0;
                e.Longitude = 0;
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
                return true;

            if (_loadGeoIpServiceAttempts > _maximumLoadInterfaceAttempts)
                return false;

            _loadGeoIpServiceAttempts++;

            try
            {
                _geoIpInstance = _serviceProvider.GetService<IGeoIpProvider>();
            }
            catch (InvalidOperationException)
            {
				// ignore specific exception
            }

            if (_geoIpInstance != null)
                return true;

            return false;
        }

        private static bool LoadUserSessionService()
        {
            if (_userSessionService != null)
                return true;

            if (_loadUserSessionServiceAttempts > _maximumLoadInterfaceAttempts)
                return false;

            _loadUserSessionServiceAttempts++;

            try
            {
                _userSessionService = _serviceProvider.GetService<IUserSessionService>();
            }
            catch (InvalidOperationException)
            {
				// ignore specific exception
			}

			if (_userSessionService != null)
                return true;

            return false;
        }

        #endregion Private Static Methods
    }
}
