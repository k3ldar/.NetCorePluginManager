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
using System.Text;

namespace Middleware.Helpdesk
{
    public sealed class HelpdeskTicket
    {
        #region Constructors

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

        public long Id { get; private set; }

        public LookupListItem Priority { get; private set; }

        public LookupListItem Department { get; private set; }

        public LookupListItem Status { get; private set; }

        public string Key { get; private set; }

        public string Subject { get; private set; }

        public DateTime DateCreated { get; private set; }

        public DateTime DateLastUpdated { get; private set; }

        public string CreatedBy { get; private set; }

        public string CreatedByEmail { get; private set; }

        public string LastReplier { get; private set; }

        public List<HelpdeskTicketMessage> Messages { get; private set; }

        #endregion Properties

        #region Public Methods


        #endregion Public Methods
    }
}
