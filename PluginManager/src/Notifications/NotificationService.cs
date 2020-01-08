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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager
 *  
 *  File: NotificationService.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  12/05/2019  Simon Carter        Initially Created
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager (AspNetCore.PluginManager)
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using PluginManager.Abstractions;

using Shared.Classes;

namespace PluginManager.Internal
{
    internal sealed class NotificationService : ThreadManager, INotificationService
    {
        #region Private Members

        private const int MaxQueueItems = 300;
        private static readonly object _lockObject = new object();
        private static readonly object _queueLock = new object();
        private readonly Dictionary<string, List<INotificationListener>> _eventListener;
        private readonly Queue<NotificationQueueItem> _messageQueue;

        #endregion Private Members

        #region Constructors

        public NotificationService()
            : base(null, new TimeSpan(0, 0, 1), null, 1000, 200, false)
        {
            _eventListener = new Dictionary<string, List<INotificationListener>>();
            _messageQueue = new Queue<NotificationQueueItem>();
        }

        #endregion Constructors

        #region Public INotificationService Methods

        public bool RaiseEvent(in string eventId, in object param1, in object param2, ref object result)
        {
            // if no one is listening, indicate not sent
            if (!_eventListener.ContainsKey(eventId))
                return false;

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                foreach (INotificationListener listener in _eventListener[eventId])
                {
                    if (!listener.EventRaised(eventId, param1, param2, ref result))
                        break;
                }

                // if there is at least one listener, then the event has been processed
                return _eventListener[eventId].Count > 0;
            }
        }

        public void RaiseEvent(in string eventId, in object param1, in object param2)
        {
            using (TimedLock timedLock = TimedLock.Lock(_queueLock))
            {
                _messageQueue.Enqueue(new NotificationQueueItem(eventId, param1, param2));
            }
        }

        public void RaiseEvent(in string eventId, in object param1)
        {
            RaiseEvent(eventId, param1, null);
        }

        public void RaiseEvent(in string eventId)
        {
            RaiseEvent(eventId, null, null);
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

        #endregion Public INotificationService Methods

        #region ThreadManager Methods

        protected override bool Run(object parameters)
        {
            List<NotificationQueueItem> queue = new List<NotificationQueueItem>();

            using (TimedLock timedLock = TimedLock.Lock(_queueLock))
            {
                int counter = 0;
                NotificationQueueItem queueItem = null;

                do
                {
                    if (_messageQueue.Count > 0)
                    {
                        queueItem = _messageQueue.Dequeue();
                        queue.Add(queueItem);
                    }
                    else
                    {
                        queueItem = null;
                    }

                    counter++;

                } while (counter < MaxQueueItems && queueItem != null);
            }

            // queue has been cleared (upto max items) send the messages
            foreach (NotificationQueueItem item in queue)
            {
                // if no one is listening, ignore it and move on
                if (!_eventListener.ContainsKey(item.EventId))
                    continue;

                foreach (INotificationListener listener in _eventListener[item.EventId])
                {
                    listener.EventRaised(item.EventId, item.Param1, item.Param2);
                }
            }

            return !HasCancelled();
        }

        #endregion ThreadManager Methods
    }
}
