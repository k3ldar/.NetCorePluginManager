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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.Tests
 *  
 *  File: TestLogger.cs
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

namespace PluginManager.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public sealed class TestLogger : ILogger
    {
        #region Private Methods

        private string _data;
        private LogLevel _logLevel;
        private Exception _exception;
        private readonly List<string> _messages = new List<string>();
        private readonly List<TestLoggerItem> _errors;
        private readonly List<TestLoggerItem> _logs;

        #endregion Private Methods

        #region Constructors

        public TestLogger()
        {
            _errors = new List<TestLoggerItem>();
            _logs = new List<TestLoggerItem>();
        }

        #endregion Constructors

        #region Properties

        public List<TestLoggerItem> Errors
        {
            get
            {
                return _errors;
            }
        }

        public List<TestLoggerItem> Logs
        {
            get
            {
                return _logs;
            }
        }

        #endregion Properties

        #region Public Methods


        public bool ExceptionLogged(Type exception)
        {
            return exception.Equals(_exception.GetType());
        }

        public bool ContainsMessage(string message)
        {
            return _messages.Contains(message);
        }

        #endregion Public Methods

        #region ILogger Methods

        public void AddToLog(in LogLevel logLevel, in string data)
        {
            _messages.Add($"{logLevel} {data}");
            AddToLog(logLevel, String.Empty, data);
        }

        public void AddToLog(in LogLevel logLevel, in Exception exception)
        {
            _messages.Add($"{logLevel} {exception.Message}");
            AddToLog(logLevel, String.Empty, exception, String.Empty);
        }

        public void AddToLog(in LogLevel logLevel, in Exception exception, string data)
        {
            _messages.Add($"{logLevel} {exception.Message} {data}");

            _logLevel = logLevel;
            _exception = exception;
            _data = data;
            AddToLog(logLevel, String.Empty, exception, data);
        }

        public void AddToLog(in LogLevel logLevel, in string module, in string data)
        {
            _messages.Add($"{logLevel} {module} {data}");
            _logs.Add(new TestLoggerItem(logLevel, module, data));
        }

        public void AddToLog(in LogLevel logLevel, in string module, in Exception exception)
        {
            _messages.Add($"{logLevel} {module} {exception.Message}");
            AddToLog(logLevel, module, exception, String.Empty);
        }

        public void AddToLog(in LogLevel logLevel, in string module, in Exception exception, string data)
        {
            _messages.Add($"{logLevel} {module} {exception.Message} {data}");
            _errors.Add(new TestLoggerItem(logLevel, module, exception, data));
        }

        #endregion ILogger Methods
    }
}
