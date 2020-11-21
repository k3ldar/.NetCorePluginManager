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
 *  Copyright (c) 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: HelpdeskTicket.cs
 *
 *  Purpose:  Helpdesk support ticket
 *
 *  Date        Name                Reason
 *  21/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

namespace Middleware.Helpdesk
{
    /// <summary>
    /// Represents a Helpdesk support ticket and used with IHelpdeskProvider and the HelpdeskPlugin module.
    /// </summary>
    public sealed class HelpdeskTicket
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Unique id of the helpdesk ticket.</param>
        /// <param name="priority">Priority assigned to the support ticket.</param>
        /// <param name="department">Department to which the support ticket is assigned.</param>
        /// <param name="status">Current status of the helpdesk ticket.</param>
        /// <param name="key">Unique ticket key used to identify the ticket.</param>
        /// <param name="subject">Subject of the helpdesk message.</param>
        /// <param name="dateCreated">Date and time the ticket was created.</param>
        /// <param name="dateLastUpdated">Date and time the ticket was last updated.</param>
        /// <param name="createdBy">Name of the person creating the support ticket.</param>
        /// <param name="createdByEmail">Email address of the person who created the support ticket.</param>
        /// <param name="lastReplier">Name of the person who last responded to the support ticket.</param>
        /// <param name="messages">List of all HelpdeskTicketMessage messages within the support ticket.</param>
        public HelpdeskTicket(in long id, in LookupListItem priority, in LookupListItem department, 
            in LookupListItem status, in string key, 
            in string subject, in DateTime dateCreated, in DateTime dateLastUpdated,
            in string createdBy, in string createdByEmail, in string lastReplier,
            in List<HelpdeskTicketMessage> messages)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (String.IsNullOrEmpty(subject))
                throw new ArgumentNullException(nameof(subject));

            if (String.IsNullOrEmpty(createdBy))
                throw new ArgumentNullException(nameof(createdBy));

            if (String.IsNullOrEmpty(createdByEmail))
                throw new ArgumentNullException(nameof(createdByEmail));

            if (String.IsNullOrEmpty(lastReplier))
                throw new ArgumentNullException(nameof(lastReplier));

            Id = id;
            Priority = priority ?? throw new ArgumentNullException(nameof(priority));
            Department = department ?? throw new ArgumentNullException(nameof(department));
            Status = status ?? throw new ArgumentNullException(nameof(status));
            Key = key;
            Subject = subject;
            DateCreated = dateCreated;
            DateLastUpdated = dateLastUpdated;
            CreatedBy = createdBy;
            CreatedByEmail = createdByEmail;
            LastReplier = lastReplier;
            Messages = messages ?? throw new ArgumentNullException(nameof(messages));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Unique id of the helpdesk ticket.
        /// </summary>
        /// <value>long</value>
        public long Id { get; private set; }

        /// <summary>
        /// Priority assigned to the support ticket.
        /// </summary>
        /// <value>LookupListItem</value>
        public LookupListItem Priority { get; private set; }

        /// <summary>
        /// Department to which the support ticket is assigned.
        /// </summary>
        /// <value>LookupListItem</value>
        public LookupListItem Department { get; private set; }

        /// <summary>
        /// Current status of the helpdesk ticket.
        /// </summary>
        /// <value>LookupListItem</value>
        public LookupListItem Status { get; set; }

        /// <summary>
        /// Unique ticket key used to identify the ticket.
        /// </summary>
        /// <value>string</value>
        public string Key { get; private set; }

        /// <summary>
        /// Subject of the helpdesk message.
        /// </summary>
        /// <value>string</value>
        public string Subject { get; private set; }

        /// <summary>
        /// Date and time the ticket was created.
        /// </summary>
        /// <value>DateTime</value>
        public DateTime DateCreated { get; private set; }

        /// <summary>
        /// Date and time the ticket was last updated.
        /// </summary>
        /// <value>DateTime</value>
        public DateTime DateLastUpdated { get; private set; }

        /// <summary>
        /// Name of the person creating the support ticket.
        /// </summary>
        /// <value>string</value>
        public string CreatedBy { get; private set; }

        /// <summary>
        /// Email address of the person who created the support ticket.
        /// </summary>
        /// <value>string</value>
        public string CreatedByEmail { get; private set; }

        /// <summary>
        /// Name of the person who last responded to the support ticket.
        /// </summary>
        /// <value>string</value>
        public string LastReplier { get; private set; }

        /// <summary>
        /// List of all HelpdeskTicketMessage messages within the support ticket.
        /// </summary>
        /// <value>List&lt;HelpdestTicketMessage&gt;</value>
        public List<HelpdeskTicketMessage> Messages { get; private set; }

        #endregion Properties
    }
}
