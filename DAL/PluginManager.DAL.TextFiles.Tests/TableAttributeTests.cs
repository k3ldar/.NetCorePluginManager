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
