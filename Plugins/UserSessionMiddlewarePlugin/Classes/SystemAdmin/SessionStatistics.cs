using System;
using System.Collections.Generic;
using System.Text;

namespace UserSessionMiddleware.Plugin.Classes.SystemAdmin
{
    internal sealed class SessionStatistics
    {
        #region Constructors

        internal SessionStatistics()
        {
            Count = 0;
            IsBot = false;
        }

        internal SessionStatistics(in string countryCode)
            : this ()
        {
            if (String.IsNullOrEmpty(countryCode))
                throw new ArgumentNullException(nameof(countryCode));

            CountryCode = countryCode;
        }

        #endregion Constructors

        #region Static Methods

        

        #endregion Static Methods

        #region Properties

        internal int Count { get; set; }

        internal bool IsBot { get; set; }

        internal string CountryCode { get; set; }

        internal decimal Value { get; set; }

        #endregion Properties
    }
}
