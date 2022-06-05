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
 *  Product:  PluginManager.DAL.TextFiles.Tests
 *  
 *  File: TextTableInitializerTests.cs
 *
 *  Purpose:  TextTableInitializerTests tests for text based storage
 *
 *  Date        Name                Reason
 *  30/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.DAL.TextFiles.Internal;

namespace PluginManager.DAL.TextFiles.Tests
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
                new TextTableInitializer(path: null);
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
                new TextTableInitializer("");
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
                new TextTableInitializer(directory);
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
                TextTableInitializer sut = new TextTableInitializer(directory);
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
                TextTableInitializer sut = new TextTableInitializer(directory);
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
                TextTableInitializer sut = new TextTableInitializer(directory);
                Assert.AreEqual(1u, sut.MinimumVersion);
                Assert.AreEqual(directory, sut.Path);

                using (TextTableOperations<MockRow>? mockTable = new TextTableOperations<MockRow>(sut, new ForeignKeyManager()))
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
                TextTableInitializer sut = new TextTableInitializer(directory);
                Assert.AreEqual(1u, sut.MinimumVersion);
                Assert.AreEqual(directory, sut.Path);

                using (TextTableOperations<MockRow> mockTable = new TextTableOperations<MockRow>(sut, new ForeignKeyManager()))
                {
                    IReadOnlyDictionary<string, ITextTable> tables = sut.Tables;
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
                TextTableInitializer sut = new TextTableInitializer(directory);
                Assert.AreEqual(1u, sut.MinimumVersion);
                Assert.AreEqual(directory, sut.Path);
                MockForeignKeyManager foreignKeyManager = new MockForeignKeyManager();
                Assert.AreEqual(0, foreignKeyManager.RegisteredTables.Count);

                using (TextTableOperations<MockRow> mockTable = new TextTableOperations<MockRow>(sut, foreignKeyManager))
                {
                    Assert.AreEqual(1, foreignKeyManager.RegisteredTables.Count);
                    Assert.IsTrue(foreignKeyManager.RegisteredTables.Contains("MockTable"));

                    IReadOnlyDictionary<string, ITextTable> tables = sut.Tables;
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
