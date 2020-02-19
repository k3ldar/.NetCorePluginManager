using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.PluginFeatures
{
    [TestClass]
    public class BaseControllerTests
    {
        [TestMethod]
        public void CreateBaseController()
        {
            using (BaseControllerWrapper baseController = new BaseControllerWrapper())
            {
                Assert.IsNotNull(baseController);
            }
        }

        [TestMethod]
        public void GetSessionId()
        {
            using (BaseControllerWrapper baseController = new BaseControllerWrapper())
            {
                string sessionId = baseController.TestGetCoreSessionId();
                Assert.IsNotNull(sessionId);
                Assert.AreEqual("abc123", sessionId);
            }
        }

        [TestMethod]
        public void GetShoppingCartSummary()
        {
            using (BaseControllerWrapper baseController = new BaseControllerWrapper())
            {
                ShoppingCartSummary cart = baseController.TestGetShoppingCartSummary();
                Assert.IsNotNull(cart);
            }
        }
    }
}
