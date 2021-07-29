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
 *  File: DynamicContentPageTests.cs
 *
 *  Purpose:  Tests for DynamicContentPage
 *
 *  Date        Name                Reason
 *  08/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using DynamicContent.Plugin.Templates;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.DynamicContent;

namespace AspNetCore.PluginManager.Tests.Plugins.DynamicContentTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DynamicContentPageTests
    {
        private const string TestCategoryName = "Dynamic Content";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            DynamicContentPage sut = new DynamicContentPage();
            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Content);
            Assert.AreEqual("", sut.Name);
            Assert.AreEqual("", sut.RouteName);
            Assert.AreEqual(0, sut.Id);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstanceWithPropertiesSet_Success()
        {
            DynamicContentPage sut = new DynamicContentPage()
            {
                Id = 123,
                Name = "My test page"
            };

            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Content);
            Assert.IsNotNull(sut.Name);
            Assert.AreEqual("", sut.RouteName);
            Assert.AreEqual(123, sut.Id);
            Assert.AreEqual("My test page", sut.Name);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_ValidBackgroundImageAndColor_Success()
        {
            DynamicContentPage sut = new DynamicContentPage();
            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Content);
            Assert.AreEqual("", sut.Name);
            Assert.AreEqual("", sut.RouteName);
            Assert.AreEqual(DateTime.MinValue, sut.ActiveFrom);
            Assert.AreEqual(DateTime.MaxValue, sut.ActiveTo);
            Assert.AreEqual("#FFFFFF", sut.BackgroundColor);
            Assert.IsNull(sut.BackgroundImage);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddContentTemplate_InvalidParamDynamicContentTemplateNull_Throws_ArgumentNullException()
        {
            DynamicContentPage sut = new DynamicContentPage()
            {
                Id = 123,
                Name = "My test page"
            };

            sut.AddContentTemplate(null, null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AddContentTemplate_BeforeControlIdNull_Success()
        {
            DynamicContentPage sut = new DynamicContentPage()
            {
                Id = 123,
                Name = "My test page"
            };

            SpacerTemplate spacerTemplate = new SpacerTemplate();

            sut.AddContentTemplate(spacerTemplate, null);
            Assert.AreEqual(1, sut.Content.Count);
            Assert.AreNotEqual(spacerTemplate, sut.Content[0]);
            Assert.AreNotEqual(spacerTemplate.UniqueId, sut.Content[0].UniqueId);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AddContentTemplate_BeforeControlIdValid_Success()
        {
            DynamicContentPage sut = new DynamicContentPage()
            {
                Id = 123,
                Name = "My test page"
            };

            SpacerTemplate spacerTemplate = new SpacerTemplate();

            sut.AddContentTemplate(spacerTemplate, null);
            Assert.AreEqual(1, sut.Content.Count);
            Assert.AreNotEqual(spacerTemplate, sut.Content[0]);
            Assert.AreNotEqual(spacerTemplate.UniqueId, sut.Content[0].UniqueId);

            sut.AddContentTemplate(spacerTemplate, "");
            Assert.AreEqual(2, sut.Content.Count);
            Assert.AreNotEqual(spacerTemplate, sut.Content[0]);
            Assert.AreNotEqual(spacerTemplate.UniqueId, sut.Content[0].UniqueId);
            Assert.AreNotEqual(spacerTemplate, sut.Content[1]);
            Assert.AreNotEqual(spacerTemplate.UniqueId, sut.Content[1].UniqueId);
            Assert.AreEqual("Control-1", sut.Content[0].UniqueId);
            Assert.AreEqual("Control-2", sut.Content[1].UniqueId);

            sut.AddContentTemplate(spacerTemplate, "Control-1");
            Assert.AreEqual(3, sut.Content.Count);
            Assert.AreNotEqual(spacerTemplate, sut.Content[0]);
            Assert.AreNotEqual(spacerTemplate.UniqueId, sut.Content[0].UniqueId);
            Assert.AreNotEqual(spacerTemplate, sut.Content[1]);
            Assert.AreNotEqual(spacerTemplate.UniqueId, sut.Content[1].UniqueId);
            Assert.AreNotEqual(spacerTemplate, sut.Content[2]);
            Assert.AreNotEqual(spacerTemplate.UniqueId, sut.Content[2].UniqueId);

            Assert.AreEqual("Control-3", sut.Content[0].UniqueId);
            Assert.AreEqual("Control-1", sut.Content[1].UniqueId);
            Assert.AreEqual("Control-2", sut.Content[2].UniqueId);
        }
    }
}
