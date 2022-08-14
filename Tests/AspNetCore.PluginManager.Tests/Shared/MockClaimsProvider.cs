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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockClaimsProvider.cs
 *
 *  Purpose:  Mock IClaimsProvider class
 *
 *  Date        Name                Reason
 *  21/08/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;

using PluginManager.Abstractions;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class MockClaimsProvider : IClaimsProvider
    {
        private readonly Dictionary<long, List<string>> _userClaims;

        public MockClaimsProvider()
        {
            _userClaims = new Dictionary<long, List<string>>();
        }

        public MockClaimsProvider(Dictionary<long, List<string>> userClaims)
        {
            _userClaims = userClaims ?? throw new ArgumentNullException(nameof(userClaims));
        }

        public List<string> GetAllClaims()
        {
            List<string> Result = new List<string>();

            foreach (var item in _userClaims)
            {
                foreach (string claim in item.Value)
                {
                    if (!Result.Contains(claim))
                        Result.Add(claim);
                }
            }

            return Result;
        }

        public AuthenticationProperties GetAuthenticationProperties()
        {
            return new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true,
            };
        }

        public List<string> GetClaimsForUser(in long id)
        {
            if (_userClaims.ContainsKey(id))
                return _userClaims[id];

            return new List<string>();
        }

        public List<ClaimsIdentity> GetUserClaims(in long userId)
        {
            List<ClaimsIdentity> Result = new List<ClaimsIdentity>();

            if (userId == 123)
            {

                List<Claim> userClaims = new List<Claim>();
                userClaims.Add(new Claim("sub", "123"));
                userClaims.Add(new Claim(Constants.ClaimNameUsername, "Administrator"));
                userClaims.Add(new Claim(Constants.ClaimNameUserEmail, "admin@nowhere.com"));
                userClaims.Add(new Claim(Constants.ClaimNameUserId, "123"));
                Result.Add(new ClaimsIdentity(userClaims, Constants.ClaimIdentityUser));
            }

            return Result;
        }

        public bool SetClaimsForUser(in long id, in List<string> claims)
        {
            _userClaims[id] = claims;
            return true;
        }
    }
}
