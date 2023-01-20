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
 *  Product:  SimpleDB.Tests
 *  
 *  File: DatabaseTimingTests.cs
 *
 *  Purpose:  Database timing tests for SimpleDB
 *
 *  Date        Name                Reason
 *  08/01/2023  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shared.Classes;

using SimpleDB.Internal;
using SimpleDB.Tests.Mocks;

namespace SimpleDB.Tests
{
	[TestClass]
	public sealed class DatabaseTimingTests
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_NullParam_Throws_ArgumentNullException()
		{
			new DatabaseTimings(null);
		}

		[TestMethod]
		public void RetrieveTimings_FromAllRegisteredTables_Success()
		{
			ThreadManager.Initialise();
			string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
			try
			{
				Directory.CreateDirectory(directory);
				SimpleDBManager simpleDbManager = new SimpleDBManager(directory);
				DatabaseTimings sut = new(simpleDbManager);
				Assert.AreEqual(1u, simpleDbManager.MinimumVersion);
				Assert.AreEqual(directory, simpleDbManager.Path);
				MockForeignKeyManager foreignKeyManager = new MockForeignKeyManager();
				Assert.AreEqual(0, foreignKeyManager.RegisteredTables.Count);

				using (SimpleDBOperations<MockRowSlidingMemory> mockTable = new SimpleDBOperations<MockRowSlidingMemory>(simpleDbManager, foreignKeyManager))
				{
					simpleDbManager.Initialize(new MockPluginClassesService());
					Assert.AreEqual(1, foreignKeyManager.RegisteredTables.Count);
					Assert.IsTrue(foreignKeyManager.RegisteredTables.Contains("MockTableSlidingMemory"));

					IReadOnlyDictionary<string, ISimpleDBTable> tables = simpleDbManager.Tables;
					Assert.AreEqual(1, tables.Count);
					Assert.IsTrue(tables.ContainsKey("MockTableSlidingMemory"));

					for (int i = 0; i < 100; i++)
					{
						mockTable.Insert(new MockRowSlidingMemory() { RowId = i });
					}

					mockTable.Delete(mockTable.Select(23));
					mockTable.Truncate();

					Dictionary<string, Dictionary<string, SharedPluginFeatures.Timings>> timings = sut.GetDatabaseTimings();

					Assert.AreEqual(1, timings.Count);
					Assert.AreEqual(12, timings["MockTableSlidingMemory"].Count);
					Assert.AreEqual(100u, timings["MockTableSlidingMemory"]["TimingsInsert"].Requests);
					Assert.IsTrue(timings["MockTableSlidingMemory"]["TimingsInsert"].Average > 0);
					Assert.AreEqual(1u, timings["MockTableSlidingMemory"]["TimingsDelete"].Requests);
					Assert.AreEqual(1u, timings["MockTableSlidingMemory"]["TimingsTruncate"].Requests);
				}

				Assert.AreEqual(0, foreignKeyManager.RegisteredTables.Count);
			}
			finally
			{
				Directory.Delete(directory, true);
				ThreadManager.Finalise();
			}
		}
	}
}
