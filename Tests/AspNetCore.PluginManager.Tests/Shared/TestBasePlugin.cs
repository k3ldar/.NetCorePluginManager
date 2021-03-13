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
 *  File: SearchTestBase.cs
 *
 *  Purpose:  Base class for search tests, creates default data used by multiple tests
 *
 *  Date        Name                Reason
 *  19/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;

using SharedPluginFeatures;

using static SharedPluginFeatures.Constants;

using pm = PluginManager.Internal;
using sl = Shared.Classes;

namespace AspNetCore.PluginManager.Tests
{
    [ExcludeFromCodeCoverage]
    public class TestBasePlugin
    {
        protected static TestPluginManager _testPluginBadEgg = new TestPluginManager();
        protected static bool? _pluginLoadedBadEgg = null;
        protected static IPluginClassesService _pluginServicesBadEgg;

        protected static TestPluginManager _testPluginSmokeTest = new TestPluginManager();
        protected static bool? _pluginLoadedSmokeTest = null;
        protected static IPluginClassesService _pluginServicesSmokeTest;

        protected static TestPluginManager _testPluginLogin = new TestPluginManager();
        protected static bool? _pluginLoadedLogin = null;
        protected static IPluginClassesService _pluginServicesLogin;

        protected static TestPluginManager _testPluginSpider = new TestPluginManager();
        protected static bool? _pluginLoadedSpider = null;
        protected static IPluginClassesService _pluginServicesSpider;

        protected static TestPluginManager _testPluginSubdomain = new TestPluginManager();
        protected static bool? _pluginLoadedSubdomain = null;
        protected static IPluginClassesService _pluginServicesSubdomain;

        protected static TestPluginManager _testPluginDocs = new TestPluginManager();
        protected static bool? _pluginLoadedDocs = null;
        protected static IPluginClassesService _pluginServicesDocs;
        protected static IDocumentationService _documentationService;


        protected void InitializeDocumentationPluginManager()
        {
            lock (_testPluginDocs)
            {
                while (_pluginLoadedDocs.HasValue && !_pluginLoadedDocs.Value)
                {
                    System.Threading.Thread.Sleep(30);
                }

                if (_pluginLoadedDocs.HasValue && _pluginLoadedDocs.Value)
                {
                    return;
                }

                if (_pluginLoadedDocs == null)
                {
                    _pluginLoadedDocs = false;
                }

                _testPluginDocs.AddAssembly(Assembly.GetExecutingAssembly());
                _testPluginDocs.UsePlugin(typeof(DemoWebsite.Classes.PluginInitialisation));
                _testPluginDocs.UsePlugin(typeof(DocumentationPlugin.PluginInitialisation));
                _testPluginDocs.UsePlugin(typeof(MemoryCache.Plugin.PluginInitialisation));
                _testPluginDocs.UsePlugin(typeof(ProductPlugin.PluginInitialisation));
                _testPluginDocs.UsePlugin(typeof(SearchPlugin.PluginInitialisation));
                _testPluginDocs.UsePlugin(typeof(BadEgg.Plugin.PluginInitialisation));

                _testPluginDocs.ConfigureServices();

                _pluginServicesDocs = new pm.PluginServices(_testPluginDocs) as IPluginClassesService;
                TimeSpan docLoadTime = new TimeSpan(0, 0, 30);
                DateTime startLoadDocs = DateTime.Now;

                while (sl.ThreadManager.Exists(DocumentationLoadThread))
                {
                    System.Threading.Thread.Sleep(100);

                    if (DateTime.Now - startLoadDocs > docLoadTime)
                        break;
                }

                Assert.IsFalse(sl.ThreadManager.Exists(Constants.DocumentationLoadThread));

                _documentationService = (IDocumentationService)_testPluginDocs.GetServiceProvider()
                    .GetService(typeof(IDocumentationService));

                Assert.IsNotNull(_documentationService);

                Assert.IsTrue(_documentationService.GetDocuments().Count > 100);
                _pluginLoadedDocs = true;
            }

            Assert.IsNotNull(_pluginServicesDocs);

        }

        protected void InitializeSmokeTestPluginManager()
        {
            lock (_testPluginSmokeTest)
            {
                while (_pluginLoadedSmokeTest.HasValue && !_pluginLoadedSmokeTest.Value)
                {
                    System.Threading.Thread.Sleep(30);
                }

                if (_pluginLoadedSmokeTest.HasValue && _pluginLoadedSmokeTest.Value)
                {
                    return;
                }

                if (_pluginLoadedSmokeTest == null)
                {
                    _pluginLoadedSmokeTest = false;
                }

                _testPluginSmokeTest.AddAssembly(Assembly.GetExecutingAssembly());
                _testPluginSmokeTest.UsePlugin(typeof(LoginPlugin.PluginInitialisation));

                _testPluginSmokeTest.ConfigureServices();

                _pluginServicesSmokeTest = new pm.PluginServices(_testPluginSmokeTest) as IPluginClassesService;

                _pluginLoadedSmokeTest = true;
            }

            Assert.IsNotNull(_pluginServicesSmokeTest);
        }

        protected void InitializeLoginPluginManager()
        {
            lock (_testPluginLogin)
            {
                while (_pluginLoadedLogin.HasValue && !_pluginLoadedLogin.Value)
                {
                    System.Threading.Thread.Sleep(30);
                }

                if (_pluginLoadedLogin.HasValue && _pluginLoadedLogin.Value)
                {
                    return;
                }

                if (_pluginLoadedLogin == null)
                {
                    _pluginLoadedLogin = false;
                }

                _testPluginLogin.AddAssembly(Assembly.GetExecutingAssembly());
                _testPluginLogin.UsePlugin(typeof(DemoWebsite.Classes.PluginInitialisation));
                _testPluginLogin.UsePlugin(typeof(MemoryCache.Plugin.PluginInitialisation));
                _testPluginLogin.UsePlugin(typeof(LoginPlugin.PluginInitialisation));

                _testPluginLogin.ConfigureServices();

                _pluginServicesLogin = new pm.PluginServices(_testPluginLogin) as IPluginClassesService;

                _pluginLoadedLogin = true;
            }

            Assert.IsNotNull(_pluginServicesLogin);

        }

        protected void InitializeBadEggPluginManager()
        {
            lock (_testPluginBadEgg)
            {
                while (_pluginLoadedBadEgg.HasValue && !_pluginLoadedBadEgg.Value)
                {
                    System.Threading.Thread.Sleep(30);
                }

                if (_pluginLoadedBadEgg.HasValue && _pluginLoadedBadEgg.Value)
                {
                    return;
                }

                if (_pluginLoadedBadEgg == null)
                {
                    _pluginLoadedBadEgg = false;
                }

                _testPluginBadEgg.AddAssembly(Assembly.GetExecutingAssembly());
                _testPluginBadEgg.UsePlugin(typeof(DemoWebsite.Classes.PluginInitialisation));
                _testPluginBadEgg.UsePlugin(typeof(MemoryCache.Plugin.PluginInitialisation));
                _testPluginBadEgg.UsePlugin(typeof(ProductPlugin.PluginInitialisation));
                _testPluginBadEgg.UsePlugin(typeof(SearchPlugin.PluginInitialisation));
                _testPluginBadEgg.UsePlugin(typeof(BadEgg.Plugin.PluginInitialisation));

                _testPluginBadEgg.ConfigureServices();

                _pluginServicesBadEgg = new pm.PluginServices(_testPluginBadEgg) as IPluginClassesService;

                _pluginLoadedBadEgg = true;
            }

            Assert.IsNotNull(_pluginServicesBadEgg);

        }

        protected void InitializeSpiderPluginManager()
        {
            lock (_testPluginSpider)
            {
                while (_pluginLoadedSpider.HasValue && !_pluginLoadedSpider.Value)
                {
                    System.Threading.Thread.Sleep(30);
                }

                if (_pluginLoadedSpider.HasValue && _pluginLoadedSpider.Value)
                {
                    return;
                }

                if (_pluginLoadedSpider == null)
                {
                    _pluginLoadedSpider = false;
                }

                _testPluginSpider.AddAssembly(Assembly.GetExecutingAssembly());
                _testPluginSpider.UsePlugin(typeof(DemoWebsite.Classes.PluginInitialisation));
                _testPluginSpider.UsePlugin(typeof(MemoryCache.Plugin.PluginInitialisation));
                _testPluginSpider.UsePlugin(typeof(LoginPlugin.PluginInitialisation));
                _testPluginSpider.UsePlugin(typeof(Spider.Plugin.PluginInitialisation));
                _testPluginSpider.UsePlugin(typeof(LoginPlugin.PluginInitialisation));

                _testPluginSpider.ConfigureServices();

                _pluginServicesSpider = new pm.PluginServices(_testPluginSpider) as IPluginClassesService;

                _pluginLoadedSpider = true;
            }

            Assert.IsNotNull(_pluginServicesSpider);

        }

        protected void InitializeSubdomainManager()
        {
            lock (_testPluginSubdomain)
            {
                while (_pluginLoadedSubdomain.HasValue && !_pluginLoadedSubdomain.Value)
                {
                    System.Threading.Thread.Sleep(30);
                }

                if (_pluginLoadedSubdomain.HasValue && _pluginLoadedSubdomain.Value)
                {
                    return;
                }

                if (_pluginLoadedSubdomain == null)
                {
                    _pluginLoadedSubdomain = false;
                }

                _testPluginSubdomain.AddAssembly(Assembly.GetExecutingAssembly());
                _testPluginSubdomain.UsePlugin(typeof(Subdomain.Plugin.PluginInitialisation));
                _testPluginSubdomain.UsePlugin(typeof(DemoWebsite.Classes.PluginInitialisation));
                _testPluginSubdomain.UsePlugin(typeof(MemoryCache.Plugin.PluginInitialisation));
                _testPluginSubdomain.UsePlugin(typeof(LoginPlugin.PluginInitialisation));
                _testPluginSubdomain.UsePlugin(typeof(Blog.Plugin.PluginInitialisation));
                _testPluginSubdomain.UsePlugin(typeof(HelpdeskPlugin.PluginInitialisation));
                _testPluginSubdomain.UsePlugin(typeof(UserAccount.Plugin.PluginInitialisation));

                _testPluginSubdomain.ConfigureServices();

                _pluginServicesSubdomain = new pm.PluginServices(_testPluginSubdomain) as IPluginClassesService;

                _pluginLoadedSubdomain = true;
            }

            Assert.IsNotNull(_testPluginSubdomain);
        }
    }
}
