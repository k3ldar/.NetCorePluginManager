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
using SimpleDB;

using SharedPluginFeatures;

namespace PluginManager.DAL.TextFiles.Providers
{
	internal class ClaimsProvider : IClaimsProvider
	{
		#region Private Members

		private readonly IPluginClassesService _pluginClassesService;
		private readonly ISimpleDBOperations<UserDataRow> _users;
		private readonly ISimpleDBOperations<ExternalUsersDataRow> _externalUsers;
		private readonly ISimpleDBOperations<UserClaimsDataRow> _userClaims;
		private readonly IApplicationClaimProvider _additionalClaimsProvider;

		#endregion Private Members

		#region Constructors

		public ClaimsProvider(IPluginClassesService pluginClassesService,
			ISimpleDBOperations<UserDataRow> users,
			ISimpleDBOperations<ExternalUsersDataRow> externalUsers,
			ISimpleDBOperations<UserClaimsDataRow> userClaims)
		{
			_pluginClassesService = pluginClassesService ?? throw new ArgumentNullException(nameof(pluginClassesService));
			_users = users ?? throw new ArgumentNullException(nameof(users));
			_externalUsers = externalUsers ?? throw new ArgumentNullException(nameof(externalUsers));
			_userClaims = userClaims ?? throw new ArgumentNullException(nameof(userClaims));
			_additionalClaimsProvider = _pluginClassesService.GetPluginClasses<IApplicationClaimProvider>().FirstOrDefault();
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
			List<ClaimsIdentity> Result = [];

			UserDataRow user = _users.Select(userId);
			List<Claim> userClaims = [];


			if (user == null)
			{
				ExternalUsersDataRow externalUser = _externalUsers.Select(userId);

				if (externalUser == null)
					return Result;

				userClaims.Add(new Claim(SharedPluginFeatures.Constants.ClaimNameUsername, externalUser.UserName));
				userClaims.Add(new Claim(SharedPluginFeatures.Constants.ClaimNameUserEmail, externalUser.Email));
				userClaims.Add(new Claim(SharedPluginFeatures.Constants.ClaimNameUserId, externalUser.Id.ToString()));
				Result.Add(new ClaimsIdentity(userClaims, SharedPluginFeatures.Constants.ClaimIdentityUser));
			}
			else
			{
				userClaims.Add(new Claim(SharedPluginFeatures.Constants.ClaimNameUsername, user.FullName));
				userClaims.Add(new Claim(SharedPluginFeatures.Constants.ClaimNameUserEmail, user.Email));
				userClaims.Add(new Claim(SharedPluginFeatures.Constants.ClaimNameUserId, user.Id.ToString()));
				Result.Add(new ClaimsIdentity(userClaims, SharedPluginFeatures.Constants.ClaimIdentityUser));

				GetUserClaims(user, Result);
			}


			List<Claim> additionalClaims = _additionalClaimsProvider?.AdditionalUserClaims(userId);

			if (additionalClaims != null && additionalClaims.Count > 0)
				Result.Add(new ClaimsIdentity(additionalClaims, SharedPluginFeatures.Constants.ClaimIdentityApplication));

			return Result;
		}

		private void GetUserClaims(UserDataRow user, List<ClaimsIdentity> Result)
		{
			List<Claim> webClaims = [];

			UserClaimsDataRow claims = _userClaims.Select().FirstOrDefault(uc => uc.UserId.Equals(user.Id));

			if (claims == null)
				return;

			claims.Claims.ForEach(c => webClaims.Add(new Claim(c, true.ToString().ToLower())));

			Result.Add(new ClaimsIdentity(webClaims, SharedPluginFeatures.Constants.ClaimIdentityWebsite));
		}

		public bool SetClaimsForUser(in long id, in List<string> claims)
		{
			UserDataRow user = _users.Select(id);

			if (user == null)
			{
				return false;
			}

			UserClaimsDataRow userClaims = _userClaims.Select().FirstOrDefault(uc => uc.UserId.Equals(user.Id));

			if (userClaims == null)
			{
				userClaims = new UserClaimsDataRow();
				userClaims.Claims.AddRange(claims);
				userClaims.UserId = id;
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
			List<string> Result = [];

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
			List<string> Result = [];

			UserDataRow user = _users.Select(id);

			if (user == null)
				return Result;


			UserClaimsDataRow claims = _userClaims.Select().FirstOrDefault(uc => uc.UserId.Equals(user.Id));

			if (claims == null)
				return Result;

			claims.Claims.ForEach(c => Result.Add(c));

			return Result;
		}

		#endregion IClaimsProvider Methods
	}
}
