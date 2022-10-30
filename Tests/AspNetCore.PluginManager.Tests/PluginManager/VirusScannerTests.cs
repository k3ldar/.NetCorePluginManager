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
 *  File: VirusScannerTests.cs
 *
 *  Purpose:  Tests for virus scanning
 *
 *  Date        Name                Reason
 *  02/06/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using AspNetCore.PluginManager.Internal;
using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Tests.Mocks;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.AspNetCore.PluginManager
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class VirusScannerTests : GenericBaseClass
    {
        private const string TestCategoryName = "AspNetCore Plugin Manager Tests";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_Placebo_Success()
        {
            PlaceboVirusScanner sut = new PlaceboVirusScanner();
            Assert.IsInstanceOfType(sut, typeof(IVirusScanner));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Placebo_DoesNotThrowExceptions_Success()
        {
            PlaceboVirusScanner sut = new PlaceboVirusScanner();
            sut.ScanDirectory(null);
            sut.ScanFile(fileName: null);
            sut.ScanFile(fileNames: null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_MicrosoftDefender_InvalidParam_Null_Throws_ArgumentNullException()
        {
            MicrosoftDefenderVirusScanner sut = new MicrosoftDefenderVirusScanner(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_MicrosoftDefender_Success()
        {
            MicrosoftDefenderVirusScanner sut = new MicrosoftDefenderVirusScanner(new MockLogger());
            Assert.IsNotNull(sut);
            Assert.IsInstanceOfType(sut, typeof(IVirusScanner));
            Assert.IsTrue(sut.Enabled);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void MicrosoftDefender_ScanFolder_FolderDoesNotExist_LogsAndReturns_Success()
        {
            string testFolder = TestHelper.GetTestPath();
            try
            {
                MockLogger logger = new MockLogger();
                MicrosoftDefenderVirusScanner sut = new MicrosoftDefenderVirusScanner(logger);
                Assert.IsNotNull(sut);
                Assert.IsTrue(sut.Enabled);
                sut.ScanDirectory(testFolder);
                Assert.AreEqual(1, logger.Logs.Count);
                Assert.AreEqual($"Directory does not exist: {testFolder}", logger.Logs[0].Data);
            }
            finally
            {
                if (Directory.Exists(testFolder))
                    Directory.Delete(testFolder, true);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void MicrosoftDefender_ScanFolder_InvalidFolderName_Null_FailsSilently_Success()
        {
            string testFolder = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(testFolder);
                ExtractImageResources(testFolder);
                MockLogger logger = new MockLogger();
                MicrosoftDefenderVirusScanner sut = new MicrosoftDefenderVirusScanner(logger);
                Assert.IsNotNull(sut);
                Assert.IsTrue(sut.Enabled);
                sut.ScanDirectory(null);
                Assert.AreEqual(0, logger.Errors.Count);
                Assert.AreEqual(0, logger.Logs.Count);
            }
            finally
            {
                if (Directory.Exists(testFolder))
                    Directory.Delete(testFolder, true);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void MicrosoftDefender_ScanFolder_InvalidFolderName_EmptyString_FailsSilently_Success()
        {
            string testFolder = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(testFolder);
                ExtractImageResources(testFolder);
                MockLogger logger = new MockLogger();
                MicrosoftDefenderVirusScanner sut = new MicrosoftDefenderVirusScanner(logger);
                Assert.IsNotNull(sut);
                Assert.IsTrue(sut.Enabled);
                sut.ScanDirectory("");
                Assert.AreEqual(0, logger.Errors.Count);
                Assert.AreEqual(0, logger.Logs.Count);
            }
            finally
            {
                if (Directory.Exists(testFolder))
                    Directory.Delete(testFolder, true);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void MicrosoftDefender_ScanFolder_Success()
        {
            string testFolder = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(testFolder);
                ExtractImageResources(testFolder);
                MockLogger logger = new MockLogger();
                MicrosoftDefenderVirusScanner sut = new MicrosoftDefenderVirusScanner(logger);
                Assert.IsNotNull(sut);
                Assert.IsTrue(sut.Enabled);
                sut.ScanDirectory(testFolder);
                Assert.AreEqual(0, logger.Errors.Count);
                Assert.AreEqual(1, logger.Logs.Count);
                Assert.IsTrue(logger.Logs[0].Data.Contains("found no threats"));
            }
            finally
            {
                if (Directory.Exists(testFolder))
                    Directory.Delete(testFolder, true);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void MicrosoftDefender_ScanFiles_Success()
        {
            string testFolder = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(testFolder);
                ExtractImageResources(testFolder);
                string[] files = Directory.GetFiles(testFolder, "*.*");
                List<string> fileList = files.ToList();
                fileList.Insert(3, "");
                fileList.Insert(2, "asdf");
                files = fileList.ToArray();

                MockLogger logger = new MockLogger();
                MicrosoftDefenderVirusScanner sut = new MicrosoftDefenderVirusScanner(logger);
                Assert.IsNotNull(sut);
                Assert.IsTrue(sut.Enabled);
                sut.ScanFile(files);
                Assert.AreEqual(0, logger.Errors.Count);
                Assert.IsTrue(logger.Logs.Count >= 9);
                Assert.IsTrue(sut.ScanTimings.Requests >= 9u);
            }
            finally
            {
                if (Directory.Exists(testFolder))
                    Directory.Delete(testFolder, true);
            }
        }
    }
}
