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
 *  Product:  Helpdesk Plugin
 *  
 *  File: ViewTicketViewModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

namespace HelpdeskPlugin.Models
{
    public sealed class ViewTicketViewModel : BaseModel
    {
        #region Constructors

        public ViewTicketViewModel(in BaseModelData modelData,
            in long id, in string priority, in string department,
            in string status, in string key,
            in string subject, in DateTime dateCreated, in DateTime dateLastUpdated,
            in string createdBy, in string lastReplier,
            in List<ViewTicketResponseViewModel> messages)
            : base(modelData)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (String.IsNullOrEmpty(subject))
                throw new ArgumentNullException(nameof(subject));

            if (String.IsNullOrEmpty(createdBy))
                throw new ArgumentNullException(nameof(createdBy));

            if (String.IsNullOrEmpty(lastReplier))
                throw new ArgumentNullException(nameof(lastReplier));

            if (String.IsNullOrEmpty(priority))
                throw new ArgumentNullException(nameof(priority));

            if (String.IsNullOrEmpty(status))
                throw new ArgumentNullException(nameof(status));

            if (String.IsNullOrEmpty(department))
                throw new ArgumentNullException(nameof(department));

            Id = id;
            Priority = priority;
            Department = department;
            Status = status;
            Key = key;
            Subject = subject;
            DateCreated = dateCreated;
            DateLastUpdated = dateLastUpdated;
            CreatedBy = createdBy;
            LastReplier = lastReplier;
            Messages = messages ?? throw new ArgumentNullException(nameof(messages));

            TicketResponse = new TicketResponseViewModel(id, createdBy);
        }

        #endregion Constructors

        #region Properties

        public long Id { get; private set; }

        public string Priority { get; private set; }

        public string Department { get; private set; }

        public string Status { get; private set; }

        public string Key { get; private set; }

        public string Subject { get; private set; }

        public DateTime DateCreated { get; private set; }

        public DateTime DateLastUpdated { get; private set; }

        public string CreatedBy { get; private set; }

        public string LastReplier { get; private set; }

        public List<ViewTicketResponseViewModel> Messages { get; private set; }

        public TicketResponseViewModel TicketResponse { get; private set; }

        #endregion Properties
    }
}
