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
 *  Copyright (c) 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: GCAnalysisTests.cs
 *
 *  Purpose:  Test units for garbage collection analysis class
 *
 *  Date        Name                Reason
 *  15/09/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemAdmin.Plugin.Classes;

namespace AspNetCore.PluginManager.Tests.Plugins.SystemAdminTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class GCAnalysisTests
    {
        private const string TestsCategory = "SystemAdmin Tests";

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void GCAnalysis_Construct_Success()
        {
            GCAnalysis sut = new GCAnalysis();
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void GCAnalysis_RetrieveGCData_RetrievesInstantiatedList_Success()
        {
            GCAnalysis sut = new GCAnalysis();
            List<GCSnapshot> snapshot = GCAnalysis.RetrieveGCData();

            Assert.IsNotNull(snapshot);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void GCSnapshot_Construct_ValidInstance_Success()
        {
            DateTime createDate = DateTime.UtcNow;
            GCSnapshot sut = new GCSnapshot(1.23D, 568);
            Assert.IsNotNull(sut);

            TimeSpan span = sut.TimeStarted - createDate;

            Assert.AreEqual(1.23D, sut.TimeTaken);
            Assert.AreEqual(568, sut.MemorySaved);
            Assert.IsTrue(span.TotalMilliseconds > 0);
            Assert.IsTrue(span.TotalMilliseconds < 300);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void GCSnapshot_Construct_InvalidMemorySaved_Success()
        {
            DateTime createDate = DateTime.UtcNow;
            GCSnapshot sut = new GCSnapshot(1.23D, -12324);
            Assert.IsNotNull(sut);

            TimeSpan span = sut.TimeStarted - createDate;

            Assert.AreEqual(1.23D, sut.TimeTaken);
            Assert.AreEqual(12324, sut.MemorySaved);
            Assert.IsTrue(span.TotalMilliseconds > 0);
            Assert.IsTrue(span.TotalMilliseconds < 300);
        }
    }
}
