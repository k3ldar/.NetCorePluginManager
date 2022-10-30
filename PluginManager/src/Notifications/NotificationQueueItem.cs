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
 *  File: NotificationQueueItem.cs
 *
 *  Purpose:  Item used for queueing notifications
 *
 *  Date        Name                Reason
 *  15/05/2019  Simon Carter        Initially Created
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager (AspNetCore.PluginManager)
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace PluginManager.Internal
{
    internal sealed class NotificationQueueItem
    {
        #region Constructors

        internal NotificationQueueItem(in string eventId, in object param1, in object param2)
        {
            if (String.IsNullOrEmpty(eventId))
                throw new ArgumentNullException(nameof(eventId));

            EventId = eventId;
            Param1 = param1;
            Param2 = param2;
        }

        #endregion Constructors

        #region Properties

        internal string EventId { get; }

        internal object Param1 { get; }

        internal object Param2 { get; }

        #endregion Properties
    }
}
