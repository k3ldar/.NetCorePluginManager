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

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.ApiAuthorizationTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public sealed class HmacApiAuthorizationServiceTests
    {
        private const string TestCategoryName = "Api Authorization";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            HmacApiAuthorizationService sut = new HmacApiAuthorizationService(new MockUserApiQueryProvider());
            Assert.IsNotNull(sut);
            Assert.IsInstanceOfType(sut, typeof(IApiAuthorizationService));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_UserApiQueryProvider_Null_Throws_ArgumentNullException()
        {
            new HmacApiAuthorizationService(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetTimings_Success()
        {
            Timings sut = HmacApiAuthorizationService.GetTimings;
            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.IsCloned);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHeader_ApiKey_NotPresent_Returns_Error400()
        {
            HmacApiAuthorizationService sut = new HmacApiAuthorizationService(new MockUserApiQueryProvider());

            bool executeResult = sut.ValidateApiRequest(CreateActionExecutingRequest(null), String.Empty, out int responseCode);

            Assert.IsFalse(executeResult);
            Assert.AreEqual(400, responseCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHeader_ApiKey_EmptyString_Returns_Error400()
        {
            HmacApiAuthorizationService sut = new HmacApiAuthorizationService(new MockUserApiQueryProvider());

            bool executeResult = sut.ValidateApiRequest(CreateActionExecutingRequest(""), String.Empty, out int responseCode);

            Assert.IsFalse(executeResult);
            Assert.AreEqual(400, responseCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHeader_Token_NotPresent_Returns_Error400()
        {
            HmacApiAuthorizationService sut = new HmacApiAuthorizationService(new MockUserApiQueryProvider());
            bool executeResult = sut.ValidateApiRequest(CreateActionExecutingRequest("key", null), String.Empty, out int responseCode);

            Assert.IsFalse(executeResult);
            Assert.AreEqual(400, responseCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHeader_Token_EmptyString_Returns_Error400()
        {
            HmacApiAuthorizationService sut = new HmacApiAuthorizationService(new MockUserApiQueryProvider());
            bool executeResult = sut.ValidateApiRequest(CreateActionExecutingRequest("key", ""), String.Empty, out int responseCode);

            Assert.IsFalse(executeResult);
            Assert.AreEqual(400, responseCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHeader_Nonce_NotPresent_Returns_Error400()
        {
            HmacApiAuthorizationService sut = new HmacApiAuthorizationService(new MockUserApiQueryProvider());
            bool executeResult = sut.ValidateApiRequest(CreateActionExecutingRequest("key", "token", null), String.Empty, out int responseCode);

            Assert.IsFalse(executeResult);
            Assert.AreEqual(400, responseCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHeader_Nonce_EmptyString_Returns_Error400()
        {
            HmacApiAuthorizationService sut = new HmacApiAuthorizationService(new MockUserApiQueryProvider());
            bool executeResult = sut.ValidateApiRequest(CreateActionExecutingRequest("key", "token", ""), String.Empty, out int responseCode);

            Assert.IsFalse(executeResult);
            Assert.AreEqual(400, responseCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHeader_Nonce_InvalidNumber_Returns_Error400()
        {
            HmacApiAuthorizationService sut = new HmacApiAuthorizationService(new MockUserApiQueryProvider());
            bool executeResult = sut.ValidateApiRequest(CreateActionExecutingRequest("key", "token", "AB123"), String.Empty, out int responseCode);

            Assert.IsFalse(executeResult);
            Assert.AreEqual(400, responseCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHeader_TimeStamp_NotPresent_Returns_Error400()
        {
            HmacApiAuthorizationService sut = new HmacApiAuthorizationService(new MockUserApiQueryProvider());
            bool executeResult = sut.ValidateApiRequest(CreateActionExecutingRequest("key", "token", "123", null), String.Empty, out int responseCode);

            Assert.IsFalse(executeResult);
            Assert.AreEqual(400, responseCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHeader_TimeStamp_EmptyString_Returns_Error400()
        {
            HmacApiAuthorizationService sut = new HmacApiAuthorizationService(new MockUserApiQueryProvider());
            bool executeResult = sut.ValidateApiRequest(CreateActionExecutingRequest("key", "token", "123", ""), String.Empty, out int responseCode);

            Assert.IsFalse(executeResult);
            Assert.AreEqual(400, responseCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHeader_TimeStamp_NotNumeric_Returns_Error400()
        {
            HmacApiAuthorizationService sut = new HmacApiAuthorizationService(new MockUserApiQueryProvider());
            bool executeResult = sut.ValidateApiRequest(CreateActionExecutingRequest("key", "token", "123", "123ABC"), String.Empty, out int responseCode);

            Assert.IsFalse(executeResult);
            Assert.AreEqual(400, responseCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHeader_Authorization_NotPresent_Returns_Error400()
        {
            HmacApiAuthorizationService sut = new HmacApiAuthorizationService(new MockUserApiQueryProvider());
            bool executeResult = sut.ValidateApiRequest(CreateActionExecutingRequest("key", "token", "123", "123", null), String.Empty, out int responseCode);

            Assert.IsFalse(executeResult);
            Assert.AreEqual(400, responseCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHeader_Authorization_EmptyString_Returns_Error400()
        {
            HmacApiAuthorizationService sut = new HmacApiAuthorizationService(new MockUserApiQueryProvider());
            bool executeResult = sut.ValidateApiRequest(CreateActionExecutingRequest("key", "token", "123", "123", ""), String.Empty, out int responseCode);

            Assert.IsFalse(executeResult);
            Assert.AreEqual(400, responseCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHeader_UserSecretNotFound_Returns_Error401()
        {
            HmacApiAuthorizationService sut = new HmacApiAuthorizationService(new MockUserApiQueryProvider());
            long epochDate = new DateTime(DateTime.UtcNow.Ticks - DateTime.UnixEpoch.Ticks, DateTimeKind.Utc).Ticks;
            bool executeResult = sut.ValidateApiRequest(CreateActionExecutingRequest("key", "token", "123", epochDate.ToString(), "auth"), String.Empty, out int responseCode);

            Assert.IsFalse(executeResult);
            Assert.AreEqual(401, responseCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHeader_RequestOutOfSpecifiedTimeRange_Returns_Error400()
        {
            HmacApiAuthorizationService sut = new HmacApiAuthorizationService(new MockUserApiQueryProvider(), new TimeSpan(1));
            long epochDate = new DateTime(DateTime.UtcNow.Ticks - DateTime.UnixEpoch.Ticks, DateTimeKind.Utc).Ticks;
            bool executeResult = sut.ValidateApiRequest(CreateActionExecutingRequest("key", "token", "123", epochDate.ToString(), "auth"), String.Empty, out int responseCode);

            Assert.IsFalse(executeResult);
            Assert.AreEqual(400, responseCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateApiRequest_NoBodyData_ValidRequest_Returns_Response200()
        {
            string userSecret = "my secret";

            HmacApiAuthorizationService sut = new HmacApiAuthorizationService(new MockUserApiQueryProvider(userSecret), new TimeSpan(1, 0, 0));
            long epochDate = HmacGenerator.EpochDateTime();

            string hmacAuthorization = HmacGenerator.GenerateHmac("key", "my secret", epochDate, 123, "token", "");
            MockHttpRequest httpRequest = CreateActionExecutingRequest("key", "token", "123", epochDate.ToString(), hmacAuthorization);

            bool executeResult = sut.ValidateApiRequest(httpRequest, String.Empty, out int responseCode);

            Assert.IsTrue(executeResult);
            Assert.AreEqual(200, responseCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateApiRequest_WithBodyData_ValidRequest_Returns_Response200()
        {
            string userSecret = "my secret";

            HmacApiAuthorizationService sut = new HmacApiAuthorizationService(new MockUserApiQueryProvider(userSecret), new TimeSpan(1, 0, 0));
            long epochDate = HmacGenerator.EpochDateTime();

            string hmacAuthorization = HmacGenerator.GenerateHmac("key", "my secret", epochDate, 123, "token", "{some data}");
            MockHttpRequest httpRequest = CreateActionExecutingRequest("key", "token", "123", epochDate.ToString(), hmacAuthorization);
            httpRequest.SetBodyText("{some data}");
			httpRequest.Headers["payloadLength"] = "11";
            bool executeResult = sut.ValidateApiRequest(httpRequest, String.Empty, out int responseCode);

            Assert.IsTrue(executeResult);
            Assert.AreEqual(200, responseCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateApiRequest_WithBodyData_InvalidHmac_Returns_Response401()
        {
            string userSecret = "my secret";

            HmacApiAuthorizationService sut = new HmacApiAuthorizationService(new MockUserApiQueryProvider(userSecret), new TimeSpan(1, 0, 0));
            long epochDate = HmacGenerator.EpochDateTime();

            string hmacAuthorization = HmacGenerator.GenerateHmac("key", "my secret", epochDate, 123, "token", "{some data}");
            MockHttpRequest httpRequest = CreateActionExecutingRequest("key", "token", "123", epochDate.ToString(), "H" + hmacAuthorization.Substring(1));
            httpRequest.SetBodyText("{some data}");
            bool executeResult = sut.ValidateApiRequest(httpRequest, String.Empty, out int responseCode);

            Assert.IsFalse(executeResult);
            Assert.AreEqual(401, responseCode);
        }

        private MockHttpRequest CreateActionExecutingRequest(string apiKey = "", string token = "",
            string nonce = "", string timestamp = "", string authorization = "")
        {
            MockHeaderDictionary headers = new MockHeaderDictionary();

            if (apiKey != null)
                headers.Add("apikey", apiKey);

            if (token != null)
                headers.Add("merchantId", token);

            if (nonce != null)
                headers.Add("nonce", nonce);

            if (timestamp != null)
                headers.Add("timestamp", timestamp);

            if (authorization != null)
                headers.Add("authcode", authorization);

            return new MockHttpRequest(headers);
        }
    }
}
