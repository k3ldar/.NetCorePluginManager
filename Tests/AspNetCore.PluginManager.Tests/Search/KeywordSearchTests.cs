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
 *  File: KeywordSearchTests.cs
 *
 *  Purpose:  Tests for Keyword Search
 *
 *  Date        Name                Reason
 *  03/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.Search;

using PluginManager.Abstractions;
using PluginManager.Tests.Mocks;

using pm = PluginManager.Internal;

namespace AspNetCore.PluginManager.Tests.Search
{
    [TestClass]
    public class KeywordSearchTests
    {
        [TestMethod]
        public void KeywordLoggedInValidSearchTerm()
        {
            KeywordSearchOptions options = new KeywordSearchOptions(true, "test");
        }

        [TestMethod]
        public void KeywordLoggedOutValidSearchTerm()
        {
            KeywordSearchOptions options = new KeywordSearchOptions(false, "test");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void KeywordLoggedOutInvalidSearchTermNull()
        {
            KeywordSearchOptions options = new KeywordSearchOptions(false, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void KeywordLoggedOutInvalidSearchTermEmptyString()
        {
            KeywordSearchOptions options = new KeywordSearchOptions(false, "");
        }

        [TestMethod]
        public void KeywordSearchFindAllProviders()
        {
            using (TestPluginManager pluginManager = new TestPluginManager())
            {
                pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
                IPluginClassesService pluginServices = new pm.PluginServices(pluginManager) as IPluginClassesService;

                Assert.IsNotNull(pluginServices);

                List<Type> classTypes = pluginServices.GetPluginClassTypes<ISearchKeywordProvider>();

                Assert.AreEqual(classTypes.Count, 2);

                Assert.AreEqual(classTypes[0].FullName, "AspNetCore.PluginManager.Tests.Search.Mocks.MockKeywordSearchProviderA");
            }

        }
    }
}
