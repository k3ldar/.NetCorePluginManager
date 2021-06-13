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

using DynamicContent.Plugin.Templates;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedPluginFeatures;
using SharedPluginFeatures.DynamicContent;

namespace AspNetCore.PluginManager.Tests.Plugins.DynamicContentTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ImageTemplateTests
    {
        private const string TestCategoryName = "Dynamic Content";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_Success()
        {
            ImageTemplate sut = new ImageTemplate();

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AssemblyNameValid_Success()
        {
            ImageTemplate sut = new ImageTemplate();

            Assert.IsTrue(sut.AssemblyQualifiedName.StartsWith("DynamicContent.Plugin.Templates.ImageTemplate, DynamicContent.Plugin, Version="));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditorAction_ReturnsCorrectControllerAction_Success()
        {
            ImageTemplate sut = new ImageTemplate();

            Assert.AreEqual("/DynamicContent/ImageTemplateEditor/", sut.EditorAction);
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

            Assert.AreEqual(2, sut.Width);
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

            Assert.AreEqual(DateTime.MinValue, sut.ActiveFrom);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ActiveTo_DefaultValue_Success()
        {
            ImageTemplate sut = new ImageTemplate();

            Assert.AreEqual(DateTime.MaxValue, sut.ActiveTo);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypeColumn_Valid()
        {
            ImageTemplate sut = new ImageTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 8;

            string content = sut.Content();

            Assert.AreEqual("<div class=\"col-sm-8\" style=\"height:200px !important;\"></div>", content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypePercentage_Valid()
        {
            ImageTemplate sut = new ImageTemplate();
            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = 40;

            string content = sut.Content();

            Assert.AreEqual("<div style=\"width:40% !important;height:200px !important;display:block;\"></div>", content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypePixels_Valid()
        {
            ImageTemplate sut = new ImageTemplate();
            sut.WidthType = DynamicContentWidthType.Pixels;
            sut.Width = 538;
            string content = sut.Content();

            Assert.AreEqual("<div style=\"width:538px !important;height:200px !important;display:block;\"></div>", content);
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
    }
}
