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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: TestLogger.cs
 *
 *  Purpose:  Mock ILogger class
 *
 *  Date        Name                Reason
 *  12/10/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using PluginManager;
using PluginManager.Abstractions;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class TestLogger : ILogger
    {
        private string _data;
        private LogLevel _logLevel;
        private Exception _exception;
        private List<string> _messages = new List<string>();

        public void AddToLog(in LogLevel logLevel, in String data)
        {
            _messages.Add($"{logLevel} {data}");
        }

        public void AddToLog(in LogLevel logLevel, in Exception exception)
        {
            _messages.Add($"{logLevel} {exception.Message}");
        }

        public void AddToLog(in LogLevel logLevel, in Exception exception, String data)
        {
            _messages.Add($"{logLevel} {exception.Message} {data}");

            _logLevel = logLevel;
            _exception = exception;
            _data = data;
        }

        public void AddToLog(in LogLevel logLevel, in String moduleName, in String data)
        {
            _messages.Add($"{logLevel} {moduleName} {data}");
        }

        public void AddToLog(in LogLevel logLevel, in String moduleName, in Exception exception)
        {
            _messages.Add($"{logLevel} {moduleName} {exception.Message}");
        }

        public void AddToLog(in LogLevel logLevel, in String moduleName, in Exception exception, String data)
        {
            _messages.Add($"{logLevel} {moduleName} {exception.Message} {data}");
        }

        public bool ExceptionLogged(Type exception)
        {
            return exception.Equals(_exception.GetType());
        }

        public bool ContainsMessage(string message)
        {
            return _messages.Contains(message);
        }
    }
}
