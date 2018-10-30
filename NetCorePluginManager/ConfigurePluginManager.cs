using System.Linq;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;


namespace AspNetCore.PluginManager
{
    public static class ConfigurePluginManagerExtender
    {
        public static IMvcBuilder ConfigurePluginManager(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.ConfigureApplicationPartManager(manager =>
             {
                 var oldMetadataReferenceFeatureProvider = manager.FeatureProviders.First(f => f is MetadataReferenceFeatureProvider);
                 manager.FeatureProviders.Remove(oldMetadataReferenceFeatureProvider);
                 manager.FeatureProviders.Add(new Classes.PluginFeatureProvider());
             });

             return (mvcBuilder);
        }
    }
}
