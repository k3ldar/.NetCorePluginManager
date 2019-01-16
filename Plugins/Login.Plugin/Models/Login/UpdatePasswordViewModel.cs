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
 *  Product:  Login Plugin
 *  
 *  File: UpdatePasswordViewModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  21/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.ComponentModel.DataAnnotations;

using SharedPluginFeatures;

namespace LoginPlugin.Models
{
    public sealed class UpdatePasswordViewModel
    {
        #region Constructors

        public UpdatePasswordViewModel()
        {
            Username = String.Empty;
        }

        #endregion Constructors

        #region Properties

        [Required(ErrorMessage = nameof(Languages.LanguageStrings.PleaseEnterUserNameOrEmail))]
        [Display(Name = nameof(Languages.LanguageStrings.Username))]
        public string Username { get; set; }

        [Required(ErrorMessage = nameof(Languages.LanguageStrings.PleaseEnterPassword))]
        [StringLength(Constants.MaximumPasswordLength, MinimumLength = Constants.MinimumPasswordLength)]
        [Display(Name = nameof(Languages.LanguageStrings.CurrentPassword))]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = nameof(Languages.LanguageStrings.PleaseEnterPassword))]
        [StringLength(Constants.MaximumPasswordLength, MinimumLength = Constants.MinimumPasswordLength)]
        [Display(Name = nameof(Languages.LanguageStrings.NewPassword))]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = nameof(Languages.LanguageStrings.PasswordDoesNotMatch))]
        [StringLength(Constants.MaximumPasswordLength, MinimumLength = Constants.MinimumPasswordLength)]
        [Display(Name = nameof(Languages.LanguageStrings.ConfirmPassword))]
        public string ConfirmNewPassword { get; set; }

        #endregion Properties
    }
}
