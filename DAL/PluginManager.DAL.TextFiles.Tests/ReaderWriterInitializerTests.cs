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
 *  File: ReaderWriterInitializerTests.cs
 *
 *  Purpose:  ReaderWriterInitializerTests tests for text based storage
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
    public class ReaderWriterInitializerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_NullPath_Throws_ArgumentNullException()
        {
            try
            {
                new ReaderWriterInitializer(null);
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
                new ReaderWriterInitializer("");
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
            string directory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                new ReaderWriterInitializer(directory);
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
            string directory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                System.IO.Directory.CreateDirectory(directory);
                ReaderWriterInitializer sut = new ReaderWriterInitializer(directory);
                Assert.AreEqual(1u, sut.MinimumVersion);
                Assert.AreEqual(directory, sut.Path);
            }
            finally
            {
                System.IO.Directory.Delete(directory);
            }
        }
    }
}
