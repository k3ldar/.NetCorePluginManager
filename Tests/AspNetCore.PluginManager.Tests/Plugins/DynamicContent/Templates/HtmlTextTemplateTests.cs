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
 *  File: HtmlTextTemplateTests.cs
 *
 *  Purpose:  Tests for html text template
 *
 *  Date        Name                Reason
 *  24/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using DynamicContent.Plugin.Templates;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedPluginFeatures;
using SharedPluginFeatures.DynamicContent;

namespace AspNetCore.PluginManager.Tests.Plugins.DynamicContentTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HtmlTextTemplateTests : GenericBaseClass
    {
        private const string TestCategoryName = "Dynamic Content";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            Assert.IsNotNull(sut);
            Assert.AreEqual(DynamicContentTemplateType.Default, sut.TemplateType);
            Assert.AreEqual(200, sut.TemplateSortOrder);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AssemblyNameValid_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            Assert.IsTrue(sut.AssemblyQualifiedName.StartsWith("DynamicContent.Plugin.Templates.HtmlTextTemplate, DynamicContent.Plugin, Version="));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditorAction_ReturnsValidControllerName_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            Assert.AreEqual("/DynamicContent/TextTemplateEditor/", sut.EditorAction);
            Assert.AreEqual("", sut.EditorInstructions);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Name_ReturnsValidValidName_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            Assert.AreEqual("Html Content", sut.Name);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SortOrder_ReturnsDefaultSortOrder_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            Assert.AreEqual(0, sut.SortOrder);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SortOrder_RemembersNewValue_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();
            sut.SortOrder = 123;

            Assert.AreEqual(123, sut.SortOrder);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void HeightType_ReturnsAutomatic_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            Assert.AreEqual(DynamicContentHeightType.Automatic, sut.HeightType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void HeightType_SettingOtherValueReturnsAutomatic_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();
            sut.HeightType = DynamicContentHeightType.Percentage;

            Assert.AreEqual(DynamicContentHeightType.Automatic, sut.HeightType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Height_ReturnsDefaultValue_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            Assert.AreEqual(-1, sut.Height);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Height_SettingOtherValueReturnsDefaultHeight_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();
            sut.Height = 100;

            Assert.AreEqual(-1, sut.Height);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void WidthType_ReturnsAutomatic_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            Assert.AreEqual(DynamicContentWidthType.Columns, sut.WidthType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void WidthType_SettingOtherValueReturnsAutomatic_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();
            sut.WidthType = DynamicContentWidthType.Percentage;

            Assert.AreEqual(DynamicContentWidthType.Percentage, sut.WidthType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Width_ReturnsDefaultValue_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            Assert.AreEqual(12, sut.Width);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Width_SettingOtherValueReturnsDefaultWidth_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();
            sut.Width = 100;

            Assert.AreEqual(100, sut.Width);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Data_DefaultValue_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            Assert.AreEqual(null, sut.Data);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Data_SetValue_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 10;
            sut.Data = "<p>new data</p>";

            string content = sut.Content();

            Assert.AreEqual("<div class=\"col-sm-10\"><p>new data</p></div>", content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ActiveFrom_DefaultValue_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            Assert.AreEqual(DefaultActiveFrom, sut.ActiveFrom);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ActiveTo_DefaultValue_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            Assert.AreEqual(DefaultActiveTo, sut.ActiveTo);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypeColumn_Valid()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 8;
            sut.Data = "<p>test</p>";

            string content = sut.Content();

            Assert.AreEqual("<div class=\"col-sm-8\"><p>test</p></div>", content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditorContent_WidthTypeColumn_Valid()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 8;
            sut.Data = "<p>test</p>";

            string content = sut.EditorContent();

            Assert.AreEqual("<div class=\"col-sm-12\"><p>test</p></div>", content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypeColumn_EditorContent_Valid()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 8;
            sut.Data = "<p>test</p>";

            string content = sut.EditorContent();

            Assert.AreEqual("<div class=\"col-sm-12\"><p>test</p></div>", content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypePercentage_Valid()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();
            sut.Data = "<p>test</p>";
            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = 40;

            string content = sut.Content();

            Assert.AreEqual("<div style=\"width:40% !important;display:block;\"><p>test</p></div>", content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypePixels_Valid()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();
            sut.Data = "<p>test</p>";
            sut.WidthType = DynamicContentWidthType.Pixels;
            sut.Width = 538;
            string content = sut.Content();

            Assert.AreEqual("<div style=\"width:538px !important;display:block;\"><p>test</p></div>", content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Clone_EmptyUniqueId_ContainsGuid_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            DynamicContentTemplate clone = sut.Clone(String.Empty);

            Assert.IsNotNull(clone);
            Assert.IsInstanceOfType(clone, typeof(HtmlTextTemplate));
            bool guidParsed = Guid.TryParse(clone.UniqueId, out Guid uniqueId);
            Assert.IsTrue(guidParsed);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Clone_NullUniqueId_ContainsGuid_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            DynamicContentTemplate clone = sut.Clone(null);

            Assert.IsNotNull(clone);
            Assert.IsInstanceOfType(clone, typeof(HtmlTextTemplate));
            bool guidParsed = Guid.TryParse(clone.UniqueId, out Guid uniqueId);
            Assert.IsTrue(guidParsed);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Clone_ValidUniqueId_ContainsGuid_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            DynamicContentTemplate clone = sut.Clone("my-unique-id");

            Assert.IsNotNull(clone);
            Assert.IsInstanceOfType(clone, typeof(HtmlTextTemplate));
            bool guidParsed = Guid.TryParse(clone.UniqueId, out Guid uniqueId);
            Assert.IsFalse(guidParsed);

            Assert.AreEqual("my-unique-id", clone.UniqueId);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WithCssClassAndStyle_StyleAndClassIgnored_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 10;
            sut.Data = "<p>new data</p>";
            sut.CssClassName = "myclass";
            sut.CssStyle = "border: 1px solid yellow;";

            string content = sut.Content();

            Assert.AreEqual("<div class=\"col-sm-10\"><p>new data</p></div>", content);
        }
    }
}
