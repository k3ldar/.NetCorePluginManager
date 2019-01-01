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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
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
using System.Diagnostics;

namespace SharedPluginFeatures
{
    public sealed class Timings
    {
        #region Private Members

        private long _fastest;
        private long _slowest;
        private readonly object _lockObject;

        #endregion Private Members

        #region Constructors

        public Timings()
        {
            _lockObject = new object();
            _fastest = long.MaxValue;
            _slowest = long.MinValue;
        }

        #endregion Constructors

        #region Properties

        public uint Requests { get; private set; }

        public long Slowest
        {
            get
            {
                if (_slowest == long.MinValue)
                    return(0);

                return (_slowest);
            }
        }

        public long Fastest
        {
            get
            {
                if (_fastest == long.MaxValue)
                    return (0);

                return (_fastest);
            }
        }

        public decimal Average { get; private set; }

        public long Total { get; private set; }

        #endregion Properties

        #region Public Methods

        public void Increment(in Stopwatch stopWatch)
        {
            Increment(stopWatch.ElapsedMilliseconds);
        }

        public void Increment(in long totalMilliseconds)
        {
            lock (_lockObject)
            {
                Requests++;

                if (totalMilliseconds < _fastest)
                    _fastest = totalMilliseconds;

                if (totalMilliseconds > _slowest)
                    _slowest = totalMilliseconds;

                Total += totalMilliseconds;

                if (Total > 0)
                    Average = Total / Requests;
            }
        }

        #endregion Public Methods
    }
}
