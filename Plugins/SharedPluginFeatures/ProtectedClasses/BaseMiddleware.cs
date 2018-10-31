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
 *  File: BaseMiddleware.cs
 *
 *  Purpose:  Base Middleware Class
 *
 *  Date        Name                Reason
 *  19/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

namespace SharedPluginFeatures
{
    public class BaseMiddleware : BaseCoreClass
    {
        #region Constants

        private const string LoweredRoute = "RouteLowered";
        private const string ExtensionLowered = "ExtensionLowered";

        #endregion Constants

        #region Protected Methods

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

        #endregion Protected Methods
    }
}
