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
 *  Product:  SystemAdmin.Plugin
 *  
 *  File: UserPermissionsViewModel.cs
 *
 *  Purpose:  Model for user claims
 *
 *  Date        Name                Reason
 *  07/08/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

#pragma warning disable CS1591

namespace SystemAdmin.Plugin.Models
{
    public class UserPermissionsViewModel
    {
        #region Constructors

        public UserPermissionsViewModel()
        {
            SelectedClaims = String.Empty;
        }

        public UserPermissionsViewModel(in long userId, in List<string> userClaims, in List<string> systemClaims)
        {
            UserId = userId;
            UserClaims = userClaims ?? throw new ArgumentNullException(nameof(userClaims));
            SystemClaims = systemClaims ?? throw new ArgumentNullException(nameof(systemClaims));
            SelectedClaims = String.Join(';', userClaims);
        }

        #endregion Constructors

        #region Properties

        public long UserId { get; set; }

        public List<string> UserClaims { get; set; }

        public List<string> SystemClaims { get; set; }

        public string SelectedClaims { get; set; }

        #endregion Properties
    }
}

#pragma warning restore CS1591