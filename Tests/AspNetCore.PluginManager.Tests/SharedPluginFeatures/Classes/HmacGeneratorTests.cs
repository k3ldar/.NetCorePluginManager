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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
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
    public class HmacGeneratorTests
    {
        private const string Category = "SharedPluginFeatures";

        [TestMethod]
        [TestCategory(Category)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ApiKey_Null_Throws_ArgumentNullException()
        {
            HmacGenerator.GenerateHmac(null, "123", 123, 321, "token", "payload");
        }

        [TestMethod]
        [TestCategory(Category)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ApiKey_EmptyString_Throws_ArgumentNullException()
        {
            HmacGenerator.GenerateHmac("", "123", 123, 321, "token", "payload");
        }

        [TestMethod]
        [TestCategory(Category)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ApiSecret_Null_Throws_ArgumentNullException()
        {
            HmacGenerator.GenerateHmac("123", null, 123, 321, "token", "payload");
        }

        [TestMethod]
        [TestCategory(Category)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ApiSecret_EmptyString_Throws_ArgumentNullException()
        {
            HmacGenerator.GenerateHmac("123", "", 123, 321, "token", "payload");
        }

        [TestMethod]
        [TestCategory(Category)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Token_Null_Throws_ArgumentNullException()
        {
            HmacGenerator.GenerateHmac("123", "321", 123, 321, null, "payload");
        }

        [TestMethod]
        [TestCategory(Category)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Token_EmptyString_Throws_ArgumentNullException()
        {
            HmacGenerator.GenerateHmac("123", "321", 123, 321, "", "payload");
        }

        [TestMethod]
        [TestCategory(Category)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Payload_Null_Throws_ArgumentNullException()
        {
            HmacGenerator.GenerateHmac("123", "321", 123, 321, "token", null);
        }

        [TestMethod]
        [TestCategory(Category)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Construct_InvalidParam_Nonce_LessThanOne_Throws_ArgumentOutOfRangeException()
        {
            HmacGenerator.GenerateHmac("123", "321", 123, 0, "token", "payload");
        }

        [TestMethod]
        [TestCategory(Category)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Construct_InvalidParam_Epoch_LessThanZero_Throws_ArgumentOutOfRangeException()
        {
            HmacGenerator.GenerateHmac("123", "321", -1, 0, "token", "payload");
        }

        [TestMethod]
        [TestCategory(Category)]
        public void EpochDateTime_ReturnsValidEpochTime_Success()
        {
            long epochTime = HmacGenerator.EpochDateTime();
            long localEpoch = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;

            Assert.IsTrue(epochTime > localEpoch - 1000, $"Difference: {localEpoch - epochTime}");
            Assert.IsTrue(epochTime < localEpoch + 1000, $"Difference: {epochTime - localEpoch}");
        }
        
        [TestMethod]
        [TestCategory(Category)]
        public void GenerateNonce_Success()
        {
            ulong nonce = HmacGenerator.GenerateNonce();

            Assert.IsTrue(nonce >= (long)Int32.MaxValue);
            Assert.IsTrue(nonce <= UInt64.MaxValue);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void GenerateHmac_ReturnsValidHmac_Success()
        {
            long localEpoch = (long)(new DateTime(2021, 11, 21, 15, 57, 32, DateTimeKind.Utc) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;

            string hmac = HmacGenerator.GenerateHmac("my api", "my secret", localEpoch, 987654321, "the token", "The data being sent with the request");

            Assert.AreEqual("ODU5Yzg5YjlkOWVhMGU4NWJmZTBlNjQ1MDE5N2I4YWZlMmQxMDg1NTM4OTVkOTMzM2U4ODdhYzA4MjAzZjAzNw==", hmac);
        }
    }
}
