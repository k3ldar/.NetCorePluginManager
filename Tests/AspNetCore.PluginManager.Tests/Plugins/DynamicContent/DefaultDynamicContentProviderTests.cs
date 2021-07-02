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
using System.Text;
using System.Threading;

using AspNetCore.PluginManager.Tests.Shared;

using DynamicContent.Plugin.Internal;
using DynamicContent.Plugin.Templates;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.DynamicContent;

using SharedPluginFeatures;
using SharedPluginFeatures.DynamicContent;

namespace AspNetCore.PluginManager.Tests.Plugins.DynamicContentTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DefaultDynamicContentProviderTests
    {
        private const string HtmlTemplateAssemblyQualifiedName = "DynamicContent.Plugin.Templates.HtmlTextTemplate, DynamicContent.Plugin, Version=4.1.0.0, Culture=neutral, PublicKeyToken=null";
        private string _currentTestPath = null;

        [TestInitialize]
        public void TestInitialize()
        {
            _currentTestPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());

            while (Directory.Exists(_currentTestPath))
            {
                Thread.Sleep(10);
                _currentTestPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            }
        }

        [TestCleanup]
        public void TestFinalize()
        {
            if (!String.IsNullOrEmpty(_currentTestPath))
            {
                if (Directory.Exists(_currentTestPath))
                    Directory.Delete(_currentTestPath, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamPluginClassServices_Null_Throws_ArgumentNullException()
        {
            new DefaultDynamicContentProvider(null, GetSettingsProvider());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamSettingsProvider_Null_Throws_ArgumentNullException()
        {
            new DefaultDynamicContentProvider(new TestPluginClassesService(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Save_InvalidParam_Null_Throws_ArgumentNullException()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new TestPluginClassesService(), GetSettingsProvider(null, false));
            bool saved = sut.Save(null);
        }

        [TestMethod]
        public void Save_DynamicContentAddedToCustomList_ContentSavedToDisk_Success()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new TestPluginClassesService(), GetSettingsProvider(null, false));
            bool saved = sut.Save(GetPage1());
            Assert.IsTrue(saved);
            Assert.IsTrue(File.Exists(Path.Combine(_currentTestPath, "wwwroot", "DynamicContent", "1.page")));
        }

        [TestMethod]
        public void ConvertFromByteArray_InvalidHeader_FirstByte_ReturnsNullInstance_Success()
        {
            byte[] dynamicContent = CreateByteArray();
            dynamicContent[0] = 50;
            IDynamicContentPage sut = DefaultDynamicContentProvider.ConvertFromByteArray(dynamicContent);
            Assert.IsNull(sut);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConvertFromByteArray_InvalidVersion_Throws_InvalidOperationException()
        {
            byte[] dynamicContent = CreateByteArray(98);
            IDynamicContentPage sut = DefaultDynamicContentProvider.ConvertFromByteArray(dynamicContent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConvertFromByteArray_InvalidArray_Null_ArgumentNullException()
        {
            byte[] dynamicContent = null;
            IDynamicContentPage sut = DefaultDynamicContentProvider.ConvertFromByteArray(dynamicContent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConvertFromByteArray_InvalidArray_TooShort_ArgumentException()
        {
            byte[] dynamicContent = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            IDynamicContentPage sut = DefaultDynamicContentProvider.ConvertFromByteArray(dynamicContent);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConvertFromByteArray_ClassPartsDoesNotHaveAssemblyName_Throws_InvalidOperationException()
        {
            byte[] dynamicContent = CreateByteArray(1, "classname");
            DefaultDynamicContentProvider.ConvertFromByteArray(dynamicContent);
        }

        [TestMethod]
        public void ConvertFromByteArray_AssemblyNotFound_SubstitutesTemplateWithHtmlTemplate_Success()
        {
            byte[] dynamicContent = CreateByteArray(1, "classname, myAssembly");
            IDynamicContentPage sut = DefaultDynamicContentProvider.ConvertFromByteArray(dynamicContent);
            Assert.IsNotNull(sut);
            Assert.AreEqual(1, sut.Content.Count);
            Assert.AreEqual(HtmlTemplateAssemblyQualifiedName, sut.Content[0].AssemblyQualifiedName);
            Assert.AreEqual("<p>Content template not found</p>", sut.Content[0].Data);
        }

        [TestMethod]
        public void ConvertFromByteArray_ClassNotFound_SubstitutesTemplateWithHtmlTemplate_Success()
        {
            byte[] dynamicContent = CreateByteArray(1, "classname, DynamicContent.Plugin");
            IDynamicContentPage sut = DefaultDynamicContentProvider.ConvertFromByteArray(dynamicContent);
            Assert.IsNotNull(sut);
            Assert.AreEqual(1, sut.Content.Count);
            Assert.AreEqual(HtmlTemplateAssemblyQualifiedName, sut.Content[0].AssemblyQualifiedName);
            Assert.AreEqual("<p>Content template not found</p>", sut.Content[0].Data);
        }

        [TestMethod]
        public void ConvertFromByteArray_InvalidHeader_SecondByte_ReturnsNullInstance_Success()
        {
            byte[] dynamicContent = CreateByteArray();
            dynamicContent[1] = 50;
            IDynamicContentPage sut = DefaultDynamicContentProvider.ConvertFromByteArray(dynamicContent);
            Assert.IsNull(sut);
        }

        [TestMethod]
        public void GetCustomPageList_ReturnsCustomPages_Success()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new TestPluginClassesService(), GetSettingsProvider());
            List<LookupListItem> pages = sut.GetCustomPageList();
            Assert.AreEqual(4, pages.Count);
            Assert.AreEqual(1, pages[0].Id);
            Assert.AreEqual(10, pages[1].Id);
            Assert.AreEqual(2, pages[2].Id);
            Assert.AreEqual(3, pages[3].Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RouteNameExists_InvalidParamRouteName_Null_Throws_ArgumentNullException()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new TestPluginClassesService(), GetSettingsProvider());
            sut.RouteNameExists(1, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RouteNameExists_InvalidParamRouteName_EmptyString_Throws_ArgumentNullException()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new TestPluginClassesService(), GetSettingsProvider());
            sut.RouteNameExists(1, "");
        }

        [TestMethod]
        public void RouteNameExists_ExistingRouteNameNotFound_ReturnsFalse()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new TestPluginClassesService(), GetSettingsProvider());
            bool routeNameFound = sut.RouteNameExists(1, "non-existant");
            Assert.IsFalse(routeNameFound);
        }

        [TestMethod]
        public void RouteNameExists_ExistingRouteNameFound_ReturnsTrue()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new TestPluginClassesService(), GetSettingsProvider());
            bool routeNameFound = sut.RouteNameExists(1, "Page-10");
            Assert.IsTrue(routeNameFound);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PageNameExists_InvalidParamPageName_Null_Throws_ArgumentNullException()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new TestPluginClassesService(), GetSettingsProvider());
            sut.PageNameExists(1, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PageNameExists_InvalidParamPageName_EmptyString_Throws_ArgumentNullException()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new TestPluginClassesService(), GetSettingsProvider());
            sut.PageNameExists(1, "");
        }

        [TestMethod]
        public void PageNameExists_ExistingNameNotFound_ReturnsFalse()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new TestPluginClassesService(), GetSettingsProvider());
            bool routeNameFound = sut.PageNameExists(1, "non-existant");
            Assert.IsFalse(routeNameFound);
        }

        [TestMethod]
        public void PageNameExists_ExistingNameFound_ReturnsTrue()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new TestPluginClassesService(), GetSettingsProvider());
            bool routeNameFound = sut.PageNameExists(1, "Custom Page 2");
            Assert.IsTrue(routeNameFound);
        }

        [TestMethod]
        public void GetCustomPages_ReturnsListOfDynamicPages_Success()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new TestPluginClassesService(), GetSettingsProvider());
            List<IDynamicContentPage> pages = sut.GetCustomPages();
            Assert.IsNotNull(pages);
            Assert.AreEqual(4, pages.Count);
        }

        [TestMethod]
        public void GetCustomPage_PageFound_ReturnsValidInstance()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new TestPluginClassesService(), GetSettingsProvider());
            IDynamicContentPage page = sut.GetCustomPage(10);
            Assert.IsNotNull(page);
            Assert.AreEqual(10, page.Id);
        }

        [TestMethod]
        public void GetCustomPage_PageNotFound_ReturnsNull()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new TestPluginClassesService(), GetSettingsProvider());
            IDynamicContentPage page = sut.GetCustomPage(100000);
            Assert.IsNull(page);
        }

        [TestMethod]
        public void Templates_RetrieveListOfAllTemplates_Success()
        {
            List<object> testTemplates = new List<object>();

            testTemplates.Add(new HorizontalRuleTemplate());
            testTemplates.Add(new HtmlTextTemplate());
            testTemplates.Add(new YouTubeVideoTemplate());

            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new TestPluginClassesService(testTemplates), GetSettingsProvider());

            List<DynamicContentTemplate> templates = sut.Templates();
            Assert.IsNotNull(templates);
            Assert.AreEqual(3, templates.Count);

            // rerun to get list from memory
            templates = sut.Templates();
            Assert.IsNotNull(templates);
            Assert.AreEqual(3, templates.Count);
        }

        [TestMethod]
        public void CreateCustomPage_CreatesAndSavesPage_Success()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new TestPluginClassesService(), GetSettingsProvider());
            Assert.AreEqual(4, sut.GetCustomPageList().Count); 
            int newPageId = sut.CreateCustomPage();

            Assert.AreEqual(5, sut.GetCustomPageList().Count);
            Assert.AreEqual(4, newPageId);
            Assert.IsTrue(File.Exists(Path.Combine(_currentTestPath, "4.page")));
        }


        #region Private Methods


        private IDynamicContentPage GetPage1()
        {
            IDynamicContentPage Result = new DynamicContentPage()
            {
                Id = 1,
                Name = "Custom Page 1",
                RouteName = "Page-1",
            };

            HtmlTextTemplate htmlLayout1 = new HtmlTextTemplate()
            {
                UniqueId = "1",
                SortOrder = 0,
                WidthType = DynamicContentWidthType.Columns,
                Width = 12,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is <br />html over<br />three lines</p>"
            };

            Result.Content.Add(htmlLayout1);

            return Result;
        }

        private IDynamicContentPage GetPage2()
        {
            IDynamicContentPage Result = new DynamicContentPage()
            {
                Id = 2,
                Name = "Custom Page 2",
                RouteName = "Page-2",
            };

            HtmlTextTemplate htmlLayout1 = new HtmlTextTemplate()
            {
                UniqueId = "control-1",
                SortOrder = 0,
                WidthType = DynamicContentWidthType.Columns,
                Width = 12,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is <br />html over<br />three lines</p>"
            };

            Result.Content.Add(htmlLayout1);

            HtmlTextTemplate htmlLayout2 = new HtmlTextTemplate()
            {
                UniqueId = "control-2",
                SortOrder = 2,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />over two lines</p>"
            };

            Result.Content.Add(htmlLayout2);

            return Result;
        }

        private IDynamicContentPage GetPage3()
        {
            IDynamicContentPage Result = new DynamicContentPage()
            {
                Id = 3,
                Name = "Custom Page 3",
                RouteName = "Page-3",
            };

            HtmlTextTemplate htmlLayout1 = new HtmlTextTemplate()
            {
                UniqueId = "control-1",
                SortOrder = 0,
                WidthType = DynamicContentWidthType.Columns,
                Width = 12,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is <br />html over<br />three lines</p>"
            };

            Result.Content.Add(htmlLayout1);

            SpacerTemplate spacerTemplate1 = new SpacerTemplate()
            {
                SortOrder = 1,
                Width = 8
            };

            Result.Content.Add(spacerTemplate1);

            HtmlTextTemplate htmlLayout2 = new HtmlTextTemplate()
            {
                UniqueId = "control-2",
                SortOrder = 2,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />over two lines</p>"
            };

            Result.Content.Add(htmlLayout2);

            return Result;
        }

        private IDynamicContentPage GetPage10()
        {
            IDynamicContentPage Result = new DynamicContentPage()
            {
                Id = 10,
                Name = "Custom Page 10",
                RouteName = "Page-10"
            };

            HtmlTextTemplate htmlLayout1 = new HtmlTextTemplate()
            {
                UniqueId = "control-1",
                SortOrder = 0,
                WidthType = DynamicContentWidthType.Columns,
                Width = 12,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is <br />html over<br />three lines</p>"
            };

            Result.Content.Add(htmlLayout1);

            HtmlTextTemplate htmlLayout2 = new HtmlTextTemplate()
            {
                UniqueId = "control-2",
                SortOrder = 2,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />Content 2</p>"
            };

            Result.Content.Add(htmlLayout2);

            HtmlTextTemplate htmlLayout3 = new HtmlTextTemplate()
            {
                UniqueId = "control-3",
                SortOrder = 9,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />Content 3</p>"
            };

            Result.Content.Add(htmlLayout3);

            HtmlTextTemplate htmlLayout4 = new HtmlTextTemplate()
            {
                UniqueId = "control-4",
                SortOrder = 8,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />Content 4</p>"
            };

            Result.Content.Add(htmlLayout4);

            HtmlTextTemplate htmlLayout5 = new HtmlTextTemplate()
            {
                UniqueId = "control-5",
                SortOrder = 7,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />Content 5</p>"
            };

            Result.Content.Add(htmlLayout5);

            HtmlTextTemplate htmlLayout6 = new HtmlTextTemplate()
            {
                UniqueId = "control-6",
                SortOrder = 6,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />Content 6</p>"
            };

            Result.Content.Add(htmlLayout6);

            HtmlTextTemplate htmlLayout7 = new HtmlTextTemplate()
            {
                UniqueId = "control-7",
                SortOrder = 5,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />Content 7</p>"
            };

            Result.Content.Add(htmlLayout7);

            HtmlTextTemplate htmlLayout8 = new HtmlTextTemplate()
            {
                UniqueId = "control-8",
                SortOrder = 4,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />Content 8</p>"
            };

            Result.Content.Add(htmlLayout8);

            HtmlTextTemplate htmlLayout9 = new HtmlTextTemplate()
            {
                UniqueId = "control-9",
                SortOrder = 3,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />Content 9</p>"
            };

            Result.Content.Add(htmlLayout9);

            HtmlTextTemplate htmlLayout10 = new HtmlTextTemplate()
            {
                UniqueId = "control-10",
                SortOrder = 20,
                WidthType = DynamicContentWidthType.Columns,
                Width = 4,
                HeightType = DynamicContentHeightType.Automatic,
                Data = "<p>This is html<br />Content 10</p>"
            };

            Result.Content.Add(htmlLayout10);

            return Result;
        }

        private TestSettingsProvider GetSettingsProvider(string json = null, bool exportTestFiles = true)
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

            return new TestSettingsProvider(json);
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
                writer.Write(0);
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
