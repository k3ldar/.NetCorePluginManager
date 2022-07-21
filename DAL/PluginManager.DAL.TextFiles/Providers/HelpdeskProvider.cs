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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: HelpdeskProvider.cs
 *
 *  Purpose:  IHelpdeskProvider for text based storage
 *
 *  Date        Name                Reason
 *  25/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Middleware;
using Middleware.Helpdesk;

using PluginManager.DAL.TextFiles.Tables;

using Shared.Classes;

namespace PluginManager.DAL.TextFiles.Providers
{
    internal class HelpdeskProvider : IHelpdeskProvider
    {
        #region Private Members

        private const int LowPriority = 1;
        private const int StatusOpen = 2;
        private const int SupportDepartment = 2;

        private static readonly CacheManager _memoryCache = new CacheManager("Helpdesk", new TimeSpan(0, 30, 0), true, true);

        private readonly ITextTableOperations<UserDataRow> _userDataRow;
        private readonly ITextTableOperations<FeedbackDataRow> _feedbackDataRow;
        private readonly ITextTableOperations<FAQDataRow> _faqDataRow;
        private readonly ITextTableOperations<TicketDataRow> _tickets;
        private readonly ITextTableOperations<TicketMessageDataRow> _ticketMessages;
        private readonly ITextTableOperations<TicketStatusDataRow> _ticketStatus;
        private readonly ITextTableOperations<TicketPrioritiesDataRow> _ticketPriority;
        private readonly ITextTableOperations<TicketDepartmentsDataRow> _ticketDepartments;

        #endregion Private Members

        #region Constructors

        public HelpdeskProvider(ITextTableOperations<UserDataRow> userDataRow,
            ITextTableOperations<FeedbackDataRow> feedbackDataRow, 
            ITextTableOperations<FAQDataRow> faqDataRow,
            ITextTableOperations<TicketDataRow> tickets,
            ITextTableOperations<TicketMessageDataRow> ticketMessages,
            ITextTableOperations<TicketStatusDataRow> ticketStatus,
            ITextTableOperations<TicketPrioritiesDataRow> ticketPriority,
            ITextTableOperations<TicketDepartmentsDataRow> ticketDepartments)
        {
            _userDataRow = userDataRow ?? throw new ArgumentNullException(nameof(userDataRow));
            _feedbackDataRow = feedbackDataRow ?? throw new ArgumentNullException(nameof(feedbackDataRow));
            _faqDataRow = faqDataRow ?? throw new ArgumentNullException(nameof(faqDataRow));
            _tickets = tickets ?? throw new ArgumentNullException(nameof(tickets));
            _ticketMessages = ticketMessages ?? throw new ArgumentNullException(nameof(ticketMessages));
            _ticketStatus = ticketStatus ?? throw new ArgumentNullException(nameof(ticketStatus));
            _ticketPriority = ticketPriority ?? throw new ArgumentNullException(nameof(ticketPriority));
            _ticketDepartments = ticketDepartments ?? throw new ArgumentNullException(nameof(ticketDepartments));

            //_tickets = new List<HelpdeskTicket>()
            //{
            //    new HelpdeskTicket(1,
            //        GetTicketPriorities().Where(p => p.Id == 1).FirstOrDefault(),
            //        GetTicketDepartments().Where(d => d.Id == 2).FirstOrDefault(),
            //        GetTicketStatus().Where(s => s.Id == 3).FirstOrDefault(),
            //        "ABC-123456", "Test 1", DateTime.Now, DateTime.Now, "Joe Bloggs",
            //        "joe@bloggs.com", "Joe Bloggs", new List<HelpdeskTicketMessage>()
            //        {
            //            new HelpdeskTicketMessage(DateTime.Now, "Joe Bloggs", "Hello\r\nLine 2"),
            //        }),
            //    new HelpdeskTicket(2,
            //        GetTicketPriorities().Where(p => p.Id == 1).FirstOrDefault(),
            //        GetTicketDepartments().Where(d => d.Id == 2).FirstOrDefault(),
            //        GetTicketStatus().Where(s => s.Id == 3).FirstOrDefault(),
            //        "DEF-987654", "Test 2", DateTime.Now, DateTime.Now, "Jane Doe",
            //        "jane@doe.com", "Service Representative 1", new List<HelpdeskTicketMessage>()
            //        {
            //            new HelpdeskTicketMessage(DateTime.Now, "Jane Doe", "Hello\r\nLine 2"),
            //            new HelpdeskTicketMessage(DateTime.Now, "Service Rep 1", "Hello\r\n\r\nTo you too!")
            //        }),
            //};

            //_faq = new List<KnowledgeBaseGroup>()
            //{
            //    new KnowledgeBaseGroup(0, "Plugin Interfaces", "Frequently asked questions about plugin interfaces", 0, 0, null,
            //        new List<KnowledgeBaseItem>()
            //        {
            //            new KnowledgeBaseItem(0, "IPlugin", 0, "Primary interface used to register an assembly with plugin manager"),
            //            new KnowledgeBaseItem(1, "IPluginTypesService", 0, "Retrieves all classes with the specified attribute"),
            //            new KnowledgeBaseItem(2, "IPluginHelperService", 0, "Plugin services and stuff"),
            //            new KnowledgeBaseItem(3, "IPluginVersion", 0, "Retrieves version from assembly"),
            //        }),
            //    new KnowledgeBaseGroup(1, "Shared Interfaces", "Frequently asked questions about shared interfaces", 0, 0, null,
            //       new List<KnowledgeBaseItem>()
            //       {
            //                new KnowledgeBaseItem(4, "IMemoryCache", 0, "Provides two caches with variable expire times."),
            //                new KnowledgeBaseItem(5, "ISettingsProvider", 0, "Allows plugins to easily load setting data."),
            //       }),
            //};

            //_faq.Add(new KnowledgeBaseGroup(3, "Geo Ip Interfaces", "Interaces for geoip usage", 0, 0, _faq[0],
            //    new List<KnowledgeBaseItem>()
            //    {
            //    }));

            //_faq.Add(new KnowledgeBaseGroup(4, "Supported Geo Ip Services", "details on supported Geo Ip Services", 0, 0, _faq[2],
            //    new List<KnowledgeBaseItem>()
            //    {
            //        new KnowledgeBaseItem(6, "SieraDelta Geo Ip", 0, "Details about Geo Ip Service"),
            //        new KnowledgeBaseItem(7, "Net77", 0, "Net 77 Geo Ip"),
            //        new KnowledgeBaseItem(8, "IpStack", 0, "Ip Stack Geo Ip Servies"),
            //    }));

            //_faq.Add(new KnowledgeBaseGroup(5, "Geo Ip Sub Sub Group", "Another Sub Group", 0, 0, _faq[2],
            //    new List<KnowledgeBaseItem>()
            //    {
            //        new KnowledgeBaseItem(6, "SieraDelta Geo Ip", 0, "Details about Geo Ip Service")
            //    }));
        }

        #endregion Constructors

        #region Internal Methods

        internal static void ClearCache()
        {
            _memoryCache.Clear();
        }

        #endregion Internal Methods

        #region Public Feedback Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Intended for developers not end users")]
        public List<Feedback> GetFeedback(in bool publiclyVisible)
        {
            CacheItem feedbackCache = _memoryCache.Get(nameof(GetFeedback));

            if (feedbackCache == null)
            {
                List<FeedbackDataRow> allFeedback = null;

                if (publiclyVisible)
                    allFeedback = _feedbackDataRow.Select().Where(pv => pv.ShowOnWebsite).ToList();
                else
                    allFeedback = _feedbackDataRow.Select().ToList();

                List<Feedback> Result = new List<Feedback>();

                allFeedback.ForEach(f => Result.Add(new Feedback(f.Id, f.UserName, f.Message, f.ShowOnWebsite)));

                feedbackCache = new CacheItem(nameof(GetFeedback), Result);

            }

            return (List<Feedback>)feedbackCache.Value;
        }

        public bool SubmitFeedback(in long userId, in string name, in string feedback)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrEmpty(feedback))
                throw new ArgumentNullException(nameof(feedback));

            _feedbackDataRow.Insert(new FeedbackDataRow() { UserId = userId, UserName = name, Message = feedback, ShowOnWebsite = false });

            return true;
        }

        #endregion Public Feedback Methods

        #region Public Ticket Methods

        public List<LookupListItem> GetTicketDepartments()
        {
            CacheItem cacheItem = _memoryCache.Get(nameof(GetTicketDepartments));

            if (cacheItem == null)
            {
                List<LookupListItem> Result = new List<LookupListItem>();

                List<TicketDepartmentsDataRow> departments = _ticketDepartments.Select().ToList();

                departments.ForEach(d => Result.Add(new LookupListItem((int)d.Id, d.Description)));

                cacheItem = new CacheItem(nameof(GetTicketDepartments), Result);
            }

            return (List<LookupListItem>)cacheItem.Value;
        }

        public List<LookupListItem> GetTicketPriorities()
        {
            CacheItem cacheItem = _memoryCache.Get(nameof(GetTicketPriorities));

            if (cacheItem == null)
            {
                List<LookupListItem> Result = new List<LookupListItem>();

                List<TicketPrioritiesDataRow> departments = _ticketPriority.Select().ToList();

                departments.ForEach(d => Result.Add(new LookupListItem((int)d.Id, d.Description)));

                cacheItem = new CacheItem(nameof(GetTicketPriorities), Result);
            }

            return (List<LookupListItem>)cacheItem.Value;
        }

        public List<LookupListItem> GetTicketStatus()
        {
            CacheItem cacheItem = _memoryCache.Get(nameof(GetTicketStatus));

            if (cacheItem == null)
            {
                List<LookupListItem> Result = new List<LookupListItem>();

                List<TicketStatusDataRow> departments = _ticketStatus.Select().ToList();

                departments.ForEach(d => Result.Add(new LookupListItem((int)d.Id, d.Description)));

                cacheItem = new CacheItem(nameof(GetTicketStatus), Result);
            }

            return (List<LookupListItem>)cacheItem.Value;
        }

        public bool SubmitTicket(in long userId, in int department, in int priority,
            in string userName, in string email, in string subject, in string message,
            out HelpdeskTicket ticket)
        {
            ticket = null;

            if (String.IsNullOrEmpty(userName))
                return false;

            if (String.IsNullOrEmpty(email))
                return false;

            if (String.IsNullOrEmpty(subject))
                return false;

            if (String.IsNullOrEmpty(message))
                return false;

            int idPriority = _ticketPriority.IdExists(priority) ? priority : LowPriority;
            int idStatus = StatusOpen;
            int idDepartment = _ticketDepartments.IdExists(department) ? department : SupportDepartment;

            TicketDataRow ticketDataRow = new TicketDataRow()
            {
                Priority = idPriority,
                Department = idDepartment,
                Status = idStatus,
                Key = Shared.Utilities.GetRandomKey(),
                Subject = subject,
                CreatedBy = userName,
                CreatedByEmail = email,
            };

            _tickets.Insert(ticketDataRow);

            _ticketMessages.Insert(new TicketMessageDataRow()
            {
                TicketId = ticketDataRow.Id,
                UserName = userName,
                Message = message,
            });



            ticket = new HelpdeskTicket(ticketDataRow.Id,
                GetTicketPriorities().Where(p => p.Id == idPriority).FirstOrDefault(),
                GetTicketDepartments().Where(d => d.Id == idDepartment).FirstOrDefault(),
                GetTicketStatus().Where(s => s.Id == idStatus).FirstOrDefault(),
                ticketDataRow.Key,
                subject,
                DateTime.Now,
                DateTime.Now,
                userName,
                email, 
                userName,
                new List<HelpdeskTicketMessage>()
                {
                    new HelpdeskTicketMessage(DateTime.Now, userName, message)
                });

            return true;
        }

        public HelpdeskTicket GetTicket(in long id)
        {
            //foreach (HelpdeskTicket ticket in _tickets)
            //{
            //    if (ticket.Id == id)
            //        return ticket;
            //}

            //return null;
            throw new NotImplementedException();
        }

        public HelpdeskTicket GetTicket(in string email, in string ticketKey)
        {
            //foreach (HelpdeskTicket ticket in _tickets)
            //{
            //    if (ticket.Key == ticketKey && ticket.CreatedByEmail.Equals(email, StringComparison.CurrentCultureIgnoreCase))
            //        return ticket;
            //}

            //return null;
            throw new NotImplementedException();
        }

        public bool TicketRespond(in HelpdeskTicket ticket, in string name, in string message)
        {
            //if (ticket == null)
            //    throw new ArgumentNullException(nameof(ticket));

            //if (String.IsNullOrEmpty(name))
            //    throw new ArgumentNullException(nameof(name));

            //if (String.IsNullOrEmpty(message))
            //    throw new ArgumentNullException(nameof(message));

            //ticket.Messages.Add(new HelpdeskTicketMessage(DateTime.Now, name, message));
            //ticket.Status = GetTicketStatus().Where(s => s.Id == 2).FirstOrDefault();

            //return true;
            throw new NotImplementedException();
        }

        #endregion Public Ticket Methods

        #region Public FaQ Methods

        public List<KnowledgeBaseGroup> GetKnowledgebaseGroups(in long userId, in KnowledgeBaseGroup parent)
        {
            //if (parent == null)
            //    return _faq.Where(f => f.Parent == null).ToList();

            //int parentId = parent.Id;

            //return _faq.Where(f => f.Parent != null && f.Parent.Id == parentId).ToList();
            throw new NotImplementedException();
        }

        public KnowledgeBaseGroup GetKnowledgebaseGroup(in long userId, in int id)
        {
            //int searchId = id;

            //foreach (KnowledgeBaseGroup group in _faq)
            //{
            //    if (group.Id == searchId)
            //        return group;
            //}

            //return null;
            throw new NotImplementedException();
        }

        public bool GetKnowledgebaseItem(in long userId, in int id,
            out KnowledgeBaseItem knowledgebaseItem, out KnowledgeBaseGroup parentGroup)
        {
            //foreach (KnowledgeBaseGroup group in _faq)
            //{
            //    foreach (KnowledgeBaseItem item in group.Items)
            //    {
            //        if (item.Id == id)
            //        {
            //            knowledgebaseItem = item;
            //            parentGroup = group;
            //            return true;
            //        }
            //    }
            //}

            //knowledgebaseItem = null;
            //parentGroup = null;

            //return false;
            throw new NotImplementedException();
        }

        public void KnowledbaseView(in KnowledgeBaseItem item)
        {
            throw new NotImplementedException();
            //if (item == null)
            //    throw new ArgumentNullException(nameof(item));

            //item.IncreastViewCount();
        }

        #endregion Public FaQ Methods

        #region Private Methods


        #endregion Private Methods
    }
}
