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
 *  File: TestLoggerItem.cs
 *
 *  Purpose:  Mock ILogger implementation
 *
 *  Date        Name                Reason
 *  15/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

namespace PluginManager.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public sealed class TestLoggerItem
    {
        #region Constructors

        public TestLoggerItem(in LogLevel logLevel, in string module, in string data)
        {
            IsException = false;
            LogLevel = logLevel;
            Module = module ?? String.Empty;
            Data = data ?? String.Empty;
        }

        public TestLoggerItem(in LogLevel logLevel, in string module, Exception error, in string data)
            : this(logLevel, module, data)
        {
            IsException = true;
            Error = error;
        }

        #endregion Constructors

        #region Properties

        public bool IsException { get; private set; }

        public LogLevel LogLevel { get; private set; }

        public string Module { get; private set; }

        public string Data { get; private set; }

        public Exception Error { get; private set; }

        #endregion Properties
    }
}
