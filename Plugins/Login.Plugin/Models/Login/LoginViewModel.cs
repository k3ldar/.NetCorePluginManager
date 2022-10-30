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
 *  Product:  Login Plugin
 *  
 *  File: LoginModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  19/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.ComponentModel.DataAnnotations;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace LoginPlugin.Models
{
    public sealed class LoginViewModel : BaseModel
    {
        #region Constructors

        public LoginViewModel()
        {

        }

        public LoginViewModel(in BaseModelData modelData,
            string returnUrl, bool showRememberMe, bool showGoogle, bool showFacebook)
            : base(modelData)
        {
            ReturnUrl = returnUrl ?? throw new ArgumentNullException(nameof(returnUrl));
            ShowRememberMe = showRememberMe;
            ShowGoogle = showGoogle;
            ShowFacebook = showFacebook;
        }

        #endregion Constructors

        #region Properties

        public string ReturnUrl { get; set; }

        [Required(ErrorMessage = nameof(Languages.LanguageStrings.PleaseEnterUserNameOrEmail))]
        [Display(Name = nameof(Languages.LanguageStrings.Username))]
        public string Username { get; set; }

        [Required(ErrorMessage = nameof(Languages.LanguageStrings.PleaseEnterPassword))]
        [StringLength(Constants.MaximumPasswordLength, MinimumLength = Constants.MinimumPasswordLength)]
        [Display(Name = nameof(Languages.LanguageStrings.Password))]
        public string Password { get; set; }

        [Display(Name = nameof(Languages.LanguageStrings.Code))]
        public string CaptchaText { get; set; }

        public bool ShowCaptchaImage { get; set; }

        [Display(Name = nameof(Languages.LanguageStrings.RememberMe))]
        public bool RememberMe { get; set; }

        public bool ShowRememberMe { get; set; }

        public bool ShowGoogle { get; set; }

        public bool ShowFacebook { get; set; }

        #endregion Properties
    }
}

#pragma warning restore CS1591