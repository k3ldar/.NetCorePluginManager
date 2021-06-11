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
 *  Copyright (c) 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: ProcessImagesViewModelTests.cs
 *
 *  Purpose:  Unit tests for Process Images View Model
 *
 *  Date        Name                Reason
 *  21/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AspNetCore.PluginManager.Tests.Shared;

using ImageManager.Plugin.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Images;

namespace AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ProcessImagesViewModelTests : GenericBaseClass
    {
        private const string ImageManagerTestsCategory = "Image Manager Tests";

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParameter_FileUploadId_Null_Throws_ArgumentNullException()
        {
            ProcessImagesViewModel sut = new ProcessImagesViewModel(GenerateTestBaseModelData(), false, "My Group", "subgroup", 
                null, new Dictionary<string, List<string>>(), new List<ImageFile>(), null);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParameter_FileUploadId_EmptyString_Throws_ArgumentNullException()
        {
            ProcessImagesViewModel sut = new ProcessImagesViewModel(GenerateTestBaseModelData(), false, "My Group", "subgroup", 
                null, new Dictionary<string, List<string>>(), new List<ImageFile>(), "");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidParameters_Success()
        {
            ProcessImagesViewModel sut = new ProcessImagesViewModel(GenerateTestBaseModelData(), false, "My Group", String.Empty, null,
                new Dictionary<string, List<string>>(), new List<ImageFile>(), "file cache name");
            Assert.IsNotNull(sut);
            Assert.AreEqual("file cache name", sut.FileUploadId);
            Assert.IsNull(sut.SubgroupName);
            Assert.IsFalse(sut.ShowSubgroup);
            Assert.IsNull(sut.AdditionalData);
            Assert.IsFalse(sut.AdditionalDataMandatory);
            Assert.IsNull(sut.AdditionalDataName);
        }
    }
}
