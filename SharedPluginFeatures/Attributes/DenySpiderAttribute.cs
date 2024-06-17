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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatures
 *  
 *  File: DenySpiderAttribute.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  29/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
	/// <summary>
	/// The deny spider attribute is used on Controller Action methods to indicate that a spider should not use that particular route.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	public sealed class DenySpiderAttribute : Attribute
	{
		#region Constructors

		/// <summary>
		/// Default constructor, indicates that all user agents are denied.
		/// </summary>
		public DenySpiderAttribute()
			: this("*")
		{

		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="userAgent">Specify the specific user agent that is to be denied access to the route.</param>
		public DenySpiderAttribute(string userAgent)
			: this(userAgent, String.Empty)
		{

		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="userAgent">Specify the specific user agent that is to be denied access to the route.</param>
		/// <param name="comment">Comment to be included in the automatically generated robots.txt file.</param>
		/// <exception cref="ArgumentNullException">Raised if userAgent is null or empty.</exception>
		public DenySpiderAttribute(string userAgent, string comment)
		{
			if (String.IsNullOrEmpty(userAgent))
				throw new ArgumentNullException(nameof(userAgent));

			UserAgent = userAgent;
			Comment = comment;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// The user agent that is to be denied access to the route.
		/// </summary>
		/// <value>string</value>
		public string UserAgent { get; private set; }

		/// <summary>
		/// Optional comment that will appear in the robots.txt file.
		/// </summary>
		/// <value>string</value>
		public string Comment { get; private set; }

		/// <summary>
		/// Route associated with the Deny attribute
		/// </summary>
		public string Route { get; set; }

		#endregion Properties
	}
}
