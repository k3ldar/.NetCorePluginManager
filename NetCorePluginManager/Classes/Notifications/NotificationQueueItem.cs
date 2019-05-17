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
 *  File: NotificationQueueItem.cs
 *
 *  Purpose:  Item used for queueing notifications
 *
 *  Date        Name                Reason
 *  15/05/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace AspNetCore.PluginManager
{
    internal sealed class NotificationQueueItem
    {
        #region Constructors

        public NotificationQueueItem(in string eventId, in object param1, in object param2)
        {
            if (String.IsNullOrEmpty(eventId))
                throw new ArgumentNullException(nameof(eventId));

            EventId = eventId;
            Param1 = param1;
            Param2 = param2;
        }

        #endregion Constructors

        #region Properties

        public string EventId { get; private set; }

        public object Param1 { get; private set; }

        public object Param2 { get; private set; }

        #endregion Properties
    }
}
