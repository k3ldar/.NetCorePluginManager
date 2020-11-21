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
 *  Product:  PluginManager.Tests
 *  
 *  File: NotificationEventTests.cs
 *
 *  Purpose:  Tests for notification service
 *
 *  Date        Name                Reason
 *  12/05/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Internal;
using PluginManager.Abstractions;

namespace PluginManager.Tests
{
    [TestClass]
    public class NotificationEventTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidParam1()
        {
            INotificationService service = new NotificationService() as INotificationService;

            Assert.IsNotNull(service);

            service.RegisterListener(new InvalidEvents1());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidParam2()
        {
            INotificationService service = new NotificationService() as INotificationService;

            Assert.IsNotNull(service);

            service.RegisterListener(new InvalidEvents1());
        }

        [TestMethod]
        public void ValidParam()
        {
            INotificationService service = new NotificationService() as INotificationService;

            Assert.IsNotNull(service);

            service.RegisterListener(new ValidEvents1());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidEventForClass()
        {
            INotificationService service = new NotificationService() as INotificationService;

            Assert.IsNotNull(service);

            service.RegisterListener(new InvalidEvents1());
            object result = new object();
            service.RaiseEvent("Test2345", null, null, ref result);
        }

        [TestMethod]
        public void ListenerTest1()
        {
            ValidEvents1 valid1 = new ValidEvents1();
            ValidEvents2 valid2 = new ValidEvents2();
            INotificationService service = new NotificationService() as INotificationService;

            Assert.IsNotNull(service);

            service.RegisterListener(valid1);
            service.RegisterListener(valid2);

            object result = new object();
            bool eventProcessed = service.RaiseEvent("Test1", null, null, ref result);

            Assert.IsTrue(eventProcessed);
            Assert.IsTrue(valid1.EventCount == 1);
            Assert.IsTrue(valid2.EventCount == 1);


            eventProcessed = service.RaiseEvent("Test1", null, null, ref result);

            Assert.IsTrue(eventProcessed);
            Assert.IsTrue(valid1.EventCount == 2);
            Assert.IsTrue(valid2.EventCount == 2);

            eventProcessed = service.RaiseEvent("Test1", null, null, ref result);

            Assert.IsTrue(eventProcessed);
            Assert.IsTrue(valid1.EventCount == 3);
            Assert.IsTrue(valid2.EventCount == 3);

            eventProcessed = service.RaiseEvent("Test1", null, null, ref result);

            Assert.IsTrue(eventProcessed);
            Assert.IsTrue(valid1.EventCount == 4);
            Assert.IsTrue(valid2.EventCount == 4);
        }

        [TestMethod]
        public void ListenerTest2()
        {
            ValidEvents1 valid1 = new ValidEvents1();
            ValidEvents2 valid2 = new ValidEvents2();
            INotificationService service = new NotificationService() as INotificationService;

            Assert.IsNotNull(service);

            service.RegisterListener(valid1);
            service.RegisterListener(valid2);

            object result = new object();
            service.RaiseEvent("Test2", null, null, ref result);

            Assert.IsTrue(valid1.EventCount == 1);
            Assert.IsTrue(valid2.EventCount == 0);
        }
    }

    public class InvalidEvents1 : INotificationListener
    {
        public bool EventRaised(in string eventId, in object param1, in object param2, ref object result)
        {
            throw new NotImplementedException();
        }

        public void EventRaised(in string eventId, in object param1, in object param2)
        {
            throw new NotImplementedException();
        }

        public List<string> GetEvents()
        {
            return null;
        }
    }

    public class InvalidEvents2 : INotificationListener
    {
        public bool EventRaised(in string eventId, in object param1, in object param2, ref object result)
        {
            throw new NotImplementedException();
        }

        public void EventRaised(in string eventId, in object param1, in object param2)
        {
            throw new NotImplementedException();
        }

        public List<string> GetEvents()
        {
            return new List<string>() { "" };
        }
    }

    public class ValidEvents1 : INotificationListener
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Intended for developers not end users")]
        public bool EventRaised(in string eventId, in object param1, in object param2, ref object result)
        {
            switch (eventId)
            {
                case "Test1":
                    EventCount++;
                    return true;
                case "Test2":
                    EventCount++;
                    return false;
                default:
                    throw new InvalidOperationException("Invalid Event Name");
            }
        }

        public List<string> GetEvents()
        {
            return new List<string>() { "Test1", "Test2" };
        }

        public void EventRaised(in string eventId, in object param1, in object param2)
        {
            throw new NotImplementedException();
        }

        public uint EventCount { get; private set; }
    }

    public class ValidEvents2 : INotificationListener
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Intended for developers not end users")]
        public bool EventRaised(in string eventId, in object param1, in object param2, ref object result)
        {
            switch (eventId)
            {
                case "Test1":
                    EventCount++;
                    return true;
                case "Test2":
                    EventCount++;
                    return true;
                default:
                    throw new InvalidOperationException("Invalid Event Name");
            }
        }

        public List<string> GetEvents()
        {
            return new List<string>() { "Test1", "Test2" };
        }

        public void EventRaised(in string eventId, in object param1, in object param2)
        {
            throw new NotImplementedException();
        }

        public uint EventCount { get; private set; }
    }
}
