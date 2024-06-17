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
 *  File: LoggedInAttribute.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  08/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
	/// <summary>
	/// This attribute is used in conjunction with the UserSessionMiddleware.Plugin module and indicates that a user
	/// must be logged in when attempting to gain access to the route.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public sealed class LoggedInAttribute : Attribute
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// 
		/// If used the login page is set to the default value of /Login/
		/// </summary>
		public LoggedInAttribute()
		{
			LoginPage = "/Login/";
		}

		/// <summary>
		/// Constructor
		/// 
		/// Allows the route to specify a route to the login page that will be used.
		/// </summary>
		/// <param name="loginPage">string.  Url of login page.</param>
		public LoggedInAttribute(in string loginPage)
		{
			if (String.IsNullOrEmpty(loginPage))
				throw new ArgumentNullException(nameof(loginPage));

			LoginPage = loginPage;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// The url of the page the user will be redirected to, in order to login and gain access to the route where the attribute was applied.
		/// </summary>
		public string LoginPage { get; private set; }

		#endregion Properties
	}
}
