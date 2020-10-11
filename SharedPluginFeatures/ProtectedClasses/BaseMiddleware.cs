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
    /// <summary>
    /// Generic functions that can be used by middleware plugins to obtain generic information 
    /// to be used when serving requests within the pipeline.
    /// </summary>
    public class BaseMiddleware : BaseCoreClass
    {
        #region Constants

        private const string LoweredRoute = "RouteLowered";
        private const string RouteNormal = "Route";
        private const string ExtensionLowered = "ExtensionLowered";

        #endregion Constants

        #region Protected Methods

        /// <summary>
        /// Retrieves the current Uri for the request.
        /// </summary>
        /// <param name="context">Valid HttpContext for the request.</param>
        /// <returns>Uri</returns>
        protected Uri GetCurrentUri(in HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            UriBuilder uriBuilder = new UriBuilder(context.Request.Scheme,
                context.Request.Host.Host.ToString(),
                context.Request.Host.Port.Value,
                context.Request.Path.ToString())
            {
                Query = context.Request.QueryString.ToString()
            };

            return uriBuilder.Uri;
        }

        /// <summary>
        /// Retreives the host name for the context.
        /// </summary>
        /// <param name="context">Valid HttpContext for the request.</param>
        /// <returns>string</returns>
        protected string GetHost(in HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Request.Host.Port.HasValue)
            {
                return new UriBuilder(context.Request.Scheme,
                    context.Request.Host.Host.ToString(),
                    context.Request.Host.Port.Value).ToString();
            }
            else
            {
                return new UriBuilder(context.Request.Scheme,
                    context.Request.Host.Host.ToString()).ToString();
            }
        }

        /// <summary>
        /// Retrieves an instance of ITempDataDictionary used to manipulate temp data for the curent request.
        /// </summary>
        /// <param name="context">Valid HttpContext for the request.</param>
        /// <returns></returns>
        protected ITempDataDictionary GetTempData(in HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            ITempDataDictionaryFactory factory = context.RequestServices.GetService(typeof(ITempDataDictionaryFactory)) as ITempDataDictionaryFactory;
            return factory.GetTempData(context);
        }

        /// <summary>
        /// Retrieves the current users UserSession instance which contains data for the user.
        /// 
        /// Requires UserSessionMiddleware.Plugin module to be loaded.
        /// </summary>
        /// <param name="context">Valid HttpContext for the request.</param>
        /// <returns>null if the UserSessionMiddleware.Plugin is not loaded otherwise a valid UserSession item representing 
        /// the current users session.</returns>
        protected UserSession GetUserSession(in HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Items.ContainsKey(Constants.UserSession))
            {
                return (UserSession)context.Items[Constants.UserSession];
            }

            return null;
        }

        /// <summary>
        /// Determines if the current user is logged in or not.
        /// 
        /// Requires UserSessionMiddleware.Plugin module to be loaded.
        /// </summary>
        /// <param name="context">Valid HttpContext for the request.</param>
        /// <returns>True if the user is logged in, otherwise false.</returns>
        protected bool IsUserLoggedIn(in HttpContext context)
        {
            UserSession session = GetUserSession(context);

            if (session != null)
            {
                return !String.IsNullOrEmpty(session.UserEmail);
            }

            return false;
        }

        /// <summary>
        /// Retrieves the current route being requested through the pipeline.
        /// </summary>
        /// <param name="context">Valid HttpContext for the request.</param>
        /// <returns>string</returns>
        protected string Route(in HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Items.ContainsKey(RouteNormal))
                return context.Items[RouteNormal].ToString();

            string route = context.Request.Path.ToString();

            context.Items.Add(RouteNormal, route);

            return route;
        }

        /// <summary>
        /// Retrieves the current route being requested through the pipeline in lowercase.
        /// </summary>
        /// <param name="context">Valid HttpContext for the request.</param>
        /// <returns>string</returns>
        protected string RouteLowered(in HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Items.ContainsKey(LoweredRoute))
                return context.Items[LoweredRoute].ToString();

            string routeLowered = Route(context).ToLower();

            context.Items.Add(LoweredRoute, routeLowered);

            return routeLowered;
        }

        /// <summary>
        /// Retrieves the file extension for the file requested in the current request in lowercase.
        /// 
        /// Primarily used to determine if the request is for a static file.
        /// </summary>
        /// <param name="context">Valid HttpContext for the request.</param>
        /// <returns>string</returns>
        protected string RouteFileExtension(in HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Items.ContainsKey(ExtensionLowered))
                return context.Items[ExtensionLowered].ToString();

            string loweredExtension = System.IO.Path.GetExtension(RouteLowered(context));

            context.Items.Add(ExtensionLowered, loweredExtension);

            return loweredExtension;
        }

        /// <summary>
        /// Retrieves the current Ip address for the current request.
        /// </summary>
        /// <param name="context">Valid HttpContext for the request.</param>
        /// <returns>string</returns>
        protected string GetIpAddress(in HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            foreach (string key in Constants.ForwardForHeader)
                if (context.Request.Headers.ContainsKey(key))
                    return context.Request.Headers[key];

            string Result = context.Request.HttpContext.Connection.RemoteIpAddress.ToString();

            if (Result == "::1")
                Result = "127.0.0.1";

            return Result;
        }


        /// <summary>
        /// Retrieves a list of all local Ip Addresses on the current server.
        /// </summary>
        /// <param name="ipAddressList">List of HashSet&lt;string&gt; which will be populated with the ip addresses from the current computer.</param>
        protected void GetLocalIpAddresses(in HashSet<string> ipAddressList)
        {
            if (ipAddressList == null)
                throw new ArgumentNullException(nameof(ipAddressList));

            ipAddressList.Clear();

            ipAddressList.Add("::1");
            ipAddressList.Add("127.0.0.1");

            foreach (string ip in Shared.Utilities.LocalIPAddresses())
                if (!ipAddressList.Contains(ip))
                    ipAddressList.Add(ip);

            ipAddressList.Add("0.0.0.0");
        }

        #region Cookies

        /// <summary>
        /// Determines whether a cookie exists or not.
        /// </summary>
        /// <param name="context">Valid HttpContext for the request.</param>
        /// <param name="name">Name of the cookie.</param>
        /// <returns>bool</returns>
        protected bool CookieExists(in HttpContext context, in string name)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            return context.Request.Cookies.ContainsKey(name);
        }

        /// <summary>
        /// Deletes a cookie if it exists.
        /// </summary>
        /// <param name="context">Valid HttpContext for the request.</param>
        /// <param name="name">Name of the cookie.</param>
        protected void CookieDelete(in HttpContext context, in string name)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (context.Request.Cookies.ContainsKey(name))
                context.Response.Cookies.Append(name, String.Empty, new CookieOptions() { Expires = DateTime.Now.AddDays(-1) });
        }

        /// <summary>
        /// Retrieves the contents of a cookie.
        /// </summary>
        /// <param name="context">Valid HttpContext for the request.</param>
        /// <param name="name">Name of the cookie.</param>
        /// <param name="defaultValue">Value to be returned if the cookie does not exist.</param>
        /// <returns>string</returns>
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

        /// <summary>
        /// Retrieves the contents of a cookie.
        /// </summary>
        /// <param name="context">Valid HttpContext for the request.</param>
        /// <param name="name">Name of the cookie.</param>
        /// <param name="defaultValue">Value to be returned if the cookie does not exist.</param>
        /// <returns>long</returns>
        protected long CookieValue(in HttpContext context, in string name, in long defaultValue)
        {
            string value = CookieValue(context, name, String.Empty);

            if (String.IsNullOrEmpty(value))
                return defaultValue;

            if (Int64.TryParse(value, out long result))
                return result;

            return defaultValue;
        }

        /// <summary>
        /// Retrieves the contents of a cookie.
        /// </summary>
        /// <param name="context">Valid HttpContext for the request.</param>
        /// <param name="name">Name of the cookie.</param>
        /// <param name="encryptionKey">Key used to decrypt the contents when retrieved.</param>
        /// <param name="defaultValue">Value to be returned if the cookie does not exist.</param>
        /// <returns>string</returns>
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

        /// <summary>
        /// Adds a cookie.
        /// </summary>
        /// <param name="context">Valid HttpContext for the request.</param>
        /// <param name="name">Name of the cookie.</param>
        /// <param name="value">Value to be stored within the cookie.</param>
        /// <param name="days">Number of days the cookie is valid for.  A value of -1 indicates a session cookie which will expire when the session ends.</param>
        protected void CookieAdd(in HttpContext context, in string name, in string value, in int days)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            CookieOptions options = new CookieOptions()
            {
                HttpOnly = false,
                SameSite = SameSiteMode.Strict,
            };

            if (days > -1)
                options.Expires = DateTime.Now.AddDays(days);

            context.Response.Cookies.Append(name, value, options);
        }

        #endregion Cookies

        #endregion Protected Methods
    }
}
