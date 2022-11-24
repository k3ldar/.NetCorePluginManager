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
 *  File: UserClaimsRowDefaults.cs
 *
 *  Purpose:  Default values for user claims data row
 *
 *  Date        Name                Reason
 *  19/11/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using PluginManager.Abstractions;

using SharedPluginFeatures;

using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables.Users
{
	internal class UserClaimsRowDefaults : ITableDefaults<UserClaimsDataRow>
	{
		private readonly IPluginClassesService _pluginClassesService;

		public UserClaimsRowDefaults(IPluginClassesService pluginClassesService)
		{
			_pluginClassesService = pluginClassesService ?? throw new ArgumentNullException(nameof(pluginClassesService));
		}

		public long PrimarySequence => 1;

		public long SecondarySequence => 1;

		public ushort Version => 1;

		public List<UserClaimsDataRow> InitialData(ushort version)
		{
			if (version == 1)
			{
				ObservableList<string> claims = new();

				foreach (IClaimsService claimsService in _pluginClassesService.GetPluginClasses<IClaimsService>())
				{
					foreach (string claim in claimsService.GetClaims())
					{
						if (!claims.Contains(claim))
							claims.Add(claim);
					}
				}

				claims.Remove(SharedPluginFeatures.Constants.ClaimNameUsername);
				claims.Remove(SharedPluginFeatures.Constants.ClaimNameUserEmail);
				claims.Remove(SharedPluginFeatures.Constants.ClaimNameUserId);

				return new List<UserClaimsDataRow>()
				{
					new UserClaimsDataRow()
					{
						UserId = 1,
						Claims = claims,
					}
				};
			}

			return null;
		}
	}
}
