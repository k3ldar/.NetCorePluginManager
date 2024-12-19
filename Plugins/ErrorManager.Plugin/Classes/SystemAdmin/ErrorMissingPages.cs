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
 *  Product:  ErrorManager.Plugin
 *  
 *  File: ErrorMissingPages.cs
 *
 *  Purpose:  Shows a list of missing pages in system admin console
 *
 *  Date        Name                Reason
 *  29/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Text;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace ErrorManager.Plugin.Classes.SystemAdmin
{
	/// <summary>
	/// Returns a list of pages that are missing (404) within ErrorManager.Plugin.  
	/// 
	/// This class descends from SystemAdminSubMenu.
	/// </summary>
	public class ErrorMissingPages : SystemAdminSubMenu
	{
		public override string Action()
		{
			return String.Empty;
		}

		public override string Area()
		{
			return String.Empty;
		}

		public override string Controller()
		{
			return String.Empty;
		}

		/// <summary>
		/// Returns all missing pages processed by ErrorManager.Plugin.
		/// </summary>
		/// <returns>string</returns>
		public override string Data()
		{
			StringBuilder Result = new("Page|Count");

			Dictionary<string, uint> missingPages = ErrorManagerMiddleware.GetMissingPages();

			foreach (KeyValuePair<string, uint> item in missingPages)
			{
				Result.Append($"\r{item.Key}|{item.Value}");
			}

			return Result.ToString();
		}

		public override string Image()
		{
			return String.Empty;
		}

		public override Enums.SystemAdminMenuType MenuType()
		{
			return Enums.SystemAdminMenuType.Grid;
		}

		public override string Name()
		{
			return "Missing Links";
		}

		public override string ParentMenuName()
		{
			return "Errors";
		}

		public override int SortOrder()
		{
			return 0;
		}
	}
}

#pragma warning restore CS1591
