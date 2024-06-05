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
 *  Product:  PluginManager.Tests
 *  
 *  File: NotificationEventTests.cs
 *
 *  Purpose:  Tests for notification service
 *
 *  Date        Name                Reason
 *  26/07/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Tests.Mocks;

using Shared.Classes;

namespace PluginManager.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ThreadManagerServicesTests
    {
        private const string TestCategoryName = "Plugin Manager";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterStartupThread_InvalidThreadName_Null_Throws_ArgumentNullException()
        {
            using (MockPluginManager pluginManager = new())
            {
                pluginManager.RegisterStartupThread(null, typeof(TestThreadManager));
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterStartupThread_InvalidThreadName_EmptyString_Throws_ArgumentNullException()
        {
            using (MockPluginManager pluginManager = new())
            {
                pluginManager.RegisterStartupThread("", typeof(TestThreadManager));
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterStartupThread_InvalidType_DoesNotDescendFromThreadManager_Throws_ArgumentException()
        {
            using (MockPluginManager pluginManager = new())
            {
                pluginManager.RegisterStartupThread("My thread", this.GetType());
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RegisterStartupThread_ThreadNameAlreadyExists_Throws_InvalidOperationException()
        {
            using (MockPluginManager pluginManager = new())
            {
                pluginManager.RegisterStartupThread("test", typeof(TestThreadManager));
                pluginManager.RegisterStartupThread("test", typeof(TestThreadManager));
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void RegisterStartupThread_RegistersThreadForStartingAfterConfiguration_Success()
        {
            using (MockPluginManager pluginManager = new())
            {
                pluginManager.RegisterStartupThread("test", typeof(TestThreadManager));

                Assert.IsTrue(pluginManager.ContainsRegisteredStartupThread("test", typeof(TestThreadManager)));
            }
        }
    }

    [ExcludeFromCodeCoverage]
    public class TestThreadManager : ThreadManager
    {
        public TestThreadManager()
            : base (null, new TimeSpan())
        {

        }

        protected override bool Run(object parameters)
        {
            return false;
        }
    }
}
