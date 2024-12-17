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
 *  Product:  Spider.Plugin
 *  
 *  File: CustomAgentModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  02/11/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;

#pragma warning disable CS1591

namespace Spider.Plugin.Models
{
	public class CustomAgentModel
	{
		#region Constructors

		public CustomAgentModel(string agent, string route, bool allowed, bool isCustom, string comment)
		{
			if (String.IsNullOrEmpty(agent))
				throw new ArgumentNullException(nameof(agent));

			if (String.IsNullOrEmpty(route))
				throw new ArgumentNullException(nameof(route));

			Agent = agent;
			Route = route;
			Allowed = allowed;
			IsCustom = isCustom;
			Comment = comment ?? String.Empty;
		}

		#endregion Constructors

		#region Properties

		public string Agent { get; private set; }

		public string Comment { get; private set; }

		public string Route { get; private set; }

		public bool Allowed { get; private set; }

		public bool IsCustom { get; private set; }

		#endregion Properties
	}
}

#pragma warning restore CS1591