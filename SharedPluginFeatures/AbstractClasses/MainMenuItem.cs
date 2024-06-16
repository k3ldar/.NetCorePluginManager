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
 *  File: MainMenuItem.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  29/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
namespace SharedPluginFeatures
{
	/// <summary>
	/// Abstract class used to provide easy access to menu items for display within a view.
	/// </summary>
	public abstract class MainMenuItem
	{
		/// <summary>
		/// Name of the area to be used when building a route
		/// </summary>
		/// <returns></returns>
		public abstract string Area();

		/// <summary>
		/// Name of the controller to be used when building a route.
		/// </summary>
		/// <returns>string.  Controller name, e.g. Helpdesk</returns>
		public abstract string Controller();

		/// <summary>
		/// Name of the action within the controller to be used when building a route.
		/// </summary>
		/// <returns>string.  Name of the action, e.g. Index</returns>
		public abstract string Action();

		/// <summary>
		/// Name to be displayed when the menu is shown.
		/// </summary>
		/// <returns>string.  Name of menu item</returns>
		public abstract string Name();

		/// <summary>
		/// Int depicting the order for which the menu item will be sorted in comparison to other menu items.
		/// </summary>
		/// <returns>int. depicting sort order.</returns>
		public abstract int SortOrder();
	}
}
