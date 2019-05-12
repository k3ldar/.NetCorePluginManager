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
 *  File: NotificationService.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  12/05/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Shared.Classes;
using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Notifications
{
    public sealed class NotificationService : INotificationService
    {
        #region Private Members

        private static object _lockObject = new object();
        private readonly Dictionary<string, List<INotificationListener>> _eventListener;

        #endregion Private Members

        #region Constructors

        public NotificationService()
        {
            _eventListener = new Dictionary<string, List<INotificationListener>>();
        }

        #endregion Constructors

        #region INotificationService Methods

        public void RaiseEvent(in string eventId, in object param1, in object param2)
        {
            if (!_eventListener.ContainsKey(eventId))
                throw new InvalidOperationException(Constants.EventNameNotRegistered);

            foreach (INotificationListener listener in _eventListener[eventId])
            {
                if (!listener.EventRaised(eventId, param1, param2))
                    return;
            }
        }

        public bool RegisterListener(in INotificationListener listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener));

            List<string> events = listener.GetEvents();

            if (events == null || events.Count < 1)
                throw new InvalidOperationException(Constants.InvalidListener);

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                foreach (string eventName in events)
                {
                    if (String.IsNullOrEmpty(eventName))
                        throw new InvalidOperationException(Constants.InvalidEventName);

                    if (!_eventListener.ContainsKey(eventName))
                        _eventListener.Add(eventName, new List<INotificationListener>());

                    if (!_eventListener[eventName].Contains(listener))
                        _eventListener[eventName].Add(listener);
                }
            }

            return true;
        }

        public bool UnregisterListener(in INotificationListener listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener));

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                List<string> events = listener.GetEvents();

                if (events == null || events.Count < 1)
                    throw new InvalidOperationException(Constants.InvalidListener);

                foreach (KeyValuePair<string, List<INotificationListener>> eventHolder in _eventListener)
                {
                    if (eventHolder.Value.Contains(listener))
                        eventHolder.Value.Remove(listener);
                }
            }

            return true;
        }

        #endregion INotificationService Methods
    }
}
