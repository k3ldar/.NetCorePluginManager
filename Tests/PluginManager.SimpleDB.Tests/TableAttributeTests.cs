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
 *  File: TableAttributeTests.cs
 *
 *  Purpose:  TableAttributeTests tests for SimpleDB
 *
 *  Date        Name                Reason
 *  30/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#pragma warning disable CA1806

namespace SimpleDB.Tests
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
            TableAttribute sut = new("table");
            Assert.IsNotNull(sut);
            Assert.AreEqual(CompressionType.None, sut.Compression);
            Assert.AreEqual("table", sut.TableName);
        }

        [TestMethod]
        public void Construct_ValidInstance_WithCompression_success()
        {
            TableAttribute sut = new("table", CompressionType.Brotli);
            Assert.IsNotNull(sut);
            Assert.AreEqual(CompressionType.Brotli, sut.Compression);
            Assert.AreEqual(CachingStrategy.None, sut.CachingStrategy);
            Assert.AreEqual("table", sut.TableName);
        }

        [TestMethod]
        public void Construct_ValidInstance_WithMemoryCachingStrategy_success()
        {
            TableAttribute sut = new("table", CompressionType.Brotli, CachingStrategy.Memory);
            Assert.IsNotNull(sut);
            Assert.AreEqual(CompressionType.Brotli, sut.Compression);
            Assert.AreEqual(CachingStrategy.Memory, sut.CachingStrategy);
            Assert.AreEqual("table", sut.TableName);
        }

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void SlidingMemoryTimeout_Set_CachingStrategyNotSlidingMemory_Throws_InvalidOperationException()
		{
			TableAttribute sut = new("table", CompressionType.Brotli, CachingStrategy.None);
			sut.SlidingMemoryTimeoutMilliseconds = 0;
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void SlidingMemoryTimeout_Set_ValueLessThanZero_Throws_ArgumentOutOfRangeException()
		{
			TableAttribute sut = new("table", CompressionType.Brotli, CachingStrategy.SlidingMemory);
			sut.SlidingMemoryTimeoutMilliseconds = -1;
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void SlidingMemoryTimeout_Set_ValueLessToZero_Throws_ArgumentOutOfRangeException()
		{
			TableAttribute sut = new("table", CompressionType.Brotli, CachingStrategy.SlidingMemory);
			sut.SlidingMemoryTimeoutMilliseconds = 0;
		}

		[TestMethod]
		public void SlidingMemoryTimeout_Set_Success()
		{
			TableAttribute sut = new("table", CompressionType.Brotli, CachingStrategy.SlidingMemory);
			sut.SlidingMemoryTimeoutMilliseconds = 60000;

			Assert.AreEqual(sut.SlidingMemoryTimeout, TimeSpan.FromMinutes(1));
		}
	}
}

#pragma warning restore CA1806