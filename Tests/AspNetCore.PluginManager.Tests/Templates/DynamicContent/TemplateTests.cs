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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: TemplateTests.cs
 *
 *  Purpose:  Template tests
 *
 *  Date        Name                Reason
 *  01/10/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;

using DynamicContent.Plugin.Templates;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Templates.DynamicContent
{
    [TestClass]
    public class TemplateTests
    {
        #region HtmlTextTemplate Tests

        [TestMethod]
        public void HtmlTextTemplate_ValidateTypeName()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            Assert.IsTrue(sut.AssemblyQualifiedName.StartsWith("DynamicContent.Plugin.Templates.HtmlTextTemplate"));
        }

        [TestMethod]
        public void HtmlTextTemplate_Validate_Name()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            Assert.AreEqual("Html Content", sut.Name);
        }

        [TestMethod]
        public void HtmlTextTemplate_Validate_EditorAction()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            Assert.AreEqual("/DynamicContent/TextTemplateEditor/", sut.EditorAction);
        }

        [TestMethod]
        public void HtmlTextTemplate_Validate_DefaultWidth_12Columns()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            Assert.AreEqual(12, sut.Width);
        }

        [TestMethod]
        public void HtmlTextTemplate_Validate_DefaultWidthType_Columns()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            Assert.AreEqual(DynamicContentWidthType.Columns, sut.WidthType);
        }

        [TestMethod]
        public void HtmlTextTemplate_Validate_DefaultHeight_MinusOne_AutomaticallyExpandToFullHeight()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            Assert.AreEqual(-1, sut.Height);
        }

        [TestMethod]
        public void HtmlTextTemplate_Validate_DefaultHeightType_Automatic()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            Assert.AreEqual(DynamicContentHeightType.Automatic, sut.HeightType);
        }

        [TestMethod]
        public void HtmlTextTemplate_ChangeHeight_FailsSilently()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            sut.Height = 528;

            Assert.AreEqual(-1, sut.Height);
        }

        [TestMethod]
        public void HtmlTextTemplate_ChangeHeightType_FailsSilently()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            sut.HeightType = DynamicContentHeightType.Pixels;

            Assert.AreEqual(DynamicContentHeightType.Automatic, sut.HeightType);
        }

        [TestMethod]
        public void HtmlTextTemplate_ChangeWidth_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            sut.Width = 528;

            Assert.AreEqual(528, sut.Width);
        }

        [TestMethod]
        public void HtmlTextTemplate_ChangeWidthType_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            sut.WidthType = DynamicContentWidthType.Pixels;

            Assert.AreEqual(DynamicContentWidthType.Pixels, sut.WidthType);
        }

        [TestMethod]
        public void HtmlTextTemplate_Validate_DataIsRemembered()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            sut.Data = "Test Data";

            Assert.AreEqual("Test Data", sut.Data);
        }

        [TestMethod]
        public void HtmlTextTemplate_ValidateConstructInstance_FromTypeName()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            string typeName = sut.AssemblyQualifiedName;

            Type type = Type.GetType(typeName);
            HtmlTextTemplate newInstance = (HtmlTextTemplate)Activator.CreateInstance(type);

            Assert.IsNotNull(newInstance);
            Assert.IsTrue(newInstance.AssemblyQualifiedName.StartsWith("DynamicContent.Plugin.Templates.HtmlTextTemplate"));
        }

        [TestMethod]
        public void HtmlTextTemplate_SerializeAndDeserializeInstance_Success()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = 57;
            sut.Data = "Just some test data {:}";
            sut.SortOrder = -5214;

            string serializedClass = JsonConvert.SerializeObject(sut);

            Type type = Type.GetType(sut.AssemblyQualifiedName);


            HtmlTextTemplate newInstance = (HtmlTextTemplate)JsonConvert.DeserializeObject(serializedClass, type);

            Assert.AreEqual(-5214, newInstance.SortOrder);
            Assert.AreEqual(DynamicContentWidthType.Percentage, newInstance.WidthType);
            Assert.AreEqual(57, newInstance.Width);
            Assert.AreEqual("Just some test data {:}", newInstance.Data);
            Assert.IsTrue(newInstance.AssemblyQualifiedName.StartsWith("DynamicContent.Plugin.Templates.HtmlTextTemplate"));
        }

        [TestMethod]
        public void HtmlTextTemplate_Validate_SortOrder()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();
            sut.SortOrder = 987;

            Assert.AreEqual(987, sut.SortOrder);
        }

        [TestMethod]
        public void Template_Width_100Percent_Returns_12Columns()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = 100;
            sut.Data = "Just some test data {:}";

            Assert.AreEqual(12, sut.ColumnCount);
        }

        [TestMethod]
        public void Template_Width_50Percent_Returns_6Columns()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = 50;
            sut.Data = "Just some test data {:}";

            Assert.AreEqual(6, sut.ColumnCount);
        }

        [TestMethod]
        public void Template_Width_30Percent_Returns_3Columns()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = 30;
            sut.Data = "Just some test data {:}";

            Assert.AreEqual(3, sut.ColumnCount);
        }

        [TestMethod]
        public void Template_Width_0Percent_Returns_1Column()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = 0;
            sut.Data = "Just some test data {:}";

            Assert.AreEqual(1, sut.ColumnCount);
        }

        [TestMethod]
        public void Template_Width_800Pixels_Returns_12Columns()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            sut.WidthType = DynamicContentWidthType.Pixels;
            sut.Width = 800;
            sut.Data = "Just some test data {:}";

            Assert.AreEqual(12, sut.ColumnCount);
        }

        [TestMethod]
        public void Template_Width_400Pixels_Returns_6Columns()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            sut.WidthType = DynamicContentWidthType.Pixels;
            sut.Width = 400;
            sut.Data = "Just some test data {:}";

            Assert.AreEqual(6, sut.ColumnCount);
        }

        [TestMethod]
        public void Template_Width_270Pixels_Returns_4Columns()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            sut.WidthType = DynamicContentWidthType.Pixels;
            sut.Width = 270;
            sut.Data = "Just some test data {:}";

            Assert.AreEqual(4, sut.ColumnCount);
        }

        [TestMethod]
        public void Template_Width_0Pixels_Returns_1Column()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            sut.WidthType = DynamicContentWidthType.Pixels;
            sut.Width = 0;
            sut.Data = "Just some test data {:}";

            Assert.AreEqual(1, sut.ColumnCount);
        }

        [TestMethod]
        public void Template_Width_1170Pixels_Returns_12Columns()
        {
            HtmlTextTemplate sut = new HtmlTextTemplate();

            sut.WidthType = DynamicContentWidthType.Pixels;
            sut.Width = 1170;
            sut.Data = "Just some test data {:}";

            Assert.AreEqual(12, sut.ColumnCount);
        }

        #endregion HtmlTextTemplate Tests
    }
}
