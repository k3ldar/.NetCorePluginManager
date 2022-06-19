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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: IEmailSender.cs
 *
 *  Purpose:  Interface for sending emails
 *
 *  Date        Name                Reason
 *  17/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppSettings;

using PluginManager.Abstractions;

using Shared.Classes;
using Shared.Communication;

namespace Middleware.Classes
{
    public class DefaultEmailSender : ThreadManager, IEmailSender
    {
        #region Private Members

        private const int MaxEmailsPerBatch = 30;

        private readonly ILogger _logger;
        private readonly string _host;
        private readonly string _password;
        private readonly int _port;
        private readonly bool _ssl;
        private readonly string _userName;
        private readonly object _emailQueueLock = new object();
        private readonly Queue<EmailToSend> _emailsToSend = new Queue<EmailToSend>();

        #endregion Private Members

        #region Private Classes / Records

#if NET5_0_OR_GREATER
        private record EmailToSend(string RecipientName, string RecipientEmail, string Message, string Subject, bool IsHtml, string[] Attachments);
#else
        private sealed class EmailToSend
        {
            public EmailToSend(string recipientName, string recipientEmail, string message, string subject, bool isHtml, string[] attachments)
            {
                RecipientName = recipientName;
                RecipientEmail = recipientEmail;
                Message = message;
                Subject = subject;
                IsHtml = isHtml;
                Attachments = attachments;
            }

        public string RecipientName { get; }
        public string RecipientEmail { get; }
        public string Message { get; }
        public string Subject { get; }
        public bool IsHtml { get; }
        public string[] Attachments { get; }
        }
#endif 

        #endregion Private Classes / Records

        #region Constructors

        public DefaultEmailSender(ISettingsProvider settingsProvider, ILogger logger)
            : base(null, new TimeSpan(0, 0, 1), null, 20000, 200, false, true)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            DefaultEmailSenderSettings settings = settingsProvider.GetSettings<DefaultEmailSenderSettings>(nameof(DefaultEmailSender));
            _host = settings.Host;
            _password = settings.Password;
            _userName = settings.UserName;

            if (Int32.TryParse(settings.Port, out int port))
                _port = port;

            _ssl = settings.SSL.Equals(Boolean.TrueString, StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion Constructors

        #region Properties

        public bool IsValid => !String.IsNullOrEmpty(_host) && !String.IsNullOrEmpty(_password) && !String.IsNullOrEmpty(_userName) && _port > 0;

        public int QueueLength
        {
            get
            {
                using (TimedLock timedLock = TimedLock.Lock(_emailQueueLock))
                    return _emailsToSend.Count;
            }
        }

        #endregion Properties

        #region IEmailSender Methods

        public void SendEmail(string recipientName, string recipientEmail, string message, string subject, bool isHtml, params string[] attachments)
        {
            using (TimedLock timedLock = TimedLock.Lock(_emailQueueLock))
            {
                _emailsToSend.Enqueue(new EmailToSend(recipientName, recipientEmail, message, subject, isHtml, attachments));
            }
        }

        #endregion IEmailSender Methods

        #region Overridden Methods

        protected override bool Run(object parameters)
        {
            if (!IsValid)
            {
                _logger.AddToLog(PluginManager.LogLevel.Error, "Unable to send emails as system is not configured");
                return true;
            }

            List<EmailToSend> emailToSends = new List<EmailToSend>();

            using (TimedLock timedLock = TimedLock.Lock(_emailQueueLock))
            {
                for (int i = 0; i < MaxEmailsPerBatch; i++)
                {
                    if (_emailsToSend.TryDequeue(out EmailToSend emailToSend))
                    {
                        emailToSends.Add(emailToSend);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            Email email = new Shared.Communication.Email(_userName, _host, _userName, _password, _port, _ssl);
            emailToSends.ForEach((Action<EmailToSend>)(e =>
            {
                if (email.SendEmail(_userName, e.RecipientName, e.RecipientEmail, e.Message, e.Subject, e.IsHtml, e.Attachments))
                {
                    _logger.AddToLog(PluginManager.LogLevel.Information, $"Email sent to {e.RecipientName} : {e.RecipientEmail}");
                }
                else
                {
                    _logger.AddToLog(PluginManager.LogLevel.Warning, $"Failed to send email to {e.RecipientName} : {e.RecipientEmail}");
                }
            }));

            return !CancelRequested;
        }

        #endregion Overridden Methods
    }
}