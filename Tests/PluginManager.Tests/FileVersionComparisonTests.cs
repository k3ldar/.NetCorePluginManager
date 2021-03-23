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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.Tests
 *  
 *  File: FileVersionComparisonTests.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  21/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Internal;
using PluginManager.Tests.Mocks;

namespace PluginManager.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class FileVersionComparisonTests
    {
        [TestMethod]
        public void Compare_NullComparisonObjects_Returns_Zero()
        {
            FileVersionComparison sut = new FileVersionComparison();

            int result = sut.Compare(null, null);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Equals_NullComparisonObjects_Returns_Zero()
        {
            FileVersionComparison sut = new FileVersionComparison();

            bool result = sut.Equals(null, null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Newer_ValidComparison_Returns_True()
        {
            FileVersionComparison sut = new FileVersionComparison();

            FileInfo fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            bool result = sut.Equals(fileInfo, fileInfo);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Newer_NullComparisonObjects_Returns_True()
        {
            FileVersionComparison sut = new FileVersionComparison();

            bool result = sut.Newer(null, null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Newer_ValidComparison_Returns_False()
        {
            FileVersionComparison sut = new FileVersionComparison();

            FileInfo fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            bool result = sut.Newer(fileInfo, fileInfo);

            Assert.IsFalse(result);
        }
    }
}
