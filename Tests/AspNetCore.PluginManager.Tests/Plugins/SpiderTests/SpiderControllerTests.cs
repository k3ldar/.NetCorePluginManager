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
 *  File: SpiderControllerTests.cs
 *
 *  Purpose:  Tests for spider controller
 *
 *  Date        Name                Reason
 *  01/10/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using AspNetCore.PluginManager.Tests.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;

using SharedPluginFeatures;

using Spider.Plugin.Classes;
using Spider.Plugin.Controllers;
using Spider.Plugin.Models;

using pm = PluginManager.Internal;

namespace AspNetCore.PluginManager.Tests.Plugins.SpiderTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public sealed class SpiderControllerTests : BaseControllerTests
    {
        [TestInitialize]
        public void InitializeSpiderControllerTests()
        {
            InitializeSpiderPluginPluginManager();
        }

        [TestMethod]
        public void SpiderController_ValidateAttributes()
        {
            Assert.IsTrue(ClassHasAttribute<RestrictedIpRouteAttribute>(typeof(SpiderController)));
            Assert.IsTrue(ClassHasAttribute<AuthorizeAttribute>(typeof(SpiderController)));
            Assert.IsTrue(ClassHasAttribute<DenySpiderAttribute>(typeof(SpiderController)));
        }

        [TestMethod]
        public void Create_SpiderControllerInstance_Success()
        {
            SpiderController sut = CreateSpiderControllerInstance();
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void Invoke_SpiderController_Index_Success()
        {
            SpiderController sut = CreateSpiderControllerInstance();
            IActionResult response = sut.Index();

            // Assert
            Assert.IsInstanceOfType(response, typeof(PartialViewResult));

            PartialViewResult viewResult = response as PartialViewResult;

            Assert.IsNotNull(viewResult.Model);

            ValidateBaseModel(viewResult);

            Assert.AreEqual("/Views/Spider/_systemRobots.cshtml", viewResult.ViewName);

            Assert.IsInstanceOfType(viewResult.Model, typeof(EditRobotsModel));
        }

        [TestMethod]
        public void SpiderController_Index_ValidateAttributes()
        {
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(SpiderController), "Index"));
            Assert.IsTrue(MethodHasAttribute<LoggedInAttribute>(typeof(SpiderController), "Index"));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(SpiderController), "Index"));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(SpiderController), "Index"));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(SpiderController), "Index"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_EditRobotsModel_InvalidModelData_Null_Throws_ArgumentNullException()
        {
            new EditRobotsModel(null, new List<string>(), new List<CustomAgentModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_EditRobotsModel_InvalidAgentList_Null_Throws_ArgumentNullException()
        {
            SetTestControllerContext();
            new EditRobotsModel(GetModelData(), null, new List<CustomAgentModel>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_EditRobotsModel_InvalidCustomAgentModelList_Null_Throws_ArgumentNullException()
        {
            SetTestControllerContext();
            new EditRobotsModel(GetModelData(), new List<string>(), null);
        }

        [TestMethod]
        public void SpiderControllerModel_Contains_Agents_Success()
        {
            SpiderController sut = CreateSpiderControllerInstance();
            IActionResult response = sut.Index();
            PartialViewResult viewResult = response as PartialViewResult;

            Assert.IsNotNull(viewResult.Model);
            EditRobotsModel model = viewResult.Model as EditRobotsModel;

            Assert.IsNotNull(model);

            Assert.IsTrue(model.Agents.Count > 1);
        }

        [TestMethod]
        public void SpiderControllerModel_CreateAndGetCustomAgentRoutes()
        {
            SpiderController sut = CreateSpiderControllerInstance();

            IActionResult response = sut.Index();
            PartialViewResult viewResult = response as PartialViewResult;

            Assert.IsNotNull(viewResult.Model);
            EditRobotsModel model = viewResult.Model as EditRobotsModel;

            Assert.IsNotNull(model);

            Assert.IsTrue(model.Routes.Count > 1);
            Assert.AreEqual("/Views/Spider/_systemRobots.cshtml", viewResult.ViewName);
        }

        [TestMethod]
        public void Construct_SpiderControllerModel_DefaultConstructor()
        {
            EditRobotsModel sut = new EditRobotsModel();
            Assert.IsNotNull(sut);
            Assert.IsNull(sut.Routes);
            Assert.IsNull(sut.Agents);
        }

        [TestMethod]
        public void SpiderControllerModel_AddCustomRoute_ValidateAttributes()
        {
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(SpiderController), "AddCustomRoute"));
            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(SpiderController), "AddCustomRoute"));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(SpiderController), "AddCustomRoute"));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(SpiderController), "AddCustomRoute"));
        }

        [TestMethod]
        public void SpiderController_AddCustomRoute_NullModel()
        {
            SpiderController sut = CreateSpiderControllerInstance();

            IActionResult response = sut.AddCustomRoute(null);
            PartialViewResult viewResult = response as PartialViewResult;
            Assert.IsTrue(viewResult.ViewData.ModelState.IsValid);
            Assert.IsNotNull(viewResult.Model);
            EditRobotsModel model = viewResult.Model as EditRobotsModel;

            Assert.IsNotNull(model);

            Assert.AreEqual("/Views/Spider/_systemRobots.cshtml", viewResult.ViewName);
        }

        [TestMethod]
        public void SpiderController_AddCustomRoute_Invalid_NullAgentName_ReturnsInvalidModelState()
        {
            SpiderController sut = CreateSpiderControllerInstance();

            IActionResult response = sut.AddCustomRoute(new EditRobotsModel());
            PartialViewResult viewResult = response as PartialViewResult;
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
            Assert.IsTrue(viewResult.ViewData.ModelState.ErrorCount > 0);
            Assert.IsTrue(viewResult.ViewData.ModelState.ContainsKey(nameof(EditRobotsModel.AgentName)));
        }

        [TestMethod]
        public void SpiderController_AddCustomRoute_Invalid_EmptyAgentName_ReturnsInvalidModelState()
        {
            SpiderController sut = CreateSpiderControllerInstance();

            IActionResult response = sut.AddCustomRoute(new EditRobotsModel());
            PartialViewResult viewResult = response as PartialViewResult;
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
            Assert.IsTrue(viewResult.ViewData.ModelState.ErrorCount > 0);
            Assert.IsTrue(viewResult.ViewData.ModelState.ContainsKey(nameof(EditRobotsModel.AgentName)));
        }

        [TestMethod]
        public void SpiderController_AddCustomRoute_Invalid_NullRoute_ReturnsInvalidModelState()
        {
            SpiderController sut = CreateSpiderControllerInstance();
            EditRobotsModel model = new EditRobotsModel()
            {
                AgentName = "MyAgent"
            };

            IActionResult response = sut.AddCustomRoute(model);
            PartialViewResult viewResult = response as PartialViewResult;
            Assert.IsTrue(viewResult.ViewData.ModelState.ContainsKey(nameof(EditRobotsModel.Route)));
            Assert.IsFalse(viewResult.ViewData.ModelState.ContainsKey(nameof(EditRobotsModel.AgentName)));
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
            Assert.IsTrue(viewResult.ViewData.ModelState.ErrorCount > 0);
        }

        [TestMethod]
        public void SpiderController_AddCustomRoute_Invalid_EmptyRoute_ReturnsInvalidModelState()
        {
            SpiderController sut = CreateSpiderControllerInstance();
            EditRobotsModel model = new EditRobotsModel()
            {
                AgentName = "MyAgent"
            };

            IActionResult response = sut.AddCustomRoute(model);
            PartialViewResult viewResult = response as PartialViewResult;
            Assert.IsTrue(viewResult.ViewData.ModelState.ContainsKey(nameof(EditRobotsModel.Route)));
            Assert.IsFalse(viewResult.ViewData.ModelState.ContainsKey(nameof(EditRobotsModel.AgentName)));
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
            Assert.IsTrue(viewResult.ViewData.ModelState.ErrorCount > 0);
        }

        [TestMethod]
        public void SpiderController_AddCustomRoute_Invalid_AlreadyExistsInDeniedList()
        {
            SpiderController sut = CreateSpiderControllerInstance();
            EditRobotsModel model = new EditRobotsModel()
            {
                AgentName = "*",
                Route = "/Home/Error/"
            };

            IActionResult response = sut.AddCustomRoute(model);
            PartialViewResult viewResult = response as PartialViewResult;
            Assert.IsFalse(viewResult.ViewData.ModelState.ContainsKey(nameof(EditRobotsModel.Route)));
            Assert.IsFalse(viewResult.ViewData.ModelState.ContainsKey(nameof(EditRobotsModel.AgentName)));
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
            Assert.IsTrue(viewResult.ViewData.ModelState.ErrorCount == 1);
            Assert.IsTrue(viewResult.ViewData.ModelState.ContainsKey(String.Empty));
            Assert.AreEqual("Route already exists", viewResult.ViewData.ModelState[String.Empty].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void SpiderController_AddCustomRoute_Invalid_AlreadyExistsInCustomList()
        {
            SpiderController sut = CreateSpiderControllerInstance(out IRobots robots);
            robots.AgentAdd("testcustomAgent");
            robots.AddAllowedRoute("testcustomAgent", "/MyRoute/");

            EditRobotsModel model = new EditRobotsModel()
            {
                AgentName = "testCustomAgent",
                Route = "/MyRoute/"
            };

            IActionResult response = sut.AddCustomRoute(model);
            PartialViewResult viewResult = response as PartialViewResult;
            Assert.IsFalse(viewResult.ViewData.ModelState.ContainsKey(nameof(EditRobotsModel.Route)));
            Assert.IsFalse(viewResult.ViewData.ModelState.ContainsKey(nameof(EditRobotsModel.AgentName)));
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
            Assert.IsTrue(viewResult.ViewData.ModelState.ErrorCount == 1);
            Assert.IsTrue(viewResult.ViewData.ModelState.ContainsKey(String.Empty));
            Assert.AreEqual("Custom route already exists", viewResult.ViewData.ModelState[String.Empty].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void SpiderController_AddCustomRoute_ValidDeniedRoute()
        {
            SpiderController sut = CreateSpiderControllerInstance();

            EditRobotsModel model = new EditRobotsModel()
            {
                AgentName = "testCustomAgent",
                Route = "/MyRoute/"
            };

            IActionResult response = sut.AddCustomRoute(model);
            PartialViewResult viewResult = response as PartialViewResult;
            Assert.IsTrue(viewResult.ViewData.ModelState.IsValid);
            Assert.IsTrue(viewResult.ViewData.ModelState.ErrorCount == 0);
            EditRobotsModel returnModel = viewResult.Model as EditRobotsModel;

            Assert.IsNotNull(returnModel);
            Assert.IsTrue(returnModel.Agents.Contains(model.AgentName));

            CustomAgentModel customAgentModel = returnModel.Routes
                .Where(ca =>
                    ca.Agent.Equals(model.AgentName) &&
                    ca.Route.Equals(model.Route) &&
                    ca.IsCustom &&
                    !ca.Allowed)
                .FirstOrDefault();

            Assert.IsNotNull(customAgentModel);
        }

        [TestMethod]
        public void SpiderController_AddCustomRoute_ValidAllowedRoute()
        {
            MockSaveData mockSaveData = new MockSaveData();
            SpiderController sut = CreateSpiderControllerInstance(out IRobots _, mockSaveData);

            EditRobotsModel model = new EditRobotsModel()
            {
                AgentName = "testCustomAgent",
                Route = "/MyAllowedRoute/",
                Allowed = true
            };

            IActionResult response = sut.AddCustomRoute(model);
            PartialViewResult viewResult = response as PartialViewResult;
            Assert.IsTrue(viewResult.ViewData.ModelState.IsValid);
            Assert.IsTrue(viewResult.ViewData.ModelState.ErrorCount == 0);
            EditRobotsModel returnModel = viewResult.Model as EditRobotsModel;

            Assert.IsNotNull(returnModel);
            Assert.IsTrue(returnModel.Agents.Contains(model.AgentName));

            CustomAgentModel customAgentModel = returnModel.Routes
                .Where(ca =>
                    ca.Agent.Equals(model.AgentName) &&
                    ca.Route.Equals(model.Route) &&
                    ca.IsCustom &&
                    ca.Allowed)
                .FirstOrDefault();

            Assert.IsNotNull(customAgentModel);
            Assert.IsTrue(mockSaveData.SaveDataCalled);
        }

        [TestMethod]
        public void SpiderController_AddCustomRoute_ValidateAttributes()
        {
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(SpiderController), "AddCustomRoute"));
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(SpiderController), "AddCustomRoute"));
            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(SpiderController), "AddCustomRoute"));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(SpiderController), "AddCustomRoute"));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(SpiderController), "AddCustomRoute"));
        }

        [TestMethod]
        public void SpiderController_DeleteCustomRoute_Invalid_NullModel()
        {
            SpiderController sut = CreateSpiderControllerInstance();

            IActionResult response = sut.DeleteCustomRoute(null);
            PartialViewResult viewResult = response as PartialViewResult;
            Assert.IsTrue(viewResult.ViewData.ModelState.IsValid);
            Assert.IsNotNull(viewResult.Model);
            EditRobotsModel model = viewResult.Model as EditRobotsModel;

            Assert.IsNotNull(model);

            Assert.AreEqual("/Views/Spider/_systemRobots.cshtml", viewResult.ViewName);
        }

        [TestMethod]
        public void SpiderController_DeleteCustomRoute_Invalid_ModelAgent()
        {
            SpiderController sut = CreateSpiderControllerInstance();

            EditRobotsModel model = new EditRobotsModel();

            IActionResult response = sut.DeleteCustomRoute(model);
            PartialViewResult viewResult = response as PartialViewResult;
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
            Assert.IsNotNull(viewResult.Model);

            EditRobotsModel returnModel = viewResult.Model as EditRobotsModel;

            Assert.IsNotNull(returnModel);

            Assert.IsTrue(viewResult.ViewData.ModelState.ContainsKey(nameof(EditRobotsModel.AgentName)));
            Assert.IsTrue(viewResult.ViewData.ModelState.ErrorCount > 0);
            Assert.AreEqual("Invalid agent name", viewResult.ViewData.ModelState[nameof(EditRobotsModel.AgentName)].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void SpiderController_DeleteCustomRoute_Invalid_ModelRoute()
        {
            SpiderController sut = CreateSpiderControllerInstance();

            EditRobotsModel model = new EditRobotsModel()
            {
                AgentName = "MyCoolAgent"
            };

            IActionResult response = sut.DeleteCustomRoute(model);
            PartialViewResult viewResult = response as PartialViewResult;
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
            Assert.IsNotNull(viewResult.Model);

            EditRobotsModel returnModel = viewResult.Model as EditRobotsModel;

            Assert.IsNotNull(returnModel);

            Assert.IsTrue(viewResult.ViewData.ModelState.ContainsKey(nameof(EditRobotsModel.Route)));
            Assert.IsTrue(viewResult.ViewData.ModelState.ErrorCount > 0);
            Assert.AreEqual("Invalid route", viewResult.ViewData.ModelState[nameof(EditRobotsModel.Route)].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void SpiderController_DeleteCustomRoute_Invalid_CanNotDeleteNonCustomRoute()
        {
            SpiderController sut = CreateSpiderControllerInstance();

            EditRobotsModel model = new EditRobotsModel()
            {
                AgentName = "*",
                Route = "/home/error/"
            };

            IActionResult response = sut.DeleteCustomRoute(model);
            PartialViewResult viewResult = response as PartialViewResult;
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
            Assert.IsNotNull(viewResult.Model);

            EditRobotsModel returnModel = viewResult.Model as EditRobotsModel;

            Assert.IsNotNull(returnModel);

            Assert.IsTrue(viewResult.ViewData.ModelState.ContainsKey(String.Empty));
            Assert.IsTrue(viewResult.ViewData.ModelState.ErrorCount > 0);
            Assert.AreEqual("Unable to delete non custom route", viewResult.ViewData.ModelState[String.Empty].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void SpiderController_DeleteCustomRoute_Invalid_CustomRouteNotFound()
        {
            SpiderController sut = CreateSpiderControllerInstance();

            EditRobotsModel model = new EditRobotsModel()
            {
                AgentName = "myCustomRoute",
                Route = "/home/error/"
            };

            IActionResult response = sut.DeleteCustomRoute(model);
            PartialViewResult viewResult = response as PartialViewResult;
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
            Assert.IsNotNull(viewResult.Model);

            EditRobotsModel returnModel = viewResult.Model as EditRobotsModel;

            Assert.IsNotNull(returnModel);

            Assert.IsTrue(viewResult.ViewData.ModelState.ContainsKey(String.Empty));
            Assert.IsTrue(viewResult.ViewData.ModelState.ErrorCount > 0);
            Assert.AreEqual("Custom route not found", viewResult.ViewData.ModelState[String.Empty].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void SpiderController_DeleteCustomRoute_Valid_CustomRouteDeleted()
        {
            MockSaveData mockSaveData = new MockSaveData();
            SpiderController sut = CreateSpiderControllerInstance(out IRobots robots, mockSaveData);
            robots.AgentAdd("myCustomRoute");
            robots.AddAllowedRoute("myCustomRoute", "/MyCustom/Route/");

            EditRobotsModel model = new EditRobotsModel()
            {
                AgentName = "myCustomRoute",
                Route = "/MyCustom/Route/"
            };

            IActionResult response = sut.DeleteCustomRoute(model);
            PartialViewResult viewResult = response as PartialViewResult;
            Assert.IsTrue(viewResult.ViewData.ModelState.IsValid);
            Assert.IsNotNull(viewResult.Model);

            EditRobotsModel returnModel = viewResult.Model as EditRobotsModel;

            Assert.IsNotNull(returnModel);

            CustomAgentModel customAgentModel = returnModel.Routes
                .Where(r => r.Agent.Equals(model.AgentName) && r.Route.Equals(model.Route))
                .FirstOrDefault();
            Assert.IsNull(customAgentModel);
            Assert.IsTrue(mockSaveData.SaveDataCalled);
        }

        [TestMethod]
        public void SpiderController_DeleteCustomRoute_ValidateAttributes()
        {
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(SpiderController), "DeleteCustomRoute"));
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(SpiderController), "DeleteCustomRoute"));
            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(SpiderController), "DeleteCustomRoute"));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(SpiderController), "DeleteCustomRoute"));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(SpiderController), "DeleteCustomRoute"));
        }

        #region Private Methods

        private SpiderController CreateSpiderControllerInstance(bool createDescriptors = true)
        {
            return CreateSpiderControllerInstance(out IRobots _, new MockSaveData(), createDescriptors);
        }

        private SpiderController CreateSpiderControllerInstance(out IRobots robots)
        {
            return CreateSpiderControllerInstance(out robots, new MockSaveData());
        }

        private SpiderController CreateSpiderControllerInstance(out IRobots robots, MockSaveData mockSaveData, bool createDescriptors = true)
        {
            IPluginTypesService pluginTypesServices = new pm.PluginServices(_testSpiderPlugin) as IPluginTypesService;

            ActionDescriptorCollection actionDescriptorCollection = null;

            if (createDescriptors)
            {
                var descriptors = new List<ActionDescriptor>()
                {
                    new ActionDescriptor()
                    {
                        DisplayName = "LoginPlugin.Controllers.LoginController",
                        AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo()
                        {
                            Template = "Test",
                            Name = "Login"
                        }
                    },
                    new ActionDescriptor()
                    {
                        DisplayName = "AspNetCore.PluginManager.DemoWebsite.Controllers.HomeController.Error",
                        AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo()
                        {
                            Template = "Home",
                            Name = "Error"
                        }
                    },
                    new ActionDescriptor()
                    {
                        DisplayName = "LoginPlugin.Controllers.LoginController.GetCaptchaImage",
                        AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo()
                        {
                            Template = "Login",
                            Name = "Captcha"
                        }
                    }
                };

                actionDescriptorCollection = new ActionDescriptorCollection(descriptors, 1);
            }
            else
            {
                actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            }

            robots = new Robots(new TestActionDescriptorCollectionProvider(actionDescriptorCollection),
                new RouteDataServices(), pluginTypesServices, new MockLoadData());

            SpiderController Result = new SpiderController(robots, mockSaveData);

            Result.ControllerContext = CreateTestControllerContext();

            return Result;
        }

        #endregion Private Methods
    }
}
