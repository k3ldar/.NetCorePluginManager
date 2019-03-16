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
    public class BaseController : Controller
    {
        #region User Sessions

        protected UserSession GetUserSession()
        {
            if (HttpContext.Items.ContainsKey(Constants.UserSession))
            {
                return ((UserSession)HttpContext.Items[Constants.UserSession]);
            }

            return (null);
        }

        protected bool IsUserLoggedIn()
        {
            UserSession session = GetUserSession();

            if (session != null)
                return (!String.IsNullOrEmpty(session.UserEmail));

            return (false);
        }

        protected Int64 UserId()
        {
            UserSession session = GetUserSession();

            if (session != null)
                return session.UserID;

            return (-1);
        }

        protected string GetCoreSessionId()
        {
            return (HttpContext.Session.Id);
        }

        #endregion User Sessions

        #region Breadcrumbs

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
                IShoppingCartService shoppingCartService = (IShoppingCartService)HttpContext.RequestServices.GetService(typeof(IShoppingCartService));

                if (shoppingCartService != null)
                    return shoppingCartService.GetSummary(userSession.UserBasketId);
            }

            return new ShoppingCartSummary(0, 0, 0, System.Threading.Thread.CurrentThread.CurrentUICulture);
        }

        protected long GetShoppingCartId()
        {
            if (HttpContext.Items.ContainsKey(Constants.ShoppingCart))
            {
                return (long)HttpContext.Items[Constants.ShoppingCart];
            }

            return (0);
        }

        protected void AddToCart()
        {

        }

        #endregion Shopping Cart

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

        #region Ip Address

        protected string GetIpAddress()
        {
            return (HttpContext.Connection.RemoteIpAddress.ToString());
        }

        #endregion Ip Address

        #region Growl

        protected string GrowlGet()
        {
            string Result = String.Empty;

            if (TempData.ContainsKey("growl"))
                Result = (string)TempData["growl"];

            return Result;
        }

        protected void GrowlAdd(string s)
        {
            TempData["growl"] = s;
        }

        #endregion Growl

        #region Pagination

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
