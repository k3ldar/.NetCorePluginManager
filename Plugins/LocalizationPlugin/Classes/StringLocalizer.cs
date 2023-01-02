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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Localization.Plugin
 *  
 *  File: StringLocalizer.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  12/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Threading;

using Languages;

using Microsoft.Extensions.Localization;

using PluginManager;
using PluginManager.Abstractions;

using SharedPluginFeatures;

namespace Localization.Plugin
{
    internal sealed class StringLocalizer : IStringLocalizer
    {
        #region Private Members

        private readonly ILogger _logger;
        private static readonly ResourceManager _resourceManager = new ResourceManager("Languages.LanguageStrings",
            typeof(LanguageStrings).Assembly);

        private static readonly Timings _timings = new Timings();

        #endregion Private Members

        #region Constructors

        public StringLocalizer(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        #endregion Constructors

        #region IStringLocalizer Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "it's ok here, nothing to see, move along")]
        public LocalizedString this[string name]
        {
            get
            {
                using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timings))
                {
                    try
                    {
                        StringBuilder resourceName = new StringBuilder(name.Length);

                        // strip out any non alpha numeric characters
                        foreach (char c in name)
                        {
                            if (c >= 65 && c <= 90)
                                resourceName.Append(c);
                            else if (c >= 61 && c <= 122)
                                resourceName.Append(c);
                            else if (c >= 48 && c <= 57)
                                resourceName.Append(c);
                        }

                        string locString = _resourceManager.GetString(resourceName.ToString(),
                            Thread.CurrentThread.CurrentUICulture);

                        if (String.IsNullOrEmpty(locString))
                            return new LocalizedString(name, name);

                        return new LocalizedString(name, locString);
                    }
                    catch (Exception error)
                    {
                        _logger.AddToLog(LogLevel.Error, nameof(StringLocalizer), error, name);
                        return new LocalizedString(name, name);
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "it's ok here, nothing to see, move along")]
        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timings))
                {
                    try
                    {
                        StringBuilder resourceName = new StringBuilder(name.Length);

                        // strip out any non alpha numeric characters
                        foreach (char c in name)
                        {
                            if (c >= 65 && c <= 90)
                                resourceName.Append(c);
                            else if (c >= 61 && c <= 122)
                                resourceName.Append(c);
                            else if (c >= 48 && c <= 57)
                                resourceName.Append(c);
                        }

                        string resourceString = _resourceManager.GetString(resourceName.ToString(), Thread.CurrentThread.CurrentUICulture);
                        return new LocalizedString(name, String.Format(resourceString, arguments));
                    }
                    catch (Exception error)
                    {
                        _logger.AddToLog(LogLevel.Error, nameof(StringLocalizer), error, name);
                        return new LocalizedString(name, String.Format(name, arguments));
                    }
                }
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new InvalidOperationException();
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            throw new InvalidOperationException();
        }

        #endregion IStringLocalizer Methods

        #region Internal Properties

        internal static Timings LocalizationTimings
        {
            get
            {
                return _timings.Clone();
            }
        }

        #endregion Internal Properties
    }
}
