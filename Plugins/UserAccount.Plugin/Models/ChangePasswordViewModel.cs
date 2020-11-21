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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  UserAccount.Plugin
 *  
 *  File: ChangePasswordViewModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  08/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.ComponentModel.DataAnnotations;

using SharedPluginFeatures;

namespace UserAccount.Plugin.Models
{
#pragma warning disable CS1591

    public class ChangePasswordViewModel : BaseModel
    {
        #region Constructors

        public ChangePasswordViewModel()
        {

        }

        public ChangePasswordViewModel(in BaseModelData baseModelData)
            : base(baseModelData)
        {

        }

        public ChangePasswordViewModel(in BaseModelData baseModelData, in string username)
            : this(baseModelData)
        {
            if (String.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            Username = username;
        }

        #endregion Constructors

        #region Properties

        [Required]
        [Display(Name = nameof(Languages.LanguageStrings.Username))]
        [StringLength(100, MinimumLength = 5)]
        public string Username { get; set; }

        [Required]
        [Display(Name = nameof(Languages.LanguageStrings.CurrentPassword))]
        [StringLength(Constants.MaximumPasswordLength, MinimumLength = Constants.MinimumPasswordLength)]
        public string CurrentPassword { get; set; }

        [Required]
        [Display(Name = nameof(Languages.LanguageStrings.NewPassword))]
        [StringLength(Constants.MaximumPasswordLength, MinimumLength = Constants.MinimumPasswordLength)]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = nameof(Languages.LanguageStrings.ConfirmNewPassword))]
        [StringLength(Constants.MaximumPasswordLength, MinimumLength = Constants.MinimumPasswordLength)]
        public string ConfirmNewPassword { get; set; }

        #endregion Properties
    }

#pragma warning restore CS1591
}
