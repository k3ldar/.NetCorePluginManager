/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  Plugin Manager is distributed under the GNU General Public License version 3 and  
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
 *  Product:  PluginManager
 *  
 *  File: LoggerQueueItem.cs
 *
 *  Purpose:  ILogger queue item
 *
 *  Date        Name                Reason
 *  30/01/2019  Simon Carter        Initially Created
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager (AspNetCore.PluginManager)
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace PluginManager
{
    /// <summary>
    /// Log entry item that is held in a queue.
    /// 
    /// The Plugin Manager keeps the last n log entries in a list, these can be retrieved for viewing.
    /// </summary>
    public sealed class LoggerQueueItem
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logLevel">Current log level</param>
        /// <param name="message">Log message</param>
        public LoggerQueueItem(in LogLevel logLevel, in string message)
        {
            if (String.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            Date = DateTime.Now;
            Level = logLevel;
            Message = message;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Date and time the log entry was made
        /// </summary>
        /// <value>DateTime</value>
        public DateTime Date { get; }

        /// <summary>
        /// Log level, the severity or log type for the entry.
        /// </summary>
        /// <value>LogLevel</value>
        public LogLevel Level { get; }

        /// <summary>
        /// The log entry message
        /// </summary>
        /// <value>string</value>
        public string Message { get; }

        #endregion Properties
    }
}
