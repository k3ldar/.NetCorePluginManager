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
 *  File: BaseController.cs
 *
 *  Purpose:  Base Controller Class
 *
 *  Date        Name                Reason
 *  17/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using static Shared.Utilities;
using Shared.Classes;

namespace SharedPluginFeatures
{
    /// <summary>
    /// Base Controller for use as a base for all controllers to obtain information from plugins loaded through PluginManager.
    /// </summary>
    public class BaseController : Controller
    {
        #region User Sessions

        /// <summary>
        /// Retrieves the current users UserSession instance which contains data for the user.
        /// 
        /// Requires UserSessionMiddleware.Plugin module to be loaded.
        /// </summary>
        /// <returns>null if the UserSessionMiddleware.Plugin is not loaded otherwise a valid UserSession item representing 
        /// the current users session.</returns>
        protected UserSession GetUserSession()
        {
            if (HttpContext.Items.ContainsKey(Constants.UserSession))
            {
                return ((UserSession)HttpContext.Items[Constants.UserSession]);
            }

            return (null);
        }

        /// <summary>
        /// Determines if the current user is logged in or not.
        /// 
        /// Requires UserSessionMiddleware.Plugin module to be loaded.
        /// </summary>
        /// <returns>True if the user is logged in, otherwise false.</returns>
        protected bool IsUserLoggedIn()
        {
            UserSession session = GetUserSession();

            if (session != null)
                return (!String.IsNullOrEmpty(session.UserEmail));

            return (false);
        }

        /// <summary>
        /// Retrieves a unique guid representing the currently logged in user.
        /// 
        /// Requires UserSessionMiddleware.Plugin module to be loaded.
        /// </summary>
        /// <returns>Guid.Empty if user is not logged in or a guid is not used, otherwise a valid unique guid for the user.</returns>
        protected Guid UserGuid()
        {
            UserSession session = GetUserSession();

            if (session != null)
                return session.UserGuid;

            return Guid.Empty;
        }

        /// <summary>
        /// Retrieves a unique id representing the currently logged in user.
        /// 
        /// Requires UserSessionMiddleware.Plugin module to be loaded.
        /// </summary>
        /// <returns>Valid user id if the user is logged in, otherwise -1 will be returned.</returns>
        protected long UserId()
        {
            UserSession session = GetUserSession();

            if (session != null)
                return session.UserID;

            return (-1);
        }

        /// <summary>
        /// Retrieves a unique http session id for the current users session.  This is not related
        /// to UserSession.
        /// </summary>
        /// <returns>string.  Unique http session id.</returns>
        protected string GetSessionId()
        {
            UserSession session = GetUserSession();

            if (session != null && !String.IsNullOrEmpty(session.SessionID))
                return session.SessionID;

            return HttpContext.Session.Id;
        }

        /// <summary>
        /// Retrieves a unique http session id for the current session.  this is not related
        /// to UserSession
        /// </summary>
        /// <returns>string.  Unique http session id.</returns>
        protected string GetCoreSessionId()
        {
            return (HttpContext.Session.Id);
        }

        #endregion User Sessions

        #region Breadcrumbs

        /// <summary>
        /// Retrieves the breadcrumbs created for the request from Breadcrumb.Plugin module.  If
        /// no breadcrumbs exist an empty list will be returned.
        /// </summary>
        /// <returns>List&lt;BreadcrumbItem&gt;</returns>
        protected List<BreadcrumbItem> GetBreadcrumbs()
        {
            if (HttpContext.Items.ContainsKey(Constants.Breadcrumbs))
            {
                return (List<BreadcrumbItem>)HttpContext.Items[Constants.Breadcrumbs];
            }

            return new List<BreadcrumbItem>();
        }

        #endregion Breadcrumbs

        #region Shopping Cart

        /// <summary>
        /// Returns a valid ShoppingCartSummary class representing the shopping cart
        /// for the current user.
        /// </summary>
        /// <returns>ShoppingCartSummary</returns>
        protected ShoppingCartSummary GetCartSummary()
        {
            if (HttpContext.Items.ContainsKey(Constants.BasketSummary))
            {
                return (ShoppingCartSummary)HttpContext.Items[Constants.BasketSummary];
            }

            UserSession userSession = GetUserSession();

            // if we have a user session, and that session has a cart id, try 
            // and get the cart via IShoppingCartService
            if (userSession != null && userSession.UserBasketId != 0)
            {
                IShoppingCartService shoppingCartService = (IShoppingCartService)HttpContext
                    .RequestServices.GetService(typeof(IShoppingCartService));

                if (shoppingCartService != null)
                    return shoppingCartService.GetSummary(userSession.UserBasketId);
            }

            return new ShoppingCartSummary(0, 0, 0, 0, 0, GetDefaultTaxRate(), 
                System.Threading.Thread.CurrentThread.CurrentUICulture,
                Constants.CurrencyCodeDefault);
        }

        /// <summary>
        /// Retrieves the default tax rate for the application.
        /// 
        /// Requires ShoppingCart.Plugin module to be loaded.
        /// </summary>
        /// <returns>decimal.  Default tax rate if found, otherwise zero.</returns>
        protected decimal GetDefaultTaxRate()
        {
            if (HttpContext.Items.ContainsKey(Constants.DefaultTaxRate))
                return Convert.ToDecimal(HttpContext.Items[Constants.DefaultTaxRate]);

            return 0;
        }

        /// <summary>
        /// Retrieves the current shopping cart id for the current user session.
        /// 
        /// Requires UserSessionMiddleware.Plugin module to be loaded.
        /// </summary>
        /// <returns>long.  Unique shopping cart id.</returns>
        protected long GetShoppingCartId()
        {
            if (HttpContext.Items.ContainsKey(Constants.ShoppingCart))
            {
                return (long)HttpContext.Items[Constants.ShoppingCart];
            }

            return 0;
        }

        protected void AddToCart()
        {

        }

        #endregion Shopping Cart

        #region Cookies

        /// <summary>
        /// Determines whether a cookie exists or not.
        /// </summary>
        /// <param name="name">Name of the cookie.</param>
        /// <returns>bool</returns>
        protected bool CookieExists(in string name)
        {
            return HttpContext.Request.Cookies.ContainsKey(name);
        }

        /// <summary>
        /// Deletes an existing cookie if it exists.
        /// </summary>
        /// <param name="name">Name of the cookie.</param>
        protected void CookieDelete(in string name)
        {
            if (HttpContext.Request.Cookies.ContainsKey(name))
                HttpContext.Response.Cookies.Append(name, String.Empty, new CookieOptions() { Expires = DateTime.Now.AddDays(-1) });
        }

        /// <summary>
        /// Retrieves the value from an existing cookie, if it exists.
        /// </summary>
        /// <param name="name">Name of the cookie.</param>
        /// <param name="defaultValue">Value to be returned if the cookie does not exist.</param>
        /// <returns>string.  Value from the cookie.</returns>
        protected string CookieValue(in string name, in string defaultValue = "")
        {
            if (!CookieExists(name))
                return defaultValue;

            return HttpContext.Request.Cookies[name];
        }

        /// <summary>
        /// Adds a cookie.
        /// </summary>
        /// <param name="name">Name of the cookie.</param>
        /// <param name="value">Value to be stored within the cookie.</param>
        /// <param name="days">Number of days the cookie is valid for.  A value of 
        /// -1 means it will be a session cookie and will expire when the user session ends.</param>
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

        #region Ip Address

        /// <summary>
        /// Retrieves the Ip address for the current user session.
        /// 
        /// Please note that this could be masked if the user is using a proxy service or something similar.
        /// </summary>
        /// <returns>string</returns>
        protected string GetIpAddress()
        {
            return (HttpContext.Connection.RemoteIpAddress.ToString());
        }

        #endregion Ip Address

        #region Growl

        /// <summary>
        /// Retreives a previously stored growl message.
        /// </summary>
        /// <returns>string.  A valid growl message if one exists, otherwise an empty string.</returns>
        protected string GrowlGet()
        {
            string Result = String.Empty;

            if (TempData.ContainsKey("growl"))
                Result = (string)TempData["growl"];

            return Result;
        }

        /// <summary>
        /// Adds a growl message which can be retrieved on the next request.
        /// </summary>
        /// <param name="s"></param>
        protected void GrowlAdd(string s)
        {
            TempData["growl"] = s;
        }

        #endregion Growl

        #region Pagination

        /// <summary>
        /// Builds a paginated list of html li elements for display in a view where pages are required.
        /// </summary>
        /// <param name="itemCount">int.  Number of items.</param>
        /// <param name="itemsPerPage">int.  Number of items per page.</param>
        /// <param name="currentPage">int.  Current page number.</param>
        /// <param name="page">string.  Page or route being used to obtain pagination</param>
        /// <param name="parameters">string.  Parameters to be added to each page item.</param>
        /// <param name="previous">string.  Display text (localized or not) to be shown to indicate previous page.</param>
        /// <param name="next">string.  Display text (localized or not) to be shown to indicate next page.</param>
        /// <returns>string.  List of html li elements for pagination within a view.</returns>
        protected string BuildPagination(in int itemCount, in int itemsPerPage, in int currentPage,
            in string page, in string parameters, in string previous, in string next)
        {
            string Result = "";
            int pageCount = CheckMinMax(RoundUp(itemCount, itemsPerPage), 1, int.MaxValue);

            string paginationParameters = parameters;

            if (paginationParameters != "")
            {
                if (paginationParameters[0] != '&')
                    paginationParameters = "&" + paginationParameters;
            }

            if (currentPage == 1 || pageCount == 1)
                Result += String.Format("<li class=\"disabled\"><a href=\"javascript: void(0)\">&laquo; {0}</a></li>", previous);
            else
                Result += String.Format("<li><a href=\"{0}Page/{1}/{2}\">&laquo; {3}</a></li>", page, currentPage - 1, paginationParameters, previous);

            //can only allow max of 7 items normal page and 5 for mobile
            int startFrom = 1;
            int endAt = pageCount;
            int maxAllowed = GetUserSession().MobileRedirect ? 5 : 7;
            int currPage = maxAllowed == 5 ? 2 : 4;
            int pageOffset = maxAllowed == 5 ? 1 : 3;

            if (pageCount > maxAllowed)
            {
                if (currentPage > currPage)
                {
                    if (currentPage >= pageCount)
                    {
                        startFrom = pageCount - maxAllowed;
                        endAt = pageCount;
                    }
                    else
                    {
                        startFrom = currentPage - pageOffset;
                        endAt = currentPage + pageOffset;

                        if (endAt > pageCount)
                        {
                            startFrom = pageCount - maxAllowed;
                            endAt = pageCount;
                        }
                    }
                }
                else
                {
                    startFrom = 1;
                    endAt = maxAllowed;
                }
            }

            for (int i = startFrom; i <= endAt; i++)
            {
                if (i == currentPage)
                    Result += String.Format("<li class=\"current\"><a href=\"{0}Page/{1}/{2}\">{1}</a></li>", page, i, paginationParameters);
                else
                    Result += String.Format("<li><a href=\"{0}Page/{1}/{2}\">{1}</a></li>", page, i, paginationParameters);
            }

            if (currentPage >= pageCount)
                Result += String.Format("<li class=\"disabled\"><a href=\"javascript: void(0)\">{0} &raquo;</a></li>", next);
            else
                Result += String.Format("<li><a href=\"{0}Page/{1}/{2}\">{3} &raquo;</a></li>", page, currentPage + 1, paginationParameters, next);

            return Result;
        }

        #endregion Pagination
    }
}
