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
 *  File: ImageTemplateEditorModelTests.cs
 *
 *  Purpose:  Tests for image template model
 *
 *  Date        Name                Reason
 *  13/06/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests.Mocks;

using DynamicContent.Plugin.Model;

using ImageManager.Plugin.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ImageTemplateEditorModelTests
    {
        private const string TestCategoryName = "Dynamic Content";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ImageProvider_Null_Throws_ArgumentNullException()
        {
            new ImageTemplateEditorModel(null, "");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_InvalidParamData_Null_ConvertsToEmptyString_Success()
        {
            ImageTemplateEditorModel sut = new ImageTemplateEditorModel(MockImageProvider.CreateDefaultMockImageProvider(), null);
            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Data);
            Assert.AreEqual("", sut.Data);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_ExistingGroupNotFound_Success()
        {
            ImageTemplateEditorModel sut = new ImageTemplateEditorModel(
                MockImageProvider.CreateDefaultMockImageProviderWithSubgroupAndImage(), "/images/Group1/MyFile.gif");

            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Data);
            Assert.AreEqual("/images/Group1/MyFile.gif", sut.Data);
            Assert.IsNotNull(sut.Groups);
            Assert.AreEqual(2, sut.Groups.Length);
            Assert.AreEqual("Products", sut.Groups[0]);
            Assert.AreEqual("General", sut.Groups[1]);
            Assert.AreEqual("Group1", sut.ActiveGroup);
            Assert.IsNull(sut.ActiveSubgroup);
            Assert.AreEqual("MyFile.gif", sut.ActiveFile);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_ExistingGroupFound_ReturnsValidSubgroups_Success()
        {
            ImageTemplateEditorModel sut = new ImageTemplateEditorModel(
                MockImageProvider.CreateDefaultMockImageProviderWithSubgroupAndImage(), "/images/Products/MyFile.gif");

            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Data);
            Assert.AreEqual("/images/Products/MyFile.gif", sut.Data);
            Assert.IsNotNull(sut.Groups);
            Assert.AreEqual(2, sut.Groups.Length);
            Assert.AreEqual("Products", sut.Groups[0]);
            Assert.AreEqual("General", sut.Groups[1]);
            Assert.AreEqual("Products", sut.ActiveGroup);
            Assert.IsNull(sut.ActiveSubgroup);
            Assert.AreEqual("MyFile.gif", sut.ActiveFile);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_ExistingGroupAndSubgroupFound_ReturnsValidSubgroups_Success()
        {
            ImageTemplateEditorModel sut = new ImageTemplateEditorModel(
                MockImageProvider.CreateDefaultMockImageProviderWithSubgroupAndImage(), "/images/Products/C230/MyFile.gif");

            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Data);
            Assert.AreEqual("/images/Products/C230/MyFile.gif", sut.Data);
            Assert.IsNotNull(sut.Groups);
            Assert.AreEqual(2, sut.Groups.Length);
            Assert.AreEqual("Products", sut.Groups[0]);
            Assert.AreEqual("General", sut.Groups[1]);
            Assert.AreEqual("Products", sut.ActiveGroup);
            Assert.AreEqual("C230", sut.ActiveSubgroup);
            Assert.AreEqual("MyFile.gif", sut.ActiveFile);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_RequiredValuesAreNeverNull_WithSubgroup_Success()
        {
            ImageTemplateEditorModel sut = new ImageTemplateEditorModel(
                MockImageProvider.CreateDefaultMockImageProviderWithSubgroupAndImage(), "/images/Products/C230/MyFile.gif");

            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Groups);
            Assert.IsNotNull(sut.Subgroups);
            Assert.IsNotNull(sut.Images);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_RequiredValuesAreNeverNull_WithoutSubgroups_Success()
        {
            ImageTemplateEditorModel sut = new ImageTemplateEditorModel(
                MockImageProvider.CreateDefaultMockImageProviderWithSubgroupAndImage(), "/images/Products/MyFile.gif");

            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Groups);
            Assert.IsNotNull(sut.Subgroups);
            Assert.IsNotNull(sut.Images);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_RequiredValuesAreNeverNull_WithInvalidData_Success()
        {
            ImageTemplateEditorModel sut = new ImageTemplateEditorModel(
                MockImageProvider.CreateDefaultMockImageProviderWithSubgroupAndImage(), "blah");

            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Groups);
            Assert.IsNotNull(sut.Subgroups);
            Assert.IsNotNull(sut.Images);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            ImageTemplateEditorModel sut = new ImageTemplateEditorModel(
                MockImageProvider.CreateDefaultMockImageProviderWithSubgroupAndImage(), "my data");
            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Data);
            Assert.AreEqual("my data", sut.Data);
            Assert.IsNotNull(sut.Groups);
            Assert.AreEqual(2, sut.Groups.Length);
            Assert.AreEqual("Products", sut.Groups[0]);
            Assert.AreEqual("General", sut.Groups[1]);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_SelectsCorrectGroup_SubGroupAndImage_ValidInstance_Success()
        {
            ImageTemplateEditorModel sut = new ImageTemplateEditorModel(
                MockImageProvider.CreateDefaultMockImageProviderWithSubgroupAndImage(),
                "/images/Product/C230/myfile.gif");
            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Data);
            Assert.AreEqual("/images/Product/C230/myfile.gif", sut.Data);
            Assert.AreEqual("Product", sut.ActiveGroup);
            Assert.AreEqual("C230", sut.ActiveSubgroup);
            Assert.AreEqual("myfile.gif", sut.ActiveFile);
            Assert.IsNotNull(sut.Groups);
            Assert.AreEqual(2, sut.Groups.Length);
            Assert.AreEqual("Products", sut.Groups[0]);
            Assert.AreEqual("General", sut.Groups[1]);
        }
    }
}
