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
        private List<int> CreateList(in int count)
        {
            Random random = new Random();

            List<int> Result = new List<int>();

            while (Result.Count < count)
                Result.Add(random.Next(0, Int32.MaxValue));

            return Result;
        }

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

        [TestMethod]
        public void CalucalatePageOffsets_Page1_PerPage5_13Items()
        {
            using (BaseControllerWrapper baseController = new BaseControllerWrapper())
            {
                baseController.TestCalculatePageOffsets(13, 1, 5, out int startItem, out int endItem, out int availablePages);

                Assert.AreEqual(1, startItem);
                Assert.AreEqual(5, endItem);
                Assert.AreEqual(3, availablePages);
            }
        }

        [TestMethod]
        public void CalucalatePageOffsets_Page8_PerPage5_13Items()
        {
            using (BaseControllerWrapper baseController = new BaseControllerWrapper())
            {
                baseController.TestCalculatePageOffsets(13, 8, 5, out int startItem, out int endItem, out int availablePages);

                Assert.AreEqual(11, startItem);
                Assert.AreEqual(13, endItem);
                Assert.AreEqual(3, availablePages);
            }
        }

        [TestMethod]
        public void CalucalatePageOffsets_Page1_PerPage50_3Items()
        {
            using (BaseControllerWrapper baseController = new BaseControllerWrapper())
            {
                baseController.TestCalculatePageOffsets(3, 1, 50, out int startItem, out int endItem, out int availablePages);

                Assert.AreEqual(1, startItem);
                Assert.AreEqual(3, endItem);
                Assert.AreEqual(1, availablePages);
            }
        }

        [TestMethod]
        public void CalucalatePageOffsets_Page4_PerPage50_7328Items()
        {
            using (BaseControllerWrapper baseController = new BaseControllerWrapper())
            {
                baseController.TestCalculatePageOffsets(7328, 4, 50, out int startItem, out int endItem, out int availablePages);

                Assert.AreEqual(151, startItem);
                Assert.AreEqual(200, endItem);
                Assert.AreEqual(147, availablePages);
            }
        }

        [TestMethod]
        public void CalucalatePageOffsets_Page147_PerPage50_7328Items()
        {
            using (BaseControllerWrapper baseController = new BaseControllerWrapper())
            {
                baseController.TestCalculatePageOffsets(7328, 147, 50, out int startItem, out int endItem, out int availablePages);

                Assert.AreEqual(7301, startItem);
                Assert.AreEqual(7328, endItem);
                Assert.AreEqual(147, availablePages);
            }
        }

        [TestMethod]
        public void CalucalatePageOffsets_List_Page1_PerPage5_13Items()
        {
            using (BaseControllerWrapper baseController = new BaseControllerWrapper())
            {
                baseController.TestCalculatePageOffsets<int>(CreateList(13), 1, 5, out int startItem, out int endItem, out int availablePages);

                Assert.AreEqual(0, startItem);
                Assert.AreEqual(4, endItem);
                Assert.AreEqual(3, availablePages);
            }
        }

        [TestMethod]
        public void CalucalatePageOffsets_List_Page8_PerPage5_13Items()
        {
            using (BaseControllerWrapper baseController = new BaseControllerWrapper())
            {
                baseController.TestCalculatePageOffsets<int>(CreateList(13), 8, 5, out int startItem, out int endItem, out int availablePages);

                Assert.AreEqual(10, startItem);
                Assert.AreEqual(12, endItem);
                Assert.AreEqual(3, availablePages);
            }
        }

        [TestMethod]
        public void CalucalatePageOffsets_List_Page1_PerPage50_3Items()
        {
            using (BaseControllerWrapper baseController = new BaseControllerWrapper())
            {
                baseController.TestCalculatePageOffsets<int>(CreateList(3), 1, 50, out int startItem, out int endItem, out int availablePages);

                Assert.AreEqual(0, startItem);
                Assert.AreEqual(2, endItem);
                Assert.AreEqual(1, availablePages);
            }
        }

        [TestMethod]
        public void CalucalatePageOffsets_List_Page4_PerPage50_7328Items()
        {
            using (BaseControllerWrapper baseController = new BaseControllerWrapper())
            {
                baseController.TestCalculatePageOffsets<int>(CreateList(7328), 4, 50, out int startItem, out int endItem, out int availablePages);

                Assert.AreEqual(150, startItem);
                Assert.AreEqual(199, endItem);
                Assert.AreEqual(147, availablePages);
            }
        }

        [TestMethod]
        public void CalucalatePageOffsets_List_Page147_PerPage50_7328Items()
        {
            using (BaseControllerWrapper baseController = new BaseControllerWrapper())
            {
                baseController.TestCalculatePageOffsets<int>(CreateList(7328), 147, 50, out int startItem, out int endItem, out int availablePages);

                Assert.AreEqual(7300, startItem);
                Assert.AreEqual(7327, endItem);
                Assert.AreEqual(147, availablePages);
            }
        }
    }
}
