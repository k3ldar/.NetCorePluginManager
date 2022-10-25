﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  UserSessionMiddleware.Plugin
 *  
 *  File: IUrlHash.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  02/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using Middleware;

namespace UserSessionMiddleware.Plugin.Classes.SessionData
{
	/// <summary>
	/// Interface for setting a Url hash
	/// </summary>
	public interface IUrlHash
	{
		/// <summary>
		/// Allows the setting of an IUrlHashProvider
		/// </summary>
		/// <param name="urlHashProvider"></param>
		void SetUrlHash(IUrlHashProvider urlHashProvider);
	}
}
