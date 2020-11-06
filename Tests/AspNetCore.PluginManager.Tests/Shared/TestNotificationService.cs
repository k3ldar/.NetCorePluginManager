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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: TestNotificationService.cs
 *
 *  Purpose:  Mock INotificationService class
 *
 *  Date        Name                Reason
 *  12/10/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;

using PluginManager.Abstractions;

namespace AspNetCore.PluginManager.Tests.Shared
{
    public class TestNotificationService : INotificationService
    {
        private List<string> _raisedEvents = new List<string>();
        private object _result = null;

        public TestNotificationService()
        {

        }

        public TestNotificationService(object result)
        {
            _result = result;
        }

        public int NotificationRaised(string eventId)
        {
            return _raisedEvents.Where(e => e.Equals(eventId)).Count();
        }

        public Boolean RaiseEvent(in String eventId, in Object param1, in Object param2, ref Object result)
        {
            _raisedEvents.Add(eventId);
            result = _result;
            return result != null;
        }

        public void RaiseEvent(in String eventId, in Object param1, in Object param2)
        {

        }

        public void RaiseEvent(in String eventId, in Object param1)
        {

        }

        public void RaiseEvent(in String eventId)
        {

        }

        public Boolean RegisterListener(in INotificationListener listener)
        {
            throw new NotImplementedException();
        }

        public Boolean UnregisterListener(in INotificationListener listener)
        {
            throw new NotImplementedException();
        }
    }
}
