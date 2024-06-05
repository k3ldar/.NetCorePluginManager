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
 *  Product:  Helpdesk Plugin
 *  
 *  File: ImportEmailIntoHelpdesk.cs
 *
 *  Purpose:  Thread used to import emails into Helpdesk tickets
 *
 *  Date        Name                Reason
 *  15/12/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Middleware;
using Middleware.Helpdesk;

using PluginManager.Abstractions;

using Shared.Classes;
using Shared.Communication;

using SharedPluginFeatures;

#if NET6_0_OR_GREATER

namespace HelpdeskPlugin.Classes
{

	/// <summary>
	/// Thread which imports emails into helpdesk system
	/// </summary>
	public sealed class ImportEmailIntoHelpdeskThread : ThreadManager
	{
		private readonly object _lock = new();
		private readonly IHelpdeskProvider _helpdeskProvider;
		private readonly IPop3ClientFactory _pop3ClientFactory;
		private readonly ILogger _logger;
		private readonly IUserSearch _userSearch;
		private readonly int _priorityId;
		private readonly int _departmentId;
		private readonly bool _registeredEmailsOnly;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="helpdeskProvider">Valid IHelpdeskProvider instance</param>
		/// <param name="pop3ClientFactory">Valid IPop3Client instance</param>
		/// <param name="userSearch">Valid IUserSearch instacnce</param>
		/// <param name="settingsProvider">Valid ISettingsProvider instance</param>
		/// <param name="logger">Valid logger instance</param>
		public ImportEmailIntoHelpdeskThread(IHelpdeskProvider helpdeskProvider, IPop3ClientFactory pop3ClientFactory, 
			IUserSearch userSearch, ISettingsProvider settingsProvider, ILogger logger)
			: this(helpdeskProvider, pop3ClientFactory, userSearch, settingsProvider, logger, TimeSpan.FromMinutes(1))
		{

		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="helpdeskProvider">Valid IHelpdeskProvider instance</param>
		/// <param name="pop3ClientFactory">Valid IPop3Client instance</param>
		/// <param name="userSearch">Valid IUserSearch instance</param>
		/// <param name="settingsProvider">Valid ISettingsProvider instance</param>
		/// <param name="logger">Valid logger instance</param>
		/// <param name="timeSpan">Timspan depicting the interval between checks</param>
		public ImportEmailIntoHelpdeskThread(IHelpdeskProvider helpdeskProvider, IPop3ClientFactory pop3ClientFactory, 
			IUserSearch userSearch, ISettingsProvider settingsProvider, ILogger logger, TimeSpan timeSpan)
			: base(null, timeSpan)
		{
			_helpdeskProvider = helpdeskProvider ?? throw new ArgumentNullException(nameof(helpdeskProvider));
			_pop3ClientFactory = pop3ClientFactory ?? throw new ArgumentNullException(nameof(pop3ClientFactory));
			_userSearch = userSearch ?? throw new ArgumentNullException(nameof(userSearch));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));

			_priorityId = _helpdeskProvider.GetTicketPriorities().Find(s => s.Description.Equals("medium", StringComparison.InvariantCultureIgnoreCase))?.Id ?? 0;
			_departmentId = _helpdeskProvider.GetTicketDepartments().Find(s => s.Description.Equals("support", StringComparison.InvariantCultureIgnoreCase))?.Id ?? 0;

			HelpdeskSettings settings = settingsProvider.GetSettings<HelpdeskSettings>(nameof(HelpdeskSettings));
			_registeredEmailsOnly = !settings.AnyUserEmailCanSubmitTickets;
		}

		/// <summary>
		/// Thread used to check for emails and import into helpdesk
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		protected override bool Run(object parameters)
		{
			ProcessIncomingEmails();

			return !base.HasCancelled();
		}

		/// <summary>
		/// Processes incoming emails
		/// </summary>
		public void ProcessIncomingEmails()
		{
			List<ReceivedEmail> messages = new();

			RetrieveEmailsFromPop3Server(messages);

			foreach (ReceivedEmail message in messages)
			{
				if (!message.IsValid)
				{
					_logger.AddToLog(PluginManager.LogLevel.Warning, "Invalid email received from Pop3 Server");
					continue;
				}

				string fromName = message.From[..(message.From.IndexOf("<") - 1)].Trim();
				string fromEmail = message.From.Trim()[(message.From.IndexOf("<") + 1)..].Replace(">", "");
				string messageId = $"P3{message.MessageId[1..^1].GetHashCode()}";


				HelpdeskTicket existingTicket = _helpdeskProvider.GetTicket(fromEmail, messageId);

				if (existingTicket == null)
				{
					Middleware.Users.SearchUser user = _userSearch.GetUsers(1, int.MaxValue, String.Empty, String.Empty)
						.Find(u => u.Email.Equals(fromEmail, StringComparison.InvariantCultureIgnoreCase));

					if (user == null && _registeredEmailsOnly)
					{
						_logger.AddToLog(PluginManager.LogLevel.Information, $"Email received from {fromEmail} but discarded as not a registered user");
						continue;
					}

					long userId = user == null ? 0 : user.Id;

					_helpdeskProvider.SubmitTicket(userId, _departmentId, _priorityId, fromName, fromEmail, message.Subject, 
						message.TextMessage ?? message.HtmlMessage, 
						messageId, out HelpdeskTicket _);
				}
				else
				{
					_helpdeskProvider.TicketRespond(existingTicket, fromName, message.TextMessage ?? message.HtmlMessage);
				}
			}
		}

		private void RetrieveEmailsFromPop3Server(List<ReceivedEmail> messages)
		{
			using (TimedLock timedLock = TimedLock.Lock(_lock))
			{
				string pop3Server = Environment.GetEnvironmentVariable("EmailPop3ServerName");
				string userName = Environment.GetEnvironmentVariable("EmailUserName");
				string password = Environment.GetEnvironmentVariable("EmailUserPassword");
				string port = Environment.GetEnvironmentVariable("EmailPop3Port");

				if (String.IsNullOrEmpty(pop3Server) || String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(port))
				{
					_logger.AddToLog(PluginManager.LogLevel.Error, "Environment variables not setup for email");
					pop3Server = String.Empty;
					userName = String.Empty;
					password = String.Empty;
					port = String.Empty;
				}

				using IPop3Client popClient = _pop3ClientFactory.Create();

				popClient.Initialize(pop3Server, userName, password, ushort.Parse(port));
				int mailCount = popClient.GetMailCount(out int sizeInOctets);

				for (int i = 0; i < mailCount; i++)
				{
					try
					{
						string message = popClient.RetrieveMessage(i + 1, out string initialResponse);
						messages.Add(new ReceivedEmail(new StringBuilder(message)));
						_ = popClient.DeleteMessage(i + 1);
					}
					catch (Exception ex)
					{
						_logger.AddToLog(PluginManager.LogLevel.Error, ex);
					}
				}
			}
		}
	}
}

#endif
