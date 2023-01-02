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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockHelpdeskProvider.cs
 *
 *  Purpose:  Mock helpdesk provider
 *
 *  Date        Name                Reason
 *  16/12/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Middleware;
using Middleware.Helpdesk;

namespace AspNetCore.PluginManager.Tests.Shared
{
	internal class MockHelpdeskProvider : IHelpdeskProvider
	{
		public List<Feedback> GetFeedback(in bool publiclyVisible)
		{
			throw new NotImplementedException();
		}

		public KnowledgeBaseGroup GetKnowledgebaseGroup(in long userId, in long id)
		{
			throw new NotImplementedException();
		}

		public List<KnowledgeBaseGroup> GetKnowledgebaseGroups(in long userId, in KnowledgeBaseGroup parent)
		{
			throw new NotImplementedException();
		}

		public bool GetKnowledgebaseItem(in long userId, in long id, out KnowledgeBaseItem knowledgebaseItem, out KnowledgeBaseGroup parentGroup)
		{
			throw new NotImplementedException();
		}

		public HelpdeskTicket GetTicket(in long id)
		{
			throw new NotImplementedException();
		}

		public HelpdeskTicket GetTicket(in string email, in string ticketKey)
		{
			string key = ticketKey;
			return TicketsSubmitted.FirstOrDefault(ts => ts.Key.Equals(new String(key)));
		}

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

		public void KnowledgebaseView(in KnowledgeBaseItem item)
		{
			throw new NotImplementedException();
		}

		public bool SubmitFeedback(in long userId, in string name, in string feedback)
		{
			throw new NotImplementedException();
		}

		public bool SubmitTicket(in long userId, in int department, in int priority, in string userName, in string email, 
			in string subject, in string message, out HelpdeskTicket ticket)
		{
			return SubmitTicket(userId, department, priority, userName, email, subject, message, "key 123", out ticket);
		}

		public bool SubmitTicket(in long userId, in int department, in int priority, in string userName, in string email,
			in string subject, in string message, in string ticketId, out HelpdeskTicket ticket)
		{
			List<HelpdeskTicketMessage> ticketMessages = new()
			{
				new HelpdeskTicketMessage(DateTime.UtcNow, userName, message)
			};

			ticket = new HelpdeskTicket(TicketsSubmitted.Count + 1, GetTicketPriorities()[1], GetTicketDepartments()[1],
				GetTicketStatus()[1], ticketId, subject, DateTime.UtcNow, DateTime.UtcNow, userName, email,
				userName, ticketMessages);

			TicketsSubmitted.Add(ticket);
			return true;
		}

		public bool TicketRespond(in HelpdeskTicket ticket, in string name, in string message)
		{
			ticket.LastReplier = name;
			ticket.Messages.Add(new (DateTime.UtcNow, name, message));
			return true;
		}

		public List<HelpdeskTicket> TicketsSubmitted { get; } = new();
	}
}
