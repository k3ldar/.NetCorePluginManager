﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Product:  Blog Plugin
 *  
 *  File: BlogSettings.cs
 *
 *  Purpose:  Settings
 *
 *  Date        Name                Reason
 *  20/06/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using SharedPluginFeatures;

namespace Blog.Plugin
{
	/// <summary>
	/// Settings that affect how the Blog.Plugin module is configured.
	/// </summary>
	public class BlogSettings : IPluginSettings
	{
		/// <summary>
		/// Settings name
		/// </summary>
		public string SettingsName => nameof(BlogSettings);

		/// <summary>
		/// Determines whether people can leave comments or not.
		/// </summary>
		public bool AllowComments { get; set; }
	}
}
