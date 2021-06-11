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
 *  File: BaseControllerWrapper.cs
 *
 *  Purpose:  Tests wrappers for BaseController tests
 *
 *  Date        Name                Reason
 *  01/10/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Mvc;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.SharedPluginFeatures
{
    [ExcludeFromCodeCoverage]
    internal class TestBaseControllerWrapper : BaseController
    {
        public TestBaseControllerWrapper()
        {
            ControllerContext.HttpContext = new TestHttpContext();
        }

        internal string TestGetCoreSessionId()
        {
            return GetCoreSessionId();
        }

        internal ShoppingCartSummary TestGetShoppingCartSummary()
        {
            return GetCartSummary();
        }

        internal void TestCalculatePageOffsets<T>(List<T> items, int page, int pageSize,
            out int startItem, out int endItem, out int availablePages)
        {
            CalculatePageOffsets<T>(items, page, pageSize, out startItem, out endItem, out availablePages);
        }

        internal void TestCalculatePageOffsets(int totalItems, int page, int pageSize,
            out int startItem, out int endItem, out int availablePages)
        {
            CalculatePageOffsets(totalItems, page, pageSize, out startItem, out endItem, out availablePages);
        }

        internal JsonResult TestGenerateJsonErrorResponse(int statusCode, string jsonData)
        {
            return GenerateJsonErrorResponse(statusCode, jsonData);
        }

        internal JsonResult TestGenerateJsonSuccessResponse()
        {
            return GenerateJsonSuccessResponse();
        }

        internal JsonResult TestGenerateJsonSuccessResponse(object responseData)
        {
            return GenerateJsonSuccessResponse(responseData);
        }
    }
}
