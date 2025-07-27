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
 *  Product:  UserAccount.Plugin
 *  
 *  File: UserContactDetailsViewModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  12/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.ComponentModel.DataAnnotations;

using SharedPluginFeatures;

namespace UserAccount.Plugin.Models
{
#pragma warning disable CS1591

	public class UserContactDetailsViewModel : BaseModel
	{
		#region Constructors

		public UserContactDetailsViewModel()
		{

		}

		public UserContactDetailsViewModel(in IBaseModelData baseModelData,
			in string firstName, in string lastName, in string email,
			in bool emailConfirmed, in string telephone, in bool telephoneConfirmed, in bool showTelephone)
			: base(baseModelData)
		{
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			EmailConfirmed = emailConfirmed;
			Telephone = telephone;
			TelephoneConfirmed = telephoneConfirmed;
			ShowTelephone = showTelephone;
		}

		#endregion Constructors

		#region Properties

		[Required]
		[Display(Name = nameof(Languages.LanguageStrings.FirstName))]
		[StringLength(50, MinimumLength = 2)]
		public string FirstName { get; set; }

		[Required]
		[Display(Name = nameof(Languages.LanguageStrings.LastName))]
		[StringLength(50, MinimumLength = 2)]
		public string LastName { get; set; }

		[Required]
		[Display(Name = nameof(Languages.LanguageStrings.Email))]
		[StringLength(100, MinimumLength = 5)]
		public string Email { get; set; }

		public bool EmailConfirmed { get; set; }

		public bool ShowTelephone { get; set; }

		[Display(Name = nameof(Languages.LanguageStrings.Telephone))]
		[StringLength(20, MinimumLength = 0)]
		public string Telephone { get; set; }

		public bool TelephoneConfirmed { get; set; }

		#endregion Properties
	}

#pragma warning restore CS1591
}
