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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: BaseMiddlewareWrapper.cs
 *
 *  Purpose:  Tests wrappers for BaseMiddleware tests
 *
 *  Date        Name                Reason
 *  13/02/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using Shared.Classes;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.SharedPluginFeatures
{
    [ExcludeFromCodeCoverage]
    public class BaseMiddlewareWrapper : BaseMiddleware
    {
        public Uri TestGetCurrentUri(HttpContext context)
        {
            return GetCurrentUri(context);
        }

        public string TestGetHost(HttpContext context)
        {
            return GetHost(context);
        }

        public ITempDataDictionary TestGetTempData(HttpContext context)
        {
            return GetTempData(context);
        }

        public UserSession TestGetUserSession(HttpContext context)
        {
            return GetUserSession(context);
        }

        public bool TestIsUserLoggedIn(HttpContext context)
        {
            return IsUserLoggedIn(context);
        }

        public string TestRoute(HttpContext context)
        {
            return Route(context);
        }

        public string TestRouteLowered(HttpContext context)
        {
            return RouteLowered(context);
        }
    }
}
