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

namespace AspNetCore.PluginManager.Tests.SharedPluginFeatures
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class JsonResponseModelTests
    {
        private const string Category = "SharedPluginFeatures";

        [TestMethod]
        [TestCategory(Category)]
        public void Construct_DefaultConstructor_Success_ResultsFalse()
        {
            JsonResponseModel sut = new JsonResponseModel();

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
            Assert.AreEqual("", sut.ResponseData);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Construct_SuccessConstructor_Success_ResultsFalse()
        {
            JsonResponseModel sut = new JsonResponseModel(false);

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
            Assert.AreEqual("", sut.ResponseData);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Construct_SuccessConstructor_Success_ResultsTrue()
        {
            JsonResponseModel sut = new JsonResponseModel(true);

            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.Success);
            Assert.AreEqual("", sut.ResponseData);
        }

        [TestMethod]
        [TestCategory(Category)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_DataConstructor_InvalidParam_Null_Throws_ArgumentNullException()
        {
            JsonResponseModel sut = new JsonResponseModel(null);
        }

        [TestMethod]
        [TestCategory(Category)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_DataConstructor_InvalidParam_EmptyString_Throws_ArgumentNullException()
        {
            JsonResponseModel sut = new JsonResponseModel("");
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Construct_DataConstructor_Success_ResultsTrue()
        {
            JsonResponseModel sut = new JsonResponseModel("some data");

            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.Success);
            Assert.AreEqual("some data", sut.ResponseData);
        }

        [TestMethod]
        [TestCategory(Category)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_SuccessAndDataConstructor_InvalidParamData_Null_Throws_ArgumentNullException()
        {
            JsonResponseModel sut = new JsonResponseModel(true, null);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Construct_SuccessAndDataConstructor_ValidParamData_EmptyString_Success()
        {
            JsonResponseModel sut = new JsonResponseModel(true, "");
            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.Success);
            Assert.AreEqual("", sut.ResponseData);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Construct_SuccessAndDataConstructor_Success()
        {
            JsonResponseModel sut = new JsonResponseModel(false, "An error occurred");

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
            Assert.AreEqual("An error occurred", sut.ResponseData);
        }
    }
}
