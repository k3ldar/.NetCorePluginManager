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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SimpleDB.Tests
 *  
 *  File: TextTableInitializerTests.cs
 *
 *  Purpose:  TextTableInitializerTests tests for SimpleDB
 *
 *  Date        Name                Reason
 *  30/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SimpleDB.Internal;
using SimpleDB.Tests.Mocks;

#pragma warning disable CA1806

namespace SimpleDB.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TextTableInitializerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_NullPath_Throws_ArgumentNullException()
        {
            try
            {
                new SimpleDBInitializer(path: null);
            }
            catch (ArgumentNullException e)
            {
                Assert.AreEqual("path", e.ParamName);
                throw;
            }
        }
         
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_EmptyStringPath_Throws_ArgumentNullException()
        {
            try
            {
                new SimpleDBInitializer("");
            }
            catch (ArgumentNullException e)
            {
                Assert.AreEqual("path", e.ParamName);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Construct_DirectoryDoesNotExists_Throws_ArgumentException()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                new SimpleDBInitializer(directory);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("path", e.ParamName);
                Assert.AreEqual($"Path does not exist: {directory} (Parameter 'path')", e.Message);
                throw;
            }
        }

        [TestMethod]
        public void Construct_ValidInstance_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                SimpleDBInitializer sut = new SimpleDBInitializer(directory);
                Assert.AreEqual(1u, sut.MinimumVersion);
                Assert.AreEqual(directory, sut.Path);
            }
            finally
            {
                Directory.Delete(directory);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterTable_InvalidParam_Null_Throws_ArgumentNullException()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                SimpleDBInitializer sut = new SimpleDBInitializer(directory);
                Assert.AreEqual(1u, sut.MinimumVersion);
                Assert.AreEqual(directory, sut.Path);

                sut.RegisterTable(null);
            }
            finally
            {
                Directory.Delete(directory);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterTable_TableAlreadyRegistered_Throws_IOException()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                SimpleDBInitializer sut = new SimpleDBInitializer(directory);
                Assert.AreEqual(1u, sut.MinimumVersion);
                Assert.AreEqual(directory, sut.Path);

                using (SimpleDBOperations<MockRow> mockTable = new SimpleDBOperations<MockRow>(sut, new ForeignKeyManager(), new MockPluginClassesService()))
                    sut.RegisterTable(mockTable);
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void RegisterTable_TableRegistered_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                SimpleDBInitializer sut = new SimpleDBInitializer(directory);
                Assert.AreEqual(1u, sut.MinimumVersion);
                Assert.AreEqual(directory, sut.Path);

                using (SimpleDBOperations<MockRow> mockTable = new SimpleDBOperations<MockRow>(sut, new ForeignKeyManager(), new MockPluginClassesService()))
                {
                    IReadOnlyDictionary<string, ISimpleDBTable> tables = sut.Tables;
                    Assert.AreEqual(1, tables.Count);
                    Assert.IsTrue(tables.ContainsKey("MockTable"));
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void RegisterAndUnregisterTable_WithForeignKeyManager_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                SimpleDBInitializer sut = new SimpleDBInitializer(directory);
                Assert.AreEqual(1u, sut.MinimumVersion);
                Assert.AreEqual(directory, sut.Path);
                MockForeignKeyManager foreignKeyManager = new MockForeignKeyManager();
                Assert.AreEqual(0, foreignKeyManager.RegisteredTables.Count);

                using (SimpleDBOperations<MockRow> mockTable = new SimpleDBOperations<MockRow>(sut, foreignKeyManager, new MockPluginClassesService()))
                {
                    Assert.AreEqual(1, foreignKeyManager.RegisteredTables.Count);
                    Assert.IsTrue(foreignKeyManager.RegisteredTables.Contains("MockTable"));

                    IReadOnlyDictionary<string, ISimpleDBTable> tables = sut.Tables;
                    Assert.AreEqual(1, tables.Count);
                    Assert.IsTrue(tables.ContainsKey("MockTable"));
                }

                Assert.AreEqual(0, foreignKeyManager.RegisteredTables.Count);
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }
    }
}

#pragma warning restore CA1806