﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Login Plugin
 *  
 *  File: AccountLockedViewModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  21/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.ComponentModel.DataAnnotations;

namespace LoginPlugin.Models
{
    public sealed class AccountLockedViewModel
    {
        #region Constructors

        public AccountLockedViewModel()
        {
            Username = String.Empty;
        }

        public AccountLockedViewModel(string username)
        {
            if (String.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            Username = username;
        }

        #endregion Constructors

        #region Properties

        [Required]
        [Display(Name = "User name or email")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter the unlock code")]
        [Display(Name = "Validation Code")]
        public string UnlockCode { get; set; }

        #endregion Properties
    }
}
