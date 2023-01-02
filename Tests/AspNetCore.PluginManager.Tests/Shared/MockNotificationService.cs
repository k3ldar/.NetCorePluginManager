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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockNotificationService.cs
 *
 *  Purpose:  Mock INotificationService class
 *
 *  Date        Name                Reason
 *  12/10/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using PluginManager.Abstractions;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class MockNotificationService : INotificationService
    {
        private List<string> _raisedEvents = new List<string>();
        private object _result = null;
        public readonly List<INotificationListener> RegisteredListeners = new List<INotificationListener>();

        public MockNotificationService()
        {

        }

        public MockNotificationService(object result)
        {
            _result = result;
        }

        public int NotificationRaised(string eventId)
        {
            return _raisedEvents.Where(e => e.Equals(eventId)).Count();
        }

        public bool NotificationRaised(string eventId, object eventParam1)
        {
            if (_raisedEvents.Contains(eventId))
                return eventParam1.Equals(EventParam1);

            return false;
        }

        public Boolean RaiseEvent(in String eventId, in Object param1, in Object param2, ref Object result)
        {
            _raisedEvents.Add(eventId);
            result = _result;

            if (eventId.Equals(EventParam1Name) && EventParam1 == null)
            {
                EventParam1 = param1;
            }

            return result != null;
        }

        public void RaiseEvent(in String eventId, in Object param1, in Object param2)
        {
            _raisedEvents.Add(eventId);
        }

        public void RaiseEvent(in String eventId, in Object param1)
        {
            _raisedEvents.Add(eventId);
        }

        public void RaiseEvent(in String eventId)
        {
            _raisedEvents.Add(eventId);
        }

        public Boolean RegisterListener(in INotificationListener listener)
        {
            RegisteredListeners.Add(listener);

            return RegisteredListeners.Contains(listener);
        }

        public Boolean UnregisterListener(in INotificationListener listener)
        {
            RegisteredListeners.Remove(listener);

            return !RegisteredListeners.Contains(listener);
        }

        public object EventParam1 { get; private set; }

        public string EventParam1Name { get; set; }
    }
}
