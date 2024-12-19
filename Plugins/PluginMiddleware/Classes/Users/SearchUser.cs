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
 *  Copyright (c) 2012 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: SearchUser.cs
 *
 *  Purpose:  User search model
 *
 *  Date        Name                Reason
 *  09/08/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace Middleware.Users
{
	/// <summary>
	/// Class used to return results for user searches.
	/// </summary>
	public sealed class SearchUser
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		public SearchUser()
		{

		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id">Id of user.</param>
		/// <param name="name">Name of user.</param>
		/// <param name="email">Email address for user.</param>
		public SearchUser(in long id, in string name, in string email)
		{
			Id = id;
			Name = name;
			Email = email;
		}

		#region Properties

		/// <summary>
		/// Unique id for the user.
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Name of the user.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Email for the user.
		/// </summary>
		public string Email { get; set; }

		#endregion Properties
	}
}
