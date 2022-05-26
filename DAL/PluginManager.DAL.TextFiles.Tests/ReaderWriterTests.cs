using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.DAL.TextFiles.Interfaces;
using PluginManager.DAL.TextFiles.Internal;

using io = System.IO;

namespace PluginManager.DAL.TextFiles.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ReaderWriterTests
    {
        [TestMethod]
        public void Construct_ValidInstance_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ReaderWriterInitializer initializer = new ReaderWriterInitializer(directory);

                using TextReaderWriter<MockTable> sut = new TextReaderWriter<MockTable>(initializer);
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Initialize_InvalidParam_Null_Throws_ArgumentNullException()
        {
            using TextReaderWriter<MockTable> sut = new TextReaderWriter<MockTable>(null);
        }

        [TestMethod]
        public void Initialize_TableCanNotBeDeletedOrWrittenWhilstOpen()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ReaderWriterInitializer initializer = new ReaderWriterInitializer(directory);

                using (TextReaderWriter<MockTable> sut = new TextReaderWriter<MockTable>(initializer))
                {
                    Assert.IsTrue(io.File.Exists(io.Path.Combine(directory, "MockTable.dat")));

                    try
                    {
                        io.File.Delete(io.Path.Combine(directory, "MockTable.dat"));
                    }
                    catch (System.IO.IOException err)
                    {
                        Assert.AreEqual($"The process cannot access the file '{directory}\\MockTable.dat' because it is being used by another process.", err.Message);
                    }
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Initialize_TableFileNotFound_CreatesNewTableFile_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ReaderWriterInitializer initializer = new ReaderWriterInitializer(directory);

                using TextReaderWriter<MockTable> sut = new TextReaderWriter<MockTable>(initializer);

                Assert.IsTrue(io.File.Exists(io.Path.Combine(directory, "MockTable.dat")));
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SaveAllRecords_InvalidParamNull_Throws_ArgumentNullException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ReaderWriterInitializer initializer = new ReaderWriterInitializer(directory);

                using TextReaderWriter<MockTable> sut = new TextReaderWriter<MockTable>(initializer);

                sut.Save(null);
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void SaveAllRecords_ObjectAlreadyDisposed_Throws_ArgumentNullException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ReaderWriterInitializer initializer = new ReaderWriterInitializer(directory);

                using TextReaderWriter<MockTable> sut = new TextReaderWriter<MockTable>(initializer);
                sut.Dispose();
                sut.Save(new List<MockTable>());

            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void SaveAllRecords_Writes15168Records_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                IReaderWriterInitializer initializer = new ReaderWriterInitializer(directory);

                using (TextReaderWriter<MockTable> sut = new TextReaderWriter<MockTable>(initializer))
                {
                    List<MockTable> testData = new List<MockTable>();

                    for (int i = 0; i < 15168; i++)
                        testData.Add(new MockTable(i));

                    sut.Save(testData);
                }

                using (TextReaderWriter<MockTable> readSut = new TextReaderWriter<MockTable>(initializer))
                {
                    Assert.AreEqual(15168, readSut.RecordCount);
                    Assert.AreEqual(186075, readSut.DataLength);
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void SaveAllRecords_Writes15168Records_Compressed_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                IReaderWriterInitializer initializer = new ReaderWriterInitializer(directory);

                using (TextReaderWriter<MockTableCompressed> sut = new TextReaderWriter<MockTableCompressed>(initializer))
                {
                    List<MockTableCompressed> testData = new List<MockTableCompressed>();

                    for (int i = 0; i < 15168; i++)
                        testData.Add(new MockTableCompressed(i));

                    sut.Save(testData);
                }

                using (TextReaderWriter<MockTable> readSut = new TextReaderWriter<MockTable>(initializer))
                {
                    Assert.AreEqual(15168, readSut.RecordCount);
                    Assert.AreEqual(11623, readSut.DataLength);
                    List<MockTableCompressed> testData = readSut.Read();
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void NextSequence_ObjectAlreadyDisposed_Throws_ArgumentNullException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ReaderWriterInitializer initializer = new ReaderWriterInitializer(directory);

                using TextReaderWriter<MockTable> sut = new TextReaderWriter<MockTable>(initializer);
                sut.Dispose();
                _ = sut.NextSequence();

            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void NextSequence_IncrementsAndIsSavedStraightAwayToFile_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                IReaderWriterInitializer initializer = new ReaderWriterInitializer(directory);

                for (int i = 0; i < 100; i++)
                {
                    using (TextReaderWriter<MockTableCompressed> sut = new TextReaderWriter<MockTableCompressed>(initializer))
                    {
                        long nextSequence = sut.NextSequence();
                        Assert.AreEqual(i, (long)nextSequence);
                    }
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void ResetSequence_ObjectAlreadyDisposed_Throws_ArgumentNullException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ReaderWriterInitializer initializer = new ReaderWriterInitializer(directory);

                using TextReaderWriter<MockTable> sut = new TextReaderWriter<MockTable>(initializer);
                sut.Dispose();
                sut.ResetSequence(123);

            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ResetSequence_SetsCorrectValue_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                IReaderWriterInitializer initializer = new ReaderWriterInitializer(directory);

                using (TextReaderWriter<MockTableCompressed> sut = new TextReaderWriter<MockTableCompressed>(initializer))
                {
                    sut.ResetSequence(368745);
                }

                using (TextReaderWriter<MockTableCompressed> sut = new TextReaderWriter<MockTableCompressed>(initializer))
                {
                    long nextSequence = sut.NextSequence();
                    Assert.AreEqual(368746, (long)nextSequence);
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }
    }
}