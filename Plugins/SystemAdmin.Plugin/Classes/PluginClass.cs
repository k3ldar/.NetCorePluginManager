using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SharedPluginFeatures;

namespace SystemAdmin.Plugin.Classes
{
    public class PluginClass : IPlugin, IPluginVersion
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISystemAdminHelperService, SystemAdminHelper>();
        }

        public void Finalise()
        {
            
        }

        public ushort GetVersion()
        {
            return (1);
        }

        public void Initialise(ILogger logger)
        {
            
        }
    }
}
