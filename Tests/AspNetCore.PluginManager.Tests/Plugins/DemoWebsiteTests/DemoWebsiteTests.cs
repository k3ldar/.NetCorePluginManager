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
 *  File: DemoWebsiteTests.cs
 *
 *  Purpose:  Unit tests for demo webiste
 *
 *  Date        Name                Reason
 *  05/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.DemoWebsite.Classes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNetCore.PluginManager.Tests.Plugins.DemoWebsiteTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DemoWebsiteTests
    {
        #region ErrorManagerProvider

        [TestMethod]
        public void ErrorManagerProvider_Construct_Success()
        {
            ErrorManagerProvider sut = new ErrorManagerProvider();
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void ErrorManagerProvider_ErrorRaised_Success()
        {
            ErrorManagerProvider sut = new ErrorManagerProvider();
			Assert.IsNotNull(sut);
			sut.ErrorRaised(null);
        }

        [TestMethod]
        public void ErrorManagerProvider_MissingPage_ReturnsFalse()
        {
            string replacePath = String.Empty;
            ErrorManagerProvider sut = new ErrorManagerProvider();

            bool result = sut.MissingPage(String.Empty, ref replacePath);

            Assert.IsFalse(result);

            Assert.AreEqual(String.Empty, replacePath);
        }

        #endregion ErrorManagerProvider
    }
}
