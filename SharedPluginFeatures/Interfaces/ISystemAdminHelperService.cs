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
 *  Product:  SharedPluginFeatures
 *  
 *  File: ISystemAdminHelperService.cs
 *
 *  Purpose:  System Admin Helper Classes
 *
 *  Date        Name                Reason
 *  2/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

namespace SharedPluginFeatures
{
	/// <summary>
	/// This class is implemented internallyby the SystemAdmin.Plugin and is used to manage
	/// System Admin Menu Items.  An instance of this interface is not available for general
	/// use by other modules.
	/// </summary>
	public interface ISystemAdminHelperService
	{
		/// <summary>
		/// Retrieves a list of Main menu items.
		/// </summary>
		/// <returns>List&lt;SystemAdminMainMenu&gt;</returns>
		List<SystemAdminMainMenu> GetSystemAdminMainMenu();

		/// <summary>
		/// Retrieves a specific menu item by Id
		/// </summary>
		/// <param name="id">Id of menu item.</param>
		/// <returns>SystemAdminMainMenu</returns>
		SystemAdminMainMenu GetSystemAdminMainMenu(in int id);

		/// <summary>
		/// Retrieves the Default main menu.
		/// </summary>
		/// <returns>SystemAdminMainMenu</returns>
		SystemAdminMainMenu GetSystemAdminDefaultMainMenu();

		/// <summary>
		/// Retrieves all sub menu items.
		/// </summary>
		/// <returns>List&lt;SystemAdminSubMenu&gt;</returns>
		List<SystemAdminSubMenu> GetSubMenuItems();

		/// <summary>
		/// Retrieves all sub menus for a specific menu item.
		/// </summary>
		/// <param name="mainMenuName">Name of menu item.</param>
		/// <returns>List&lt;SystemAdminSubMenu&gt;</returns>
		List<SystemAdminSubMenu> GetSubMenuItems(in string mainMenuName);

		/// <summary>
		/// Retrieves a specific sub menu.
		/// </summary>
		/// <param name="id">Id of submenu</param>
		/// <returns>SystemAdminSubMenu</returns>
		SystemAdminSubMenu GetSubMenuItem(in int id);
	}
}
