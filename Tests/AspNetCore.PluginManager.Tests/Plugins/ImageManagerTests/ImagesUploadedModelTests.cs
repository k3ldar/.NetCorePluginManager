/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: ImagesUploadedModelTests.cs
 *
 *  Purpose:  Unit tests for ImagesUploadedModel
 *
 *  Date        Name                Reason
 *  20/05/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImageManager.Plugin.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ImagesUploadedModelTests
    {
        private const string ImageManagerTestsCategory = "Image Manager Tests";

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_GroupName_Null_Throws_ArgumentNullException()
        {
            ImagesUploadedModel sut = new ImagesUploadedModel(null, null, "cache");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_GroupName_EmptyString_Throws_ArgumentNullException()
        {
            ImagesUploadedModel sut = new ImagesUploadedModel(null, null, "cache");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_MemoryCacheName_Null_Throws_ArgumentNullException()
        {
            ImagesUploadedModel sut = new ImagesUploadedModel("group", null, null);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_MemoryCacheName_EmptyString_Throws_ArgumentNullException()
        {
            ImagesUploadedModel sut = new ImagesUploadedModel("group", null, "");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidInstance_NullSubGroup_Success()
        {
            ImagesUploadedModel sut = new ImagesUploadedModel("group", null, "memCache");
            Assert.IsNotNull(sut);
            Assert.AreEqual("group", sut.GroupName);
            Assert.AreEqual("memCache", sut.MemoryCacheName);
            Assert.IsNull(sut.SubgroupName);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidInstance_ValidSubGroup_Success()
        {
            ImagesUploadedModel sut = new ImagesUploadedModel("group", "sub", "memCache");
            Assert.IsNotNull(sut);
            Assert.AreEqual("group", sut.GroupName);
            Assert.AreEqual("sub", sut.SubgroupName);
            Assert.AreEqual("memCache", sut.MemoryCacheName);
        }
    }
}