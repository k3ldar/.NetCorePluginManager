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
 *  File: HelpdeskTicketMessage.cs
 *
 *  Purpose:  Helpdesk support ticket Message
 *
 *  Date        Name                Reason
 *  21/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace Middleware.Helpdesk
{
	/// <summary>
	/// Individual message for a HelpdeskTicket message.
	/// </summary>
	public sealed class HelpdeskTicketMessage
	{
		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dateCreated">Date/time message was created.</param>
		/// <param name="userName">Name of the person creating the message.</param>
		/// <param name="message">Message.</param>
		public HelpdeskTicketMessage(in DateTime dateCreated, in string userName, in string message)
		{
			if (String.IsNullOrEmpty(userName))
				throw new ArgumentNullException(nameof(userName));

			if (String.IsNullOrEmpty(message))
				throw new ArgumentNullException(nameof(message));

			DateCreated = dateCreated;
			UserName = userName;
			Message = message;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Date/time message was created.
		/// </summary>
		/// <value>DateTime</value>
		public DateTime DateCreated { get; private set; }

		/// <summary>
		/// Name of the person creating the message.
		/// </summary>
		/// <value>string</value>
		public string UserName { get; private set; }

		/// <summary>
		/// Message.
		/// </summary>
		/// <value>string</value>
		public string Message { get; private set; }

		#endregion Properties
	}
}
