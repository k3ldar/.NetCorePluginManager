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
 *  File: JsonResponseModelTests.cs
 *
 *  Purpose:  Tests for JsonResponseModel
 *
 *  Date        Name                Reason
 *  05/05/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.SharedPluginFeatures.Classes
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CachedImageUploadTests
    {
        private const string Category = "SharedPluginFeatures";

        [TestMethod]
        [TestCategory(Category)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_GroupName_Null_Throws_ArgumentNullException()
        {
            CachedImageUpload sut = new CachedImageUpload(null, "subgroup");
        }

        [TestMethod]
        [TestCategory(Category)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_GroupName_EmptyString_Throws_ArgumentNullException()
        {
            CachedImageUpload sut = new CachedImageUpload("", "subgroup");
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Construct_ValidInstance_WithSubgroup_Success()
        {
            CachedImageUpload sut = new CachedImageUpload("My group", "subgroup");
            sut.MemoryCacheName = "test 123";
            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Files);
            Assert.IsNotNull(sut.GroupName);
            Assert.AreNotEqual("", sut.GroupName);
            Assert.IsNotNull(sut.SubgroupName);
            Assert.AreNotEqual("", sut.SubgroupName);
            Assert.AreEqual("test 123", sut.MemoryCacheName);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Construct_ValidInstance_WithoutSubgroup_Success()
        {
            CachedImageUpload sut = new CachedImageUpload("My group");
            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Files);
            Assert.IsNotNull(sut.GroupName);
            Assert.AreNotEqual("", sut.GroupName);
            Assert.IsNull(sut.SubgroupName);
            Assert.IsNull(sut.MemoryCacheName);
        }
    }
}
