using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AspNetCore.PluginManager.DemoWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Initialise the plugin manager service
            AspNetCore.PluginManager.PluginManagerService.Initialise(new Classes.Logger());

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
