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
 *  Product:  SharedPluginFeatures
 *  
 *  File: Breadcrumb.cs
 *
 *  Purpose:  Contains breadcrumb data for a route
 *
 *  Date        Name                Reason
 *  20/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
	/// <summary>
	/// This class is used to contain basic breadcrumb data.
	/// </summary>
	public sealed class Breadcrumb
	{
		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">Name of the breadcrumb.</param>
		/// <param name="route">Route that the breadcrumb is aligned to.</param>
		public Breadcrumb(in string name, in string route)
		{
			if (String.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			if (String.IsNullOrEmpty(route))
				throw new ArgumentNullException(nameof(route));

			Name = name;
			Route = route;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Name of the breadcrumb.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Route that the breadcrumb is aligned to.
		/// </summary>
		public string Route { get; private set; }

		#endregion Properties
	}
}
