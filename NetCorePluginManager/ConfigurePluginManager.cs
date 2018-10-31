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
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: ConfigurePluginManagerExtender.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  30/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Linq;

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
