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
 *  File: MockMvcBuilder.cs
 *
 *  Purpose:  Mock class for testing MvcBuilder
 *
 *  Date        Name                Reason
 *  09/09/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

using PluginManager.Tests.Mocks;

namespace AspNetCore.PluginManager.Tests.Shared
{
    class MockMvcBuilder : IMvcBuilder
    {
        MockServiceCollection _mockServiceCollection;

        public MockMvcBuilder(MockServiceCollection mockServiceCollection)
        {
            _mockServiceCollection = mockServiceCollection ?? throw new ArgumentNullException(nameof(mockServiceCollection));
        }

        public IServiceCollection Services => _mockServiceCollection;

        public ApplicationPartManager PartManager => throw new NotImplementedException();
    }
}
