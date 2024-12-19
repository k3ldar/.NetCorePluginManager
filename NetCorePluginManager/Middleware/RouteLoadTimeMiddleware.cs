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
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: PageLoadSpeedMiddleware.cs
 *
 *  Purpose:  Middleware to monitor page load times
 *
 *  Date        Name                Reason
 *  29/07/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace AspNetCore.PluginManager.Middleware
{
	public sealed class RouteLoadTimeMiddleware : BaseMiddleware
	{
		#region Private Members

		private readonly RequestDelegate _next;
		private readonly static object _lockObject = new();
		private readonly static Dictionary<string, Timings> _pageTimings = [];

		#endregion Private Members

		#region Constructors

		public RouteLoadTimeMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		#endregion Constructors

		#region Public Methods

		public async Task Invoke(HttpContext context)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(GetPageTimings(RouteLowered(context))))
			{
				if (_next != null)
					await _next(context);
			}
		}

		#endregion Public Methods

		#region Internal Methods

		internal static void ClearPageTimings()
		{
			using (TimedLock tl = TimedLock.Lock(_lockObject))
			{
				_pageTimings.Clear();
			}
		}

		internal static Dictionary<string, Timings> ClonePageTimings()
		{
			using (TimedLock tl = TimedLock.Lock(_lockObject))
			{
				Dictionary<string, Timings> Result = [];

				foreach (KeyValuePair<string, Timings> entry in _pageTimings)
				{
					Result.Add(entry.Key, entry.Value.Clone());
				}

				return Result;
			}
		}

		#endregion Internal Methods

		#region Private Methods

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Timings GetPageTimings(string route)
		{
			using (TimedLock tl = TimedLock.Lock(_lockObject))
			{
				if (!_pageTimings.TryGetValue(route, out Timings timings))
				{
					timings = new Timings();
					_pageTimings.Add(route, timings);
				}

				return timings;
			}
		}

		#endregion Private Methods
	}
}

#pragma warning restore CS1591