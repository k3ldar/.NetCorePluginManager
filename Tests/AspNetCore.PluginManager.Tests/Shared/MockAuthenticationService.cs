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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockAuthenticationService.cs
 *
 *  Purpose:  Mock IAuthenticationService class
 *
 *  Date        Name                Reason
 *  20/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class MockAuthenticationService : IAuthenticationService
    {
        #region IAuthenticationService Methods

        public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, String scheme)
        {
            throw new NotImplementedException();
        }

        public Task ChallengeAsync(HttpContext context, String scheme, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task ForbidAsync(HttpContext context, String scheme, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task SignInAsync(HttpContext context, String scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
        {
            SignInAsyncCalled = true;
            return Task.Delay(0);
        }

        public Task SignOutAsync(HttpContext context, String scheme, AuthenticationProperties properties)
        {
            SignOutAsyncCalled = true;
            return Task.Delay(0);
        }

        #endregion IAuthenticationService Methods

        #region Properties

        public bool SignInAsyncCalled { get; private set; }

        public bool SignOutAsyncCalled { get; private set; }

        #endregion Properties
    }
}
