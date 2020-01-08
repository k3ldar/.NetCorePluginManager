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
 *  File: CreateAccountViewModel.cs
 *
 *  Purpose:  Create account view model
 *
 *  Date        Name                Reason
 *  13/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.ComponentModel.DataAnnotations;

using SharedPluginFeatures;

namespace UserAccount.Plugin.Models
{
#pragma warning disable CS1591

    public class CreateAccountViewModel : BaseModel
    {
        #region Constructors

        public CreateAccountViewModel()
        {

        }

        public CreateAccountViewModel(in BaseModelData baseModelData, string returnUrl)
            : base(baseModelData)
        {
            ReturnUrl = returnUrl;
        }

        #endregion Constructors

        #region Properties

        [Required]
        [Display(Name = nameof(Languages.LanguageStrings.Email))]
        [StringLength(100, MinimumLength = 5)]
        public string Email { get; set; }

        [Required]
        [Display(Name = nameof(Languages.LanguageStrings.FirstName))]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = nameof(Languages.LanguageStrings.LastName))]
        [StringLength(50, MinimumLength = 2)]
        public string Surname { get; set; }

        [Required]
        [Display(Name = nameof(Languages.LanguageStrings.Password))]
        [StringLength(Constants.MaximumPasswordLength, MinimumLength = Constants.MinimumPasswordLength)]
        public string Password { get; set; }

        [Required]
        [Display(Name = nameof(Languages.LanguageStrings.ConfirmPassword))]
        [StringLength(Constants.MaximumPasswordLength, MinimumLength = Constants.MinimumPasswordLength)]
        public string ConfirmPassword { get; set; }

        public bool ShowBusinessName { get; set; }

        [Display(Name = nameof(Languages.LanguageStrings.BusinessName))]
        [StringLength(20, MinimumLength = 0)]
        public string BusinessName { get; set; }

        public bool ShowTelephone { get; set; }

        [Display(Name = nameof(Languages.LanguageStrings.Telephone))]
        [StringLength(20, MinimumLength = 0)]
        public string Telephone { get; set; }

        public bool ShowAddressLine1 { get; set; }

        [Display(Name = nameof(Languages.LanguageStrings.AddressLine1))]
        [StringLength(20, MinimumLength = 0)]
        public string AddressLine1 { get; set; }

        public bool ShowAddressLine2 { get; set; }

        [Display(Name = nameof(Languages.LanguageStrings.AddressLine2))]
        [StringLength(20, MinimumLength = 0)]
        public string AddressLine2 { get; set; }

        public bool ShowAddressLine3 { get; set; }

        [Display(Name = nameof(Languages.LanguageStrings.AddressLine3))]
        [StringLength(20, MinimumLength = 0)]
        public string AddressLine3 { get; set; }

        public bool ShowCity { get; set; }

        [Display(Name = nameof(Languages.LanguageStrings.City))]
        [StringLength(20, MinimumLength = 0)]
        public string City { get; set; }

        public bool ShowCounty { get; set; }

        [Display(Name = nameof(Languages.LanguageStrings.County))]
        [StringLength(20, MinimumLength = 0)]
        public string County { get; set; }

        public bool ShowPostcode { get; set; }

        [Display(Name = nameof(Languages.LanguageStrings.Postcode))]
        [StringLength(20, MinimumLength = 0)]
        public string Postcode { get; set; }

        [Display(Name = nameof(Languages.LanguageStrings.Country))]
        public string Country { get; set; }

        [Display(Name = nameof(Languages.LanguageStrings.Code))]
        public string CaptchaText { get; set; }

        public bool ShowCaptchaImage { get; set; }

        public string ReturnUrl { get; set; }

        #endregion Properties
    }

#pragma warning restore CS1591
}
