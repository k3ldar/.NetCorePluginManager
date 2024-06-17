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
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;

using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;
using PluginManager.Tests.Mocks;

using SimpleDB;
using SimpleDB.Tests.Mocks;

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
			CountryProvider sut = new(new MockTextTableOperations<CountryDataRow>());

			Assert.IsNotNull(sut);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CountryCreate_InvalidParamName_Null_Throws_ArgumentNullException()
		{
			CountryProvider sut = new(new MockTextTableOperations<CountryDataRow>());

			Assert.IsNotNull(sut);

			sut.CountryCreate(null, "DK", true);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CountryCreate_InvalidParamCode_Null_Throws_ArgumentNullException()
		{
			CountryProvider sut = new(new MockTextTableOperations<CountryDataRow>());

			Assert.IsNotNull(sut);

			sut.CountryCreate("Angola", null, true);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void CountryCreate_InvalidParamCode_LengthGreaterThan3_Throws_ArgumentException()
		{
			CountryProvider sut = new(new MockTextTableOperations<CountryDataRow>());

			Assert.IsNotNull(sut);

			sut.CountryCreate("Angola", "Angola", true);
		}

		[TestMethod]
		public void CountryCreate_Success()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out PluginInitialisation pluginInitialisation, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					pluginInitialisation.AfterConfigure(new MockApplicationBuilder(provider));

					ISimpleDBOperations<CountryDataRow> countryTable = provider.GetRequiredService<ISimpleDBOperations<CountryDataRow>>();
					Assert.IsNotNull(countryTable);

					ICountryProvider sut = provider.GetRequiredService<ICountryProvider>();

					Assert.IsNotNull(sut);
					sut.CountryCreate("Unknown", "UK", false);

					IReadOnlyList<CountryDataRow> countries = countryTable.Select();
					Assert.IsNotNull(countries);
					Assert.AreEqual(252, countries.Count);
					Assert.AreEqual("Unknown", countries[251].Name);
					Assert.AreEqual("UK", countries[251].Code);
					Assert.IsFalse(countries[251].Visible);
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
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out PluginInitialisation pluginInitialisation, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					pluginInitialisation.AfterConfigure(new MockApplicationBuilder(provider));

					ISimpleDBOperations<CountryDataRow> countryTable = provider.GetRequiredService<ISimpleDBOperations<CountryDataRow>>();
					Assert.IsNotNull(countryTable);

					ICountryProvider sut = provider.GetRequiredService<ICountryProvider>();

					Assert.IsNotNull(sut);
					sut.CountryCreate("Unknown", "UK", false);

					IReadOnlyList<CountryDataRow> countries = countryTable.Select();
					Assert.IsNotNull(countries);
					Assert.AreEqual(252, countries.Count);
					Assert.AreEqual("Unknown", countries[251].Name);
					Assert.AreEqual("UK", countries[251].Code);
					Assert.IsFalse(countries[251].Visible);

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
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISimpleDBOperations<CountryDataRow> countryTable = provider.GetRequiredService<ISimpleDBOperations<CountryDataRow>>();
					Assert.IsNotNull(countryTable);

					ICountryProvider sut = provider.GetRequiredService<ICountryProvider>();

					Assert.IsNotNull(sut);

					bool updated = sut.CountryUpdate(new Country("we do not know", "XX", true));
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
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISimpleDBOperations<CountryDataRow> countryTable = provider.GetRequiredService<ISimpleDBOperations<CountryDataRow>>();
					Assert.IsNotNull(countryTable);

					ICountryProvider sut = provider.GetRequiredService<ICountryProvider>();

					Assert.IsNotNull(sut);
					sut.CountryCreate("Unknown", "XX", false);

					Assert.IsNull(sut.GetVisibleCountries().Find(c => c.Code.Equals("XX")));

					bool updated = sut.CountryUpdate(new Country("we do not know", "XX", true));
					Assert.IsTrue(updated);
					Country country = sut.GetVisibleCountries().Find(c => c.Code.Equals("XX"));

					Assert.IsNotNull(country);
					Assert.AreEqual("we do not know", country.Name);
					Assert.AreEqual("XX", country.Code);
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
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out PluginInitialisation pluginInitialisation, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					pluginInitialisation.AfterConfigure(new MockApplicationBuilder(provider));

					ISimpleDBOperations<CountryDataRow> countryTable = provider.GetRequiredService<ISimpleDBOperations<CountryDataRow>>();
					Assert.IsNotNull(countryTable);

					ICountryProvider sut = provider.GetRequiredService<ICountryProvider>();

					Assert.IsNotNull(sut);

					List<Country> allCountries = sut.GetAllCountries();
					Assert.IsNotNull(allCountries);
					Assert.AreEqual(251, allCountries.Count);
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
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISimpleDBOperations<CountryDataRow> countryTable = provider.GetRequiredService<ISimpleDBOperations<CountryDataRow>>();
					Assert.IsNotNull(countryTable);

					ICountryProvider sut = provider.GetRequiredService<ICountryProvider>();

					Assert.IsNotNull(sut);

					bool updated = sut.CountryDelete(new Country("we do not know", "XX", true));
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
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISimpleDBOperations<CountryDataRow> countryTable = provider.GetRequiredService<ISimpleDBOperations<CountryDataRow>>();
					Assert.IsNotNull(countryTable);

					ICountryProvider sut = provider.GetRequiredService<ICountryProvider>();

					Assert.IsNotNull(sut);
					sut.CountryCreate("Unknown", "XX", false);

					bool updated = sut.CountryDelete(new Country("we do not know", "XX", true));
					Assert.IsTrue(updated);

					List<Country> allCountries = sut.GetAllCountries();

					Assert.IsFalse(allCountries.Exists(c => c.Code.Equals("XX")));
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
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out PluginInitialisation pluginInitialisation, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					pluginInitialisation.AfterConfigure(new MockApplicationBuilder(provider));

					ISimpleDBOperations<CountryDataRow> countryTable = provider.GetRequiredService<ISimpleDBOperations<CountryDataRow>>();
					Assert.IsNotNull(countryTable);

					Assert.AreEqual(251, countryTable.RecordCount);

					CountryDataRow unknown = countryTable.Select().FirstOrDefault(c => c.Code.Equals("ZZ"));
					Assert.IsNotNull(unknown);
					Assert.IsFalse(unknown.Visible);
					Assert.AreEqual(1, unknown.Id);

					Assert.AreEqual(250, countryTable.Select().Count(c => c.Visible));
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}
	}
}
