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
 *  File: ForeignKeyManagerTests.cs
 *
 *  Purpose:  ForeignKeyManagerTests tests for text based storage
 *
 *  Date        Name                Reason
 *  02/06/2022  Simon Carter        Initially Created
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
    public class ForeignKeyManagerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterTable_TIsNotOfTypeTextReaderWriter_Throws_ArgumentException()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UnRegisterTable_TIsNotOfTypeTextReaderWriter_Throws_ArgumentException()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void RegisterTable_TIsTypeTextReaderWriter_Success()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void UnRegisterTable_TIsTypeTextReaderWriter_Success()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        [ExpectedException(typeof(ForeignKeyException))]
        public void ForeignKey_InsertRecordWhenKeyDoesNotExists_Throws_ForeignKeyException()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        [ExpectedException(typeof(ForeignKeyException))]
        public void ForeignKey_UpdateRecordWhenKeyDoesNotExists_Throws_ForeignKeyException()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        [ExpectedException(typeof(ForeignKeyException))]
        public void ForeignKey_DeleteWhenForeignKeyExists_Throws_ForeignKeyException()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void ForeignKey_InsertRecordWhenKeyExists_Success()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void ForeignKey_UpdateRecordWhenKeyExists_Success()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void ForeignKey_DeleteRecordWhenKeyExists_Success()
        {
            Assert.IsTrue(false);
        }
    }
}
