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
 *  Product:  Company.Plugin
 *  
 *  File: Logger.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  08/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using PluginManager;
using PluginManager.Abstractions;

#pragma warning disable CS1591

namespace Company.Plugin.Classes
{
    public class Logger : ILogger
    {
        #region ILogger Methods

        public void AddToLog(in LogLevel logLevel, in string data)
        {
            AddToLog(logLevel, String.Empty, data);
        }

        public void AddToLog(in LogLevel logLevel, in Exception exception)
        {
            AddToLog(logLevel, String.Empty, exception);
        }

        public void AddToLog(in LogLevel logLevel, in Exception exception, string data)
        {
            AddToLog(logLevel, String.Empty, exception, data);
        }

        public void AddToLog(in LogLevel logLevel, in string module, in string data)
        {
#if TRACE
            System.Diagnostics.Trace.WriteLine($"{logLevel.ToString()} {data}");
#endif   
        }

        public void AddToLog(in LogLevel logLevel, in string module, in Exception exception)
        {
#if TRACE
            System.Diagnostics.Trace.WriteLine($"{logLevel.ToString()} {exception?.Message}");
#endif
        }

        public void AddToLog(in LogLevel logLevel, in string module, in Exception exception, string data)
        {
#if TRACE
            System.Diagnostics.Trace.WriteLine($"{logLevel.ToString()} {exception?.Message}\r\n{data}");
#endif
        }

        #endregion ILogger Methods
    }
}

#pragma warning restore CS1591