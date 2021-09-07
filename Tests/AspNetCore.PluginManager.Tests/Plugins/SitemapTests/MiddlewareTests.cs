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
 *  File: SitemapMenuTimingTests.cs
 *
 *  Purpose:  Tests for sitemap timing system admin menu
 *
 *  Date        Name                Reason
 *  31/08/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Tests.Mocks;

using Sitemap.Plugin;
using Sitemap.Plugin.Classes.SystemAdmin;

using static SharedPluginFeatures.Enums;

namespace AspNetCore.PluginManager.Tests.Plugins.SitemapTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class MiddlewareTests
    {
        private const string TestCategoryName = "Sitemap Plugin";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_INotificationService_Null_Throws_ArgumentNullException()
        {
            SitemapMiddleware sut = new SitemapMiddleware(null, new TestPluginClassesService(), new TestMemoryCache(), null, new TestLogger());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_IPluginClassesService_Null_Throws_ArgumentNullException()
        {
            SitemapMiddleware sut = new SitemapMiddleware(null, null, new TestMemoryCache(), new TestNotificationService(), new TestLogger());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_IMemoryCache_Null_Throws_ArgumentNullException()
        {
            SitemapMiddleware sut = new SitemapMiddleware(null, new TestPluginClassesService(), null, new TestNotificationService(), new TestLogger());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ILogger_Null_Throws_ArgumentNullException()
        {
            SitemapMiddleware sut = new SitemapMiddleware(null, new TestPluginClassesService(), new TestMemoryCache(), new TestNotificationService(), null);
        }
    }
}
