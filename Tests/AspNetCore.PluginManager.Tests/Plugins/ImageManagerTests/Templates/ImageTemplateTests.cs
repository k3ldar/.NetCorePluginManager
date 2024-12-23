﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: ImageTemplateTests.cs
 *
 *  Purpose:  Tests for spacer template
 *
 *  Date        Name                Reason
 *  12/06/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using ImageManager.Plugin.Templates;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedPluginFeatures;
using SharedPluginFeatures.DynamicContent;

namespace AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ImageTemplateTests : GenericBaseClass
    {
        private const string TestCategoryName = "Dynamic Content";
        private const string ContentWithCssStyleAndClass = "<div style=\"width:538px !important;height:200px !important;display:block;\"><img src=\"/images/myimage.gif\" alt=\"image\" style=\"max-height:100%;border:1px solid blue;\" class=\"myclass\"></div>";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_Success()
        {
            ImageTemplate sut = new ImageTemplate();

            Assert.IsNotNull(sut);
            Assert.AreEqual(DynamicContentTemplateType.Default, sut.TemplateType);
            Assert.AreEqual(400, sut.TemplateSortOrder);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AssemblyNameValid_Success()
        {
            ImageTemplate sut = new ImageTemplate();

            Assert.IsTrue(sut.AssemblyQualifiedName.StartsWith("ImageManager.Plugin.Templates.ImageTemplate, ImageManager.Plugin, Version="));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditorAction_ReturnsCorrectControllerAction_Success()
        {
            ImageTemplate sut = new ImageTemplate();

            Assert.AreEqual("/ImageManager/ImageTemplateEditor/", sut.EditorAction);
            Assert.AreEqual("", sut.EditorInstructions);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Name_ReturnsValidValidName_Success()
        {
            ImageTemplate sut = new ImageTemplate();

            Assert.AreEqual("Image", sut.Name);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SortOrder_ReturnsDefaultSortOrder_Success()
        {
            ImageTemplate sut = new ImageTemplate();

            Assert.AreEqual(0, sut.SortOrder);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SortOrder_RemembersNewValue_Success()
        {
            ImageTemplate sut = new ImageTemplate();
            sut.SortOrder = 123;

            Assert.AreEqual(123, sut.SortOrder);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void HeightType_ReturnsPixels_Success()
        {
            ImageTemplate sut = new ImageTemplate();

            Assert.AreEqual(DynamicContentHeightType.Pixels, sut.HeightType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void HeightType_SettingOtherValue_Success()
        {
            ImageTemplate sut = new ImageTemplate();
            sut.HeightType = DynamicContentHeightType.Percentage;

            Assert.AreEqual(DynamicContentHeightType.Percentage, sut.HeightType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Height_ReturnsDefaultValue_Success()
        {
            ImageTemplate sut = new ImageTemplate();

            Assert.AreEqual(200, sut.Height);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Height_SettingOtherValueReturnsDefaultHeight_Success()
        {
            ImageTemplate sut = new ImageTemplate();
            sut.Height = 100;

            Assert.AreEqual(100, sut.Height);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void WidthType_ReturnsAutomatic_Success()
        {
            ImageTemplate sut = new ImageTemplate();

            Assert.AreEqual(DynamicContentWidthType.Columns, sut.WidthType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void WidthType_SettingOtherValueReturnsAutomatic_Success()
        {
            ImageTemplate sut = new ImageTemplate();
            sut.WidthType = DynamicContentWidthType.Percentage;

            Assert.AreEqual(DynamicContentWidthType.Percentage, sut.WidthType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Width_ReturnsDefaultValue_Success()
        {
            ImageTemplate sut = new ImageTemplate();

            Assert.AreEqual(3, sut.Width);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Width_SettingOtherValueReturnsDefaultWidth_Success()
        {
            ImageTemplate sut = new ImageTemplate();
            sut.Width = 100;

            Assert.AreEqual(100, sut.Width);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Data_DefaultValue_Success()
        {
            ImageTemplate sut = new ImageTemplate();

            Assert.AreEqual(String.Empty, sut.Data);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Data_SetValue_ValueIsRemembered_Success()
        {
            ImageTemplate sut = new ImageTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 10;
            sut.Data = "/Images/Product/CS1234/Image.png";

            Assert.AreEqual("/Images/Product/CS1234/Image.png", sut.Data);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ActiveFrom_DefaultValue_Success()
        {
            ImageTemplate sut = new ImageTemplate();

            Assert.AreEqual(DefaultActiveFrom, sut.ActiveFrom);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ActiveTo_DefaultValue_Success()
        {
            ImageTemplate sut = new ImageTemplate();

            Assert.AreEqual(DefaultActiveTo, sut.ActiveTo);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypeColumn_Valid()
        {
            ImageTemplate sut = new ImageTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 8;

            string content = sut.Content();

            Assert.AreEqual("<div class=\"col-sm-8\" style=\"height:200px !important;\"><p>Please select an image</p></div>", content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditorContent_WidthTypeColumn_WithData_Valid()
        {
            ImageTemplate sut = new ImageTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 8;
            sut.Data = "/images/test.gif";

            string content = sut.EditorContent();

            Assert.AreEqual("<div class=\"col-sm-12\" style=\"height:200px !important;\"><img src=\"/images/test.gif\" alt=\"image\" style=\"max-height:100%;width:100%;\"></div>", content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypeColumn_WithData_Valid()
        {
            ImageTemplate sut = new ImageTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 8;
            sut.Data = "/images/test.gif";

            string content = sut.Content();

            Assert.AreEqual("<div class=\"col-sm-8\" style=\"height:200px !important;\"><img src=\"/images/test.gif\" alt=\"image\" style=\"max-height:100%;\"></div>", content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypePercentage_Valid()
        {
            ImageTemplate sut = new ImageTemplate();
            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = 40;

            string content = sut.EditorContent();

            Assert.AreEqual("<div style=\"width:40% !important;height:200px !important;display:block;\"><p>Please select an image</p></div>", content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypePixels_Valid()
        {
            ImageTemplate sut = new ImageTemplate();
            sut.WidthType = DynamicContentWidthType.Pixels;
            sut.Width = 538;
            string content = sut.Content();

            Assert.AreEqual("<div style=\"width:538px !important;height:200px !important;display:block;\"><p>Please select an image</p></div>", content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Clone_EmptyUniqueId_ContainsGuid_Success()
        {
            ImageTemplate sut = new ImageTemplate();

            DynamicContentTemplate clone = sut.Clone(String.Empty);

            Assert.IsNotNull(clone);
            Assert.IsInstanceOfType(clone, typeof(ImageTemplate));
            bool guidParsed = Guid.TryParse(clone.UniqueId, out Guid uniqueId);
            Assert.IsTrue(guidParsed);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Clone_NullUniqueId_ContainsGuid_Success()
        {
            ImageTemplate sut = new ImageTemplate();

            DynamicContentTemplate clone = sut.Clone(null);

            Assert.IsNotNull(clone);
            Assert.IsInstanceOfType(clone, typeof(ImageTemplate));
            bool guidParsed = Guid.TryParse(clone.UniqueId, out Guid uniqueId);
            Assert.IsTrue(guidParsed);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Clone_ValidUniqueId_ContainsGuid_Success()
        {
            ImageTemplate sut = new ImageTemplate();

            DynamicContentTemplate clone = sut.Clone("my-unique-id");

            Assert.IsNotNull(clone);
            Assert.IsInstanceOfType(clone, typeof(ImageTemplate));
            bool guidParsed = Guid.TryParse(clone.UniqueId, out Guid uniqueId);
            Assert.IsFalse(guidParsed);

            Assert.AreEqual("my-unique-id", clone.UniqueId);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_ContainsCssClassAndCssStyle_Success()
        {
            ImageTemplate sut = new ImageTemplate();
            sut.WidthType = DynamicContentWidthType.Pixels;
            sut.Width = 538;
            sut.Data = "/images/myimage.gif";
            sut.CssClassName = "myclass";
            sut.CssStyle = "border:1px solid blue;";
            string content = sut.Content();

            Assert.AreEqual(ContentWithCssStyleAndClass, content);
        }
    }
}
