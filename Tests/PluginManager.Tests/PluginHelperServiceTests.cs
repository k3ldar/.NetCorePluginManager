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
 *  Product:  PluginManager.Tests
 *  
 *  File: PluginHelperServiceTests.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  28/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;
using PluginManager.Internal;
using PluginManager.Tests.Mocks;

namespace PluginManager.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PluginHelperServiceTests
    {
        #region Private Members


        #endregion Private Members

        [TestInitialize]
        public void PluginServices_TestInitialise()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PluginServices_Construct_NullPluginManager_Throws_ArgumentNullException()
        {
            new PluginServices(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PluginServices_AddAssembly_NullPluginManager_Throws_ArgumentNullException()
        {
            using (TestPluginManager pluginManager = new TestPluginManager())
            {
                IPluginHelperService pluginServices = new PluginServices(pluginManager) as IPluginHelperService;
                pluginServices.AddAssembly(null);
            }
        }

        [TestMethod]
        public void PluginServices_TestAddAssembly()
        {
            using (TestPluginManager pluginManager = new TestPluginManager())
            {
                IPluginHelperService pluginServices = new PluginServices(pluginManager) as IPluginHelperService;

                Assert.IsNotNull(pluginServices);

                Assembly current = Assembly.GetExecutingAssembly();

                DynamicLoadResult loadResult = pluginServices.AddAssembly(current);

                Assert.IsTrue(loadResult == DynamicLoadResult.Success);

                pluginServices.PluginLoaded(System.IO.Path.GetFileName(current.Location), out int version);

                Assert.IsTrue(version == 1);
            }
        }

        [TestMethod]
        public void PluginServices_TestAddAssemblyTwice()
        {
            using (TestPluginManager pluginManager = new TestPluginManager())
            {
                IPluginHelperService pluginServices = new PluginServices(pluginManager) as IPluginHelperService;

                Assert.IsNotNull(pluginServices);

                Assembly current = Assembly.GetExecutingAssembly();

                DynamicLoadResult loadResult = pluginServices.AddAssembly(current);

                Assert.IsTrue(loadResult == DynamicLoadResult.Success);

                pluginServices.PluginLoaded(System.IO.Path.GetFileName(current.Location), out int version);

                Assert.IsTrue(version == 1);

                loadResult = pluginServices.AddAssembly(current);

                Assert.IsTrue(loadResult == DynamicLoadResult.AlreadyLoaded);
            }
        }

        [TestMethod]
        public void PluginServices_GetPluginClassTypes_ReturnsValidTypes()
        {
            using (TestPluginManager pluginManager = new TestPluginManager())
            {
                IPluginClassesService pluginClassesServices = new PluginServices(pluginManager) as IPluginClassesService;
                IPluginHelperService pluginServices = new PluginServices(pluginManager) as IPluginHelperService;

                Assert.IsNotNull(pluginClassesServices);

                Assembly current = Assembly.GetExecutingAssembly();

                DynamicLoadResult loadResult = pluginServices.AddAssembly(current);

                Assert.IsTrue(loadResult == DynamicLoadResult.Success);

                List<Type> classTypes = pluginClassesServices.GetPluginClassTypes<ILogger>();

                Assert.IsNotNull(classTypes);
                Assert.AreEqual(3, classTypes.Count);
            }
        }

        [TestMethod]
        public void PluginServices_GetPluginClass_ReturnsValidClasses()
        {
            using (TestPluginManager pluginManager = new TestPluginManager())
            {
                IPluginClassesService pluginClassesServices = new PluginServices(pluginManager) as IPluginClassesService;
                IPluginHelperService pluginServices = new PluginServices(pluginManager) as IPluginHelperService;

                Assert.IsNotNull(pluginClassesServices);

                Assembly current = Assembly.GetExecutingAssembly();

                DynamicLoadResult loadResult = pluginServices.AddAssembly(current);

                Assert.IsTrue(loadResult == DynamicLoadResult.Success);

                List<ILogger> classes = pluginClassesServices.GetPluginClasses<ILogger>();

                Assert.IsNotNull(classes);
                Assert.AreEqual(2, classes.Count);
            }
        }

        [TestMethod]
        public void PluginServices_GetPluginTypesWithAttributes_TestClasses_ReturnsValidClasses()
        {
            using (TestPluginManager pluginManager = new TestPluginManager())
            {
                IPluginTypesService pluginTypesServices = new PluginServices(pluginManager) as IPluginTypesService;
                IPluginHelperService pluginServices = new PluginServices(pluginManager) as IPluginHelperService;

                Assert.IsNotNull(pluginTypesServices);

                Assembly current = Assembly.GetExecutingAssembly();

                DynamicLoadResult loadResult = pluginServices.AddAssembly(current);

                Assert.IsTrue(loadResult == DynamicLoadResult.Success);

                List<Type> classTypesWithAttributes = pluginTypesServices.GetPluginTypesWithAttribute<TestClassAttribute>();

                Assert.IsNotNull(classTypesWithAttributes);
                Assert.IsTrue(classTypesWithAttributes.Count >= 5);
            }
        }
    }
}
