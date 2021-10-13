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
 *  Copyright (c) 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockBreadcrumbService.cs
 *
 *  Purpose:  Mock class for testing IBreadcrumbService
 *
 *  Date        Name                Reason
 *  01/10/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class MockBreadcrumbService : IBreadcrumbService
    {
        public MockBreadcrumbService()
        {
            RegisteredRoutes = new Dictionary<string, string>();
        }

        public Dictionary<string, string> RegisteredRoutes { get; }

        public void AddBreadcrumb(in string name, in string route, in bool hasParameters)
        {
            if (RegisteredRoutes.ContainsKey(name))
                throw new InvalidOperationException($"Route {name} is already registered");

            RegisteredRoutes.Add(name, route);
        }

        public void AddBreadcrumb(in string name, in string route, in string parentRoute, in bool hasParameters)
        {
            if (RegisteredRoutes.ContainsKey(name))
                throw new InvalidOperationException($"Route {name} is already registered");

            RegisteredRoutes.Add(name, route);
        }
    }
}
