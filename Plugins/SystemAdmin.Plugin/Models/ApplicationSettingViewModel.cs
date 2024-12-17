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
 *  Product:  SystemAdmin.Plugin
 *  
 *  File: ApplicationSettingViewModel.cs
 *
 *  Purpose:  View model for individual settings
 *
 *  Date        Name                Reason
 *  20/12/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
namespace SystemAdmin.Plugin.Models
{
	/// <summary>
	/// View model for individual application settings
	/// </summary>
	public sealed class ApplicationSettingViewModel
	{
		/// <summary>
		/// Name of setting
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Type of setting, string, int, bool etc
		/// </summary>
		public string DataType { get; set; }

		/// <summary>
		/// Current value of setting
		/// </summary>
		public string Value { get; set; }
	}
}
