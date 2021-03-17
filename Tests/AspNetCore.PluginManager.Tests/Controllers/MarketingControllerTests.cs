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
 *  File: MarketingControllerTests.cs
 *
 *  Purpose:  Tests for marketing controller
 *
 *  Date        Name                Reason
 *  14/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MarketingPlugin;
using SharedPluginFeatures;
using PluginManager.Abstractions;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using MarketingPlugin.Classes.SystemAdmin;
using MarketingPlugin.Controllers;

namespace AspNetCore.PluginManager.Tests.Controllers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class MarketingControllerTests : BaseControllerTests
    {
        private const string TestCategoryName = "Marketing Tests";

        [TestInitialize]
        public void InitializeSpiderControllerTests()
        {
            InitializeMarketingPluginPluginManager();
        }

        #region Marketing Settings

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void MarketingSettings_ValidateDefaultValue_ProcessStaticFiles_ReturnsFalse()
        {
            TestSettingsProvider testSettingsProvider = new TestSettingsProvider("{}");
            MarketingSettings sut = testSettingsProvider.GetSettings<MarketingSettings>(OffersController.Name);

            Assert.IsFalse(sut.ProcessStaticFiles);
        }


        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void MarketingSettings_ValidateDefaultValue_ProcessStaticFiles_ReturnsTrue()
        {
            TestSettingsProvider testSettingsProvider = new TestSettingsProvider("{\"Offers\":{\"ProcessStaticFiles\":true}}");
            MarketingSettings sut = testSettingsProvider.GetSettings<MarketingSettings>(OffersController.Name);

            Assert.IsTrue(sut.ProcessStaticFiles);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void MarketingSettings_ValidateDefaultValue_StaticFileExtensions_ReturnsDefaultStaticFiles()
        {
            TestSettingsProvider testSettingsProvider = new TestSettingsProvider("{}");
            MarketingSettings sut = testSettingsProvider.GetSettings<MarketingSettings>(OffersController.Name);

            Assert.AreEqual(Constants.StaticFileExtensions, sut.StaticFileExtensions);
        }

        #endregion Marketing Settings

        #region Marketing Timings

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void MarketingTimingsSubMenu_Construct_DefaultInstance_Success()
        {
            MarketingTimingsSubMenu sut = new MarketingTimingsSubMenu();
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void MarketingTimingsSubMenu_ValidateInstanceOfSystemAdminSubMenu()
        {
            MarketingTimingsSubMenu sut = new MarketingTimingsSubMenu();
            Assert.IsInstanceOfType(sut, typeof(SystemAdminSubMenu));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void MarketingTimingsSubMenu_SortOrder_ReturnsZero()
        {
            MarketingTimingsSubMenu sut = new MarketingTimingsSubMenu();
            Assert.AreEqual(0, sut.SortOrder());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void MarketingTimingsSubMenu_Action_ReturnsEmptyString()
        {
            MarketingTimingsSubMenu sut = new MarketingTimingsSubMenu();
            Assert.AreEqual(String.Empty, sut.Action());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void MarketingTimingsSubMenu_Area_ReturnsEmptyString()
        {
            MarketingTimingsSubMenu sut = new MarketingTimingsSubMenu();
            Assert.AreEqual(String.Empty, sut.Area());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void MarketingTimingsSubMenu_Controller_ReturnsEmptyString()
        {
            MarketingTimingsSubMenu sut = new MarketingTimingsSubMenu();
            Assert.AreEqual(String.Empty, sut.Controller());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void MarketingTimingsSubMenu_Image_ReturnsStopwatch()
        {
            MarketingTimingsSubMenu sut = new MarketingTimingsSubMenu();
            Assert.AreEqual("stopwatch", sut.Image());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void MarketingTimingsSubMenu_MenuType_ReturnsGrid()
        {
            MarketingTimingsSubMenu sut = new MarketingTimingsSubMenu();
            Assert.AreEqual(Enums.SystemAdminMenuType.Grid, sut.MenuType());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void MarketingTimingsSubMenu_Name_ReturnsMarketing()
        {
            MarketingTimingsSubMenu sut = new MarketingTimingsSubMenu();
            Assert.AreEqual("Marketing", sut.Name());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void MarketingTimingsSubMenu_ParentMenuName_ReturnsTimings()
        {
            MarketingTimingsSubMenu sut = new MarketingTimingsSubMenu();
            Assert.AreEqual("Timings", sut.ParentMenuName());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void MarketingTimingsSubMenu_Data_ReturnsSettingsValue()
        {
            MarketingTimingsSubMenu sut = new MarketingTimingsSubMenu();
            string data = sut.Data();

            string[] rows = data.Split("\r");
            Assert.AreEqual("Setting|Value", rows[0]);
            Assert.IsTrue(rows[1].StartsWith("Total Requests|"));
            Assert.IsTrue(rows[2].StartsWith("Fastest ms|"));
            Assert.IsTrue(rows[3].StartsWith("Slowest ms|"));
            Assert.IsTrue(rows[4].StartsWith("Average ms|"));
            Assert.IsTrue(rows[5].StartsWith("Trimmed Avg ms|"));
            Assert.IsTrue(rows[6].StartsWith("Total ms|"));
        }

        #endregion Marketing Timings

        #region PluginInitialisation

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void PluginInitialisation_ExtendsIPluginAndIInitialiseEvents()
        {
            PluginInitialisation sut = new PluginInitialisation();

            Assert.IsInstanceOfType(sut, typeof(IPlugin));
            Assert.IsInstanceOfType(sut, typeof(IInitialiseEvents));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void PluginInitialisation_AfterConfigure_DoesNotConfigurePipeline_Success()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.AfterConfigure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void PluginInitialisation_Configure_DoesNotConfigurePipeline_Success()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.Configure(testApplicationBuilder);

            Assert.IsTrue(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void PluginInitialisation_BeforeConfigure_DoesNotRegisterApplicationsToBuilder()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.BeforeConfigure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void PluginInitialisation_Initialise_DoesNotThrowException()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.Initialise(new TestLogger());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void PluginInitialisation_Finalise_DoesNotThrowException()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.Finalise();
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void PluginInitialisation_ConfigureServices_DoesNotThrowException()
        {
            IServiceCollection serviceCollection = new ServiceCollection() as IServiceCollection;
            PluginInitialisation sut = new PluginInitialisation();

            sut.ConfigureServices(serviceCollection);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void PluginInitialisation_GetVersion_ReturnsOne()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            ushort version = sut.GetVersion();

            Assert.AreEqual(1, version);
        }

        #endregion PluginInitialisation
    }
}
