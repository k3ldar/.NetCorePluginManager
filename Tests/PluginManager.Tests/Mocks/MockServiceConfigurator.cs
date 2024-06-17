/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager
 *  
 *  File: MockServiceConfigurator.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  17/08/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;

using PluginManager.Abstractions;

namespace PluginManager.Tests.Mocks
{
	[ExcludeFromCodeCoverage]
	public class MockServiceConfigurator : IServiceConfigurator
	{
		public void RegisterServices(IServiceCollection services)
		{
			RegisterServicesCalled = true;
		}

		public bool RegisterServicesCalled { get; private set; }
	}
}
