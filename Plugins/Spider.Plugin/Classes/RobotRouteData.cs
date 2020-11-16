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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: SpiderMiddlewareTests.cs
 *
 *  Purpose:  Test units for MVC Spider Middleware class
 *
 *  Date        Name                Reason
 *  18/10/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

#pragma warning disable CS1591, CA1303

namespace Spider.Plugin.Classes
{
    public class RobotRouteData : IRobotRouteData
    {
        public RobotRouteData()
        {

        }

        public RobotRouteData(string agent, string comment, string route, bool allowed, bool isCustom)
        {
            if (String.IsNullOrEmpty(agent))
                throw new ArgumentNullException(nameof(agent));

            if (String.IsNullOrEmpty(route))
                throw new ArgumentNullException(nameof(route));

            if (!Uri.TryCreate(route, UriKind.Relative, out _))
                throw new ArgumentException("route must be a partial Uri", nameof(route));

            Agent = agent;
            Comment = comment ?? String.Empty;
            Route = route;
            Allowed = allowed;
            IsCustom = isCustom;
        }

        public string Agent { get; set; }

        public string Comment { get; set; }

        public string Route { get; set; }

        public bool Allowed { get; set; }

        public bool IsCustom { get; set; }
    }
}

#pragma warning restore CS1591, CA1303