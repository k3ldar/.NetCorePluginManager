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
 *  File: ValidateConnectionsTests.cs
 *
 *  Purpose:  Test units for ip management
 *
 *  Date        Name                Reason
 *  17/10/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Web;

using AspNetCore.PluginManager.Tests.Shared;

using BadEgg.Plugin.WebDefender;

using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedPluginFeatures;

using static SharedPluginFeatures.Enums;

namespace AspNetCore.PluginManager.Tests.Plugins.BadEggTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
	[DoNotParallelize]
	public class ValidateConnectionsTests
    {
        private const string TestCategoryName = "BadEgg";

        [TestInitialize]
        public void TestStart()
        {
            ValidateConnections.InternalClearAllConnectionInformation();
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_DefaultConstructor_Success()
        {
            ValidateConnections sut = new ValidateConnections();
            Assert.IsNotNull(sut);
            Assert.AreEqual(5, sut.ConnectionTimeout.Minutes);
            Assert.AreEqual(4294967295u, sut.InternalMaximumConnectionsPerSecond);
            Assert.AreEqual(200, sut.HackProbability);
            Assert.AreEqual(700, sut.HackAttempt);
            Assert.AreEqual(3, sut.BotHitsPerSecondProbability);
            Assert.AreEqual(10, sut.BotHitsPerSecond);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddBlackListedIp_InvalidParam_Null_Throws_ArgumentNullException()
        {
            ValidateConnections sut = new ValidateConnections();
            sut.AddToBlackList(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddToBlackList_InvalidParam_EmptyString_Throws_ArgumentNullException()
        {
            ValidateConnections sut = new ValidateConnections();
            sut.AddToBlackList("");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AddToBlackList_Success()
        {
            ValidateConnections sut = new ValidateConnections();
            Assert.AreEqual(0, ValidateConnections.InternalIpAddressList.Count);
            sut.AddToBlackList("10.10.10.10");
            Assert.AreEqual(1, ValidateConnections.InternalIpAddressList.Count);
            Assert.IsTrue(ValidateConnections.InternalIpAddressList["10.10.10.10"]);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddToWhiteList_InvalidParam_Null_Throws_ArgumentNullException()
        {
            ValidateConnections sut = new ValidateConnections();
            sut.AddToWhiteList(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddToWhiteList_InvalidParam_EmptyString_Throws_ArgumentNullException()
        {
            ValidateConnections sut = new ValidateConnections();
            sut.AddToWhiteList("");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AddToWhiteList_Success()
        {
            ValidateConnections sut = new ValidateConnections();
            Assert.AreEqual(0, ValidateConnections.InternalIpAddressList.Count);
            sut.AddToWhiteList("10.10.10.11");
            Assert.AreEqual(1, ValidateConnections.InternalIpAddressList.Count);
            Assert.IsFalse(ValidateConnections.InternalIpAddressList["10.10.10.11"]);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateRequest_IPNotRegistered_Returns_Undetermined()
        {
            ValidateConnections sut = new ValidateConnections();
            ValidateRequestResult Result = sut.ValidateRequest("/", "10.10.10.21", out int count);
            sut.ProcessAllConnectionData();
            Assert.AreEqual(ValidateRequestResult.Undetermined, Result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateRequest_IPBlackListed_Returns_BlackListed()
        {
            ValidateConnections sut = new ValidateConnections();
            sut.AddToBlackList("10.10.10.21");
            ValidateRequestResult Result = sut.ValidateRequest("/", "10.10.10.21", out int count);
            Assert.AreEqual(ValidateRequestResult.IpBlackListed, Result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateRequest_IPWhiteListed_Returns_WhiteListed()
        {
            ValidateConnections sut = new ValidateConnections();
            sut.AddToWhiteList("10.10.10.21");
            ValidateRequestResult Result = sut.ValidateRequest("/", "10.10.10.21", out int count);
            Assert.AreEqual(ValidateRequestResult.IpWhiteListed, Result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateRequest_ProbableHackAttack_Returns_PossibleHackAttempt()
        {
            ValidateConnections sut = new ValidateConnections();
            ValidateRequestResult Result = sut.ValidateRequest("/?null=null&include=", "10.10.10.21", out int count);
            sut.ProcessAllConnectionData();
            Assert.AreEqual(ValidateRequestResult.PossibleHackAttempt, Result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHttpRequest_WhitelistedIp_Returns_IpWhiteListed()
        {
            ValidateConnections sut = new ValidateConnections();
            sut.AddToWhiteList("10.10.10.210");
            MockHttpRequest mockHttpRequest = new MockHttpRequest("10.10.10.210", "?null=null&include=", "/");

            ValidateRequestResult Result = sut.ValidateRequest(mockHttpRequest, false, out int count);
            Assert.AreEqual(ValidateRequestResult.IpWhiteListed, Result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHttpRequest_BlackListedIp_Returns_IpBlackListed()
        {
            ValidateConnections sut = new ValidateConnections();
            sut.AddToBlackList("10.10.10.211");
            MockHttpRequest mockHttpRequest = new MockHttpRequest("10.10.10.211", "?null=null&include=", "/");

            ValidateRequestResult Result = sut.ValidateRequest(mockHttpRequest, false, out int count);
            Assert.AreEqual(ValidateRequestResult.IpBlackListed, Result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHttpRequest__ProbableHackAttack_Returns_PossibleHackAttempt()
        {
            ValidateConnections sut = new ValidateConnections();
            MockHttpRequest mockHttpRequest = new MockHttpRequest("10.10.10.21", "?null=null&include=", "/");

            ValidateRequestResult Result = sut.ValidateRequest(mockHttpRequest, false, out int count);
            sut.ProcessAllConnectionData();
            Assert.AreEqual(ValidateRequestResult.PossibleHackAttempt, Result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHttpRequest_WithForward_HTTP_X_FORWARDED_FOR_ProbableHackAttack_Returns_PossibleHackAttempt()
        {
            ValidateConnections sut = new ValidateConnections();
            MockHttpRequest mockHttpRequest = new MockHttpRequest("10.10.10.21", "?null=null&include=", "/");
            mockHttpRequest.Headers.Add("HTTP_X_FORWARDED_FOR", "91.91.91.91");
            ValidateRequestResult Result = sut.ValidateRequest(mockHttpRequest, false, out int count);

            sut.ProcessAllConnectionData();
            Assert.AreEqual(ValidateRequestResult.PossibleHackAttempt, Result);

            string memoryStatus = ValidateConnections.GetMemoryStatus();
            Assert.IsTrue(memoryStatus.StartsWith("91.91.91.91"));
            Assert.IsFalse(memoryStatus.Contains("10.10.10.21"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHttpRequest_WithForward_X_Forwarded_FOR_ProbableHackAttack_Returns_PossibleHackAttempt()
        {
            ValidateConnections sut = new ValidateConnections();
            MockHttpRequest mockHttpRequest = new MockHttpRequest("10.10.10.21", "?null=null&include=", "/");
            mockHttpRequest.Headers.Add("X-Forwarded-For", "91.91.91.94");
            ValidateRequestResult Result = sut.ValidateRequest(mockHttpRequest, false, out int count);

            sut.ProcessAllConnectionData();
            Assert.AreEqual(ValidateRequestResult.PossibleHackAttempt, Result);

            string memoryStatus = ValidateConnections.GetMemoryStatus();
            Assert.IsTrue(memoryStatus.StartsWith("91.91.91.94"));
            Assert.IsFalse(memoryStatus.Contains("10.10.10.21"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHttpRequest_WithForward_X_Forwarded_dash_FOR_ProbableHackAttack_Returns_PossibleHackAttempt()
        {
            ValidateConnections sut = new ValidateConnections();
            MockHttpRequest mockHttpRequest = new MockHttpRequest("10.10.10.21", "?null=null&include=", "/");
            mockHttpRequest.Headers.Add("http-X-Forwarded-For", "91.91.91.93");
            ValidateRequestResult Result = sut.ValidateRequest(mockHttpRequest, false, out int count);

            sut.ProcessAllConnectionData();
            Assert.AreEqual(ValidateRequestResult.PossibleHackAttempt, Result);

            string memoryStatus = ValidateConnections.GetMemoryStatus();
            Assert.IsTrue(memoryStatus.StartsWith("91.91.91.93"));
            Assert.IsFalse(memoryStatus.Contains("10.10.10.21"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHttpRequest_WithForward_X_Real_IP_ProbableHackAttack_Returns_PossibleHackAttempt()
        {
            ValidateConnections sut = new ValidateConnections();
            MockHttpRequest mockHttpRequest = new MockHttpRequest("10.10.10.21", "?null=null&include=", "/");
            mockHttpRequest.Headers.Add("X-Real-IP", "91.91.91.92");
            ValidateRequestResult Result = sut.ValidateRequest(mockHttpRequest, false, out int count);

            sut.ProcessAllConnectionData();
            Assert.AreEqual(ValidateRequestResult.PossibleHackAttempt, Result);

            string memoryStatus = ValidateConnections.GetMemoryStatus();
            Assert.IsTrue(memoryStatus.StartsWith("91.91.91.92"));
            Assert.IsFalse(memoryStatus.Contains("10.10.10.21"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHttpRequest_InvalidPathBase_Returns_HackAttempt()
        {
            ValidateConnections sut = new ValidateConnections();
            MockHttpRequest mockHttpRequest = new MockHttpRequest("10.10.10.21", "", "/../../../source/filemanager/file/editor/runit.bat");
            mockHttpRequest.PathBase = new PathString("/test.cshtml");

            ValidateRequestResult Result = sut.ValidateRequest(mockHttpRequest, false, out int count);
            sut.ProcessAllConnectionData();
            Assert.AreEqual(ValidateRequestResult.HackAttempt, Result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHttpRequest_RandomWords_Returns_PossibleHackAttempt()
        {
            ValidateConnections sut = new ValidateConnections();
            MockHttpRequest mockHttpRequest = new MockHttpRequest("10.10.10.21", "?1=AZbf3&2=Y9345iJp&3=n78YHjk&type=filemanager&plugin=provider", "/../../filemanager/include/provider");
            mockHttpRequest.PathBase = new PathString("/test.cshtml");

            ValidateRequestResult Result = sut.ValidateRequest(mockHttpRequest, false, out int count);
            sut.ProcessAllConnectionData();
            Assert.AreEqual(ValidateRequestResult.HackAttempt, Result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHttpRequest_SqlInjection_Returns_PossibleSQLInjectionAttack()
        {
            ValidateConnections sut = new ValidateConnections();
            string sqlInjection = HttpUtility.HtmlEncode("SELECT 1 FROM TABLE WHERE 1=1 Inner join");
            MockHttpRequest mockHttpRequest = new MockHttpRequest("10.10.10.29", $"?email={sqlInjection}", "/");
            mockHttpRequest.PathBase = new PathString("/test.cshtml");

            ValidateRequestResult Result = sut.ValidateRequest(mockHttpRequest, false, out int count);
            sut.ProcessAllConnectionData();
            Assert.AreEqual(ValidateRequestResult.PossibleSQLInjectionAttack, Result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHttpRequest_SqlInjection_Returns_SQLInjectionAttack()
        {
            ValidateConnections sut = new ValidateConnections();
            string sqlInjection = HttpUtility.HtmlEncode("SELECT 1 FROM TABLE t1 Inner join OtherTable t2 ON t1.Id = t2.Id outer join ThirdTable t3 on t3.id IS NOT NULL");
            MockHttpRequest mockHttpRequest = new MockHttpRequest("10.10.10.29", $"?email={sqlInjection}", "/");

            ValidateRequestResult Result = sut.ValidateRequest(mockHttpRequest, false, out int count);
            sut.ProcessAllConnectionData();
            Assert.AreEqual(ValidateRequestResult.SQLInjectionAttack | ValidateRequestResult.PossibleHackAttempt, Result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHttpRequest_Spider_Returns_PossibleSpiderBot()
        {
            ValidateConnections sut = new ValidateConnections();

            ValidateRequestResult Result = ValidateRequestResult.Undetermined;

            for (int i = 0; i < 5; i++)
            {
                MockHttpRequest mockHttpRequest = new MockHttpRequest("10.10.10.29", "", $"/{i}");
                Result = sut.ValidateRequest(mockHttpRequest, false, out int count);
            }

            sut.ProcessAllConnectionData();
            Assert.AreEqual(ValidateRequestResult.PossibleSpiderBot, Result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHttpRequest_Spider_Returns_SpiderBot()
        {
            ValidateConnections sut = new ValidateConnections();

            ValidateRequestResult Result = ValidateRequestResult.Undetermined;

            for (int i = 0; i < 20; i++)
            {
                MockHttpRequest mockHttpRequest = new MockHttpRequest("10.10.10.29", "", $"/{i}");
                Result = sut.ValidateRequest(mockHttpRequest, false, out int count);
            }

            sut.ProcessAllConnectionData();
            Assert.AreEqual(ValidateRequestResult.SpiderBot, Result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHttpRequest_ValidateFormValues___RequestVerificationToken_Ignored()
        {
            ValidateConnections sut = new ValidateConnections();

            ValidateRequestResult Result = ValidateRequestResult.Undetermined;

            Dictionary<string, string> formKeys = new Dictionary<string, string>();
            formKeys.Add("__RequestVerificationToken", "some data");
            MockFormCollection formCollections = new MockFormCollection(formKeys);

            MockHttpRequest mockHttpRequest = new MockHttpRequest("10.10.10.121", "", $"/");
            mockHttpRequest.Form = formCollections;

            Result = sut.ValidateRequest(mockHttpRequest, true, out int count);

            sut.ProcessAllConnectionData();
            Assert.AreEqual(ValidateRequestResult.Undetermined, Result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateHttpRequest_FormContent_AttemptedSqlInjection_Returns_SqlInjectionAttack()
        {
            ValidateConnections sut = new ValidateConnections();

            ValidateRequestResult Result = ValidateRequestResult.Undetermined;

            Dictionary<string, string> formKeys = new Dictionary<string, string>();
            formKeys.Add("MyForm", "value=SELECT 1 FROM TABLE t1 Inner join OtherTable t2 ON t1.Id = t2.Id outer join ThirdTable t3 on t3.id IS NOT NULL");
            MockFormCollection formCollections = new MockFormCollection(formKeys);

            MockHttpRequest mockHttpRequest = new MockHttpRequest("10.10.10.121", "", $"/");
            mockHttpRequest.Form = formCollections;

            Result = sut.ValidateRequest(mockHttpRequest, true, out int count);

            sut.ProcessAllConnectionData();
            Assert.AreEqual(ValidateRequestResult.SQLInjectionAttack, Result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetMemoryStatus_Success()
        {
            ValidateConnections sut = new ValidateConnections();

            ValidateRequestResult Result = sut.ValidateRequest("/?null=null&include=", "10.10.10.21", out int count);
            Assert.AreEqual(ValidateRequestResult.PossibleHackAttempt, Result);

            sut.ValidateRequest("/?null=null&include=", "10.10.10.22", out count);
            sut.ValidateRequest("/?null=null&include=", "10.10.10.22", out count);
            sut.ValidateRequest("/?null=null&include=", "10.10.10.23", out count);
            sut.ValidateRequest("/?null=null&include=", "10.10.10.23", out count);
            sut.ValidateRequest("/?null=null&include=", "10.10.10.23", out count);
            sut.ProcessAllConnectionData();

            string memoryStatus = ValidateConnections.GetMemoryStatus();
            Assert.IsFalse(String.IsNullOrEmpty(memoryStatus));
            string[] lines = memoryStatus.Split('\r');
            Assert.AreEqual(3, lines.Length);
            Assert.IsTrue(lines[0].StartsWith("10.10.10.21"));
            Assert.IsTrue(lines[0].Trim().EndsWith("#1#PossibleHackAttempt"), memoryStatus);
            Assert.IsTrue(lines[1].StartsWith("10.10.10.22"));
            Assert.IsTrue(lines[1].Trim().EndsWith("#2#PossibleHackAttempt"));
            Assert.IsTrue(lines[2].StartsWith("10.10.10.23"));
            Assert.IsTrue(lines[2].Trim().EndsWith("#3#PossibleHackAttempt"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Event_ConnectionAdd_NotifiedOfNewConnection_Success()
        {
            bool connectionAdded = false;
            ValidateConnections sut = new ValidateConnections();
            sut.ConnectionAdd += new DefenderConnectionAddEventHandler(delegate (object sender, ConnectionEventArgs e)
            {
                connectionAdded = e.IPAddress.Equals("10.10.10.21");
            });

            ValidateRequestResult Result = sut.ValidateRequest("/?null=null&include=", "10.10.10.21", out int count);
            sut.ProcessAllConnectionData();

            Assert.AreEqual(ValidateRequestResult.PossibleHackAttempt, Result);
            Assert.IsTrue(connectionAdded);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Event_OnReportConnection_NotifiedOfNewConnection_Success()
        {
            bool connectionReported = false;
            ValidateConnections sut = new ValidateConnections(new TimeSpan(0, 0, 0, 0, 50), 3);
            sut.OnReportConnection += new DefenderReportConnection(delegate (object sender, ConnectionReportEventArgs e)
            {
                connectionReported = e.IPAddress.Equals("10.10.10.21") &&
                    e.Result.Equals(ValidateRequestResult.PossibleHackAttempt) &&
                    e.QueryString.Equals("/?null=null&include=");
            });

            ValidateRequestResult Result = sut.ValidateRequest("/?null=null&include=", "10.10.10.21", out int count);
            sut.ProcessAllConnectionData();

            Assert.AreEqual(ValidateRequestResult.PossibleHackAttempt, Result);
            Assert.IsTrue(connectionReported);

            Thread.Sleep(150);

            for (int i = 0; i < 12; i++)
                sut.ProcessAllConnectionData();
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Run_SuccessfullyRemovesConnections_WithEventsCalled_Success()
        {
            bool connectionReported = false;
            int connectionsAdded = 0;
            int connectionsRemoved = 0;
            int blackListCount = 0;
            bool localIpBanRequest = false;

            ValidateConnections sut = new ValidateConnections(new TimeSpan(0, 0, 0, 0, 200), 2u);

            sut.OnReportConnection += new DefenderReportConnection(delegate (object sender, ConnectionReportEventArgs e)
            {
                if (!connectionReported)
                {
                    connectionReported = e.IPAddress.Equals("10.10.10.21") &&
                        e.Result.Equals(ValidateRequestResult.PossibleHackAttempt) &&
                        e.QueryString.Equals("/?null=null&include=");
                }
            });
            sut.OnBanIPAddress += new DefenderRequestBan(delegate (object sender, RequestBanEventArgs e)
            {
                if (e.IPAddress.Equals("127.0.0.1") && !localIpBanRequest)
                {
                    localIpBanRequest = true;
                }
                else
                {
                    e.AddToBlackList = e.IPAddress.Equals("10.10.10.99");

                    if (e.AddToBlackList)
                        blackListCount++;
                }
            });
            sut.ConnectionAdd += new DefenderConnectionAddEventHandler(delegate (object sender, ConnectionEventArgs e)
            {
                connectionsAdded++;
            });
            sut.ConnectionRemove += new DefenderConnectionRemoveEventHandler(delegate (object sender, ConnectionRemoveEventArgs e)
            {
                connectionsRemoved++;
            });

            // normal
            sut.ValidateRequest("/", "192.192.192.192", out int normalCount);
            Assert.AreEqual(0, normalCount);

            // hack attempt
            ValidateRequestResult Result = sut.ValidateRequest("/?null=null&include=", "10.10.10.21", out int hackCount);
            Assert.AreEqual(ValidateRequestResult.PossibleHackAttempt, Result);
            Assert.AreEqual(6, hackCount);

            MockHttpRequest mockHackFile = new MockHttpRequest("10.10.10.99", "", "/../../../source/filemanager/file/editor/runit.bat");
            mockHackFile.PathBase = new PathString("/test.cshtml");
            ValidateRequestResult mockHackFileResult = sut.ValidateRequest(mockHackFile, false, out int count);
            Assert.AreEqual(ValidateRequestResult.HackAttempt, mockHackFileResult);


            MockHttpRequest mockLocalIpHackFile = new MockHttpRequest("127.0.0.1", "", "/../../../source/filemanager/file/editor/runit.bat");
            mockLocalIpHackFile.PathBase = new PathString("/test1.cshtml");
            ValidateRequestResult mockLocalHackFileResult = sut.ValidateRequest(mockLocalIpHackFile, false, out int localIpCount);
            Assert.AreEqual(ValidateRequestResult.HackAttempt, mockLocalHackFileResult);

            // sql injection attempt
            string sqlInjection = HttpUtility.HtmlEncode("SELECT 1 FROM TABLE t1 Inner join OtherTable t2 ON t1.Id = t2.Id outer join ThirdTable t3 on t3.id IS NOT NULL");
            MockHttpRequest mockSqlInjection = new MockHttpRequest("10.10.10.29", $"?email={sqlInjection}", "/");
            Result = sut.ValidateRequest(mockSqlInjection, false, out int sqlInjectCount);
            Assert.AreEqual(12, sqlInjectCount);
            //Assert.AreEqual(ValidateRequestResult.SQLInjectionAttack, Result);

            // spider
            int spiderCount = 0;
            for (int i = 0; i < 20; i++)
            {
                MockHttpRequest mockSpider = new MockHttpRequest("10.10.10.23", "", $"/{i}");
                sut.ValidateRequest(mockSpider, false, out spiderCount);
            }

            Assert.AreEqual(0, spiderCount);

            for (int i = 0; i < 5; i++)
            {
                sut.ProcessAllConnectionData();
            }

            Thread.Sleep(300);

            for (int i = 0; i < 4; i++)
            {
                sut.ProcessAllConnectionData();
            }

            Thread.Sleep(250);

            sut.ProcessAllConnectionData();

            Assert.IsTrue(connectionReported);
            Assert.AreEqual(6, connectionsAdded);
            Assert.AreEqual(6, connectionsRemoved);
            Assert.AreEqual(1, blackListCount);
            Assert.IsFalse(localIpBanRequest);
        }


        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateAndBanIPAddresses_TooManyRequestsPerSecond_ReducedAfterInactivity()
        {
            ValidateConnections sut = new ValidateConnections(new TimeSpan(0, 1, 0), 2);

            ValidateRequestResult Result = ValidateRequestResult.Undetermined;

            for (int i = 0; i < 3; i++)
            {
                MockHttpRequest mockLoopHttpRequest = new MockHttpRequest("10.10.10.29", "", $"/{i}");
                Result = sut.ValidateRequest(mockLoopHttpRequest, false, out int loopCount);
            }

            sut.ProcessAllConnectionData();

            string memory = ValidateConnections.GetMemoryStatus();
            Assert.IsTrue(memory.EndsWith("TooManyRequests"));

            MockHttpRequest finalRequest = new MockHttpRequest("10.10.10.29", "", "/");
            Result = sut.ValidateRequest(finalRequest, false, out int finalRequestCount);
            Assert.AreEqual(ValidateRequestResult.TooManyRequests, Result);

            sut.ProcessAllConnectionData();

            Result = sut.ValidateRequest(finalRequest, false, out finalRequestCount);
            Assert.AreEqual(ValidateRequestResult.Undetermined, Result);

            memory = ValidateConnections.GetMemoryStatus();
            Assert.IsFalse(memory.EndsWith("TooManyRequests"));
        }

    }
}
