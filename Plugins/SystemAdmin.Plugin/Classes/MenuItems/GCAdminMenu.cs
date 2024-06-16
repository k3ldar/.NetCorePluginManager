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
 *  Product:  SystemAdmin.Plugin
 *  
 *  File: GCAdminMenu.cs
 *
 *  Purpose:  Displays garbage collection data if monitoring
 *
 *  Date        Name                Reason
 *  07/11/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace SystemAdmin.Plugin.Classes.MenuItems
{
	public class GCAdminMenu : SystemAdminSubMenu
	{
		public override String Action()
		{
			return null;
		}

		public override String Area()
		{
			return null;
		}

		public override String Controller()
		{
			return null;
		}

		public override String Data()
		{
			StringBuilder Result = new("Date/Time|Duration|Memory Reclaimed\r");

			List<GCSnapshot> snapshots = GCAnalysis.RetrieveGCData();

			foreach (GCSnapshot snapshot in snapshots)
			{
				Result.Append($"{snapshot.TimeStarted.ToString(Thread.CurrentThread.CurrentUICulture)}|");
				Result.Append($"{snapshot.TimeTaken.ToString("n3")}ms|");
				Result.Append($"{Shared.Utilities.FileSize(snapshot.MemorySaved, 2)}\r");
			}

			return Result.ToString();
		}

		public override String Image()
		{
			return null;
		}

		public override Enums.SystemAdminMenuType MenuType()
		{
			return Enums.SystemAdminMenuType.Grid;
		}

		public override String Name()
		{
			return "GC Timings";
		}

		public override String ParentMenuName()
		{
			return "System";
		}

		public override Int32 SortOrder()
		{
			return 0;
		}
	}
}

#pragma warning restore CS1591
