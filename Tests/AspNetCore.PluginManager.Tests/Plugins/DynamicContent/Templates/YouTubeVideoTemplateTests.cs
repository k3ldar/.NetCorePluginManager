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
 *  File: YouTubeVideoTemplateTests.cs
 *
 *  Purpose:  Tests for html text template
 *
 *  Date        Name                Reason
 *  19/06/2021  Simon Carter        Initially Created
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
    public class YouTubeVideoTemplateTests
    {
        private const string TestCategoryName = "Dynamic Content";

        private const string WidthTypeValidationColumn = "<div class=\"col-sm-8\" style=\"height:450px !important;\"><iframe type=\"text/html\" width=\"100%\" height=\"100%\" src=\"https://www.youtube.com/embed/aAbBcCdD?autoplay=1\" frameborder=\"0\" allow=\"autoplay\"></iframe></div>";
        private const string WidthTypeValidationPercent = "<div style=\"width:40% !important;height:450px !important;display:block;\"><iframe type=\"text/html\" width=\"100%\" height=\"100%\" src=\"https://www.youtube.com/embed/<p>test</p>?autoplay=1\" frameborder=\"0\" allow=\"autoplay\"></iframe></div>";
        private const string WidthTypeValidationPixels = "<div style=\"width:538px !important;height:450px !important;display:block;\"><iframe type=\"text/html\" width=\"100%\" height=\"100%\" src=\"https://www.youtube.com/embed/<p>test</p>?autoplay=1\" frameborder=\"0\" allow=\"autoplay\"></iframe></div>";
        private const string ExtraValuesResponse = "<div class=\"col-sm-12\" style=\"height:450px !important;\"><iframe type=\"text/html\" width=\"100%\" height=\"100%\" src=\"https://www.youtube.com/embed/test\" frameborder=\"0\"></iframe></div>";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_Success()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AssemblyNameValid_Success()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();

            Assert.IsTrue(sut.AssemblyQualifiedName.StartsWith("DynamicContent.Plugin.Templates.YouTubeVideoTemplate, DynamicContent.Plugin, Version="));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditorAction_ReturnsEmptyControllerName_Success()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();

            Assert.AreEqual("/DynamicContent/YouTubeTemplateEditor/", sut.EditorAction);
            Assert.AreEqual("", sut.EditorInstructions);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Name_ReturnsValidValidName_Success()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();

            Assert.AreEqual("YouTube Video", sut.Name);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SortOrder_ReturnsDefaultSortOrder_Success()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();

            Assert.AreEqual(0, sut.SortOrder);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SortOrder_RemembersNewValue_Success()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();
            sut.SortOrder = 123;

            Assert.AreEqual(123, sut.SortOrder);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void HeightType_ReturnsAutomatic_Success()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();

            Assert.AreEqual(DynamicContentHeightType.Pixels, sut.HeightType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void HeightType_SettingOtherValueReturnsAutomatic_Success()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();
            sut.HeightType = DynamicContentHeightType.Automatic;

            Assert.AreEqual(DynamicContentHeightType.Automatic, sut.HeightType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Height_ReturnsDefaultValue_Success()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();

            Assert.AreEqual(450, sut.Height);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Height_SettingOtherValueReturnsDefaultHeight_Success()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();
            sut.Height = 100;

            Assert.AreEqual(100, sut.Height);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void WidthType_ReturnsAutomatic_Success()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();

            Assert.AreEqual(DynamicContentWidthType.Columns, sut.WidthType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void WidthType_SettingOtherValueReturnsAutomatic_Success()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();
            sut.WidthType = DynamicContentWidthType.Percentage;

            Assert.AreEqual(DynamicContentWidthType.Percentage, sut.WidthType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Width_ReturnsDefaultValue_Success()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();

            Assert.AreEqual(12, sut.Width);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Width_SettingOtherValueReturnsDefaultWidth_Success()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();
            sut.Width = 100;

            Assert.AreEqual(100, sut.Width);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Data_DefaultValue_Success()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();

            Assert.AreEqual("|True", sut.Data);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Data_SetValueMissingYouTubeId_Success()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 10;
            sut.Data = "";

            string content = sut.Content();

            Assert.AreEqual("<div class=\"col-sm-10\" style=\"height:450px !important;\"><p>Please enter a valid video Id</p></div>", content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ActiveFrom_DefaultValue_Success()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();

            Assert.AreEqual(DateTime.MinValue, sut.ActiveFrom);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ActiveTo_DefaultValue_Success()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();

            Assert.AreEqual(DateTime.MaxValue, sut.ActiveTo);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypeColumn_Valid()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 8;
            sut.Data = "aAbBcCdD|True";

            string content = sut.Content();

            Assert.AreEqual(WidthTypeValidationColumn, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditorContent_MultipleExtraValues_Valid()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 8;
            sut.Data = "test|123|value|another";

            string content = sut.EditorContent();

            Assert.AreEqual(ExtraValuesResponse, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypePercentage_Valid()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();
            sut.Data = "<p>test</p>";
            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = 40;

            string content = sut.Content();

            Assert.AreEqual(WidthTypeValidationPercent, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypePixels_Valid()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();
            sut.Data = "<p>test</p>";
            sut.WidthType = DynamicContentWidthType.Pixels;
            sut.Width = 538;
            string content = sut.Content();

            Assert.AreEqual(WidthTypeValidationPixels, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Clone_EmptyUniqueId_ContainsGuid_Success()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();

            DynamicContentTemplate clone = sut.Clone(String.Empty);

            Assert.IsNotNull(clone);
            Assert.IsInstanceOfType(clone, typeof(YouTubeVideoTemplate));
            bool guidParsed = Guid.TryParse(clone.UniqueId, out Guid uniqueId);
            Assert.IsTrue(guidParsed);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Clone_NullUniqueId_ContainsGuid_Success()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();

            DynamicContentTemplate clone = sut.Clone(null);

            Assert.IsNotNull(clone);
            Assert.IsInstanceOfType(clone, typeof(YouTubeVideoTemplate));
            bool guidParsed = Guid.TryParse(clone.UniqueId, out Guid uniqueId);
            Assert.IsTrue(guidParsed);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Clone_ValidUniqueId_ContainsGuid_Success()
        {
            YouTubeVideoTemplate sut = new YouTubeVideoTemplate();

            DynamicContentTemplate clone = sut.Clone("my-unique-id");

            Assert.IsNotNull(clone);
            Assert.IsInstanceOfType(clone, typeof(YouTubeVideoTemplate));
            bool guidParsed = Guid.TryParse(clone.UniqueId, out Guid uniqueId);
            Assert.IsFalse(guidParsed);

            Assert.AreEqual("my-unique-id", clone.UniqueId);
        }
    }
}
