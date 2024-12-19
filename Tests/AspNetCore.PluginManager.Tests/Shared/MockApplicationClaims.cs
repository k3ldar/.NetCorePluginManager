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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockBotTrap.cs
 *
 *  Purpose:  Mock IApplicationClaimProvider class
 *
 *  Date        Name                Reason
 *  29/11/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Security.Claims;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Shared
{
	public class MockApplicationClaims : IApplicationClaimProvider
	{

		public MockApplicationClaims(List<Claim> additionalClaims)
		{
			AdditionalClaims = additionalClaims ?? throw new ArgumentNullException(nameof(additionalClaims)); ;
		}

		public List<Claim> AdditionalUserClaims(in long userId)
		{
			return AdditionalClaims;
		}
		public List<Claim> AdditionalClaims { get; }
	}
}
