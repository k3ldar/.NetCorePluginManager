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
 *  File: BaseController.cs
 *
 *  Purpose:  Base Controller Class
 *
 *  Date        Name                Reason
 *  17/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

using Shared.Classes;

namespace SharedPluginFeatures
{
    public class BaseController : Controller
    {
        #region User Sessions

        protected UserSession GetUserSession()
        {
            if (HttpContext.Items.ContainsKey("UserSession"))
            {
                return ((UserSession)HttpContext.Items["UserSession"]);
            }

            return (null);
        }

        protected bool IsUserLoggedIn()
        {
            UserSession session = GetUserSession();

            if (session != null)
            {
                return (!String.IsNullOrEmpty(session.UserEmail));
            }

            return (false);
        }

        protected string GetCoreSettionId()
        {
            return (HttpContext.Session.Id);
        }

        #endregion User Sessions

        #region Cookies

        protected bool CookieExists(in string name)
        {
            return HttpContext.Request.Cookies.ContainsKey(name);
        }

        protected void CookieDelete(in string name)
        {
            if (HttpContext.Request.Cookies.ContainsKey(name))
                HttpContext.Response.Cookies.Append(name, String.Empty, new CookieOptions() { Expires = DateTime.Now.AddDays(-1) });
        }

        protected string CookieValue(in string name, in string defaultValue = "")
        {
            if (!CookieExists(name))
                return defaultValue;

            return HttpContext.Request.Cookies[name];
        }

        protected void CookieAdd(in string name, in string value, in int days)
        {
            CookieOptions options = new CookieOptions()
            {
                HttpOnly = false
            };

            if (days > -1)
                options.Expires = DateTime.Now.AddDays(days);

            HttpContext.Response.Cookies.Append(name, value, options);
        }

        #endregion Cookies

        #region Settings

        protected T GetSettings<T>(in string storageName, in string sectionName)
        {
            ISettingsProvider settings = (ISettingsProvider)HttpContext.RequestServices.GetService(typeof(ISettingsProvider));

            if (settings == null)
                throw new InvalidOperationException($"Unable to find ISettingsProvider");

            if (String.IsNullOrEmpty(storageName))
                throw new ArgumentNullException(nameof(storageName));

            if (String.IsNullOrEmpty(sectionName))
                throw new ArgumentNullException(nameof(sectionName));

            return AppSettings.ValidateSettings<T>.Validate((T)settings.GetSettings<ISettingsProvider>(storageName, sectionName));
        }

        protected T GetSettings<T>(in string sectionName)
        {
            return (GetSettings<T>("appsettings.json", sectionName));
        }

        #endregion Settings

        #region Ip Address

        protected string GetIpAddress()
        {
            return (HttpContext.Connection.RemoteIpAddress.ToString());
        }

        #endregion Ip Address
    }
}
