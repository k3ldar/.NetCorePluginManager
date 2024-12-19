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
 *  File: MockTokenUserDetails.cs
 *
 *  Purpose:  Mock ITokenUserDetails class
 *
 *  Date        Name                Reason
 *  16/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using Middleware;

namespace AspNetCore.PluginManager.Tests.Shared
{
	[ExcludeFromCodeCoverage]
	public class MockTokenUserDetails : ITokenUserDetails
    {
        public MockTokenUserDetails(string email, string provider)
        {
            Email = email;
            Provider = provider;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string GivenName { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public string Provider { get; set; }
        public bool Verify { get; set; }
    }
}
