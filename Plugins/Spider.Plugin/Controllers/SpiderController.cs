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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SharedPluginFeatures;

using Spider.Plugin.Models;

#pragma warning disable CS1591

namespace Spider.Plugin.Controllers
{
    [LoggedIn]
    [RestrictedIpRoute("SystemAdminRoute")]
    [Authorize(Policy = SharedPluginFeatures.Constants.PolicyNameStaff)]
    [DenySpider]
    public class SpiderController : BaseController
    {
        #region Private Members

        private readonly IRobots _robots;

        #endregion Private Members

        #region Constructors

        public SpiderController(IRobots robots)
        {
            _robots = robots ?? throw new ArgumentNullException(nameof(robots));
        }

        #endregion Constructors

        #region Constants

        public const string Name = "Spider";

        #endregion Constants

        #region Controller Action Methods

        [HttpGet]
        public IActionResult Index()
        {
            return CreateDefaultPartialView();
        }

        [HttpPost]
        public IActionResult AddCustomRoute(EditRobotsModel model)
        {
            if (model == null)
                return CreateDefaultPartialView();

            if (String.IsNullOrEmpty(model.AgentName))
                ModelState.AddModelError(nameof(EditRobotsModel.AgentName), "Invalid agent name");

            if (String.IsNullOrEmpty(model.Route))
                ModelState.AddModelError(nameof(EditRobotsModel.Route), "Invalid Route");


            if (!ModelState.IsValid)
                return CreateDefaultPartialView();

            throw new NotImplementedException();
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
                customAgents.Add(new CustomAgentModel(deniedRoute.UserAgent, deniedRoute.Route, true, String.Empty));
            }

            return new EditRobotsModel(GetModelData(), _robots.Agents, customAgents);
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591