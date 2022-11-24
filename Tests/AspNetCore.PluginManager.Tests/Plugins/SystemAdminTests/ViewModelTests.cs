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
 *  File: ViewModelTests.cs
 *
 *  Purpose:  Tests for System Admin View Models
 *
 *  Date        Name                Reason
 *  04/10/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedPluginFeatures;

using SystemAdmin.Plugin.Models;

namespace AspNetCore.PluginManager.Tests.Plugins.SystemAdminTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ViewModelTests : GenericBaseClass
    {
        private const string TestCategoryName = "System Admin";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AvailableIconViewModel_SuccessfullyCreatedWithoutHomeItems()
        {
            AvailableIconViewModel sut = new AvailableIconViewModel(GenerateTestBaseModelData());
            Assert.IsNull(sut.HomeIcons);
            Assert.IsNull(sut.MenuItems);
            Assert.IsNull(sut.Title);
            Assert.AreEqual(0, sut.Breadcrumbs.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AvailableIconViewModel_SuccessfullyCreatedWithHomeItems()
        {
            List<SystemAdminMainMenu> homeIcons = new List<SystemAdminMainMenu>();
            AvailableIconViewModel sut = new AvailableIconViewModel(GenerateTestBaseModelData(), homeIcons);
            Assert.IsNotNull(sut.HomeIcons);
            Assert.AreEqual(0, sut.HomeIcons.Count);
            Assert.IsNull(sut.MenuItems);
            Assert.IsNull(sut.Title);
            Assert.AreEqual(0, sut.Breadcrumbs.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AvailableIconViewModel_InvalidParam_HomeMenuItems_Throws_ArgumentNullException()
        {
            AvailableIconViewModel sut = new AvailableIconViewModel(GenerateTestBaseModelData(), homeMenuItems: null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AvailableIconViewModel_InvalidParam_MainMenu_Throws_ArgumentNullException()
        {
            AvailableIconViewModel sut = new AvailableIconViewModel(GenerateTestBaseModelData(), mainMenu: null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AvailableIconViewModel_SuccessfullyCreatedWithMainMenu()
        {
            SystemAdminMainMenu selectedMenu = new SystemAdminMainMenu("test", -123);
            AvailableIconViewModel sut = new AvailableIconViewModel(GenerateTestBaseModelData(), selectedMenu);
            Assert.IsNull(sut.HomeIcons);
            Assert.IsNotNull(sut.MenuItems);
            Assert.IsNotNull(sut.Title);
            Assert.AreEqual("test", sut.Title);
            Assert.AreEqual(0, sut.Breadcrumbs.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AvailableIconViewModel_ProcessImages_Success()
        {
            SystemAdminMainMenu selectedMenu = new SystemAdminMainMenu("test", -123);
            AvailableIconViewModel sut = new AvailableIconViewModel(GenerateTestBaseModelData(), selectedMenu);
            Assert.AreEqual("/images/SystemAdmin/badegg.png", sut.ProcessImage("badegg"));
            Assert.AreEqual("/images/SystemAdmin/stopwatch.png", sut.ProcessImage("stopwatch"));
            Assert.AreEqual("/images/SystemAdmin/chart.png", sut.ProcessImage("chart"));
            Assert.AreEqual("/images/SystemAdmin/setting-icon.png", sut.ProcessImage(""));
            Assert.AreEqual("/images/SystemAdmin/setting-icon.png", sut.ProcessImage(null));
            Assert.AreEqual("/test.png", sut.ProcessImage("/test.png"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AvailableIconViewModel_GetMenuLink_InvalidParam_Throws_ArgumentNullException()
        {
            AvailableIconViewModel sut = new AvailableIconViewModel(GenerateTestBaseModelData());
            sut.GetMenuLink(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AvailableIconViewModel_GetMenuLink_Success()
        {
            AvailableIconViewModel sut = new AvailableIconViewModel(GenerateTestBaseModelData());
            Assert.AreEqual("/SystemAdmin/Grid/0", sut.GetMenuLink(new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.Grid)));
            Assert.AreEqual("/SystemAdmin/Text/0", sut.GetMenuLink(new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.Text)));
            Assert.AreEqual("/TestC/TestA/", sut.GetMenuLink(new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.View)));
            Assert.AreEqual("/SystemAdmin/Map/0", sut.GetMenuLink(new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.Map)));
            Assert.AreEqual("/SystemAdmin/TextEx/0", sut.GetMenuLink(new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.FormattedText)));
            Assert.AreEqual("/SystemAdmin/View/0", sut.GetMenuLink(new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.PartialView)));
            Assert.AreEqual("/SystemAdmin/Chart/0", sut.GetMenuLink(new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.Chart)));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AvailableIconViewModel_GetMenuLink_InvalidMenuType_Throws_InvalidOperationException()
        {
            AvailableIconViewModel sut = new AvailableIconViewModel(GenerateTestBaseModelData());
            sut.GetMenuLink(new MockSystemAdminSubMenu((Enums.SystemAdminMenuType)23));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChartViewModel_Construct_InvalidSubMenu_Throws_ArgumentNullException()
        {
            ChartViewModel sut = new ChartViewModel(GenerateTestBaseModelData(), null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ChartViewModel_Construct_Success()
        {
            ChartViewModel sut = new ChartViewModel(GenerateTestBaseModelData(), new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.Chart));
            Assert.AreEqual("Mock Sub Menu", sut.Title);
            Assert.AreEqual("Test Title", sut.ChartTitle);
            Assert.AreEqual(2, sut.DataValues.Count);
            Assert.AreEqual(2, sut.DataNames.Count);
            Assert.AreEqual(1.1m, sut.DataValues["value 1"][0]);
            Assert.AreEqual(2.2m, sut.DataValues["value 1"][1]);
            Assert.AreEqual(3.3m, sut.DataValues["value 1"][2]);
            Assert.AreEqual(4.4m, sut.DataValues["value 2"][0]);
            Assert.AreEqual(5.5m, sut.DataValues["value 2"][1]);
            Assert.AreEqual(6.6m, sut.DataValues["value 2"][2]);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ErrorViewModel_Construct_NoParams_Success()
        {
            ErrorViewModel sut = new ErrorViewModel();
            Assert.IsNull(sut.RequestId);
            Assert.IsFalse(sut.ShowRequestId);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ErrorViewModel_Construct_WithParams_Success()
        {
            ErrorViewModel sut = new ErrorViewModel(GenerateTestBaseModelData(), "my request");
            Assert.IsNotNull(sut.RequestId);
            Assert.AreEqual("my request", sut.RequestId);
            Assert.IsTrue(sut.ShowRequestId);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GridViewModel_Construct_InvalidParams_Null_Throws_ArgumentNullException()
        {
            new GridViewModel(GenerateTestBaseModelData(), null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentException))]
        public void GridViewModel_NullData_Throws_ArgumentException()
        {
            new GridViewModel(GenerateTestBaseModelData(), new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.Grid, null));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GridViewModel_InvalidHeaderData_Throws_InvalidOperationException()
        {
            new GridViewModel(GenerateTestBaseModelData(), new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.Grid, "  "));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GridViewModel_ColumnCountDoesNotMatchHeaderColumnCount_TooMany_Throws_InvalidOperationException()
        {
            new GridViewModel(GenerateTestBaseModelData(), new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.Grid, "header\rcol 1|col 2"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GridViewModel_ColumnCountDoesNotMatchHeaderColumnCount_TooFew_Throws_InvalidOperationException()
        {
            new GridViewModel(GenerateTestBaseModelData(), new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.Grid, "header1|header 2\rcol 1"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GridViewModel_GenerateValidViewModel_Success()
        {
            const string data = "header 1|header 2\rcol r1a | col r1b\rcol r2a|col r2b";
            GridViewModel sut = new GridViewModel(GenerateTestBaseModelData(), new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.Grid, data));

            Assert.AreEqual("Mock Sub Menu", sut.Title);
            Assert.AreEqual(2, sut.HeaderColumnCount);
            Assert.AreEqual("header 1", sut.Headers[0]);
            Assert.AreEqual("header 2", sut.Headers[1]);
            Assert.AreEqual(2, sut.Items.Count);
            Assert.AreEqual("col r1a ", sut.Items[0][0]);
            Assert.AreEqual(" col r1b", sut.Items[0][1]);
            Assert.AreEqual("col r2a", sut.Items[1][0]);
            Assert.AreEqual("col r2b", sut.Items[1][1]);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapViewModel_Construct_InvalidParam_SettingsProvider_Null_Throws_ArgumentNullException()
        {
            new MapViewModel(GenerateTestBaseModelData(), null, new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.Map));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapViewModel_Construct_InvalidParam_SubMenu_Null_Throws_ArgumentNullException()
        {
            new MapViewModel(GenerateTestBaseModelData(), new MockSettingsProvider(), null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MapViewModel_Construct_InvalidParam_SubMenuData_Null_Throws_InvalidOperationException()
        {
            new MapViewModel(GenerateTestBaseModelData(), new MockSettingsProvider(),
                new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.Map, null));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MapViewModel_Construct_InvalidParam_SubMenuData_WhiteSpace_Throws_InvalidOperationException()
        {
            new MapViewModel(GenerateTestBaseModelData(), new MockSettingsProvider(),
                new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.Map, "\r \t"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void MapViewModel_Construct_GoogleApiKeyFound_Success()
        {
            MapViewModel sut = new MapViewModel(GenerateTestBaseModelData(), new MockSettingsProvider("{\"SystemAdmin\":{\"GoogleMapApiKey\":\"the key to google maps is...\"}}"),
                new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.Map, "random data"));

            Assert.AreEqual("the key to google maps is...", sut.GoogleMapApiKey);
            Assert.AreEqual("random data", sut.MapLocationData);
            Assert.AreEqual("Mock Sub Menu", sut.Title);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PartialViewModel_Construct_InvalidSubMenuParam_Null_Throws_ArgumentNullException()
        {
            new PartialViewModel(GenerateTestBaseModelData(), null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void PartialViewModel_Construct_ValidInstance_Success()
        {
            PartialViewModel sut = new PartialViewModel(GenerateTestBaseModelData(),
                new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.PartialView));

            Assert.AreEqual("Mock Sub Menu", sut.Title);
            Assert.AreEqual("/TestC/", sut.ControllerRoot);
            Assert.AreEqual("/TestC/TestA", sut.PartialView);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void PermissionsModel_Construct_ValidInstance_Success()
        {
            PermissionsModel sut = new PermissionsModel(GenerateTestBaseModelData());
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SeoDataModel_Construct_NoParams_ValidInstance_Success()
        {
            SeoDataModel sut = new SeoDataModel();
            Assert.IsNotNull(sut);
            Assert.AreEqual("", sut.SeoTitle);
            Assert.IsNull(sut.SeoMetaDescription);
            Assert.AreEqual("", sut.SeoTags);
            Assert.AreEqual("", sut.SeoAuthor);
            Assert.AreEqual("", sut.SeoUrl);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SeoDataModel_Construct_InvalidUrl_Null_Throws_ArgumentNullException()
        {
            new SeoDataModel(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SeoDataModel_Construct_InvalidUrl_EmptyString_Throws_ArgumentNullException()
        {
            new SeoDataModel("");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SeoDataModel_Construct_ValidInstance_Success()
        {
            SeoDataModel sut = new SeoDataModel("/url")
            {
                SeoTitle = "Title",
                SeoMetaDescription = "Meta",
                SeoTags = "Tags",
                SeoAuthor = "Author"
            };

            Assert.AreEqual("/url", sut.SeoUrl);
            Assert.AreEqual("Title", sut.SeoTitle);
            Assert.AreEqual("Author", sut.SeoAuthor);
            Assert.AreEqual("Tags", sut.SeoTags);
            Assert.AreEqual("Meta", sut.SeoMetaDescription);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TextExViewModel_Construct_InvalidSubMenu_Null_Throws_ArgumentNullException()
        {
            new TextExViewModel(GenerateTestBaseModelData(), new MockSettingsProvider(), null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TextExViewModel_Construct_InvalidSettingsProvider_Null_Throws_ArgumentNullException()
        {
            new TextExViewModel(GenerateTestBaseModelData(), null, new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.FormattedText));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void TextExViewModel_Construct_SettingsDoesNotAllowFormattedText_Success()
        {
            TextExViewModel sut = new TextExViewModel(GenerateTestBaseModelData(),
                new MockSettingsProvider("{\"SystemAdmin\":{\"EnableFormattedText\": false}}"),
                new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.FormattedText));
            Assert.AreEqual("Mock Sub Menu", sut.Title);
            Assert.AreEqual("Formatted Text is not enabed", sut.Text);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void TextExViewModel_Construct_SettingsAllowsFormattedText_Success()
        {
            TextExViewModel sut = new TextExViewModel(GenerateTestBaseModelData(),
                new MockSettingsProvider("{\"SystemAdmin\":{\"EnableFormattedText\": true}}"),
                new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.FormattedText));
            Assert.AreEqual("Mock Sub Menu", sut.Title);
            Assert.AreEqual("Formatted Text", sut.Text);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TextViewModel_Construct_InvalidSubMenu_Null_Throws_ArgumentNullException()
        {
            new TextViewModel(GenerateTestBaseModelData(), null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void TextViewModel_Construct_ValidInstance_Success()
        {
            TextViewModel sut = new TextViewModel(GenerateTestBaseModelData(), new MockSystemAdminSubMenu(Enums.SystemAdminMenuType.Text));

            Assert.AreEqual("Mock Sub Menu", sut.Title);
            Assert.AreEqual("Some&nbsp;data&nbsp;&nbsp;&nbsp;&nbsp;to<br />with&nbsp;&lt;html&gt;markup", sut.Text);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UserPermissionsViewModel_Construct_DefaultConstructor_Success()
        {
            UserPermissionsViewModel sut = new UserPermissionsViewModel();
            Assert.AreEqual("", sut.SelectedClaims);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserPermissionsViewModel_Construct_InvalidUserClaims_Null_Throws_ArgumentNullException()
        {
            new UserPermissionsViewModel(1, null, new List<string>());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserPermissionsViewModel_Construct_InvalidSystemClaims_Null_Throws_ArgumentNullException()
        {
            new UserPermissionsViewModel(1, new List<string>(), null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UserPermissionsViewModel_Construct_ValidInstance_Success()
        {
            List<string> userClaims = new List<string>();
            userClaims.Add("claim 1");
            userClaims.Add("claim 2");
            userClaims.Add("claim 3");

            List<string> systemClaims = new List<string>();
            systemClaims.Add("Sys claim 1");
            systemClaims.Add("Sys claim 2");

            UserPermissionsViewModel sut = new UserPermissionsViewModel(123, userClaims, systemClaims);
            Assert.AreEqual(123L, sut.UserId);
            Assert.AreEqual(3, sut.UserClaims.Count);
            Assert.AreEqual("claim 1", sut.UserClaims[0]);
            Assert.AreEqual("claim 2", sut.UserClaims[1]);
            Assert.AreEqual("claim 3", sut.UserClaims[2]);

            Assert.AreEqual(2, sut.SystemClaims.Count);
            Assert.AreEqual("Sys claim 1", sut.SystemClaims[0]);
            Assert.AreEqual("Sys claim 2", sut.SystemClaims[1]);

            Assert.AreEqual("claim 1;claim 2;claim 3", sut.SelectedClaims);
        }
    }
}