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
using System.Runtime.CompilerServices;

using Middleware;
using Middleware.Helpdesk;

using PluginManager.DAL.TextFiles.Tables;

using SimpleDB;

using Shared.Classes;

namespace PluginManager.DAL.TextFiles.Providers
{
    internal class HelpdeskProvider : IHelpdeskProvider
    {
        #region Private Members

        private const int MaxRecursionDepth = 10;
        private const int LowPriority = 1;
        private const int StatusOpen = 1;
        private const int StatusOnHold = 2;
        private const int SupportDepartment = 2;

        private static readonly CacheManager _memoryCache = new CacheManager("Helpdesk", new TimeSpan(0, 30, 0), true, true);

        private readonly ISimpleDBOperations<UserDataRow> _userDataRow;
        private readonly ISimpleDBOperations<FeedbackDataRow> _feedbackDataRow;
        private readonly ISimpleDBOperations<FAQDataRow> _faqDataRow;
        private readonly ISimpleDBOperations<FAQItemDataRow> _faqItemDataRow;
        private readonly ISimpleDBOperations<TicketDataRow> _tickets;
        private readonly ISimpleDBOperations<TicketMessageDataRow> _ticketMessages;
        private readonly ISimpleDBOperations<TicketStatusDataRow> _ticketStatus;
        private readonly ISimpleDBOperations<TicketPrioritiesDataRow> _ticketPriority;
        private readonly ISimpleDBOperations<TicketDepartmentsDataRow> _ticketDepartments;

        #endregion Private Members

        #region Constructors

        public HelpdeskProvider(ISimpleDBOperations<UserDataRow> userDataRow,
            ISimpleDBOperations<FeedbackDataRow> feedbackDataRow, 
            ISimpleDBOperations<FAQDataRow> faqDataRow,
            ISimpleDBOperations<FAQItemDataRow> faqItemDataRow,
            ISimpleDBOperations<TicketDataRow> tickets,
            ISimpleDBOperations<TicketMessageDataRow> ticketMessages,
            ISimpleDBOperations<TicketStatusDataRow> ticketStatus,
            ISimpleDBOperations<TicketPrioritiesDataRow> ticketPriority,
            ISimpleDBOperations<TicketDepartmentsDataRow> ticketDepartments)
        {
            _userDataRow = userDataRow ?? throw new ArgumentNullException(nameof(userDataRow));
            _feedbackDataRow = feedbackDataRow ?? throw new ArgumentNullException(nameof(feedbackDataRow));
            _faqDataRow = faqDataRow ?? throw new ArgumentNullException(nameof(faqDataRow));
            _faqItemDataRow = faqItemDataRow ?? throw new ArgumentNullException(nameof(faqItemDataRow));
            _tickets = tickets ?? throw new ArgumentNullException(nameof(tickets));
            _ticketMessages = ticketMessages ?? throw new ArgumentNullException(nameof(ticketMessages));
            _ticketStatus = ticketStatus ?? throw new ArgumentNullException(nameof(ticketStatus));
            _ticketPriority = ticketPriority ?? throw new ArgumentNullException(nameof(ticketPriority));
            _ticketDepartments = ticketDepartments ?? throw new ArgumentNullException(nameof(ticketDepartments));

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

            string newKey = Shared.Utilities.GetRandomKey();
            int loopCount = 0;

            while (_tickets.IndexExists(TicketDataRow.IndexUserKey, $"{newKey}{email}"))
            {
                newKey = Shared.Utilities.GetRandomKey();
                loopCount++;

                if (loopCount > 100)
                    return false;
            }

            TicketDataRow ticketDataRow = new TicketDataRow()
            {
                Priority = idPriority,
                Department = idDepartment,
                Status = idStatus,
                Key = newKey,
                Subject = subject,
                CreatedBy = userName,
                CreatedByEmail = email,
                LastReplier = userName,
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
            TicketDataRow ticket = _tickets.Select(id);

            if (ticket == null)
                return null;

           return ConvertTicketDataRow(ticket, _ticketMessages.Select().Where(tm => tm.TicketId.Equals(ticket.Id)).ToList());
        }

        public HelpdeskTicket GetTicket(in string email, in string ticketKey)
        {
            if (String.IsNullOrEmpty(email))
                return null;

            if (String.IsNullOrEmpty(ticketKey))
                return null;

            string emailAddress = email;
            string key = ticketKey;

            TicketDataRow ticket = _tickets.Select()
                .Where(t => t.CreatedByEmail.Equals(emailAddress, StringComparison.InvariantCultureIgnoreCase) && 
                    t.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (ticket == null)
                return null;

            return ConvertTicketDataRow(ticket, _ticketMessages.Select().Where(tm => tm.TicketId.Equals(ticket.Id)).ToList());
        }

        public bool TicketRespond(in HelpdeskTicket ticket, in string name, in string message)
        {
            if (ticket == null)
                throw new ArgumentNullException(nameof(ticket));

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            _ticketMessages.Insert(new TicketMessageDataRow() { TicketId = ticket.Id, UserName = name, Message = message });
            ticket.Messages.Add(new HelpdeskTicketMessage(DateTime.Now, name, message));

            int statusId = name.Equals(ticket.CreatedBy, StringComparison.InvariantCultureIgnoreCase) ? StatusOpen : StatusOnHold;
            ticket.Status = GetTicketStatus().Where(s => s.Id == statusId).FirstOrDefault();
            ticket.LastReplier = name;

            TicketDataRow ticketDataRow = _tickets.Select(ticket.Id);
            ticketDataRow.LastReplier = name;
            ticketDataRow.Status = statusId;
            _tickets.Update(ticketDataRow);

            return true;
        }

        #endregion Public Ticket Methods

        #region Public FaQ Methods

        public List<KnowledgeBaseGroup> GetKnowledgebaseGroups(in long userId, in KnowledgeBaseGroup parent)
        {
            string cacheName = $"{nameof(GetKnowledgebaseGroups)} {(parent == null ? 0 : parent.Id)}";
            CacheItem cacheItem = _memoryCache.Get(cacheName);

            if (cacheItem == null)
            {
                List<KnowledgeBaseGroup> items = null;

                long parentId = parent == null ? 0 : parent.Id;
                items = ConvertFaqDataListToKbGroupList(_faqDataRow.Select().Where(f => f.Parent.Equals(parentId)), parent);

                cacheItem = new CacheItem(cacheName, items);

                _memoryCache.Add(cacheName, cacheItem);
            }

            return (List<KnowledgeBaseGroup>)cacheItem.Value;
        }

        public KnowledgeBaseGroup GetKnowledgebaseGroup(in long userId, in long id)
        {
            return InternalGetKnowledgebaseGroup(id, 0);
        }

        private KnowledgeBaseGroup InternalGetKnowledgebaseGroup(long id, int recursionDepth)
        {
            if (recursionDepth > MaxRecursionDepth)
                return null;

            string cacheName = $"{nameof(GetKnowledgebaseGroup)} {id}";
            CacheItem cacheItem = _memoryCache.Get(cacheName);

            if (cacheItem == null)
            {
                FAQDataRow faqDataRow = _faqDataRow.Select(id);

                if (faqDataRow == null)
                    return null;

                FAQDataRow parentDataRow = _faqDataRow.Select(faqDataRow.Parent);

                KnowledgeBaseGroup item = ConvertFaqDataRowToKbGroup(faqDataRow, InternalGetKnowledgebaseGroup(faqDataRow.Parent, ++recursionDepth));

                cacheItem = new CacheItem(cacheName, item);

                _memoryCache.Add(cacheName, cacheItem);
            }

            return (KnowledgeBaseGroup)cacheItem.Value;
        }

        public bool GetKnowledgebaseItem(in long userId, in long id,
            out KnowledgeBaseItem knowledgebaseItem, out KnowledgeBaseGroup parentGroup)
        {
            FAQItemDataRow item = _faqItemDataRow.Select(id);

            if (item != null)
            {
                parentGroup = InternalGetKnowledgebaseGroup(item.ParentId, 0);
                knowledgebaseItem = new KnowledgeBaseItem(item.Id, item.Description, item.ViewCount, item.Content);
                return true;
            }

            knowledgebaseItem = null;
            parentGroup = null;

            return false;
        }

        public void KnowledgebaseView(in KnowledgeBaseItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            FAQItemDataRow faqItemDataRow = _faqItemDataRow.Select(item.Id);

            if (faqItemDataRow == null)
                return;

            faqItemDataRow.ViewCount++;
            _faqItemDataRow.Update(faqItemDataRow);
            item.IncreaseViewCount();
        }

        #endregion Public FaQ Methods

        #region Private Methods

        private List<KnowledgeBaseGroup> ConvertFaqDataListToKbGroupList(IEnumerable<FAQDataRow> faqDataRow, KnowledgeBaseGroup parent)
        {
            List<KnowledgeBaseGroup> Result = new List<KnowledgeBaseGroup>();

            foreach (FAQDataRow item in faqDataRow)
            {
                Result.Add(ConvertFaqDataRowToKbGroup(item, parent));
            }

            return Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private KnowledgeBaseGroup ConvertFaqDataRowToKbGroup(FAQDataRow faqDataRow, KnowledgeBaseGroup parent)
        {
            if (faqDataRow == null)
                return null;

            List<KnowledgeBaseItem> childItems = ConvertFaqItemDataToFaqItemList(faqDataRow);
            return new KnowledgeBaseGroup(faqDataRow.Id, faqDataRow.Name, faqDataRow.Description, faqDataRow.Order, faqDataRow.ViewCount, parent, childItems);
        }

        private List<KnowledgeBaseItem> ConvertFaqItemDataToFaqItemList(FAQDataRow faqDataItem)
        {
            List<KnowledgeBaseItem> Result = new List<KnowledgeBaseItem>();

            foreach (FAQItemDataRow item in _faqItemDataRow.Select().Where(i => i.ParentId.Equals(faqDataItem.Id)))
            {
                Result.Add(new KnowledgeBaseItem(item.Id, item.Description, item.ViewCount, item.Content));
            }

            return Result;
        }

        private HelpdeskTicket ConvertTicketDataRow(TicketDataRow ticketDataRow, List<TicketMessageDataRow> messages)
        {
            if (ticketDataRow == null)
                return null;

            List<HelpdeskTicketMessage> messageList = new List<HelpdeskTicketMessage>();

            foreach (TicketMessageDataRow messageDataRow in messages)
                messageList.Add(new HelpdeskTicketMessage(messageDataRow.Created, messageDataRow.UserName, messageDataRow.Message));

            return new HelpdeskTicket(ticketDataRow.Id, 
                GetTicketPriorities().Where(tp => tp.Id.Equals((int)ticketDataRow.Priority)).First(),
                GetTicketDepartments().Where(td => td.Id.Equals((int)ticketDataRow.Department)).First(),
                GetTicketStatus().Where(ts => ts.Id.Equals((int)ticketDataRow.Status)).First(), 
                ticketDataRow.Key, 
                ticketDataRow.Subject, ticketDataRow.Created, ticketDataRow.Updated, ticketDataRow.CreatedBy, 
                ticketDataRow.CreatedByEmail, ticketDataRow.LastReplier, messageList);
        }

        #endregion Private Methods
    }
}
