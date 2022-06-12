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
 *  Product:  PluginManager.DAL.TextFiles.Tests
 *  
 *  File: LoginProviderTests.cs
 *
 *  Purpose:  LoginProviderTests Tests for text based storage
 *
 *  Date        Name                Reason
 *  12/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Accounts;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Internal;
using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;
using PluginManager.DAL.TextFiles.Tests.Mocks;

using SharedPluginFeatures;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class LoginProviderTests : BaseProviderTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidInstance_TableUserNull_Throws_ArgumentNullException()
        {
            new LoginProvider(null, new MockTextTableOperations<TableExternalUsers>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidInstance_TableExternalUsersNull_Throws_ArgumentNullException()
        {
            new LoginProvider(new MockTextTableOperations<TableUser>(), null);
        }

        [TestMethod]
        public void Construct_ValidInstance_Success()
        {
            LoginProvider sut = new LoginProvider(new MockTextTableOperations<TableUser>(), new MockTextTableOperations<TableExternalUsers>());
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void Validate_ExternalUsersIdStartsAtInt64Minimum_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();
                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    IPluginClassesService pluginClassesService = new MockPluginClassesService(new List<object>() { new TableExternalUsersDefaults() });
                    
                    ITextTableInitializer initializer = new TextTableInitializer(directory);
                    IForeignKeyManager keyManager = new ForeignKeyManager();
                    using (TextTableOperations<TableExternalUsers> sut = new TextTableOperations<TableExternalUsers>(initializer, keyManager, pluginClassesService))
                    {
                        Assert.IsNotNull(sut);

                        Assert.AreEqual(Int64.MinValue, sut.Sequence);
                    }
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }
    }
}
