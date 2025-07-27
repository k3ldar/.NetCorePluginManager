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
 *  Product:  Helpdesk Plugin
 *  
 *  File: SubmitTicketViewModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  21/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Middleware;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace HelpdeskPlugin.Models
{
	public sealed class SubmitTicketViewModel : BaseModel
	{
		#region Constructors

		public SubmitTicketViewModel()
		{

		}

		public SubmitTicketViewModel(in IBaseModelData modelData,
			in List<LookupListItem> departments, in List<LookupListItem> priorities,
			in string username, in string email, in string subject, in string message,
			in bool readonlyUser)
			: base(modelData)
		{
			Departments = departments ?? throw new ArgumentNullException(nameof(departments));
			Priorities = priorities ?? throw new ArgumentNullException(nameof(priorities));
			Username = username;
			Email = email;
			Subject = subject ?? String.Empty;
			Message = message ?? String.Empty;
			ReadonlyUser = readonlyUser;
		}

		#endregion Constructors

		#region Properties

		public List<LookupListItem> Departments { get; private set; }

		public List<LookupListItem> Priorities { get; private set; }

		public int Department { get; set; }

		public int Priority { get; set; }

		[Required(ErrorMessage = nameof(Languages.LanguageStrings.PleaseEnterFirstLastName))]
		public string Username { get; set; }

		[Required(ErrorMessage = nameof(Languages.LanguageStrings.InvalidEmailAddress))]
		public string Email { get; set; }

		[Required(ErrorMessage = nameof(Languages.LanguageStrings.SubjectInvalid))]
		public string Subject { get; set; }

		[Required(ErrorMessage = nameof(Languages.LanguageStrings.SupportTicketMessageRequired))]
		public string Message { get; set; }

		[Required(ErrorMessage = nameof(Languages.LanguageStrings.CodePleaseEnter))]
		public string CaptchaText { get; set; }

		public bool ReadonlyUser { get; set; }

		#endregion Properties
	}
}

#pragma warning restore CS1591