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
 *  File: IApplicationSettingsProvider.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  02/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SharedPluginFeatures
{
	/// <summary>
	/// Application settings provider interface
	/// </summary>
    public interface IApplicationSettingsProvider
    {
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T">Type of setting value</typeparam>
		/// <param name="name">Name of setting</param>
		/// <param name="value">Setting value</param>
		void UpdateSetting<T>(string name, T value);

		/// <summary>
		/// Retrieves a setting value of type T
		/// </summary>
		/// <typeparam name="T">Type of setting</typeparam>
		/// <param name="name">Name of setting</param>
		/// <returns>T</returns>
		T RetrieveSetting<T>(string name);

		/// <summary>
		/// Retrieves a setting value
		/// </summary>
		/// <param name="name">Name of setting</param>
		/// <returns>string</returns>
		string RetrieveSetting(string name);

		/// <summary>
		/// Retrieves a setting
		/// </summary>
		/// <param name="name">Name of setting</param>
		/// <param name="defaultValue">Default value if setting does not exist</param>
		/// <returns>string</returns>
		string RetrieveSetting(string name, string defaultValue);

		/// <summary>
		/// Deletes a setting if it exists
		/// </summary>
		/// <param name="name">Name of setting</param>
        void DeleteSetting(string name);
    }
}
