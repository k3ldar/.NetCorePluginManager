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
 *  File: SearchTestBase.cs
 *
 *  Purpose:  Base class for search tests, creates default data used by multiple tests
 *
 *  Date        Name                Reason
 *  19/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;

using SharedPluginFeatures;

using Constants = SharedPluginFeatures.Constants;
using pm = PluginManager.Internal;

namespace AspNetCore.PluginManager.Tests.Search
{
    public class SearchTestBase
    {
        protected static TestSearchPluginManager _testPlugin = new TestSearchPluginManager();
        protected static bool? _pluginLoaded = null;
        protected static IPluginClassesService _pluginServices;
        protected static IDocumentationService _documentationService;


        [TestInitialize]
        public void InitializeDocumentationLoadTest()
        {
            lock (_testPlugin)
            {
                while (_pluginLoaded.HasValue && !_pluginLoaded.Value)
                {
                    System.Threading.Thread.Sleep(30);
                }

                if (_pluginLoaded.HasValue && _pluginLoaded.Value)
                {
                    return;
                }

                if (_pluginLoaded == null)
                {
                    _pluginLoaded = false;
                }

                _testPlugin.AddAssembly(Assembly.GetExecutingAssembly());
                _testPlugin.UsePlugin(typeof(DemoWebsite.Classes.PluginInitialisation));
                _testPlugin.UsePlugin(typeof(DocumentationPlugin.PluginInitialisation));
                _testPlugin.UsePlugin(typeof(MemoryCache.Plugin.PluginInitialisation));
                _testPlugin.UsePlugin(typeof(ProductPlugin.PluginInitialisation));
                _testPlugin.UsePlugin(typeof(SearchPlugin.PluginInitialisation));

                _testPlugin.ConfigureServices();

                _pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;
                TimeSpan docLoadTime = new TimeSpan(0, 0, 30);
                DateTime startLoadDocs = DateTime.Now;

                while (Shared.Classes.ThreadManager.Exists(SharedPluginFeatures.Constants.DocumentationLoadThread))
                {
                    System.Threading.Thread.Sleep(100);

                    if (DateTime.Now - startLoadDocs > docLoadTime)
                        break;
                }

                Assert.IsFalse(Shared.Classes.ThreadManager.Exists(Constants.DocumentationLoadThread));

                _documentationService = (IDocumentationService)_testPlugin.GetServiceProvider()
                    .GetService(typeof(IDocumentationService));

                Assert.IsNotNull(_documentationService);

                Assert.IsTrue(_documentationService.GetDocuments().Count > 100);
                _pluginLoaded = true;
            }

            Assert.IsNotNull(_pluginServices);

        }
    }
}
