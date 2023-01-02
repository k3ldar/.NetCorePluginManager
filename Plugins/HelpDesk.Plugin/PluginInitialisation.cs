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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Helpdesk Plugin
 *  
 *  File: PluginInitialisation.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  15/12/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using HelpdeskPlugin.Classes;

using Microsoft.Extensions.DependencyInjection;

using PluginManager.Abstractions;

using Shared.Communication;

using SharedPluginFeatures;

#pragma warning disable IDE0060, CS1591

namespace HelpdeskPlugin
{
	/// <summary>
	/// Implements IPlugin and IPluginVersion which allows the HelpdeskPlugin module to be
	/// loaded as a plugin module
	/// </summary>
	public class PluginInitialisation : IPlugin
	{
		public PluginInitialisation(IThreadManagerServices threadManagerServices)
		{
			if (threadManagerServices == null)
				throw new ArgumentNullException(nameof(threadManagerServices));

#if NET6_0_OR_GREATER
			threadManagerServices.RegisterStartupThread(Constants.ImportEmailIntoHelpdeskThread, typeof(ImportEmailIntoHelpdeskThread));
#endif
		}

		public void ConfigureServices(IServiceCollection services)
		{
#if NET6_0_OR_GREATER
			services.AddTransient(typeof(ImportEmailIntoHelpdeskThread));
			services.AddTransient<IPop3ClientFactory, Pop3ClientFactory>();
#endif
		}

		public void Finalise()
		{
			// from interface but unused in this context
		}

		public ushort GetVersion()
		{
			return 1;
		}

		public void Initialise(ILogger logger)
		{
			// from interface but unused in this context
		}
	}
}

#pragma warning restore IDE0060, CS1591