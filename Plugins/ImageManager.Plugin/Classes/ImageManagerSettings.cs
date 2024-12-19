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
 *  Copyright (c) 2012 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  ImageManager Plugin
 *  
 *  File: ImageManagerSettings.cs
 *
 *  Purpose:  Image manager settings
 *
 *  Date        Name                Reason
 *  18/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AppSettings;

using SharedPluginFeatures;

namespace ImageManager.Plugin.Classes
{
	/// <summary>
	/// Settings for controlling image manager
	/// </summary>
	public sealed class ImageManagerSettings : IPluginSettings
	{
		/// <summary>
		/// Settings name
		/// </summary>
		public string SettingsName => nameof(ImageManager);

		/// <summary>
		/// Root path of all images
		/// </summary>
		/// <value>string</value>
		[SettingOptional]
		[SettingValidPath]
		public string ImagePath { get; set; }
	}
}
