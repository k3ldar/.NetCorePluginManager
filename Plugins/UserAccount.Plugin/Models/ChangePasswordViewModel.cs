using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;


namespace UserAccount.Plugin.Models
{
    public sealed class ChangePasswordViewModel
    {
        #region Constructors

        public ChangePasswordViewModel()
        {
            Username = String.Empty;
        }

        #endregion Constructors

        #region Properties

        [Required]
        [Display(Name = "User name or email")]
        public string Username { get; set; }

        [Required]
        [StringLength(Constants.MaximumPasswordLength, MinimumLength = Constants.MinimumPasswordLength)]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(Constants.MaximumPasswordLength, MinimumLength = Constants.MinimumPasswordLength)]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(Constants.MaximumPasswordLength, MinimumLength = Constants.MinimumPasswordLength)]
        public string ConfirmNewPassword { get; set; }

        #endregion Properties
    }
}
