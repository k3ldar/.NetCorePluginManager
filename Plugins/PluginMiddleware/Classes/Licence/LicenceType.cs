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
 *  Product:  PluginMiddleware
 *  
 *  File: LicenceType.cs
 *
 *  Purpose:  Licence Types
 *
 *  Date        Name                Reason
 *  05/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace Middleware.Accounts.Licences
{
	/// <summary>
	/// List item for type of licences available.
	/// </summary>
	public sealed class LicenceType
	{
		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id">Id of licence type.</param>
		/// <param name="description">Description of licence type.</param>
		public LicenceType(in long id, in string description)
		{
			if (String.IsNullOrEmpty(description))
				throw new ArgumentNullException(nameof(description));

			Id = id;
			Description = description;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Id of licence type.
		/// </summary>
		/// <value>int</value>
		public long Id { get; private set; }

		/// <summary>
		/// Description of licence type.
		/// </summary>
		/// <value>string</value>
		public string Description { get; private set; }

		#endregion Properties
	}
}
