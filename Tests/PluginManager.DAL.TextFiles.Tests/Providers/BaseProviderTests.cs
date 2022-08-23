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
 *  File: BaseProviderTests.cs
 *
 *  Purpose:  BaseProviderTests class for text based storage
 *
 *  Date        Name                Reason
 *  06/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;

using SimpleDB.Internal;
using SimpleDB;
using SharedPluginFeatures;
using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Tables;
using DynamicContent.Plugin.Templates;
using PluginManager.DAL.TextFiles.Tables.Products;
using PluginManager.Tests.Mocks;
using PluginManager.DAL.TextFiles.Tables.Sessions;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
	[ExcludeFromCodeCoverage]
    public class BaseProviderTests
    {
        protected const string TestPathSettings = "{\"SimpleDBSettings\":{\"Path\":\"$$\"}}";

        protected IProductProvider GetTestProductProvider(ServiceProvider provider, bool addProducts = true)
        {
            IProductProvider Result = provider.GetService<IProductProvider>();
            Assert.IsNotNull(Result);

            if (addProducts)
            {
                if (!Result.ProductSave(-1, 1, "test product", "This is a description of my test product 1", "", "", true, false, 1.99m, "sku1", false, true, out string errorMessage))
                    throw new InvalidOperationException("product should have saved; Error: " + errorMessage);

                if (!Result.ProductSave(-1, 1, "test product", "This is a description of my test product 2", "", "", true, false, 2.99m, "sku2", false, true, out errorMessage))
                    throw new InvalidOperationException("product should have saved; Error: " + errorMessage);

                if (!Result.ProductSave(-1, 1, "test product", "This is a description of my test product 3", "", "", true, false, 3.99m, "sku3", false, true, out errorMessage))
                    throw new InvalidOperationException("product should have saved; Error: " + errorMessage);
            }

            return Result;
        }

		protected ServiceCollection CreateDefaultServiceCollection(string directory, out MockPluginClassesService mockPluginClassesService)
		{
			PluginInitialisation initialisation = new PluginInitialisation();
			ServiceCollection services = new ServiceCollection();

			services.AddSingleton<IMemoryCache, MockMemoryCache>();
			services.AddSingleton<IPluginClassesService, MockPluginClassesService>();
			services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
			services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
			services.AddSingleton<ISimpleDBManager, SimpleDBManager>();

			services.AddSingleton(typeof(ISimpleDBOperations<>), typeof(SimpleDBOperations<>));

			ISettingsProvider settingsProvider = new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\")));

			List<object> classServices = new List<object>()
				{
					new TicketDepartmentsDataRowDefaults(),
					new TicketStatusDataRowDefaults(),
					new TicketPrioritiesDataRowDefaults(),
					new DownloadItemsDataRowDefaults(),
					new UserDataRowDefaults(settingsProvider),
					new HorizontalRuleTemplate(),
					new HtmlTextTemplate(),
					new YouTubeVideoTemplate(),
					new SettingsDataRowDefaults(),
					new ProductGroupDataRowDefaults(),
					new ProductGroupDataTriggers(),
					new ProductDataTriggers(),
					new ShoppingCartDataRowDefaults(),
					new AddressDataRowDefaults(),
					new VoucherDataRowTriggers(),
					new ExternalUsersDataRowDefaults(),
					new CountryDataRowDefaults(),
					new FAQDataRowDefaults(),
					new InvoiceDataRowTriggers(),
					new InvoiceItemDataRowTriggers(),
					new TicketDepartmentsDataRowDefaults(),
					new SessionDataRowDefaults(),
				};

			mockPluginClassesService = new MockPluginClassesService(classServices);

			services.AddSingleton<ISettingsProvider>(settingsProvider);
			services.AddSingleton<IPluginClassesService>(mockPluginClassesService);

			services.AddSingleton<ILogger>(new MockLogger());

			initialisation.BeforeConfigureServices(services);

			return services;
		}
	}
}
