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
 *  Product:  SimpleDB
 *  
 *  File: DatabaseTimings.cs
 *
 *  Purpose:  Retrieves database timings
 *
 *  Date        Name                Reason
 *  08/01/2023  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SharedPluginFeatures;

namespace SimpleDB.Internal
{
	internal sealed class DatabaseTimings : IDatabaseTimings
	{
		#region Private Members

		private readonly ISimpleDBManager _simpleDBManager;

		#endregion Private Members

		#region Constructors

		public DatabaseTimings(ISimpleDBManager simpleDBManager)
		{
			_simpleDBManager = simpleDBManager ?? throw new ArgumentNullException(nameof(simpleDBManager));
		}

		#endregion Constructors

		#region IDatabaseTimings Methods

		public Dictionary<string, Dictionary<string, Timings>> GetDatabaseTimings()
		{
			Dictionary<string, Dictionary<string, Timings>> Result = [];

			foreach (KeyValuePair<string, ISimpleDBTable> table in _simpleDBManager.Tables)
			{
				Result.Add(table.Key, table.Value.GetAllTimings);
			}

			return Result;
		}

		#endregion IDatabaseTimings Methods
	}
}
