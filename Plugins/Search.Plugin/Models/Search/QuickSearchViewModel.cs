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
 *  Product:  Search Plugin
 *  
 *  File: QuickSearchViewModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  13/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using System.ComponentModel.DataAnnotations;

namespace SearchPlugin.Models
{
	/// <summary>
	/// Quick search view model, designed for quickly searching all registered areas
	/// </summary>
	public class QuickSearchViewModel
	{
		#region Constructors

		/// <summary>
		/// Constructor for quick searching with search text 
		/// </summary>
		public QuickSearchViewModel()
			: base()
		{
			SearchText = String.Empty;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Text to be searched for
		/// </summary>
		[Display(Name = nameof(Languages.LanguageStrings.SearchDescription))]
		[StringLength(200, MinimumLength = 3)]
		[Required(ErrorMessage = nameof(Languages.LanguageStrings.SearchInvalid))]
		public string SearchText { get; set; }

		/// <summary>
		/// Id of the current search, this will be used when verifying that the call is legit
		/// </summary>
		/// <value>string</value>
		public string SearchId { get; set; }

		#endregion Properties
	}
}
