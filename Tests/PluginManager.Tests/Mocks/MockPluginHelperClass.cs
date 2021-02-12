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
 *  File: MockPluginHelperClass.cs
 *
 *  Purpose:  Requires IPluginHelperService as constructor param for testing
 *
 *  Date        Name                Reason
 *  16/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using PluginManager.Abstractions;

namespace PluginManager.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class MockPluginHelperClass
    {
        public MockPluginHelperClass(IPluginHelperService pluginHelperService)
        {
            PluginHelperService = pluginHelperService;
        }

        public IPluginHelperService PluginHelperService { get; private set; }
    }

    [ExcludeFromCodeCoverage]
    public class MockPluginHelperTest : MockPluginHelperClass
    {
        public MockPluginHelperTest(IPluginHelperService pluginHelperService)
            : base(pluginHelperService)
        {

        }
    }
}
