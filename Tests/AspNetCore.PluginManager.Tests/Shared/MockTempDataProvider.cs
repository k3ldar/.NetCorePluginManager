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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockActionDescriptorCollectionProvider.cs
 *
 *  Purpose:  Mock IActionDescriptorCollectionProvider class
 *
 *  Date        Name                Reason
 *  21/02/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace AspNetCore.PluginManager.Tests
{
    [ExcludeFromCodeCoverage]
    public class MockTempDataProvider : ITempDataProvider
    {
        private readonly Dictionary<string, object> _tempData;

        public MockTempDataProvider()
        {
            _tempData = new Dictionary<string, object>();
        }

        public IDictionary<string, object> LoadTempData(HttpContext context)
        {
            return _tempData;
        }

        public void SaveTempData(HttpContext context, IDictionary<string, object> values)
        {
            foreach (var item in values)
            {
                _tempData.Add(item.Key, item.Value);
            }
        }
    }
}
