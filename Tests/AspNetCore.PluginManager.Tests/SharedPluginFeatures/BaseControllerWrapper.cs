using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.PluginFeatures
{
    internal class BaseControllerWrapper : BaseController
    {
        public BaseControllerWrapper()
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
    }


}
