﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  File: TableAttributeTests.cs
 *
 *  Purpose:  TableAttributeTests tests for text based storage
 *
 *  Date        Name                Reason
 *  30/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PluginManager.DAL.TextFiles.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TableAttributeTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_TableNameNull_Throws_ArgumentNullException()
        {
            try
            {
                new TableAttribute(null);
            }
            catch (ArgumentNullException e)
            {
                Assert.AreEqual("tableName", e.ParamName);
                Assert.AreEqual("Value cannot be null. (Parameter 'tableName')", e.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_TableNameEmptyString_Throws_ArgumentNullException()
        {
            try
            {
                new TableAttribute("");
            }
            catch (ArgumentNullException e)
            {
                Assert.AreEqual("tableName", e.ParamName);
                Assert.AreEqual("Value cannot be null. (Parameter 'tableName')", e.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Construct_TableNameContainsInvalidCharacters_Throws_ArgumentException()
        {
            try
            {
                new TableAttribute("table< > !");
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("tableName", e.ParamName);
                Assert.AreEqual("Tablename contains invalid character: < (Parameter 'tableName')", e.Message);
                throw;
            }
        }

        [TestMethod]
        public void Construct_ValidInstance_DefaultParams_success()
        {
            TableAttribute sut = new TableAttribute("table");
            Assert.IsNotNull(sut);
            Assert.AreEqual(CompressionType.None, sut.Compression);
            Assert.AreEqual("table", sut.TableName);
        }

        [TestMethod]
        public void Construct_ValidInstance_WithCompression_success()
        {
            TableAttribute sut = new TableAttribute("table", CompressionType.Brotli);
            Assert.IsNotNull(sut);
            Assert.AreEqual(CompressionType.Brotli, sut.Compression);
            Assert.AreEqual("table", sut.TableName);
        }
    }
}