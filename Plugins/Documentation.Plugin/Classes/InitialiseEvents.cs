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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Documentation Plugin
 *  
 *  File: InitialiseEvents.cs
 *
 *  Purpose:  Adds a default documentation service implementation if one 
 *  has not already been included
 *
 *  Date        Name                Reason
 *  19/05/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Threading;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using PluginManager.Abstractions;

using SharedPluginFeatures;

using static SharedPluginFeatures.Constants;

#pragma warning disable CS1591

namespace DocumentationPlugin.Classes
{
    /// <summary>
    /// Implements IPlugin which allows the Documentation.Plugin module to be
    /// loaded as a plugin module.  Extends IInitialiseEvents interface.
    /// </summary>
    public sealed class InitialiseEvents : IInitialiseEvents
    {
        public void AfterConfigure(in IApplicationBuilder app)
        {

        }

        public void AfterConfigureServices(in IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            IDocumentationService defaultDocumentation = new DefaultDocumentationService(
                serviceProvider.GetRequiredService<ISettingsProvider>(),
                serviceProvider.GetRequiredService<IMemoryCache>());

            if (serviceProvider.GetService(typeof(IDocumentationService)) == null)
                services.AddSingleton(defaultDocumentation);

            DocumentLoadThread documentLoadThread = new DocumentLoadThread(defaultDocumentation as IDocumentationService);
            Shared.Classes.ThreadManager.ThreadStart(documentLoadThread,
                DocumentationLoadThread,
                ThreadPriority.BelowNormal);
        }

        public void BeforeConfigure(in IApplicationBuilder app)
        {

        }

        public void BeforeConfigureServices(in IServiceCollection services)
        {

        }

        public void Configure(in IApplicationBuilder app)
        {

        }
    }
}

#pragma warning restore CS1591