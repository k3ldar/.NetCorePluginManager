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
 *  Product:  PluginManager.Tests
 *  
 *  File: MockLogger.cs
 *
 *  Purpose:  Mock ILogger implementation
 *
 *  Date        Name                Reason
 *  15/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using PluginManager.Abstractions;

using Shared.Classes;

namespace PluginManager.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public sealed class MockLogger : ILogger
    {
        #region Private Methods

        private readonly object _lockObject = new object();
        private string _data;
        private LogLevel _logLevel;
        private Exception _exception;
        private readonly List<string> _messages = new List<string>();
        private readonly List<MockLoggerItem> _errors;
        private readonly List<MockLoggerItem> _logs;

        #endregion Private Methods

        #region Constructors

        public MockLogger()
        {
            _errors = new List<MockLoggerItem>();
            _logs = new List<MockLoggerItem>();
        }

        #endregion Constructors

        #region Properties

        public List<MockLoggerItem> Errors
        {
            get
            {
                using (TimedLock timedLock = TimedLock.Lock(_lockObject))
                {
                    return _errors;
                }
            }
        }

        public List<MockLoggerItem> Logs
        {
            get
            {
                using (TimedLock timedLock = TimedLock.Lock(_lockObject))
                {
                    return _logs;
                }
            }
        }

        #endregion Properties

        #region Public Methods


        public bool ExceptionLogged(Type exception)
        {
            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                return exception.Equals(_exception.GetType());
            }
        }

        public bool ContainsMessage(string message)
        {
            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                if (_messages.Contains(message))
                    return true;

                foreach (string msg in _messages)
                {
                    if (msg.StartsWith(message))
                        return true;
                }

                return false;
            }
        }

        #endregion Public Methods

        #region ILogger Methods

        public void AddToLog(in LogLevel logLevel, in string data)
        {
            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                _messages.Add($"{logLevel} {data}");
                AddToLog(logLevel, String.Empty, data);
            }
        }

        public void AddToLog(in LogLevel logLevel, in Exception exception)
        {
            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                _messages.Add($"{logLevel} {exception.Message}");
                AddToLog(logLevel, String.Empty, exception, String.Empty);
            }
        }

        public void AddToLog(in LogLevel logLevel, in Exception exception, string data)
        {
            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                _messages.Add($"{logLevel} {exception.Message} {data}");

                _logLevel = logLevel;
                _exception = exception;
                _data = data;
                AddToLog(logLevel, String.Empty, exception, data);
            }
        }

        public void AddToLog(in LogLevel logLevel, in string module, in string data)
        {
            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                _messages.Add($"{logLevel} {module} {data}");
                _logs.Add(new MockLoggerItem(logLevel, module, data));
            }
        }

        public void AddToLog(in LogLevel logLevel, in string module, in Exception exception)
        {
            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                _messages.Add($"{logLevel} {module} {exception.Message}");
                AddToLog(logLevel, module, exception, String.Empty);
            }
        }

        public void AddToLog(in LogLevel logLevel, in string module, in Exception exception, string data)
        {
            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                _messages.Add($"{logLevel} {module} {exception.Message} {data}");
                _errors.Add(new MockLoggerItem(logLevel, module, exception, data));
            }
        }

        #endregion ILogger Methods
    }
}
