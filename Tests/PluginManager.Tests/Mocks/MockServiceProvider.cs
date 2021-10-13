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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.Tests
 *  
 *  File: MockServiceProvider.cs
 *
 *  Purpose:  Mock IServiceProvider class
 *
 *  Date        Name                Reason
 *  10/08/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PluginManager.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class MockServiceProvider : IServiceProvider
    {
        private readonly Dictionary<Type, object> _services;

        public MockServiceProvider(Dictionary<Type, object> services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public object GetService(Type serviceType)
        {
            if (_services.ContainsKey(serviceType))
                return _services[serviceType];

            return null;
        }
    }
}
