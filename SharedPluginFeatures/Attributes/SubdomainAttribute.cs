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
 *  Copyright (c) 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatures
 *  
 *  File: SubdomainAttribute.cs
 *
 *  Purpose:  Attribute for Subdomains
 *
 *  Date        Name                Reason
 *  13/02/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

#if NET_CORE_3_1
#pragma warning disable CS1574
#endif

namespace SharedPluginFeatures
{
	/// <summary>
	/// Attribute which provides the name of the configuration setting to be used when
	/// routing requests to a controller through to a subdomain of the main site
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class SubdomainAttribute : Attribute
	{
		/// <summary>
		/// Initialises a new subdomain and applies the configuration name for the controller
		/// </summary>
		/// <param name="configurationName">Name of the configuration for the subdomain.</param>
		/// <exception cref="ArgumentNullException">Thrown if configurationName is null or empty</exception>
		public SubdomainAttribute(string configurationName)
		{
			if (String.IsNullOrEmpty(configurationName))
				throw new ArgumentNullException(nameof(configurationName));

			ConfigurationName = configurationName;
		}

		/// <summary>
		/// Name of the configuration entry which is used to manage the subdomain 
		/// </summary>
		public string ConfigurationName { get; private set; }
	}
}

#if NET_CORE_3_1
#pragma warning restore CS1574
#endif
