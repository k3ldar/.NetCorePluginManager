﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Product:  SimpleDB
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

using PluginManager.Abstractions;
using SimpleDB.Internal;

using SharedPluginFeatures;

namespace SimpleDB
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
            services.AddSingleton<ISimpleDBInitializer, SimpleDBInitializer>();
            services.AddSingleton(typeof(ISimpleDBOperations<>), typeof(SimpleDBOperations<>));
        }

        public void AfterConfigureServices(in IServiceCollection services)
        {

        }

        #endregion IInitialiseEvents Methods
    }
}