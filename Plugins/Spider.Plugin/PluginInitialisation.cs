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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Spider.Plugin
 *  
 *  File: PluginInitialisation.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  29/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace Spider.Plugin
{
    /// <summary>
    /// Implements IPlugin which allows the Spider.Plugin module to be
    /// loaded as a plugin module
    /// </summary>
    public sealed class PluginInitialisation : IPlugin
    {
        #region Constructors

        public PluginInitialisation()
        {
        }

        #endregion Constructors

        #region Internal Static Properties

        internal static IServiceProvider GetServiceProvider { get; private set; }

        internal static ILogger GetLogger { get; private set; }

        #endregion Internal Static Properties

        #region IPlugin Methods

        public void Initialise(ILogger logger)
        {
            GetLogger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Finalise()
        {

        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSpider();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            GetServiceProvider = services.BuildServiceProvider();
        }

        #endregion IPlugin Methods
    }
}

#pragma warning restore CS1591