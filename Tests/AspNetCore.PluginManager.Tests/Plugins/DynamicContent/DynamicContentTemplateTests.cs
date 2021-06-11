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
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.DynamicContentTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DynamicContentTemplateTests
    {
        private const string TestCategoryName = "Dynamic Content";

        [TestMethod]
        public void Construct_Test()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();
        }

        [TestMethod]
        public void UniqueId_Get_ExistingIsNull_ReturnsNewGuid_Success()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();
            string uniqueId = sut.UniqueId;

            Assert.IsFalse(String.IsNullOrEmpty(uniqueId));

            bool canConvertToGuid = Guid.TryParse(uniqueId, out Guid newUniqueId);

            Assert.IsTrue(canConvertToGuid);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UniqueId_Set_InvalidValue_Null_Throws_InvalidOperationException()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();

            sut.UniqueId = null;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UniqueId_Set_InvalidValue_EmptyString_Throws_InvalidOperationException()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();

            sut.UniqueId = "";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UniqueId_Set_InvalidValue_InvalidChars_Throws_InvalidOperationException()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();

            sut.UniqueId = "abc^&";
        }

        [TestMethod]
        public void UniqueId_Set_ValidValue_Success()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();
            const string ValidValue = "AaBbCcDdEeFfGgHhIi-JjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";
            sut.UniqueId = ValidValue;

            Assert.AreEqual(ValidValue, sut.UniqueId);
        }

        [TestMethod]
        public void ColumnCount_Get_Percentage_50Percent_Expects_6Columns()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();

            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = 50;

            Assert.AreEqual(6, sut.ColumnCount);
        }

        [TestMethod]
        public void ColumnCount_Get_Percentage_66Percent_Expects_8Columns()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();

            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = 66;

            Assert.AreEqual(8, sut.ColumnCount);
        }

        [TestMethod]
        public void ColumnCount_Get_Percentage_Minus10Percent_Expects_1Columns()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();

            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = -10;

            Assert.AreEqual(1, sut.ColumnCount);
        }

        [TestMethod]
        public void ColumnCount_Get_Percentage_101Percent_Expects_12Columns()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();

            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = 101;

            Assert.AreEqual(12, sut.ColumnCount);
        }

        [TestMethod]
        public void ColumnCount_Get_Pixels_400Pixels_Expects_6Columns()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();

            sut.WidthType = DynamicContentWidthType.Pixels;
            sut.Width = 400;

            Assert.AreEqual(6, sut.ColumnCount);
        }

        [TestMethod]
        public void ColumnCount_Get_Pixels_528Pixels_Expects_8Columns()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();

            sut.WidthType = DynamicContentWidthType.Pixels;
            sut.Width = 528;

            Assert.AreEqual(8, sut.ColumnCount);
        }

        [TestMethod]
        public void ColumnCount_Get_Pixels_ExceedsMaxPixedWidth_50000Pixels_Expects_12Columns()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();

            sut.WidthType = DynamicContentWidthType.Pixels;
            sut.Width = 50000;

            Assert.AreEqual(12, sut.ColumnCount);
        }

        [TestMethod]
        public void ColumnCount_Get_Pixels_BelowMinPixedWidth_Minus10Pixels_Expects_1Column()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();

            sut.WidthType = DynamicContentWidthType.Pixels;
            sut.Width = -10;

            Assert.AreEqual(1, sut.ColumnCount);
        }

        [TestMethod]
        public void ColumnCount_Get_Columns_BelowMinColumnCount_Minus10_Expects_1Column()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();

            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = -10;

            Assert.AreEqual(1, sut.ColumnCount);
        }

        [TestMethod]
        public void ColumnCount_Get_Columns_AboveMaxColumnCount_38_Expects_12Columns()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();

            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 38;

            Assert.AreEqual(12, sut.ColumnCount);
        }

        [TestMethod]
        public void ColumnCount_Get_Columns_ValidColumnCount7_Expects_7Columns()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();

            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 7;

            Assert.AreEqual(7, sut.ColumnCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HtmlEnd_InvalidParam_Null_Throws_ArgumentNullException()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();

            sut.TestHtmlEnd(null);
        }

        [TestMethod]
        public void HtmlEnd_ReturnsProperlyFormattedHtmlEndBlock_Success()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();

            StringBuilder stringBuilder = new StringBuilder();
            sut.TestHtmlEnd(stringBuilder);

            string endBlock = stringBuilder.ToString();

            Assert.AreEqual("</div>", endBlock);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HtmlStart_InvalidParam_Null_Throws_ArgumentNullException()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();

            sut.TestHtmlStart(null);
        }

        [TestMethod]
        public void HtmlStart_WidthTypeColumnsZeroPercentHeight_ReturnsValidHtmlStart()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 5;
            sut.HeightType = DynamicContentHeightType.Percentage;
            sut.Height = 0;

            StringBuilder stringBuilder = new StringBuilder();
            sut.TestHtmlStart(stringBuilder);

            string htmlStart = stringBuilder.ToString();

            Assert.AreEqual("<div class=\"col-sm-5\">", htmlStart);
        }

        [TestMethod]
        public void HtmlStart_WidthTypeColumnsAutomaticHeight_ReturnsValidHtmlStart()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 5;
            sut.HeightType = DynamicContentHeightType.Automatic;
            sut.Height = 500;

            StringBuilder stringBuilder = new StringBuilder();
            sut.TestHtmlStart(stringBuilder);

            string htmlStart = stringBuilder.ToString();

            Assert.AreEqual("<div class=\"col-sm-5\">", htmlStart);
        }

        [TestMethod]
        public void HtmlStart_WidthTypeColumns_WithHeight_ReturnsValidHtmlStart()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();
            sut.WidthType = DynamicContentWidthType.Columns;
            sut.Width = 7;
            sut.HeightType = DynamicContentHeightType.Pixels;
            sut.Height = 120;

            StringBuilder stringBuilder = new StringBuilder();
            sut.TestHtmlStart(stringBuilder);

            string htmlStart = stringBuilder.ToString();

            Assert.AreEqual("<div class=\"col-sm-7\" style=\"height:120px !important;\">", htmlStart);
        }

        [TestMethod]
        public void HtmlStart_WidthTypePercent_WithoutHeight_ReturnsValidHtmlStart()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();
            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = 63;
            sut.HeightType = DynamicContentHeightType.Percentage;
            sut.Height = 0;

            StringBuilder stringBuilder = new StringBuilder();
            sut.TestHtmlStart(stringBuilder);

            string htmlStart = stringBuilder.ToString();

            Assert.AreEqual("<div style=\"width:63% !important;display:block;\">", htmlStart);
        }

        [TestMethod]
        public void HtmlStart_WidthTypePercent_WithHeightPixels_ReturnsValidHtmlStart()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();
            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = 63;
            sut.HeightType = DynamicContentHeightType.Pixels;
            sut.Height = 236;

            StringBuilder stringBuilder = new StringBuilder();
            sut.TestHtmlStart(stringBuilder);

            string htmlStart = stringBuilder.ToString();

            Assert.AreEqual("<div style=\"width:63% !important;height:236px !important;display:block;\">", htmlStart);
        }

        [TestMethod]
        public void HtmlStart_WidthTypePercent_WithHeightPercent_ReturnsValidHtmlStart()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();
            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = 63;
            sut.HeightType = DynamicContentHeightType.Percentage;
            sut.Height = 19;

            StringBuilder stringBuilder = new StringBuilder();
            sut.TestHtmlStart(stringBuilder);

            string htmlStart = stringBuilder.ToString();

            Assert.AreEqual("<div style=\"width:63% !important;height:19% !important;display:block;\">", htmlStart);
        }

        [TestMethod]
        public void HtmlStart_WidthTypePercent_WithHeightAutomatic_ReturnsValidHtmlStart()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();
            sut.WidthType = DynamicContentWidthType.Percentage;
            sut.Width = 72;
            sut.HeightType = DynamicContentHeightType.Automatic;
            sut.Height = 23;

            StringBuilder stringBuilder = new StringBuilder();
            sut.TestHtmlStart(stringBuilder);

            string htmlStart = stringBuilder.ToString();

            Assert.AreEqual("<div style=\"width:72% !important;display:block;\">", htmlStart);
        }

        [TestMethod]
        public void HtmlStart_WidthTypePixels_WithoutHeight_ReturnsValidHtmlStart()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();
            sut.WidthType = DynamicContentWidthType.Pixels;
            sut.Width = 172;
            sut.HeightType = DynamicContentHeightType.Percentage;
            sut.Height = 0;

            StringBuilder stringBuilder = new StringBuilder();
            sut.TestHtmlStart(stringBuilder);

            string htmlStart = stringBuilder.ToString();

            Assert.AreEqual("<div style=\"width:172px !important;display:block;\">", htmlStart);
        }

        [TestMethod]
        public void HtmlStart_WidthTypePixels_WithHeightPixels_ReturnsValidHtmlStart()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();
            sut.WidthType = DynamicContentWidthType.Pixels;
            sut.Width = 365;
            sut.HeightType = DynamicContentHeightType.Pixels;
            sut.Height = 236;

            StringBuilder stringBuilder = new StringBuilder();
            sut.TestHtmlStart(stringBuilder);

            string htmlStart = stringBuilder.ToString();

            Assert.AreEqual("<div style=\"width:365px !important;height:236px !important;display:block;\">", htmlStart);
        }

        [TestMethod]
        public void HtmlStart_WidthTypePixels_WithHeightPercent_ReturnsValidHtmlStart()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();
            sut.WidthType = DynamicContentWidthType.Pixels;
            sut.Width = 687;
            sut.HeightType = DynamicContentHeightType.Percentage;
            sut.Height = 19;

            StringBuilder stringBuilder = new StringBuilder();
            sut.TestHtmlStart(stringBuilder);

            string htmlStart = stringBuilder.ToString();

            Assert.AreEqual("<div style=\"width:687px !important;height:19% !important;display:block;\">", htmlStart);
        }

        [TestMethod]
        public void HtmlStart_WidthTypePixels_WithHeightAutomatic_ReturnsValidHtmlStart()
        {
            TestDynamicTemplate sut = new TestDynamicTemplate();
            sut.WidthType = DynamicContentWidthType.Pixels;
            sut.Width = 23;
            sut.HeightType = DynamicContentHeightType.Automatic;
            sut.Height = 23;

            StringBuilder stringBuilder = new StringBuilder();
            sut.TestHtmlStart(stringBuilder);

            string htmlStart = stringBuilder.ToString();

            Assert.AreEqual("<div style=\"width:23px !important;display:block;\">", htmlStart);
        }
    }
}
