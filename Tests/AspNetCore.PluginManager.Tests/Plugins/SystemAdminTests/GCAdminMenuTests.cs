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
 *  File: GCAdminMenuTests.cs
 *
 *  Purpose:  Tests for garbage collection admin menu
 *
 *  Date        Name                Reason
 *  03/10/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shared.Classes;

using SharedPluginFeatures;

using SystemAdmin.Plugin.Classes;
using SystemAdmin.Plugin.Classes.MenuItems;

namespace AspNetCore.PluginManager.Tests.Plugins.SystemAdminTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class GCAdminMenuTests
    {
        private const string TestCategoryName = "System Admin";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void CreateValidInstance__Success()
        {
            ThreadManager.Initialise();
            try
            {
                GCAnalysis gcAnalysis = new GCAnalysis();
                ThreadManager.ThreadStart(gcAnalysis, "test GC", System.Threading.ThreadPriority.AboveNormal);
                    
                Version version = null;

                for (int I = 0; I < 100000; I++)
                {
                    version = new Version();
                }

                GC.Collect(2, GCCollectionMode.Forced, false);
                DateTime dateTime = DateTime.Now.AddSeconds(2);

                while (DateTime.Now < dateTime)
                {
                    Thread.Sleep(20);
                }



                GCAdminMenu sut = new GCAdminMenu();

                Assert.IsInstanceOfType(sut, typeof(SystemAdminSubMenu));
                Assert.IsNull(sut.Action());
                Assert.IsNull(sut.Area());
                Assert.IsNull(sut.Controller());
                Assert.IsNull(sut.Image());
                Assert.AreEqual(Enums.SystemAdminMenuType.Grid, sut.MenuType());
                Assert.AreEqual("GC Timings", sut.Name());
                Assert.AreEqual(0, sut.SortOrder());
                Assert.AreEqual("System", sut.ParentMenuName());
                Assert.AreEqual("Date/Time|Duration|Memory Reclaimed\r", sut.Data());
            }
            finally
            {
                ThreadManager.CancelAll();
                ThreadManager.Finalise();
            }
        }
    }
}
