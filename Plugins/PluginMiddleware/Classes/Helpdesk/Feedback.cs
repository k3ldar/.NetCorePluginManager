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
 *  Product:  PluginMiddleware
 *  
 *  File: Feedback.cs
 *
 *  Purpose:  Download Categories
 *
 *  Date        Name                Reason
 *  13/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace Middleware.Helpdesk
{
	/// <summary>
	/// Feedback item used in HelpdeskPlugin module and IHelpdeskProvider interface.
	/// </summary>
	public sealed class Feedback
	{
		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id">Unique id of feedback item.</param>
		/// <param name="username">Username of user providing feedback.</param>
		/// <param name="message">Feedback message provided.</param>
		/// <param name="showOnWebsite">Indicates whether the feedback can be shown on a website</param>
		public Feedback(in long id, in string username, in string message, in bool showOnWebsite)
		{
			Id = id;
			Username = username;
			Message = message;
			ShowOnWebsite = showOnWebsite;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Unique id of feedback item.
		/// </summary>
		/// <value>long</value>
		public long Id { get; private set; }

		/// <summary>
		/// Username of user providing feedback.
		/// </summary>
		/// <value>string</value>
		public string Username { get; private set; }

		/// <summary>
		/// Feedback message provided.
		/// </summary>
		/// <value>string</value>
		public string Message { get; private set; }

		/// <summary>
		/// Indicates whether the feedback can be shown on a website.
		/// </summary>
		/// <value>bool</value>
		public bool ShowOnWebsite { get; private set; }

		#endregion Properties
	}
}
