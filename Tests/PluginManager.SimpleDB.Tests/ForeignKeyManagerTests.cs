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
 *  File: ForeignKeyManagerTests.cs
 *
 *  Purpose:  ForeignKeyManagerTests tests for SimpleDB
 *
 *  Date        Name                Reason
 *  02/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using io = System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using AspNetCore.PluginManager.Tests.Shared;
using SimpleDB.Tests.Mocks;
using SimpleDB.Internal;

namespace SimpleDB.Tests
{
	[TestClass]
    [ExcludeFromCodeCoverage]
    public class ForeignKeyManagerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterTable_InvalidParamTable_Null_Throws_ArgumentNullException()
        {
            ForeignKeyManager sut = new ForeignKeyManager();
            sut.RegisterTable(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnregisterTable_InvalidParamTable_Null_Throws_ArgumentNullException()
        {
            ForeignKeyManager sut = new ForeignKeyManager();
            sut.UnregisterTable(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddRelationShip_InvalidParamSourceTable_Null_Throws_ArgumentNullException()
        {
            ForeignKeyManager sut = new ForeignKeyManager();
            sut.AddRelationShip(null, "targetTable", "Id", "Id");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddRelationShip_InvalidParamTargetTable_Null_Throws_ArgumentNullException()
        {
            ForeignKeyManager sut = new ForeignKeyManager();
            sut.AddRelationShip("sourceTable", null, "Id", "Id");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddRelationShip_InvalidParamPropertyName_Null_Throws_ArgumentNullException()
        {
            ForeignKeyManager sut = new ForeignKeyManager();
            sut.AddRelationShip("sourceTable", "targetTable", null, "Id");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddRelationShip_InvalidParamTargetPropertyName_Null_Throws_ArgumentNullException()
        {
            ForeignKeyManager sut = new ForeignKeyManager();
            sut.AddRelationShip("sourceTable", "targetTable", "Id", "");
        }

        [TestMethod]
        [ExpectedException(typeof(ForeignKeyException))]
        public void ForeignKey_InsertRecordWhenKeyDoesNotExists_Throws_ForeignKeyException()
        {
            ForeignKeyManager sut = new ForeignKeyManager();
            string directory = TestHelper.GetTestPath();
            try
            {
                io.Directory.CreateDirectory(directory);
                ISimpleDBManager simpleDBManager = new SimpleDBManager(directory);

                using SimpleDBOperations<MockTableUserRow> mockUsers = new SimpleDBOperations<MockTableUserRow>(simpleDBManager, sut);
				simpleDBManager.Initialize(new MockPluginClassesService());
                List<MockTableUserRow> testData = new List<MockTableUserRow>();

                for (int i = 0; i < 5; i++)
                    testData.Add(new MockTableUserRow(i));

                mockUsers.Insert(testData);

                using SimpleDBOperations<MockTableAddressRow> mockAddresses = new SimpleDBOperations<MockTableAddressRow>(simpleDBManager, sut);
				simpleDBManager.Initialize(new MockPluginClassesService());
				mockAddresses.Insert(new MockTableAddressRow(10));
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ForeignKey_InsertRecordWhenForeignKeyDoesNotExists_DefaultValueAllowed_DoesNotThrowException()
        {
            ForeignKeyManager sut = new ForeignKeyManager();
			Assert.IsNotNull(sut);
			string directory = TestHelper.GetTestPath();
            try
            {
                io.Directory.CreateDirectory(directory);
                ISimpleDBManager simpleDBManager = new SimpleDBManager(directory);

                using SimpleDBOperations<MockTableUserRow> mockUsers = new SimpleDBOperations<MockTableUserRow>(simpleDBManager, sut);
                mockUsers.ResetSequence(10, 10);
                List<MockTableUserRow> testData = new List<MockTableUserRow>();

                for (int i = 1; i < 6; i++)
                    testData.Add(new MockTableUserRow(i));

                mockUsers.Insert(testData);

                using SimpleDBOperations<MockTableForeignKeyDefaultAllowed> mockAddresses = new SimpleDBOperations<MockTableForeignKeyDefaultAllowed>(simpleDBManager, sut);
                mockAddresses.Insert(new MockTableForeignKeyDefaultAllowed(0));
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ForeignKeyException))]
        public void ForeignKey_UpdateRecordWhenKeyDoesNotExists_Throws_ForeignKeyException()
        {
            ForeignKeyManager sut = new ForeignKeyManager();
            string directory = TestHelper.GetTestPath();
            try
            {
                io.Directory.CreateDirectory(directory);
                ISimpleDBManager simpleDBManager = new SimpleDBManager(directory);

                using SimpleDBOperations<MockTableUserRow> mockUsers = new SimpleDBOperations<MockTableUserRow>(simpleDBManager, sut);
                List<MockTableUserRow> testData = new List<MockTableUserRow>();

                for (int i = 0; i < 5; i++)
                    testData.Add(new MockTableUserRow(i));

                mockUsers.Insert(testData);

                using SimpleDBOperations<MockTableAddressRow> mockAddresses = new SimpleDBOperations<MockTableAddressRow>(simpleDBManager, sut);
                mockAddresses.Insert(new MockTableAddressRow(3));

                MockTableAddressRow addressRow = mockAddresses.Select(0);
                Assert.IsNotNull(addressRow);

                addressRow.UserId = 10;
                mockAddresses.Update(addressRow);
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ForeignKeyException))]
        public void ForeignKey_DeleteForeignKeyWhenForeignKeyIsInUse_Throws_ForeignKeyException()
        {
            ForeignKeyManager sut = new ForeignKeyManager();
            string directory = TestHelper.GetTestPath();
            try
            {
                io.Directory.CreateDirectory(directory);
                ISimpleDBManager simpleDBManager = new SimpleDBManager(directory);

                using SimpleDBOperations<MockTableUserRow> mockUsers = new SimpleDBOperations<MockTableUserRow>(simpleDBManager, sut);
                List<MockTableUserRow> testData = new List<MockTableUserRow>();

                for (int i = 0; i < 5; i++)
                    testData.Add(new MockTableUserRow(i));

                mockUsers.Insert(testData);

                using SimpleDBOperations<MockTableAddressRow> mockAddresses = new SimpleDBOperations<MockTableAddressRow>(simpleDBManager, sut);
                mockAddresses.Insert(new MockTableAddressRow(3));

                MockTableAddressRow addressRow = mockAddresses.Select(0);
                Assert.IsNotNull(addressRow);

                mockUsers.Truncate();
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ForeignKey_InsertRecordWhenKeyExists_Success()
        {
            ForeignKeyManager sut = new ForeignKeyManager();
			Assert.IsNotNull(sut);
			string directory = TestHelper.GetTestPath();
            try
            {
                io.Directory.CreateDirectory(directory);
                ISimpleDBManager simpleDBManager = new SimpleDBManager(directory);

                using SimpleDBOperations<MockTableUserRow> mockUsers = new SimpleDBOperations<MockTableUserRow>(simpleDBManager, sut);
                List<MockTableUserRow> testData = new List<MockTableUserRow>();

                for (int i = 0; i < 5; i++)
                    testData.Add(new MockTableUserRow(i));

                mockUsers.Insert(testData);

                using SimpleDBOperations<MockTableAddressRow> mockAddresses = new SimpleDBOperations<MockTableAddressRow>(simpleDBManager, sut);
                mockAddresses.Insert(new MockTableAddressRow(3));
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ForeignKey_UpdateRecordWhenKeyExists_Success()
        {
            ForeignKeyManager sut = new ForeignKeyManager();
            string directory = TestHelper.GetTestPath();
            try
            {
                io.Directory.CreateDirectory(directory);
                ISimpleDBManager simpleDBManager = new SimpleDBManager(directory);

                using SimpleDBOperations<MockTableUserRow> mockUsers = new SimpleDBOperations<MockTableUserRow>(simpleDBManager, sut);
                List<MockTableUserRow> testData = new List<MockTableUserRow>();

                for (int i = 0; i < 5; i++)
                    testData.Add(new MockTableUserRow(i));

                mockUsers.Insert(testData);

                using SimpleDBOperations<MockTableAddressRow> mockAddresses = new SimpleDBOperations<MockTableAddressRow>(simpleDBManager, sut);
                mockAddresses.Insert(new MockTableAddressRow(3));

                MockTableAddressRow addressRow = mockAddresses.Select(0);
                Assert.IsNotNull(addressRow);

                addressRow.UserId = 2;
                mockAddresses.Update(addressRow);
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ForeignKey_DeleteRecordWhenKeyExists_Success()
        {
            ForeignKeyManager sut = new ForeignKeyManager();
            string directory = TestHelper.GetTestPath();
            try
            {
                io.Directory.CreateDirectory(directory);
                ISimpleDBManager simpleDBManager = new SimpleDBManager(directory);

                using SimpleDBOperations<MockTableUserRow> mockUsers = new SimpleDBOperations<MockTableUserRow>(simpleDBManager, sut);
                List<MockTableUserRow> testData = new List<MockTableUserRow>();

                for (int i = 0; i < 5; i++)
                    testData.Add(new MockTableUserRow(i));

                mockUsers.Insert(testData);

                using SimpleDBOperations<MockTableAddressRow> mockAddresses = new SimpleDBOperations<MockTableAddressRow>(simpleDBManager, sut);
                mockAddresses.Insert(new MockTableAddressRow(4));
                mockAddresses.Truncate();

                Assert.AreEqual(0, mockAddresses.RecordCount);
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }
    }
}
