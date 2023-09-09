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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: DynamicContentThreadManagerTests.cs
 *
 *  Purpose:  Tests DynamicContentThreadManager
 *
 *  Date        Name                Reason
 *  22/07/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;

using AspNetCore.PluginManager.DemoWebsite.Classes.Mocks;
using AspNetCore.PluginManager.Tests.Shared;

using DynamicContent.Plugin.Internal;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.DynamicContentTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
	[DoNotParallelize]
	public class DynamicContentThreadManagerTests : GenericBaseClass
    {
        private const string TestCategoryName = "Dynamic Content";
        private string _currentTestPath = null;

        [TestInitialize]
        public void TestInitialize()
        {
            ThreadManager.Initialise();
            _currentTestPath = TestHelper.GetTestPath();

            while (Directory.Exists(_currentTestPath))
            {
                Thread.Sleep(10);
                _currentTestPath = TestHelper.GetTestPath();
            }
        }

        [TestCleanup]
        public void TestFinalize()
        {
            ThreadManager.Finalise();
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_NotificationServer_Null_Throws_ArgumentNullException()
        {
            MockDynamicContentProvider dynamicContentProvider = new MockDynamicContentProvider(new MockPluginClassesService(), false);

            new DynamicContentThreadManager(null, dynamicContentProvider);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidParam_DynamicContentProvider_Null_Throws_ArgumentNullException()
        {
            INotificationService notificationService = new MockNotificationService();

            new DynamicContentThreadManager(notificationService, null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            INotificationService notificationService = new MockNotificationService();
            MockDynamicContentProvider dynamicContentProvider = new MockDynamicContentProvider(new MockPluginClassesService(), false);

            DynamicContentThreadManager sut = new DynamicContentThreadManager(notificationService, dynamicContentProvider);

            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.UpdateRequired);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetEvents_HasCorrectEventsRegistered_Success()
        {
            INotificationService notificationService = new MockNotificationService();
            MockDynamicContentProvider dynamicContentProvider = new MockDynamicContentProvider(new MockPluginClassesService(), false);

            DynamicContentThreadManager sut = new DynamicContentThreadManager(notificationService, dynamicContentProvider);

            List<string> events = sut.GetEvents();
            Assert.AreEqual(1, events.Count);
            Assert.AreEqual("DynamicContentUpdated", events[0]);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_WithBoolResult_HasCorrectEvents_ReturnsTrue()
        {
            INotificationService notificationService = new MockNotificationService();
            MockDynamicContentProvider dynamicContentProvider = new MockDynamicContentProvider(new MockPluginClassesService(), false);

            DynamicContentThreadManager sut = new DynamicContentThreadManager(notificationService, dynamicContentProvider);
            object returnValue = null;
            Assert.IsTrue(sut.EventRaised("DynamicContentUpdated", null, null, ref returnValue));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_WithBoolResult_InvalidEvent_ReturnsFalse()
        {
            INotificationService notificationService = new MockNotificationService();
            MockDynamicContentProvider dynamicContentProvider = new MockDynamicContentProvider(new MockPluginClassesService(), false);

            DynamicContentThreadManager sut = new DynamicContentThreadManager(notificationService, dynamicContentProvider);
            object returnValue = null;
            Assert.IsFalse(sut.EventRaised("Do Something", null, null, ref returnValue));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_UpdateRequiredSetToTrue_Success()
        {
            INotificationService notificationService = new MockNotificationService();
            MockDynamicContentProvider dynamicContentProvider = new MockDynamicContentProvider(new MockPluginClassesService(), false);

            DynamicContentThreadManager sut = new DynamicContentThreadManager(notificationService, dynamicContentProvider);
            ThreadManager.ThreadStart(sut, nameof(DynamicContentThreadManager), ThreadPriority.Normal);
            try
            {
                Thread.Sleep(300);
                Assert.IsFalse(sut.UpdateRequired);
            }
            finally
            {
                sut.CancelThread();
            }

            sut.EventRaised("DynamicContentUpdated", null, null);
            Assert.IsTrue(sut.UpdateRequired);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Run_UpdatesUpdateRequired_Success()
        {
            INotificationService notificationService = new MockNotificationService();
            MockDynamicContentProvider dynamicContentProvider = new MockDynamicContentProvider(new MockPluginClassesService(), false);

            DynamicContentThreadManager sut = new DynamicContentThreadManager(notificationService, dynamicContentProvider);
            Assert.IsTrue(sut.UpdateRequired);

            ThreadManager.ThreadStart(sut, nameof(DynamicContentThreadManager), ThreadPriority.Normal);
            try
            {
                Thread.Sleep(300);
                Assert.IsFalse(sut.UpdateRequired);
            }
            finally
            {
                sut.CancelThread();
            }

        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Run_CorrectlySetsNextUpdateTime_BasedOnPageBecomingActive_Success()
        {
            DateTime currentDate = DateTime.Now.AddSeconds(1);

            INotificationService notificationService = new MockNotificationService();
            MockDynamicContentProvider dynamicContentProvider = new MockDynamicContentProvider(new MockPluginClassesService(), false);

            IDynamicContentPage page1 = GetPage1();
            page1.ActiveFrom = currentDate;
            page1.ActiveTo = currentDate.AddDays(10);
            dynamicContentProvider.AddPage(page1);

            DynamicContentThreadManager sut = new DynamicContentThreadManager(notificationService, dynamicContentProvider);
            Assert.IsTrue(sut.UpdateRequired);

            ThreadManager.ThreadStart(sut, nameof(DynamicContentThreadManager), ThreadPriority.Normal);
            try
            {
                Thread.Sleep(300);
                Assert.IsFalse(sut.UpdateRequired);
                Assert.AreEqual(currentDate, sut.NextUpdate);
            }
            finally
            {
                sut.CancelThread();
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Run_CorrectlySetsNextUpdateTime_BasedOnPageDeactivating_Success()
        {
            DateTime currentDate = DateTime.Now.AddSeconds(10);

            INotificationService notificationService = new MockNotificationService();
            MockDynamicContentProvider dynamicContentProvider = new MockDynamicContentProvider(new MockPluginClassesService(), false);

            IDynamicContentPage page1 = GetPage1();
            page1.ActiveFrom = currentDate;
            page1.ActiveTo = currentDate.AddDays(10);
            dynamicContentProvider.AddPage(page1);

            DynamicContentThreadManager sut = new DynamicContentThreadManager(notificationService, dynamicContentProvider);
            Assert.IsTrue(sut.UpdateRequired);

            ThreadManager.ThreadStart(sut, nameof(DynamicContentThreadManager), ThreadPriority.Normal);
            try
            {
                Thread.Sleep(300);
                Assert.IsFalse(sut.UpdateRequired);
                Assert.AreEqual(currentDate, sut.NextUpdate);
            }
            finally
            {
                sut.CancelThread();
            }

        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
		public void Run_ProcessesDynamicPages_WhenNewPageBecomesActive_Success()
        {
            DateTime currentDate = DateTime.Now.AddSeconds(1);

            INotificationService notificationService = new MockNotificationService();
            MockDynamicContentProvider dynamicContentProvider = new MockDynamicContentProvider(new MockPluginClassesService(), false);

            IDynamicContentPage page1 = GetPage1();
            page1.ActiveFrom = currentDate.AddDays(-10);
            page1.ActiveTo = currentDate.AddDays(10);
            dynamicContentProvider.AddPage(page1);

            IDynamicContentPage page2 = GetPage2();
            page2.ActiveFrom = currentDate;
            page2.ActiveTo = currentDate.AddDays(5);
            dynamicContentProvider.AddPage(page2);

            IDynamicContentPage page3 = GetPage3();
            page3.ActiveFrom = currentDate.AddDays(-10);
            page3.ActiveTo = currentDate.AddDays(-5);
            dynamicContentProvider.AddPage(page3);

            DynamicContentThreadManager sut = new DynamicContentThreadManager(notificationService, dynamicContentProvider);
            Assert.IsTrue(sut.UpdateRequired);

            ThreadManager.ThreadStart(sut, nameof(DynamicContentThreadManager), ThreadPriority.Normal);
            try
            {
                Thread.Sleep(250);
                Assert.IsFalse(sut.UpdateRequired);
                Assert.AreEqual(page2.ActiveFrom, sut.NextUpdate);
                Assert.AreEqual(1, sut.CacheCount);
                Assert.AreEqual(1u, sut.UpdateContentTimings.Requests);

                Thread.Sleep(1200);
                Assert.IsFalse(sut.UpdateRequired);
                Assert.AreEqual(page2.ActiveTo, sut.NextUpdate);
                Assert.AreEqual(2, sut.CacheCount);
                Assert.AreEqual(2u, sut.UpdateContentTimings.Requests);

                // expire first page
                page1.ActiveTo = currentDate.AddDays(-10);
                sut.EventRaised("DynamicContentUpdated", null, null);
                Assert.IsTrue(sut.UpdateRequired);

                Thread.Sleep(600);
                Assert.IsFalse(sut.UpdateRequired);
                Assert.AreEqual(page2.ActiveTo, sut.NextUpdate);
                Assert.AreEqual(1, sut.CacheCount);
                Assert.AreEqual(3u, sut.UpdateContentTimings.Requests);
            }
            finally
            {
                sut.CancelThread();
            }
        }
    }
}
