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
 *  Product:  SharedPluginFeatures
 *  
 *  File: DeniedRoutes.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  20/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
	/// <summary>
	/// The breadcrumb attribute is used by the Breadcrumb.Plugin module to automatically generate breadcrumbs for a route.
	/// 
	/// The controller for the breadcrumb is gotten automatically when breadcrumb plugin generates a list of routes with breadcrumbs.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class BreadcrumbAttribute : Attribute
	{
		#region Constructors

		/// <summary>
		/// Constructor
		/// 
		/// The breadcrumb will appear after the breadcrumb for the parent route.
		/// </summary>
		/// <param name="name">Name of the breadcrumb item.</param>
		/// <param name="parentRoute">Route of the parent item for the route.</param>
		public BreadcrumbAttribute(string name, string parentRoute)
		{
			if (String.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			Name = name;
			ParentRoute = parentRoute;
		}

		/// <summary>
		/// Constructor
		/// 
		/// This breadcrumb has no parent route.
		/// </summary>
		/// <param name="name">Name of the breadcrumb item.</param>
		public BreadcrumbAttribute(string name)
			: this(name, String.Empty)
		{

		}

		/// <summary>
		/// Constructor
		/// 
		/// 
		/// </summary>
		/// <param name="name">Name of the breadcrumb item.</param>
		/// <param name="parentControllerName">Name of the controller which is a parent of this breadcrumb.</param>
		/// <param name="parentActionName">Name of the action which is the parent of this breadcrumb.</param>
		public BreadcrumbAttribute(string name, string parentControllerName, string parentActionName)
			: this(name, String.Empty)
		{
			if (String.IsNullOrEmpty(parentControllerName))
				throw new ArgumentNullException(nameof(parentControllerName));

			if (String.IsNullOrEmpty(parentActionName))
				throw new ArgumentNullException(nameof(parentActionName));

			if (parentControllerName.EndsWith("Controller", StringComparison.InvariantCultureIgnoreCase))
				parentControllerName = parentControllerName[..^10];

			ParentRoute = $"/{parentControllerName}/{parentActionName}";
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Name of breadcrumb.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Parent route for breadcrumb, used when generating a breadcrumb heirarchy.
		/// </summary>
		public string ParentRoute { get; private set; }

		/// <summary>
		/// Indicates that the route contains parameter values, if true then the way the breadcrumb is generated is slightly different.
		/// </summary>
		public bool HasParams { get; set; }

		#endregion Properties
	}
}
