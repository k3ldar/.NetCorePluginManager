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
 *  Product:  SharedPluginFeatures
 *  
 *  File: StopWatchTimer.cs
 *
 *  Purpose:  Timer for perfomance counting
 *
 *  Date        Name                Reason
 *  10/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics;

namespace SharedPluginFeatures
{
    public struct StopWatchTimer : IDisposable
    {
        #region Private Members

        private Stopwatch _stopwatch;
        private Timings _timings;

        #endregion Private Members

        #region Constructors

        public StopWatchTimer(in Timings timings)
        {
            _timings = timings ?? throw new ArgumentNullException(nameof(timings));
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        #endregion Constructors

        #region Static Methods

        public static StopWatchTimer Initialise(in Timings timings)
        {
            return (new StopWatchTimer(timings));
        }

        #endregion Static Methods

        #region IDisposable Methods

        public void Dispose()
        {
            _stopwatch.Stop();
            _timings.Increment(_stopwatch);
        }

        #endregion IDisposable Methods
    }
}
