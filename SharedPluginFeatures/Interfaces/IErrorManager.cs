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
 *  File: IErrorManager.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  17/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SharedPluginFeatures
{
	/// <summary>
	/// Interface which helps determine how an error is handled within a website.
	/// </summary>
	public interface IErrorManager
	{
		/// <summary>
		/// Indicates that an error has been raised and provides details of the error.
		/// </summary>
		/// <param name="errorInformation">Exception details for the error raised.</param>
		void ErrorRaised(in ErrorInformation errorInformation);

		/// <summary>
		/// Indicates that a page has been requested but not found within the available routes.
		/// 
		/// Applications can replace the page with an existing one which will be used to redirect to instead of showing the user a 404 error page.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="replacePath"></param>
		/// <returns>bool.  True if the route has been replaced, otherwise false.</returns>
		bool MissingPage(in string path, ref string replacePath);
	}
}
