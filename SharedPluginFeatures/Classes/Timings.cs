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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatures
 *  
 *  File: Timings.cs
 *
 *  Purpose:  Place holder for timings
 *
 *  Date        Name                Reason
 *  10/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics;

namespace SharedPluginFeatures
{
    /// <summary>
    /// Class used to contain timing data for requests.
    /// 
    /// This stores the number of requests and total time in milleseconds serving the requests.
    /// </summary>
    public sealed class Timings
    {
        #region Private Members

        private decimal _fastest;
        private decimal _slowest;
        private decimal _average;
        private decimal _total;
        private readonly object _lockObject;

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public Timings()
        {
            _lockObject = new object();
            _fastest = Decimal.MaxValue;
            _slowest = Decimal.MinValue;
            _average = 0;
            _total = 0;
            DecimalPlaces = 5;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Total number of requests made.
        /// </summary>
        public uint Requests { get; private set; }

        /// <summary>
        /// Indicates the total number of milliseconds used for the request that was slowest.
        /// </summary>
        public decimal Slowest
        {
            get
            {
                if (_slowest == Decimal.MinValue)
                    return 0;

                return Math.Round(_slowest / TimeSpan.TicksPerMillisecond, DecimalPlaces, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// Indicates the total number of milliseconds used for the request that was quickest.
        /// </summary>
        public decimal Fastest
        {
            get
            {
                if (_fastest == Decimal.MaxValue)
                    return 0;

                return Math.Round(_fastest / TimeSpan.TicksPerMillisecond, DecimalPlaces, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// Returns the average number of milliseconds per request.
        /// </summary>
        /// <value>decimal</value>
        public decimal Average
        {
            get
            {
                return Math.Round(_average / TimeSpan.TicksPerMillisecond, DecimalPlaces, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// Calculates the trimmed average, by removing the highest and lowest scores before averaging
        /// </summary>
        public decimal TrimmedAverage
        {
            get
            {
                if (_total == 0 || Requests < 3)
                    return 0;

                return Math.Round((_total - (_fastest + _slowest)) / (Requests - 2) / TimeSpan.TicksPerMillisecond, DecimalPlaces, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// Returns the total number of requests.
        /// </summary>
        /// <value>long</value>
        public decimal Total
        {
            get
            {
                return Math.Round(_total / TimeSpan.TicksPerMillisecond, DecimalPlaces, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// Number of decimal places the results should be rounded to, default is 5
        /// </summary>
        /// <value>byte</value>
        public byte DecimalPlaces { get; set; }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Increments the total milliseconds
        /// </summary>
        /// <param name="stopWatch"></param>
        public void Increment(in Stopwatch stopWatch)
        {
            if (stopWatch == null)
                throw new ArgumentNullException(nameof(stopWatch));

            Increment(stopWatch.ElapsedTicks);
        }

        /// <summary>
        /// Increments the total ticks
        /// </summary>
        /// <param name="totalTicks">Total number of ticks to increment by.</param>
        public void Increment(in long totalTicks)
        {
            lock (_lockObject)
            {
                Requests++;

                if (totalTicks < _fastest)
                    _fastest = totalTicks;

                if (totalTicks > _slowest)
                    _slowest = totalTicks;

                _total += totalTicks;

                if (_total > 0)
                    _average = _total / Requests;
            }
        }

        #endregion Public Methods
    }
}
