﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Error Manager Plugin
 *  
 *  File: PluginInitialisation.cs
 *
 *  Purpose:  Net Core Plugin Manager Integration
 *
 *  Date        Name                Reason
 *  16/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace ErrorManager.Plugin
{
	/// <summary>
	/// Implements IPlugin and IPluginVersion which allows the ErrorManager.Plugin module to be
	/// loaded as a plugin module
	/// </summary>
	public class PluginInitialisation : IPlugin, IInitialiseEvents
	{
		public void ConfigureServices(IServiceCollection services)
		{
			// from interface but unused in this context
		}

		public void Finalise()
		{
			if (ThreadManager.IsInitialized)
				ThreadManager.Finalise();
		}

		public ushort GetVersion()
		{
			return 1;
		}

		public void Initialise(ILogger logger)
		{
			ThreadManager.Initialise();
		}

		#region IInitialiseEvents Methods

		public void AfterConfigure(in IApplicationBuilder app)
		{
			// from interface but unused in this context
		}

		public void AfterConfigureServices(in IServiceCollection services)
		{
			if (services == null)
				throw new ArgumentNullException(nameof(services));

		}

		public void BeforeConfigure(in IApplicationBuilder app)
		{
			app.UseErrorManager();
		}

		public void BeforeConfigureServices(in IServiceCollection services)
		{
			// from interface but unused in this context
		}

		public void Configure(in IApplicationBuilder app)
		{
			// from interface but unused in this context
		}

		#endregion IInitialiseEvents Methods
	}
}

#pragma warning restore CS1591