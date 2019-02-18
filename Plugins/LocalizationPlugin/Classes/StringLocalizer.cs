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

using Microsoft.Extensions.Localization;

using Languages;

using SharedPluginFeatures;

namespace Localization.Plugin
{
    public sealed class StringLocalizer : IStringLocalizer
    {
        #region Private Members

        private static readonly ResourceManager _resourceManager = new ResourceManager("Languages.LanguageStrings", 
            typeof(LanguageStrings).Assembly);

        #endregion Private Members

        #region Constructors

        public StringLocalizer()
        {

        }


        #endregion Constructors

        #region IStringLocalizer Methods

        public LocalizedString this[string name]
        {
            get
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

                    return new LocalizedString(name, _resourceManager.GetString(resourceName.ToString(), Thread.CurrentThread.CurrentUICulture));
                }
                catch (Exception error)
                {
                    Initialisation.GetLogger.AddToLog(Enums.LogLevel.Localization, error, name);
                    return new LocalizedString(name, name);
                }
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                try
                {
                    string resourceString = _resourceManager.GetString(name, Thread.CurrentThread.CurrentUICulture);
                    return new LocalizedString(name, String.Format(resourceString, arguments));
                }
                catch (Exception error)
                {
                    Initialisation.GetLogger.AddToLog(Enums.LogLevel.Localization, error, name);
                    return new LocalizedString(name, String.Format(name, arguments));
                }
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion IStringLocalizer Methods
    }
}
