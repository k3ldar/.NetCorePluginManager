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
 *  File: FormListBoxTemplateTests.cs
 *
 *  Purpose:  Tests for html list box template
 *
 *  Date        Name                Reason
 *  17/07/2021  Simon Carter        Initially Created
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
    public class FormListBoxTemplateTests : GenericBaseClass
    {
        private const string TestCategoryName = "Dynamic Content";
        private const string BlankDataDefaultValues12ColumnsEditor = "<div class=\"col-sm-12\"><div class=\"form-group\" style=\"margin: 0 0 0 10px;min-height:32px;\"><select name=\"test\" id=\"test\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" disabled><option value=\"Option-X\" selected>Option X</option><option value=\"Option-Y\">Option Y</option><option value=\"Option-Z\">Option Z</option></select></div></div>";
        private const string BlankDataDefaultValues10Columns = "<div class=\"col-sm-10\"><div class=\"form-group\"><label for=\"test\">Select One of these!</label><select name=\"test\" id=\"test\" onclick=\"updateUC();\" onfocusout=\"updateUC();\"><option value=\"Option-1\" selected>Option 1</option><option value=\"Option-2\">Option 2</option><option value=\"Option-3\">Option 3</option></select></div></div>";
        private const string BlankDataDefaultValues8Columns = "<div class=\"col-sm-8\"><div class=\"form-group\"><label for=\"test\">Select an option:</label><select name=\"test\" id=\"test\" onclick=\"updateUC();\" onfocusout=\"updateUC();\"><option value=\"Select-this\" selected>Select this</option><option value=\"or-this\">or this</option></select></div></div>";
        private const string BlankDataDefaultValues8ColumnsNoLabel = "<div class=\"col-sm-8\"><div class=\"form-group\"><select name=\"test\" id=\"test\" onclick=\"updateUC();\" onfocusout=\"updateUC();\"><option value=\"Option-A\" selected>Option A</option><option value=\"Option-B\">Option B</option></select></div></div>";
        private const string FormWithLabelGreenBorderLineBreak = "<div class=\"col-sm-10\"><div class=\"form-group css-class\" style=\"border: 1px solid green;\"><label for=\"textbox1\">Select an option:</label><br /><select name=\"textbox1\" id=\"textbox1\" onclick=\"updateUC();\" onfocusout=\"updateUC();\"><option value=\"Option-A\" selected>Option A</option><option value=\"Option-B\">Option B</option></select></div></div>";
        private const string FormWithLabelGreenBorder = "<div class=\"col-sm-10\"><div class=\"form-group css-class\" style=\"border: 1px solid green;\"><label for=\"textbox1\">Select an option:</label><select name=\"textbox1\" id=\"textbox1\" onclick=\"updateUC();\" onfocusout=\"updateUC();\"><option value=\"Option-1\" selected>Option 1</option><option value=\"Option-2\">Option 2</option></select></div></div>";
        private const string FormWithLabelGreenBorderControlStyles = "<div class=\"col-sm-10\"><div class=\"form-group css-class\" style=\"border: 1px solid green;\"><label for=\"textbox1\" style=\"border: 1px solid green;\">Select the option</label><select name=\"textbox1\" id=\"textbox1\" onclick=\"updateUC();\" onfocusout=\"updateUC();\" style=\"width: 70%;\"><option value=\"Option-1\" selected>Option 1</option><option value=\"Option-2\">Option 2</option><option value=\"Option-3\">Option 3</option></select></div></div>";
        private const string DefaultData40Percent = "<div style=\"width:40% !important;display:block;\"><div class=\"form-group\"><label for=\"test\">Select an option:</label><select name=\"test\" id=\"test\" onclick=\"updateUC();\" onfocusout=\"updateUC();\"><option value=\"Select-this-option\" selected>Select this option</option><option value=\"Or-select-this-one\">Or select this one</option></select></div></div>";
        private const string DefaultData40PercentNoLabel = "<div style=\"width:40% !important;display:block;\"><div class=\"form-group\"><select name=\"test\" id=\"test\" onclick=\"updateUC();\" onfocusout=\"updateUC();\"><option value=\"Select-this-option\" selected>Select this option</option><option value=\"Or-select-this-one\">Or select this one</option></select></div></div>";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();

            Assert.IsNotNull(sut);
            Assert.AreEqual(DynamicContentTemplateType.Input, sut.TemplateType);
            Assert.AreEqual(15000, sut.TemplateSortOrder);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AssemblyNameValid_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();

            Assert.IsTrue(sut.AssemblyQualifiedName.StartsWith("DynamicContent.Plugin.Templates.FormListBoxTemplate, DynamicContent.Plugin, Version="));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditorAction_ReturnsEmptyControllerName_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();

            Assert.AreEqual("/DynamicContent/FormControlTemplateEditorListBox/", sut.EditorAction);
            Assert.AreEqual("", sut.EditorInstructions);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Name_ReturnsValidValidName_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();

            Assert.AreEqual("List Box", sut.Name);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SortOrder_ReturnsDefaultSortOrder_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();

            Assert.AreEqual(0, sut.SortOrder);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SortOrder_RemembersNewValue_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();
            sut.SortOrder = 123;

            Assert.AreEqual(123, sut.SortOrder);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void HeightType_ReturnsAutomatic_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();

            Assert.AreEqual(DynamicContentHeightType.Automatic, sut.HeightType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void HeightType_SettingOtherValueReturnsAutomatic_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();
            sut.HeightType = DynamicContentHeightType.Percentage;

            Assert.AreEqual(DynamicContentHeightType.Automatic, sut.HeightType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Height_ReturnsDefaultValue_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();

            Assert.AreEqual(-1, sut.Height);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Height_SettingOtherValueReturnsDefaultHeight_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();
            sut.Height = 100;

            Assert.AreEqual(-1, sut.Height);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void WidthType_ReturnsAutomatic_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();

            Assert.AreEqual(DynamicContentWidthType.Columns, sut.WidthType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void WidthType_SettingOtherValueReturnsAutomatic_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();
            sut.WidthType = DynamicContentWidthType.Percentage;

            Assert.AreEqual(DynamicContentWidthType.Percentage, sut.WidthType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Width_ReturnsDefaultValue_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();

            Assert.AreEqual(12, sut.Width);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Width_SettingOtherValueReturnsDefaultWidth_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();
            sut.Width = 100;

            Assert.AreEqual(100, sut.Width);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Data_DefaultValue_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();

            Assert.IsNull(sut.Data);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Data_SetValue_WithLabel_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 10;
            sut.Data = "test|Select One of these!||||Option 1;Option 2;Option 3";

            string content = sut.Content();

            Assert.AreEqual(BlankDataDefaultValues10Columns, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ActiveFrom_DefaultValue_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();

            Assert.AreEqual(DefaultActiveFrom, sut.ActiveFrom);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ActiveTo_DefaultValue_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();

            Assert.AreEqual(DefaultActiveTo, sut.ActiveTo);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypeColumn_Valid()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();
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
            FormListBoxTemplate sut = new FormListBoxTemplate();
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
            FormListBoxTemplate sut = new FormListBoxTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 8;
            sut.Data = "test|||||   Option X   ;  Option Y   ;     Option Z   ";

            string content = sut.EditorContent();

            Assert.AreEqual(BlankDataDefaultValues12ColumnsEditor, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypeColumn_WithoutLabel_Valid()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();
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
            FormListBoxTemplate sut = new FormListBoxTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 8;
            sut.Data = "test|Select an option:||||Select this;or this";

            string content = sut.Content();

            Assert.AreEqual(BlankDataDefaultValues8Columns, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_WidthTypePercentage_WithMultipleEmptyOptions_Valid()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();
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
            FormListBoxTemplate sut = new FormListBoxTemplate();
            sut.Data = "test|Select an option:||||Select this option;Or select this one";
            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = 40;

            string content = sut.Content();

            Assert.AreEqual(DefaultData40Percent, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Clone_EmptyUniqueId_ContainsGuid_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();

            DynamicContentTemplate clone = sut.Clone(String.Empty);

            Assert.IsNotNull(clone);
            Assert.IsInstanceOfType(clone, typeof(FormListBoxTemplate));
            bool guidParsed = Guid.TryParse(clone.UniqueId, out Guid uniqueId);
            Assert.IsTrue(guidParsed);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Clone_NullUniqueId_ContainsGuid_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();

            DynamicContentTemplate clone = sut.Clone(null);

            Assert.IsNotNull(clone);
            Assert.IsInstanceOfType(clone, typeof(FormListBoxTemplate));
            bool guidParsed = Guid.TryParse(clone.UniqueId, out Guid uniqueId);
            Assert.IsTrue(guidParsed);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Clone_ValidUniqueId_ContainsGuid_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();

            DynamicContentTemplate clone = sut.Clone("my-unique-id");

            Assert.IsNotNull(clone);
            Assert.IsInstanceOfType(clone, typeof(FormListBoxTemplate));
            bool guidParsed = Guid.TryParse(clone.UniqueId, out Guid uniqueId);
            Assert.IsFalse(guidParsed);

            Assert.AreEqual("my-unique-id", clone.UniqueId);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_ContainsCssStyleAndClass_WithLineBreak_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 10;
            sut.CssClassName = "css-class";
            sut.CssStyle = "border: 1px solid green;";
            sut.Data = "textbox1|Select an option:|true|||Option A;  Option B";

            string content = sut.Content();

            Assert.AreEqual(FormWithLabelGreenBorderLineBreak, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_ContainsCssStyleAndClass_WithoutLineBreak_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 10;
            sut.CssClassName = "css-class";
            sut.CssStyle = "border: 1px solid green;";
            sut.Data = "textbox1|Select an option:||||Option 1;Option 2|false";

            string content = sut.Content();

            Assert.AreEqual(FormWithLabelGreenBorder, content);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Content_ContainsControlAndLabelStyles_WithoutLineBreak_Success()
        {
            FormListBoxTemplate sut = new FormListBoxTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 10;
            sut.CssClassName = "css-class";
            sut.CssStyle = "border: 1px solid green;";
            sut.Data = "textbox1|Select the option|false|border: 1px solid green;|width: 70%;|Option 1;Option 2;Option 3";

            string content = sut.Content();

            Assert.AreEqual(FormWithLabelGreenBorderControlStyles, content);
        }
    }
}
