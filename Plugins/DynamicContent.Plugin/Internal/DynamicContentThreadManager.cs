/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  DynamicContent.Plugin
 *  
 *  File: DynamicContentThreadManager.cs
 *
 *  Purpose:  Manages cached dynamic content in thread
 *
 *  Date        Name                Reason
 *  22/07/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Middleware;
using Middleware.DynamicContent;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

using static SharedPluginFeatures.Constants;

#pragma warning disable CS1591

namespace DynamicContent.Plugin.Internal
{
	public sealed class DynamicContentThreadManager : ThreadManager, INotificationListener
	{
		#region Private Members

		private readonly Timings _updateContentTimings = new();
		private readonly IDynamicContentProvider _dynamicContentProvider;
		private readonly object _lockObject = new();

		#endregion Private Members

		#region Constructors

		public DynamicContentThreadManager(INotificationService notificationService, IDynamicContentProvider dynamicContentProvider)
			: base(null, new TimeSpan(0, 0, 0, 0, 250))
		{
			if (notificationService == null)
				throw new ArgumentNullException(nameof(notificationService));

			_dynamicContentProvider = dynamicContentProvider ?? throw new ArgumentNullException(nameof(dynamicContentProvider));

			UpdateRequired = true;
			ContinueIfGlobalException = true;
			notificationService.RegisterListener(this);
		}

		#endregion Constructors

		#region Properties

		public bool UpdateRequired { get; private set; }

		public DateTime NextUpdate { get; private set; }

		public Timings UpdateContentTimings
		{
			get
			{
				return _updateContentTimings.Clone();
			}
		}

		public static int CacheCount
		{
			get
			{
				return PluginInitialisation.DynamicContentCache.Count;
			}
		}

		#endregion Properties

		#region INotificationListener Methods

		public bool EventRaised(in string eventId, in object param1, in object param2, ref object result)
		{
			if (eventId.Equals(NotificationEventDynamicContentUpdated))
			{
				UpdateRequired = true;
				return true;
			}

			return false;
		}

		public void EventRaised(in string eventId, in object param1, in object param2)
		{
			if (eventId.Equals(NotificationEventDynamicContentUpdated))
				UpdateRequired = true;
		}

		public List<string> GetEvents()
		{
			return
			[
				NotificationEventDynamicContentUpdated
			];
		}

		#endregion INotificationListener Methods

		#region Overridden Methods

		protected override bool Run(object parameters)
		{
			if (UpdateRequired || DateTime.Now > NextUpdate)
			{
				using (TimedLock timedLock = TimedLock.Lock(_lockObject))
				using (StopWatchTimer stopWatch = StopWatchTimer.Initialise(_updateContentTimings))
				{
					UpdateDynamicContent();
				}
			}

			return !HasCancelled();
		}

		#endregion Overridden Methods

		#region Private Methods

		private void UpdateDynamicContent()
		{
			DateTime nextUpdate = DateTime.MaxValue;
			DateTime processTime = DateTime.Now;
			PluginInitialisation.DynamicContentCache.Clear();

			foreach (IDynamicContentPage dynamicContentPage in _dynamicContentProvider.GetCustomPages())
			{
				if (dynamicContentPage.ActiveTo > processTime && dynamicContentPage.ActiveTo < nextUpdate)
					nextUpdate = dynamicContentPage.ActiveTo;

				if (dynamicContentPage.ActiveFrom > processTime && dynamicContentPage.ActiveFrom < nextUpdate)
					nextUpdate = dynamicContentPage.ActiveFrom;

				if (dynamicContentPage.ActiveFrom <= processTime && dynamicContentPage.ActiveTo >= processTime)
				{
					CacheItem cacheItem = new(dynamicContentPage.RouteName.ToLower(), dynamicContentPage);
					PluginInitialisation.DynamicContentCache.Add(dynamicContentPage.RouteName.ToLower(), cacheItem, true);
				}
			}


			UpdateRequired = false;
			NextUpdate = nextUpdate;
		}

		#endregion Private Methods
	}
}

#pragma warning restore CS1591