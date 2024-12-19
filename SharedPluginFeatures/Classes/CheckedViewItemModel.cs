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
 *  Product:  SharedPluginFeatues
 *  
 *  File: CheckedViewItemModel.cs
 *
 *  Purpose:  View model item for checked items.
 *
 *  Date        Name                Reason
 *  15/03/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
	/// <summary>
	/// Model for views where check boxes can be used.
	/// </summary>
	public sealed class CheckedViewItemModel
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public CheckedViewItemModel()
		{

		}

		/// <summary>
		/// Constructor for creating an item with a name, selected will default to false
		/// </summary>
		/// <param name="name">Name of the checked box item.</param>
		public CheckedViewItemModel(in string name)
			: this(name, false)
		{

		}

		/// <summary>
		/// Constructor for creating an item with a specific name and selected value
		/// </summary>
		/// <param name="name">Name of the checked box item.</param>
		/// <param name="selected">Indicates whether the value is selected or not</param>
		public CheckedViewItemModel(in string name, in bool selected)
		{
			if (String.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(name);

			Name = name;
			Selected = selected;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Name of the checked box item.
		/// </summary>
		/// <value>string</value>
		public string Name { get; set; }

		/// <summary>
		/// Indicates whether the item is selected or not.
		/// </summary>
		/// <value>bool</value>
		public bool Selected { get; set; }

		#endregion
	}
}
