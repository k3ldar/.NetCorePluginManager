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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: LoggerQueueItem.cs
 *
 *  Purpose:  ILogger queue item
 *
 *  Date        Name                Reason
 *  30/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using static SharedPluginFeatures.Enums;

namespace AspNetCore.PluginManager.Classes
{
    internal class LoggerQueueItem
    {
        #region Constructors

        internal LoggerQueueItem(in LogLevel logLevel, in string message)
        {
            Date = DateTime.Now;
            Level = logLevel;
            Message = message;
        }

        #endregion Constructors

        #region Properties

        internal DateTime Date { get; private set; }

        internal LogLevel Level { get; private set; }

        internal string Message { get; private set; }

        #endregion Properties
    }
}
