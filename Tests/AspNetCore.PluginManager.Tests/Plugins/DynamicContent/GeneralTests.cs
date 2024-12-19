/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  File: HtmlTextTemplateTests.cs
 *
 *  Purpose:  Tests for html text template
 *
 *  Date        Name                Reason
 *  27/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using DynamicContent.Plugin.Classes;
using DynamicContent.Plugin.Model;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;

using SharedPluginFeatures;
using SharedPluginFeatures.DynamicContent;

namespace AspNetCore.PluginManager.Tests.Plugins.DynamicContentTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class GeneralTests : GenericBaseClass
    {
        private const string GeneralTestsCategory = "Dynamic Content General Tests";

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void UpdatePositionModel_Construct_ContainsDefaultValues()
        {
            UpdatePositionModel sut = new UpdatePositionModel();

            Assert.IsNull(sut.CacheId);
            Assert.IsNull(sut.ControlId);
            Assert.IsNull(sut.Controls);
            Assert.AreEqual(0, sut.Top);
            Assert.AreEqual(0, sut.Left);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void UpdatePositionModel_Construct_RetainsSetValues()
        {
            UpdatePositionModel sut = new UpdatePositionModel()
            {
                CacheId = "testCache",
                ControlId = "testControlId",
                Controls = new string[] { "one", "two" },
                Top = 53,
                Left = 12
            };

            Assert.AreEqual("testCache", sut.CacheId);
            Assert.AreEqual("testControlId", sut.ControlId);
            Assert.AreEqual(2, sut.Controls.Length);
            Assert.AreEqual("one", sut.Controls[0]);
            Assert.AreEqual("two", sut.Controls[1]);
            Assert.AreEqual(53, sut.Top);
            Assert.AreEqual(12, sut.Left);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void UpdatePositionModel_ValidateJsonPropertyName()
        {
            Assert.IsTrue(PropertyHasJsonAttribute(typeof(UpdatePositionModel), "CacheId", "cacheId"));
            Assert.IsTrue(PropertyHasJsonAttribute(typeof(UpdatePositionModel), "ControlId", "controlId"));
            Assert.IsTrue(PropertyHasJsonAttribute(typeof(UpdatePositionModel), "Controls", "controls"));
            Assert.IsTrue(PropertyHasJsonAttribute(typeof(UpdatePositionModel), "Top", "top"));
            Assert.IsTrue(PropertyHasJsonAttribute(typeof(UpdatePositionModel), "Left", "left"));
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void TemplatsModel_Construct_Valid()
        {
            TemplatesModel sut = new TemplatesModel();

            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Templates);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCategory(GeneralTestsCategory)]
        public void TemplateModel_Construct_InvalidTemplateIdParam_Null_Throws_ArgumentNullException()
        {
            TemplateModel sut = new TemplateModel(null, "template name", "template imaeg");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCategory(GeneralTestsCategory)]
        public void TemplateModel_Construct_InvalidTemplateIdParam_EmptyString_Throws_ArgumentNullException()
        {
            TemplateModel sut = new TemplateModel(String.Empty, "template name", "template image");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCategory(GeneralTestsCategory)]
        public void TemplateModel_Construct_InvalidTemplateNameParam_Null_Throws_ArgumentNullException()
        {
            TemplateModel sut = new TemplateModel("template Id", null, "template image");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCategory(GeneralTestsCategory)]
        public void TemplateModel_Construct_InvalidTemplateNameParam_EmptyString_Throws_ArgumentNullException()
        {
            TemplateModel sut = new TemplateModel("template Id", "template name", String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCategory(GeneralTestsCategory)]
        public void TemplateModel_Construct_InvalidTemplateImageParam_EmptyString_Throws_ArgumentNullException()
        {
            TemplateModel sut = new TemplateModel("template Id", String.Empty, "template image");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCategory(GeneralTestsCategory)]
        public void TemplateModel_Construct_InvalidTemplateImageParam_Null_Throws_ArgumentNullException()
        {
            TemplateModel sut = new TemplateModel("template Id", "template name", null);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void TemplateModel_Construct_Valid_Success()
        {
            TemplateModel sut = new TemplateModel("template Id", "template name", "template image");

            Assert.IsNotNull(sut);

            Assert.AreEqual("template Id", sut.TemplateId);
            Assert.AreEqual("template name", sut.TemplateName);
            Assert.AreEqual("template image", sut.TemplateImage);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void DeleteControlModel_Construct_EmptyConstructor_Success()
        {
            DeleteControlModel sut = new DeleteControlModel();
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCategory(GeneralTestsCategory)]
        public void DeleteControlModel_Construct_InvalidCacheIdParam_Null_Throws_ArgumentNullException()
        {
            DeleteControlModel sut = new DeleteControlModel(null, "control id");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCategory(GeneralTestsCategory)]
        public void DeleteControlModel_Construct_InvalidCacheIdParam_EmptyString_Throws_ArgumentNullException()
        {
            DeleteControlModel sut = new DeleteControlModel(String.Empty, "control id");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCategory(GeneralTestsCategory)]
        public void DeleteControlModel_Construct_InvalidControlIdParam_Null_Throws_ArgumentNullException()
        {
            DeleteControlModel sut = new DeleteControlModel("cache id", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCategory(GeneralTestsCategory)]
        public void DeleteControlModel_Construct_InvalidControlIdParam_EmptyString_Throws_ArgumentNullException()
        {
            DeleteControlModel sut = new DeleteControlModel("cache id", String.Empty);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void DeleteControlModel_Construct_Valid_Success()
        {
            DeleteControlModel sut = new DeleteControlModel("cache Id", "Control Id");
            Assert.IsNotNull(sut);
            Assert.AreEqual("cache Id", sut.CacheId);
            Assert.AreEqual("Control Id", sut.ControlId);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void TextTemplateEditorModel_Construct_Valid_Success()
        {
            TextTemplateEditorModel sut = new TextTemplateEditorModel();
            Assert.IsNotNull(sut);
            Assert.IsNull(sut.Data);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void TextTemplateEditorModel_ConstructWithNullParam_Valid_Success()
        {
            TextTemplateEditorModel sut = new TextTemplateEditorModel(null);
            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Data);
            Assert.AreEqual("", sut.Data);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void DynamicContentModel_Construct_DefaultConstructor_Valid_Success()
        {
            JsonResponseModel sut = new JsonResponseModel();
            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.ResponseData);
            Assert.AreEqual("", sut.ResponseData);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void DynamicContentModel_Construct_SuccessParamTrue_Valid_Success()
        {
            JsonResponseModel sut = new JsonResponseModel(true);
            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.ResponseData);
            Assert.AreEqual("", sut.ResponseData);
            Assert.IsTrue(sut.Success);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void DynamicContentModel_Construct_SuccessParamFalse_Valid_Success()
        {
            JsonResponseModel sut = new JsonResponseModel(false);
            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.ResponseData);
            Assert.AreEqual("", sut.ResponseData);
            Assert.IsFalse(sut.Success);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCategory(GeneralTestsCategory)]
        public void DynamicContentModel_Construct_InvalidData_Null_Throws_ArgumentNullException()
        {
            JsonResponseModel sut = new JsonResponseModel(null);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void DynamicContentModel_Construct_Data_Success()
        {
            JsonResponseModel sut = new JsonResponseModel("test data");
            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.Success);
            Assert.IsNotNull(sut.ResponseData);
            Assert.AreEqual("test data", sut.ResponseData);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void AddControlModel_Construct_Success()
        {
            AddControlModel sut = new AddControlModel();

            Assert.IsNull(sut.CacheId);
            Assert.IsNull(sut.TemplateId);
            Assert.IsNull(sut.NextControl);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void EditTemplateModel_Construct_PropertiesContainDefaultValues_Success()
        {
            EditTemplateModel sut = new EditTemplateModel();
            Assert.IsNotNull(sut);
            Assert.IsNull(sut.CacheId);
            Assert.IsNull(sut.UniqueId);
            Assert.IsNull(sut.TemplateEditor);
            Assert.IsNull(sut.Name);
            Assert.AreEqual(0, sut.SortOrder);
            Assert.AreEqual(DynamicContentWidthType.Columns, sut.WidthType);
            Assert.AreEqual(0, sut.Width);
            Assert.AreEqual(DynamicContentHeightType.Pixels, sut.HeightType);
            Assert.AreEqual(0, sut.Height);
            Assert.IsNull(sut.Data);
            Assert.AreEqual(DefaultActiveFrom, sut.ActiveFrom);
            Assert.AreEqual(DefaultActiveTo, sut.ActiveTo);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void EditTemplateModel_Construct_SetProperties_Success()
        {
            DateTime activeFromDateTime = DateTime.Now;

            EditTemplateModel sut = new EditTemplateModel()
            {
                CacheId = "cache1",
                UniqueId = "unique1",
                TemplateEditor = "template1",
                EditorInstructions = "please enter a value",
                Name = "name",
                SortOrder = 23,
                WidthType = DynamicContentWidthType.Percentage,
                Width = 45,
                HeightType = DynamicContentHeightType.Automatic,
                Height = 123,
                Data = "test data",
                ActiveFrom = activeFromDateTime
            };

            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.CacheId);
            Assert.AreEqual("cache1", sut.CacheId);
            Assert.IsNotNull(sut.UniqueId);
            Assert.AreEqual("unique1", sut.UniqueId);
            Assert.IsNotNull(sut.TemplateEditor);
            Assert.AreEqual("template1", sut.TemplateEditor);
            Assert.AreEqual("please enter a value", sut.EditorInstructions);
            Assert.IsNotNull(sut.Name);
            Assert.AreEqual("name", sut.Name);
            Assert.AreEqual(23, sut.SortOrder);
            Assert.AreEqual(DynamicContentWidthType.Percentage, sut.WidthType);
            Assert.AreEqual(45, sut.Width);
            Assert.AreEqual(DynamicContentHeightType.Automatic, sut.HeightType);
            Assert.AreEqual(123, sut.Height);
            Assert.IsNotNull(sut.Data);
            Assert.AreEqual("test data", sut.Data);
            Assert.AreEqual(activeFromDateTime, sut.ActiveFrom);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PageModel_Construct_InvalidParam_ModelData_Null_Throws_ArgumentNullException()
        {
            PageModel sut = new PageModel(null, "path", "content", null, null, true);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PageModel_Construct_InvalidParam_Content_Null_Throws_ArgumentNullException()
        {
            PageModel sut = new PageModel(GenerateTestBaseModelData(), "path", null, null, null, true);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void PageModel_Construct_ValidParams_SetsEmptyContent_Success()
        {
            PageModel sut = new PageModel(GenerateTestBaseModelData(), "path", String.Empty, null, null, true);
            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Content);
            Assert.AreEqual("", sut.Content);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PageModel_Construct_InvalidParam_Path_Null_Throws_ArgumentNullException()
        {
            new PageModel(GenerateTestBaseModelData(), null, "this is the content", null, null, true);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PageModel_Construct_InvalidParam_Path_EmptyString_Throws_ArgumentNullException()
        {
            new PageModel(GenerateTestBaseModelData(), "", "this is the content", null, null, true);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void PageModel_Construct_ValidParams_SetsContent_Success()
        {
            PageModel sut = new PageModel(GenerateTestBaseModelData(), "path", "this is the content", null, null, true);
            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Content);
            Assert.AreEqual("this is the content", sut.Content);
            Assert.IsTrue(sut.HasDataSaved);
            Assert.AreEqual("path", sut.Path);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void EditPageModel_Construct_EmptyParameter_Success()
        {
            EditPageModel sut = new EditPageModel();
            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.DynamicContents);
            Assert.AreEqual(0, sut.DynamicContents.Count);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EditPageModel_Construct_InvalidBaseModelDataParam_Null_Throws_ArgumentNullException()
        {
            new EditPageModel(null, "cache id", 1, "name", "name", DateTime.MinValue, DateTime.MaxValue, new List<DynamicContentTemplate>(), "#ffffff", "");
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EditPageModel_Construct_InvalidCacheIdParam_Null_Throws_ArgumentNullException()
        {
            new EditPageModel(GenerateTestBaseModelData(), null, 1, "name", "name", DateTime.MinValue, DateTime.MaxValue, new List<DynamicContentTemplate>(), "#ffffff", "");
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EditPageModel_Construct_InvalidCacheIdParam_EmptyString_Throws_ArgumentNullException()
        {
            new EditPageModel(GenerateTestBaseModelData(), "", 1, "name", "name", DateTime.MinValue, DateTime.MaxValue, new List<DynamicContentTemplate>(), "#ffffff", "");
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EditPageModel_Construct_InvalidNameParam_Null_Throws_ArgumentNullException()
        {
            new EditPageModel(GenerateTestBaseModelData(), "cache id", 1, null, "name", DateTime.MinValue, DateTime.MaxValue, new List<DynamicContentTemplate>(), "#ffffff", "");
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EditPageModel_Construct_InvalidNameParam_EmptyString_Throws_ArgumentNullException()
        {
            new EditPageModel(GenerateTestBaseModelData(), "cache id", 1, "", "name", DateTime.MinValue, DateTime.MaxValue, new List<DynamicContentTemplate>(), "#ffffff", "");
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EditPageModel_Construct_InvalidDynamicContentsParam_Null_Throws_ArgumentNullException()
        {
            new EditPageModel(GenerateTestBaseModelData(), "cache id", 1, "name", "name", DateTime.MinValue, DateTime.MaxValue, null, "#ffffff", "");
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void EditPageModel_Construct_ValidInstance_Success()
        {
            List<DynamicContentTemplate> dynamicContents = new List<DynamicContentTemplate>();
            dynamicContents.Add(new MockDynamicTemplate());

            EditPageModel sut = new EditPageModel(GenerateTestBaseModelData(), "cache id", 123, "the name", "name", DateTime.MinValue, DateTime.MaxValue, dynamicContents, "#ffffff", "/img/img.gif");
            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.DynamicContents);
            Assert.AreEqual(1, sut.DynamicContents.Count);
            Assert.AreEqual("cache id", sut.CacheId);
            Assert.AreEqual("the name", sut.Name);
            Assert.AreEqual(123, sut.Id);
            Assert.AreEqual("name", sut.RouteName);
            Assert.AreEqual(DateTime.MinValue, sut.ActiveFrom);
            Assert.AreEqual(DateTime.MaxValue, sut.ActiveTo);
            Assert.AreEqual("#ffffff", sut.BackgroundColor);
            Assert.AreEqual("/img/img.gif", sut.BackgroundImage);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void CustomPagesModel_Construct_DefaultConstructor_Success()
        {
            CustomPagesModel sut = new CustomPagesModel();
            Assert.IsNotNull(sut);
            Assert.IsNull(sut.CustomPages);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CustomPagesModel_Construct_InvalidParam_ModelData_Null_Throws_ArgumentNullException()
        {
            new CustomPagesModel(null, new List<LookupListItem>());
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CustomPagesModel_Construct_InvalidParam_CustomPages_Null_Throws_ArgumentNullException()
        {
            new CustomPagesModel(GenerateTestBaseModelData(), null);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void CustomPagesModel_Construct_ValidInstanceWithCustomPages()
        {
            List<LookupListItem> customPages = new List<LookupListItem>();
            customPages.Add(new LookupListItem(321, "test"));

            CustomPagesModel sut = new CustomPagesModel(GenerateTestBaseModelData(), customPages);
            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.CustomPages);
            Assert.AreEqual(1, sut.CustomPages.Count);
            Assert.AreEqual("test", sut.CustomPages[0].Name);
            Assert.AreEqual(321, sut.CustomPages[0].Id);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ApplicationOverrides_InvalidParam_VariableName_Null_Throws_ArgumentNullException()
        {
            ApplicationOverrides sut = new ApplicationOverrides();

            object value = null;
            sut.ExpandApplicationVariable(null, ref value);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ApplicationOverrides_InvalidParam_VariableName_EmptyString_Throws_ArgumentNullException()
        {
            ApplicationOverrides sut = new ApplicationOverrides();

            object value = null;
            sut.ExpandApplicationVariable(null, ref value);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void ApplicationOverrides_ValidateRootPath_ReturnsTrue()
        {
            ApplicationOverrides sut = new ApplicationOverrides();

            object value = null;
            bool success = sut.ExpandApplicationVariable("RootPath", ref value);

            Assert.IsTrue(success);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void ApplicationOverrides_ValidateUnknownValue_ReturnsFalse()
        {
            ApplicationOverrides sut = new ApplicationOverrides();

            object value = null;
            bool success = sut.ExpandApplicationVariable("Unknown", ref value);

            Assert.IsFalse(success);
        }
    }
}
