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
 *  File: ICultureProvider.cs
 *
 *  Purpose:  Provides interface for Managing cultures
 *
 *  Date        Name                Reason
 *  14/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Globalization;

namespace SharedPluginFeatures
{
	/// <summary>
	/// This interface is implemented by the Localization.Plugin module and can be used to 
	/// determine which cultures are currently supported.  An instance of this interface is
	/// available via the DI container.
	/// </summary>
	public interface ICultureProvider
	{
		/// <summary>
		/// Determines whether a specific culture is valid and implemented by the localization
		/// plugin module.
		/// </summary>
		/// <param name="cultureInfo">CultureInfo instance being checked.</param>
		/// <returns>bool</returns>
		bool IsCultureValid(in CultureInfo cultureInfo);

		/// <summary>
		/// Retrieves a list of available culture codes within the Localization.Plugin module.
		/// </summary>
		/// <returns>string[]</returns>
		string[] AvailableCultures();
	}
}
