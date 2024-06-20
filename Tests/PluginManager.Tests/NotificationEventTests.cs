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
using System.Diagnostics.CodeAnalysis;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;
using PluginManager.Internal;
using PluginManager.Tests.Mocks;

using Shared.Classes;

#pragma warning disable S2925

namespace PluginManager.Tests
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class NotificationEventTests
	{
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void InvalidParam1()
		{
			INotificationService service = new NotificationService() as INotificationService;

			Assert.IsNotNull(service);

			service.RegisterListener(new InvalidEventsListenerNullEvents());
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void InvalidParam2()
		{
			INotificationService service = new NotificationService() as INotificationService;

			Assert.IsNotNull(service);

			service.RegisterListener(new InvalidEventsListenerNullEvents());
		}

		[TestMethod]
		public void ValidParam()
		{
			INotificationService service = new NotificationService() as INotificationService;

			Assert.IsNotNull(service);

			service.RegisterListener(new ValidEventsListener());
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void InvalidEventForClass()
		{
			INotificationService service = new NotificationService() as INotificationService;

			Assert.IsNotNull(service);

			service.RegisterListener(new InvalidEventsListenerNullEvents());
			object result = new();
			service.RaiseEvent("Test2345", null, null, ref result);
		}

		[TestMethod]
		public void RaiseEvent_TwoListeners_FirstListenerRespondsTrue_SecondListenerNotProcessed_Success()
		{
			ValidEventsListener validListnerProcessingEvent = new();
			ValidEvents2 validListenerNotProcessingEvent = new();
			INotificationService service = new NotificationService() as INotificationService;

			Assert.IsNotNull(service);

			service.RegisterListener(validListnerProcessingEvent);
			service.RegisterListener(validListenerNotProcessingEvent);

			object result = new();
			bool eventProcessed = service.RaiseEvent("Test1", null, null, ref result);

			Assert.IsTrue(eventProcessed);
			Assert.IsTrue(validListnerProcessingEvent.EventCount == 1);
			Assert.IsTrue(validListenerNotProcessingEvent.EventCount == 0);


			eventProcessed = service.RaiseEvent("Test1", null, null, ref result);

			Assert.IsTrue(eventProcessed);
			Assert.IsTrue(validListnerProcessingEvent.EventCount == 2);
			Assert.IsTrue(validListenerNotProcessingEvent.EventCount == 0);

			eventProcessed = service.RaiseEvent("Test1", null, null, ref result);

			Assert.IsTrue(eventProcessed);
			Assert.IsTrue(validListnerProcessingEvent.EventCount == 3);
			Assert.IsTrue(validListenerNotProcessingEvent.EventCount == 0);

			eventProcessed = service.RaiseEvent("Test1", null, null, ref result);

			Assert.IsTrue(eventProcessed);
			Assert.IsTrue(validListnerProcessingEvent.EventCount == 4);
			Assert.IsTrue(validListenerNotProcessingEvent.EventCount == 0);
		}

		[TestMethod]
		public void RaiseEvent_MultipleListeners_FirstListenerDoesNotProcessEvent_SecondListenerDoes_Success()
		{
			ValidEventsListener listenerNotProcessing = new();
			ValidEvents2 listenerProcessing = new();
			INotificationService service = new NotificationService() as INotificationService;

			Assert.IsNotNull(service);

			service.RegisterListener(listenerNotProcessing);
			service.RegisterListener(listenerProcessing);

			object result = new();
			service.RaiseEvent("Test2", null, null, ref result);

			Assert.IsTrue(listenerNotProcessing.EventCount == 1);
			Assert.IsTrue(listenerProcessing.EventCount == 1);
			Assert.IsFalse(listenerNotProcessing.EventProcessed);
			Assert.IsTrue(listenerProcessing.EventProcessed);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NotificationService_RegisterListener_ArgumentNullException_InvalidParam_Null_Throws_ArgumentNullException()
		{
			NotificationService sut = new();
			sut.RegisterListener(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NotificationService_UnregisterListener_ArgumentNullException_InvalidParam_Null_Throws_ArgumentNullException()
		{
			NotificationService sut = new();
			sut.UnregisterListener(null);
		}

		[TestMethod]
		public void NotificationService_RaiseEvent_EventNotRegistered_Returns_False()
		{
			NotificationService sut = new();
			object result = new();
			bool raiseResult = sut.RaiseEvent("unknownevent", null, null, ref result);

			Assert.IsFalse(raiseResult);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void NotificationService_RegisterListener_ContainsNullEventName_Throws_InvalidOperationException()
		{
			NotificationService sut = new();
			sut.RegisterListener(new InvalidEventsListenerNullEventName());
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void NotificationService_UnregisterListener_ContainsNoEventNames_Throws_InvalidOperationException()
		{
			NotificationService sut = new();
			sut.UnregisterListener(new InvalidEventsListenerNoEvents());
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void NotificationService_UnregisterListener_ContainsInvalidEventNames_Null_Throws_InvalidOperationException()
		{
			NotificationService sut = new();
			sut.UnregisterListener(new InvalidEventsListenerNullEvents());
		}

		[TestMethod]
		public void NotificationService_UnregisterListener_Valid_Returns_True()
		{
			NotificationService sut = new();
			ValidEventsListener listener = new();
			bool registerResult = sut.RegisterListener(listener);

			Assert.IsTrue(registerResult);

			bool unregisterResult = sut.UnregisterListener(listener);

			Assert.IsTrue(unregisterResult);
		}

		[TestMethod]
		public void NotificationService_Run_NotifiesAllInterestedListeners_Success()
		{
			ThreadManager.Initialise();
			MockLogger testLogger = new();

			ThreadManagerInitialisation tmi = new();
			tmi.Initialise(testLogger);
			try
			{
				NotificationService sut = new();
				ValidEventsListener listener1 = new();
				ValidEventsListener listener2 = new();
				ValidEventsNotUsed listener3 = new();

				Assert.IsTrue(sut.RegisterListener(listener1));
				Assert.IsTrue(sut.RegisterListener(listener2));
				Assert.IsTrue(sut.RegisterListener(listener3));

				ThreadManager.ThreadStart(sut, "TestListener", System.Threading.ThreadPriority.Normal);

				sut.RaiseEvent("Test1");
				sut.RaiseEvent("Test2", 123);
				sut.RaiseEvent("Test1", "test", sut);

				int waitCount = 0;
				bool keepWaiting = true;

				while (keepWaiting)
				{
					if (waitCount > 60)
						keepWaiting = false;

					Thread.Sleep(50);
					waitCount++;

					if (listener2.EventCount > 2)
						keepWaiting = false;
				}

				Assert.AreEqual(3, listener1.EventCount);
				Assert.AreEqual(1, listener1.Event2Count);
				Assert.AreEqual(2, listener1.Event1Count);
				Assert.IsNull(listener1.param1Values[0]);
				Assert.IsNotNull(listener1.param1Values[1]);
				Assert.IsInstanceOfType(listener1.param1Values[1], typeof(Int32));
				Assert.AreEqual(123, listener1.param1Values[1]);
				Assert.IsInstanceOfType(listener1.param1Values[2], typeof(string));
				Assert.AreEqual("test", listener1.param1Values[2]);
				Assert.IsNull(listener1.param2Values[0]);
				Assert.IsNull(listener1.param2Values[1]);
				Assert.IsNotNull(listener1.param2Values[2]);
				Assert.AreEqual(sut, listener1.param2Values[2]);


				Assert.AreEqual(3, listener2.EventCount);
				Assert.AreEqual(1, listener2.Event2Count);
				Assert.AreEqual(2, listener2.Event1Count);
				Assert.IsNull(listener2.param1Values[0]);
				Assert.IsNotNull(listener2.param1Values[1]);
				Assert.IsInstanceOfType(listener2.param1Values[1], typeof(Int32));
				Assert.AreEqual(123, listener2.param1Values[1]);
				Assert.IsInstanceOfType(listener2.param1Values[2], typeof(string));
				Assert.AreEqual("test", listener2.param1Values[2]);
				Assert.IsNull(listener2.param2Values[0]);
				Assert.IsNull(listener2.param2Values[1]);
				Assert.IsNotNull(listener2.param2Values[2]);
				Assert.AreEqual(sut, listener2.param2Values[2]);


				Assert.IsTrue(sut.UnregisterListener(listener1));
				Assert.IsTrue(sut.UnregisterListener(listener2));
				Assert.IsTrue(sut.UnregisterListener(listener3));

				sut.CancelThread();
			}
			finally
			{
				tmi.Finalise();
				ThreadManager.Finalise();
			}
		}

		[TestMethod]
		public void NotificationService_Run_NotifiesAllInterestedListeners_ExceedsQueueSize_Success()
		{
			ThreadManager.Initialise();
			MockLogger testLogger = new();
			ThreadManagerInitialisation tmi = new();
			tmi.Initialise(testLogger);
			try
			{
				NotificationService sut = new();
				ValidEventsListener listener1 = new();

				Assert.IsTrue(sut.RegisterListener(listener1));

				ThreadManager.ThreadStart(sut, "TestListener", System.Threading.ThreadPriority.Normal);

				for (int i = 0; i < 305; i++)
				{
					sut.RaiseEvent("Test1");
				}

				int waitCount = 0;
				bool keepWaiting = true;

				while (keepWaiting)
				{
					if (waitCount > 60)
						keepWaiting = false;

					Thread.Sleep(50);
					waitCount++;

					if (listener1.EventCount > 300)
						keepWaiting = false;
				}

				Assert.AreEqual(305, listener1.EventCount);
				Assert.AreEqual(0, listener1.Event2Count);
				Assert.AreEqual(305, listener1.Event1Count);


				Assert.IsTrue(sut.UnregisterListener(listener1));

				sut.CancelThread();
			}
			finally
			{
				tmi.Finalise();
				ThreadManager.Finalise();
			}
		}

		[TestMethod]
		public void NotificationService_Run_NotifiesAllInterestedListeners_NoneFoundSuccess()
		{
			ThreadManager.Initialise();
			MockLogger testLogger = new();
			ThreadManagerInitialisation tmi = new();
			tmi.Initialise(testLogger);
			try
			{
				NotificationService sut = new();
				ValidEventsNotUsed listener3 = new();

				Assert.IsTrue(sut.RegisterListener(listener3));

				ThreadManager.ThreadStart(sut, "TestListener", System.Threading.ThreadPriority.Normal);

				sut.RaiseEvent("Test1");

				int waitCount = 0;
				bool keepWaiting = true;

				while (keepWaiting)
				{
					if (waitCount > 40)
						keepWaiting = false;

					Thread.Sleep(50);
					waitCount++;
				}

				Assert.AreEqual(0, listener3.EventCount);

				Assert.IsTrue(sut.UnregisterListener(listener3));

				sut.CancelThread();
			}
			finally
			{
				tmi.Finalise();
				ThreadManager.Finalise();
			}
		}

		#region Notification Queue Item

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NotificationQueueItem_Construct_InvalidParamEventId_Null_Throws_ArgumentNullException()
		{
			new NotificationQueueItem(null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NotificationQueueItem_Construct_InvalidParamEventId_EmptyString_Throws_ArgumentNullException()
		{
			new NotificationQueueItem(null, null, null);
		}

		[TestMethod]
		public void NotificationQueueItem_ContainsValidData_Success()
		{
			NotificationQueueItem sut = new("test", new MockLogger(), 123);

			Assert.AreEqual("test", sut.EventId);
			Assert.IsNotNull(sut.Param1);
			Assert.IsInstanceOfType(sut.Param1, typeof(ILogger));
			Assert.IsNotNull(sut.Param2);
			Assert.IsInstanceOfType(sut.Param2, typeof(Int32));
			Assert.AreEqual(123, (int)sut.Param2);
		}

		#endregion Notification Queue Item
	}

	[ExcludeFromCodeCoverage]
	public class InvalidEventsListenerNoEvents : INotificationListener
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
			return new List<string>();
		}
	}

	[ExcludeFromCodeCoverage]
	public class InvalidEventsListenerNullEventName : INotificationListener
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
			List<string> Result = new()
			{
				null
			};

			return Result;
		}
	}

	[ExcludeFromCodeCoverage]
	public class InvalidEventsListenerNullEvents : INotificationListener
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

	[ExcludeFromCodeCoverage]
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

	[ExcludeFromCodeCoverage]
	public class ValidEventsListener : INotificationListener
	{
		public List<object> param1Values = new();
		public List<object> param2Values = new();

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Intended for developers not end users")]
		public bool EventRaised(in string eventId, in object param1, in object param2, ref object result)
		{
			switch (eventId)
			{
				case "Test1":
					EventCount++;
					Event1Count++;
					param1Values.Add(param1);
					param2Values.Add(param2);
					EventProcessed = true;
					return true;
				case "Test2":
					EventCount++;
					Event2Count++;
					param1Values.Add(param1);
					param2Values.Add(param2);
					EventProcessed = false;
					return false;
				default:
					throw new InvalidOperationException("Invalid Event Name");
			}
		}

		public void EventRaised(in string eventId, in object param1, in object param2)
		{
			switch (eventId)
			{
				case "Test1":
					EventCount++;
					Event1Count++;
					param1Values.Add(param1);
					param2Values.Add(param2);
					break;

				case "Test2":
					EventCount++;
					Event2Count++;
					param1Values.Add(param1);
					param2Values.Add(param2);
					break;

				default:
					throw new InvalidOperationException("Invalid Event Name");
			}
		}

		public List<string> GetEvents()
		{
			return new List<string>() { "Test1", "Test2" };
		}

		public int EventCount { get; private set; }

		public int Event1Count { get; private set; }

		public int Event2Count { get; private set; }

		public bool EventProcessed { get; private set; }
	}

	[ExcludeFromCodeCoverage]
	public class ValidEventsNotUsed : INotificationListener
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Intended for developers not end users")]
		public bool EventRaised(in string eventId, in object param1, in object param2, ref object result)
		{
			switch (eventId)
			{
				case "Dependency":
					EventCount++;
					return true;
				case "Frustration":
					EventCount++;
					return true;
				default:
					throw new InvalidOperationException("Invalid Event Name");
			}
		}

		public void EventRaised(in string eventId, in object param1, in object param2)
		{
			throw new NotImplementedException();
		}

		public List<string> GetEvents()
		{
			return new List<string>() { "Dependency", "Frustration" };
		}

		public int EventCount { get; private set; }
	}

	[ExcludeFromCodeCoverage]
	public class ValidEvents2 : INotificationListener
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Intended for developers not end users")]
		public bool EventRaised(in string eventId, in object param1, in object param2, ref object result)
		{
			switch (eventId)
			{
				case "Test1":
					EventCount++;
					EventProcessed = true;
					return true;
				case "Test2":
					EventCount++;
					EventProcessed = true;
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

		public bool EventProcessed { get; private set; }
	}
}
#pragma warning restore S2925