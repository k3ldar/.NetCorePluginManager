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
 *  Copyright (c) 2012 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatues
 *  
 *  File: IRobots.cs
 *
 *  Purpose:  IRobots interface for creating a robots.txt file
 *
 *  Date        Name                Reason
 *  17/10/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;

namespace SharedPluginFeatures
{
	/// <summary>
	/// Interface use to create a robots.txt file
	/// </summary>
	public interface IRobots
	{
		/// <summary>
		/// List of known user agents
		/// </summary>
		/// <value>List&lt;string&gt;</value>
		List<string> Agents { get; }

		/// <summary>
		/// Returns a list of denied routes
		/// </summary>
		/// <value>List&lt;DeniedRoute&gt;</value>
		List<DeniedRoute> DeniedRoutes { get; }

		/// <summary>
		/// Returns a list of user defined routes
		/// </summary>
		/// <value>List&lt;IRobotRouteData&gt;</value>
		List<IRobotRouteData> CustomRoutes { get; }

		/// <summary>
		/// Adds a custom agent to the list of current agents.
		/// </summary>
		/// <param name="agentName">Name of agents</param>
		/// <returns>bool</returns>
		/// <exception cref="ArgumentNullException">Raised if agentName is null or an empty string.</exception>
		bool AgentAdd(string agentName);

		/// <summary>
		/// Removes a custom agent from the list of current agents.
		/// </summary>
		/// <param name="agentName">Name of agents</param>
		/// <returns>bool</returns>
		/// <exception cref="ArgumentNullException">Raised if agentName is null or an empty string.</exception>

		bool AgentRemove(string agentName);

		/// <summary>
		/// Returns all data on allowed and denied routes for an agent
		/// </summary>
		/// <param name="agent"></param>
		/// <returns>List&lt;string&gt;</returns>
		List<string> GetRoutes(string agent);

		/// <summary>
		/// Adds an allowed route to the custom list
		/// </summary>
		/// <param name="agent">Name of agent</param>
		/// <param name="route">Route which will be allowed</param>
		/// <returns>bool</returns>
		/// <exception cref="ArgumentException">Raised if agent does not exists</exception>
		/// <exception cref="ArgumentException">Raised if route is a duplicate entry</exception>
		/// <exception cref="ArgumentNullException">Raised if agent is null or empty</exception>
		/// <exception cref="ArgumentNullException">Raised if route is null or empty</exception>
		bool AddAllowedRoute(string agent, string route);

		/// <summary>
		/// Adds a denied route to the custom list
		/// </summary>
		/// <param name="agent">Name of agent</param>
		/// <param name="route">Route which will be denied</param>
		/// <returns>bool</returns>
		bool AddDeniedRoute(string agent, string route);

		/// <summary>
		/// Removes a previously added allowed or denied route, if found.
		/// </summary>
		/// <param name="agent">Name of agent</param>
		/// <param name="route">Route that will be removed</param>
		/// <returns>bool</returns>
		bool RemoveRoute(string agent, string route);

		/// <summary>
		/// Method for saving custom data
		/// </summary>
		/// <param name="saveData">ISaveData instance</param>
		/// <returns>bool</returns>
		/// <exception cref="ArgumentNullException">Thrown if saveData is null</exception>
		bool SaveData(ISaveData saveData);
	}
}
