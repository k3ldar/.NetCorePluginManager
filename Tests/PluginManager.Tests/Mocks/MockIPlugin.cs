﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  Plugin Manager is distributed under the GNU General Public License version 3 and  
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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.Tests
 *  
 *  File: MockIPlugin.cs
 *
 *  Purpose:  Mock IPlugin for testing
 *
 *  Date        Name                Reason
 *  19/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;

using PluginManager.Abstractions;

namespace PluginManager.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class MockIPlugin : IPlugin
    {
        public void ConfigureServices(IServiceCollection services)
        {
            throw new NotImplementedException();
        }

        public void Finalise()
        {
            throw new NotImplementedException();
        }

        public ushort GetVersion()
        {
            throw new NotImplementedException();
        }

        public void Initialise(ILogger logger)
        {
            throw new NotImplementedException();
        }
    }
}