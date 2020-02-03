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
 *  File: MinifyUnitTests.cs
 *
 *  Purpose:  Minify Unit Tests
 *
 *  Date        Name                Reason
 *  23/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

using AspNetCore.PluginManager.Classes.Minify;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests
{
    [TestClass]
    public class MinifyUnitTests
    {
        [TestMethod]
        public void RemoveThreeBlankLines()
        {
            MinifyOperation operation = new RemoveBlankLines();

            string data = "firstline\r\n\n\r\nFourth LIne";

            IMinifyResult Results = operation.Process(MinificationFileType.Razor, ref data, new List<PreserveBlock>());

            Assert.IsTrue(Results.StartLength == 25);
            Assert.IsTrue(Results.EndLength == 21);
        }

        [TestMethod]
        public void RemoveThreeCarriageReturns()
        {
            MinifyOperation operation = new RemoveCarriageReturn();

            string data = "first line\r\n\r\n\r\nFourth LIne";

            IMinifyResult Results = operation.Process(MinificationFileType.CSS, ref data, new List<PreserveBlock>());

            Assert.IsTrue(Results.StartLength == 27);
            Assert.IsTrue(Results.EndLength == 24);
        }

        [TestMethod]
        public void RemoveHtmlSingleLineComment()
        {
            MinifyOperation operation = new RemoveHtmlComments();

            string data = "a <!--test comment-->z";

            IMinifyResult Results = operation.Process(MinificationFileType.Htm, ref data, new List<PreserveBlock>());

            Assert.IsTrue(Results.StartLength == 22);
            Assert.IsTrue(Results.EndLength == 3);
            Assert.IsTrue(data.Equals("a z"));
        }

        [TestMethod]
        public void RemoveHtmlMultiLineComment()
        {
            MinifyOperation operation = new RemoveHtmlComments();

            string data = "a <!--\r\n\r\ntest comment\n\r\n-->z";

            IMinifyResult Results = operation.Process(MinificationFileType.Htm, ref data, new List<PreserveBlock>());

            Assert.IsTrue(Results.StartLength == 29);
            Assert.IsTrue(Results.EndLength == 3);
            Assert.IsTrue(data.Equals("a z"));
        }

        [TestMethod]
        public void RemoveCSSSingleLineComment()
        {
            MinifyOperation operation = new RemoveCSSComments();

            string data = "a /*test comment*/z";

            IMinifyResult Results = operation.Process(MinificationFileType.CSS, ref data, new List<PreserveBlock>());

            Assert.IsTrue(Results.StartLength == 19);
            Assert.IsTrue(Results.EndLength == 3);
            Assert.IsTrue(data.Equals("a z"));
        }

        [TestMethod]
        public void RemoveCSSMultiLineComment()
        {
            MinifyOperation operation = new RemoveCSSComments();

            string data = "a /*\r\n\r\ntest comment\n\r\n*/z";

            IMinifyResult Results = operation.Process(MinificationFileType.CSS, ref data, new List<PreserveBlock>());

            Assert.IsTrue(Results.StartLength == 26);
            Assert.IsTrue(Results.EndLength == 3);
            Assert.IsTrue(data.Equals("a z"));
        }

        [TestMethod]
        public void RemoveRazorSingleLineComment()
        {
            MinifyOperation operation = new RemoveRazorComments();

            string data = "a @*test comment*@z";

            IMinifyResult Results = operation.Process(MinificationFileType.Razor, ref data, new List<PreserveBlock>());

            Assert.IsTrue(Results.StartLength == 19);
            Assert.IsTrue(Results.EndLength == 3);
            Assert.IsTrue(data.Equals("a z"));
        }

        [TestMethod]
        public void RemoveRazorMultiLineComment()
        {
            MinifyOperation operation = new RemoveRazorComments();

            string data = "a @*\r\n\r\ntest comment\n\r\n*@z";

            IMinifyResult Results = operation.Process(MinificationFileType.Razor, ref data, new List<PreserveBlock>());

            Assert.IsTrue(Results.StartLength == 26);
            Assert.IsTrue(Results.EndLength == 3);
            Assert.IsTrue(data.Equals("a z"));
        }

        [TestMethod]
        public void RemoveRazorDoubleSpacesComment()
        {
            MinifyOperation operation = new RemoveDoubleSpaces();

            string data = "a     \r\n\r\ntest   comment\n\r\n  z ";

            IMinifyResult Results = operation.Process(MinificationFileType.Razor, ref data, new List<PreserveBlock>());

            Assert.IsTrue(Results.StartLength == 31);
            Assert.IsTrue(Results.EndLength == 24);
            Assert.IsTrue(data.Equals("a \r\n\r\ntest comment\n\r\n z "));
        }
    }
}
