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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: ThreadManagerInitialisation.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  13/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Shared.Classes;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager
{
    internal static class ThreadManagerInitialisation
    {
        #region Private Members

        private static ILogger _logger;

        #endregion Private Members

        #region Internal Methods

        internal static void Initialise(in ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            ThreadManager.ThreadAbortForced += ThreadManager_ThreadAbortForced;
            ThreadManager.ThreadForcedToClose += ThreadManager_ThreadForcedToClose;
            ThreadManager.ThreadCancellAll += ThreadManager_ThreadCancellAll;
            ThreadManager.ThreadExceptionRaised += ThreadManager_ThreadExceptionRaised;
            ThreadManager.ThreadStarted += ThreadManager_ThreadStarted;
            ThreadManager.ThreadStopped += ThreadManager_ThreadStopped;
        }

        #endregion Internal Methods

        #region Private Static Methods

        private static void ThreadManager_ThreadStopped(object sender, Shared.ThreadManagerEventArgs e)
        {
            _logger.AddToLog(Enums.LogLevel.ThreadManager,
                String.Format("Thread stopped: {0}, Unresponsive: {1}, Marked For Removal: {2}, " +
                "Start Time: {3}, End Time: {4}", e.Thread.Name, e.Thread.UnResponsive.ToString(),
                e.Thread.MarkedForRemoval.ToString(), e.Thread.TimeStart.ToString("g"),
                e.Thread.TimeFinish.ToString("g")));
        }

        private static void ThreadManager_ThreadStarted(object sender, Shared.ThreadManagerEventArgs e)
        {
            _logger.AddToLog(Enums.LogLevel.ThreadManager,
                String.Format("Thread started: {0}, Unresponsive: {1}, Marked For Removal: {2}, " +
                "Start Time: {3}, End Time: {4}", e.Thread.Name, e.Thread.UnResponsive.ToString(),
                e.Thread.MarkedForRemoval.ToString(), e.Thread.TimeStart.ToString("g"),
                e.Thread.TimeFinish.ToString("g")));
        }

        private static void ThreadManager_ThreadExceptionRaised(object sender, Shared.ThreadManagerExceptionEventArgs e)
        {
            _logger.AddToLog(Enums.LogLevel.ThreadManager, e.Error,
                String.Format("Thread exception raised: {0}, Unresponsive: {1}, Marked For Removal: {2}, " +
                "Start Time: {3}, End Time: {4}", e.Thread.Name, e.Thread.UnResponsive.ToString(),
                e.Thread.MarkedForRemoval.ToString(), e.Thread.TimeStart.ToString("g"),
                e.Thread.TimeFinish.ToString("g")));
        }

        private static void ThreadManager_ThreadCancellAll(object sender, EventArgs e)
        {
            _logger.AddToLog(Enums.LogLevel.ThreadManager, "Thread cancel all");
        }

        private static void ThreadManager_ThreadAbortForced(object sender, Shared.ThreadManagerEventArgs e)
        {
            _logger.AddToLog(Enums.LogLevel.ThreadManager,
                String.Format("Thread abort forced to close: {0}, Unresponsive: {1}, Marked For Removal: {2}, " +
                "Start Time: {3}, End Time: {4}", e.Thread.Name, e.Thread.UnResponsive.ToString(),
                e.Thread.MarkedForRemoval.ToString(), e.Thread.TimeStart.ToString("g"),
                e.Thread.TimeFinish.ToString("g")));
        }

        private static void ThreadManager_ThreadForcedToClose(object sender, Shared.ThreadManagerEventArgs e)
        {
            _logger.AddToLog(Enums.LogLevel.ThreadManager, 
                String.Format("Thread forced to close: {0}, Unresponsive: {1}, Marked For Removal: {2}, " +
                "Start Time: {3}, End Time: {4}", e.Thread.Name, e.Thread.UnResponsive.ToString(), 
                e.Thread.MarkedForRemoval.ToString(), e.Thread.TimeStart.ToString("g"),
                e.Thread.TimeFinish.ToString("g")));
        }

        #endregion Private Static Methods
    }
}
