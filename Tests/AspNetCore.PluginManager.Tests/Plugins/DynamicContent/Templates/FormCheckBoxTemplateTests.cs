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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: FormCheckBoxTemplateTests.cs
 *
 *  Purpose:  Tests for html check box template
 *
 *  Date        Name                Reason
 *  13/07/2021  Simon Carter        Initially Created
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
    public class FormCheckBoxTemplateTests : GenericBaseClass
    {
        private const string TestCategoryName = "Dynamic Content";
        private const string BlankDataDefaultValues12Columns = "<div class=\"col-sm-12\"><div class=\"form-group\"><label for=\"test\">Enter test data</label><input type=\"checkbox\" class=\"form-check-input\" id=\"test\" onclick=\"updateUC();\" onfocusout=\"updateUC();\"></div></div>";
        private const string BlankDataDefaultValues12ColumnsNoLabel = "<div class=\"col-sm-12\"><div class=\"form-group\"><input type=\"checkbox\" class=\"form-check-input\" id=\"test\" onclick=\"updateUC();\" onfocusout=\"updateUC();\"></div></div>";
        private const string BlankDataDefaultValues12ColumnsEditor = "<div class=\"col-sm-12\"><div class=\"form-group\" style=\"margin: 0 0 0 10px;min-height:32px;\"><input type=\"checkbox\" class=\"form-check-input\" id=\"test\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" disabled><label for=\"test\" class=\"form-check-label\">Enter test data</label></div></div>";
        private const string BlankDataDefaultValues12ColumnsEditorNoLabel = "<div class=\"col-sm-12\"><div class=\"form-group\" style=\"margin: 0 0 0 10px;min-height:32px;\"><input type=\"checkbox\" class=\"form-check-input\" id=\"test\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" disabled></div></div>";
        private const string BlankDataDefaultValues10Columns = "<div class=\"col-sm-10\"><div class=\"form-group\"><input type=\"checkbox\" class=\"form-check-input\" id=\"test\" onclick=\"updateUC();\" onfocusout=\"updateUC();\"><label for=\"test\" class=\"form-check-label\">Enter test data</label></div></div>";
        private const string BlankDataDefaultValues10ColumnsNoLabel = "<div class=\"col-sm-10\"><div class=\"form-group\"><input type=\"checkbox\" class=\"form-check-input\" id=\"test\" onclick=\"updateUC();\" onfocusout=\"updateUC();\"></div></div>";
        private const string BlankDataDefaultValues8Columns = "<div class=\"col-sm-8\"><div class=\"form-group\"><input type=\"checkbox\" class=\"form-check-input\" id=\"test\" onclick=\"updateUC();\" onfocusout=\"updateUC();\"><label for=\"test\" class=\"form-check-label\">Enter test data</label></div></div>";
        private const string BlankDataDefaultValues8ColumnsNoLabel = "<div class=\"col-sm-8\"><div class=\"form-group\"><input type=\"checkbox\" class=\"form-check-input\" id=\"test\" onclick=\"updateUC();\" onfocusout=\"updateUC();\"></div></div>";
        private const string FormWithLabelGreenBorderLineBreak = "<div class=\"col-sm-10\"><div class=\"form-group css-class\" style=\"border: 1px solid green;\"><label for=\"textbox1\" class=\"form-check-label\">Fill in the data</label><br /><input type=\"checkbox\" class=\"form-check-input\" id=\"textbox1\" onclick=\"updateUC();\" onfocusout=\"updateUC();\"></div></div>";
        private const string FormWithLabelGreenBorder = "<div class=\"col-sm-10\"><div class=\"form-group css-class\" style=\"border: 1px solid green;\"><input type=\"checkbox\" class=\"form-check-input\" id=\"textbox1\" onclick=\"updateUC();\" onfocusout=\"updateUC();\"><label for=\"textbox1\" class=\"form-check-label\">Fill in the data</label></div></div>";
        private const string FormWithLabelGreenBorderControlStyles = "<div class=\"col-sm-10\"><div class=\"form-group css-class\" style=\"border: 1px solid green;\"><input type=\"checkbox\" class=\"form-check-input\" id=\"textbox1\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" style=\"width: 70%;\"><label for=\"textbox1\" class=\"form-check-label\" style=\"border: 1px solid green;\">Fill in the data</label></div></div>";
        private const string DefaultData538Pixels = "<div style=\"width:538px !important;display:block;\"><div class=\"form-group\"><input type=\"checkbox\" class=\"form-check-input\" id=\"test\" onclick=\"updateUC();\" onfocusout=\"updateUC();\"><label for=\"test\" class=\"form-check-label\">Enter test data</label></div></div>";
        private const string DefaultData538PixelsNoLabel = "<div style=\"width:538px !important;display:block;\"><div class=\"form-group\"><input type=\"checkbox\" class=\"form-check-input\" id=\"test\" onclick=\"updateUC();\" onfocusout=\"updateUC();\"></div></div>";
        private const string DefaultData40Percent = "<div style=\"width:40% !important;display:block;\"><div class=\"form-group\"><input type=\"checkbox\" class=\"form-check-input\" id=\"test\" onclick=\"updateUC();\" onfocusout=\"updateUC();\"><label for=\"test\" class=\"form-check-label\">Enter test data</label></div></div>";
        private const string DefaultData40PercentNoLabel = "<div style=\"width:40% !important;display:block;\"><div class=\"form-group\"><input type=\"checkbox\" class=\"form-check-input\" id=\"test\" onclick=\"updateUC();\" onfocusout=\"updateUC();\"></div></div>";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();

            Assert.IsNotNull(sut);
            Assert.AreEqual(DynamicContentTemplateType.Input, sut.TemplateType);
            Assert.AreEqual(15000, sut.TemplateSortOrder);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AssemblyNameValid_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();

            Assert.IsTrue(sut.AssemblyQualifiedName.StartsWith("DynamicContent.Plugin.Templates.FormCheckBoxTemplate, DynamicContent.Plugin, Version="));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditorAction_ReturnsEmptyControllerName_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();

            Assert.AreEqual("/DynamicContent/FormControlTemplateEditorRightAlign/", sut.EditorAction);
            Assert.AreEqual("", sut.EditorInstructions);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Name_ReturnsValidValidName_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();

            Assert.AreEqual("Check Box", sut.Name);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SortOrder_ReturnsDefaultSortOrder_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();

            Assert.AreEqual(0, sut.SortOrder);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SortOrder_RemembersNewValue_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();
            sut.SortOrder = 123;

            Assert.AreEqual(123, sut.SortOrder);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void HeightType_ReturnsAutomatic_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();

            Assert.AreEqual(DynamicContentHeightType.Automatic, sut.HeightType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void HeightType_SettingOtherValueReturnsAutomatic_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();
            sut.HeightType = DynamicContentHeightType.Percentage;

            Assert.AreEqual(DynamicContentHeightType.Automatic, sut.HeightType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Height_ReturnsDefaultValue_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();

            Assert.AreEqual(-1, sut.Height);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Height_SettingOtherValueReturnsDefaultHeight_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();
            sut.Height = 100;

            Assert.AreEqual(-1, sut.Height);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void WidthType_ReturnsAutomatic_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();

            Assert.AreEqual(DynamicContentWidthType.Columns, sut.WidthType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void WidthType_SettingOtherValueReturnsAutomatic_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();
            sut.WidthType = DynamicContentWidthType.Percentage;

            Assert.AreEqual(DynamicContentWidthType.Percentage, sut.WidthType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Width_ReturnsDefaultValue_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();

            Assert.AreEqual(12, sut.Width);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Width_SettingOtherValueReturnsDefaultWidth_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();
            sut.Width = 100;

            Assert.AreEqual(100, sut.Width);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Data_DefaultValue_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();

            Assert.IsNull(sut.Data);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Data_SetValue_WithoutLabel_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 10;
            sut.Data = "test";

            string content = sut.Content();

            Assert.AreEqual(BlankDataDefaultValues10ColumnsNoLabel, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Data_SetValue_WithLabel_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 10;
            sut.Data = "test|Enter test data";

            string content = sut.Content();

            Assert.AreEqual(BlankDataDefaultValues10Columns, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ActiveFrom_DefaultValue_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();

            Assert.AreEqual(DefaultActiveFrom, sut.ActiveFrom);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ActiveTo_DefaultValue_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();

            Assert.AreEqual(DefaultActiveTo, sut.ActiveTo);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypeColumn_Valid()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 8;
            sut.Data = "test";

            string content = sut.Content();

            Assert.AreEqual(BlankDataDefaultValues8ColumnsNoLabel, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditorContent_WidthTypeColumn_WithoutLabel_Valid()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 8;
            sut.Data = "test";

            string content = sut.EditorContent();

            Assert.AreEqual(BlankDataDefaultValues12ColumnsEditorNoLabel, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditorContent_WidthTypeColumn_WithLabel_Valid()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 8;
            sut.Data = "test|Enter test data";

            string content = sut.EditorContent();

            Assert.AreEqual(BlankDataDefaultValues12ColumnsEditor, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypeColumn_WithoutLabel_Valid()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 8;
            sut.Data = "test";

            string content = sut.Content();

            Assert.AreEqual(BlankDataDefaultValues8ColumnsNoLabel, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypeColumn_WithLabel_Valid()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 8;
            sut.Data = "test|Enter test data";

            string content = sut.Content();

            Assert.AreEqual(BlankDataDefaultValues8Columns, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypePercentage_WithoutLabel_Valid()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();
            sut.Data = "test";
            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = 40;

            string content = sut.Content();

            Assert.AreEqual(DefaultData40PercentNoLabel, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypePercentage_WithLabel_Valid()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();
            sut.Data = "test|Enter test data";
            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = 40;

            string content = sut.Content();

            Assert.AreEqual(DefaultData40Percent, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypePixels_WithoutLabel_Valid()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();
            sut.Data = "test";
            sut.WidthType = DynamicContentWidthType.Pixels;
            sut.Width = 538;
            string content = sut.Content();

            Assert.AreEqual(DefaultData538PixelsNoLabel, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypePixels_WithLabel_Valid()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();
            sut.Data = "test|Enter test data";
            sut.WidthType = DynamicContentWidthType.Pixels;
            sut.Width = 538;
            string content = sut.Content();

            Assert.AreEqual(DefaultData538Pixels, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Clone_EmptyUniqueId_ContainsGuid_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();

            DynamicContentTemplate clone = sut.Clone(String.Empty);

            Assert.IsNotNull(clone);
            Assert.IsInstanceOfType(clone, typeof(FormCheckBoxTemplate));
            bool guidParsed = Guid.TryParse(clone.UniqueId, out Guid uniqueId);
            Assert.IsTrue(guidParsed);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Clone_NullUniqueId_ContainsGuid_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();

            DynamicContentTemplate clone = sut.Clone(null);

            Assert.IsNotNull(clone);
            Assert.IsInstanceOfType(clone, typeof(FormCheckBoxTemplate));
            bool guidParsed = Guid.TryParse(clone.UniqueId, out Guid uniqueId);
            Assert.IsTrue(guidParsed);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Clone_ValidUniqueId_ContainsGuid_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();

            DynamicContentTemplate clone = sut.Clone("my-unique-id");

            Assert.IsNotNull(clone);
            Assert.IsInstanceOfType(clone, typeof(FormCheckBoxTemplate));
            bool guidParsed = Guid.TryParse(clone.UniqueId, out Guid uniqueId);
            Assert.IsFalse(guidParsed);

            Assert.AreEqual("my-unique-id", clone.UniqueId);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_ContainsCssStyleAndClass_WithLineBreak_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 10;
            sut.CssClassName = "css-class";
            sut.CssStyle = "border: 1px solid green;";
            sut.Data = "textbox1|Fill in the data|true";

            string content = sut.Content();

            Assert.AreEqual(FormWithLabelGreenBorderLineBreak, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_ContainsCssStyleAndClass_WithoutLineBreak_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 10;
            sut.CssClassName = "css-class";
            sut.CssStyle = "border: 1px solid green;";
            sut.Data = "textbox1|Fill in the data|false";

            string content = sut.Content();

            Assert.AreEqual(FormWithLabelGreenBorder, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_ContainsControlAndLabelStyles_WithoutLineBreak_Success()
        {
            FormCheckBoxTemplate sut = new FormCheckBoxTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 10;
            sut.CssClassName = "css-class";
            sut.CssStyle = "border: 1px solid green;";
            sut.Data = "textbox1|Fill in the data|false|border: 1px solid green;|width: 70%;";

            string content = sut.Content();

            Assert.AreEqual(FormWithLabelGreenBorderControlStyles, content);
        }
    }
}
