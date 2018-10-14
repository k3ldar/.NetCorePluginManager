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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Demo Website Plugin
 *  
 *  File: Initialisation.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using SharedPluginFeatures;

namespace MemoryCache.Plugin
{
    public class Initialisation : IPlugin
    {
        #region Constructors

        public Initialisation()
        {
        }

        #endregion Constructors

        #region IPlugin Methods

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "DemoApi",
                    template: "{controller=DemoApi}/{action=Index}/{id?}");
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            
        }

        public void Finalise()
        {
            
        }

        public void Initialise(ILogger logger)
        {
            
        }

        #endregion IPlugin Methods
    }
}
