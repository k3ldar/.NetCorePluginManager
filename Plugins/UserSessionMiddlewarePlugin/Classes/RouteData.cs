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
 *  Product:  UserSessionMiddleware.Plugin
 *  
 *  File: RouteData.cs
 *
 *  Purpose:  Route data for logged in/out
 *
 *  Date        Name                Reason
 *  08/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

#pragma warning disable IDE0011

namespace UserSessionMiddleware.Plugin
{
	internal class RouteData
	{
		#region Constructors

		internal RouteData(in string route, in bool loggedIn, in string redirectPath)
		{
			if (String.IsNullOrEmpty(route))
				throw new ArgumentNullException(nameof(route));

			if (String.IsNullOrEmpty(redirectPath))
				throw new ArgumentNullException(nameof(redirectPath));

			Route = route;
			LoggedIn = loggedIn;
			RedirectPath = redirectPath;
			Ignore = false;
		}

		internal RouteData(in string route)
		{
			if (String.IsNullOrEmpty(route))
				throw new ArgumentNullException(nameof(route));

			Route = route;

			Ignore = true;
		}

		#endregion Constructors

		#region Internal Methods

		internal bool Matches(in string route)
		{
			if (String.IsNullOrEmpty(route))
				throw new ArgumentNullException(nameof(route));

			if (Route.Equals(route, StringComparison.InvariantCultureIgnoreCase))
				return true;

			if (route.EndsWith('/') && Route.Equals(route[..^1]))
				return true;

			return false;
		}

		#endregion Internal Methods

		#region Properties

		internal string Route { get; private set; }

		internal bool LoggedIn { get; private set; }

		internal string RedirectPath { get; private set; }

		internal bool Ignore { get; private set; }

		#endregion Properties
	}
}

#pragma warning restore IDE0011