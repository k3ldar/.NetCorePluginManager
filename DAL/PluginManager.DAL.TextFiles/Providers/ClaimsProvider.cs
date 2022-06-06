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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: ClaimsProvider.cs
 *
 *  Purpose:  IClaimsProvider for text based storage
 *
 *  Date        Name                Reason
 *  25/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Tables;

using SharedPluginFeatures;

namespace PluginManager.DAL.TextFiles.Providers
{
    internal class ClaimsProvider : IClaimsProvider
    {
        #region Private Members

        private readonly IPluginClassesService _pluginClassesService;
        private readonly ITextTableOperations<TableUser> _users;
        private readonly ITextTableOperations<TableUserClaims> _userClaims;

        #endregion Private Members

        #region Constructors

        public ClaimsProvider(IPluginClassesService pluginClassesService,
            ITextTableOperations<TableUser> users,
            ITextTableOperations<TableUserClaims> userClaims)
        {
            _pluginClassesService = pluginClassesService ?? throw new ArgumentNullException(nameof(pluginClassesService));
            _users = users ?? throw new ArgumentNullException(nameof(users));
            _userClaims = userClaims ?? throw new ArgumentNullException(nameof(userClaims));
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

            TableUser user = _users.Select(userId);

            if (user == null)
                return Result;

            List<Claim> userClaims = new List<Claim>();
            userClaims.Add(new Claim(Constants.ClaimNameUsername, user.FullName));
            userClaims.Add(new Claim(Constants.ClaimNameUserEmail, user.Email));
            userClaims.Add(new Claim(Constants.ClaimNameUserId, userId.ToString()));
            Result.Add(new ClaimsIdentity(userClaims, Constants.ClaimIdentityUser));

            List<Claim> webClaims = new List<Claim>();

            TableUserClaims claims = _userClaims.Select().Where(uc => uc.UserId.Equals(user.Id)).FirstOrDefault();

            if (claims == null)
                return Result;

            claims.Claims.ForEach(c => webClaims.Add(new Claim(c, true.ToString())));

            Result.Add(new ClaimsIdentity(webClaims, Constants.ClaimIdentityWebsite));

            return Result;
        }

        public bool SetClaimsForUser(in long id, in List<string> claims)
        {
            TableUser user = _users.Select(id);

            if (user == null)
            {
                return false;
            }

            TableUserClaims userClaims = _userClaims.Select().Where(uc => uc.UserId.Equals(user.Id)).FirstOrDefault();

            if (userClaims == null)
            {
                userClaims = new TableUserClaims();
                userClaims.Claims.AddRange(claims);
                _userClaims.Insert(userClaims);
            }
            else
            {
                userClaims.Claims.Clear();
                userClaims.Claims.AddRange(claims);
                _userClaims.Update(userClaims);
            }

            return true;
        }

        public List<string> GetAllClaims()
        {
            List<string> Result = new List<string>();

            foreach (IClaimsService claimsService in _pluginClassesService.GetPluginClasses<IClaimsService>())
            {
                foreach (string claim in claimsService.GetClaims())
                {
                    if (!Result.Contains(claim))
                        Result.Add(claim);
                }
            }

            return Result;
        }

        public List<string> GetClaimsForUser(in long id)
        {
            List<string> Result = new List<string>();

            TableUser user = _users.Select(id);

            if (user == null)
                return Result;


            TableUserClaims claims = _userClaims.Select().Where(uc => uc.UserId.Equals(user.Id)).FirstOrDefault();

            if (claims == null)
                return Result;

            claims.Claims.ForEach(c => Result.Add(c));

            return Result;
        }

        #endregion IClaimsProvider Methods
    }
}
