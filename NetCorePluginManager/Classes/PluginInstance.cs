using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Classes
{
    public sealed class PluginInstance : IPlugin
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
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
    }
}
