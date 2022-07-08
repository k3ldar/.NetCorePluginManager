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

        }

        public void Configure(in IApplicationBuilder app)
        {

        }

        public void BeforeConfigureServices(in IServiceCollection services)
        {
            services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
            services.AddSingleton<ITextTableInitializer, TextTableInitializer>();

            // register tables
            services.AddSingleton(typeof(TableRowDefinition), typeof(SettingsDataRow));
            services.AddSingleton(typeof(TableRowDefinition), typeof(UserDataRow));
            services.AddSingleton(typeof(TableRowDefinition), typeof(OrderDataRow));
            services.AddSingleton(typeof(TableRowDefinition), typeof(AddressDataRow));
            services.AddSingleton(typeof(TableRowDefinition), typeof(CountryDataRow));
            services.AddSingleton(typeof(TableRowDefinition), typeof(UserClaimsDataRow));
            services.AddSingleton(typeof(TableRowDefinition), typeof(ExternalUsersDataRow));
            services.AddSingleton(typeof(TableRowDefinition), typeof(SeoDataRow));
            services.AddSingleton(typeof(TableRowDefinition), typeof(UserApiDataRow));
            services.AddSingleton(typeof(TableRowDefinition), typeof(ProductGroupDataRow));
            services.AddSingleton(typeof(TableRowDefinition), typeof(ProductDataRow));
            services.AddSingleton(typeof(TableRowDefinition), typeof(BlogDataRow));
            services.AddSingleton(typeof(TableRowDefinition), typeof(BlogCommentDataRow));
            services.AddSingleton(typeof(TableRowDefinition), typeof(ShoppingCartDataRow));
            services.AddSingleton(typeof(TableRowDefinition), typeof(ShoppingCartItemDataRow));

            services.AddSingleton(typeof(ITextTableOperations<>), typeof(TextTableOperations<>));

            // register providers
            services.AddTransient<IAccountProvider, AccountProvider>();
            services.AddSingleton<IBlogProvider, BlogProvider>();
            services.AddSingleton<IClaimsProvider, ClaimsProvider>();
            services.AddSingleton<ICountryProvider, CountryProvider>();
            //services.AddSingleton<IDownloadProvider, DownloadProvider>();
            //services.AddSingleton<IDynamicContentProvider, DynamicContentProvider>();
            //services.AddSingleton<IHelpdeskProvider, HelpdeskProvider>();
            //services.AddSingleton<ILicenceProvider, LicenceProvider>();
            services.AddSingleton<ILoginProvider, LoginProvider>();
            services.AddSingleton<IProductProvider, ProductProvider>();
            services.AddSingleton<ISeoProvider, SeoProvider>();
            services.AddSingleton<IShoppingCartProvider, ShoppingCartProvider>();
            services.AddSingleton<IShoppingCartService, ShoppingCartProvider>();
            //services.AddSingleton<IStockProvider, StockProvider>();
            services.AddSingleton<IUserApiQueryProvider, UserApiQueryProvider>();
            services.AddTransient<IUserSearch, UserSearch>();
            services.AddTransient<SharedPluginFeatures.IApplicationSettingsProvider, SettingsProvider> ();
        }

        public void AfterConfigureServices(in IServiceCollection services)
        {

        }

        #endregion IInitialiseEvents Methods
    }
}
