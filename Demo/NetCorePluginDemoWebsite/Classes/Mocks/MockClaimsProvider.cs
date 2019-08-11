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
        #region Private Members

        private readonly List<string> _claimsForUser;

        #endregion Private Members

        #region Constructors

        public MockClaimsProvider()
        {
            _claimsForUser = new List<string>();
        }

        #endregion Constructors

        #region IClaimsProvider Methods

        public AuthenticationProperties GetAuthenticationProperties()
        {
            return new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true,
            };
        }

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

        public bool SetClaimsForUser(in long id, in List<string> claims)
        {
            _claimsForUser.Clear();
            _claimsForUser.AddRange(claims);
            return true;
        }

        public List<string> GetAllClaims()
        {
            return new List<string>()
            {
                Constants.ClaimNameAdministrator,
                Constants.ClaimNameCreateBlog,
                Constants.ClaimNameManageSeo,
                Constants.ClaimNameStaff,
                Constants.ClaimNameUserPermissions


                ,"Another 1",
                "Another 2",
                "Another 3",
                "Another 4",
                "Another 5",
                "Another 6",
                "Another 7",
                "Another 8",
                "Another 9",
                "Another 10",
            };
        }

        public List<string> GetClaimsForUser(in long id)
        {
            return _claimsForUser;
        }

        #endregion IClaimsProvider Methods
    }
}
