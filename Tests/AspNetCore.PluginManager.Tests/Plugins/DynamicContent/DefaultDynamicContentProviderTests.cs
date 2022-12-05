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
 *  File: DefaultDynamicContentProviderTests.cs
 *
 *  Purpose:  Tests for html text template
 *
 *  Date        Name                Reason
 *  29/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

using AspNetCore.PluginManager.Tests.Shared;

using DynamicContent.Plugin.Internal;
using DynamicContent.Plugin.Templates;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;

using SharedPluginFeatures.DynamicContent;

namespace AspNetCore.PluginManager.Tests.Plugins.DynamicContentTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DefaultDynamicContentProviderTests : GenericBaseClass
    {
        private const string TestCategoryName = "Dynamic Content";
        private const string HtmlTemplateAssemblyQualifiedName = "DynamicContent.Plugin.Templates.HtmlTextTemplate, DynamicContent.Plugin";
        private string _currentTestPath = null;

        [TestInitialize]
        public void TestInitialize()
        {
			string appSettingsFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "appsettings.json");

			if (!File.Exists(appSettingsFile))
			{
				File.WriteAllText(appSettingsFile, Encoding.UTF8.GetString(Properties.Resources.appsettings));
			}
			
			_currentTestPath = TestHelper.GetTestPath();

            while (Directory.Exists(_currentTestPath))
            {
                Thread.Sleep(10);
                _currentTestPath = TestHelper.GetTestPath();
            }
        }

        [TestCleanup]
        [TestCategory(TestCategoryName)]
        public void TestFinalize()
        {
            if (!String.IsNullOrEmpty(_currentTestPath))
            {
                if (Directory.Exists(_currentTestPath))
                    Directory.Delete(_currentTestPath, true);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamPluginClassServices_Null_Throws_ArgumentNullException()
        {
            new DefaultDynamicContentProvider(null, GetSettingsProvider());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamSettingsProvider_Null_Throws_ArgumentNullException()
        {
            new DefaultDynamicContentProvider(new MockPluginClassesService(), null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Save_InvalidParam_Null_Throws_ArgumentNullException()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new MockPluginClassesService(), GetSettingsProvider(null, false));
            bool saved = sut.Save(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Save_DynamicContentAddedToCustomList_ContentSavedToDisk_Success()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new MockPluginClassesService(), GetSettingsProvider(null, false));
            bool saved = sut.Save(GetPage1());
            Assert.IsTrue(saved);
            Assert.IsTrue(File.Exists(Path.Combine(_currentTestPath, "wwwroot", "DynamicContent", "1.page")));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SaveUserInput_ContentSavedToDisk_Success()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new MockPluginClassesService(), GetSettingsProvider(null, false));
            bool saved = sut.SaveUserInput("some data");
            Assert.IsTrue(saved);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SaveUserInput_InvalidParam_Null_ReturnsFalse()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new MockPluginClassesService(), GetSettingsProvider(null, false));
            bool saved = sut.SaveUserInput(null);
            Assert.IsFalse(saved);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SaveUserInput_InvalidParam_EmptyString_ReturnsFalse()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new MockPluginClassesService(), GetSettingsProvider(null, false));
            bool saved = sut.SaveUserInput("");
            Assert.IsFalse(saved);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ConvertFromByteArray_InvalidHeader_FirstByte_ReturnsNullInstance_Success()
        {
            byte[] dynamicContent = CreateByteArray();
            dynamicContent[0] = 50;
            IDynamicContentPage sut = DefaultDynamicContentProvider.ConvertFromByteArray(dynamicContent);
            Assert.IsNull(sut);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConvertFromByteArray_InvalidVersion_Throws_InvalidOperationException()
        {
            byte[] dynamicContent = CreateByteArray(98);
            IDynamicContentPage sut = DefaultDynamicContentProvider.ConvertFromByteArray(dynamicContent);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConvertFromByteArray_InvalidArray_Null_ArgumentNullException()
        {
            byte[] dynamicContent = null;
            IDynamicContentPage sut = DefaultDynamicContentProvider.ConvertFromByteArray(dynamicContent);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentException))]
        public void ConvertFromByteArray_InvalidArray_TooShort_ArgumentException()
        {
            byte[] dynamicContent = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            IDynamicContentPage sut = DefaultDynamicContentProvider.ConvertFromByteArray(dynamicContent);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConvertFromByteArray_ClassPartsDoesNotHaveAssemblyName_Throws_InvalidOperationException()
        {
            byte[] dynamicContent = CreateByteArray(1, "classname");
            DefaultDynamicContentProvider.ConvertFromByteArray(dynamicContent);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ConvertFromByteArray_AssemblyNotFound_SubstitutesTemplateWithHtmlTemplate_Success()
        {
            byte[] dynamicContent = CreateByteArray(1, "classname, myAssembly");
            IDynamicContentPage sut = DefaultDynamicContentProvider.ConvertFromByteArray(dynamicContent);
            Assert.IsNotNull(sut);
            Assert.AreEqual(1, sut.Content.Count);
            Assert.IsTrue(sut.Content[0].AssemblyQualifiedName.StartsWith(HtmlTemplateAssemblyQualifiedName));
            Assert.AreEqual("<p>Content template not found</p>", sut.Content[0].Data);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ConvertFromByteArray_ClassNotFound_SubstitutesTemplateWithHtmlTemplate_Success()
        {
            byte[] dynamicContent = CreateByteArray(1, "classname, DynamicContent.Plugin");
            IDynamicContentPage sut = DefaultDynamicContentProvider.ConvertFromByteArray(dynamicContent);
            Assert.IsNotNull(sut);
            Assert.AreEqual(1, sut.Content.Count);
            Assert.IsTrue(sut.Content[0].AssemblyQualifiedName.StartsWith(HtmlTemplateAssemblyQualifiedName));
            Assert.AreEqual("<p>Content template not found</p>", sut.Content[0].Data);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ConvertFromByteArray_InvalidHeader_SecondByte_ReturnsNullInstance_Success()
        {
            byte[] dynamicContent = CreateByteArray();
            dynamicContent[1] = 50;
            IDynamicContentPage sut = DefaultDynamicContentProvider.ConvertFromByteArray(dynamicContent);
            Assert.IsNull(sut);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetCustomPageList_ReturnsCustomPages_Success()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new MockPluginClassesService(), GetSettingsProvider());
            List<LookupListItem> pages = sut.GetCustomPageList();
            Assert.AreEqual(4, pages.Count);
            Assert.AreEqual(1, pages[0].Id);
            Assert.AreEqual(10, pages[1].Id);
            Assert.AreEqual(2, pages[2].Id);
            Assert.AreEqual(3, pages[3].Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCategory(TestCategoryName)]
        public void RouteNameExists_InvalidParamRouteName_Null_Throws_ArgumentNullException()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new MockPluginClassesService(), GetSettingsProvider());
            sut.RouteNameExists(1, null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RouteNameExists_InvalidParamRouteName_EmptyString_Throws_ArgumentNullException()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new MockPluginClassesService(), GetSettingsProvider());
            sut.RouteNameExists(1, "");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void RouteNameExists_ExistingRouteNameNotFound_ReturnsFalse()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new MockPluginClassesService(), GetSettingsProvider());
            bool routeNameFound = sut.RouteNameExists(1, "non-existant");
            Assert.IsFalse(routeNameFound);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void RouteNameExists_ExistingRouteNameFound_ReturnsTrue()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new MockPluginClassesService(), GetSettingsProvider());
            bool routeNameFound = sut.RouteNameExists(1, "Page-10");
            Assert.IsTrue(routeNameFound);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PageNameExists_InvalidParamPageName_Null_Throws_ArgumentNullException()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new MockPluginClassesService(), GetSettingsProvider());
            sut.PageNameExists(1, null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PageNameExists_InvalidParamPageName_EmptyString_Throws_ArgumentNullException()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new MockPluginClassesService(), GetSettingsProvider());
            sut.PageNameExists(1, "");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void PageNameExists_ExistingNameNotFound_ReturnsFalse()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new MockPluginClassesService(), GetSettingsProvider());
            bool routeNameFound = sut.PageNameExists(1, "non-existant");
            Assert.IsFalse(routeNameFound);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void PageNameExists_ExistingNameFound_ReturnsTrue()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new MockPluginClassesService(), GetSettingsProvider());
            bool routeNameFound = sut.PageNameExists(1, "Custom Page 2");
            Assert.IsTrue(routeNameFound);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetCustomPages_ReturnsListOfDynamicPages_Success()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new MockPluginClassesService(), GetSettingsProvider());
            List<IDynamicContentPage> pages = sut.GetCustomPages();
            Assert.IsNotNull(pages);
            Assert.AreEqual(4, pages.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetCustomPage_PageFound_ReturnsValidInstance()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new MockPluginClassesService(), GetSettingsProvider());
            IDynamicContentPage page = sut.GetCustomPage(10);
            Assert.IsNotNull(page);
            Assert.AreEqual(10, page.Id);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetCustomPage_PageNotFound_ReturnsNull()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new MockPluginClassesService(), GetSettingsProvider());
            IDynamicContentPage page = sut.GetCustomPage(100000);
            Assert.IsNull(page);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Templates_RetrieveListOfAllTemplates_Success()
        {
            List<object> testTemplates = new List<object>();

            testTemplates.Add(new HorizontalRuleTemplate());
            testTemplates.Add(new HtmlTextTemplate());
            testTemplates.Add(new YouTubeVideoTemplate());

            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new MockPluginClassesService(testTemplates), GetSettingsProvider());

            List<DynamicContentTemplate> templates = sut.Templates();
            Assert.IsNotNull(templates);
            Assert.AreEqual(3, templates.Count);

            // rerun to get list from memory
            templates = sut.Templates();
            Assert.IsNotNull(templates);
            Assert.AreEqual(3, templates.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void CreateCustomPage_CreatesAndSavesPage_Success()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new MockPluginClassesService(), GetSettingsProvider());
            Assert.AreEqual(4, sut.GetCustomPageList().Count);
            long newPageId = sut.CreateCustomPage();

            Assert.AreEqual(5, sut.GetCustomPageList().Count);
            Assert.AreEqual(4, newPageId);
            Assert.IsTrue(File.Exists(Path.Combine(_currentTestPath, "4.page")));
        }

        #region Private Methods

        private MockSettingsProvider GetSettingsProvider(string json = null, bool exportTestFiles = true)
        {
            if (String.IsNullOrEmpty(json))
            {
                json = "{\"DynamicContent\": {\"DynamicContentLocation\": \"" + _currentTestPath.Replace("\\", "\\\\") + "\"}}";
            }

            if (exportTestFiles)
            {
                _currentTestPath += "\\wwwroot\\DynamicContent";

                if (!Directory.Exists(_currentTestPath))
                    Directory.CreateDirectory(_currentTestPath);

                byte[] page1 = DefaultDynamicContentProvider.ConvertFromDynamicContent(GetPage1());
                File.WriteAllBytes(Path.Combine(_currentTestPath, "1.page"), page1);

                byte[] page2 = DefaultDynamicContentProvider.ConvertFromDynamicContent(GetPage2());
                File.WriteAllBytes(Path.Combine(_currentTestPath, "2.page"), page2);

                byte[] page3 = DefaultDynamicContentProvider.ConvertFromDynamicContent(GetPage3());
                File.WriteAllBytes(Path.Combine(_currentTestPath, "3.page"), page3);

                byte[] page10 = DefaultDynamicContentProvider.ConvertFromDynamicContent(GetPage10());
                File.WriteAllBytes(Path.Combine(_currentTestPath, "10.page"), page10);
            }

            return new MockSettingsProvider(json);
        }

        private byte[] CreateByteArray(int version = 1, string assemblyQualifiedName = null)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(memoryStream))
            {
                writer.Write(new byte[] { 44, 43 });
                writer.Write(0);
                writer.Write(0);
                writer.Write(version);
                writer.Write(0L);
                writer.Write(0);
                writer.Write(0);
                writer.Write(0);
                writer.Write(0);
                writer.Write(0L);
                writer.Write(0L);

                if (!String.IsNullOrEmpty(assemblyQualifiedName))
                {
                    byte[] ver = Encoding.UTF8.GetBytes("123");
                    writer.Write(1);
                    writer.Write(ver.Length);
                    writer.Write(ver);
                    writer.Write(assemblyQualifiedName.Length);
                    writer.Write(Encoding.UTF8.GetBytes(assemblyQualifiedName));
                    writer.Write(DateTime.Now.Ticks);
                    writer.Write(DateTime.Now.Ticks);
                    writer.Write(0);
                    writer.Write(3);
                    writer.Write(0);
                    writer.Write(0);
                    writer.Write(12);
                    writer.Write(0);
                }
                else
                {
                    writer.Write(0);
                }

                return memoryStream.ToArray();
            }

        }

        #endregion Private Methods
    }
}
