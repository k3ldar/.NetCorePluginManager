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
 *  Product:  SharedPluginFeatures
 *  
 *  File: BadEggAttribute.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  08/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
	/// <summary>
	/// BadEgg attribute, see BadEgg.Plugin for further information on how this attribute is used.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class BadEggAttribute : Attribute
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public BadEggAttribute()
			: this(true, true)
		{

		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="validateQuery">If true then query input values will be validated.</param>
		/// <param name="validateForm">If true then form input values will be validated</param>
		public BadEggAttribute(bool validateQuery, bool validateForm)
		{
			ValidateQueryFields = validateQuery;
			ValidateFormFields = validateForm;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Indicates that query field values should be validated for this route.
		/// </summary>
		/// <value>bool.  If true then all query fields will be validated for the route.</value>
		public bool ValidateQueryFields { get; private set; }

		/// <summary>
		/// Indicates that form field values should be validated.
		/// </summary>
		/// <value>bool.  If true then all form fields will be validated for the route.</value>
		public bool ValidateFormFields { get; private set; }

		#endregion Properties
	}
}
