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
 *  File: IpManagementTests.cs
 *
 *  Purpose:  Test units for ip management
 *
 *  Date        Name                Reason
 *  13/10/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BadEgg.Plugin;
using BadEgg.Plugin.WebDefender;

namespace AspNetCore.PluginManager.Tests.Plugins.BadEggTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class IpManagementTests
    {
        private const string TestCategoryName = "BadEgg";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            IpManagement sut = new IpManagement();
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddBlackListedIp_InvalidParam_Null_Throws_ArgumentNullException()
        {
            IpManagement sut = new IpManagement();
            sut.AddBlackListedIp(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddBlackListedIp_InvalidParam_EmptyString_Throws_ArgumentNullException()
        {
            IpManagement sut = new IpManagement();
            sut.AddBlackListedIp("");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AddBlackListedIp_Success()
        {
            IpManagement sut = new IpManagement();
            sut.ClearAllIpAddresses();
            Assert.AreEqual(0, ValidateConnections.InternalIpAddressList.Count);
            sut.AddBlackListedIp("10.10.10.10");
            Assert.AreEqual(1, ValidateConnections.InternalIpAddressList.Count);
            Assert.IsTrue(ValidateConnections.InternalIpAddressList["10.10.10.10"]);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddWhiteListedIp_InvalidParam_Null_Throws_ArgumentNullException()
        {
            IpManagement sut = new IpManagement();
            sut.AddWhiteListedIp(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddWhiteListedIp_InvalidParam_EmptyString_Throws_ArgumentNullException()
        {
            IpManagement sut = new IpManagement();
            sut.AddWhiteListedIp("");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AddWhiteListedIp_Success()
        {
            IpManagement sut = new IpManagement();
            sut.ClearAllIpAddresses();
            Assert.AreEqual(0, ValidateConnections.InternalIpAddressList.Count);
            sut.AddWhiteListedIp("10.10.10.11");
            Assert.AreEqual(1, ValidateConnections.InternalIpAddressList.Count);
            Assert.IsFalse(ValidateConnections.InternalIpAddressList["10.10.10.11"]);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveIpAddress_InvalidParam_Null_Throws_ArgumentNullException()
        {
            IpManagement sut = new IpManagement();
            sut.RemoveIpAddress(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveIpAddress_InvalidParam_EmptyString_Throws_ArgumentNullException()
        {
            IpManagement sut = new IpManagement();
            sut.RemoveIpAddress("");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void RemoveIpAddress_Success()
        {
            IpManagement sut = new IpManagement();
            sut.ClearAllIpAddresses();
            Assert.AreEqual(0, ValidateConnections.InternalIpAddressList.Count);
            sut.AddWhiteListedIp("10.10.10.12");
            Assert.AreEqual(1, ValidateConnections.InternalIpAddressList.Count);
            Assert.IsFalse(ValidateConnections.InternalIpAddressList["10.10.10.12"]);
            sut.RemoveIpAddress("10.10.10.12");
            Assert.AreEqual(0, ValidateConnections.InternalIpAddressList.Count);
        }
    }
}
