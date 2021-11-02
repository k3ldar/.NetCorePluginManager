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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  DynamicContent.Plugin
 *  
 *  File: PluginInitialisation.cs
 *
 *  Purpose:  Net Core Plugin Manager Integration
 *
 *  Date        Name                Reason
 *  13/08/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using DynamicContent.Plugin.Internal;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Middleware.DynamicContent;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

using static SharedPluginFeatures.Constants;

#pragma warning disable CS1591

namespace DynamicContent.Plugin
{
    /// <summary>
    /// Implements IPlugin and IInitialiseEvents which allows the Dynamic Content plugin module to be
    /// loaded as a plugin module
    /// </summary>
    public class PluginInitialisation : IPlugin, IInitialiseEvents
    {
        private readonly IThreadManagerServices _threadManagerServices;
        internal static readonly CacheManager DynamicContentCache = new CacheManager(CacheNameDynamicContent, new TimeSpan(100, 0, 0, 0), true);

        #region Constructors

        public PluginInitialisation(IThreadManagerServices threadManagerServices)
        {
            _threadManagerServices = threadManagerServices ?? throw new ArgumentNullException(nameof(threadManagerServices));
        }

        #endregion Constructors

        #region IInitialiseEvents Methods

        public void AfterConfigure(in IApplicationBuilder app)
        {

        }

        public void AfterConfigureServices(in IServiceCollection services)
        {
#if NET_CORE_3_X || NET_5_X
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    Constants.PolicyNameContentEditor,
                    policyBuilder => policyBuilder
                        .RequireClaim(Constants.ClaimNameManageContent)
                        .RequireClaim(Constants.ClaimNameStaff)
                        .RequireClaim(Constants.ClaimNameUsername)
                        .RequireClaim(Constants.ClaimNameUserId)
                        .RequireClaim(Constants.ClaimNameUserEmail));
            });
#endif

            services.TryAddSingleton<IDynamicContentProvider, DefaultDynamicContentProvider>();

            _threadManagerServices.RegisterStartupThread(nameof(DynamicContentThreadManager), typeof(DynamicContentThreadManager));
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

        #endregion IInitialiseEvents Methods

        #region IPlugin Methods

        public void ConfigureServices(IServiceCollection services)
        {

        }

        public void Finalise()
        {

        }

        public ushort GetVersion()
        {
            return 1;
        }

        public void Initialise(ILogger logger)
        {

        }

        #endregion IPlugin Methods
    }
}

#pragma warning restore CS1591