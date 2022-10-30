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
 *  File: FormRadioGroupTemplateTests.cs
 *
 *  Purpose:  Tests for html check box template
 *
 *  Date        Name                Reason
 *  14/07/2021  Simon Carter        Initially Created
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
    public class FormRadioGroupTemplateTests : GenericBaseClass
    {
        private const string TestCategoryName = "Dynamic Content";
        private const string BlankDataDefaultValues12ColumnsEditor = "<div class=\"col-sm-12\"><div class=\"form-group\" style=\"margin: 0 0 0 10px;min-height:32px;\"><input type=\"radio\" name=\"test\" id=\"Option-X\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" class=\"form-check-input\"  disabled><label for=\"Option-X\" class=\"form-check-label\">Option X</label><br /><input type=\"radio\" name=\"test\" id=\"Option-Y\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" class=\"form-check-input\"  disabled><label for=\"Option-Y\" class=\"form-check-label\">Option Y</label><br /><input type=\"radio\" name=\"test\" id=\"Option-Z\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" class=\"form-check-input\"  disabled><label for=\"Option-Z\" class=\"form-check-label\">Option Z</label><br /></div></div>";
        private const string BlankDataDefaultValues10Columns = "<div class=\"col-sm-10\"><div class=\"form-group\"><input type=\"radio\" name=\"test\" id=\"Option-1\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" class=\"form-check-input\" ><label for=\"Option-1\" class=\"form-check-label\">Option 1</label><br /><input type=\"radio\" name=\"test\" id=\"Option-2\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" class=\"form-check-input\" ><label for=\"Option-2\" class=\"form-check-label\">Option 2</label><br /><input type=\"radio\" name=\"test\" id=\"Option-3\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" class=\"form-check-input\" ><label for=\"Option-3\" class=\"form-check-label\">Option 3</label><br /></div></div>";
        private const string BlankDataDefaultValues8Columns = "<div class=\"col-sm-8\"><div class=\"form-group\"><input type=\"radio\" name=\"test\" id=\"Select-this\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" class=\"form-check-input\" ><label for=\"Select-this\" class=\"form-check-label\">Select this</label><br /><input type=\"radio\" name=\"test\" id=\"or-this\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" class=\"form-check-input\" ><label for=\"or-this\" class=\"form-check-label\">or this</label><br /></div></div>";
        private const string BlankDataDefaultValues8ColumnsNoLabel = "<div class=\"col-sm-8\"><div class=\"form-group\"><input type=\"radio\" name=\"test\" id=\"Option-A\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" class=\"form-check-input\" ><label for=\"Option-A\" class=\"form-check-label\">Option A</label><br /><input type=\"radio\" name=\"test\" id=\"Option-B\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" class=\"form-check-input\" ><label for=\"Option-B\" class=\"form-check-label\">Option B</label><br /></div></div>";
        private const string FormWithLabelGreenBorderLineBreak = "<div class=\"col-sm-10\"><div class=\"form-group css-class\" style=\"border: 1px solid green;\"><input type=\"radio\" name=\"textbox1\" id=\"Fill-in-the-data\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" class=\"form-check-input\" ><label for=\"Fill-in-the-data\" class=\"form-check-label\">Fill in the data</label><br /></div></div>";
        private const string FormWithLabelGreenBorder = "<div class=\"col-sm-10\"><div class=\"form-group css-class\" style=\"border: 1px solid green;\"><input type=\"radio\" name=\"textbox1\" id=\"Option-1\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" class=\"form-check-input\" ><label for=\"Option-1\" class=\"form-check-label\">Option 1</label><br /><input type=\"radio\" name=\"textbox1\" id=\"Option-2\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" class=\"form-check-input\" ><label for=\"Option-2\" class=\"form-check-label\">Option 2</label><br /></div></div>";
        private const string FormWithLabelGreenBorderControlStyles = "<div class=\"col-sm-10\"><div class=\"form-group css-class\" style=\"border: 1px solid green;\"><input type=\"radio\" name=\"textbox1\" id=\"Fill-in-the-data\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" class=\"form-check-input\"  style=\"width: 70%;\"><label for=\"Fill-in-the-data\" class=\"form-check-label\" style=\"border: 1px solid green;\">Fill in the data</label><br /></div></div>";
        private const string DefaultData40Percent = "<div style=\"width:40% !important;display:block;\"><div class=\"form-group\"><input type=\"radio\" name=\"test\" id=\"Select-this-option\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" class=\"form-check-input\" ><label for=\"Select-this-option\" class=\"form-check-label\">Select this option</label><br /><input type=\"radio\" name=\"test\" id=\"Or-select-this-one\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" class=\"form-check-input\" ><label for=\"Or-select-this-one\" class=\"form-check-label\">Or select this one</label><br /></div></div>";
        private const string DefaultData40PercentNoLabel = "<div style=\"width:40% !important;display:block;\"><div class=\"form-group\"><input type=\"radio\" name=\"test\" id=\"Select-this-option\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" class=\"form-check-input\" ><label for=\"Select-this-option\" class=\"form-check-label\">Select this option</label><br /><input type=\"radio\" name=\"test\" id=\"Or-select-this-one\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" class=\"form-check-input\" ><label for=\"Or-select-this-one\" class=\"form-check-label\">Or select this one</label><br /></div></div>";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();

            Assert.IsNotNull(sut);
            Assert.AreEqual(DynamicContentTemplateType.Input, sut.TemplateType);
            Assert.AreEqual(15000, sut.TemplateSortOrder);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AssemblyNameValid_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();

            Assert.IsTrue(sut.AssemblyQualifiedName.StartsWith("DynamicContent.Plugin.Templates.FormRadioGroupTemplate, DynamicContent.Plugin, Version="));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditorAction_ReturnsEmptyControllerName_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();

            Assert.AreEqual("/DynamicContent/FormControlTemplateEditorRadioGroup/", sut.EditorAction);
            Assert.AreEqual("", sut.EditorInstructions);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Name_ReturnsValidValidName_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();

            Assert.AreEqual("Radio Button", sut.Name);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SortOrder_ReturnsDefaultSortOrder_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();

            Assert.AreEqual(0, sut.SortOrder);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SortOrder_RemembersNewValue_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();
            sut.SortOrder = 123;

            Assert.AreEqual(123, sut.SortOrder);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void HeightType_ReturnsAutomatic_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();

            Assert.AreEqual(DynamicContentHeightType.Automatic, sut.HeightType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void HeightType_SettingOtherValueReturnsAutomatic_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();
            sut.HeightType = DynamicContentHeightType.Percentage;

            Assert.AreEqual(DynamicContentHeightType.Automatic, sut.HeightType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Height_ReturnsDefaultValue_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();

            Assert.AreEqual(-1, sut.Height);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Height_SettingOtherValueReturnsDefaultHeight_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();
            sut.Height = 100;

            Assert.AreEqual(-1, sut.Height);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void WidthType_ReturnsAutomatic_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();

            Assert.AreEqual(DynamicContentWidthType.Columns, sut.WidthType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void WidthType_SettingOtherValueReturnsAutomatic_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();
            sut.WidthType = DynamicContentWidthType.Percentage;

            Assert.AreEqual(DynamicContentWidthType.Percentage, sut.WidthType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Width_ReturnsDefaultValue_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();

            Assert.AreEqual(12, sut.Width);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Width_SettingOtherValueReturnsDefaultWidth_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();
            sut.Width = 100;

            Assert.AreEqual(100, sut.Width);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Data_DefaultValue_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();

            Assert.IsNull(sut.Data);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Data_SetValue_WithLabel_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 10;
            sut.Data = "test|||||Option 1;Option 2;Option 3";

            string content = sut.Content();

            Assert.AreEqual(BlankDataDefaultValues10Columns, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ActiveFrom_DefaultValue_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();

            Assert.AreEqual(DefaultActiveFrom, sut.ActiveFrom);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ActiveTo_DefaultValue_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();

            Assert.AreEqual(DefaultActiveTo, sut.ActiveTo);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypeColumn_Valid()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 8;
            sut.Data = "test|||||Option A;Option B";

            string content = sut.Content();

            Assert.AreEqual(BlankDataDefaultValues8ColumnsNoLabel, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditorContent_WidthTypeColumn_WithoutLabel_Valid()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 8;
            sut.Data = "test|||||Option X;Option Y;Option Z";

            string content = sut.EditorContent();

            Assert.AreEqual(BlankDataDefaultValues12ColumnsEditor, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditorContent_WidthTypeColumn_WithLabel_Valid()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 8;
            sut.Data = "test|||||   Option X;  Option Y;     Option Z";

            string content = sut.EditorContent();

            Assert.AreEqual(BlankDataDefaultValues12ColumnsEditor, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypeColumn_WithoutLabel_Valid()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 8;
            sut.Data = "test|||||Option A;Option B;;;;";

            string content = sut.Content();

            Assert.AreEqual(BlankDataDefaultValues8ColumnsNoLabel, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypeColumn_WithMultiplBlankOptions_Valid()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 8;
            sut.Data = "test|||||Select this;or this";

            string content = sut.Content();

            Assert.AreEqual(BlankDataDefaultValues8Columns, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypePercentage_WithMultipleEmptyOptions_Valid()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();
            sut.Data = "test|||||Select this option;Or select this one;;;;;;";
            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = 40;

            string content = sut.Content();

            Assert.AreEqual(DefaultData40PercentNoLabel, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypePercentage_WithLabel_Valid()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();
            sut.Data = "test|||||Select this option;Or select this one";
            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = 40;

            string content = sut.Content();

            Assert.AreEqual(DefaultData40Percent, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Clone_EmptyUniqueId_ContainsGuid_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();

            DynamicContentTemplate clone = sut.Clone(String.Empty);

            Assert.IsNotNull(clone);
            Assert.IsInstanceOfType(clone, typeof(FormRadioGroupTemplate));
            bool guidParsed = Guid.TryParse(clone.UniqueId, out Guid uniqueId);
            Assert.IsTrue(guidParsed);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Clone_NullUniqueId_ContainsGuid_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();

            DynamicContentTemplate clone = sut.Clone(null);

            Assert.IsNotNull(clone);
            Assert.IsInstanceOfType(clone, typeof(FormRadioGroupTemplate));
            bool guidParsed = Guid.TryParse(clone.UniqueId, out Guid uniqueId);
            Assert.IsTrue(guidParsed);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Clone_ValidUniqueId_ContainsGuid_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();

            DynamicContentTemplate clone = sut.Clone("my-unique-id");

            Assert.IsNotNull(clone);
            Assert.IsInstanceOfType(clone, typeof(FormRadioGroupTemplate));
            bool guidParsed = Guid.TryParse(clone.UniqueId, out Guid uniqueId);
            Assert.IsFalse(guidParsed);

            Assert.AreEqual("my-unique-id", clone.UniqueId);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_ContainsCssStyleAndClass_WithLineBreak_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 10;
            sut.CssClassName = "css-class";
            sut.CssStyle = "border: 1px solid green;";
            sut.Data = "textbox1||true|||Fill in the data";

            string content = sut.Content();

            Assert.AreEqual(FormWithLabelGreenBorderLineBreak, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_ContainsCssStyleAndClass_WithoutLineBreak_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 10;
            sut.CssClassName = "css-class";
            sut.CssStyle = "border: 1px solid green;";
            sut.Data = "textbox1||false|||Option 1;Option 2";

            string content = sut.Content();

            Assert.AreEqual(FormWithLabelGreenBorder, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_ContainsControlAndLabelStyles_WithoutLineBreak_Success()
        {
            FormRadioGroupTemplate sut = new FormRadioGroupTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 10;
            sut.CssClassName = "css-class";
            sut.CssStyle = "border: 1px solid green;";
            sut.Data = "textbox1||false|border: 1px solid green;|width: 70%;|Fill in the data";

            string content = sut.Content();

            Assert.AreEqual(FormWithLabelGreenBorderControlStyles, content);
        }
    }
}