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
 *  Product:  Middleware.Plugin
 *  
 *  File: SessionDaily.cs
 *
 *  Purpose:  Daily session data
 *
 *  Date        Name                Reason
 *  12/09/2020  Simon Carter        Initially Created
 *  02/08/2022	Simon Carter		Moved to middleware
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace Middleware.SessionData
{
	/// <summary>
	/// Daily visitor statistics
	/// </summary>
	public sealed class SessionDaily : SessionBaseData
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public SessionDaily()
			: base()
		{

		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Date of Visit
		/// </summary>
		public DateTime Date { get; set; }

		#endregion Properties
	}
}
