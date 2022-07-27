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
 *  Copyright (c) 2019 - 2021 Simon Carter.  All Rights Reserved.
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
using System.Collections.Generic;

namespace Middleware.Helpdesk
{
    /// <summary>
    /// Helpdesk provider for integrating with the HelpdeskPlugin module.
    /// 
    /// This item must be implemented by the host application and made available via DI.
    /// </summary>
    public interface IHelpdeskProvider
    {
        #region Feedback

        /// <summary>
        /// Retrieve all feedback to be displayed on the website.
        /// </summary>
        /// <param name="publiclyVisible">bool.  Determine whether all feedback should be retrieved, or only publicly visible feedback.</param>
        /// <returns>List&lt;Feedback&gt;</returns>
        List<Feedback> GetFeedback(in bool publiclyVisible);

        /// <summary>
        /// Request to submit feedback for a website.
        /// </summary>
        /// <param name="userId">Id of user leaving feedback, if they are logged in.</param>
        /// <param name="name">Name of user leaving feedback for the website.</param>
        /// <param name="feedback">Feedback being left for the website.</param>
        /// <returns>bool.  True if the feedback was successfully submitted.</returns>
        bool SubmitFeedback(in long userId, in string name, in string feedback);

        #endregion Feedback

        #region Tickets

        /// <summary>
        /// Retrieve a list of ticket priorities to be displayed to the user submitting a ticket.
        /// </summary>
        /// <returns>List&lt;LookupListItem&gt;</returns>
        List<LookupListItem> GetTicketPriorities();

        /// <summary>
        /// Retrieve a list of all ticket departments to be display to the user submitting a ticket.
        /// </summary>
        /// <returns>List&lt;LookupListItem&gt;</returns>
        List<LookupListItem> GetTicketDepartments();

        /// <summary>
        /// Retrieve a list of all available ticket statuses.
        /// </summary>
        /// <returns>List&lt;LookupListItem&gt;</returns>
        List<LookupListItem> GetTicketStatus();

        /// <summary>
        /// Submits a support ticket to the website.
        /// </summary>
        /// <param name="userId">Unique id of user submitting a ticket, if they are logged on.</param>
        /// <param name="department">Department the ticket is being submitted to.</param>
        /// <param name="priority">User defined priority of the ticket.</param>
        /// <param name="userName">Name of the user submitting the ticket.</param>
        /// <param name="email">Email address of the user submitting the ticket.</param>
        /// <param name="subject">Subject of the support ticket.</param>
        /// <param name="message">Support ticket message.</param>
        /// <param name="ticket">out.  Helpdeskticket of newly created ticket.</param>
        /// <returns>bool.  True if the ticket was successfully submitted.</returns>
        bool SubmitTicket(in long userId, in int department, in int priority,
            in string userName, in string email, in string subject, in string message,
            out HelpdeskTicket ticket);

        /// <summary>
        /// Retrieve a ticket using the unique ticket Id.
        /// </summary>
        /// <param name="id">Unique ticket id being requested.</param>
        /// <returns>HelpdeskTicket</returns>
        HelpdeskTicket GetTicket(in long id);

        /// <summary>
        /// Retreive a ticket using the users email address and the unique ticket key.
        /// </summary>
        /// <param name="email">Email address of the user who submitted the ticket.</param>
        /// <param name="ticketKey">Unique ticket key supplied when the ticket was created.</param>
        /// <returns>Hlepdeskticket</returns>
        HelpdeskTicket GetTicket(in string email, in string ticketKey);

        /// <summary>
        /// Submits a response to a Helpdesk support ticket.
        /// </summary>
        /// <param name="ticket">HelpdeskTicket being responded to.</param>
        /// <param name="name">Name of person responding.</param>
        /// <param name="message">Message response.</param>
        /// <returns>bool.  True if the response was successfully submitted.</returns>
        bool TicketRespond(in HelpdeskTicket ticket, in string name, in string message);

        #endregion Tickets

        #region Tickets FaQ

        /// <summary>
        /// Retrieves a list of all KnowledgeBaseGroup items.
        /// </summary>
        /// <param name="userId">Id of the user viewing the groups, if they are logged in.</param>
        /// <param name="parent">Group parent if there is one, otherwise null for groups at the top of the heirerarchy.</param>
        /// <returns>List&lt;KnowledgeBaseGroup&gt;</returns>
        List<KnowledgeBaseGroup> GetKnowledgebaseGroups(in long userId, in KnowledgeBaseGroup parent);

        /// <summary>
        /// Retrieves an individual KnowledgeBaseGroup item.
        /// </summary>
        /// <param name="userId">Unique id of the user viewing the group, if they are logged in.</param>
        /// <param name="id">Id of the KnowledgeBaseGroup being retrieved.</param>
        /// <returns>KnowledgeBaseGroup</returns>
        KnowledgeBaseGroup GetKnowledgebaseGroup(in long userId, in long id);

        /// <summary>
        /// Retrieves an individual KnowledgeBaseItem.
        /// </summary>
        /// <param name="userId">Id of the user viewing the KnowledgeBaseItem, if they are logged on.</param>
        /// <param name="id">Id of the KnowledgeBaseItem.</param>
        /// <param name="knowledgebaseItem">out.  KnowledgeBaseItem being retrieved.</param>
        /// <param name="parentGroup">KnowledgeBaseGroup item which is the parent group of the item being retrieved.</param>
        /// <returns>bool.  True if KnowledgeBaseItem is being returned.</returns>
        bool GetKnowledgebaseItem(in long userId, in long id,
            out KnowledgeBaseItem knowledgebaseItem, out KnowledgeBaseGroup parentGroup);

        /// <summary>
        /// Indicates that a KnowledgeBaseItem has been viewed.
        /// </summary>
        /// <param name="item">KnowledgeBaseItem being viewed.</param>
        void KnowledgebaseView(in KnowledgeBaseItem item);

        #endregion Tickets FaQ
    }
}
