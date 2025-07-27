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
 *  Product:  Error Manager Plugin
 *  
 *  File: Error404Model.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  17/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

namespace ErrorManager.Plugin.Models
{
	/// <summary>
	/// View model for a 404 error.
	/// </summary>
	public sealed class Error404Model : BaseModel
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public Error404Model()
		{

		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="modelData">Base model data.</param>
		/// <param name="title">Title to be displayed on the page.</param>
		public Error404Model(in IBaseModelData modelData,
			string title)
			: base(modelData)
		{
			if (String.IsNullOrEmpty(title))
				throw new ArgumentNullException(nameof(title));

			Title = title;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="modelData">Base model data.</param>
		/// <param name="title">Title to be displayed on the page.</param>
		/// <param name="message">Message to be displayed to the user.</param>
		/// <param name="image">Image to be displayed on the page.</param>
		public Error404Model(in IBaseModelData modelData,
			string title, string message, string image)
			: this(modelData, title)
		{
			if (String.IsNullOrEmpty(title))
				throw new ArgumentNullException(nameof(title));

			Title = title;
			Message = message;
			Image = image;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Title to be displayed on the page.
		/// </summary>
		/// <value>string</value>
		public string Title { get; set; }

		/// <summary>
		/// Message to be displayed to the user.
		/// </summary>
		/// <value>string</value>
		public string Message { get; set; }

		/// <summary>
		/// Image to be displayed on the page.
		/// </summary>
		/// <value>string</value>
		public string Image { get; set; }

		#endregion Properties
	}
}
