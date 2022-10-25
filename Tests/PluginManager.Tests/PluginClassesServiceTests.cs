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
 *  Product:  PluginManager.Tests
 *  
 *  File: PluginClassesServiceTests.cs
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
using PluginManager.Tests.Mocks;

namespace PluginManager.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PluginClassesServiceTests
    {
        [TestMethod]
        public void FindAllILoggerClassTypes()
        {
            using (MockPluginManager pluginManager = new MockPluginManager())
            {
                IPluginClassesService pluginServices = pluginManager as IPluginClassesService;

                Assert.IsNotNull(pluginServices);

                List<Type> classTypes = pluginServices.GetPluginClassTypes<ILogger>();

                Assert.AreEqual(classTypes.Count, 1);

                Assert.AreEqual(classTypes[0].FullName, "PluginManager.Internal.DefaultLogger");
            }
        }

        [TestMethod]
        public void FindTestLoggerClassTypeInstances()
        {
            using (MockPluginManager pluginManager = new MockPluginManager())
            {
                pluginManager.PluginLoad(Assembly.GetExecutingAssembly(), String.Empty, false);
                IPluginClassesService pluginServices = pluginManager as IPluginClassesService;

                Assert.IsNotNull(pluginServices);

                List<ILogger> classTypes = pluginServices.GetPluginClasses<ILogger>();

                Assert.AreEqual(classTypes.Count, 2);

                Assert.AreEqual("PluginManager.Tests.Mocks.MockLogger", classTypes[1].GetType().FullName);
            }
        }

        [TestMethod]
        public void FindTestLoggerClassTypeInstancesAddLogAndVerify()
        {
            using (MockPluginManager pluginManager = new MockPluginManager())
            {
                pluginManager.PluginLoad(Assembly.GetExecutingAssembly(), String.Empty, false);
                IPluginClassesService pluginServices = pluginManager as IPluginClassesService;

                Assert.IsNotNull(pluginServices);

                List<ILogger> classTypes = pluginServices.GetPluginClasses<ILogger>();

                Assert.AreEqual(classTypes.Count, 2);

                Assert.AreEqual("PluginManager.Tests.Mocks.MockLogger", classTypes[1].GetType().FullName);

                MockLogger testLogger = classTypes[1] as MockLogger;

                Assert.IsNotNull(testLogger);

                testLogger.AddToLog(LogLevel.Information, "test");

                Assert.AreEqual(testLogger.Logs[0].Data, "test");
                Assert.AreEqual(testLogger.Logs[0].LogLevel, LogLevel.Information);
            }
        }
    }
}
