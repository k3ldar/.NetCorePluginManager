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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Demo Website
 *  
 *  File: MockClaimsProvider.cs
 *
 *  Purpose:  Mock IClaimsProvider for tesing purpose
 *
 *  Date        Name                Reason
 *  21/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;
using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
    public class MockClaimsProvider : IClaimsProvider
    {
        public List<ClaimsIdentity> GetUserClaims(in long userId)
        {
            List<ClaimsIdentity> Result = new List<ClaimsIdentity>();

            List<Claim> userClaims = new List<Claim>();
            userClaims.Add(new Claim(Constants.ClaimNameUsername, "Administrator"));
            userClaims.Add(new Claim(Constants.ClaimNameUserEmail, "admin@nowhere.com"));
            userClaims.Add(new Claim(Constants.ClaimNameUserId, "123"));
            Result.Add(new ClaimsIdentity(userClaims, Constants.ClaimIdentityUser));

            List<Claim> webClaims = new List<Claim>();
            webClaims.Add(new Claim(Constants.ClaimNameCreateBlog, "true"));
            webClaims.Add(new Claim(Constants.ClaimNameAdministrator, "true"));
            webClaims.Add(new Claim(Constants.ClaimNameStaff, "true"));
            webClaims.Add(new Claim(Constants.ClaimNameManageSeo, "true"));
            Result.Add(new ClaimsIdentity(webClaims, Constants.ClaimIdentityWebsite));

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
    }
}
