/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Product:  Demo Website
 *  
 *  File: MockPluginConfiguration.cs
 *
 *  Purpose:  Mock IConfigureApplicationBuilder and IConfigureMvcBuilder for tesing purpose
 *
 *  Date        Name                Reason
 *  02/08/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
	[ExcludeFromCodeCoverage(Justification = "Code coverage not required for mock classes")]
	public class MockPluginConfiguration : IConfigureApplicationBuilder, IConfigureMvcBuilder
	{
		#region IConfigureApplicationBuilder Methods

		public void ConfigureApplicationBuilder(in IApplicationBuilder applicationBuilder)
		{
			// required by interface not used in mock
		}

		#endregion IConfigureApplicationBuilder Methods

		#region IConfigureMvcBuilder Methods

		public void ConfigureMvcBuilder(in IMvcBuilder mvcBuilder)
		{
			// required by interface not used in mock
		}

		#endregion IConfigureMvcBuilder Methods
	}
}
