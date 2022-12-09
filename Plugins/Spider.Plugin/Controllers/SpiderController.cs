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
 *  Product:  Spider.Plugin
 *  
 *  File: SpiderController.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  17/10/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SharedPluginFeatures;

using Spider.Plugin.Models;

#pragma warning disable CS1591

namespace Spider.Plugin.Controllers
{
    [RestrictedIpRoute("SystemAdminRoute")]
    [Authorize(Policy = SharedPluginFeatures.Constants.PolicyNameStaff)]
    [DenySpider]
    public class SpiderController : BaseController
    {
        #region Private Members

        private readonly IRobots _robots;
        private readonly ISaveData _saveData;

        #endregion Private Members

        #region Constructors

        public SpiderController(IRobots robots, ISaveData saveData)
        {
            _robots = robots ?? throw new ArgumentNullException(nameof(robots));
            _saveData = saveData ?? throw new ArgumentNullException(nameof(saveData));
        }

        #endregion Constructors

        #region Constants

        public const string Name = "Spider";

        #endregion Constants

        #region Controller Action Methods

        [HttpGet]
        [LoggedIn]
        public IActionResult Index()
        {
            return CreateDefaultPartialView();
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult AddCustomRoute(EditRobotsModel model)
        {
            if (model == null)
                return CreateDefaultPartialView();

            if (String.IsNullOrEmpty(model.AgentName))
                ModelState.AddModelError(nameof(EditRobotsModel.AgentName), "Invalid agent name");

            if (String.IsNullOrEmpty(model.Route))
                ModelState.AddModelError(nameof(EditRobotsModel.Route), "Invalid Route");

            if (_robots.DeniedRoutes.Any(dr => dr.UserAgent.Equals(model.AgentName, StringComparison.InvariantCultureIgnoreCase) && 
				dr.Route.Equals(model.Route, StringComparison.InvariantCultureIgnoreCase)))
            {
                ModelState.AddModelError(String.Empty, "Route already exists");
            }

            if (_robots.CustomRoutes.Any(cr => cr.Agent.Equals(model.AgentName, StringComparison.InvariantCultureIgnoreCase) && 
				cr.Route.Equals(model.Route, StringComparison.InvariantCultureIgnoreCase)))
            {
                ModelState.AddModelError(String.Empty, "Custom route already exists");
            }

            if (!ModelState.IsValid)
                return CreateDefaultPartialView();

            if (!_robots.Agents.Any(a => a.Equals(model.AgentName, StringComparison.InvariantCultureIgnoreCase)))
            {
                _robots.AgentAdd(model.AgentName);
            }

            if (model.Allowed)
            {
                _robots.AddAllowedRoute(model.AgentName, model.Route);
            }
            else
            {
                _robots.AddDeniedRoute(model.AgentName, model.Route);
            }

            _robots.SaveData(_saveData);

            return CreateDefaultPartialView();
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult DeleteCustomRoute(EditRobotsModel model)
        {
            if (model == null)
                return CreateDefaultPartialView();

            if (String.IsNullOrEmpty(model.AgentName))
                ModelState.AddModelError(nameof(model.AgentName), "Invalid agent name");

            if (String.IsNullOrEmpty(model.Route))
                ModelState.AddModelError(nameof(model.Route), "Invalid route");

            DeniedRoute deniedRoute = _robots.DeniedRoutes
				.FirstOrDefault(cr => cr.UserAgent.Equals(model.AgentName, StringComparison.InvariantCultureIgnoreCase) && 
				cr.Route.Equals(model.Route, StringComparison.InvariantCultureIgnoreCase));

            if (deniedRoute != null)
                ModelState.AddModelError(String.Empty, "Unable to delete non custom route");

            if (!ModelState.IsValid)
                return CreateDefaultPartialView();

            IRobotRouteData customRoute = _robots.CustomRoutes.FirstOrDefault(cr =>
                cr.Agent.Equals(model.AgentName, StringComparison.InvariantCultureIgnoreCase) &&
                cr.Route.Equals(model.Route, StringComparison.InvariantCultureIgnoreCase) &&
                cr.IsCustom);

            if (customRoute == null)
                ModelState.AddModelError(String.Empty, "Custom route not found");
			else
				_robots.RemoveRoute(customRoute.Agent, customRoute.Route);

			if (!ModelState.IsValid)
                return CreateDefaultPartialView();

            _robots.SaveData(_saveData);

            return CreateDefaultPartialView();
        }

        #endregion Controller Action Methods

        #region Private Methods

        private PartialViewResult CreateDefaultPartialView()
        {
            return PartialView("/Views/Spider/_systemRobots.cshtml", CreateRobotsModel());
        }

        private EditRobotsModel CreateRobotsModel()
        {
            List<CustomAgentModel> customAgents = new List<CustomAgentModel>();

            foreach (DeniedRoute deniedRoute in _robots.DeniedRoutes)
            {
                customAgents.Add(new CustomAgentModel(deniedRoute.UserAgent, deniedRoute.Route, false, false, String.Empty));
            }

            foreach (IRobotRouteData customRoute in _robots.CustomRoutes)
            {
                customAgents.Add(new CustomAgentModel(customRoute.Agent, customRoute.Route, customRoute.Allowed, true, customRoute.Comment));
            }

            return new EditRobotsModel(GetModelData(), _robots.Agents, customAgents);
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591