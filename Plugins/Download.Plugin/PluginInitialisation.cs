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
 *  Copyright (c) 2019 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Download Plugin
 *  
 *  File: PluginInitialisation.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  11/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.Extensions.DependencyInjection;

using PluginManager.Abstractions;

#pragma warning disable CS1591

namespace DownloadPlugin
{
    /// <summary>
    /// Implements IPlugin and IPluginVersion which allows the DownloadPlugin module to be
    /// loaded as a plugin module
    /// </summary>
    public class PluginInitialisation : IPlugin
    {
        public void ConfigureServices(IServiceCollection services)
        {

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

#pragma warning restore CS1591