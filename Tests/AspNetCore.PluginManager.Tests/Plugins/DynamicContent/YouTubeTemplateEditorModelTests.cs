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
 *  File: YouTubeTemplateEditorModel.cs
 *
 *  Purpose:  YouTubeTemplateEditorModel tests
 *
 *  Date        Name                Reason
 *  22/06/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DynamicContent.Plugin.Model;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNetCore.PluginManager.Tests.Plugins.DynamicContentTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class YouTubeTemplateEditorModelTests
    {
        [TestMethod]
        public void Construct_ValidInstance_NullData_Success()
        {
            YouTubeTemplateEditorModel sut = new YouTubeTemplateEditorModel(null);

            Assert.IsNotNull(sut);
            Assert.AreEqual("", sut.VideoId);
            Assert.IsFalse(sut.AutoPlay);
            Assert.AreEqual("|", sut.Data);
        }

        [TestMethod]
        public void Construct_ValidInstance_EmptyStringData_Success()
        {
            YouTubeTemplateEditorModel sut = new YouTubeTemplateEditorModel("");

            Assert.IsNotNull(sut);
            Assert.AreEqual("", sut.VideoId);
            Assert.IsFalse(sut.AutoPlay);
            Assert.AreEqual("|", sut.Data);
        }

        [TestMethod]
        public void Construct_ValidInstance_ValidData_Success()
        {
            YouTubeTemplateEditorModel sut = new YouTubeTemplateEditorModel("aaBBcC|TrUe");

            Assert.IsNotNull(sut);
            Assert.AreEqual("aaBBcC", sut.VideoId);
            Assert.IsTrue(sut.AutoPlay);
            Assert.AreEqual("aaBBcC|TrUe", sut.Data);
        }
    }
}
