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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.Tests
 *  
 *  File: ServiceCollectionHelperTests.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  28/09/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;
using PluginManager.Tests.Mocks;

namespace PluginManager.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ServiceCollectionHelperTests
    {
        private const string TestCategoryDescription = "Plugin Manager";

        [TestMethod]
        [TestCategory(TestCategoryDescription)]
        public void GetServiceInstance_NullIServiceCollectionInstance_Returns_NullClassInstance()
        {
            ILogger logger = ServiceCollectionHelper.GetServiceInstance<ILogger>(null);

            Assert.IsNull(logger);
        }

        [TestMethod]
        [TestCategory(TestCategoryDescription)]
        public void GetServiceInstance_TypeNotRegistered_ReturnsNull()
        {
            MockServiceCollection msc = new MockServiceCollection();
            ILogger logger = ServiceCollectionHelper.GetServiceInstance<ILogger>(msc);

            Assert.IsNull(logger);
        }

        [TestMethod]
        [TestCategory(TestCategoryDescription)]
        public void GetServiceInstance_Singleton_ImplementationInstance_ReturnsInstance_Success()
        {
            MockLogger testLogger = new MockLogger();

            MockServiceCollection msc = new MockServiceCollection();
            msc.AddSingleton<ILogger>(testLogger);

            ILogger logger = ServiceCollectionHelper.GetServiceInstance<ILogger>(msc);

            Assert.IsNotNull(logger);
            Assert.AreSame(testLogger, logger);
        }

        [TestMethod]
        [TestCategory(TestCategoryDescription)]
        public void GetServiceInstance_Singleton_ImplementationType_ReturnsInstance_Success()
        {
            MockServiceCollection msc = new MockServiceCollection();
            msc.AddSingleton<ILogger, MockLogger>();
            Assert.IsNull(msc[0].ImplementationInstance);
            Assert.IsNull(msc[0].ImplementationFactory);
            Assert.IsNotNull(msc[0].ImplementationType);


            ILogger logger = ServiceCollectionHelper.GetServiceInstance<ILogger>(msc);

            Assert.IsNotNull(msc[0].ImplementationInstance);
            Assert.IsNull(msc[0].ImplementationType);
            Assert.IsNotNull(logger);
        }

        [TestMethod]
        [TestCategory(TestCategoryDescription)]
        public void GetServiceInstance_Singleton_ImplementationFactory_ReturnsInstance_Success()
        {
            MockLogger testLogger = new MockLogger();

            MockServiceCollection msc = new MockServiceCollection();
            msc.AddSingleton<ILogger>(factory => testLogger);

            Assert.IsNotNull(msc[0].ImplementationFactory);
            Assert.IsNull(msc[0].ImplementationInstance);
            Assert.IsNull(msc[0].ImplementationType);


            ILogger logger = ServiceCollectionHelper.GetServiceInstance<ILogger>(msc);

            Assert.IsNull(msc[0].ImplementationInstance);
            Assert.IsNull(msc[0].ImplementationType);
            Assert.IsNotNull(logger);
            Assert.AreSame(logger, testLogger);
        }

        [TestMethod]
        [TestCategory(TestCategoryDescription)]
        public void GetServiceInstance_Transient_ImplementationFactory_ReturnsInstance_Success()
        {
            MockLogger testLogger = new MockLogger();

            MockServiceCollection msc = new MockServiceCollection();
            msc.AddTransient<ILogger>(factory => testLogger);

            Assert.IsNotNull(msc[0].ImplementationFactory);
            Assert.IsNull(msc[0].ImplementationInstance);
            Assert.IsNull(msc[0].ImplementationType);


            ILogger logger = ServiceCollectionHelper.GetServiceInstance<ILogger>(msc);

            Assert.IsNull(msc[0].ImplementationInstance);
            Assert.IsNull(msc[0].ImplementationType);
            Assert.IsNotNull(logger);
            Assert.AreSame(logger, testLogger);
        }

        [TestMethod]
        [TestCategory(TestCategoryDescription)]
        public void GetServiceInstance_Transient_ImplementationType_ReturnsInstance_Success()
        {
            MockServiceCollection msc = new MockServiceCollection();
            msc.AddTransient<ILogger, MockLogger>();

            Assert.IsNull(msc[0].ImplementationFactory);
            Assert.IsNull(msc[0].ImplementationInstance);
            Assert.IsNotNull(msc[0].ImplementationType);


            ILogger logger = ServiceCollectionHelper.GetServiceInstance<ILogger>(msc);

            Assert.IsNull(msc[0].ImplementationFactory);
            Assert.IsNull(msc[0].ImplementationInstance);
            Assert.IsNotNull(msc[0].ImplementationType);
            Assert.IsNotNull(logger);
        }

        private class TestClassWithConstructorParams
        {
            private readonly ILogger _logger;

            public TestClassWithConstructorParams(ILogger logger)
            {
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public ILogger Logger => _logger;
        }

        private class TestClassWithMultipleConstructors
        {
            private readonly ILogger _logger;
            private readonly IPluginClassesService _pluginClassesService;
            private readonly IPluginHelperService _pluginHelperService;

            protected TestClassWithMultipleConstructors(ILogger logger, IPluginClassesService pluginClassesService)
                : this(logger)
            {
                _pluginClassesService = pluginClassesService;
            }

            public TestClassWithMultipleConstructors(ILogger logger, IPluginClassesService pluginClassesService, IPluginHelperService pluginHelperService)
            {
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
                _pluginClassesService = pluginClassesService ?? throw new ArgumentNullException(nameof(pluginClassesService));
                _pluginHelperService = pluginHelperService ?? throw new ArgumentNullException(nameof(pluginHelperService));
            }

            public TestClassWithMultipleConstructors(ILogger logger)
            {
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public TestClassWithMultipleConstructors()
            {

            }

            static TestClassWithMultipleConstructors()
            {

            }

            public ILogger Logger => _logger;

            public IPluginClassesService PluginClassesService => _pluginClassesService;

            public IPluginHelperService PluginHelperService => _pluginHelperService;
        }

        [TestMethod]
        [TestCategory(TestCategoryDescription)]
        [ExpectedException(typeof(MissingMethodException))]
        public void GetServiceInstance_ConstructInstanceWithMissingConstructorClassesRegistered_Throws_MissingMethodException()
        {
            MockServiceCollection msc = new MockServiceCollection();
            msc.AddTransient<TestClassWithConstructorParams>();

            Assert.IsNull(msc[0].ImplementationFactory);
            Assert.IsNull(msc[0].ImplementationInstance);
            Assert.IsNotNull(msc[0].ImplementationType);

            TestClassWithConstructorParams testClass = ServiceCollectionHelper.GetServiceInstance<TestClassWithConstructorParams>(msc);
        }

        [TestMethod]
        [TestCategory(TestCategoryDescription)]
        public void GetServiceInstance_ConstructInstanceWithConstructorClassesRegistered_Success()
        {
            MockLogger testLogger = new MockLogger();

            MockServiceCollection msc = new MockServiceCollection();
            msc.AddTransient<TestClassWithConstructorParams>();
            msc.AddTransient<ILogger>(factory => testLogger);

            Assert.IsNull(msc[0].ImplementationFactory);
            Assert.IsNull(msc[0].ImplementationInstance);
            Assert.IsNotNull(msc[0].ImplementationType);

            TestClassWithConstructorParams testClass = ServiceCollectionHelper.GetServiceInstance<TestClassWithConstructorParams>(msc);

            Assert.IsNull(msc[0].ImplementationFactory);
            Assert.IsNull(msc[0].ImplementationInstance);
            Assert.IsNotNull(msc[0].ImplementationType);

            Assert.IsNotNull(testClass);
            Assert.AreEqual(testClass.Logger, testLogger);
        }

        [TestMethod]
        [TestCategory(TestCategoryDescription)]
        public void GetServiceInstance_ConstructInstanceWithMultipleConstructor_NotAllClassesRegistered_Success()
        {
            MockLogger testLogger = new MockLogger();

            MockServiceCollection msc = new MockServiceCollection();
            msc.AddTransient<TestClassWithMultipleConstructors>();
            msc.AddTransient<ILogger>(factory => testLogger);

            Assert.IsNull(msc[0].ImplementationFactory);
            Assert.IsNull(msc[0].ImplementationInstance);
            Assert.IsNotNull(msc[0].ImplementationType);

            TestClassWithMultipleConstructors testClass = ServiceCollectionHelper.GetServiceInstance<TestClassWithMultipleConstructors>(msc);

            Assert.IsNull(msc[0].ImplementationFactory);
            Assert.IsNull(msc[0].ImplementationInstance);
            Assert.IsNotNull(msc[0].ImplementationType);

            Assert.IsNotNull(testClass);
            Assert.AreEqual(testClass.Logger, testLogger);
            Assert.IsNull(testClass.PluginClassesService);
            Assert.IsNull(testClass.PluginHelperService);
        }
    }
}
