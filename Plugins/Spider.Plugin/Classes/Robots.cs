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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Spider.Plugin.Plugin
 *  
 *  File: InitialiseEvents.cs
 *
 *  Purpose:  Initialisation events
 *
 *  Date        Name                Reason
 *  07/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Mvc.Infrastructure;

using PluginManager.Abstractions;

using SharedPluginFeatures;

#pragma warning disable CS1591, CA1303

namespace Spider.Plugin.Classes
{
    public class Robots : IRobots
    {
        #region Private Members

        private readonly Dictionary<string, List<IRobotRouteData>> _agents;
        private readonly List<DeniedRoute> _deniedSpiderRoutes;
        private List<RobotRouteData> _customRoutes;

        #endregion Private Members

        #region Constructors

        public Robots(IActionDescriptorCollectionProvider routeProvider, IRouteDataService routeDataService,
            IPluginTypesService pluginTypesService, ILoadData loadData)
        {
            if (routeProvider == null)
                throw new ArgumentNullException(nameof(routeProvider));

            if (routeDataService == null)
                throw new ArgumentNullException(nameof(routeDataService));

            if (pluginTypesService == null)
                throw new ArgumentNullException(nameof(pluginTypesService));

            if (loadData == null)
                throw new ArgumentNullException(nameof(loadData));

            _deniedSpiderRoutes = new List<DeniedRoute>();
            _agents = LoadSpiderData(routeProvider, routeDataService, pluginTypesService);
            _customRoutes = new List<RobotRouteData>();

            LoadData(loadData);
        }

        #endregion Constructors

        #region IRobots Properties

        public List<string> Agents
        {
            get
            {
                List<string> Result = new List<string>();

                foreach (KeyValuePair<String, List<IRobotRouteData>> kvp in _agents)
                    Result.Add(kvp.Key);

                return Result;
            }
        }

        public bool AgentAdd(string agentName)
        {
            if (String.IsNullOrEmpty(agentName))
                throw new ArgumentNullException(nameof(agentName));

            if (_agents.ContainsKey(agentName))
                return false;

            _agents.Add(agentName, new List<IRobotRouteData>());
            return true;
        }

        public bool AgentRemove(string agentName)
        {
            if (String.IsNullOrEmpty(agentName))
                throw new ArgumentNullException(nameof(agentName));

            if (!_agents.ContainsKey(agentName))
                return false;

            if (_agents[agentName].Any(item => !item.IsCustom))
                return false;

            return _agents.Remove(agentName);
        }

        public List<DeniedRoute> DeniedRoutes
        {
            get
            {
                return _deniedSpiderRoutes;
            }
        }

        public List<IRobotRouteData> CustomRoutes
        {
            get
            {
                List<IRobotRouteData> Result = new List<IRobotRouteData>();
                _customRoutes.ForEach(cr => Result.Add(cr));

                return Result;
            }
        }

        #endregion IRobots Properties

        #region IRobots Methods

        public List<string> GetRoutes(string agent)
        {
            List<string> Result = new List<string>();

            foreach (IRobotRouteData agentData in _agents[agent])
            {
                if (!String.IsNullOrEmpty(agentData.Comment))
                    Result.Add($"#{agentData.Comment}");

                if (agentData.Allowed)
                    Result.Add($"Allow: {agentData.Route}");
                else
                    Result.Add($"Disallow: {agentData.Route}");
            }

            return Result;
        }

        public bool AddAllowedRoute(string agent, string route)
        {
            if (String.IsNullOrEmpty(agent))
                throw new ArgumentNullException(nameof(agent));

            if (String.IsNullOrEmpty(route))
                throw new ArgumentNullException(nameof(route));

            if (!Agents.Contains(agent))
                throw new ArgumentException("Agent not registered", nameof(agent));

            if (_customRoutes.Any(r => r.Agent.Equals(agent) && r.Route.Equals(route)))
                return false;

            _customRoutes.Add(new RobotRouteData(agent, null, route, true, true));
            AddCustomRoutesToKnownAgents();
            return true;
        }

        public bool AddDeniedRoute(string agent, string route)
        {
            if (String.IsNullOrEmpty(agent))
                throw new ArgumentNullException(nameof(agent));

            if (String.IsNullOrEmpty(route))
                throw new ArgumentNullException(nameof(route));

            if (!Agents.Contains(agent))
                throw new ArgumentException("Agent not registered", nameof(agent));

            if (_customRoutes.Any(r => r.Agent.Equals(agent) && r.Route.Equals(route)))
                return false;

            _customRoutes.Add(new RobotRouteData(agent, null, route, false, true));
            AddCustomRoutesToKnownAgents();
            return true;
        }

        public bool RemoveRoute(string agent, string route)
        {
            if (String.IsNullOrEmpty(agent))
                throw new ArgumentNullException(nameof(agent));

            if (String.IsNullOrEmpty(route))
                throw new ArgumentNullException(nameof(route));

            if (!Agents.Contains(agent))
                throw new ArgumentException("Agent not registered", nameof(agent));

            RobotRouteData customRoute = _customRoutes.FirstOrDefault(r => r.Agent.Equals(agent) && r.Route.Equals(route));

            if (customRoute == null)
                return false;

            _customRoutes.Remove(customRoute);
            AddCustomRoutesToKnownAgents();
            return true;
        }

        public bool SaveData(ISaveData saveData)
        {
            if (saveData == null)
                throw new ArgumentNullException(nameof(saveData));

            return saveData.Save<List<RobotRouteData>>(_customRoutes, "CustomRoutes", "Routes");
        }

        #endregion IRobots Methods

        #region Private Methods

        private void LoadData(ILoadData loadData)
        {
            List<RobotRouteData> customRobots = loadData.Load<List<RobotRouteData>>("CustomRoutes", "Routes");

            if (customRobots != null)
                _customRoutes = customRobots;

            AddCustomRoutesToKnownAgents();
        }

        private void AddCustomRoutesToKnownAgents()
        {
            foreach (string kvp in _agents.Keys)
            {
                for (int i = _agents[kvp].Count - 1; i >= 0; i--)
                {
                    IRobotRouteData item = _agents[kvp][i];

                    if (item.IsCustom)
                        _agents[kvp].RemoveAt(i);
                }
            }

            foreach (IRobotRouteData item in _customRoutes)
            {
                if (!_agents.ContainsKey(item.Agent))
                {
                    _agents.Add(item.Agent, new List<IRobotRouteData>());
                }

                if (_agents[item.Agent].Any(l => l.IsCustom && l.Route.Equals(item.Route)))
                    continue;

                _agents[item.Agent].Add(item);
            }
        }

        private Dictionary<string, List<IRobotRouteData>> LoadSpiderData(IActionDescriptorCollectionProvider routeProvider,
            IRouteDataService routeDataService, IPluginTypesService pluginTypesService)
        {
            List<Type> spiderAttributes = pluginTypesService.GetPluginTypesWithAttribute<DenySpiderAttribute>();

			Dictionary<string, List<IRobotRouteData>> Result = SortAndFilterDenyRoutesByAgent(routeProvider, routeDataService, spiderAttributes);

            string lastAgent = String.Empty;

            foreach (KeyValuePair<String, List<IRobotRouteData>> kvp in Result)
            {
                if (!lastAgent.Equals(kvp.Key))
                {
                    lastAgent = kvp.Key;
                }

                foreach (IRobotRouteData attribute in kvp.Value)
                {
                    _deniedSpiderRoutes.Add(new DeniedRoute($"{attribute.Route.ToLower()}", attribute.Agent));
                }
            }

            return Result;
        }

        private static Dictionary<string, List<IRobotRouteData>> SortAndFilterDenyRoutesByAgent(
            IActionDescriptorCollectionProvider routeProvider,
            IRouteDataService routeDataService,
            List<Type> spiderAttributes)
        {
            Dictionary<string, List<IRobotRouteData>> Result = new Dictionary<string, List<IRobotRouteData>>();

            // Cycle through all classes and methods which have the spider attribute
            foreach (Type type in spiderAttributes)
            {
                object[] attributes = type.GetCustomAttributes(true)
                    .Where(a => a is DenySpiderAttribute).ToArray();

                foreach (DenySpiderAttribute attribute in attributes)
                {
                    attribute.Route = routeDataService.GetRouteFromClass(type, routeProvider);

                    if (String.IsNullOrEmpty(attribute.Route))
                        continue;

                    AddUserAgentToDictionary(attribute, Result);
                }

                // look for specific method disallows

                foreach (MethodInfo method in type.GetMethods())
                {
                    attributes = method.GetCustomAttributes(true)
                        .Where(a => a is DenySpiderAttribute).ToArray();

                    foreach (DenySpiderAttribute attribute in attributes)
                    {
                        attribute.Route = routeDataService.GetRouteFromMethod(method, routeProvider);

                        if (String.IsNullOrEmpty(attribute.Route))
                            continue;

                        AddUserAgentToDictionary(attribute, Result);
                    }
                }
            }

            return Result;
        }

        private static void AddUserAgentToDictionary(DenySpiderAttribute denySpiderAttribute,
            in Dictionary<string, List<IRobotRouteData>> agentList)
        {
            if (!agentList.ContainsKey(denySpiderAttribute.UserAgent))
                agentList.Add(denySpiderAttribute.UserAgent, new List<IRobotRouteData>());

            string route = denySpiderAttribute.Route;

            if (!route.EndsWith("/"))
                route += "/";

            agentList[denySpiderAttribute.UserAgent].Add(
                new RobotRouteData(denySpiderAttribute.UserAgent, denySpiderAttribute.Comment, route, false, false));
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591, CA1303