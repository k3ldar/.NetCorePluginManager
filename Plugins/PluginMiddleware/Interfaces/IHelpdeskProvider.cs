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
 *  File: IHelpdeskProvider.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  13/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

namespace Middleware.Helpdesk
{
    public interface IHelpdeskProvider
    {
        #region Feedback

        List<Feedback> GetFeedback(in bool publiclyVisible);

        bool SubmitFeedback(in long userId, in string name, in string feedback);

        #endregion Feedback

        #region Tickets

        List<LookupListItem> GetTicketPriorities();

        List<LookupListItem> GetTicketDepartments();

        List<LookupListItem> GetTicketStatus();

        bool SubmitTicket(long userId, in int department, in int priority,
            in string userName, in string email, in string subject, in string message,
            out HelpdeskTicket ticket);

        HelpdeskTicket GetTicket(in long id);

        HelpdeskTicket GetTicket(in string email, in string ticketKey);

        bool TicketRespond(in HelpdeskTicket ticket, in string name, in string message);

        #endregion Tickets

        #region Tickets FaQ

        List<KnowledgeBaseGroup> GetKnowledgebaseGroups(in long userId, in KnowledgeBaseGroup parent);

        KnowledgeBaseGroup GetKnowledgebaseGroup(in long userId, in int id);

        bool GetKnowledgebaseItem(in long userId, in int id, 
            out KnowledgeBaseItem knowledgebaseItem, out KnowledgeBaseGroup parentGroup);

        void KnowledbaseView(in KnowledgeBaseItem group);

        #endregion Tickets FaQ
    }
}
