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
 *  File: SystemAdminControllerTests.cs
 *
 *  Purpose:  Tests for System Admin Controller
 *
 *  Date        Name                Reason
 *  07/10/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;

using AspNetCore.PluginManager.Tests.Controllers;
using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Users;

using SharedPluginFeatures;

using SystemAdmin.Plugin.Classes.MenuItems;
using SystemAdmin.Plugin.Controllers;
using SystemAdmin.Plugin.Models;

namespace AspNetCore.PluginManager.Tests.Plugins.SystemAdminTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SystemAdminControllerTests : BaseControllerTests
    {
        private const string TestCategoryName = "System Admin";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_SettingProvider_Null_Throws_ArgumentNullException()
        {
            new SystemAdminController(null, new MockSystemAdminHelperService(), new MockSeoProvider(),
                new MockUserSearch(), new MockClaimsProvider(), new MockPluginClassesService(),
				new MockPluginManagerConfiguration());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_SystemAdminHelperService_Null_Throws_ArgumentNullException()
        {
            new SystemAdminController(new MockSettingsProvider(), null, new MockSeoProvider(),
                new MockUserSearch(), new MockClaimsProvider(), new MockPluginClassesService(),
				new MockPluginManagerConfiguration());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_SeoProvider_Null_Throws_ArgumentNullException()
        {
            new SystemAdminController(new MockSettingsProvider(), new MockSystemAdminHelperService(), null,
                new MockUserSearch(), new MockClaimsProvider(), new MockPluginClassesService(),
				new MockPluginManagerConfiguration());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_userSearch_Null_Throws_ArgumentNullException()
        {
            new SystemAdminController(new MockSettingsProvider(), new MockSystemAdminHelperService(), new MockSeoProvider(),
                null, new MockClaimsProvider(), new MockPluginClassesService(),
				new MockPluginManagerConfiguration());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_PluginClassesService_Null_Throws_ArgumentNullException()
        {
			new SystemAdminController(new MockSettingsProvider(), new MockSystemAdminHelperService(), new MockSeoProvider(),
				new MockUserSearch(), null, null,
				new MockPluginManagerConfiguration());
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidParam_PluginManagerConfiguration_Null_Throws_ArgumentNullException()
		{
			new SystemAdminController(new MockSettingsProvider(), new MockSystemAdminHelperService(), new MockSeoProvider(),
				new MockUserSearch(), null, new MockPluginClassesService(),
				null);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidParam_ClaimsProvider_Null_Throws_ArgumentNullException()
		{
			new SystemAdminController(new MockSettingsProvider(), new MockSystemAdminHelperService(), new MockSeoProvider(),
				new MockUserSearch(), null, new MockPluginClassesService(),
				new MockPluginManagerConfiguration());
		}

		[TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            SystemAdminController sut = new SystemAdminController(new MockSettingsProvider(), new MockSystemAdminHelperService(), new MockSeoProvider(),
                new MockUserSearch(), new MockClaimsProvider(), new MockPluginClassesService(),
				new MockPluginManagerConfiguration());

            Assert.IsNotNull(sut);
        }


        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_HasCorrectAttributes_Success()
        {
            string MethodName = "Index";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(SystemAdminController), MethodName));

            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(SystemAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_MenuNotFound_ReturnsDefaultMenu_Success()
        {
            List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
            adminMenuItems.Add(new MockSystemAdminMainMenu("test", 123));
            MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
            SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

            ViewResult viewResult = sut.Index(-1) as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNull(viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

            AvailableIconViewModel viewModel = viewResult.Model as AvailableIconViewModel;
            Assert.IsNotNull(viewModel);
            Assert.IsNotNull(viewModel.HomeIcons);
            Assert.AreEqual(123, viewModel.HomeIcons[0].UniqueId);
            Assert.AreEqual("test", viewModel.HomeIcons[0].Name);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_MenuFound_ReturnsSelectedView_Success()
        {
            List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
            adminMenuItems.Add(new MockSystemAdminMainMenu("test", 123));
            adminMenuItems[0].ChildMenuItems.Add(new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.Grid));
            MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
            SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

            ViewResult viewResult = sut.Index(123) as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNull(viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

            AvailableIconViewModel viewModel = viewResult.Model as AvailableIconViewModel;
            Assert.IsNotNull(viewModel);
            Assert.IsNull(viewModel.HomeIcons);
            Assert.AreEqual("test", viewModel.Title);
            Assert.IsNotNull(viewModel.MenuItems);
            Assert.AreEqual(1, viewModel.MenuItems.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Grid_HasCorrectAttributes_Success()
        {
            string MethodName = "Grid";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(SystemAdminController), MethodName));

            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(SystemAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Grid_MenuNotFound_ReturnsRedirectToIndex_Success()
        {
            List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
            adminMenuItems.Add(new MockSystemAdminMainMenu("test", 123));
            MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
            SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

            RedirectResult redirectResult = sut.Grid(-1) as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.IsFalse(redirectResult.Permanent);
            Assert.AreEqual("/SystemAdmin/", redirectResult.Url);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Grid_MenuFound_ReturnsSelectedView_Success()
        {
            List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
            adminMenuItems.Add(new MockSystemAdminMainMenu("test", 123));
            adminMenuItems[0].ChildMenuItems.Add(new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.Grid));
            adminMenuItems[0].ChildMenuItems[0].UniqueId = 321;
            MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
            SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

            ViewResult viewResult = sut.Grid(321) as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNull(viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

            GridViewModel viewModel = viewResult.Model as GridViewModel;
            Assert.IsNotNull(viewModel);
            Assert.AreEqual("Mock Sub Menu", viewModel.Title);
            Assert.AreEqual(2, viewModel.Headers.Length);
            Assert.AreEqual(1, viewModel.Items.Count);
            Assert.AreEqual(2, viewModel.Items[0].Length);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Map_HasCorrectAttributes_Success()
        {
            string MethodName = "Map";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(SystemAdminController), MethodName));

            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(SystemAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Map_MenuNotFound_ReturnsRedirectToIndex_Success()
        {
            List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
            adminMenuItems.Add(new MockSystemAdminMainMenu("test", 123));
            MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
            SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

            RedirectResult redirectResult = sut.Map(-1) as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.IsFalse(redirectResult.Permanent);
            Assert.AreEqual("/SystemAdmin/", redirectResult.Url);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Map_MenuFound_ReturnsSelectedView_Success()
        {
            List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
            adminMenuItems.Add(new MockSystemAdminMainMenu("test", 123));
            adminMenuItems[0].ChildMenuItems.Add(new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.Map));
            adminMenuItems[0].ChildMenuItems[0].UniqueId = 321;
            MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
            SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

            ViewResult viewResult = sut.Map(321) as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNull(viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

            MapViewModel viewModel = viewResult.Model as MapViewModel;
            Assert.IsNotNull(viewModel);
            Assert.AreEqual("Mock Sub Menu", viewModel.Title);
            Assert.AreEqual("Map Data", viewModel.MapLocationData);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void View_HasCorrectAttributes_Success()
        {
            string MethodName = "View";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(SystemAdminController), MethodName, new Type[] { typeof(int) }));

            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(SystemAdminController), MethodName, new Type[] { typeof(int) }));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(SystemAdminController), MethodName, new Type[] { typeof(int) }));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(SystemAdminController), MethodName, new Type[] { typeof(int) }));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(SystemAdminController), MethodName, new Type[] { typeof(int) }));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(SystemAdminController), MethodName, new Type[] { typeof(int) }));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(SystemAdminController), MethodName, new Type[] { typeof(int) }));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void View_MenuNotFound_ReturnsRedirectToIndex_Success()
        {
            List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
            adminMenuItems.Add(new MockSystemAdminMainMenu("test", 123));
            MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
            SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

            RedirectResult redirectResult = sut.View(-1) as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.IsFalse(redirectResult.Permanent);
            Assert.AreEqual("/SystemAdmin/", redirectResult.Url);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void View_MenuFound_ReturnsSelectedView_Success()
        {
            List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
            adminMenuItems.Add(new MockSystemAdminMainMenu("test", 123));
            adminMenuItems[0].ChildMenuItems.Add(new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.PartialView));
            adminMenuItems[0].ChildMenuItems[0].UniqueId = 321;
            MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
            SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

            ViewResult viewResult = sut.View(321) as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewName);
            Assert.AreEqual("PartialView", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

            PartialViewModel viewModel = viewResult.Model as PartialViewModel;
            Assert.IsNotNull(viewModel);
            Assert.AreEqual("Mock Sub Menu", viewModel.Title);
            Assert.AreEqual("/TestC/", viewModel.ControllerRoot);
            Assert.AreEqual("/TestC/TestA", viewModel.PartialView);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Text_HasCorrectAttributes_Success()
        {
            string MethodName = "Text";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(SystemAdminController), MethodName));

            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(SystemAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Text_MenuNotFound_ReturnsRedirectToIndex_Success()
        {
            List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
            adminMenuItems.Add(new MockSystemAdminMainMenu("test", 123));
            MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
            SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

            RedirectResult redirectResult = sut.Text(-1) as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.IsFalse(redirectResult.Permanent);
            Assert.AreEqual("/SystemAdmin/", redirectResult.Url);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Text_MenuFound_ReturnsSelectedView_Success()
        {
            List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
            adminMenuItems.Add(new MockSystemAdminMainMenu("test", 123));
            adminMenuItems[0].ChildMenuItems.Add(new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.Text));
            adminMenuItems[0].ChildMenuItems[0].UniqueId = 321;
            MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
            SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

            ViewResult viewResult = sut.Text(321) as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNull(viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

            TextViewModel viewModel = viewResult.Model as TextViewModel;
            Assert.IsNotNull(viewModel);
            Assert.AreEqual("Mock Sub Menu", viewModel.Title);
            Assert.AreEqual("Some&nbsp;data&nbsp;&nbsp;&nbsp;&nbsp;to<br />with&nbsp;&lt;html&gt;markup", viewModel.Text);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void TextEx_HasCorrectAttributes_Success()
        {
            string MethodName = "TextEx";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(SystemAdminController), MethodName));

            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(SystemAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void TextEx_MenuNotFound_ReturnsRedirectToIndex_Success()
        {
            List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
            adminMenuItems.Add(new MockSystemAdminMainMenu("test", 123));
            MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
            SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

            RedirectResult redirectResult = sut.TextEx(-1) as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.IsFalse(redirectResult.Permanent);
            Assert.AreEqual("/SystemAdmin/", redirectResult.Url);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void TextEx_MenuFound__SettingsDisabled_ReturnsSelectedView_Success()
        {
            List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
            adminMenuItems.Add(new MockSystemAdminMainMenu("test", 123));
            adminMenuItems[0].ChildMenuItems.Add(new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.FormattedText));
            adminMenuItems[0].ChildMenuItems[0].UniqueId = 321;
            MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
            SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

            ViewResult viewResult = sut.TextEx(321) as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNull(viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

            TextExViewModel viewModel = viewResult.Model as TextExViewModel;
            Assert.IsNotNull(viewModel);
            Assert.AreEqual("Mock Sub Menu", viewModel.Title);
            Assert.AreEqual("Formatted Text is not enabed", viewModel.Text);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void TextEx_MenuFound__SettingsEnabled_ReturnsSelectedView_Success()
        {
            List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
            adminMenuItems.Add(new MockSystemAdminMainMenu("test", 123));
            adminMenuItems[0].ChildMenuItems.Add(new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.FormattedText));
            adminMenuItems[0].ChildMenuItems[0].UniqueId = 321;
            MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
            SystemAdminController sut = CreateSystemAdminController(
                new MockSettingsProvider("{\"SystemAdmin\":{\"EnableFormattedText\":true}}"),
                mockSystemAdminHelperService);

            ViewResult viewResult = sut.TextEx(321) as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNull(viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

            TextExViewModel viewModel = viewResult.Model as TextExViewModel;
            Assert.IsNotNull(viewModel);
            Assert.AreEqual("Mock Sub Menu", viewModel.Title);
            Assert.AreEqual("Formatted Text", viewModel.Text);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(TestCategoryName)]
        public void Chart_HasCorrectAttributes_Success()
        {
            string MethodName = "Chart";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(SystemAdminController), MethodName));

            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(SystemAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Chart_MenuNotFound_ReturnsRedirectToIndex_Success()
        {
            List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
            adminMenuItems.Add(new MockSystemAdminMainMenu("test", 123));
            MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
            SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

            RedirectResult redirectResult = sut.Chart(-1) as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.IsFalse(redirectResult.Permanent);
            Assert.AreEqual("/SystemAdmin/", redirectResult.Url);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Chart_MenuFound_ReturnsSelectedView_Success()
        {
            List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
            adminMenuItems.Add(new MockSystemAdminMainMenu("test", 123));
            adminMenuItems[0].ChildMenuItems.Add(new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.Chart));
            adminMenuItems[0].ChildMenuItems[0].UniqueId = 321;
            MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
            SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

            ViewResult viewResult = sut.Chart(321) as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNull(viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

            ChartViewModel viewModel = viewResult.Model as ChartViewModel;
            Assert.IsNotNull(viewModel);
            Assert.AreEqual("Mock Sub Menu", viewModel.Title);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UserSearch_HasCorrectAttributes_Success()
        {
            string MethodName = "UserSearch";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(SystemAdminController), MethodName));

            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(SystemAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserSearch_InvalidModel_Null_Throws_ArgumentNullException()
        {
            List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
            adminMenuItems.Add(new MockSystemAdminMainMenu("test", 123));
            MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
            SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

            sut.UserSearch(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UserSearch_ReturnsValidJsonResult_Success()
        {
            List<SearchUser> users = new List<SearchUser>();
            users.Add(new SearchUser(1, "John Doe", "john@doe.com"));
            users.Add(new SearchUser(2, "Jane Doe", "jane@doe.com"));
            MockUserSearch mockUserSearch = new MockUserSearch(users);

            SystemAdminController sut = CreateSystemAdminController(null, null, null, mockUserSearch);
            BootgridRequestData requestData = new BootgridRequestData();

            JsonResult jsonResult = sut.UserSearch(requestData);
            Assert.IsNotNull(jsonResult);
            Assert.IsNull(jsonResult.ContentType);
            Assert.IsNull(jsonResult.SerializerSettings);
            Assert.IsNotNull(jsonResult.Value);

            BootgridResponseData<SearchUser> responseData = jsonResult.Value as BootgridResponseData<SearchUser>;
            Assert.IsNotNull(responseData);
            Assert.AreEqual(2, responseData.total);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Permissions_HasCorrectAttributes_Success()
        {
            string MethodName = "Permissions";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(SystemAdminController), MethodName));

            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(SystemAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Permissions_MenuFound_ReturnsSelectedView_Success()
        {
            List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
            adminMenuItems.Add(new MockSystemAdminMainMenu("test", 123));
            adminMenuItems[0].ChildMenuItems.Add(new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.Chart));
            adminMenuItems[0].ChildMenuItems[0].UniqueId = 321;
            MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
            SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

            ViewResult viewResult = sut.Permissions() as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNull(viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

            PermissionsModel viewModel = viewResult.Model as PermissionsModel;
            Assert.IsNotNull(viewModel);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetUserPermissions_HasCorrectAttributes_Success()
        {
            string MethodName = "GetUserPermissions";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsTrue(MethodHasAuthorizeAttribute(typeof(SystemAdminController), MethodName, "ManagePermissions"));

            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(SystemAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetUserPermissions_UserNotFound_NoClaimsReturned_Success()
        {
            SystemAdminController sut = CreateSystemAdminController();

            PartialViewResult partialViewResult = sut.GetUserPermissions(-1) as PartialViewResult;
            Assert.IsNotNull(partialViewResult);
            Assert.AreEqual("_UserPermissions", partialViewResult.ViewName);
            Assert.IsNotNull(partialViewResult.Model);

            UserPermissionsViewModel viewModel = partialViewResult.Model as UserPermissionsViewModel;
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(0, viewModel.SelectedClaims.Length);
            Assert.AreEqual(0, viewModel.SystemClaims.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetUserPermissions_UserFoundWithValidClaims_Success()
        {
            Dictionary<long, List<string>> mockClaims = new Dictionary<long, List<string>>();
            mockClaims.Add(21, new List<string>() { "Claim 1", "Claim 2", "Claim 3" });
            mockClaims.Add(28716, new List<string>() { "Claim 3", "Claim 4", "Claim 5" });
            MockClaimsProvider mockClaimsProvider = new MockClaimsProvider(mockClaims);
            SystemAdminController sut = CreateSystemAdminController(null, null, null, null, mockClaimsProvider);

            PartialViewResult partialViewResult = sut.GetUserPermissions(21) as PartialViewResult;
            Assert.IsNotNull(partialViewResult);
            Assert.AreEqual("_UserPermissions", partialViewResult.ViewName);
            Assert.IsNotNull(partialViewResult.Model);

            UserPermissionsViewModel viewModel = partialViewResult.Model as UserPermissionsViewModel;
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(21L, viewModel.UserId);
            Assert.AreEqual("Claim 1;Claim 2;Claim 3", viewModel.SelectedClaims);
            Assert.AreEqual(5, viewModel.SystemClaims.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SetUserPermissions_HasCorrectAttributes_Success()
        {
            string MethodName = "SetUserPermissions";
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsTrue(MethodHasAuthorizeAttribute(typeof(SystemAdminController), MethodName, "ManagePermissions"));

            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(SystemAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetUserPermissions_InvalidModel_Null_Throws_ArgumentNullException()
        {
            SystemAdminController sut = CreateSystemAdminController();
            sut.SetUserPermissions(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SetUserPermissions_RemovesAllClaimsForUser_Success()
        {
            Dictionary<long, List<string>> mockClaims = new Dictionary<long, List<string>>();
            mockClaims.Add(21, new List<string>() { "Claim 1", "Claim 2", "Claim 3" });
            mockClaims.Add(28716, new List<string>() { "Claim 3", "Claim 4", "Claim 5" });
            MockClaimsProvider mockClaimsProvider = new MockClaimsProvider(mockClaims);
            SystemAdminController sut = CreateSystemAdminController(null, null, null, null, mockClaimsProvider);
            UserPermissionsViewModel viewModel = new UserPermissionsViewModel();
            viewModel.UserId = 21;
            viewModel.SelectedClaims = null;
            JsonResult jsonResult = sut.SetUserPermissions(viewModel) as JsonResult;
            Assert.IsNotNull(jsonResult);
            dynamic results = jsonResult.Value;
            Assert.AreEqual(true, results.updated);
            Assert.AreEqual(0, mockClaims[21].Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SetUserPermissions_UpdateClaimsForUser_Success()
        {
            Dictionary<long, List<string>> mockClaims = new Dictionary<long, List<string>>();
            mockClaims.Add(21, new List<string>() { "Claim 1", "Claim 2", "Claim 3" });
            mockClaims.Add(28716, new List<string>() { "Claim 3", "Claim 4", "Claim 5" });
            MockClaimsProvider mockClaimsProvider = new MockClaimsProvider(mockClaims);
            SystemAdminController sut = CreateSystemAdminController(null, null, null, null, mockClaimsProvider);
            UserPermissionsViewModel viewModel = new UserPermissionsViewModel();
            viewModel.UserId = 21;
            viewModel.SelectedClaims = "Claim 1;Claim 2;Claim 6;Claim7;";
            JsonResult jsonResult = sut.SetUserPermissions(viewModel) as JsonResult;
            Assert.IsNotNull(jsonResult);
            dynamic results = jsonResult.Value;
            Assert.AreEqual(true, results.updated);
            Assert.AreEqual(4, mockClaims[21].Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SeoData_HasCorrectAttributes_Success()
        {
            string MethodName = "SeoData";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsTrue(MethodHasAuthorizeAttribute(typeof(SystemAdminController), MethodName, "AlterSeo"));
            Assert.IsTrue(MethodHasRouteAttribute(typeof(SystemAdminController), MethodName, "SystemAdmin/SeoData/{routeName}/"));

            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(SystemAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SeoData_NullRouteName_ReturnsBadRequest()
        {
            SystemAdminController sut = CreateSystemAdminController();
            StatusCodeResult statusCodeResult = sut.SeoData(null) as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(400, statusCodeResult.StatusCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SeoData_NoSeoDataFoundForRoute_ReturnsDefaultData()
        {
            SystemAdminController sut = CreateSystemAdminController();
            PartialViewResult partialViewResult = sut.SeoData("/RouteNoSeo") as PartialViewResult;
            Assert.IsNotNull(partialViewResult);
            Assert.AreEqual("_SeoUpdate", partialViewResult.ViewName);

            SeoDataModel seoDataModel = partialViewResult.Model as SeoDataModel;
            Assert.IsNotNull(seoDataModel);
			Assert.AreEqual("", seoDataModel.SeoAuthor);
            Assert.IsNull(seoDataModel.SeoMetaDescription);
            Assert.AreEqual("", seoDataModel.SeoTitle);
            Assert.AreEqual("", seoDataModel.SeoTags);
            Assert.AreEqual("/RouteNoSeo", seoDataModel.SeoUrl);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SeoData_SeoDataFoundForRoute_ReturnsValidSeoData()
        {
            MockSeoData mockSeoData = new MockSeoData()
            {
                Author = "The Author",
                Keywords = new List<string>()
                {
                    "Keyword 1",
                    "Keyword 2"
                },
                MetaDescription = "Meta Desc",
                Title = "The Title"
            };

            Dictionary<string, MockSeoData> mockSeo = new Dictionary<string, MockSeoData>();
            mockSeo.Add("/RouteWithSeo", mockSeoData);
            MockSeoProvider mockSeoProvider = new MockSeoProvider(mockSeo);
            SystemAdminController sut = CreateSystemAdminController(null, null, mockSeoProvider);
            PartialViewResult partialViewResult = sut.SeoData("/RouteWithSeo") as PartialViewResult;
            Assert.IsNotNull(partialViewResult);
            Assert.AreEqual("_SeoUpdate", partialViewResult.ViewName);

            SeoDataModel seoDataModel = partialViewResult.Model as SeoDataModel;
            Assert.IsNotNull(seoDataModel);
            Assert.AreEqual("The Author", seoDataModel.SeoAuthor);
            Assert.AreEqual("Meta Desc", seoDataModel.SeoMetaDescription);
            Assert.AreEqual("The Title", seoDataModel.SeoTitle);
            Assert.AreEqual(19, seoDataModel.SeoTags.Length);
            Assert.AreEqual("/RouteWithSeo", seoDataModel.SeoUrl);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SeoUpdateData_HasCorrectAttributes_Success()
        {
            string MethodName = "SeoUpdateData";
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsTrue(MethodHasAuthorizeAttribute(typeof(SystemAdminController), MethodName, "AlterSeo"));

            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(SystemAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(SystemAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SeoUpdateData_InvalidModel_Null_Throws_ArgumentNullException()
        {
            SystemAdminController sut = CreateSystemAdminController();
            sut.SeoUpdateData(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SeoUpdateData_UpdateRouteWithNoCurrentData_Success()
        {
            MockSeoProvider mockSeoProvider = new MockSeoProvider();
            SystemAdminController sut = CreateSystemAdminController(null, null, mockSeoProvider);
            SeoDataModel seoDataModel = new SeoDataModel("/RouteWithNoSeo")
            {
                SeoAuthor = "New Author",
                SeoMetaDescription = "New Meta",
                SeoTags = "Tag1 Tag2  ",
                SeoTitle = "New Title"
            };

            RedirectResult redirectResult = sut.SeoUpdateData(seoDataModel) as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.IsFalse(redirectResult.Permanent);
            Assert.AreEqual("/RouteWithNoSeo", redirectResult.Url);

            if (!mockSeoProvider.GetSeoDataForRoute("/RouteWithNoSeo", out string title, out string meta, out string author, out List<string> keywords))
                throw new InvalidOperationException("Seo data not found");

            Assert.AreEqual("New Title", title);
            Assert.AreEqual("New Meta", meta);
            Assert.AreEqual("New Author", author);
            Assert.AreEqual(2, keywords.Count);
            Assert.AreEqual("Tag1", keywords[0]);
            Assert.AreEqual("Tag2", keywords[1]);
        }

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Settings_SystemAdmin_Settings_ReturnsSelectedView_Success()
		{
			List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
			adminMenuItems.Add(new MockSystemAdminMainMenu("settings", 123));
			adminMenuItems[0].ChildMenuItems.Add(new SettingsMenuItem(new SystemAdmin.Plugin.SystemAdminSettings()));
			adminMenuItems[0].ChildMenuItems[0].UniqueId = 23;
			MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
			SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

			ViewResult viewResult = sut.Settings(23) as ViewResult;
			Assert.IsNotNull(viewResult);
			Assert.IsNull(viewResult.ViewName);
			Assert.IsNotNull(viewResult.Model);

			SettingsViewModel viewModel = viewResult.Model as SettingsViewModel;
			Assert.IsNotNull(viewModel);

			Assert.AreEqual("SystemAdmin", viewModel.SettingsName);
			Assert.AreEqual(23, viewModel.SettingId);
			Assert.AreEqual(3, viewModel.Settings.Count);

			Assert.AreEqual("GoogleMapApiKey", viewModel.Settings[0].Name);
			Assert.AreEqual("", viewModel.Settings[0].Value);
			Assert.AreEqual("String", viewModel.Settings[0].DataType);

			Assert.AreEqual("ShowAppSettingsJson", viewModel.Settings[1].Name);
			Assert.AreEqual("False", viewModel.Settings[1].Value);
			Assert.AreEqual("Boolean", viewModel.Settings[1].DataType);

			Assert.AreEqual("EnableFormattedText", viewModel.Settings[2].Name);
			Assert.AreEqual("False", viewModel.Settings[2].Value);
			Assert.AreEqual("Boolean", viewModel.Settings[2].DataType);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Settings_Helpdesk_Settings_ReturnsSelectedView_Success()
		{
			List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
			adminMenuItems.Add(new MockSystemAdminMainMenu("settings", 123));
			adminMenuItems[0].ChildMenuItems.Add(new SettingsMenuItem(new HelpdeskPlugin.Classes.HelpdeskSettings()));
			adminMenuItems[0].ChildMenuItems[0].UniqueId = 29;
			MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
			SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

			ViewResult viewResult = sut.Settings(29) as ViewResult;
			Assert.IsNotNull(viewResult);
			Assert.IsNull(viewResult.ViewName);
			Assert.IsNotNull(viewResult.Model);

			SettingsViewModel viewModel = viewResult.Model as SettingsViewModel;
			Assert.IsNotNull(viewModel);

			Assert.AreEqual("HelpdeskSettings", viewModel.SettingsName);
			Assert.AreEqual(29, viewModel.SettingId);
			Assert.AreEqual(5, viewModel.Settings.Count);

			Assert.AreEqual("ShowCaptchaText", viewModel.Settings[0].Name);
			Assert.AreEqual("True", viewModel.Settings[0].Value);
			Assert.AreEqual("Boolean", viewModel.Settings[0].DataType);

			Assert.AreEqual("CaptchaWordLength", viewModel.Settings[1].Name);
			Assert.AreEqual("6", viewModel.Settings[1].Value);
			Assert.AreEqual("Int32", viewModel.Settings[1].DataType);

			Assert.AreEqual("ShowTickets", viewModel.Settings[2].Name);
			Assert.AreEqual("True", viewModel.Settings[2].Value);
			Assert.AreEqual("Boolean", viewModel.Settings[2].DataType);

			Assert.AreEqual("ShowFaq", viewModel.Settings[3].Name);
			Assert.AreEqual("True", viewModel.Settings[3].Value);
			Assert.AreEqual("Boolean", viewModel.Settings[3].DataType);

			Assert.AreEqual("ShowFeedback", viewModel.Settings[4].Name);
			Assert.AreEqual("True", viewModel.Settings[4].Value);
			Assert.AreEqual("Boolean", viewModel.Settings[4].DataType);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Settings_Post_NullModel_RedirectsToIndex()
		{
			List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
			adminMenuItems.Add(new MockSystemAdminMainMenu("settings", 123));
			adminMenuItems[0].ChildMenuItems.Add(new SettingsMenuItem(new HelpdeskPlugin.Classes.HelpdeskSettings()));
			adminMenuItems[0].ChildMenuItems[0].UniqueId = 29;
			MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
			SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

			RedirectToActionResult viewResult = sut.Settings(null) as RedirectToActionResult;
			Assert.IsNotNull(viewResult);
			Assert.AreEqual("Index", viewResult.ActionName);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Settings_Post_SettingsIdNotFound_RedirectsToIndex()
		{
			List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
			adminMenuItems.Add(new MockSystemAdminMainMenu("settings", 123));
			adminMenuItems[0].ChildMenuItems.Add(new SettingsMenuItem(new SystemAdmin.Plugin.SystemAdminSettings()));
			adminMenuItems[0].ChildMenuItems[0].UniqueId = 23;
			MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
			SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

			ViewResult viewResult = sut.Settings(23) as ViewResult;

			Assert.IsNotNull(viewResult);
			Assert.IsNull(viewResult.ViewName);
			Assert.IsNotNull(viewResult.Model);
			SettingsViewModel viewModel = viewResult.Model as SettingsViewModel;
			viewModel.SettingId = -98745;

			RedirectToActionResult redirectResult = sut.Settings(viewModel) as RedirectToActionResult;
			Assert.IsNotNull(redirectResult);
			Assert.AreEqual("Index", redirectResult.ActionName);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Settings_Post_InvalidSettingsItem_RedirectsToIndex()
		{
			List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
			adminMenuItems.Add(new MockSystemAdminMainMenu("settings", 123));
			adminMenuItems[0].ChildMenuItems.Add(new SettingsMenuItem(new SystemAdmin.Plugin.SystemAdminSettings()));
			adminMenuItems[0].ChildMenuItems[0].UniqueId = 23;
			MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
			SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

			ViewResult viewResult = sut.Settings(23) as ViewResult;
			adminMenuItems[0].ChildMenuItems[0] = new GCAdminMenu();
			adminMenuItems[0].ChildMenuItems[0].UniqueId = 23;

			Assert.IsNotNull(viewResult);
			Assert.IsNull(viewResult.ViewName);
			Assert.IsNotNull(viewResult.Model);
			SettingsViewModel viewModel = viewResult.Model as SettingsViewModel;

			RedirectToActionResult redirectResult = sut.Settings(viewModel) as RedirectToActionResult;
			Assert.IsNotNull(redirectResult);
			Assert.AreEqual("Index", redirectResult.ActionName);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Settings_Post_PropertyNamesDoNotMatch_ReturnsModelStateError()
		{
			List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
			adminMenuItems.Add(new MockSystemAdminMainMenu("settings", 123));
			adminMenuItems[0].ChildMenuItems.Add(new SettingsMenuItem(new SystemAdmin.Plugin.SystemAdminSettings()));
			adminMenuItems[0].ChildMenuItems[0].UniqueId = 23;
			MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
			SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

			ViewResult viewResult = sut.Settings(23) as ViewResult;

			Assert.IsNotNull(viewResult);
			Assert.IsNull(viewResult.ViewName);
			Assert.IsNotNull(viewResult.Model);
			SettingsViewModel viewModel = viewResult.Model as SettingsViewModel;

			viewModel.Settings[0].Name += "A";
			viewModel.Settings[1].Name += "A";
			viewModel.Settings[2].Name += "A";

			JsonResult postViewResult = sut.Settings(viewModel) as JsonResult;
			Assert.IsNotNull(postViewResult);

			JsonResponseModel jsonResponseModel = postViewResult.Value as JsonResponseModel;
			Assert.IsNotNull(jsonResponseModel);
			Assert.IsFalse(jsonResponseModel.Success);

			List<string> errors = JsonSerializer.Deserialize<List<string>>(jsonResponseModel.ResponseData);

			Assert.AreEqual(3, errors.Count);
			Assert.AreEqual("Property GoogleMapApiKeyA was not found", errors[0]);
			Assert.AreEqual("Property ShowAppSettingsJsonA was not found", errors[1]);
			Assert.AreEqual("Property EnableFormattedTextA was not found", errors[2]);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Settings_Post_SystemSettings_ValidateSettingsAgainstAppSettings_ErrorsIdentified()
		{
			List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
			adminMenuItems.Add(new MockSystemAdminMainMenu("settings", 123));
			adminMenuItems[0].ChildMenuItems.Add(new SettingsMenuItem(new SystemAdmin.Plugin.SystemAdminSettings()));
			adminMenuItems[0].ChildMenuItems[0].UniqueId = 23;
			MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
			SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

			ViewResult viewResult = sut.Settings(23) as ViewResult;

			Assert.IsNotNull(viewResult);
			Assert.IsNull(viewResult.ViewName);
			Assert.IsNotNull(viewResult.Model);
			SettingsViewModel viewModel = viewResult.Model as SettingsViewModel;
			viewModel.Settings[0].Value = "short api";

			JsonResult postViewResult = sut.Settings(viewModel) as JsonResult;
			Assert.IsNotNull(postViewResult);

			JsonResponseModel jsonResponseModel = postViewResult.Value as JsonResponseModel;
			Assert.IsNotNull(jsonResponseModel);
			Assert.IsFalse(jsonResponseModel.Success);
			List<string> errors = JsonSerializer.Deserialize<List<string>>(jsonResponseModel.ResponseData);

			Assert.AreEqual(1, errors.Count);
			Assert.AreEqual("GoogleMapApiKey: Minimum length should be at least 15 characters long, is currently 9 characters", errors[0]);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Settings_Post_SystemSettings_ValidateSettingsAgainstAppSettings_Success()
		{
			List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
			adminMenuItems.Add(new MockSystemAdminMainMenu("settings", 123));
			adminMenuItems[0].ChildMenuItems.Add(new SettingsMenuItem(new SystemAdmin.Plugin.SystemAdminSettings()));
			adminMenuItems[0].ChildMenuItems[0].UniqueId = 23;
			MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
			SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

			ViewResult viewResult = sut.Settings(23) as ViewResult;

			Assert.IsNotNull(viewResult);
			Assert.IsNull(viewResult.ViewName);
			Assert.IsNotNull(viewResult.Model);
			SettingsViewModel viewModel = viewResult.Model as SettingsViewModel;
			viewModel.Settings[0].Value = "short api asdfasdf";

			JsonResult postViewResult = sut.Settings(viewModel) as JsonResult;
			Assert.IsNotNull(postViewResult);

			JsonResponseModel jsonResponseModel = postViewResult.Value as JsonResponseModel;
			Assert.IsNotNull(jsonResponseModel);
			Assert.IsTrue(jsonResponseModel.Success);
			string growl = jsonResponseModel.ResponseData;
			Assert.AreEqual("The settings have been updated", growl);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Settings_Post_BadEggSettings_ValidateSettingsAgainstAppSettings_Success()
		{
			List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
			adminMenuItems.Add(new MockSystemAdminMainMenu("settings", 123));
			adminMenuItems[0].ChildMenuItems.Add(new SettingsMenuItem(new SystemAdmin.Plugin.SystemAdminSettings()));
			adminMenuItems[0].ChildMenuItems[0].UniqueId = 23;
			MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
			SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

			ViewResult viewResult = sut.Settings(23) as ViewResult;

			Assert.IsNotNull(viewResult);
			Assert.IsNull(viewResult.ViewName);
			Assert.IsNotNull(viewResult.Model);
			SettingsViewModel viewModel = viewResult.Model as SettingsViewModel;

			JsonResult postViewResult = sut.Settings(viewModel) as JsonResult;
			Assert.IsNotNull(postViewResult);

			JsonResponseModel jsonResponseModel = postViewResult.Value as JsonResponseModel;
			Assert.IsNotNull(jsonResponseModel);
			Assert.IsTrue(jsonResponseModel.Success);
			string growl = jsonResponseModel.ResponseData;
			Assert.AreEqual("The settings have been updated", growl);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Settings_Post_InvalidBoolValue_ReturnsError()
		{
			List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
			adminMenuItems.Add(new MockSystemAdminMainMenu("settings", 123));
			adminMenuItems[0].ChildMenuItems.Add(new SettingsMenuItem(new SystemAdmin.Plugin.SystemAdminSettings()));
			adminMenuItems[0].ChildMenuItems[0].UniqueId = 23;
			MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
			SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

			ViewResult viewResult = sut.Settings(23) as ViewResult;

			Assert.IsNotNull(viewResult);
			Assert.IsNull(viewResult.ViewName);
			Assert.IsNotNull(viewResult.Model);
			SettingsViewModel viewModel = viewResult.Model as SettingsViewModel;
			viewModel.Settings[1].Value = "9874569874569874569874569874569874";

			JsonResult postViewResult = sut.Settings(viewModel) as JsonResult;
			Assert.IsNotNull(postViewResult);

			JsonResponseModel jsonResponseModel = postViewResult.Value as JsonResponseModel;
			Assert.IsNotNull(jsonResponseModel);
			Assert.IsFalse(jsonResponseModel.Success);
			Assert.AreEqual("[\"Property ShowAppSettingsJson does not contain a valid Boolean value\"]", jsonResponseModel.ResponseData);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Settings_Post_InvalidInt32Value_ReturnsError()
		{
			List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
			adminMenuItems.Add(new MockSystemAdminMainMenu("settings", 123));
			adminMenuItems[0].ChildMenuItems.Add(new SettingsMenuItem(new HelpdeskPlugin.Classes.HelpdeskSettings()));
			adminMenuItems[0].ChildMenuItems[0].UniqueId = 23;
			MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
			SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

			ViewResult viewResult = sut.Settings(23) as ViewResult;

			Assert.IsNotNull(viewResult);
			Assert.IsNull(viewResult.ViewName);
			Assert.IsNotNull(viewResult.Model);
			SettingsViewModel viewModel = viewResult.Model as SettingsViewModel;
			viewModel.Settings[1].Value = "9874569874569874569874569874569874";

			JsonResult postViewResult = sut.Settings(viewModel) as JsonResult;
			Assert.IsNotNull(postViewResult);

			JsonResponseModel jsonResponseModel = postViewResult.Value as JsonResponseModel;
			Assert.IsNotNull(jsonResponseModel);
			Assert.IsFalse(jsonResponseModel.Success);
			Assert.AreEqual("[\"Property CaptchaWordLength does not contain a valid Int32 value\"]", jsonResponseModel.ResponseData);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Settings_Post_InvalidInt64Value_ReturnsError()
		{
			List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
			adminMenuItems.Add(new MockSystemAdminMainMenu("settings", 123));
			adminMenuItems[0].ChildMenuItems.Add(new SettingsMenuItem(new BadEgg.Plugin.BadEggSettings()));
			adminMenuItems[0].ChildMenuItems[0].UniqueId = 23;
			MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
			SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

			ViewResult viewResult = sut.Settings(23) as ViewResult;

			Assert.IsNotNull(viewResult);
			Assert.IsNull(viewResult.ViewName);
			Assert.IsNotNull(viewResult.Model);
			SettingsViewModel viewModel = viewResult.Model as SettingsViewModel;
			viewModel.Settings[1].Value = "987456987456987456987456987456asdf12q349874";

			JsonResult postViewResult = sut.Settings(viewModel) as JsonResult;
			Assert.IsNotNull(postViewResult);

			JsonResponseModel jsonResponseModel = postViewResult.Value as JsonResponseModel;
			Assert.IsNotNull(jsonResponseModel);
			Assert.IsFalse(jsonResponseModel.Success);
			Assert.AreEqual("[\"Property ConnectionsPerSecond does not contain a valid UInt32 value\"]", jsonResponseModel.ResponseData);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Settings_SettingsContainsStringArray_ReturnsAsValidProperty()
		{
			List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
			adminMenuItems.Add(new MockSystemAdminMainMenu("settings", 123));
			adminMenuItems[0].ChildMenuItems.Add(new SettingsMenuItem(new ErrorManager.Plugin.ErrorManagerSettings()));
			adminMenuItems[0].ChildMenuItems[0].UniqueId = 23;
			MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
			SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

			ViewResult viewResult = sut.Settings(23) as ViewResult;

			Assert.IsNotNull(viewResult);
			Assert.IsNull(viewResult.ViewName);
			Assert.IsNotNull(viewResult.Model);
			SettingsViewModel viewModel = viewResult.Model as SettingsViewModel;
			Assert.AreEqual(4, viewModel.Settings.Count);
			Assert.AreEqual("String[]", viewModel.Settings[1].DataType);
			Assert.AreEqual(603, viewModel.Settings[1].Value.Length);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Settings_SettingsUsesApplicationVariables_DisplaysAppVariables()
		{
			List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
			adminMenuItems.Add(new MockSystemAdminMainMenu("settings", 123));
			adminMenuItems[0].ChildMenuItems.Add(new SettingsMenuItem(new LoginPlugin.LoginControllerSettings()));
			adminMenuItems[0].ChildMenuItems[0].UniqueId = 23;
			MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
			SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService);

			ViewResult viewResult = sut.Settings(23) as ViewResult;

			Assert.IsNotNull(viewResult);
			Assert.IsNull(viewResult.ViewName);
			Assert.IsNotNull(viewResult.Model);
			SettingsViewModel viewModel = viewResult.Model as SettingsViewModel;
			Assert.AreEqual(13, viewModel.Settings.Count);
			Assert.AreEqual("FacebookSecret", viewModel.Settings[12].Name);
			Assert.AreEqual("%FacebookSecret%", viewModel.Settings[12].Value);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Settings_Post_WebSmokeTestSettings_UpdateJson_Success()
		{
			MockPluginManagerConfiguration mockPluginManagerConfiguration = new MockPluginManagerConfiguration();

			List<SystemAdminMainMenu> adminMenuItems = new List<SystemAdminMainMenu>();
			adminMenuItems.Add(new MockSystemAdminMainMenu("settings", 123));
			adminMenuItems[0].ChildMenuItems.Add(new SettingsMenuItem(new WebSmokeTest.Plugin.WebSmokeTestSettings()));
			adminMenuItems[0].ChildMenuItems[0].UniqueId = 23;
			MockSystemAdminHelperService mockSystemAdminHelperService = new MockSystemAdminHelperService(adminMenuItems);
			SystemAdminController sut = CreateSystemAdminController(null, mockSystemAdminHelperService, 
				null, null, null, mockPluginManagerConfiguration);

			ViewResult viewResult = sut.Settings(23) as ViewResult;

			Assert.IsNotNull(viewResult);
			Assert.IsNull(viewResult.ViewName);
			Assert.IsNotNull(viewResult.Model);
			SettingsViewModel viewModel = viewResult.Model as SettingsViewModel;
			viewModel.Settings[2].Value = "[\"NewValue1\",\"NewValue2\",\"NewValue3\"]";
			JsonResult postViewResult = sut.Settings(viewModel) as JsonResult;
			Assert.IsNotNull(postViewResult);

			JsonResponseModel jsonResponseModel = postViewResult.Value as JsonResponseModel;
			Assert.IsNotNull(jsonResponseModel);
			Assert.IsTrue(jsonResponseModel.Success);
			Assert.AreEqual("The settings have been updated", jsonResponseModel.ResponseData);

			string fileContents = System.IO.File.ReadAllText(mockPluginManagerConfiguration.ConfigurationFile);
			Assert.IsTrue(fileContents.Contains("\"SiteId\": [\r\n      \"NewValue1\",\r\n      \"NewValue2\",\r\n      \"NewValue3\"\r\n    ],"));
		}

		private SystemAdminController CreateSystemAdminController(MockSettingsProvider settingsProvider = null,
            MockSystemAdminHelperService systemAdminHelperService = null,
            MockSeoProvider seoProvider = null,
            MockUserSearch userSearch = null,
            MockClaimsProvider claimsProvider = null,
			MockPluginManagerConfiguration pluginManagerConfiguration = null)
        {
			settingsProvider = settingsProvider ?? new MockSettingsProvider();

			SystemAdminController Result = new SystemAdminController(
				settingsProvider,
                systemAdminHelperService ?? new MockSystemAdminHelperService(),
                seoProvider ?? new MockSeoProvider(),
                userSearch ?? new MockUserSearch(),
                claimsProvider ?? new MockClaimsProvider(), 
				new MockPluginClassesService(new List<object>() { settingsProvider }),
				pluginManagerConfiguration ?? new MockPluginManagerConfiguration());

            Result.ControllerContext = CreateTestControllerContext();

            return Result;
        }
    }
}
