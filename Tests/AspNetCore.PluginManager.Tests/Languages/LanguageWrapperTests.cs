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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: LanguageWrapperTests.cs
 *
 *  Purpose:  Tests for Language files
 *
 *  Date        Name                Reason
 *  14/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNetCore.PluginManager.Tests.Language
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class LanguageWrapperTests
    {
        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void GetInstalledLanguages_InvalidPath_ReturnsNoInstalledLanguages()
        {
            string path = TestHelper.GetTestPath("languages");
            Languages.LanguageWrapper.GetInstalledLanguages(path);
        }

        [TestMethod]
        public void GetInstalledLanguages_NoLanguageFilesFound_ReturnsDefaultCulture_En_GB()
        {
            string path = TestHelper.GetTestPath("languages");
            Directory.CreateDirectory(path);
            try
            {
                string[] languages = Languages.LanguageWrapper.GetInstalledLanguages(path);

                Assert.AreEqual(1, languages.Length);
                Assert.AreEqual("en-GB", languages[0]);
            }
            finally
            {
                Directory.Delete(path, true);
            }
        }

        [TestMethod]
        public void GetInstalledLanguages_NoLanguageFilesFound_ReturnsDefaultCulture_da_DK()
        {
            string path = TestHelper.GetTestPath("languages");
            Directory.CreateDirectory(path);
            try
            {
                string[] languages = Languages.LanguageWrapper.GetInstalledLanguages(path, new CultureInfo("da-DK"));

                Assert.AreEqual(2, languages.Length);
                Assert.AreEqual("da-DK", languages[0]);
            }
            finally
            {
                Directory.Delete(path, true);
            }
        }

        [TestMethod]
        public void GetInstalledLanguages_AllLanguageFilesFound_ReturnsDefaultCulture_da_DK()
        {
            string path = Directory.GetCurrentDirectory();
            string[] languages = Languages.LanguageWrapper.GetInstalledLanguages(path, new CultureInfo("da-DK"));

            Assert.AreEqual(16, languages.Length);
            Assert.AreEqual("da-DK", languages[0]);
            Assert.IsTrue(languages.Where(l => l.Equals("da-DK")).Count() == 1);
            Assert.IsTrue(languages.Where(l => l.Equals("en-GB")).Count() == 1);
        }

        [TestMethod]
        public void GetInstalledLanguages_AllLanguageFilesFound_ReturnsDefaultCulture_en_GB()
        {
            string path = Directory.GetCurrentDirectory();
            string[] languages = Languages.LanguageWrapper.GetInstalledLanguages(path);

            Assert.AreEqual(16, languages.Length);
            Assert.AreEqual("en-GB", languages[0]);
            Assert.IsTrue(languages.Where(l => l.Equals("da-DK")).Count() == 1);
            Assert.IsTrue(languages.Where(l => l.Equals("en-GB")).Count() == 1);
        }
    }
}
