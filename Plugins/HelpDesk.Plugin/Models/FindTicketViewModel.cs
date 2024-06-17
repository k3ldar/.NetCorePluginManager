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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Helpdesk Plugin
 *  
 *  File: FindTicketViewModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  23/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.ComponentModel.DataAnnotations;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace HelpdeskPlugin.Models
{
	public sealed class FindTicketViewModel : BaseModel
	{
		#region Constructors

		public FindTicketViewModel()
		{

		}

		public FindTicketViewModel(in BaseModelData modelData,
			in bool showCaptchaText)
			: base(modelData)
		{
			ShowCaptchaText = showCaptchaText;
		}

		#endregion Constructors

		#region Properties

		[Required(ErrorMessage = nameof(Languages.LanguageStrings.SupportTicketKeyRequired))]
		public string Key { get; set; }

		[Required(ErrorMessage = nameof(Languages.LanguageStrings.InvalidEmailAddress))]
		public string Email { get; set; }

		[Display(Name = nameof(Languages.LanguageStrings.Code))]
		public string CaptchaText { get; set; }

		public bool ShowCaptchaText { get; set; }

		#endregion Properties
	}
}

#pragma warning restore CS1591