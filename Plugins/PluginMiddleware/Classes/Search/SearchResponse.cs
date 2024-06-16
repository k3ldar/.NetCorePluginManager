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
 *  Product:  Plugin Middleware
 *  
 *  File: SearchResponse.cs
 *
 *  Purpose:  Base search response
 *
 *  Date        Name                Reason
 *  02/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

namespace Middleware.Search
{
	/// <summary>
	/// Base search response
	/// </summary>
	public class SearchResponse : BaseSearchOptions
	{
		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="isLoggedIn">Indicates that the search is being completed from a user who is logged in.</param>
		/// <param name="searchTerm">The search term being sought.</param>
		public SearchResponse(in bool isLoggedIn, in string searchTerm)
			: base(isLoggedIn, searchTerm)
		{
			SearchResults = new List<SearchResponseItem>();
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// List of search results
		/// </summary>
		public List<SearchResponseItem> SearchResults { get; private set; }

		#endregion Properties
	}
}
