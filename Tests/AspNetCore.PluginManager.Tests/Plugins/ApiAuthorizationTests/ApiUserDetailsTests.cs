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
 *  File: ApiAuthorizationServiceTests.cs
 *
 *  Purpose:  Tests helpers for ApiAuthorizationService
 *
 *  Date        Name                Reason
 *  18/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using ApiAuthorization.Plugin.Classes;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.ApiAuthorizationTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public sealed class ApiUserDetailsTests
    {
        private const string TestCategoryName = "Api Authorization";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ApiKey_Null_Throws_ArgumentNullException()
        {
            new ApiUserDetails(null, "token", "auth", 23, 12345);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ApiKey_EmptyString_Throws_ArgumentNullException()
        {
            new ApiUserDetails("", "token", "auth", 23, 12345);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Token_Null_Throws_ArgumentNullException()
        {
            new ApiUserDetails("apikey", null, "auth", 23, 12345);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Token_EmptyString_Throws_ArgumentNullException()
        {
            new ApiUserDetails("apikey", "", "auth", 23, 12345);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Authorization_Null_Throws_ArgumentNullException()
        {
            new ApiUserDetails("apikey", "token", null, 23, 12345);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Authorization_EmptyString_Throws_ArgumentNullException()
        {
            new ApiUserDetails("apikey", "token", "", 23, 12345);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Construct_InvalidParam_Nonce_LessThanOne_Throws_ArgumentOutOfRangeException()
        {
            new ApiUserDetails("apikey", "token", "auth", 0, 12345);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(OverflowException))]
        public void Construct_InvalidParam_Epoch_TooBig_Throws_ArgumentOutOfRangeException()
        {
            new ApiUserDetails("apikey", "token", "auth", 12345, Int64.MaxValue);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            ApiUserDetails sut = new ApiUserDetails("apikey", "token", "auth", 12345, 
                new DateTime(DateTime.UtcNow.Ticks - DateTime.UnixEpoch.Ticks, DateTimeKind.Utc).Ticks);

            Assert.AreEqual("apikey", sut.ApiKey);
            Assert.AreEqual("token", sut.MerchantId);
            Assert.AreEqual("auth", sut.Authorization);
            Assert.AreEqual((ulong)12345, sut.Nonce);
            Assert.IsTrue(sut.WithinTimeParameters(new TimeSpan(0, 0, 1)));
        }
    }
}
