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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockDatabaseTimings.cs
 *
 *  Purpose:  Mock IDatabaseTimings class
 *
 *  Date        Name                Reason
 *  08/01/2023  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Shared
{
	public sealed class MockDatabaseTimings : IDatabaseTimings
	{
		public Dictionary<string, Dictionary<string, Timings>> GetDatabaseTimings()
		{
			Dictionary<string, Dictionary<string, Timings>> Result = new();

			Result["Table 1"] = new();
			Result["Table 1"].Add("Op1", new());
			Result["Table 1"].Add("Op2", new());
			Result["Table 1"].Add("Op3", new());
			Result["Table 1"].Add("Op4", new());

			Result["Table 2"] = new();
			Result["Table 2"].Add("Op1", new());
			Result["Table 2"].Add("Op2", new());
			Result["Table 2"].Add("Op3", new());
			Result["Table 2"].Add("Op4", new());

			return Result;
		}
	}
}
