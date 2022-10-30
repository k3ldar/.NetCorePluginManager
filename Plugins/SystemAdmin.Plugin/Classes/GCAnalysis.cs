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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SystemAdmin.Plugin
 *  
 *  File: GCSnapshot.cs
 *
 *  Purpose:  Garbage collection analysis thread
 *
 *  Date        Name                Reason
 *  07/11/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace SystemAdmin.Plugin.Classes
{
    public sealed class GCAnalysis : ThreadManager
    {
        #region Private Members

        private static readonly object _lockObject = new object();
        private const int MaximumStatistics = 20;
        private const int WaitForGCTimeout = 500;
        private readonly Timings _timings = new Timings();
        private static readonly Queue<GCSnapshot> _gcStatistics = new Queue<GCSnapshot>(MaximumStatistics);

        #endregion Private Members

        #region Constructors

        public GCAnalysis()
            : base(null, TimeSpan.FromSeconds(1), null, 500, 50, false)
        {
            GC.RegisterForFullGCNotification(10, 10);
        }

        #endregion Constructors

        #region Overridden Methods

        protected override bool Run(object parameters)
        {
            long memoryStart = 0;
            long memoryFinish = 0;
            bool completed = false;

            // Check for a notification of an approaching collection.
            GCNotificationStatus status = GC.WaitForFullGCApproach(WaitForGCTimeout);

            if (status == GCNotificationStatus.Succeeded)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                using (StopWatchTimer stopWatchTimer = StopWatchTimer.Initialise(_timings))
                {
                    memoryStart = GC.GetTotalMemory(false);
                    GC.Collect(2, GCCollectionMode.Forced, false, true);

                    status = GC.WaitForFullGCComplete();

                    if (status == GCNotificationStatus.Succeeded)
                    {
                        memoryFinish = GC.GetTotalMemory(false);
                        stopwatch.Stop();
                        completed = true;
                    }
                }

                if (completed)
                {
                    using (TimedLock tl = TimedLock.Lock(_lockObject))
                    {
                        if (_gcStatistics.Count == MaximumStatistics)
                            _gcStatistics.Dequeue();

                        _gcStatistics.Enqueue(new GCSnapshot((double)stopwatch.ElapsedTicks / TimeSpan.TicksPerMillisecond, memoryStart - memoryFinish));
                    }
                }
                else
                {
                    if (stopwatch.IsRunning)
                        stopwatch.Stop();
                }
            }
            else if (status == GCNotificationStatus.NotApplicable)
            {
                return false;
            }

            return !HasCancelled();
        }

        #endregion Overridden Methods

        #region Internal Methods

        internal static List<GCSnapshot> RetrieveGCData()
        {
            using (TimedLock tl = TimedLock.Lock(_lockObject))
            {
                return _gcStatistics.ToList();
            }
        }

        #endregion Internal Methods
    }
}

#pragma warning restore CS1591