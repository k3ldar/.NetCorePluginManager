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
 *  File: RetrieveImagesModelTests.cs
 *
 *  Purpose:  Tests for retriev images model
 *
 *  Date        Name                Reason
 *  16/06/2021  Simon Carter        Initially Created
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
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RetrieveImagesModelTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_SubgroupNames_Null_Throws_ArgumentNullException()
        {
            RetrieveImagesModel sut = new RetrieveImagesModel(null, new List<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ImageNames_Null_Throws_ArgumentNullException()
        {
            RetrieveImagesModel sut = new RetrieveImagesModel(new List<string>(), null);
        }

        [TestMethod]
        public void Construct_ValidInstance_Success()
        {
            RetrieveImagesModel sut = new RetrieveImagesModel(new List<string>(), new List<string>());
            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Subgroups);
            Assert.AreEqual(0, sut.Subgroups.Count);
            Assert.IsNotNull(sut.Images);
            Assert.AreEqual(0, sut.Images.Count);
        }
    }
}
