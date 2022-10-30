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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: ProductGroupProductCountsTests.cs
 *
 *  Purpose:  Tests for ProductGroupProductCountsTests class
 *
 *  Date        Name                Reason
 *  30/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shared.Classes;
using ProductPlugin.Classes;

namespace AspNetCore.PluginManager.Tests.Plugins.ProductTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ProductGroupProductCountsTests : GenericBaseClass
    {
        private const string TestCategoryName = "Product Manager Tests";


        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ProductProvider_Null_Throws_ArgumentNullException()
        {
            new ProductGroupProductCounts(null, new List<string>());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ProductPriceInfo_Null_Throws_ArgumentNullException()
        {
            new ProductGroupProductCounts(new MockProductProvider(), null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Run_ProcessesAllProducts()
        {
            List<string> priceGroups = new List<string>()
            {
                "Main Products",
                "Other Products",
                "None Found"
            };
            MockProductProvider productProvider = new MockProductProvider();

            ProductGroupProductCounts sut = new ProductGroupProductCounts(productProvider, priceGroups);

            ThreadManager.Initialise();
            try
            {
                ThreadManager.ThreadStart(sut, "Test Run", System.Threading.ThreadPriority.Highest, false);

                Thread.Sleep(100);
                DateTime start = DateTime.Now;

                while (ThreadManager.Exists("Test Run"))
                {
                    Thread.Sleep(200);

                    TimeSpan timeTaken = DateTime.Now - start;

                    if (timeTaken.TotalSeconds > 1)
                        throw new AssertInconclusiveException("Timed out waiting for response");
                }

                Assert.AreEqual("Main Products (3)", priceGroups[0]);
                Assert.AreEqual("Other Products (6)", priceGroups[1]);
                Assert.AreEqual("None Found", priceGroups[2]);
            }
            finally
            {
                ThreadManager.Cancel("Test Run");
                ThreadManager.Finalise();
            }
        }
    }
}
