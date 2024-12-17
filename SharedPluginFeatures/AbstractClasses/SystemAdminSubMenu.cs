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
 *  Product:  SharedPluginFeatures
 *  
 *  File: SystemAdminSubMenu.cs
 *
 *  Purpose:  System Admin Plugin Sub Main Menu
 *
 *  Date        Name                Reason
 *  24/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

#pragma warning disable IDE0041 // Use 'is null' check

namespace SharedPluginFeatures
{
	/// <summary>
	/// Abstract class that plugin modules can implement in order to add menu items the the SystemAdmin.Plugin module.
	/// 
	/// The SystemAdmin.Plugin module is used to display statistical information in many forms as provided by AspNetCore.PluginManager and any plugins which wish to provide instant statistics for site owners.
	/// 
	/// Typical useage is to display timing statistics by implementing Timings class, through to showing custom data or even data shown within a map, as UserSessionMiddleware.Plugin does.
	/// </summary>
	public abstract class SystemAdminSubMenu : BaseCoreClass, IComparable<SystemAdminSubMenu>
	{
		#region Public Abstract Methods

		/// <summary>
		/// Area in which the implementation exists, or empty string if not contained within an area.
		/// 
		/// This value will be used to form part of the Url when the menu is clicked.
		/// </summary>
		/// <returns>string.  area name or empty string.</returns>
		public abstract string Area();

		/// <summary>
		/// Name of the controller that implements the menu.
		/// 
		/// This value will be used to form part of the Url when the menu is clicked.
		/// </summary>
		/// <returns>string.  Controller name.</returns>
		public abstract string Controller();

		/// <summary>
		/// Name of the action that will be called when the user clicks the menu item.
		/// </summary>
		/// <returns>string.  Name of the action.</returns>
		public abstract string Action();

		/// <summary>
		/// Name of the menu, this can also be the name of a Localized string.
		/// </summary>
		/// <returns>string.  Display name for menu item.</returns>
		public abstract string Name();

		/// <summary>
		/// Sort order for menu item.
		/// </summary>
		/// <returns>int.</returns>
		public abstract int SortOrder();

		/// <summary>
		/// Type of menu item being displayed, this is used internally to display the Data provided.
		/// </summary>
		/// <returns>SystemAdminMenuType.  </returns>
		public abstract Enums.SystemAdminMenuType MenuType();

		/// <summary>
		/// Image url to be displayed with the menu item, a default image is provided if the image does not exist.
		/// </summary>
		/// <returns>string.  Url of image or empty string.</returns>
		public abstract string Image();

		/// <summary>
		/// The data to be shown when the menu item is clicked.
		/// </summary>
		/// <returns>string</returns>
		public abstract string Data();

		/// <summary>
		/// Name of the parent view where the menu item will be displayed.
		/// </summary>
		/// <returns>string</returns>
		public abstract string ParentMenuName();

		#endregion Public Abstract Methods

		#region Public Virtual Methods

		/// <summary>
		/// Back color that will be displayed for the menu item.
		/// </summary>
		/// <returns>string</returns>
		public virtual string BackColor()
		{
			return "#707B7C";
		}

		/// <summary>
		/// Forecolor that will be displayed for the menu item.
		/// </summary>
		/// <returns>string</returns>
		public virtual string ForeColor()
		{
			return "white";
		}

		/// <summary>
		/// Determines whether the sub menu is enabled or not, if disabled it will not be shown
		/// </summary>
		/// <returns>bool</returns>
		public virtual bool Enabled()
		{
			return true;
		}

		#endregion Public Virtual Methods

		#region IComparable Methods

		/// <summary>
		/// IComparable implementation used to sort menu items by SortOrder then Name
		/// </summary>
		/// <param name="compareTo"></param>
		/// <returns></returns>
		public int CompareTo(SystemAdminSubMenu compareTo)
		{
			if (compareTo == null)
				return 1;

			int Result = SortOrder().CompareTo(compareTo.SortOrder());

			if (Result == 0)
				return String.Compare(Name(), compareTo.Name(), StringComparison.InvariantCultureIgnoreCase);

			return Result;
		}

		/// <summary>
		/// Compares instances for equality
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return obj is SystemAdminSubMenu menu &&
				   UniqueId == menu.UniqueId &&
				   EqualityComparer<SystemAdminMainMenu>.Default.Equals(ParentMenu, menu.ParentMenu);
		}

		/// <summary>
		/// Retrieves the hashcode
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return HashCode.Combine(UniqueId, ParentMenu);
		}

		#endregion IComparable

		#region Overrides

		/// <summary>
		/// Implement equality check
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(SystemAdminSubMenu left, SystemAdminSubMenu right)
		{
			if (ReferenceEquals(left, null))
			{
				return ReferenceEquals(right, null);
			}

			return left.Equals(right);
		}

		/// <summary>
		/// Implement not equal to
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator !=(SystemAdminSubMenu left, SystemAdminSubMenu right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Implement less than
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator <(SystemAdminSubMenu left, SystemAdminSubMenu right)
		{
			return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
		}

		/// <summary>
		/// Implement less than or equal to
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator <=(SystemAdminSubMenu left, SystemAdminSubMenu right)
		{
			return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
		}

		/// <summary>
		/// Implement greater than
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator >(SystemAdminSubMenu left, SystemAdminSubMenu right)
		{
			return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
		}

		/// <summary>
		/// Implement greater than or equals
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator >=(SystemAdminSubMenu left, SystemAdminSubMenu right)
		{
			return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
		}

		#endregion Overrides 

		#region Properties

		/// <summary>
		/// Unique id of the menu item.  This is assigned by the SystemAdmin.Plugin module.
		/// </summary>
		public int UniqueId { get; set; }

		/// <summary>
		/// Parent menu implementation.  This is assigned by the SystemAdmin.Plugin module.
		/// </summary>
		public SystemAdminMainMenu ParentMenu { get; set; }

		#endregion Properties
	}
}

#pragma warning restore IDE0041 // Use 'is null' check
