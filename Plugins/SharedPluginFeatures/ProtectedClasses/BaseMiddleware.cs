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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatures
 *  
 *  File: BaseMiddleware.cs
 *
 *  Purpose:  Base Middleware Class
 *
 *  Date        Name                Reason
 *  19/10/2018  Simon Carter        Initially Created
 *  11/11/2018  Simon Carter        Add Methods to get user session and logged in status
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


using Shared.Classes;

namespace SharedPluginFeatures
{
    public class BaseMiddleware : BaseCoreClass
    {
        #region Constants

        private const string LoweredRoute = "RouteLowered";
        private const string ExtensionLowered = "ExtensionLowered";

        #endregion Constants

        #region Protected Methods

        protected ITempDataDictionary GetTempData(in HttpContext context)
        {
            ITempDataDictionaryFactory factory = context.RequestServices.GetService(typeof(ITempDataDictionaryFactory)) as ITempDataDictionaryFactory;
            return (factory.GetTempData(context));
        }

        protected UserSession GetUserSession(in HttpContext context)
        {
            if (context.Items.ContainsKey(Constants.UserSession))
            {
                return ((UserSession)context.Items[Constants.UserSession]);
            }

            return (null);
        }

        protected bool IsUserLoggedIn(in HttpContext context)
        {
            UserSession session = GetUserSession(context);

            if (session != null)
            {
                return (!String.IsNullOrEmpty(session.UserEmail));
            }

            return (false);
        }

        protected string RouteLowered(in HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Items.ContainsKey(LoweredRoute))
                return (context.Items[LoweredRoute].ToString());

            string routeLowered = context.Request.Path.ToString().ToLower();

            context.Items.Add(LoweredRoute, routeLowered);

            return (routeLowered);
        }

        protected string RouteFileExtension(in HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Items.ContainsKey(ExtensionLowered))
                return (context.Items[ExtensionLowered].ToString());

            string loweredExtension = System.IO.Path.GetExtension(RouteLowered(context));

            context.Items.Add(ExtensionLowered, loweredExtension);

            return (loweredExtension);
        }

        protected string GetIpAddress(in HttpContext context)
        {
            string Result = context.Request.HttpContext.Connection.RemoteIpAddress.ToString();

            if (Result == "::1")
                Result = "127.0.0.1";

            return (Result);
        }


        protected void GetLocalIpAddresses(in HashSet<string> ipAddressList)
        {
            ipAddressList.Clear();

            ipAddressList.Add("::1");
            ipAddressList.Add("127.0.0.1");

            foreach (string ip in Shared.Utilities.LocalIPAddresses())
                if (!ipAddressList.Contains(ip))
                    ipAddressList.Add(ip);

            ipAddressList.Add("0.0.0.0");
        }

        #region Cookies

        protected bool CookieExists(in HttpContext context, in string name)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            return context.Request.Cookies.ContainsKey(name);
        }

        protected void CookieDelete(in HttpContext context, in string name)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (context.Request.Cookies.ContainsKey(name))
                context.Response.Cookies.Append(name, String.Empty, new CookieOptions() { Expires = DateTime.Now.AddDays(-1) });
        }

        protected string CookieValue(in HttpContext context, in string name, in string defaultValue = "")
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (!CookieExists(context, name))
                return defaultValue;

            return context.Request.Cookies[name];
        }

        protected string CookieValue(in HttpContext context, in string name, in string encryptionKey, in string defaultValue = "")
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrEmpty(encryptionKey))
                throw new ArgumentNullException(nameof(encryptionKey));

            if (!CookieExists(context, name))
                return defaultValue;

            string Result = context.Request.Cookies[name];

            if (String.IsNullOrEmpty(Result))
                return defaultValue;

            return Shared.Utilities.Decrypt(Result, encryptionKey);
        }

        protected void CookieAdd(in HttpContext context, in string name, in string value, in int days)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            CookieOptions options = new CookieOptions()
            {
                HttpOnly = false
            };

            if (days > -1)
                options.Expires = DateTime.Now.AddDays(days);

            context.Response.Cookies.Append(name, value, options);
        }

        #endregion Cookies


        #endregion Protected Methods
    }
}
