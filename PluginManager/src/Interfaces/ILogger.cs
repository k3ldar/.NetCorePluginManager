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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager
 *  
 *  File: ILogger.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *  14/10/2018  Simon Carter        Moved to SharedPluginFeatures
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace PluginManager.Abstractions
{
    /// <summary>
    /// Generic interface provided where plugin modules and all other parts of the system can add data to a log file.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Adds data to the log file.
        /// </summary>
        /// <param name="logLevel">LogLevel enum indicating the type of log entry</param>
        /// <param name="data">Data to be logged.</param>
        void AddToLog(in LogLevel logLevel, in string data);

        /// <summary>
        /// Logs an exception with the log file and also creates an additional exception log entry detailing the exception, call stack etc.
        /// </summary>
        /// <param name="logLevel">LogLevel enum indicating the type of log entry</param>
        /// <param name="exception">Exception that was raised.</param>
        void AddToLog(in LogLevel logLevel, in Exception exception);

        /// <summary>
        /// Logs an exception with the log file and also creates an additional exception log entry detailing the exception, call stack etc.
        /// </summary>
        /// <param name="logLevel">LogLevel enum indicating the type of log entry</param>
        /// <param name="exception"></param>
        /// <param name="data">Additional data to be logged with the exception.</param>
        void AddToLog(in LogLevel logLevel, in Exception exception, string data);
        /// <summary>
        /// Adds data to the log file.
        /// </summary>
        /// <param name="logLevel">LogLevel enum indicating the type of log entry</param>
        /// <param name="moduleName">Name of module creating the log entry</param>
        /// <param name="data">Data to be logged.</param>
        void AddToLog(in LogLevel logLevel, in string moduleName, in string data);

        /// <summary>
        /// Logs an exception with the log file and also creates an additional exception log entry detailing the exception, call stack etc.
        /// </summary>
        /// <param name="logLevel">LogLevel enum indicating the type of log entry</param>
        /// <param name="moduleName">Name of module creating the log entry</param>
        /// <param name="exception">Exception that was raised.</param>
        void AddToLog(in LogLevel logLevel, in string moduleName, in Exception exception);

        /// <summary>
        /// Logs an exception with the log file and also creates an additional exception log entry detailing the exception, call stack etc.
        /// </summary>
        /// <param name="logLevel">LogLevel enum indicating the type of log entry</param>
        /// <param name="moduleName">Name of module creating the log entry</param>
        /// <param name="exception"></param>
        /// <param name="data">Additional data to be logged with the exception.</param>
        void AddToLog(in LogLevel logLevel, in string moduleName, in Exception exception, string data);
    }
}
