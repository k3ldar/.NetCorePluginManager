/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  Plugin Manager is distributed under the GNU General Public License version 3 and  
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
 *  File: PluginModuleTests.cs
 *
 *  Purpose:  PluginModule Tests
 *
 *  Date        Name                Reason
 *  19/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Internal;
using PluginManager.Tests.Mocks;

namespace PluginManager.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PluginModuleTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamAssembly_Null_Throws_ArgumentNullException()
        {
            new PluginModule(null, "TestAssembly", new MockIPlugin());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidModuleAssembly_Null_Throws_ArgumentNullException()
        {
            new PluginModule(Assembly.GetExecutingAssembly(), null, new MockIPlugin());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidModuleAssembly_EmptyString_Throws_ArgumentNullException()
        {
            new PluginModule(Assembly.GetExecutingAssembly(), "", new MockIPlugin());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidPluginService_Null_Throws_ArgumentNullException()
        {
            new PluginModule(Assembly.GetExecutingAssembly(), "TestAssembly", null);
        }

        [TestMethod]
        public void Version_Set_RemembersSetting_Success()
        {
            PluginModule sut = new PluginModule(Assembly.GetExecutingAssembly(), "TestAssembly", new MockIPlugin());
            sut.Version = 123;

            Assert.AreEqual(123, sut.Version);
            Assert.AreEqual(123, sut.GetVersion());
        }

        [TestMethod]
        public void Module_Set_RemembersSetting_Success()
        {
            PluginModule sut = new PluginModule(Assembly.GetExecutingAssembly(), "TestAssembly", new MockIPlugin());

            Assert.AreEqual("TestAssembly", sut.Module);
        }

        [TestMethod]
        public void Assembly_Set_RemembersSetting_Success()
        {
            PluginModule sut = new PluginModule(Assembly.GetExecutingAssembly(), "TestAssembly", new MockIPlugin());

            Assert.AreEqual(Assembly.GetExecutingAssembly(), sut.Assembly);
        }

        [TestMethod]
        public void Plugin_Set_RemembersSetting_Success()
        {
            MockIPlugin mockIPlugin = new MockIPlugin();
            PluginModule sut = new PluginModule(Assembly.GetExecutingAssembly(), "TestAssembly", mockIPlugin);

            Assert.AreEqual(mockIPlugin, sut.Plugin);
        }

        [TestMethod]
        public void FileVersion_Set_RemembersSetting_Success()
        {
            MockIPlugin mockIPlugin = new MockIPlugin();
            PluginModule sut = new PluginModule(Assembly.GetExecutingAssembly(), "TestAssembly", mockIPlugin)
            {
                FileVersion = "1.2.3.4"
            };

            Assert.AreEqual("1.2.3.4", sut.FileVersion);
        }
    }
}
