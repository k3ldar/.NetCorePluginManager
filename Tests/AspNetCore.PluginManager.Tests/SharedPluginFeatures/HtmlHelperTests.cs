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
 *  File: BaseMiddlewareWrapper.cs
 *
 *  Purpose:  HtmlHelper tests
 *
 *  Date        Name                Reason
 *  06/06/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using static SharedPluginFeatures.HtmlHelper;

namespace AspNetCore.PluginManager.Tests.SharedPluginFeatures
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class HtmlHelperTests
    {
        private const string TestCategoryName = "Shared Plugin Features";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Validate_ClassMethod_RouteFriendlyName_Success()
        {
            IHtmlHelper htmlHelper = new MockHtmlHelper();
            string sut = htmlHelper.RouteFriendlyName("ab!\"£$%%^&*()_-+\\|,.<>/?#~@':;}{*-----");

            Assert.AreEqual("ab-_", sut);
        }


        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void RouteFriendlyName_InvalidParam_Null_ReturnsNull()
        {
            string sut = RouteFriendlyName(null);

            Assert.IsNull(sut);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void RouteFriendlyName_InvalidParam_EmptyString_ReturnsEmptyString()
        {
            string sut = RouteFriendlyName("");

            Assert.AreEqual("", sut);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void RouteFriendlyName_RemovesInvalidCharacters_Success()
        {
            string sut = RouteFriendlyName("ab!\"£$%%^&*()_-+\\|,.<>/?#~@':;}{*");

            Assert.AreEqual("ab-_", sut);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void RouteFriendlyName_RemovesAllInvalidCharacters_Success()
        {
            StringBuilder sb = new StringBuilder(255);

            for (int i = 0; i <= Byte.MaxValue; i++)
                sb.Append((char)i);

            string sut = RouteFriendlyName(sb.ToString());

            Assert.AreEqual("0123456789-ABCDEFGHIJKLMNOPQRSTUVWXYZ-_-abcdefghijklmnopqrstuvwxyz", sut);
        }
    }
}
