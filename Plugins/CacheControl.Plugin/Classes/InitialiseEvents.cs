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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  CacheControl.Plugin.Plugin
 *  
 *  File: InitialiseEvents.cs
 *
 *  Purpose:  Initialisation events
 *
 *  Date        Name                Reason
 *  07/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace CacheControl.Plugin.Classes
{
    public class InitialiseEvents : IInitialiseEvents
    {
        #region IInitialiseEvents Methods

        public void AfterConfigure(in IApplicationBuilder app)
        {

        }

        public void AfterConfigureServices(in IServiceCollection services)
        {


        }

        public void BeforeConfigure(in IApplicationBuilder app)
        {
            app.UseCacheControl();
        }

        public void BeforeConfigureServices(in IServiceCollection services)
        {

        }

        public void Configure(in IApplicationBuilder app)
        {

        }

        #endregion IInitialiseEvents Methods
    }
}

#pragma warning restore CS1591