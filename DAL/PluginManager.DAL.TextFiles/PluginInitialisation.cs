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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: PluginInitialisation.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  31/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Middleware;
using Middleware.Accounts;
using Middleware.DynamicContent;
using Middleware.Helpdesk;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;
using PluginManager.DAL.TextFiles.Tables.Resources;

using SharedPluginFeatures;

using SimpleDB;

namespace PluginManager.DAL.TextFiles
{
	public class PluginInitialisation : IPlugin, IInitialiseEvents
	{
		#region IPlugin Methods

		public void ConfigureServices(IServiceCollection services)
		{
			// from interface but unused in this context
		}

		public void Finalise()
		{
			// from interface but unused in this context
		}

		public void Initialise(ILogger logger)
		{
			// from interface but unused in this context
		}

		public ushort GetVersion()
		{
			return 1;
		}

		#endregion IPlugin Methods

		#region IInitialiseEvents Methods

		public void BeforeConfigure(in IApplicationBuilder app)
		{
			// from interface but unused in this context
		}

		public void AfterConfigure(in IApplicationBuilder app)
		{
			// initialize all tables
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<AddressDataRow>>();

			_ = app.ApplicationServices.GetService<ISimpleDBOperations<BlogDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<BlogCommentDataRow>>();

			_ = app.ApplicationServices.GetService<ISimpleDBOperations<CountryDataRow>>();

			_ = app.ApplicationServices.GetService<ISimpleDBOperations<DownloadCategoryDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<DownloadItemsDataRow>>();

			_ = app.ApplicationServices.GetService<ISimpleDBOperations<ContentPageDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<ContentPageItemDataRow>>();

			_ = app.ApplicationServices.GetService<ISimpleDBOperations<ExternalUsersDataRow>>();

			_ = app.ApplicationServices.GetService<ISimpleDBOperations<FeedbackDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<TicketDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<TicketDepartmentsDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<TicketMessageDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<TicketPrioritiesDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<TicketStatusDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<FAQDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<FAQItemDataRow>>();

			_ = app.ApplicationServices.GetService<ISimpleDBOperations<InvoiceDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<InvoiceItemDataRow>>();

			_ = app.ApplicationServices.GetService<ISimpleDBOperations<LicenseTypeDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<LicenseDataRow>>();

			_ = app.ApplicationServices.GetService<ISimpleDBOperations<OrderDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<OrderItemDataRow>>();

			_ = app.ApplicationServices.GetService<ISimpleDBOperations<ProductDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<ProductGroupDataRow>>();

			_ = app.ApplicationServices.GetService<ISimpleDBOperations<SeoDataRow>>();

			_ = app.ApplicationServices.GetService<ISimpleDBOperations<InitialReferralsDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<PageViewsDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<SessionDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<SessionPageDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<SessionStatsDailyDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<SessionStatsHourlyDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<SessionStatsWeeklyDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<SessionStatsMonthlyDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<SessionStatsYearlyDataRow>>();

			_ = app.ApplicationServices.GetService<ISimpleDBOperations<SettingsDataRow>>();

			_ = app.ApplicationServices.GetService<ISimpleDBOperations<ShoppingCartDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<ShoppingCartItemDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<VoucherDataRow>>();

			_ = app.ApplicationServices.GetService<ISimpleDBOperations<StockDataRow>>();

			_ = app.ApplicationServices.GetService<ISimpleDBOperations<ResourceCategoryDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<ResourceItemDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<ResourceItemUserResponseDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<ResourceBookmarkDataRow>>();

			_ = app.ApplicationServices.GetService<ISimpleDBOperations<UserApiDataRow>>();

			_ = app.ApplicationServices.GetService<ISimpleDBOperations<UserDataRow>>();
			_ = app.ApplicationServices.GetService<ISimpleDBOperations<UserClaimsDataRow>>();

			_ = app.ApplicationServices.GetService<IUserSessionService>();
		}

		public void Configure(in IApplicationBuilder app)
		{
			// from interface but unused in this context
		}

		public void BeforeConfigureServices(in IServiceCollection services)
		{
			// register tables
			services.AddSingleton(typeof(TableRowDefinition), typeof(AddressDataRow));

			services.AddSingleton(typeof(TableRowDefinition), typeof(BlogDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(BlogCommentDataRow));

			services.AddSingleton(typeof(TableRowDefinition), typeof(CountryDataRow));

			services.AddSingleton(typeof(TableRowDefinition), typeof(DownloadCategoryDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(DownloadItemsDataRow));

			services.AddSingleton(typeof(TableRowDefinition), typeof(ContentPageDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(ContentPageItemDataRow));

			services.AddSingleton(typeof(TableRowDefinition), typeof(ExternalUsersDataRow));

			services.AddSingleton(typeof(TableRowDefinition), typeof(FeedbackDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(TicketDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(TicketDepartmentsDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(TicketMessageDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(TicketPrioritiesDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(TicketStatusDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(FAQDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(FAQItemDataRow));

			services.AddSingleton(typeof(TableRowDefinition), typeof(InvoiceDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(InvoiceItemDataRow));

			services.AddSingleton(typeof(TableRowDefinition), typeof(LicenseTypeDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(LicenseDataRow));

			services.AddSingleton(typeof(TableRowDefinition), typeof(OrderDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(OrderItemDataRow));

			services.AddSingleton(typeof(TableRowDefinition), typeof(ProductDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(ProductGroupDataRow));

			services.AddSingleton(typeof(TableRowDefinition), typeof(ResourceCategoryDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(ResourceItemDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(ResourceItemUserResponseDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(ResourceBookmarkDataRow));

			services.AddSingleton(typeof(TableRowDefinition), typeof(SeoDataRow));

			services.AddSingleton(typeof(TableRowDefinition), typeof(InitialReferralsDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(PageViewsDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(SessionDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(SessionPageDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(SessionStatsDailyDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(SessionStatsHourlyDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(SessionStatsWeeklyDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(SessionStatsMonthlyDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(SessionStatsYearlyDataRow));

			services.AddSingleton(typeof(TableRowDefinition), typeof(SettingsDataRow));

			services.AddSingleton(typeof(TableRowDefinition), typeof(ShoppingCartDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(ShoppingCartItemDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(VoucherDataRow));

			services.AddSingleton(typeof(TableRowDefinition), typeof(StockDataRow));

			services.AddSingleton(typeof(TableRowDefinition), typeof(UserApiDataRow));

			services.AddSingleton(typeof(TableRowDefinition), typeof(UserClaimsDataRow));
			services.AddSingleton(typeof(TableRowDefinition), typeof(UserDataRow));

			// register providers
			services.AddTransient<IAccountProvider, AccountProvider>();
			services.AddSingleton<IBlogProvider, BlogProvider>();
			services.AddSingleton<IClaimsProvider, ClaimsProvider>();
			services.AddSingleton<ICountryProvider, CountryProvider>();
			services.AddSingleton<IDownloadProvider, DownloadProvider>();
			services.AddSingleton<IDynamicContentProvider, DynamicContentProvider>();
			services.AddSingleton<IHelpdeskProvider, HelpdeskProvider>();
			services.AddSingleton<ILicenceProvider, LicenceProvider>();
			services.AddSingleton<ILoginProvider, LoginProvider>();
			services.AddSingleton<IProductProvider, ProductProvider>();
			services.AddSingleton<IResourceProvider, ResourceProvider>();
			services.AddSingleton<ISeoProvider, SeoProvider>();
			services.AddSingleton<IShoppingCartProvider, ShoppingCartProvider>();
			services.AddSingleton<IShoppingCartService, ShoppingCartProvider>();
			services.AddSingleton<IStockProvider, StockProvider>();
			services.AddSingleton<IUserApiQueryProvider, UserApiQueryProvider>();
			services.AddTransient<IUserSearch, UserSearch>();
			services.AddTransient<IApplicationSettingsProvider, SettingsProvider>();
			services.AddSingleton<ISessionStatisticsProvider, SessionStatisticsProvider>();
			services.AddSingleton<IUrlHashProvider, UrlHashProvider>();
			services.AddSingleton<IUserSessionService, UserSessionService>();
		}

		public void AfterConfigureServices(in IServiceCollection services)
		{
			// from interface but unused in this context
		}

		#endregion IInitialiseEvents Methods
	}
}
