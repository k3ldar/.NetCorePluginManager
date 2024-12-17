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
 *  Product:  AspNetCore.PluginManager.DemoWebsite
 *  
 *  File: SystemUptimeMenu.cs
 *
 *  Purpose:  Menu to show system uptime
 *
 *  Date        Name                Reason
 *  01/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Text;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace SystemAdmin.Plugin.Classes.MenuItems
{
	public class SystemUptimeMenu : SystemAdminSubMenu
	{
		#region Private Members

		private readonly static DateTime _loadTime = DateTime.UtcNow;

		#endregion Private Members

		#region SystemAdminSubMenu Methods

		public override String Action()
		{
			return String.Empty;
		}

		public override String Area()
		{
			return String.Empty;
		}

		public override String Controller()
		{
			return String.Empty;
		}

		public override String Data()
		{
			TimeSpan span = DateTime.UtcNow - _loadTime;


			StringBuilder Result = new("Status|Value\rLoad Time|", 2048);
			Result.Append(_loadTime.ToString("R"));
			Result.Append('\r');

			Result.Append("Total Time|");

			if (span.Days > 1)
			{
				Result.Append($"{span.Days} days ");
			}
			else if (span.Days > 0)
			{
				Result.Append($"{span.Days} day ");
			}

			if (span.Hours > 1)
			{
				Result.Append($"{span.Hours} hours ");
			}
			else if (span.Hours > 0)
			{
				Result.Append($"{span.Hours} hour ");
			}

			if (span.Minutes > 1)
			{
				Result.Append($"{span.Minutes} minutes ");
			}
			else if (span.Minutes > 0)
			{
				Result.Append($"{span.Minutes} minute ");
			}

			Result.Append($"{span.Seconds} seconds");

			return Result.ToString();
		}

		public override string Image()
		{
			return Constants.SystemImageUptime;
		}

		public override Enums.SystemAdminMenuType MenuType()
		{
			return Enums.SystemAdminMenuType.Grid;
		}

		public override string Name()
		{
			return Languages.LanguageStrings.Uptime;
		}

		public override string ParentMenuName()
		{
			return nameof(Languages.LanguageStrings.System);
		}

		public override int SortOrder()
		{
			return int.MinValue;
		}

		#endregion SystemAdminSubMenu Methods
	}
}

#pragma warning restore CS1591
