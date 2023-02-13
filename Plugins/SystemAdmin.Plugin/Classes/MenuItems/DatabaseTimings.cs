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
 *  Product:  AspNetCore.PluginManager.DemoWebsite
 *  
 *  File: DatabaseTimings.cs
 *
 *  Purpose:  Displays all timings for a database
 *
 *  Date        Name                Reason
 *  08/01/2023  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PluginManager.Abstractions;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace SystemAdmin.Plugin.Classes.MenuItems
{
	public sealed class DatabaseTimings : SystemAdminSubMenu
	{
		#region Private Members

		private const int MinimumResetMilliseconds = 1500;

		private DateTime _lastRun = DateTime.MinValue;
		private string _lastData;

		private readonly IDatabaseTimings _databaseTimings;

		#endregion Private Members

		#region Constructors

		public DatabaseTimings(IDatabaseTimings databaseTimings)
		{
			_databaseTimings = databaseTimings ?? throw new ArgumentNullException(nameof(databaseTimings));
		}

		public DatabaseTimings()
		{ 
			_databaseTimings = null;
		}

		#endregion Constructors

		#region SystemAdminSubMenu Methods

		public override bool Enabled()
		{
			return _databaseTimings != null;
		}

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
			if (_databaseTimings == null)
				return String.Empty;

			TimeSpan span = DateTime.Now - _lastRun;

			if (span.TotalMilliseconds > MinimumResetMilliseconds)
			{
				_lastRun = DateTime.Now;

				StringBuilder Result = new StringBuilder("Operation|Total Requests|Fastest|Slowest|Average|Trimmed Avg ms|Total ms\r");
				bool isFirst = true;

				foreach (KeyValuePair<string, Dictionary<string, Timings>> item in _databaseTimings.GetDatabaseTimings())
				{
					StringBuilder tableData = new StringBuilder(2048);

					if (isFirst)
					{
						isFirst = false;
					}
					else
					{
						tableData.Append("||||||\r");
					}

					tableData.Append($"{item.Key}||||||\r");

					foreach (KeyValuePair<string, Timings> table in item.Value)
					{
						tableData.Append(table.Key);
						tableData.Append("|");
						tableData.Append($"{table.Value.Requests}");
						tableData.Append("|");
						tableData.Append($"{table.Value.Fastest}");
						tableData.Append("|");
						tableData.Append($"{table.Value.Slowest}");
						tableData.Append("|");
						tableData.Append($"{table.Value.Average}");
						tableData.Append("|");
						tableData.Append($"{table.Value.TrimmedAverage}");
						tableData.Append("|");
						tableData.Append($"{table.Value.Total}");
						tableData.Append("\r");
					}

					Result.Append(tableData);
				}

				_lastData = Result.ToString().Trim(); 
			}

			return _lastData;
		}

		public override string Image()
		{
			return Constants.SystemImageStopWatch;
		}

		public override Enums.SystemAdminMenuType MenuType()
		{
			return Enums.SystemAdminMenuType.Grid;
		}

		public override string Name()
		{
			return nameof(Languages.LanguageStrings.DatabaseTimings);
		}

		public override string ParentMenuName()
		{
			return nameof(Languages.LanguageStrings.Database);
		}

		public override int SortOrder()
		{
			return int.MinValue;
		}

		#endregion SystemAdminSubMenu Methods
	}
}

#pragma warning restore CS1591