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
 *  File: SpiderControllerTests.cs
 *
 *  Purpose:  Tests for dynamic content controller
 *
 *  Date        Name                Reason
 *  01/12/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using AspNetCore.PluginManager.DemoWebsite.Classes.Mocks;
using AspNetCore.PluginManager.Tests.Controllers;
using AspNetCore.PluginManager.Tests.Shared;

using DynamicContent.Plugin.Controllers;
using DynamicContent.Plugin.Model;
using DynamicContent.Plugin.Templates;

using MemoryCache.Plugin;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.DynamicContent;

using PluginManager.Abstractions;
using PluginManager.Internal;

using Shared.Abstractions;
using Shared.Classes;

using SharedPluginFeatures;
using SharedPluginFeatures.DynamicContent;

using pm = PluginManager.Internal;

namespace AspNetCore.PluginManager.Tests.Plugins.DynamicContentTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
	[DoNotParallelize]
	public class DynamicContentControllerTests : BaseControllerTests
    {
        #region Private Members

        private const string TestCategoryName = "Dynamic Content";

        private const string GetContentData = "<li id=\"control-1\" class=\"col-edit-12 ui-state-default editControl\"><p class=\"ctlHeader\">" +
            "Html Content<span class=\"deleteBtn\" id=\"control-1\">X</span><span class=\"editBtn\" id=\"control-1\" data-cs=\"/DynamicContent/TextTemplateEditor/\">" +
            "Edit</span></p><div class=\"ctlContent\"><div class=\"col-sm-12\"><p>This is <br />html over<br />three lines</p></div></div></li><li id=\"control-2\" " +
            "class=\"col-edit-4 ui-state-default editControl\"><p class=\"ctlHeader\">Html Content<span class=\"deleteBtn\" id=\"control-2\">X</span>" +
            "<span class=\"editBtn\" id=\"control-2\" data-cs=\"/DynamicContent/TextTemplateEditor/\">Edit</span></p><div " +
            "class=\"ctlContent\"><div class=\"col-sm-12\"><p>This is html<br />over two lines</p></div></div></li>";

        private const string PreviewContent = "<div class=\"col-sm-12\"><p>This is <br />html over<br />three lines</p></div><div class=\"col-sm-4\"><p>This is html<br />Content 2</p></div>" +
            "<div class=\"col-sm-4\"><p>This is html<br />Content 9</p></div><div class=\"col-sm-4\"><p>This is html<br />Content 8</p></div><div class=\"col-sm-4\"><p>This is html<br />" +
            "Content 7</p></div><div class=\"col-sm-4\"><p>This is html<br />Content 6</p></div><div class=\"col-sm-4\"><p>This is html<br />Content 5</p></div><div class=\"col-sm-4\"><p>" +
            "This is html<br />Content 4</p></div><div class=\"col-sm-4\"><p>This is html<br />Content 3</p></div><div class=\"col-sm-4\"><p>This is html<br />Content 10</p></div>";

        #endregion Private Members

        [TestInitialize]
        public void InitializeDynamicContentControllerTests()
        {
			string appSettingsFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "appsettings.json");

			if (!System.IO.File.Exists(appSettingsFile))
			{
				System.IO.File.WriteAllText(appSettingsFile, Encoding.UTF8.GetString(Properties.Resources.appsettings));
			}

			ICacheManagerFactory cacheManagerFactory = new CacheManagerFactory();
            cacheManagerFactory.ClearAllCaches();
            DynamicContent.Plugin.PluginInitialisation.DynamicContentCache.Clear();
            InitializeDynamicContentPluginManager();
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_InvalidPath_Null_Returns404Response()
        {
            DynamicContentController sut = CreateDynamicContentController();
            IActionResult response = sut.Index(null);

            StatusCodeResult result = response as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_InvalidPath_EmptyString_Returns404Response()
        {
            DynamicContentController sut = CreateDynamicContentController();
            IActionResult response = sut.Index("");

            StatusCodeResult result = response as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_PathNotFoundInCache_Returns404Response()
        {
            DynamicContentController sut = CreateDynamicContentController();
            IActionResult response = sut.Index("mypage");

            StatusCodeResult result = response as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_PathFoundInCache_InvalidCacheItem_Returns404Response()
        {
            DynamicContent.Plugin.PluginInitialisation.DynamicContentCache.Add("mypage", new CacheItem("mypage", "test"));
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            DynamicContentController sut = CreateDynamicContentController();
            IActionResult response = sut.Index("mypage");

            StatusCodeResult result = response as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_PathFoundInCache_ReturnsValidModel_Success()
        {
            IPluginClassesService pluginServices = _testDynamicContentPlugin as IPluginClassesService;
            MockDynamicContentProvider mockDynamicContentProvider = new MockDynamicContentProvider(pluginServices);
            IDynamicContentPage page = mockDynamicContentProvider.GetCustomPages()[0];
            DynamicContent.Plugin.PluginInitialisation.DynamicContentCache.Add(page.RouteName, new CacheItem(page.RouteName, page));

            DynamicContentController sut = CreateDynamicContentController(null, null, mockDynamicContentProvider);
            IActionResult response = sut.Index(page.RouteName);

            ViewResult result = response as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual("/Views/DynamicContent/Index.cshtml", result.ViewName);


            PageModel pageModel = result.Model as PageModel;

            Assert.IsNotNull(pageModel);
            Assert.AreEqual("<div class=\"col-sm-12\"><p>This is <br />html over<br />three lines</p></div>", pageModel.Content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_ReturnsValidModel_WithInputControlIds_Success()
        {
            IPluginClassesService pluginServices = _testDynamicContentPlugin as IPluginClassesService;
            MockDynamicContentProvider mockDynamicContentProvider = new MockDynamicContentProvider(pluginServices);
            IDynamicContentPage page = mockDynamicContentProvider.GetCustomPages()[0];

            FormRadioGroupTemplate radioGroup = new FormRadioGroupTemplate()
            {
                Data = "test|||||Option A;Option B"
            };
            page.Content.Add(radioGroup);

            FormTextBoxTemplate textBox = new FormTextBoxTemplate()
            {
                Data = "textbox1|Fill in the data|true"
            };
            page.Content.Add(textBox);

            FormCheckBoxTemplate checkBox = new FormCheckBoxTemplate()
            {

            };
            page.Content.Add(checkBox);

            DynamicContent.Plugin.PluginInitialisation.DynamicContentCache.Add(page.RouteName, new CacheItem(page.RouteName, page));

            DynamicContentController sut = CreateDynamicContentController(null, null, mockDynamicContentProvider);
            IActionResult response = sut.Index(page.RouteName);

            ViewResult result = response as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual("/Views/DynamicContent/Index.cshtml", result.ViewName);


            PageModel pageModel = result.Model as PageModel;

            Assert.IsNotNull(pageModel);
            Assert.IsTrue(pageModel.HasInputControls);
            Assert.AreEqual(2, pageModel.DynamicContentIds.Length);
            Assert.AreEqual("test", pageModel.DynamicContentIds[0]);
            Assert.AreEqual("textbox1", pageModel.DynamicContentIds[1]);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_ReturnsValidModel_WithBackgroundImageAndColor_Success()
        {
            IPluginClassesService pluginServices = _testDynamicContentPlugin as IPluginClassesService;
            MockDynamicContentProvider mockDynamicContentProvider = new MockDynamicContentProvider(pluginServices);
            IDynamicContentPage page = mockDynamicContentProvider.GetCustomPages()[0];
            page.BackgroundColor = "#294b6y;";
            page.BackgroundImage = "/images/custombackground.jpg";
            DynamicContent.Plugin.PluginInitialisation.DynamicContentCache.Add(page.RouteName, new CacheItem(page.RouteName, page));

            DynamicContentController sut = CreateDynamicContentController(null, null, mockDynamicContentProvider);
            IActionResult response = sut.Index(page.RouteName);

            ViewResult result = response as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual("/Views/DynamicContent/Index.cshtml", result.ViewName);


            PageModel pageModel = result.Model as PageModel;

            Assert.IsNotNull(pageModel);
            Assert.AreEqual("<div class=\"col-sm-12\"><p>This is <br />html over<br />three lines</p></div>", pageModel.Content);
            Assert.AreEqual("<style>body {background-color: #294b6y;background-image: url('/images/custombackground.jpg');background-size: 100% 100%;}</style>", pageModel.PageCSS);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_ReturnsValidModel_WithNoBackgroundImage_HasBackgroundColor_Success()
        {
            IPluginClassesService pluginServices = _testDynamicContentPlugin as IPluginClassesService;
            MockDynamicContentProvider mockDynamicContentProvider = new MockDynamicContentProvider(pluginServices);
            IDynamicContentPage page = mockDynamicContentProvider.GetCustomPages()[0];
            page.BackgroundColor = "#294b6y;";
            DynamicContent.Plugin.PluginInitialisation.DynamicContentCache.Add(page.RouteName, new CacheItem(page.RouteName, page));

            DynamicContentController sut = CreateDynamicContentController(null, null, mockDynamicContentProvider);
            IActionResult response = sut.Index("PaGe-1");

            ViewResult result = response as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual("/Views/DynamicContent/Index.cshtml", result.ViewName);


            PageModel pageModel = result.Model as PageModel;

            Assert.IsNotNull(pageModel);
            Assert.AreEqual("<div class=\"col-sm-12\"><p>This is <br />html over<br />three lines</p></div>", pageModel.Content);
            Assert.AreEqual("<style>body {background-color: #294b6y;}</style>", pageModel.PageCSS);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_ReturnsValidModel_WithBackgroundImage_HasNoBackgroundColor_Success()
        {
            IPluginClassesService pluginServices = _testDynamicContentPlugin as IPluginClassesService;
            MockDynamicContentProvider mockDynamicContentProvider = new MockDynamicContentProvider(pluginServices);
            IDynamicContentPage page = mockDynamicContentProvider.GetCustomPages()[0];
            page.BackgroundColor = null;
            page.BackgroundImage = "/images/custombackground.jpg";
            DynamicContent.Plugin.PluginInitialisation.DynamicContentCache.Add(page.RouteName, new CacheItem(page.RouteName, page));

            DynamicContentController sut = CreateDynamicContentController(null, null, mockDynamicContentProvider);
            IActionResult response = sut.Index("Page-1");

            ViewResult result = response as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual("/Views/DynamicContent/Index.cshtml", result.ViewName);


            PageModel pageModel = result.Model as PageModel;

            Assert.IsNotNull(pageModel);
            Assert.AreEqual("<div class=\"col-sm-12\"><p>This is <br />html over<br />three lines</p></div>", pageModel.Content);
            Assert.AreEqual("<style>body {background-image: url('/images/custombackground.jpg');background-size: 100% 100%;}</style>", pageModel.PageCSS);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SubmitData_Validate_Attributes()
        {
            string MethodName = "SubmitData";
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), MethodName));

            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(DynamicContentController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), MethodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(DynamicContentController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SubmitData_InvalidPath_Null_Returns404Response()
        {
            DynamicContentController sut = CreateDynamicContentController();
            IActionResult response = sut.SubmitData(null, "");

            StatusCodeResult result = response as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SubmitData_InvalidPath_EmptyString_Returns404Response()
        {
            DynamicContentController sut = CreateDynamicContentController();
            IActionResult response = sut.SubmitData("", "");

            StatusCodeResult result = response as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SubmitData_PathNotFoundInCache_Returns404Response()
        {
            DynamicContentController sut = CreateDynamicContentController();
            IActionResult response = sut.SubmitData("mypage", "");

            StatusCodeResult result = response as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SubmitData_PathFoundInCache_InvalidCacheItem_Returns404Response()
        {
            DynamicContent.Plugin.PluginInitialisation.DynamicContentCache.Add("mypage", new CacheItem("mypage", "test"));
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            DynamicContentController sut = CreateDynamicContentController();
            IActionResult response = sut.SubmitData("mypage", "");

            StatusCodeResult result = response as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SubmitData_PathFoundInCache_ReturnsValidModel_Success()
        {
            IPluginClassesService pluginServices = _testDynamicContentPlugin as IPluginClassesService;
            MockDynamicContentProvider mockDynamicContentProvider = new MockDynamicContentProvider(pluginServices);
            IDynamicContentPage page = mockDynamicContentProvider.GetCustomPages()[0];
            DynamicContent.Plugin.PluginInitialisation.DynamicContentCache.Add(page.RouteName, new CacheItem(page.RouteName, page));

            DynamicContentController sut = CreateDynamicContentController(null, null, mockDynamicContentProvider);
            IActionResult response = sut.SubmitData(page.RouteName, "");

            ViewResult result = response as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual("/Views/DynamicContent/Index.cshtml", result.ViewName);


            PageModel pageModel = result.Model as PageModel;
            Assert.IsNotNull(pageModel);
            Assert.IsTrue(pageModel.HasDataSaved);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SubmitData_ReturnsValidModel_WithInputControlIds_Success()
        {
            IPluginClassesService pluginServices = _testDynamicContentPlugin as IPluginClassesService;
            MockDynamicContentProvider mockDynamicContentProvider = new MockDynamicContentProvider(pluginServices);
            IDynamicContentPage page = mockDynamicContentProvider.GetCustomPages()[0];

            FormRadioGroupTemplate radioGroup = new FormRadioGroupTemplate()
            {
                Data = "test|||||Option A;Option B"
            };
            page.Content.Add(radioGroup);

            FormTextBoxTemplate textBox = new FormTextBoxTemplate()
            {
                Data = "textbox1|Fill in the data|true"
            };
            page.Content.Add(textBox);

            FormCheckBoxTemplate checkBox = new FormCheckBoxTemplate()
            {

            };
            page.Content.Add(checkBox);

            DynamicContent.Plugin.PluginInitialisation.DynamicContentCache.Add(page.RouteName, new CacheItem(page.RouteName, page));

            DynamicContentController sut = CreateDynamicContentController(null, null, mockDynamicContentProvider);
            IActionResult response = sut.SubmitData(page.RouteName, "");

            ViewResult result = response as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual("/Views/DynamicContent/Index.cshtml", result.ViewName);


            PageModel pageModel = result.Model as PageModel;

            Assert.IsNotNull(pageModel);
            Assert.IsTrue(pageModel.HasInputControls);
            Assert.AreEqual(2, pageModel.DynamicContentIds.Length);
            Assert.IsTrue(pageModel.HasDataSaved);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SubmitData_ReturnsValidModel_WithBackgroundImageAndColor_Success()
        {
            IPluginClassesService pluginServices = _testDynamicContentPlugin as IPluginClassesService;
            MockDynamicContentProvider mockDynamicContentProvider = new MockDynamicContentProvider(pluginServices);
            IDynamicContentPage page = mockDynamicContentProvider.GetCustomPages()[0];
            page.BackgroundColor = "#294b6y;";
            page.BackgroundImage = "/images/custombackground.jpg";
            DynamicContent.Plugin.PluginInitialisation.DynamicContentCache.Add(page.RouteName, new CacheItem(page.RouteName, page));

            DynamicContentController sut = CreateDynamicContentController(null, null, mockDynamicContentProvider);
            IActionResult response = sut.SubmitData(page.RouteName, "some data");

            ViewResult result = response as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual("/Views/DynamicContent/Index.cshtml", result.ViewName);


            PageModel pageModel = result.Model as PageModel;

            Assert.IsNotNull(pageModel);
            Assert.IsTrue(pageModel.HasDataSaved);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SubmitData_ReturnsValidModel_WithNoBackgroundImage_HasBackgroundColor_Success()
        {
            IPluginClassesService pluginServices = _testDynamicContentPlugin as IPluginClassesService;
            MockDynamicContentProvider mockDynamicContentProvider = new MockDynamicContentProvider(pluginServices);
            IDynamicContentPage page = mockDynamicContentProvider.GetCustomPages()[0];
            page.BackgroundColor = "#294b6y;";
            DynamicContent.Plugin.PluginInitialisation.DynamicContentCache.Add(page.RouteName, new CacheItem(page.RouteName, page));

            DynamicContentController sut = CreateDynamicContentController(null, null, mockDynamicContentProvider);
            IActionResult response = sut.SubmitData(page.RouteName, "");

            ViewResult result = response as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual("/Views/DynamicContent/Index.cshtml", result.ViewName);


            PageModel pageModel = result.Model as PageModel;

            Assert.IsNotNull(pageModel);
            Assert.IsTrue(pageModel.HasDataSaved);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SubmitData_ReturnsValidModel_WithBackgroundImage_HasNoBackgroundColor_Success()
        {
            IPluginClassesService pluginServices = _testDynamicContentPlugin as IPluginClassesService;
            MockDynamicContentProvider mockDynamicContentProvider = new MockDynamicContentProvider(pluginServices);
            IDynamicContentPage page = mockDynamicContentProvider.GetCustomPages()[0];
            page.BackgroundColor = null;
            page.BackgroundImage = "/images/custombackground.jpg";
            DynamicContent.Plugin.PluginInitialisation.DynamicContentCache.Add(page.RouteName, new CacheItem(page.RouteName, page));

            DynamicContentController sut = CreateDynamicContentController(null, null, mockDynamicContentProvider);
            IActionResult response = sut.SubmitData(page.RouteName, "");

            ViewResult result = response as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual("/Views/DynamicContent/Index.cshtml", result.ViewName);


            PageModel pageModel = result.Model as PageModel;

            Assert.IsNotNull(pageModel);
            Assert.IsTrue(pageModel.HasDataSaved);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateClassAttributes_Success()
        {
            Assert.IsTrue(ClassHasAttribute<DenySpiderAttribute>(typeof(DynamicContentController)));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_DynamicContentController_InvalidDynamicContentProvider_Throws_ArgumentNullException()
        {
            ISettingsProvider settingsProvider = new DefaultSettingProvider(Directory.GetCurrentDirectory(), null, null);
            DynamicContentController Result = new DynamicContentController(null, new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10)), new MockNotificationService());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_DynamicContentController_InvalidMemoryCache_Throws_ArgumentNullException()
        {
            IPluginClassesService pluginServices = _testDynamicContentPlugin as IPluginClassesService;
            DynamicContentController Result = new DynamicContentController(new MockDynamicContentProvider(pluginServices), null, new MockNotificationService());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_DynamicContentController_InvalidNotificationService_Throws_ArgumentNullException()
        {
            IPluginClassesService pluginServices = _testDynamicContentPlugin as IPluginClassesService;
            ISettingsProvider settingsProvider = new DefaultSettingProvider(Directory.GetCurrentDirectory(), null, null);
            DynamicContentController Result = new DynamicContentController(new MockDynamicContentProvider(pluginServices), 
				new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10)), null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateAttributes()
        {
            Assert.IsTrue(ClassHasAttribute<DenySpiderAttribute>(typeof(DynamicContentController)));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_DynamicContentControllerInstance_Success()
        {
            DynamicContentController sut = CreateDynamicContentController();

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetCustomPages_Validate_Attributes()
        {
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetCustomPages)));
            Assert.IsTrue(MethodHasAttribute<BreadcrumbAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetCustomPages)));
            Assert.IsTrue(MethodHasAttribute<LoggedInAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetCustomPages)));

            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetCustomPages)));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetCustomPages)));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetCustomPages)));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetCustomPages)));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetCustomPages_Validate_IActionResult()
        {
            DynamicContentController sut = CreateDynamicContentController();

            IActionResult response = sut.GetCustomPages();

            // Assert
            Assert.IsInstanceOfType(response, typeof(ViewResult));

            ViewResult viewResult = response as ViewResult;

            Assert.IsNotNull(viewResult.Model);

            ValidateBaseModel(viewResult);

            Assert.AreEqual("/Views/DynamicContent/CustomPages.cshtml", viewResult.ViewName);

            Assert.IsInstanceOfType(viewResult.Model, typeof(CustomPagesModel));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetCustomPages_Validate_ContainsExistingModels()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.GetCustomPages();

            ViewResult viewResult = response as ViewResult;

            Assert.IsNotNull(viewResult.Model);

            Assert.IsInstanceOfType(viewResult.Model, typeof(CustomPagesModel));

            CustomPagesModel sut = viewResult.Model as CustomPagesModel;

            Assert.IsNotNull(sut);

            Assert.AreEqual(4, sut.CustomPages.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditCustomPage_Validate_Attributes()
        {
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.EditPage)));
            Assert.IsTrue(MethodHasAttribute<BreadcrumbAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.EditPage)));
            Assert.IsTrue(MethodHasAttribute<LoggedInAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.EditPage)));

            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.EditPage)));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.EditPage)));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.EditPage)));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.EditPage)));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditCustomPage_InvalidPageId_RedirectsToGetcustomPages()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.EditPage(-1);

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult sut = response as RedirectToActionResult;

            Assert.AreEqual(nameof(DynamicContentController.GetCustomPages), sut.ActionName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditCustomPage_ReturnsValidCustomPage()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController(breadcrumbs: GetDynamicBreadcrumbs());

            IActionResult response = dynamicContentController.EditPage(1);

            ViewResult viewResult = response as ViewResult;

            Assert.IsNotNull(viewResult.Model);

            Assert.IsInstanceOfType(viewResult.Model, typeof(EditPageModel));

            ValidateBaseModel(viewResult);

            EditPageModel sut = viewResult.Model as EditPageModel;

            Assert.IsNotNull(sut);
            Assert.AreEqual(1, sut.Id);
            Assert.AreEqual("Custom Page 1", sut.Name);
            Assert.AreEqual(1, sut.DynamicContents.Count);
            Assert.IsNotNull(sut.CacheId);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetContent_Validate_Attributes()
        {
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetContent)));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetContent)));
            Assert.IsTrue(MethodHasAttribute<LoggedInAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetContent)));
            Assert.IsTrue(MethodHasRouteAttribute(typeof(DynamicContentController), nameof(DynamicContentController.GetContent), "DynamicContent/GetContent/{*cacheId}"));

            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetContent)));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetContent)));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetContent)));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetContent_InvalidCahceId_Null_Returns_FailResult()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();
            IActionResult response = dynamicContentController.GetContent(null);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetContent_InvalidCahceId_EmptyString_Returns_FailResult()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();
            IActionResult response = dynamicContentController.GetContent("");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetContent_CacheItemNotFound_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();
            IActionResult response = dynamicContentController.GetContent("cachetest");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetContent_CacheItemFound_ReturnsSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory(), null, null);
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(2);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            IActionResult response = dynamicContentController.GetContent(editPageModel.CacheId);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(200, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.Success);
            Assert.AreEqual(GetContentData, sut.ResponseData);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetContent_CacheItemFound_EmptyPageReturnsEmptyString_Success()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory(), null, null);
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));
            IPluginClassesService pluginServices = _testDynamicContentPlugin as IPluginClassesService;
            MockDynamicContentProvider mockDynamicContentProvider = new MockDynamicContentProvider(pluginServices);
            mockDynamicContentProvider.AddPage(new DynamicContentPage(59)
            {
                Name = "test"
            });

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs(), mockDynamicContentProvider);

            IActionResult editPageResponse = dynamicContentController.EditPage(59);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            IActionResult response = dynamicContentController.GetContent(editPageModel.CacheId);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(200, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.Success);
            Assert.AreEqual("", sut.ResponseData);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateControlPosition_Validate_Attributes()
        {
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.UpdateControlPosition)));
            Assert.IsTrue(MethodHasAttribute<LoggedInAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.UpdateControlPosition)));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.UpdateControlPosition)));
            Assert.IsTrue(MethodHasRouteAttribute(typeof(DynamicContentController), nameof(DynamicContentController.UpdateControlPosition), "DynamicContent/UpdatePosition"));

            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.UpdateControlPosition)));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.UpdateControlPosition)));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.UpdateControlPosition)));
            Assert.IsFalse(MethodHasAttribute<BreadcrumbAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.UpdateControlPosition)));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateControlPosition_NullValue_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();
            IActionResult response = dynamicContentController.UpdateControlPosition(null);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateControlPosition_ValidModel_InvalidCacheId_Null_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();
            IActionResult response = dynamicContentController.UpdateControlPosition(new UpdatePositionModel());
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateControlPosition_ValidModel_InvalidCacheId_EmptyString_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();
            IActionResult response = dynamicContentController.UpdateControlPosition(new UpdatePositionModel() { CacheId = "" });
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateControlPosition_ValidModel_InvalidCacheId_NotFound_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();
            IActionResult response = dynamicContentController.UpdateControlPosition(new UpdatePositionModel()
            {
                CacheId = "blah",
                ControlId = "control-id"
            });
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateControlPosition_ValidModel_InvalidControlId_Null_ReturnsNonSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory(), null, null);
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(2);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            IActionResult response = dynamicContentController.UpdateControlPosition(new UpdatePositionModel() { CacheId = editPageModel.CacheId });
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateControlPosition_ValidModel_InvalidControlId_EmptyString_ReturnsNonSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory(), null, null);
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(2);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            IActionResult response = dynamicContentController.UpdateControlPosition(new UpdatePositionModel() { CacheId = editPageModel.CacheId, ControlId = "" });
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateControlPosition_ValidModel_InvalidControlId_NotFound_ReturnsNonSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory(), null, null);
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(2);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            IActionResult response = dynamicContentController.UpdateControlPosition(new UpdatePositionModel() { CacheId = editPageModel.CacheId, ControlId = "asdfasdfasdf" });
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateControlPosition_InalidCacheItem_NotValidJsonResponseModel_Returns404()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory(), null, null);
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;
            memoryCache.GetExtendingCache().Add(editPageModel.CacheId, new CacheItem(editPageModel.CacheId, "a string"), true);

            IActionResult response = dynamicContentController.UpdateControlPosition(new UpdatePositionModel()
            {
                CacheId = editPageModel.CacheId,
                ControlId = "control-2",
                Controls = new string[] { "control-8", "control-7" }
            });

            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateControlPosition_ValidModel_ValidControlId_PositionsUpdated_ReturnsSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory(), null, null);
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            IActionResult response = dynamicContentController.UpdateControlPosition(new UpdatePositionModel()
            {
                CacheId = editPageModel.CacheId,
                ControlId = "control-2",
                Controls = new string[] { "control-8", "control-7" }
            });

            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(200, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.Success);

            CacheItem cacheItem = memoryCache.GetExtendingCache().Get(editPageModel.CacheId);
            DynamicContentPage dynamicContentPage = cacheItem.Value as DynamicContentPage;
            Assert.AreEqual(10, dynamicContentPage.Content[0].SortOrder);
            Assert.AreEqual(11, dynamicContentPage.Content[1].SortOrder);
            Assert.AreEqual(12, dynamicContentPage.Content[2].SortOrder);
            Assert.AreEqual(13, dynamicContentPage.Content[3].SortOrder);
            Assert.AreEqual(14, dynamicContentPage.Content[4].SortOrder);
            Assert.AreEqual(15, dynamicContentPage.Content[5].SortOrder);
            Assert.AreEqual(1, dynamicContentPage.Content[6].SortOrder);
            Assert.AreEqual(0, dynamicContentPage.Content[7].SortOrder);
            Assert.AreEqual(16, dynamicContentPage.Content[8].SortOrder);
            Assert.AreEqual(17, dynamicContentPage.Content[9].SortOrder);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetWidthTypes_ReturnsSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory(), null, null);
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            List<LookupListItem> widthTypes = DynamicContentController.GetWidthTypes();
            Assert.AreEqual(3, widthTypes.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetHeightTypes_ReturnsSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory(), null, null);
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            List<LookupListItem> heightTypes = DynamicContentController.GetHeightTypes();
            Assert.AreEqual(3, heightTypes.Count);
        }

        #region Preview

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Preview_Validate_Attributes()
        {
            Assert.IsTrue(MethodHasAttribute<LoggedInAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.Preview)));
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.Preview)));
            Assert.IsTrue(MethodHasRouteAttribute(typeof(DynamicContentController), nameof(DynamicContentController.Preview), "DynamicContent/Preview/{cacheId}"));

            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.Preview)));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.Preview)));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.Preview)));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.Preview)));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Preview_InvalidCacheId_EmptyString_Returns_StatusCode400()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.Preview("");
            StatusCodeResult sut = response as StatusCodeResult;

            Assert.IsNotNull(sut);

            Assert.AreEqual(400, sut.StatusCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Preview_InvalidCacheId_Null_Returns_StatusCode400()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.Preview(null);
            StatusCodeResult sut = response as StatusCodeResult;

            Assert.IsNotNull(sut);

            Assert.AreEqual(400, sut.StatusCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Preview_InvalidCacheItem_NotIDynamicContentPage_Returns_StatusCode400()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory(), null, null);
            DefaultMemoryCache defaultMemoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));
            DynamicContentController dynamicContentController = CreateDynamicContentController(defaultMemoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);

            ValidateBaseModel(viewResult);

            EditPageModel editPageModel = viewResult.Model as EditPageModel;

            defaultMemoryCache.GetExtendingCache().Add(editPageModel.CacheId, new CacheItem(editPageModel.CacheId, "string"), true);

            IActionResult response = dynamicContentController.Preview(editPageModel.CacheId);
            StatusCodeResult sut = response as StatusCodeResult;

            Assert.IsNotNull(sut);

            Assert.AreEqual(400, sut.StatusCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Preview_InvalidCache_ItemNotFound_Returns_StatusCode400()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.Preview("test123");
            StatusCodeResult sut = response as StatusCodeResult;

            Assert.IsNotNull(sut);

            Assert.AreEqual(400, sut.StatusCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Preview_ValidCache_PreviewPageCreated_Returns_StatusCode200()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory(), null, null);
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);

            ValidateBaseModel(viewResult);

            EditPageModel editPageModel = viewResult.Model as EditPageModel;

            IActionResult response = dynamicContentController.Preview(editPageModel.CacheId);

            ViewResult sut = response as ViewResult;

            Assert.AreEqual("/Views/DynamicContent/Index.cshtml", sut.ViewName);
            Assert.IsInstanceOfType(sut.Model, typeof(PageModel));

            PageModel model = sut.Model as PageModel;
            Assert.AreEqual(PreviewContent, model.Content);
            Assert.AreEqual(null, sut.StatusCode);
        }

        #endregion Preview

        #region Template Editor

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void TemplateEditor_Validate_Attributes()
        {
            Assert.IsTrue(MethodHasAttribute<LoggedInAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TemplateEditor)));
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TemplateEditor)));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TemplateEditor)));
            Assert.IsTrue(MethodHasRouteAttribute(typeof(DynamicContentController), nameof(DynamicContentController.TemplateEditor), "DynamicContent/TemplateEditor/{cacheId}/{controlId}"));

            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TemplateEditor)));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TemplateEditor)));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TemplateEditor)));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void TemplateEditor_InvalidCachedId_Null_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.TemplateEditor(null, "control-1");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void TemplateEditor_InvalidCachedId_EmptyString_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.TemplateEditor("", "control-1");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void TemplateEditor_InvalidControlId_Null_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.TemplateEditor("cachename", null);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void TemplateEditor_InvalidCacheItem_NotDynamicContent_ReturnsNonSuccess()
        {
            DefaultMemoryCache defaultMemoryCache = new DefaultMemoryCache(new MockSettingsProvider(), true, DateTime.UtcNow.AddDays(10));
            defaultMemoryCache.GetExtendingCache().Add("cachename", new CacheItem("cachename", new List<string>()));
            DynamicContentController dynamicContentController = CreateDynamicContentController(defaultMemoryCache);

            IActionResult response = dynamicContentController.TemplateEditor("cachename", "my-control");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void TemplateEditor_InvalidControlId_EmptyString_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.TemplateEditor("cachename", "");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void TemplateEditor_InvalidCache_NotFound_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.TemplateEditor("cachename", "controlname");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void TemplateEditor_ValidCache_InvalidControlId_ReturnsSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory(), null, null);
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            IActionResult response = dynamicContentController.TemplateEditor(editPageModel.CacheId, "my-control");

            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void TemplateEditor_ValidCache_ValidControlId_ReturnsValidPartialView()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            IActionResult response = dynamicContentController.TemplateEditor(editPageModel.CacheId, "control-1");

            PartialViewResult sut = response as PartialViewResult;

            Assert.AreEqual("/Views/DynamicContent/_TemplateEditor.cshtml", sut.ViewName);
            Assert.IsInstanceOfType(sut.Model, typeof(EditTemplateModel));

            EditTemplateModel model = sut.Model as EditTemplateModel;

            Assert.AreEqual(DefaultActiveFrom, model.ActiveFrom);
            Assert.AreEqual(DefaultActiveTo, model.ActiveTo);
            Assert.AreEqual(-1, model.Height);
            Assert.AreEqual(DynamicContentHeightType.Automatic, model.HeightType);
            Assert.AreEqual("control-1", model.UniqueId);
            Assert.AreEqual(12, model.Width);
            Assert.AreEqual(DynamicContentWidthType.Columns, model.WidthType);
            Assert.AreEqual("/DynamicContent/TextTemplateEditor/", model.TemplateEditor);
        }

        #endregion Template Editor

        #region Update Template

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateTemplate_Validate_Attributes()
        {
            Assert.IsTrue(MethodHasAttribute<LoggedInAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.UpdateTemplate)));
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.UpdateTemplate)));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.UpdateTemplate)));

            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.UpdateTemplate)));
            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.UpdateTemplate)));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.UpdateTemplate)));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.UpdateTemplate)));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateTemplate_InvalidModel_Returns_NonSucess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.UpdateTemplate(null);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateTemplate_InvalidCacheId_Null_Returns_NonSucess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            EditTemplateModel model = new EditTemplateModel();

            IActionResult response = dynamicContentController.UpdateTemplate(model);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateTemplate_InvalidCacheId_EmptyString_Returns_NonSucess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            EditTemplateModel model = new EditTemplateModel()
            {
                CacheId = ""
            };

            IActionResult response = dynamicContentController.UpdateTemplate(model);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateTemplate_InvalidUniqueId_Null_Returns_NonSucess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            EditTemplateModel model = new EditTemplateModel()
            {
                CacheId = "test"
            };

            IActionResult response = dynamicContentController.UpdateTemplate(model);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateTemplate_InvalidUniqueId_EmptyString_Returns_NonSucess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            EditTemplateModel model = new EditTemplateModel()
            {
                CacheId = "test",
                UniqueId = ""
            };

            IActionResult response = dynamicContentController.UpdateTemplate(model);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateTemplate_InvalidCacheId_CacheNotFound_Returns_NonSucess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            EditTemplateModel model = new EditTemplateModel()
            {
                CacheId = "test",
                UniqueId = "control-7"
            };

            IActionResult response = dynamicContentController.UpdateTemplate(model);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateTemplate_InvalidCache_NotValidJsonResponseModel_Returns_NonSucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache defaultMemoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));
            defaultMemoryCache.GetExtendingCache().Add("test", new CacheItem("test", "string"));
            DynamicContentController dynamicContentController = CreateDynamicContentController(defaultMemoryCache);

            EditTemplateModel model = new EditTemplateModel()
            {
                CacheId = "test",
                UniqueId = "control-7"
            };

            IActionResult response = dynamicContentController.UpdateTemplate(model);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateTemplate_InvalidControlId_ControlNotFound_Returns_NonSucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(1);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            EditTemplateModel model = new EditTemplateModel()
            {
                CacheId = editPageModel.CacheId,
                UniqueId = "control-7"
            };

            IActionResult response = dynamicContentController.UpdateTemplate(model);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateTemplate_InvalidWidth_TooManyColumns_Returns_NonSucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            EditTemplateModel model = new EditTemplateModel()
            {
                CacheId = editPageModel.CacheId,
                UniqueId = "control-7",
                WidthType = DynamicContentWidthType.Columns,
                Width = 40
            };

            IActionResult response = dynamicContentController.UpdateTemplate(model);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(200, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
            Assert.AreEqual("Width must be between 1 and 12", sut.ResponseData);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateTemplate_InvalidWidth_NotEnoughColumns_Returns_NonSucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            EditTemplateModel model = new EditTemplateModel()
            {
                CacheId = editPageModel.CacheId,
                UniqueId = "control-7",
                WidthType = DynamicContentWidthType.Columns,
                Width = 0
            };

            IActionResult response = dynamicContentController.UpdateTemplate(model);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(200, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
            Assert.AreEqual("Width must be between 1 and 12", sut.ResponseData);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateTemplate_InvalidWidth_Below1Percent_Returns_NonSucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            EditTemplateModel model = new EditTemplateModel()
            {
                CacheId = editPageModel.CacheId,
                UniqueId = "control-7",
                WidthType = DynamicContentWidthType.Percentage,
                Width = 0
            };

            IActionResult response = dynamicContentController.UpdateTemplate(model);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(200, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
            Assert.AreEqual("Width must be between 1 and 100", sut.ResponseData);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateTemplate_ValidHeight_PixelsOne_Returns_NonSucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            EditTemplateModel model = new EditTemplateModel()
            {
                CacheId = editPageModel.CacheId,
                UniqueId = "control-7",
                WidthType = DynamicContentWidthType.Percentage,
                Width = 10,
                HeightType = DynamicContentHeightType.Pixels,
                Height = 1
            };

            IActionResult response = dynamicContentController.UpdateTemplate(model);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(200, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.Success);
            Assert.AreEqual("", sut.ResponseData);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateTemplate_ValidHeight_PercentageOne_Returns_NonSucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            EditTemplateModel model = new EditTemplateModel()
            {
                CacheId = editPageModel.CacheId,
                UniqueId = "control-7",
                WidthType = DynamicContentWidthType.Percentage,
                Width = 10,
                HeightType = DynamicContentHeightType.Percentage,
                Height = 1
            };

            IActionResult response = dynamicContentController.UpdateTemplate(model);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(200, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.Success);
            Assert.AreEqual("", sut.ResponseData);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateTemplate_InvalidWidth_Above100Percent_Returns_NonSucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            EditTemplateModel model = new EditTemplateModel()
            {
                CacheId = editPageModel.CacheId,
                UniqueId = "control-7",
                WidthType = DynamicContentWidthType.Percentage,
                Width = 101
            };

            IActionResult response = dynamicContentController.UpdateTemplate(model);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(200, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
            Assert.AreEqual("Width must be between 1 and 100", sut.ResponseData);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateTemplate_InvalidWidth_Below1Pixel_Returns_NonSucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            EditTemplateModel model = new EditTemplateModel()
            {
                CacheId = editPageModel.CacheId,
                UniqueId = "control-7",
                WidthType = DynamicContentWidthType.Pixels,
                Width = 0
            };

            IActionResult response = dynamicContentController.UpdateTemplate(model);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(200, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
            Assert.AreEqual("Width must be at least 1", sut.ResponseData);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateTemplate_InvalidHeight_PercentageTooHigh_Returns_NonSucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            EditTemplateModel model = new EditTemplateModel()
            {
                CacheId = editPageModel.CacheId,
                UniqueId = "control-7",
                WidthType = DynamicContentWidthType.Columns,
                Width = 5,
                HeightType = DynamicContentHeightType.Percentage,
                Height = 101
            };

            IActionResult response = dynamicContentController.UpdateTemplate(model);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(200, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
            Assert.AreEqual("Height must be between 1 and 100", sut.ResponseData);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateTemplate_InvalidHeight_PercentageTooLow_Returns_NonSucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            EditTemplateModel model = new EditTemplateModel()
            {
                CacheId = editPageModel.CacheId,
                UniqueId = "control-7",
                WidthType = DynamicContentWidthType.Columns,
                Width = 5,
                HeightType = DynamicContentHeightType.Percentage,
                Height = 0
            };

            IActionResult response = dynamicContentController.UpdateTemplate(model);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(200, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
            Assert.AreEqual("Height must be between 1 and 100", sut.ResponseData);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateTemplate_InvalidHeight_NotEnoughPixels_Returns_NonSucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            EditTemplateModel model = new EditTemplateModel()
            {
                CacheId = editPageModel.CacheId,
                UniqueId = "control-7",
                WidthType = DynamicContentWidthType.Columns,
                Width = 5,
                HeightType = DynamicContentHeightType.Pixels,
                Height = 0
            };

            IActionResult response = dynamicContentController.UpdateTemplate(model);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(200, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
            Assert.AreEqual("Height must be at least 1", sut.ResponseData);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UpdateTemplate_Valid_Returns_Sucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            EditTemplateModel model = new EditTemplateModel()
            {
                CacheId = editPageModel.CacheId,
                UniqueId = "control-7",
                WidthType = DynamicContentWidthType.Percentage,
                Width = 33,
                HeightType = DynamicContentHeightType.Pixels,
                Height = 250,
                Data = "happy days"
            };

            IActionResult response = dynamicContentController.UpdateTemplate(model);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(200, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.Success);


            CacheItem cacheItem = memoryCache.GetExtendingCache().Get(model.CacheId);

            Assert.IsNotNull(cacheItem);

            DynamicContentPage dynamicContentPage = cacheItem.Value as DynamicContentPage;

            Assert.IsNotNull(dynamicContentPage);

            DynamicContentTemplate control = dynamicContentPage.Content.FirstOrDefault(ctl => ctl.UniqueId.Equals(model.UniqueId));

            Assert.IsNotNull(control);

            Assert.AreEqual("happy days", control.Data);
            Assert.AreEqual(-1, control.Height);
            Assert.AreEqual(DynamicContentHeightType.Automatic, control.HeightType);
            Assert.AreEqual(33, control.Width);
            Assert.AreEqual(DynamicContentWidthType.Percentage, control.WidthType);
        }

        #endregion Update Template

        #region TextTemplate

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void TextTemplateEditor_Validate_Attributes()
        {
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TextTemplateEditor)));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TextTemplateEditor)));

            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TextTemplateEditor)));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TextTemplateEditor)));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TextTemplateEditor)));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void TextTemplateEditor_Validate_IActionResult_ReturnsPartialView()
        {
            DynamicContentController sut = CreateDynamicContentController();

            IActionResult response = sut.TextTemplateEditor("hello world");

            Assert.IsInstanceOfType(response, typeof(PartialViewResult));

            PartialViewResult viewResult = response as PartialViewResult;

            Assert.IsNotNull(viewResult.Model);

            Assert.AreEqual("/Views/DynamicContent/_TextTemplateEditor.cshtml", viewResult.ViewName);

            Assert.IsInstanceOfType(viewResult.Model, typeof(TextTemplateEditorModel));
            TextTemplateEditorModel model = viewResult.Model as TextTemplateEditorModel;
            model.Data.Equals("hello world");
        }

        #endregion TextTemplate

        #region Delete Template

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteControl_Validate_Attributes()
        {
            Assert.IsTrue(MethodHasAttribute<LoggedInAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.DeleteControl)));
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.DeleteControl)));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.DeleteControl)));
            Assert.IsTrue(MethodHasRouteAttribute(typeof(DynamicContentController), nameof(DynamicContentController.DeleteControl), "DynamicContent/DeleteControl/{cacheId}/{controlId}"));

            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.DeleteControl)));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.DeleteControl)));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.DeleteControl)));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteControl_InvalidCachedId_Null_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.DeleteControl(null, "control-1");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteControl_InvalidCachedId_EmptyString_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.DeleteControl("", "control-1");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteControl_InvalidControlId_Null_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.DeleteControl("cachename", null);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteControl_InvalidControlId_EmptyString_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.DeleteControl("cachename", "");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteControl_InvalidCache_NotFound_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.DeleteControl("cachename", "controlname");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteControl_InvalidCache_NotJsonResponseModel_ReturnsNonSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache defaultMemoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));
            defaultMemoryCache.GetExtendingCache().Add("cachename", new CacheItem("cachename", "string"));
            DynamicContentController dynamicContentController = CreateDynamicContentController(defaultMemoryCache);

            IActionResult response = dynamicContentController.DeleteControl("cachename", "controlname");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteControl_ValidCache_InvalidControlId_ReturnsSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            IActionResult response = dynamicContentController.DeleteControl(editPageModel.CacheId, "my-control");

            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteControl_ValidCache_ValidControlId_ReturnsValidPartialView()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            IActionResult response = dynamicContentController.DeleteControl(editPageModel.CacheId, "control-1");

            PartialViewResult sut = response as PartialViewResult;

            Assert.AreEqual("/Views/DynamicContent/_DeleteControl.cshtml", sut.ViewName);
            Assert.IsInstanceOfType(sut.Model, typeof(DeleteControlModel));

            DeleteControlModel model = sut.Model as DeleteControlModel;

            Assert.IsNotNull(model);
            Assert.AreEqual("control-1", model.ControlId);
            Assert.AreEqual(editPageModel.CacheId, model.CacheId);
        }

        #endregion Delete Template

        #region Delete Item

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteItem_Validate_Attributes()
        {
            Assert.IsTrue(MethodHasAttribute<LoggedInAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.DeleteItem)));
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.DeleteItem)));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.DeleteItem)));

            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.DeleteItem)));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.DeleteItem)));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.DeleteItem)));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.DeleteItem)));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteItem_InvalidModel_Null_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.DeleteItem(null);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteItem_InvalidCachedId_Null_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            DeleteControlModel deleteControlModel = new DeleteControlModel()
            {
                CacheId = null,
                ControlId = "adsf"
            };

            IActionResult response = dynamicContentController.DeleteItem(deleteControlModel);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteItem_InvalidCachedId_EmptyString_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            DeleteControlModel deleteControlModel = new DeleteControlModel()
            {
                CacheId = "",
                ControlId = "adsf"
            };

            IActionResult response = dynamicContentController.DeleteItem(deleteControlModel);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteItem_InvalidControlId_Null_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            DeleteControlModel deleteControlModel = new DeleteControlModel()
            {
                CacheId = "adf2",
                ControlId = null
            };

            IActionResult response = dynamicContentController.DeleteItem(deleteControlModel);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteItem_InvalidControlId_EmptyString_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            DeleteControlModel deleteControlModel = new DeleteControlModel()
            {
                CacheId = "asdf",
                ControlId = ""
            };

            IActionResult response = dynamicContentController.DeleteItem(deleteControlModel);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteItem_InvalidCache_NotFound_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            DeleteControlModel deleteControlModel = new DeleteControlModel()
            {
                CacheId = "test",
                ControlId = "adsf"
            };

            IActionResult response = dynamicContentController.DeleteItem(deleteControlModel);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteItem_InvalidCache_NotJsonResponseModel_ReturnsNonSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache defaultMemoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));
            defaultMemoryCache.GetExtendingCache().Add("test", new CacheItem("test", "string"));
            DynamicContentController dynamicContentController = CreateDynamicContentController(defaultMemoryCache);

            DeleteControlModel deleteControlModel = new DeleteControlModel()
            {
                CacheId = "test",
                ControlId = "adsf"
            };

            IActionResult response = dynamicContentController.DeleteItem(deleteControlModel);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteItem_ValidCache_InvalidControlId_ReturnsSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;

            DeleteControlModel deleteControlModel = new DeleteControlModel()
            {
                CacheId = editPageModel.CacheId,
                ControlId = "adsf"
            };

            IActionResult response = dynamicContentController.DeleteItem(deleteControlModel);

            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteItem_ValidCache_ValidControlId_ReturnsValidPartialView()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;

            CacheItem cacheItem = memoryCache.GetExtendingCache().Get(editPageModel.CacheId);
            DynamicContentPage dynamicContentPage = cacheItem.Value as DynamicContentPage;

            Assert.IsNotNull(dynamicContentPage);
            Assert.AreEqual(10, dynamicContentPage.Content.Count);

            DeleteControlModel deleteControlModel = new DeleteControlModel()
            {
                CacheId = editPageModel.CacheId,
                ControlId = "control-1"
            };

            IActionResult response = dynamicContentController.DeleteItem(deleteControlModel);

            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(200, jsonResult.StatusCode);

            JsonResponseModel sut = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.Success);

            Assert.AreEqual(9, dynamicContentPage.Content.Count);
            Assert.IsFalse(dynamicContentPage.Content.Where(dcp => dcp.UniqueId.Equals("control-1")).Any());
        }

        #endregion Delete Item

        #region Retrieve Templates

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetTemplates_Validate_Attributes()
        {
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetTemplates)));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetTemplates)));
            Assert.IsTrue(MethodHasAttribute<LoggedInAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetTemplates)));

            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetTemplates)));
            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetTemplates)));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetTemplates)));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetTemplates)));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetTemplates_ReturnsValidTemplatesList_Success()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.GetTemplates();

            PartialViewResult viewResult = response as PartialViewResult;

            Assert.IsNotNull(viewResult.Model);

            Assert.IsInstanceOfType(viewResult.Model, typeof(TemplatesModel));

            TemplatesModel sut = viewResult.Model as TemplatesModel;

            Assert.IsNotNull(sut);

            Assert.AreEqual(14, sut.Templates.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetTemplates_ReturnsValidViewName()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.GetTemplates();

            PartialViewResult viewResult = response as PartialViewResult;

            Assert.AreEqual("/Views/DynamicContent/_Templates.cshtml", viewResult.ViewName);
        }

        #endregion Retrieve Templates

        #region Add Template To Page

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AddTemplateToPage_Validate_Attributes()
        {
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.AddTemplateToPage)));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.AddTemplateToPage)));
            Assert.IsTrue(MethodHasAttribute<LoggedInAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.AddTemplateToPage)));
            Assert.IsTrue(MethodHasRouteAttribute(typeof(DynamicContentController), nameof(DynamicContentController.AddTemplateToPage), "DynamicContent/AddTemplate/"));

            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.AddTemplateToPage)));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.AddTemplateToPage)));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.AddTemplateToPage)));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AddTemplateToPage_NullModel_Returns400()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.AddTemplateToPage(null);

            JsonResult sut = response as JsonResult;

            Assert.AreEqual(400, sut.StatusCode.Value);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AddTemplateToPage_NullCacheId_Returns400()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            AddControlModel model = new AddControlModel()
            {
                CacheId = null,
                TemplateId = "123",
                NextControl = ""
            };

            IActionResult response = dynamicContentController.AddTemplateToPage(model);

            JsonResult sut = response as JsonResult;

            Assert.AreEqual(400, sut.StatusCode.Value);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AddTemplateToPage_EmptyCacheId_Returns400()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            AddControlModel model = new AddControlModel()
            {
                CacheId = "",
                TemplateId = "123",
                NextControl = ""
            };

            IActionResult response = dynamicContentController.AddTemplateToPage(model);

            JsonResult sut = response as JsonResult;

            Assert.AreEqual(400, sut.StatusCode.Value);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AddTemplateToPage_NullTemplateIdId_Returns400()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            AddControlModel model = new AddControlModel()
            {
                CacheId = "123",
                TemplateId = null,
                NextControl = ""
            };

            IActionResult response = dynamicContentController.AddTemplateToPage(model);

            JsonResult sut = response as JsonResult;

            Assert.AreEqual(400, sut.StatusCode.Value);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AddTemplateToPage_EmptyTemplateId_Returns400()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            AddControlModel model = new AddControlModel()
            {
                CacheId = "123",
                TemplateId = "",
                NextControl = ""
            };

            IActionResult response = dynamicContentController.AddTemplateToPage(model);

            JsonResult sut = response as JsonResult;

            Assert.AreEqual(400, sut.StatusCode.Value);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AddTemplateToPage_CacheItemDoesNotExist_Returns400()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            AddControlModel model = new AddControlModel()
            {
                CacheId = "123",
                TemplateId = "123",
                NextControl = ""
            };

            IActionResult response = dynamicContentController.AddTemplateToPage(model);

            JsonResult sut = response as JsonResult;

            Assert.AreEqual(400, sut.StatusCode.Value);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AddTemplateToPage_CacheItemIsNotDynamicContentPage_Returns400()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;

            memoryCache.GetExtendingCache().Add(editPageModel.CacheId, new CacheItem(editPageModel.CacheId, "a string"), true);

            AddControlModel model = new AddControlModel()
            {
                CacheId = editPageModel.CacheId,
                TemplateId = "123",
                NextControl = ""
            };

            IActionResult response = dynamicContentController.AddTemplateToPage(model);

            JsonResult sut = response as JsonResult;

            Assert.AreEqual(400, sut.StatusCode.Value);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AddTemplateToPage_TemplateIdNotFound_Returns400()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;

            AddControlModel model = new AddControlModel()
            {
                CacheId = editPageModel.CacheId,
                TemplateId = "123",
                NextControl = ""
            };

            IActionResult response = dynamicContentController.AddTemplateToPage(model);

            JsonResult sut = response as JsonResult;

            Assert.AreEqual(400, sut.StatusCode.Value);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AddTemplateToPage_TemplateIdFoundAndAdded_ReturnsPartialViewContainingNewTemplate()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            var templates = dynamicContentController.GetTemplates();
            PartialViewResult templateResult = templates as PartialViewResult;
            Assert.IsNotNull(templateResult.Model);
            Assert.IsInstanceOfType(templateResult.Model, typeof(TemplatesModel));
            TemplatesModel templatesModel = templateResult.Model as TemplatesModel;
            TemplateModel template = templatesModel.Templates.FirstOrDefault(t => t.TemplateName.Equals("Spacer"));
            Assert.IsNotNull(template);

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;

            string currentTemplateIdPosition3 = editPageModel.DynamicContents[3].UniqueId;
            Assert.AreEqual(10, editPageModel.DynamicContents.Count);

            AddControlModel model = new AddControlModel()
            {
                CacheId = editPageModel.CacheId,
                TemplateId = template.TemplateId,
                NextControl = editPageModel.DynamicContents[3].UniqueId
            };

            IActionResult response = dynamicContentController.AddTemplateToPage(model);

            ViewResult addTemplateViewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(addTemplateViewResult.Model);
            EditPageModel addTemplatePageModel = addTemplateViewResult.Model as EditPageModel;

            Assert.IsNotNull(addTemplatePageModel);

            Assert.AreEqual(11, addTemplatePageModel.DynamicContents.Count);
            Assert.AreNotEqual(currentTemplateIdPosition3, addTemplatePageModel.DynamicContents[3].UniqueId);
            Assert.AreEqual("Spacer", addTemplatePageModel.DynamicContents[3].Name);
        }

        #endregion Add Template To Page

        #region SavePage

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SavePage_Validate_Attributes()
        {
            string methodName = "SavePage";
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsTrue(MethodHasAttribute<LoggedInAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsTrue(MethodHasAuthorizeAttribute(typeof(DynamicContentController), methodName, "ContentEditor"));
            Assert.IsTrue(MethodHasRouteAttribute(typeof(DynamicContentController), methodName, "DynamicContent/EditPage"));

            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), methodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SavePage_InvalidModel_Null_RedirectsToGetCustomPages_Success()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());
            IActionResult savePageResponse = dynamicContentController.SavePage(null);
            RedirectToActionResult redirectResult = savePageResponse as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("GetCustomPages", redirectResult.ActionName);
            Assert.IsFalse(redirectResult.Permanent);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SavePage_InvalidModel_CacheIdNotFound_RedirectsToGetCustomPages_Success()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            EditPageModel model = new EditPageModel()
            {
                CacheId = "not found"
            };

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());
            IActionResult savePageResponse = dynamicContentController.SavePage(model);
            RedirectToActionResult redirectResult = savePageResponse as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("GetCustomPages", redirectResult.ActionName);
            Assert.IsFalse(redirectResult.Permanent);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SavePage_ValidModel_CacheIdFound_NotValidDynamicPage_RedirectsToGetCustomPages_Success()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));
            memoryCache.GetExtendingCache().Add("notIDynamicContentPage",
                new CacheItem("notIDynamicContentPage", new List<string>()));
            EditPageModel model = new EditPageModel()
            {
                CacheId = "notIDynamicContentPage"
            };

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());
            IActionResult savePageResponse = dynamicContentController.SavePage(model);
            RedirectToActionResult redirectResult = savePageResponse as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("GetCustomPages", redirectResult.ActionName);
            Assert.IsFalse(redirectResult.Permanent);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SavePage_PageNameAlreadyExists_AddsErrorToModelState_Success()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentPage cachedPage = new DynamicContentPage()
            {
                Id = 2,
                Name = "My Page 2",
                RouteName = "my-route-2"
            };

            memoryCache.GetExtendingCache().Add("abc", new CacheItem("abc", cachedPage));


            IPluginClassesService pluginServices = _testDynamicContentPlugin as IPluginClassesService;
            DynamicContentPage dynamicContentPageExistingName = new DynamicContentPage()
            {
                Id = 1,
                Name = "My Page",
                RouteName = "my-route"
            };
            MockDynamicContentProvider mockDynamicContentProvider = new MockDynamicContentProvider(pluginServices);
            mockDynamicContentProvider.UseDefaultContent = false;
            mockDynamicContentProvider.AllowSavePage = true;
            mockDynamicContentProvider.AddPage(dynamicContentPageExistingName);

            EditPageModel model = new EditPageModel()
            {
                CacheId = "abc",
                Name = "My Page",
                RouteName = "my-route"
            };

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs(), mockDynamicContentProvider);
            IActionResult savePageResponse = dynamicContentController.SavePage(model);
            ViewResult viewResult = savePageResponse as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("/Views/DynamicContent/EditPage.cshtml", viewResult.ViewName);
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
            Assert.AreEqual(1, viewResult.ViewData.ModelState.ErrorCount);
            Assert.IsFalse(ViewResultContainsModelStateError(viewResult, "", "Failed to save page"));
            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "Name", "Name already exists"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SavePage_RouteAlreadyExists_AcceptableAsStartStopTimeMayVary_Success()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentPage cachedPage = new DynamicContentPage()
            {
                Id = 2,
                Name = "My Page 2",
                RouteName = "my-route-2"
            };

            memoryCache.GetExtendingCache().Add("abc", new CacheItem("abc", cachedPage));


            IPluginClassesService pluginServices = _testDynamicContentPlugin as IPluginClassesService;
            DynamicContentPage dynamicContentPageExistingName = new DynamicContentPage()
            {
                Id = 1,
                Name = "My Page",
                RouteName = "my-route"
            };
            MockDynamicContentProvider mockDynamicContentProvider = new MockDynamicContentProvider(pluginServices);
            mockDynamicContentProvider.UseDefaultContent = false;
            mockDynamicContentProvider.AllowSavePage = true;
            mockDynamicContentProvider.AddPage(dynamicContentPageExistingName);

            EditPageModel model = new EditPageModel()
            {
                CacheId = "abc",
                Name = "My Page 3",
                RouteName = "my-route"
            };

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs(), mockDynamicContentProvider);
            IActionResult savePageResponse = dynamicContentController.SavePage(model);
            RedirectToActionResult redirectResult = savePageResponse as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.IsFalse(redirectResult.Permanent);
            Assert.AreEqual("My Page 3", cachedPage.Name);
            Assert.AreEqual("my-route", cachedPage.RouteName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SavePage_PageFailedToSave_AddsErrorToModelState_Success()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentPage cachedPage = new DynamicContentPage()
            {
                Id = 2,
                Name = "My Page 2",
                RouteName = "my-route-2"
            };

            memoryCache.GetExtendingCache().Add("abc", new CacheItem("abc", cachedPage));


            IPluginClassesService pluginServices = _testDynamicContentPlugin as IPluginClassesService;
            MockDynamicContentProvider mockDynamicContentProvider = new MockDynamicContentProvider(pluginServices);
            mockDynamicContentProvider.UseDefaultContent = false;
            mockDynamicContentProvider.AllowSavePage = false;

            EditPageModel model = new EditPageModel()
            {
                CacheId = "abc",
                Name = "My Page",
                RouteName = "my-route"
            };

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs(), mockDynamicContentProvider);
            IActionResult savePageResponse = dynamicContentController.SavePage(model);
            ViewResult viewResult = savePageResponse as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("/Views/DynamicContent/EditPage.cshtml", viewResult.ViewName);
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
            Assert.AreEqual(1, viewResult.ViewData.ModelState.ErrorCount);
            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "", "Failed to save page"));
            Assert.IsFalse(ViewResultContainsModelStateError(viewResult, "Name", "Name already exists"));
            Assert.IsFalse(ViewResultContainsModelStateError(viewResult, "RouteName", "Route name already exists"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SavePage_PageFailedToSave_ContainsModelErrorsWhenCalled_Success()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));

            DynamicContentPage cachedPage = new DynamicContentPage()
            {
                Id = 2,
                Name = "My Page 2",
                RouteName = "my-route-2"
            };

            memoryCache.GetExtendingCache().Add("abc", new CacheItem("abc", cachedPage));


            IPluginClassesService pluginServices = _testDynamicContentPlugin as IPluginClassesService;
            MockDynamicContentProvider mockDynamicContentProvider = new MockDynamicContentProvider(pluginServices);
            mockDynamicContentProvider.UseDefaultContent = false;
            mockDynamicContentProvider.AllowSavePage = false;

            EditPageModel model = new EditPageModel()
            {
                CacheId = "abc",
                Name = "My Page",
                RouteName = "my-route"
            };

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs(), mockDynamicContentProvider);
            dynamicContentController.ModelState.AddModelError("keyName", "an error");
            IActionResult savePageResponse = dynamicContentController.SavePage(model);
            ViewResult viewResult = savePageResponse as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("/Views/DynamicContent/EditPage.cshtml", viewResult.ViewName);
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
            Assert.AreEqual(1, viewResult.ViewData.ModelState.ErrorCount);
            Assert.IsFalse(ViewResultContainsModelStateError(viewResult, "", "Failed to save page"));
            Assert.IsFalse(ViewResultContainsModelStateError(viewResult, "Name", "Name already exists"));
            Assert.IsFalse(ViewResultContainsModelStateError(viewResult, "RouteName", "Route name already exists"));
            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "keyName", "an error"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SavePage_PageSavedSuccessfully_Success()
        {
            DateTime activeFrom = DateTime.Now.AddDays(-1);
            DateTime activeTo = DateTime.Now.AddDays(3);

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10));
            MockNotificationService testNotificationService = new MockNotificationService();

            DynamicContentPage cachedPage = new DynamicContentPage()
            {
                Id = 2,
                Name = "My Page 2",
                RouteName = "my-route-2",
                ActiveFrom = DateTime.MinValue,
                ActiveTo = DateTime.MaxValue,
            };

            memoryCache.GetExtendingCache().Add("abc", new CacheItem("abc", cachedPage));

            IPluginClassesService pluginServices = _testDynamicContentPlugin as IPluginClassesService;
            MockDynamicContentProvider mockDynamicContentProvider = new MockDynamicContentProvider(pluginServices);
            mockDynamicContentProvider.UseDefaultContent = false;
            mockDynamicContentProvider.AllowSavePage = true;

            EditPageModel model = new EditPageModel()
            {
                CacheId = "abc",
                Name = "My Page",
                RouteName = "my-route",
                ActiveFrom = activeFrom,
                ActiveTo = activeTo,
            };

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs(),
                mockDynamicContentProvider, testNotificationService);
            IActionResult savePageResponse = dynamicContentController.SavePage(model);
            RedirectToActionResult redirectResult = savePageResponse as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.IsFalse(redirectResult.Permanent);
            Assert.AreEqual("My Page", cachedPage.Name);
            Assert.AreEqual("my-route", cachedPage.RouteName);
            Assert.AreEqual(activeFrom, cachedPage.ActiveFrom);
            Assert.AreEqual(activeTo, cachedPage.ActiveTo);
            Assert.AreEqual(1, testNotificationService.NotificationRaised("DynamicContentUpdated"));
        }

        #endregion SavePage

        #region New Page


        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void NewPage_Validate_Attributes()
        {
            string methodName = "NewPage";

            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsTrue(MethodHasAttribute<LoggedInAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsTrue(MethodHasAuthorizeAttribute(typeof(DynamicContentController), methodName, "ContentEditor"));

            Assert.IsFalse(MethodHasAttribute<BreadcrumbAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), methodName));
        }


        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void NewPage_CreatesANewPage_RedirectsToEditPage()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.NewPage();

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult sut = response as RedirectToActionResult;

            Assert.AreEqual("EditPage", sut.ActionName);
            Assert.AreEqual(1, sut.RouteValues.Keys.Count);
            Assert.IsTrue(sut.RouteValues.Keys.Contains("id"));
            Assert.AreEqual(50L, sut.RouteValues["id"]);
        }


        #endregion New Page

        #region YouTube Template Editor

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void YouTubeTemplateEditor_Validate_Attributes()
        {
            string methodName = "YouTubeTemplateEditor";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), methodName));

            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), methodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void YouTubeTemplateEditor_ReturnsValidViewAndModel_Success()
        {
            DynamicContentController sut = CreateDynamicContentController();

            IActionResult response = sut.YouTubeTemplateEditor("hello world");

            Assert.IsInstanceOfType(response, typeof(PartialViewResult));

            PartialViewResult viewResult = response as PartialViewResult;

            Assert.IsNotNull(viewResult.Model);

            Assert.AreEqual("/Views/DynamicContent/_YouTubeTemplateEditor.cshtml", viewResult.ViewName);

            Assert.IsInstanceOfType(viewResult.Model, typeof(YouTubeTemplateEditorModel));
            YouTubeTemplateEditorModel model = viewResult.Model as YouTubeTemplateEditorModel;
            Assert.AreEqual("hello world", model.Data);
            Assert.IsFalse(model.AutoPlay);
            Assert.AreEqual("hello world", model.VideoId);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void YouTubeTemplateEditor_ReturnsValidViewAndModel_WithAutoplay_Success()
        {
            DynamicContentController sut = CreateDynamicContentController();

            IActionResult response = sut.YouTubeTemplateEditor("ZzSsBbNnJjKK|TrUe");

            Assert.IsInstanceOfType(response, typeof(PartialViewResult));

            PartialViewResult viewResult = response as PartialViewResult;

            Assert.IsNotNull(viewResult.Model);

            Assert.AreEqual("/Views/DynamicContent/_YouTubeTemplateEditor.cshtml", viewResult.ViewName);

            Assert.IsInstanceOfType(viewResult.Model, typeof(YouTubeTemplateEditorModel));
            YouTubeTemplateEditorModel model = viewResult.Model as YouTubeTemplateEditorModel;
            Assert.AreEqual("ZzSsBbNnJjKK|TrUe", model.Data);
            Assert.IsTrue(model.AutoPlay);
            Assert.AreEqual("ZzSsBbNnJjKK", model.VideoId);
        }

        #endregion YouTube Template Editor

        #region Form Input Template Editor

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void FormControlTemplateEditor_Validate_Attributes()
        {
            string methodName = "FormControlTemplateEditor";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), methodName));

            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), methodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void FormControlTemplateEditor_ReturnsValidViewAndModel_Success()
        {
            DynamicContentController sut = CreateDynamicContentController();

            IActionResult response = sut.FormControlTemplateEditor("hello world");

            Assert.IsInstanceOfType(response, typeof(PartialViewResult));

            PartialViewResult viewResult = response as PartialViewResult;

            Assert.IsNotNull(viewResult.Model);

            Assert.AreEqual("/Views/DynamicContent/_FormControlTemplateEditor.cshtml", viewResult.ViewName);

            Assert.IsInstanceOfType(viewResult.Model, typeof(FormTemplateEditorModel));
            FormTemplateEditorModel model = viewResult.Model as FormTemplateEditorModel;
            Assert.AreEqual("hello world", model.Data);
            Assert.AreEqual("hello-world", model.ControlName);
            Assert.AreEqual("", model.LabelText);
            Assert.AreEqual("Align label to the left", model.AlignLeftText);
            Assert.IsFalse(model.AlignTop);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void FormControlTemplateEditor_ReturnsValidViewAndModel_WithAutoplay_Success()
        {
            DynamicContentController sut = CreateDynamicContentController();

            IActionResult response = sut.FormControlTemplateEditor("ZzSsBbNnJjKK|MyControl|TrUe");

            Assert.IsInstanceOfType(response, typeof(PartialViewResult));

            PartialViewResult viewResult = response as PartialViewResult;

            Assert.IsNotNull(viewResult.Model);

            Assert.AreEqual("/Views/DynamicContent/_FormControlTemplateEditor.cshtml", viewResult.ViewName);

            Assert.IsInstanceOfType(viewResult.Model, typeof(FormTemplateEditorModel));
            FormTemplateEditorModel model = viewResult.Model as FormTemplateEditorModel;
            Assert.AreEqual("ZzSsBbNnJjKK|MyControl|TrUe", model.Data);
            Assert.AreEqual("ZzSsBbNnJjKK", model.ControlName);
            Assert.AreEqual("MyControl", model.LabelText);
            Assert.AreEqual("Align label to the left", model.AlignLeftText);
            Assert.IsTrue(model.AlignTop);
        }

        #endregion Form Input Template Editor

        #region Form Input Template Editor Right Align

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void FormControlTemplateEditorRightAlign_Validate_Attributes()
        {
            string methodName = "FormControlTemplateEditorRightAlign";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), methodName));

            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), methodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void FormControlTemplateEditorRightAlign_ReturnsValidViewAndModel_Success()
        {
            DynamicContentController sut = CreateDynamicContentController();

            IActionResult response = sut.FormControlTemplateEditorRightAlign("hello world");

            Assert.IsInstanceOfType(response, typeof(PartialViewResult));

            PartialViewResult viewResult = response as PartialViewResult;

            Assert.IsNotNull(viewResult.Model);

            Assert.AreEqual("/Views/DynamicContent/_FormControlTemplateEditor.cshtml", viewResult.ViewName);

            Assert.IsInstanceOfType(viewResult.Model, typeof(FormTemplateEditorModel));
            FormTemplateEditorModel model = viewResult.Model as FormTemplateEditorModel;
            Assert.AreEqual("hello world", model.Data);
            Assert.AreEqual("hello-world", model.ControlName);
            Assert.AreEqual("", model.LabelText);
            Assert.AreEqual("Align label to the right", model.AlignLeftText);
            Assert.IsFalse(model.AlignTop);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void FormControlTemplateEditorRightAlign_ReturnsValidViewAndModel_WithAutoplay_Success()
        {
            DynamicContentController sut = CreateDynamicContentController();

            IActionResult response = sut.FormControlTemplateEditorRightAlign("ZzSsBbNnJjKK|MyControl|TrUe");

            Assert.IsInstanceOfType(response, typeof(PartialViewResult));

            PartialViewResult viewResult = response as PartialViewResult;

            Assert.IsNotNull(viewResult.Model);

            Assert.AreEqual("/Views/DynamicContent/_FormControlTemplateEditor.cshtml", viewResult.ViewName);

            Assert.IsInstanceOfType(viewResult.Model, typeof(FormTemplateEditorModel));
            FormTemplateEditorModel model = viewResult.Model as FormTemplateEditorModel;
            Assert.AreEqual("ZzSsBbNnJjKK|MyControl|TrUe", model.Data);
            Assert.AreEqual("ZzSsBbNnJjKK", model.ControlName);
            Assert.AreEqual("MyControl", model.LabelText);
            Assert.AreEqual("Align label to the right", model.AlignLeftText);
            Assert.IsTrue(model.AlignTop);
        }

        #endregion Form Input Template Editor Right Align

        #region Form Input Template Editor Radio Group

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void FormControlTemplateEditorRadioGroup_Validate_Attributes()
        {
            string methodName = "FormControlTemplateEditorRadioGroup";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), methodName));

            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), methodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void FormControlTemplateEditorRadioGroup_ReturnsValidViewAndModel_Success()
        {
            DynamicContentController sut = CreateDynamicContentController();

            IActionResult response = sut.FormControlTemplateEditorRadioGroup("hello world");

            Assert.IsInstanceOfType(response, typeof(PartialViewResult));

            PartialViewResult viewResult = response as PartialViewResult;

            Assert.IsNotNull(viewResult.Model);

            Assert.AreEqual("/Views/DynamicContent/_FormControlTemplateEditorRadioGroup.cshtml", viewResult.ViewName);

            Assert.IsInstanceOfType(viewResult.Model, typeof(FormTemplateEditorModel));
            FormTemplateEditorModel model = viewResult.Model as FormTemplateEditorModel;
            Assert.AreEqual("hello world", model.Data);
            Assert.AreEqual("hello-world", model.ControlName);
            Assert.AreEqual("", model.LabelText);
            Assert.AreEqual("Align label to the right", model.AlignLeftText);
            Assert.IsFalse(model.AlignTop);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void FormControlTemplateEditorRadioGroup_ReturnsValidViewAndModel_WithAutoplay_Success()
        {
            DynamicContentController sut = CreateDynamicContentController();

            IActionResult response = sut.FormControlTemplateEditorRadioGroup("ZzSsBbNnJjKK|MyControl|TrUe");

            Assert.IsInstanceOfType(response, typeof(PartialViewResult));

            PartialViewResult viewResult = response as PartialViewResult;

            Assert.IsNotNull(viewResult.Model);

            Assert.AreEqual("/Views/DynamicContent/_FormControlTemplateEditorRadioGroup.cshtml", viewResult.ViewName);

            Assert.IsInstanceOfType(viewResult.Model, typeof(FormTemplateEditorModel));
            FormTemplateEditorModel model = viewResult.Model as FormTemplateEditorModel;
            Assert.AreEqual("ZzSsBbNnJjKK|MyControl|TrUe", model.Data);
            Assert.AreEqual("ZzSsBbNnJjKK", model.ControlName);
            Assert.AreEqual("MyControl", model.LabelText);
            Assert.AreEqual("Align label to the right", model.AlignLeftText);
            Assert.IsTrue(model.AlignTop);
        }

        #endregion Form Input Template Editor Radio Group

        #region Form Input Template Editor List Box

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void FormControlTemplateEditorListBox_Validate_Attributes()
        {
            string methodName = "FormControlTemplateEditorListBox";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), methodName));

            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), methodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void FormControlTemplateEditorListBox_ReturnsValidViewAndModel_Success()
        {
            DynamicContentController sut = CreateDynamicContentController();

            IActionResult response = sut.FormControlTemplateEditorListBox("hello world");

            Assert.IsInstanceOfType(response, typeof(PartialViewResult));

            PartialViewResult viewResult = response as PartialViewResult;

            Assert.IsNotNull(viewResult.Model);

            Assert.AreEqual("/Views/DynamicContent/_FormControlTemplateEditorListBox.cshtml", viewResult.ViewName);

            Assert.IsInstanceOfType(viewResult.Model, typeof(FormTemplateEditorModel));
            FormTemplateEditorModel model = viewResult.Model as FormTemplateEditorModel;
            Assert.AreEqual("hello world", model.Data);
            Assert.AreEqual("hello-world", model.ControlName);
            Assert.AreEqual("", model.LabelText);
            Assert.AreEqual("Align label to the left", model.AlignLeftText);
            Assert.IsFalse(model.AlignTop);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void FormControlTemplateEditorListBox_ReturnsValidViewAndModel_WithAutoplay_Success()
        {
            DynamicContentController sut = CreateDynamicContentController();

            IActionResult response = sut.FormControlTemplateEditorListBox("ZzSsBbNnJjKK|MyControl|TrUe");

            Assert.IsInstanceOfType(response, typeof(PartialViewResult));

            PartialViewResult viewResult = response as PartialViewResult;

            Assert.IsNotNull(viewResult.Model);

            Assert.AreEqual("/Views/DynamicContent/_FormControlTemplateEditorListBox.cshtml", viewResult.ViewName);

            Assert.IsInstanceOfType(viewResult.Model, typeof(FormTemplateEditorModel));
            FormTemplateEditorModel model = viewResult.Model as FormTemplateEditorModel;
            Assert.AreEqual("ZzSsBbNnJjKK|MyControl|TrUe", model.Data);
            Assert.AreEqual("ZzSsBbNnJjKK", model.ControlName);
            Assert.AreEqual("MyControl", model.LabelText);
            Assert.AreEqual("Align label to the left", model.AlignLeftText);
            Assert.IsTrue(model.AlignTop);
        }

        #endregion Form Input Template Editor List Box

        #region Private Methods

        private DynamicContentController CreateDynamicContentController(DefaultMemoryCache memoryCache = null,
            List<BreadcrumbItem> breadcrumbs = null,
            MockDynamicContentProvider mockDynamicContentProvider = null,
            MockNotificationService testNotificationService = null)
        {
            IPluginClassesService pluginServices = _testDynamicContentPlugin as IPluginClassesService;
            IPluginHelperService pluginHelperService = _testDynamicContentPlugin as IPluginHelperService;
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            DynamicContentController Result = new DynamicContentController(
                mockDynamicContentProvider ?? new MockDynamicContentProvider(pluginServices),
                memoryCache ?? new DefaultMemoryCache(settingsProvider, true, DateTime.UtcNow.AddDays(10)),
                testNotificationService ?? new MockNotificationService());

            Result.ControllerContext = CreateTestControllerContext(breadcrumbs);

            return Result;
        }

        private List<BreadcrumbItem> GetDynamicBreadcrumbs()
        {
            List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>();
            breadcrumbs.Add(new BreadcrumbItem("Home", "/", false));
            breadcrumbs.Add(new BreadcrumbItem("Custom Pages", "/DynamicContent/GetCustomPages", false));
            return breadcrumbs;
        }

        #endregion Private Methods
    }
}
