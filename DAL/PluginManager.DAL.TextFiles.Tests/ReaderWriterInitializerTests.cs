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
