using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SharedPluginFeatures;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
    public class MockPluginConfiguration : IConfigureApplicationBuilder, IConfigureMvcBuilder
    {
        #region IConfigureApplicationBuilder Methods

        public void ConfigureApplicationBuilder(in IApplicationBuilder applicationBuilder)
        {
            
        }

        #endregion IConfigureApplicationBuilder Methods

        #region IConfigureMvcBuilder Methods

        public void ConfigureMvcBuilder(in IMvcBuilder mvcBuilder)
        {
            
        }

        #endregion IConfigureMvcBuilder Methods
    }
}
