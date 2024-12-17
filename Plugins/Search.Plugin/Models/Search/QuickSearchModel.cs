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
 *  Product:  Search Plugin
 *  
 *  File: QuickSearchModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  24/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

#pragma warning disable IDE1006

namespace SearchPlugin.Models
{
	/// <summary>
	/// Search model passed to search controller for ajax calls
	/// </summary>
	public class QuickSearchModel
	{
		/// <summary>
		/// Keywords to be searched
		/// </summary>
		/// <value>string</value>
		public string keywords { get; set; }

		/// <summary>
		/// Unique search id used to verify the call
		/// </summary>
		/// <value>string</value>
		public string searchid { get; set; }
	}
}

#pragma warning restore IDE1006