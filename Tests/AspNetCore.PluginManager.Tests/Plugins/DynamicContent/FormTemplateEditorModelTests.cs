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
 *  File: HtmlTextTemplateTests.cs
 *
 *  Purpose:  Tests for html text template
 *
 *  Date        Name                Reason
 *  08/07/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using DynamicContent.Plugin.Model;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;

using SharedPluginFeatures;
using SharedPluginFeatures.DynamicContent;

namespace AspNetCore.PluginManager.Tests.Plugins.DynamicContentTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class FormTemplateEditorModelTests
    {
        private const string TestsCategory = "Dynamic Content";

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void Construct_NullData_ContainsDefaultValues()
        {
            FormTemplateEditorModel sut = new FormTemplateEditorModel(null);

            Assert.AreEqual("Align label to the left", sut.AlignLeftText);
            Assert.AreEqual("|", sut.Data);
            Assert.AreEqual("", sut.ControlName);
            Assert.AreEqual("", sut.LabelText);
            Assert.IsFalse(sut.AlignTop);
            Assert.IsTrue(sut.AutoWidth);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void Construct_NoSeperators_ContainsDefaultValues()
        {
            FormTemplateEditorModel sut = new FormTemplateEditorModel("data");

            Assert.AreEqual("data", sut.Data);
            Assert.AreEqual("data", sut.ControlName);
            Assert.AreEqual("", sut.LabelText);
            Assert.IsFalse(sut.AlignTop);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void Construct_InvalidAlignTopValue_ContainsDefaultValues()
        {
            FormTemplateEditorModel sut = new FormTemplateEditorModel("data|label|alignTop");

            Assert.AreEqual("data|label|alignTop", sut.Data);
            Assert.AreEqual("data", sut.ControlName);
            Assert.AreEqual("label", sut.LabelText);
            Assert.IsFalse(sut.AlignTop);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void Construct_ValidAlignTopWrongCase_Success()
        {
            FormTemplateEditorModel sut = new FormTemplateEditorModel("data|label|TRuE");

            Assert.AreEqual("data|label|TRuE", sut.Data);
            Assert.AreEqual("data", sut.ControlName);
            Assert.AreEqual("label", sut.LabelText);
            Assert.IsTrue(sut.AlignTop);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void Construct_SetDataAfterConstruction_Success()
        {
            FormTemplateEditorModel sut = new FormTemplateEditorModel(null);
            sut.Data = "data|label|TRuE";

            Assert.AreEqual("data|label|TRuE", sut.Data);
            Assert.AreEqual("data", sut.ControlName);
            Assert.AreEqual("label", sut.LabelText);
            Assert.AreEqual(String.Empty, sut.ControlStyle);
            Assert.AreEqual(String.Empty, sut.LabelStyle);
            Assert.IsTrue(sut.AlignTop);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void Construct_SetDataAfterConstruction_WithControlStyles_Success()
        {
            FormTemplateEditorModel sut = new FormTemplateEditorModel(null);
            sut.Data = "data|label|TRuE|border: 1px solid blue;|width: 70%;";

            Assert.AreEqual("data|label|TRuE|border: 1px solid blue;|width: 70%;", sut.Data);
            Assert.AreEqual("data", sut.ControlName);
            Assert.AreEqual("label", sut.LabelText);
            Assert.AreEqual("border: 1px solid blue;", sut.LabelStyle);
            Assert.AreEqual("width: 70%;", sut.ControlStyle);

            Assert.IsTrue(sut.AlignTop);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void Construct_AllowAlignRight_Success()
        {
            FormTemplateEditorModel sut = new FormTemplateEditorModel(null, true);
            sut.Data = "data|label|TRuE|border: 1px solid blue;|width: 70%;";

            Assert.AreEqual("data|label|TRuE|border: 1px solid blue;|width: 70%;", sut.Data);
            Assert.AreEqual("data", sut.ControlName);
            Assert.AreEqual("label", sut.LabelText);
            Assert.AreEqual("border: 1px solid blue;", sut.LabelStyle);
            Assert.AreEqual("width: 70%;", sut.ControlStyle);
            Assert.AreEqual("Align label to the right", sut.AlignLeftText);
            Assert.IsFalse(sut.AutoWidth);
            Assert.IsTrue(sut.AlignTop);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void Construct_WithOptions_Success()
        {
            FormTemplateEditorModel sut = new FormTemplateEditorModel(null, true);
            sut.Data = "data|label|TRuE|border: 1px solid blue;|width: 70%;|Option 1;Option 2;Option 3";

            Assert.AreEqual("data|label|TRuE|border: 1px solid blue;|width: 70%;|Option 1;Option 2;Option 3", sut.Data);
            Assert.AreEqual("data", sut.ControlName);
            Assert.AreEqual("label", sut.LabelText);
            Assert.AreEqual("border: 1px solid blue;", sut.LabelStyle);
            Assert.AreEqual("width: 70%;", sut.ControlStyle);
            Assert.AreEqual("Align label to the right", sut.AlignLeftText);
            Assert.IsFalse(sut.AutoWidth);
            Assert.IsTrue(sut.AlignTop);
            Assert.AreEqual(3, sut.Options.Length);
            Assert.AreEqual("Option 1", sut.Options[0]);
            Assert.AreEqual("Option 2", sut.Options[1]);
            Assert.AreEqual("Option 3", sut.Options[2]);
        }
    }
}
