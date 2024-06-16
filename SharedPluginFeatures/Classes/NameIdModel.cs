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
 *  Copyright (c) 2012 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatues
 *  
 *  File: NameIdModel.cs
 *
 *  Purpose:  MVC model class for holding id and name
 *
 *  Date        Name                Reason
 *  129/11/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
	/// <summary>
	/// Default model for containing Names with Ids
	/// </summary>
	public sealed class NameIdModel
	{
		#region Constructors

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		public NameIdModel(long id, string name)
		{
			if (String.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			Id = id;
			Name = name;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Id value
		/// </summary>
		/// <value>long</value>
		public long Id { get; private set; }

		/// <summary>
		/// Name
		/// </summary>
		/// <value>string</value>
		public string Name { get; private set; }

		#endregion Properties
	}
}
