/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: ImageFileTests.cs
 *
 *  Purpose:  Tests for ImageFile class
 *
 *  Date        Name                Reason
 *  19/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Images;

namespace AspNetCore.PluginManager.Tests.Middleware
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ImageFileTests
    {
        private const string TestCategoryName = "Middleware";
        private const string ForwardSlash = "/";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamUri_Null_Throws_ArgumentNullException()
        {
            new ImageFile(null, "file.txt");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamFileName_Null_Throws_ArgumentNullException()
        {
            new ImageFile(new Uri(ForwardSlash, UriKind.RelativeOrAbsolute), null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamFileName_EmptyString_Throws_ArgumentNullException()
        {
            new ImageFile(new Uri(ForwardSlash, UriKind.RelativeOrAbsolute), "");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Construct_InvalidParamFileName_DoesNotExist_Throws_ArgumentException()
        {
            string tmpFile = Path.GetTempFileName();

            if (File.Exists(tmpFile))
                File.Delete(tmpFile);

            new ImageFile(new Uri(ForwardSlash, UriKind.RelativeOrAbsolute), tmpFile);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_ExistingFile_Success()
        {
            string tmpFile = Path.GetTempFileName();
            File.WriteAllText(tmpFile, "Temp file for unit testing purposes");


            ImageFile sut = new ImageFile(new Uri($"/{Path.GetFileName(tmpFile)}", UriKind.RelativeOrAbsolute), tmpFile);

            if (File.Exists(tmpFile))
                File.Delete(tmpFile);

            Assert.AreEqual($"/{Path.GetFileName(tmpFile)}", sut.ImageUri.ToString());
            Assert.AreEqual(".tmp", sut.FileExtension);
            Assert.AreEqual(Path.GetFileName(tmpFile), sut.Name);
            Assert.AreEqual(35, sut.Size);
            Assert.IsTrue(sut.CreateDate > DateTime.MinValue);
            Assert.IsTrue(sut.CreateDate <= DateTime.Now);
            Assert.IsTrue(sut.ModifiedDate > DateTime.MinValue);
            Assert.IsTrue(sut.ModifiedDate <= DateTime.Now);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_NonFileBased_InvalidParamUri_Null_Throws_ArgumentNullException()
        {
            new ImageFile(null, "file.txt", ".txt", 23, DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_NonFileBased_InvalidParamFileName_Null_Throws_ArgumentNullException()
        {
            new ImageFile(new Uri(ForwardSlash, UriKind.RelativeOrAbsolute), null, ".txt", 23, DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_NonFileBased_InvalidParamFileName_EmptyString_Throws_ArgumentNullException()
        {
            new ImageFile(new Uri(ForwardSlash, UriKind.RelativeOrAbsolute), "", ".txt", 23, DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_NonFileBased_InvalidParamFileExtension_Null_Throws_ArgumentNullException()
        {
            new ImageFile(new Uri(ForwardSlash, UriKind.RelativeOrAbsolute), "testfile.gif", null, 23, DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_NonFileBased_InvalidParamFileExtension_EmptyString_Throws_ArgumentNullException()
        {
            new ImageFile(new Uri(ForwardSlash, UriKind.RelativeOrAbsolute), "testfile.gif", "", 23, DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Construct_NonFileBased_InvalidParamFileExtension_EmptyString_Throws_ArgumentOutOfRangeExceptionn()
        {
            new ImageFile(new Uri(ForwardSlash, UriKind.RelativeOrAbsolute), "testfile.gif", ".gif", -1, DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_NonFileBased_ValidInstance_ExistingFile_Success()
        {
            DateTime fileDate = DateTime.Now.AddMinutes(-5);
            string tmpFile = Path.GetTempFileName();
            File.WriteAllText(tmpFile, "Temp file for unit testing purposes");


            ImageFile sut = new ImageFile(new Uri("/testfile.gif", UriKind.RelativeOrAbsolute), "testfile.gif", ".gif", 23, fileDate, fileDate);

            Assert.AreEqual("/testfile.gif", sut.ImageUri.ToString());
            Assert.AreEqual(".gif", sut.FileExtension);
            Assert.AreEqual("testfile.gif", sut.Name);
            Assert.AreEqual(23, sut.Size);
            Assert.AreEqual(fileDate, sut.CreateDate);
            Assert.AreEqual(fileDate, sut.ModifiedDate);
        }
    }
}
