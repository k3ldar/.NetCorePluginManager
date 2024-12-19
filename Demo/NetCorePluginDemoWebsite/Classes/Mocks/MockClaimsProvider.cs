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
 *  File: MockClaimsProvider.cs
 *
 *  Purpose:  Mock IClaimsProvider for tesing purpose
 *
 *  Date        Name                Reason
 *  21/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;

using PluginManager.Abstractions;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
	[ExcludeFromCodeCoverage(Justification = "Code coverage not required for mock classes")]
	public class MockClaimsProvider : IClaimsProvider
	{
		#region Private Members

		private readonly IPluginClassesService _pluginClassesService;
		private readonly List<string> _claimsForUser;

		#endregion Private Members

		#region Constructors

		public MockClaimsProvider(IPluginClassesService pluginClassesService)
		{
			_pluginClassesService = pluginClassesService ?? throw new ArgumentNullException(nameof(pluginClassesService));
			_claimsForUser = [];
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

			List<Claim> userClaims =
			[
				new Claim("sub", userId.ToString()),
				new Claim(Constants.ClaimNameUsername, "Administrator"),
				new Claim(Constants.ClaimNameUserEmail, "admin@nowhere.com"),
				new Claim(Constants.ClaimNameUserId, userId.ToString()),
			];
			Result.Add(new ClaimsIdentity(userClaims, Constants.ClaimIdentityUser));

			List<Claim> webClaims =
			[
				new Claim(Constants.ClaimNameCreateBlog, "true"),
				new Claim(Constants.ClaimNameAdministrator, "true"),
				new Claim(Constants.ClaimNameStaff, "true"),
				new Claim(Constants.ClaimNameManageSeo, "true"),
				new Claim(Constants.ClaimNameViewImageManager, "true"),
				new Claim(Constants.ClaimNameManageContent, "true"),
				new Claim(Constants.ClaimNameAddResources, "true"),
			];

			// Only enable the following if the file exists to prevent malicious use
			// when deployed live as a demo site
			if (File.Exists("t:\\testimages.tst"))
			{
				webClaims.Add(new Claim(Constants.ClaimNameManageSystemSettings, "true"));
				webClaims.Add(new Claim(Constants.ClaimNameUserPermissions, "true"));
				webClaims.Add(new Claim(Constants.ClaimNameManageImages, "true"));
				webClaims.Add(new Claim(Constants.ClaimNameManageProducts, "true"));
				webClaims.Add(new Claim(Constants.ClaimNameManageResources, "true"));
			}

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
			return _claimsForUser;
		}

		#endregion IClaimsProvider Methods
	}
}
