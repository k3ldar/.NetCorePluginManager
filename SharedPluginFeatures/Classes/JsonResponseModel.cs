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
 *  File: JsonResponseModel.cs
 *
 *  Purpose:  Json response model
 *
 *  Date        Name                Reason
 *  05/05/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
	/// <summary>
	/// Simple model that is used in combination with JsonResult to supply specific data
	/// in response to a request
	/// </summary>
	public sealed class JsonResponseModel
	{
		#region Constructors

		/// <summary>
		/// Default constructor, sets success to false and provides empty data string.
		/// </summary>
		public JsonResponseModel()
			: this(false)
		{
		}

		/// <summary>
		/// Constructor allowing the setting of the success element and provides empty data string.
		/// </summary>
		/// <param name="success">Indicates whether the response is successful or not.</param>
		public JsonResponseModel(bool success)
		{
			Success = success;
			ResponseData = String.Empty;
		}

		/// <summary>
		/// Constructor for success with mandatory data string.
		/// </summary>
		/// <param name="data">response data, json, xml plain string etc</param>
		/// <exception cref="ArgumentNullException">Thrown if data is null or empty string.</exception>
		public JsonResponseModel(string data)
		{
			if (String.IsNullOrEmpty(data))
				throw new ArgumentNullException(nameof(data));

			Success = true;
			ResponseData = data;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="success">Indicates whether the response is successful or not.</param>
		/// <param name="data">response data, json, xml plain string etc</param>
		public JsonResponseModel(bool success, string data)
		{
			Success = success;
			ResponseData = data ?? throw new ArgumentNullException(nameof(data));
		}

		#endregion Constructors

		#region Public Properties

		/// <summary>
		/// Indicates success or failure
		/// </summary>
		/// <value>bool</value>
		public bool Success { get; set; }

		/// <summary>
		/// Response data, this can be a string of any type including Json, xml etc
		/// </summary>
		/// <value>string</value>
		public string ResponseData { get; set; }

		#endregion Public Properties
	}
}
