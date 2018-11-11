using System;
using System.Collections.Generic;
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
