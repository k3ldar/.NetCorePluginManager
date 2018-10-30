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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.DemoWebsite
 *  
 *  File: Logger.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  25/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

using Shared.Classes;

using static SharedPluginFeatures.Enums;

namespace SystemAdmin.Plugin.Classes
{
    public class Logger : SystemAdminSubMenu, ILogger
    {
        #region Private Members

        private const int _maximumMemoryItems = 100;
        private static object _lockObject = new object();
        private static Queue<LogData> _logData = new Queue<LogData>();

        #endregion Private Members

        #region Constructors

        public Logger()
        {

        }

        #endregion Constructors

        #region ILogger Methods

        public void AddToLog(in LogLevel logLevel, in string data)
        {
            AddLogData(new LogData(logLevel, data));
#if TRACE
            System.Diagnostics.Trace.WriteLine($"{logLevel.ToString()} {data}");
#endif
        }

        public void AddToLog(in LogLevel logLevel, in Exception exception)
        {
            AddLogData(new LogData(logLevel, String.Empty, exception.Message));
#if TRACE
            System.Diagnostics.Trace.WriteLine($"{logLevel.ToString()} {exception.Message}");
#endif
        }

        public void AddToLog(in LogLevel logLevel, in Exception exception, string data)
        {
            AddLogData(new LogData(logLevel, data, exception.Message));
#if TRACE
            System.Diagnostics.Trace.WriteLine($"{logLevel.ToString()} {exception.Message}\r\n{data}");
#endif
        }

        #endregion ILogger Methods

        #region SystemAdminSubMenu Methods

        public override string Action()
        {
            return (String.Empty);
        }

        public override string Area()
        {
            return (String.Empty);
        }

        public override string Controller()
        {
            return (String.Empty);
        }

        public override string Data()
        {
            using (TimedLock.Lock(_lockObject))
            {
                string Result = "Date Time|Log Level|Message|Error";

                foreach (LogData logData in _logData)
                {
                    Result += $"\r{logData.LogLevel.ToString()}|{logData.Date.ToString("g")}|" +
                        $"{logData.Data}|{logData.Error}";
                }

                return (Result);
            }
        }

        public override string Image()
        {
            return (String.Empty);
        }

        public override SystemAdminMenuType MenuType()
        {
            return (SystemAdminMenuType.Grid);
        }

        public override string Name()
        {
            return ("Log Files");
        }

        public override string ParentMenuName()
        {
            return ("System");
        }

        public override int SortOrder()
        {
            return (0);
        }

        #endregion SystemAdminSubMenu Methods

        #region Private Methods

        private void AddLogData(in LogData logData)
        {
            using (TimedLock.Lock(_lockObject))
            {
                _logData.Enqueue(logData);

                if (_logData.Count > _maximumMemoryItems)
                    _logData.Dequeue();
            }
        }

        #endregion Private Methods
    }

    internal class LogData
    {
        #region Constructors

        internal LogData(in LogLevel logLevel, in string data)
        {
            Date = DateTime.Now;
            LogLevel = logLevel;
            Data = data;
            Error = String.Empty;
        }

        internal LogData(in LogLevel logLevel, in string data, in string error)
            : this (logLevel, data)
        {
            Error = error ?? String.Empty;
        }

        #endregion Constructors

        #region Internal Methods

        internal DateTime Date { get; private set; }

        internal LogLevel LogLevel { get; private set; }

        internal string Data { get; private set; }

        internal string Error { get; private set; }

        #endregion Internal Methods
    }
}
