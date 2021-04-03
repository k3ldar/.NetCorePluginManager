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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
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

using AspNetCore.PluginManager.DemoWebsite.Classes.Mocks;

using DynamicContent.Plugin.Controllers;
using DynamicContent.Plugin.Model;

using MemoryCache.Plugin;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.DynamicContent;

using PluginManager.Abstractions;
using PluginManager.Internal;

using Shared.Classes;

using SharedPluginFeatures;
using SharedPluginFeatures.DynamicContent;

using pm = PluginManager.Internal;

namespace AspNetCore.PluginManager.Tests.Controllers
{
    [TestClass]
    [ExcludeFromCodeCoverage]

    public class DynamicContentControllerTests : BaseControllerTests
    {
        #region Private Members

        private const string GetContentData = "<li id=\"control-1\" class=\"col-edit-12 ui-state-default editControl\"><p class=\"ctlHeader\">" +
            "Html Content<span class=\"deleteBtn\" id=\"control-1\">X</span><span class=\"editBtn\" id=\"control-1\" data-cs=\"/DynamicContent/TextTemplateEditor/\">" +
            "Edit</span></p><div class=\"ctlContent\"><p>This is <br />html over<br />three lines</p></div></li><li id=\"control-2\" " +
            "class=\"col-edit-4 ui-state-default editControl\"><p class=\"ctlHeader\">Html Content<span class=\"deleteBtn\" id=\"control-2\">X</span>" +
            "<span class=\"editBtn\" id=\"control-2\" data-cs=\"/DynamicContent/TextTemplateEditor/\">Edit</span></p><div " +
            "class=\"ctlContent\"><p>This is html<br />over two lines</p></div></li>";

        private const string PreviewContent = "<div class=\"col-sm-12\"><p>This is <br />html over<br />three lines</p></div><div class=\"col-sm-4\"><p>This is html<br />Content 2</p></div>" +
            "<div class=\"col-sm-4\"><p>This is html<br />Content 9</p></div><div class=\"col-sm-4\"><p>This is html<br />Content 8</p></div><div class=\"col-sm-4\"><p>This is html<br />" +
            "Content 7</p></div><div class=\"col-sm-4\"><p>This is html<br />Content 6</p></div><div class=\"col-sm-4\"><p>This is html<br />Content 5</p></div><div class=\"col-sm-4\"><p>" +
            "This is html<br />Content 4</p></div><div class=\"col-sm-4\"><p>This is html<br />Content 3</p></div><div class=\"col-sm-4\"><p>This is html<br />Content 10</p></div>";

        #endregion Private Members

        [TestInitialize]
        public void InitializeDynamicContentControllerTests()
        {
            InitializeDynamicContentPluginManager();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_DynamicContentController_InvalidDynamicContentProvider_Throws_ArgumentNullException()
        {
            ISettingsProvider settingsProvider = new DefaultSettingProvider(Directory.GetCurrentDirectory());
            DynamicContentController Result = new DynamicContentController(null, new DefaultMemoryCache(settingsProvider));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_DynamicContentController_InvalidMemoryCache_Throws_ArgumentNullException()
        {
            IPluginClassesService pluginServices = new pm.PluginServices(_testDynamicContentPlugin) as IPluginClassesService;
            DynamicContentController Result = new DynamicContentController(new MockDynamicContentProvider(pluginServices), null);
        }

        [TestMethod]
        public void DynamicContentController_ValidateAttributes()
        {
            Assert.IsTrue(ClassHasAttribute<DenySpiderAttribute>(typeof(DynamicContentController)));
        }

        [TestMethod]
        public void Construct_DynamicContentControllerInstance_Success()
        {
            DynamicContentController sut = CreateDynamicContentController();

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void DynamicContentController_GetCustomPages_Validate_Attributes()
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
        public void DynamicContentController_GetCustomPages_Validate_IActionResult()
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
        public void DynamicContentController_GetCustomPages_Validate_ContainsExistingModels()
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
        public void DynamicContentController_EditCustomPage_Validate_Attributes()
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
        public void DynamicContentController_EditCustomPage_InvalidPageId_RedirectsToGetcustomPages()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.EditPage(-1);

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult sut = response as RedirectToActionResult;

            Assert.AreEqual(nameof(DynamicContentController.GetCustomPages), sut.ActionName);
        }

        [TestMethod]
        public void DynamicContentController_EditCustomPage_ReturnsValidCustomPage()
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
        public void DynamicContentController_GetContent_Validate_Attributes()
        {
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetContent)));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetContent)));
            Assert.IsTrue(MethodHasAttribute<LoggedInAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetContent)));
            Assert.IsTrue(MethodRouteAttribute(typeof(DynamicContentController), nameof(DynamicContentController.GetContent), "DynamicContent/GetContent/{*cacheId}"));

            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetContent)));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetContent)));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.GetContent)));
        }

        [TestMethod]
        public void DynamicContentController_GetContent_InvalidCahceId_Null_Returns_FailResult()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();
            IActionResult response = dynamicContentController.GetContent(null);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void DynamicContentController_GetContent_InvalidCahceId_EmptyString_Returns_FailResult()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();
            IActionResult response = dynamicContentController.GetContent("");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void DynamicContentController_GetContent_CacheItemNotFound_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();
            IActionResult response = dynamicContentController.GetContent("cachetest");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void DynamicContentController_GetContent_CacheItemFound_ReturnsSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(2);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            IActionResult response = dynamicContentController.GetContent(editPageModel.CacheId);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(200, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.Success);
            Assert.AreEqual(GetContentData, sut.Data);
        }

        [TestMethod]
        public void DynamicContentController_UpdateControlPosition_Validate_Attributes()
        {
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.UpdateControlPosition)));
            Assert.IsTrue(MethodHasAttribute<LoggedInAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.UpdateControlPosition)));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.UpdateControlPosition)));
            Assert.IsTrue(MethodRouteAttribute(typeof(DynamicContentController), nameof(DynamicContentController.UpdateControlPosition), "DynamicContent/UpdatePosition"));

            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.UpdateControlPosition)));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.UpdateControlPosition)));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.UpdateControlPosition)));
            Assert.IsFalse(MethodHasAttribute<BreadcrumbAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.UpdateControlPosition)));
        }

        [TestMethod]
        public void DynamicContentController_UpdateControlPosition_NullValue_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();
            IActionResult response = dynamicContentController.UpdateControlPosition(null);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void DynamicContentController_UpdateControlPosition_ValidModel_InvalidCacheId_Null_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();
            IActionResult response = dynamicContentController.UpdateControlPosition(new UpdatePositionModel());
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void DynamicContentController_UpdateControlPosition_ValidModel_InvalidCacheId_EmptyString_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();
            IActionResult response = dynamicContentController.UpdateControlPosition(new UpdatePositionModel() { CacheId = "" });
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void DynamicContentController_UpdateControlPosition_ValidModel_InvalidCacheId_NotFound_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();
            IActionResult response = dynamicContentController.UpdateControlPosition(new UpdatePositionModel() { CacheId = "blah" });
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void DynamicContentController_UpdateControlPosition_ValidModel_InvalidControlId_Null_ReturnsNonSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(2);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            IActionResult response = dynamicContentController.UpdateControlPosition(new UpdatePositionModel() { CacheId = editPageModel.CacheId });
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void DynamicContentController_UpdateControlPosition_ValidModel_InvalidControlId_EmptyString_ReturnsNonSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(2);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            IActionResult response = dynamicContentController.UpdateControlPosition(new UpdatePositionModel() { CacheId = editPageModel.CacheId, ControlId = "" });
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void DynamicContentController_UpdateControlPosition_ValidModel_InvalidControlId_NotFound_ReturnsNonSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(2);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            IActionResult response = dynamicContentController.UpdateControlPosition(new UpdatePositionModel() { CacheId = editPageModel.CacheId, ControlId = "asdfasdfasdf" });
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void DynamicContentController_UpdateControlPosition_ValidModel_ValidControlId_PositionsUpdated_ReturnsSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

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
        public void DynamicContentController_GetWidthTypes_ReturnsSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            List<Middleware.LookupListItem> widthTypes = DynamicContentController.GetWidthTypes();
            Assert.AreEqual(3, widthTypes.Count);
        }

        [TestMethod]
        public void DynamicContentController_GetHeightTypes_ReturnsSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            List<Middleware.LookupListItem> heightTypes = DynamicContentController.GetHeightTypes();
            Assert.AreEqual(3, heightTypes.Count);
        }

        #region Preview

        [TestMethod]
        public void Preview_Validate_Attributes()
        {
            Assert.IsTrue(MethodHasAttribute<LoggedInAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.Preview)));
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.Preview)));
            Assert.IsTrue(MethodRouteAttribute(typeof(DynamicContentController), nameof(DynamicContentController.Preview), "DynamicContent/Preview/{cacheId}"));

            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.Preview)));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.Preview)));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.Preview)));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.Preview)));
        }

        [TestMethod]
        public void Preview_InvalidCacheId_EmptyString_Returns_StatusCode400()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.Preview("");
            StatusCodeResult sut = response as StatusCodeResult;

            Assert.IsNotNull(sut);

            Assert.AreEqual(400, sut.StatusCode);
        }

        [TestMethod]
        public void Preview_InvalidCacheId_Null_Returns_StatusCode400()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.Preview(null);
            StatusCodeResult sut = response as StatusCodeResult;

            Assert.IsNotNull(sut);

            Assert.AreEqual(400, sut.StatusCode);
        }

        [TestMethod]
        public void Preview_InvalidCache_ItemNotFound_Returns_StatusCode400()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.Preview("test123");
            StatusCodeResult sut = response as StatusCodeResult;

            Assert.IsNotNull(sut);

            Assert.AreEqual(400, sut.StatusCode);
        }

        [TestMethod]
        public void Preview_ValidCache_PreviewPageCreated_Returns_StatusCode200()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

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
        public void TemplateEditor_Validate_Attributes()
        {
            Assert.IsTrue(MethodHasAttribute<LoggedInAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TemplateEditor)));
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TemplateEditor)));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TemplateEditor)));
            Assert.IsTrue(MethodRouteAttribute(typeof(DynamicContentController), nameof(DynamicContentController.TemplateEditor), "DynamicContent/TemplateEditor/{cacheId}/{controlId}"));

            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TemplateEditor)));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TemplateEditor)));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TemplateEditor)));
        }

        [TestMethod]
        public void TemplateEditor_InvalidCachedId_Null_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.TemplateEditor(null, "control-1");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void TemplateEditor_InvalidCachedId_EmptyString_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.TemplateEditor("", "control-1");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void TemplateEditor_InvalidControlId_Null_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.TemplateEditor("cachename", null);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void TemplateEditor_InvalidControlId_EmptyString_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.TemplateEditor("cachename", "");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void TemplateEditor_InvalidCache_NotFound_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.TemplateEditor("cachename", "controlname");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void TemplateEditor_ValidCache_InvalidControlId_ReturnsSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            IActionResult response = dynamicContentController.TemplateEditor(editPageModel.CacheId, "my-control");

            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void TemplateEditor_ValidCache_ValidControlId_ReturnsValidPartialView()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

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

            Assert.AreEqual(DateTime.MinValue, model.ActiveFrom);
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
        public void UpdateTemplate_InvalidModel_Returns_NonSucess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.UpdateTemplate(null);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void UpdateTemplate_InvalidCacheId_Null_Returns_NonSucess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            EditTemplateModel model = new EditTemplateModel();

            IActionResult response = dynamicContentController.UpdateTemplate(model);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void UpdateTemplate_InvalidControlId_ControlNotFound_Returns_NonSucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void UpdateTemplate_InvalidWidth_TooManyColumns_Returns_NonSucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
            Assert.AreEqual("Width must be between 1 and 12", sut.Data);
        }

        [TestMethod]
        public void UpdateTemplate_InvalidWidth_NotEnoughColumns_Returns_NonSucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
            Assert.AreEqual("Width must be between 1 and 12", sut.Data);
        }

        [TestMethod]
        public void UpdateTemplate_InvalidWidth_Below1Percent_Returns_NonSucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
            Assert.AreEqual("Width must be between 1 and 100", sut.Data);
        }

        [TestMethod]
        public void UpdateTemplate_InvalidWidth_Above100Percent_Returns_NonSucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
            Assert.AreEqual("Width must be between 1 and 100", sut.Data);
        }

        [TestMethod]
        public void UpdateTemplate_InvalidWidth_Below1Pixel_Returns_NonSucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
            Assert.AreEqual("Width must be at least 1", sut.Data);
        }

        [TestMethod]
        public void UpdateTemplate_InvalidHeight_PercentageTooHigh_Returns_NonSucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
            Assert.AreEqual("Height must be between 1 and 100", sut.Data);
        }

        [TestMethod]
        public void UpdateTemplate_InvalidHeight_PercentageTooLow_Returns_NonSucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
            Assert.AreEqual("Height must be between 1 and 100", sut.Data);
        }

        [TestMethod]
        public void UpdateTemplate_InvalidHeight_NotEnoughPixels_Returns_NonSucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
            Assert.AreEqual("Height must be at least 1", sut.Data);
        }

        [TestMethod]
        public void UpdateTemplate_Valid_Returns_Sucess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.Success);


            CacheItem cacheItem = memoryCache.GetExtendingCache().Get(model.CacheId);

            Assert.IsNotNull(cacheItem);

            DynamicContentPage dynamicContentPage = cacheItem.Value as DynamicContentPage;

            Assert.IsNotNull(dynamicContentPage);

            DynamicContentTemplate control = dynamicContentPage.Content.Where(ctl => ctl.UniqueId.Equals(model.UniqueId)).FirstOrDefault();

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
        public void TextTemplateEditor_Validate_Attributes()
        {
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TextTemplateEditor)));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TextTemplateEditor)));

            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TextTemplateEditor)));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TextTemplateEditor)));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.TextTemplateEditor)));
        }

        [TestMethod]
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
        public void DeleteControl_Validate_Attributes()
        {
            Assert.IsTrue(MethodHasAttribute<LoggedInAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.DeleteControl)));
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.DeleteControl)));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.DeleteControl)));
            Assert.IsTrue(MethodRouteAttribute(typeof(DynamicContentController), nameof(DynamicContentController.DeleteControl), "DynamicContent/DeleteControl/{cacheId}/{controlId}"));

            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.DeleteControl)));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.DeleteControl)));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.DeleteControl)));
        }

        [TestMethod]
        public void DeleteControl_InvalidCachedId_Null_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.DeleteControl(null, "control-1");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void DeleteControl_InvalidCachedId_EmptyString_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.DeleteControl("", "control-1");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void DeleteControl_InvalidControlId_Null_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.DeleteControl("cachename", null);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void DeleteControl_InvalidControlId_EmptyString_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.DeleteControl("cachename", "");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void DeleteControl_InvalidCache_NotFound_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.DeleteControl("cachename", "controlname");
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void DeleteControl_ValidCache_InvalidControlId_ReturnsSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

            DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            IActionResult editPageResponse = dynamicContentController.EditPage(10);
            ViewResult viewResult = editPageResponse as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            EditPageModel editPageModel = viewResult.Model as EditPageModel;


            IActionResult response = dynamicContentController.DeleteControl(editPageModel.CacheId, "my-control");

            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void DeleteControl_ValidCache_ValidControlId_ReturnsValidPartialView()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

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
        public void DeleteItem_InvalidModel_Null_ReturnsNonSuccess()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.DeleteItem(null);
            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void DeleteItem_ValidCache_InvalidControlId_ReturnsSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        public void DeleteItem_ValidCache_ValidControlId_ReturnsValidPartialView()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

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

            DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.Success);

            Assert.AreEqual(9, dynamicContentPage.Content.Count);
            Assert.IsFalse(dynamicContentPage.Content.Where(dcp => dcp.UniqueId.Equals("control-1")).Any());
        }

        #endregion Delete Item

        #region Retrieve Templates

        [TestMethod]
        public void DynamicContentController_GetTemplates_Validate_Attributes()
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
        public void DynamicContentController_GetTemplates_ReturnsValidTemplatesList()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.GetTemplates();

            PartialViewResult viewResult = response as PartialViewResult;

            Assert.IsNotNull(viewResult.Model);

            Assert.IsInstanceOfType(viewResult.Model, typeof(TemplatesModel));

            TemplatesModel sut = viewResult.Model as TemplatesModel;

            Assert.IsNotNull(sut);

            Assert.AreEqual(3, sut.Templates.Count);
        }

        [TestMethod]
        public void DynamicContentController_GetTemplates_ReturnsValidViewName()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.GetTemplates();

            PartialViewResult viewResult = response as PartialViewResult;

            Assert.AreEqual("/Views/DynamicContent/_Templates.cshtml", viewResult.ViewName);
        }

        #endregion Retrieve Templates

        #region Add Template To Page

        [TestMethod]
        public void DynamicContentController_AddTemplateToPage_Validate_Attributes()
        {
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.AddTemplateToPage)));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.AddTemplateToPage)));
            Assert.IsTrue(MethodHasAttribute<LoggedInAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.AddTemplateToPage)));
            Assert.IsTrue(MethodRouteAttribute(typeof(DynamicContentController), nameof(DynamicContentController.AddTemplateToPage), "DynamicContent/AddTemplate/{cacheId}/{templateId}"));

            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.AddTemplateToPage)));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.AddTemplateToPage)));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(DynamicContentController), nameof(DynamicContentController.AddTemplateToPage)));
        }

        [TestMethod]
        public void DynamicContentController_AddTemplateToPage_NullCacheId_Returns400()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.AddTemplateToPage(null, "123");

            JsonResult sut = response as JsonResult;

            Assert.AreEqual(400, sut.StatusCode.Value);
        }

        [TestMethod]
        public void DynamicContentController_AddTemplateToPage_EmptyCacheId_Returns400()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.AddTemplateToPage("", "123");

            JsonResult sut = response as JsonResult;

            Assert.AreEqual(400, sut.StatusCode.Value);
        }

        [TestMethod]
        public void DynamicContentController_AddTemplateToPage_NullTemplateIdId_Returns400()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.AddTemplateToPage("123", null);

            JsonResult sut = response as JsonResult;

            Assert.AreEqual(400, sut.StatusCode.Value);
        }

        [TestMethod]
        public void DynamicContentController_AddTemplateToPage_EmptyTemplateId_Returns400()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.AddTemplateToPage("123", "");

            JsonResult sut = response as JsonResult;

            Assert.AreEqual(400, sut.StatusCode.Value);
        }

        [TestMethod]
        public void DynamicContentController_AddTemplateToPage_CacheItemDoesNotExist()
        {
            DynamicContentController dynamicContentController = CreateDynamicContentController();

            IActionResult response = dynamicContentController.AddTemplateToPage("123", "123");

            JsonResult sut = response as JsonResult;

            Assert.AreEqual(400, sut.StatusCode.Value);
            //ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            //DefaultMemoryCache memoryCache = new DefaultMemoryCache(settingsProvider);

            //DynamicContentController dynamicContentController = CreateDynamicContentController(memoryCache, GetDynamicBreadcrumbs());

            //IActionResult editPageResponse = dynamicContentController.EditPage(10);
            //ViewResult viewResult = editPageResponse as ViewResult;
            //Assert.IsNotNull(viewResult.Model);
            //EditPageModel editPageModel = viewResult.Model as EditPageModel;

            //CacheItem cacheItem = memoryCache.GetExtendingCache().Get(editPageModel.CacheId);
            //DynamicContentPage dynamicContentPage = cacheItem.Value as DynamicContentPage;

            //Assert.IsNotNull(dynamicContentPage);
            //Assert.AreEqual(10, dynamicContentPage.Content.Count);

            //DeleteControlModel deleteControlModel = new DeleteControlModel()
            //{
            //    CacheId = editPageModel.CacheId,
            //    ControlId = "control-1"
            //};

            //IActionResult response = dynamicContentController.DeleteItem(deleteControlModel);

            //JsonResult jsonResult = response as JsonResult;

            //Assert.AreEqual("application/json", jsonResult.ContentType);
            //Assert.AreEqual(200, jsonResult.StatusCode);

            //DynamicContentModel sut = jsonResult.Value as DynamicContentModel;

            //Assert.IsNotNull(sut);
            //Assert.IsTrue(sut.Success);

            //Assert.AreEqual(9, dynamicContentPage.Content.Count);
            //Assert.IsFalse(dynamicContentPage.Content.Where(dcp => dcp.UniqueId.Equals("control-1")).Any());
        }

        #endregion Add Template To Page

        #region Private Methods

        private DynamicContentController CreateDynamicContentController(DefaultMemoryCache memoryCache = null,
            List<BreadcrumbItem> breadcrumbs = null)
        {
            IPluginClassesService pluginServices = new pm.PluginServices(_testDynamicContentPlugin) as IPluginClassesService;
            IPluginHelperService pluginHelperService = new pm.PluginServices(_testDynamicContentPlugin) as IPluginHelperService;
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            DynamicContentController Result = new DynamicContentController(new MockDynamicContentProvider(pluginServices),
                memoryCache ?? new DefaultMemoryCache(settingsProvider));

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
