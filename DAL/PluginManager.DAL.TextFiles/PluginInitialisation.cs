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
using Middleware.Helpdesk;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Internal;
using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;

using SharedPluginFeatures;

namespace PluginManager.DAL.TextFiles
{
    public class PluginInitialisation : IPlugin, IPluginVersion, IInitialiseEvents
    {
        #region IPlugin Methods

        public void ConfigureServices(IServiceCollection services)
        {

        }

        public void Finalise()
        {

        }

        public void Initialise(ILogger logger)
        {

        }

        public ushort GetVersion()
        {
            return 1;
        }

        #endregion IPlugin Methods

        #region IInitialiseEvents Methods

        public void BeforeConfigure(in IApplicationBuilder app)
        {

        }

        public void AfterConfigure(in IApplicationBuilder app)
        {
            // initialize all tables
            _ = app.ApplicationServices.GetService<ITextTableOperations<AddressDataRow>>();

            _ = app.ApplicationServices.GetService<ITextTableOperations<BlogDataRow>>();
            _ = app.ApplicationServices.GetService<ITextTableOperations<BlogCommentDataRow>>();

            _ = app.ApplicationServices.GetService<ITextTableOperations<CountryDataRow>>();

            _ = app.ApplicationServices.GetService<ITextTableOperations<ExternalUsersDataRow>>();

            _ = app.ApplicationServices.GetService<ITextTableOperations<FeedbackDataRow>>();
            _ = app.ApplicationServices.GetService<ITextTableOperations<TicketDataRow>>();
            _ = app.ApplicationServices.GetService<ITextTableOperations<TicketDepartmentsDataRow>>();
            _ = app.ApplicationServices.GetService<ITextTableOperations<TicketMessageDataRow>>();
            _ = app.ApplicationServices.GetService<ITextTableOperations<TicketPrioritiesDataRow>>();
            _ = app.ApplicationServices.GetService<ITextTableOperations<TicketStatusDataRow>>();
            _ = app.ApplicationServices.GetService<ITextTableOperations<FAQDataRow>>();
            _ = app.ApplicationServices.GetService<ITextTableOperations<FAQItemDataRow>>();

            _ = app.ApplicationServices.GetService<ITextTableOperations<InvoiceDataRow>>();
            _ = app.ApplicationServices.GetService<ITextTableOperations<InvoiceItemDataRow>>();

            _ = app.ApplicationServices.GetService<ITextTableOperations<LicenseTypeDataRow>>();
            _ = app.ApplicationServices.GetService<ITextTableOperations<LicenseDataRow>>();

            _ = app.ApplicationServices.GetService<ITextTableOperations<OrderDataRow>>();
            _ = app.ApplicationServices.GetService<ITextTableOperations<OrderItemDataRow>>();

            _ = app.ApplicationServices.GetService<ITextTableOperations<ProductDataRow>>();
            _ = app.ApplicationServices.GetService<ITextTableOperations<ProductGroupDataRow>>();

            _ = app.ApplicationServices.GetService<ITextTableOperations<SeoDataRow>>();

            _ = app.ApplicationServices.GetService<ITextTableOperations<SettingsDataRow>>();

            _ = app.ApplicationServices.GetService<ITextTableOperations<ShoppingCartDataRow>>();
            _ = app.ApplicationServices.GetService<ITextTableOperations<ShoppingCartItemDataRow>>();
            _ = app.ApplicationServices.GetService<ITextTableOperations<VoucherDataRow>>();

            _ = app.ApplicationServices.GetService<ITextTableOperations<StockDataRow>>();

            _ = app.ApplicationServices.GetService<ITextTableOperations<UserApiDataRow>>();

            _ = app.ApplicationServices.GetService<ITextTableOperations<UserDataRow>>();
            _ = app.ApplicationServices.GetService<ITextTableOperations<UserClaimsDataRow>>();

        }

        public void Configure(in IApplicationBuilder app)
        {

        }

        public void BeforeConfigureServices(in IServiceCollection services)
        {
            services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
            services.AddSingleton<ITextTableInitializer, TextTableInitializer>();

            // register tables
            services.AddSingleton(typeof(TableRowDefinition), typeof(AddressDataRow));

            services.AddSingleton(typeof(TableRowDefinition), typeof(BlogDataRow));
            services.AddSingleton(typeof(TableRowDefinition), typeof(BlogCommentDataRow));

            services.AddSingleton(typeof(TableRowDefinition), typeof(CountryDataRow));

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

            services.AddSingleton(typeof(TableRowDefinition), typeof(SeoDataRow));

            services.AddSingleton(typeof(TableRowDefinition), typeof(SettingsDataRow));

            services.AddSingleton(typeof(TableRowDefinition), typeof(ShoppingCartDataRow));
            services.AddSingleton(typeof(TableRowDefinition), typeof(ShoppingCartItemDataRow));
            services.AddSingleton(typeof(TableRowDefinition), typeof(VoucherDataRow));

            services.AddSingleton(typeof(TableRowDefinition), typeof(StockDataRow));

            services.AddSingleton(typeof(TableRowDefinition), typeof(UserApiDataRow));

            services.AddSingleton(typeof(TableRowDefinition), typeof(UserClaimsDataRow));
            services.AddSingleton(typeof(TableRowDefinition), typeof(UserDataRow));


            services.AddSingleton(typeof(ITextTableOperations<>), typeof(TextTableOperations<>));

            // register providers
            services.AddTransient<IAccountProvider, AccountProvider>();
            services.AddSingleton<IBlogProvider, BlogProvider>();
            services.AddSingleton<IClaimsProvider, ClaimsProvider>();
            services.AddSingleton<ICountryProvider, CountryProvider>();
            //services.AddSingleton<IDownloadProvider, DownloadProvider>();
            //services.AddSingleton<IDynamicContentProvider, DynamicContentProvider>();
            services.AddSingleton<IHelpdeskProvider, HelpdeskProvider>();
            services.AddSingleton<ILicenceProvider, LicenceProvider>();
            services.AddSingleton<ILoginProvider, LoginProvider>();
            services.AddSingleton<IProductProvider, ProductProvider>();
            services.AddSingleton<ISeoProvider, SeoProvider>();
            services.AddSingleton<IShoppingCartProvider, ShoppingCartProvider>();
            services.AddSingleton<IShoppingCartService, ShoppingCartProvider>();
            services.AddSingleton<IStockProvider, StockProvider>();
            services.AddSingleton<IUserApiQueryProvider, UserApiQueryProvider>();
            services.AddTransient<IUserSearch, UserSearch>();
            services.AddTransient<IApplicationSettingsProvider, SettingsProvider> ();
        }

        public void AfterConfigureServices(in IServiceCollection services)
        {

        }

        #endregion IInitialiseEvents Methods
    }
}
