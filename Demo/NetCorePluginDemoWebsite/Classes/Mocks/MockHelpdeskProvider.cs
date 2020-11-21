﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Product:  Demo Website
 *  
 *  File: MockHelpdeskProvider.cs
 *
 *  Purpose:  Mock IHelpdeskProvider for tesing purpose
 *
 *  Date        Name                Reason
 *  13/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;

using Middleware;
using Middleware.Helpdesk;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
    public class MockHelpdeskProvider : IHelpdeskProvider
    {
        #region Private Members

        private static List<Feedback> _feedback;
        private static List<HelpdeskTicket> _tickets;
        private static List<KnowledgeBaseGroup> _faq;

        #endregion Private Members

        #region Constructors

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Intended for developers not end users")]
        public MockHelpdeskProvider()
        {
            _tickets = new List<HelpdeskTicket>()
            {
                new HelpdeskTicket(1,
                    GetTicketPriorities().Where(p => p.Id == 1).FirstOrDefault(),
                    GetTicketDepartments().Where(d => d.Id == 2).FirstOrDefault(),
                    GetTicketStatus().Where(s => s.Id == 3).FirstOrDefault(),
                    "ABC-123456", "Test 1", DateTime.Now, DateTime.Now, "Joe Bloggs",
                    "joe@bloggs.com", "Joe Bloggs", new List<HelpdeskTicketMessage>()
                    {
                        new HelpdeskTicketMessage(DateTime.Now, "Joe Bloggs", "Hello\r\nLine 2"),
                    }),
                new HelpdeskTicket(2,
                    GetTicketPriorities().Where(p => p.Id == 1).FirstOrDefault(),
                    GetTicketDepartments().Where(d => d.Id == 2).FirstOrDefault(),
                    GetTicketStatus().Where(s => s.Id == 3).FirstOrDefault(),
                    "DEF-987654", "Test 2", DateTime.Now, DateTime.Now, "Jane Doe",
                    "jane@doe.com", "Service Representative 1", new List<HelpdeskTicketMessage>()
                    {
                        new HelpdeskTicketMessage(DateTime.Now, "Jane Doe", "Hello\r\nLine 2"),
                        new HelpdeskTicketMessage(DateTime.Now, "Service Rep 1", "Hello\r\n\r\nTo you too!")
                    }),
            };

            _faq = new List<KnowledgeBaseGroup>()
            {
                new KnowledgeBaseGroup(0, "Plugin Interfaces", "Frequently asked questions about plugin interfaces", 0, 0, null,
                    new List<KnowledgeBaseItem>()
                    {
                        new KnowledgeBaseItem(0, "IPlugin", 0, "Primary interface used to register an assembly with plugin manager"),
                        new KnowledgeBaseItem(1, "IPluginTypesService", 0, "Retrieves all classes with the specified attribute"),
                        new KnowledgeBaseItem(2, "IPluginHelperService", 0, "Plugin services and stuff"),
                        new KnowledgeBaseItem(3, "IPluginVersion", 0, "Retrieves version from assembly"),
                    }),
                new KnowledgeBaseGroup(1, "Shared Interfaces", "Frequently asked questions about shared interfaces", 0, 0, null,
                   new List<KnowledgeBaseItem>()
                   {
                            new KnowledgeBaseItem(4, "IMemoryCache", 0, "Provides two caches with variable expire times."),
                            new KnowledgeBaseItem(5, "ISettingsProvider", 0, "Allows plugins to easily load setting data."),
                   }),
            };

            _faq.Add(new KnowledgeBaseGroup(3, "Geo Ip Interfaces", "Interaces for geoip usage", 0, 0, _faq[0],
                new List<KnowledgeBaseItem>()
                {
                }));

            _faq.Add(new KnowledgeBaseGroup(4, "Supported Geo Ip Services", "details on supported Geo Ip Services", 0, 0, _faq[2],
                new List<KnowledgeBaseItem>()
                {
                    new KnowledgeBaseItem(6, "SieraDelta Geo Ip", 0, "Details about Geo Ip Service"),
                    new KnowledgeBaseItem(7, "Net77", 0, "Net 77 Geo Ip"),
                    new KnowledgeBaseItem(8, "IpStack", 0, "Ip Stack Geo Ip Servies"),
                }));

            _faq.Add(new KnowledgeBaseGroup(5, "Geo Ip Sub Sub Group", "Another Sub Group", 0, 0, _faq[2],
                new List<KnowledgeBaseItem>()
                {
                    new KnowledgeBaseItem(6, "SieraDelta Geo Ip", 0, "Details about Geo Ip Service")
                }));
        }

        #endregion Constructors

        #region Public Feedback Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Intended for developers not end users")]
        public List<Feedback> GetFeedback(in bool publiclyVisible)
        {
            if (_feedback == null)
            {
                _feedback = new List<Feedback>()
                {
                    new Feedback(1, "Joe Bloggs", "Asp Net core is awesome", true),
                    new Feedback(2, "Jane Doe", "AspNetCore.PluginManager is extremely flexible", true),
                };
            }

            return _feedback;
        }

        public bool SubmitFeedback(in long userId, in string name, in string feedback)
        {
            List<Feedback> fb = GetFeedback(true);

            fb.Add(new Feedback(fb.Count + 1, name, feedback, true));

            return true;
        }

        #endregion Public Feedback Methods

        #region Public Ticket Methods

        public List<LookupListItem> GetTicketDepartments()
        {
            return new List<LookupListItem>()
            {
                new LookupListItem(1, "Sales"),
                new LookupListItem(2, "Support"),
                new LookupListItem(3, "Returns"),
            };
        }

        public List<LookupListItem> GetTicketPriorities()
        {
            return new List<LookupListItem>()
            {
                new LookupListItem(1, "Low"),
                new LookupListItem(2, "Medium"),
                new LookupListItem(3, "High"),
            };
        }

        public List<LookupListItem> GetTicketStatus()
        {
            return new List<LookupListItem>()
            {
                new LookupListItem(1, "Closed"),
                new LookupListItem(2, "Open"),
                new LookupListItem(3, "On Hold"),
            };
        }

        public bool SubmitTicket(in long userId, in int department, in int priority,
            in string userName, in string email, in string subject, in string message,
            out HelpdeskTicket ticket)
        {
            int idPriority = priority;
            int idStatus = 2;
            int idDepartment = department;

            ticket = new HelpdeskTicket(_tickets.Count + 1,
                GetTicketPriorities().Where(p => p.Id == idPriority).FirstOrDefault(),
                GetTicketDepartments().Where(d => d.Id == idDepartment).FirstOrDefault(),
                GetTicketStatus().Where(s => s.Id == idStatus).FirstOrDefault(),
                Shared.Utilities.GetRandomKey(),
                subject,
                DateTime.Now,
                DateTime.Now,
                userName,
                email, userName,
                new List<HelpdeskTicketMessage>()
                {
                    new HelpdeskTicketMessage(DateTime.Now, userName, message)
                });

            _tickets.Add(ticket);

            return true;
        }

        public HelpdeskTicket GetTicket(in long id)
        {
            foreach (HelpdeskTicket ticket in _tickets)
            {
                if (ticket.Id == id)
                    return ticket;
            }

            return null;
        }

        public HelpdeskTicket GetTicket(in string email, in string ticketKey)
        {
            foreach (HelpdeskTicket ticket in _tickets)
            {
                if (ticket.Key == ticketKey && ticket.CreatedByEmail.Equals(email, StringComparison.CurrentCultureIgnoreCase))
                    return ticket;
            }

            return null;
        }

        public bool TicketRespond(in HelpdeskTicket ticket, in string name, in string message)
        {
            if (ticket == null)
                throw new ArgumentNullException(nameof(ticket));

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            ticket.Messages.Add(new HelpdeskTicketMessage(DateTime.Now, name, message));
            ticket.Status = GetTicketStatus().Where(s => s.Id == 2).FirstOrDefault();

            return true;
        }

        #endregion Public Ticket Methods

        #region Public FaQ Methods

        public List<KnowledgeBaseGroup> GetKnowledgebaseGroups(in long userId, in KnowledgeBaseGroup parent)
        {
            if (parent == null)
                return _faq.Where(f => f.Parent == null).ToList();

            int parentId = parent.Id;

            return _faq.Where(f => f.Parent != null && f.Parent.Id == parentId).ToList();
        }

        public KnowledgeBaseGroup GetKnowledgebaseGroup(in long userId, in int id)
        {
            int searchId = id;

            foreach (KnowledgeBaseGroup group in _faq)
            {
                if (group.Id == searchId)
                    return group;
            }

            return null;
        }

        public bool GetKnowledgebaseItem(in long userId, in int id,
            out KnowledgeBaseItem knowledgebaseItem, out KnowledgeBaseGroup parentGroup)
        {
            foreach (KnowledgeBaseGroup group in _faq)
            {
                foreach (KnowledgeBaseItem item in group.Items)
                {
                    if (item.Id == id)
                    {
                        knowledgebaseItem = item;
                        parentGroup = group;
                        return true;
                    }
                }
            }

            knowledgebaseItem = null;
            parentGroup = null;

            return false;
        }

        public void KnowledbaseView(in KnowledgeBaseItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            item.IncreastViewCount();
        }

        #endregion Public FaQ Methods

        #region Private Methods


        #endregion Private Methods

    }
}
