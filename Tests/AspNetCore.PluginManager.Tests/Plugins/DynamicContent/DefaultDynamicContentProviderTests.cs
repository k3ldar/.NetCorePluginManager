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
 *  File: DefaultDynamicContentProviderTests.cs
 *
 *  Purpose:  Tests for html text template
 *
 *  Date        Name                Reason
 *  29/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using DynamicContent.Plugin.Internal;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNetCore.PluginManager.Tests.Plugins.DynamicContentTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DefaultDynamicContentProviderTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamPluginService_Null_Throws_ArgumentNullException()
        {
            new DefaultDynamicContentProvider(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void RenderDynamicContent_TemporaryTest_Throws_NotImplementedException()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new TestPluginClassesService());
            sut.RenderDynamicPage(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetActiveCustomPages_TemporaryTest_Throws_NotImplementedException()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new TestPluginClassesService());
            sut.GetCustomPageList();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetCustomPage_TemporaryTest_Throws_NotImplementedException()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new TestPluginClassesService());
            sut.GetCustomPage(1);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Templates_TemporaryTest_Throws_NotImplementedException()
        {
            DefaultDynamicContentProvider sut = new DefaultDynamicContentProvider(new TestPluginClassesService());
            sut.Templates();
        }
    }
}
