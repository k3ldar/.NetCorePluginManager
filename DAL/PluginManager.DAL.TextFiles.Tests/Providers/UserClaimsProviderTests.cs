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
 *  File: UserClaimsTests.cs
 *
 *  Purpose:  User claims tests for text based storage
 *
 *  Date        Name                Reason
 *  06/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Claims;
using System.Linq;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Middleware.Accounts;

using PluginManager.Abstractions;

using SharedPluginFeatures;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class UserClaimsProviderTests : BaseProviderTests
    {
        [TestMethod]
        public void SetClaimsForUser_UserDoesNotExists_ReturnsEmptyList()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    IClaimsProvider sut = provider.GetRequiredService<IClaimsProvider>();

                    Assert.IsNotNull(sut);

                    List<ClaimsIdentity> userClaims = sut.GetUserClaims(3);
                    Assert.IsNotNull(userClaims);
                    Assert.AreEqual(0, userClaims.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void SetClaimsForUser_UserDoesNotExist_ReturnsFalse()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    IAccountProvider accountProvider = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(accountProvider);

                    IClaimsProvider sut = provider.GetRequiredService<IClaimsProvider>();
                    Assert.IsNotNull(sut);

                    List<string> newClaims = new List<string>()
                    {
                        "Administrator",
                        "Staff",
                        "ManageSeo",
                        "ViewImageManager"
                    };

                    bool setClaimsResult = sut.SetClaimsForUser(100, newClaims);

                    Assert.IsFalse(setClaimsResult);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetUserClaims_NoAdditionalClaimsFound_ReturnsDefaultUserClaims()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    IAccountProvider accountProvider = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(accountProvider);

                    bool created = accountProvider.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    IClaimsProvider sut = provider.GetRequiredService<IClaimsProvider>();
                    Assert.IsNotNull(sut);


                    List<ClaimsIdentity> userClaims = sut.GetUserClaims(userId);
                    Assert.IsNotNull(userClaims);
                    Assert.AreEqual(1, userClaims.Count);
                    Assert.AreEqual("Name", userClaims[0].Claims.ToList()[0].Type);
                    Assert.AreEqual("Joe Bloggs", userClaims[0].Claims.ToList()[0].Value);
                    Assert.AreEqual("Email", userClaims[0].Claims.ToList()[1].Type);
                    Assert.AreEqual("me@here.com", userClaims[0].Claims.ToList()[1].Value);
                    Assert.AreEqual("UserId", userClaims[0].Claims.ToList()[2].Type);
                    Assert.AreEqual("0", userClaims[0].Claims.ToList()[2].Value);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void SetClaimsForUser_AddsCorrectClaimsForUser_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    IAccountProvider accountProvider = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(accountProvider);

                    bool created = accountProvider.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    IClaimsProvider sut = provider.GetRequiredService<IClaimsProvider>();
                    Assert.IsNotNull(sut);

                    List<string> newClaims = new List<string>()
                    { 
                        "Administrator",
                        "Staff",
                        "ManageSeo",
                        "ViewImageManager"
                    };

                    bool setClaimsResult = sut.SetClaimsForUser(userId, newClaims);

                    Assert.IsTrue(setClaimsResult);


                    List<ClaimsIdentity> userClaims = sut.GetUserClaims(userId);
                    Assert.IsNotNull(userClaims);
                    Assert.AreEqual(2, userClaims.Count);
                    Assert.AreEqual("Name", userClaims[0].Claims.ToList()[0].Type);
                    Assert.AreEqual("Joe Bloggs", userClaims[0].Claims.ToList()[0].Value);
                    Assert.AreEqual("Email", userClaims[0].Claims.ToList()[1].Type);
                    Assert.AreEqual("me@here.com", userClaims[0].Claims.ToList()[1].Value);
                    Assert.AreEqual("UserId", userClaims[0].Claims.ToList()[2].Type);
                    Assert.AreEqual("0", userClaims[0].Claims.ToList()[2].Value);

                    Assert.AreEqual("Administrator", userClaims[1].Claims.ToList()[0].Type);
                    Assert.AreEqual("True", userClaims[1].Claims.ToList()[0].Value);
                    Assert.AreEqual("Staff", userClaims[1].Claims.ToList()[1].Type);
                    Assert.AreEqual("True", userClaims[1].Claims.ToList()[1].Value);
                    Assert.AreEqual("ManageSeo", userClaims[1].Claims.ToList()[2].Type);
                    Assert.AreEqual("True", userClaims[1].Claims.ToList()[2].Value);
                    Assert.AreEqual("ViewImageManager", userClaims[1].Claims.ToList()[3].Type);
                    Assert.AreEqual("True", userClaims[1].Claims.ToList()[3].Value);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetClaimsForUser_UserHadAdditionalClaims_ReturnsCorrectList()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    IAccountProvider accountProvider = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(accountProvider);

                    bool created = accountProvider.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    IClaimsProvider sut = provider.GetRequiredService<IClaimsProvider>();
                    Assert.IsNotNull(sut);

                    List<string> newClaims = new List<string>()
                    {
                        "Administrator",
                        "Staff",
                        "ManageSeo",
                        "ViewImageManager"
                    };

                    bool setClaimsResult = sut.SetClaimsForUser(userId, newClaims);

                    Assert.IsTrue(setClaimsResult);

                    List<string> userClaims = sut.GetClaimsForUser(userId);
                    Assert.IsNotNull(userClaims);

                    Assert.AreEqual(4, userClaims.Count);
                    Assert.AreEqual("Administrator", userClaims[0]);
                    Assert.AreEqual("Staff", userClaims[1]);
                    Assert.AreEqual("ManageSeo", userClaims[2]);
                    Assert.AreEqual("ViewImageManager", userClaims[3]);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }
    }
}
