using System;
using System.Diagnostics;
using System.Text;

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
                if (_slowest == long.MaxValue)
                    return(0);

                return (_slowest);
            }
        }

        public long Fastest
        {
            get
            {
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
