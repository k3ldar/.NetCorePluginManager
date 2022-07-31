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
 *  File: CountriesProviderTests.cs
 *
 *  Purpose:  Countries test for text based storage
 *
 *  Date        Name                Reason
 *  07/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;

using PluginManager.SimpleDB;
using PluginManager.SimpleDB.Tests.Mocks;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CountriesProviderTests : BaseProviderTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_CountriesNull_Throws_ArgumentNullException()
        {
            new CountryProvider(null);
        }

        [TestMethod]
        public void Construct_ValidInstance_Success()
        {
            CountryProvider sut = new CountryProvider(new MockTextTableOperations<CountryDataRow>());

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CountryCreate_InvalidParamName_Null_Throws_ArgumentNullException()
        {
            CountryProvider sut = new CountryProvider(new MockTextTableOperations<CountryDataRow>());

            Assert.IsNotNull(sut);

            sut.CountryCreate(null, "DK", true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CountryCreate_InvalidParamCode_Null_Throws_ArgumentNullException()
        {
            CountryProvider sut = new CountryProvider(new MockTextTableOperations<CountryDataRow>());

            Assert.IsNotNull(sut);

            sut.CountryCreate("Angola", null, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CountryCreate_InvalidParamCode_LengthGreaterThan3_Throws_ArgumentException()
        {
            CountryProvider sut = new CountryProvider(new MockTextTableOperations<CountryDataRow>());

            Assert.IsNotNull(sut);

            sut.CountryCreate("Angola", "Angola", true);
        }

        [TestMethod]
        public void CountryCreate_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(new MockPluginClassesService(new List<object>() { new UserDataRowTriggers() }));
                services.AddSingleton<IPluginClassesService>(new MockPluginClassesService(new List<object>() { new UserDataRowTriggers() }));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<CountryDataRow> countryTable = provider.GetRequiredService<ITextTableOperations<CountryDataRow>>();
                    Assert.IsNotNull(countryTable);

                    ICountryProvider sut = provider.GetRequiredService<ICountryProvider>();

                    Assert.IsNotNull(sut);
                    sut.CountryCreate("Unknown", "UK", false);

                    IReadOnlyList<CountryDataRow> countries = countryTable.Select();
                    Assert.IsNotNull(countries);
                    Assert.AreEqual(1, countries.Count);
                    Assert.AreEqual("Unknown", countries[0].Name);
                    Assert.AreEqual("UK", countries[0].Code);
                    Assert.IsFalse(countries[0].Visible);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UniqueIndexException))]
        public void CountryCreate_DuplicateCountryCode_Throws_UniqueIndexException()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(new MockPluginClassesService(new List<object>() { new UserDataRowTriggers() }));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<CountryDataRow> countryTable = provider.GetRequiredService<ITextTableOperations<CountryDataRow>>();
                    Assert.IsNotNull(countryTable);

                    ICountryProvider sut = provider.GetRequiredService<ICountryProvider>();

                    Assert.IsNotNull(sut);
                    sut.CountryCreate("Unknown", "UK", false);

                    IReadOnlyList<CountryDataRow> countries = countryTable.Select();
                    Assert.IsNotNull(countries);
                    Assert.AreEqual(1, countries.Count);
                    Assert.AreEqual("Unknown", countries[0].Name);
                    Assert.AreEqual("UK", countries[0].Code);
                    Assert.IsFalse(countries[0].Visible);

                    sut.CountryCreate("Unknown", "UK", false);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void CountryUpdate_CountryNotFound_ReturnsFalse()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(new MockPluginClassesService(new List<object>() { new UserDataRowTriggers() }));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<CountryDataRow> countryTable = provider.GetRequiredService<ITextTableOperations<CountryDataRow>>();
                    Assert.IsNotNull(countryTable);

                    ICountryProvider sut = provider.GetRequiredService<ICountryProvider>();

                    Assert.IsNotNull(sut);
                    sut.CountryCreate("Unknown", "UK", false);
                    sut.CountryCreate("USA", "US", true);
                    sut.CountryCreate("Great Britain", "GB", true);

                    bool updated = sut.CountryUpdate(new Country("we do not know", "ZZ", true));
                    Assert.IsFalse(updated);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void CountryUpdate_NewNameAndVisibilityApplied_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(new MockPluginClassesService(new List<object>() { new UserDataRowTriggers() }));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<CountryDataRow> countryTable = provider.GetRequiredService<ITextTableOperations<CountryDataRow>>();
                    Assert.IsNotNull(countryTable);

                    ICountryProvider sut = provider.GetRequiredService<ICountryProvider>();

                    Assert.IsNotNull(sut);
                    sut.CountryCreate("Unknown", "UK", false);
                    sut.CountryCreate("USA", "US", true);
                    sut.CountryCreate("Great Britain", "GB", true);

                    Assert.IsNull(sut.GetVisibleCountries().Where(c => c.Code.Equals("UK")).FirstOrDefault());

                    bool updated = sut.CountryUpdate(new Country("we do not know", "UK", true));
                    Assert.IsTrue(updated);
                    Country country = sut.GetVisibleCountries().Where(c => c.Code.Equals("UK")).FirstOrDefault();

                    Assert.IsNotNull(country);
                    Assert.AreEqual("we do not know", country.Name);
                    Assert.AreEqual("UK", country.Code);
                    Assert.IsTrue(country.Visible);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetAllCountries_RetrievesVisibileAndNonVisibileCountries_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(new MockPluginClassesService(new List<object>() { new UserDataRowTriggers() }));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<CountryDataRow> countryTable = provider.GetRequiredService<ITextTableOperations<CountryDataRow>>();
                    Assert.IsNotNull(countryTable);

                    ICountryProvider sut = provider.GetRequiredService<ICountryProvider>();

                    Assert.IsNotNull(sut);
                    sut.CountryCreate("Unknown", "UK", false);
                    sut.CountryCreate("USA", "US", true);
                    sut.CountryCreate("Great Britain", "GB", true);

                    List<Country> allCountries = sut.GetAllCountries();
                    Assert.IsNotNull(allCountries);
                    Assert.AreEqual(3, allCountries.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void CountryDelete_CountryCodeNotFound_ReturnsFalse()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(new MockPluginClassesService(new List<object>() { new UserDataRowTriggers() }));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<CountryDataRow> countryTable = provider.GetRequiredService<ITextTableOperations<CountryDataRow>>();
                    Assert.IsNotNull(countryTable);

                    ICountryProvider sut = provider.GetRequiredService<ICountryProvider>();

                    Assert.IsNotNull(sut);
                    sut.CountryCreate("Unknown", "UK", false);
                    sut.CountryCreate("USA", "US", true);
                    sut.CountryCreate("Great Britain", "GB", true);

                    bool updated = sut.CountryDelete(new Country("we do not know", "ZZ", true));
                    Assert.IsFalse(updated);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void CountryDelete_CountryFoundAndDeleted_ReturnsTrue()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(new MockPluginClassesService(new List<object>() { new UserDataRowTriggers() }));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<CountryDataRow> countryTable = provider.GetRequiredService<ITextTableOperations<CountryDataRow>>();
                    Assert.IsNotNull(countryTable);

                    ICountryProvider sut = provider.GetRequiredService<ICountryProvider>();

                    Assert.IsNotNull(sut);
                    sut.CountryCreate("Unknown", "UK", false);
                    sut.CountryCreate("USA", "US", true);
                    sut.CountryCreate("Great Britain", "GB", true);

                    bool updated = sut.CountryDelete(new Country("we do not know", "UK", true));
                    Assert.IsTrue(updated);

                    List<Country> allCountries = sut.GetAllCountries();

                    Assert.IsFalse(allCountries.Where(c => c.Code.Equals("UK")).Any());
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void CountryCreate_WithDefaultData_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(new MockPluginClassesService(new List<object>() { new CountryDataRowDefaults() }));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<CountryDataRow> countryTable = provider.GetRequiredService<ITextTableOperations<CountryDataRow>>();
                    Assert.IsNotNull(countryTable);

                    Assert.AreEqual(251, countryTable.RecordCount);

                    CountryDataRow unknown = countryTable.Select().Where(c => c.Code.Equals("ZZ")).FirstOrDefault();
                    Assert.IsNotNull(unknown);
                    Assert.IsFalse(unknown.Visible);
                    Assert.AreEqual(1, unknown.Id);

                    Assert.AreEqual(250, countryTable.Select().Where(c => c.Visible).Count());
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }
    }
}
