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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Error Manager Plugin
 *  
 *  File: ErrorManagerSettings.cs
 *
 *  Purpose:  Settings
 *
 *  Date        Name                Reason
 *  17/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using AppSettings;

namespace ErrorManager.Plugin
{
    /// <summary>
    /// Settings that affect how the ErrorManager.Plugin module is configured.
    /// </summary>
    public class ErrorManagerSettings
    {
        /// <summary>
        /// Use random quotes
        /// </summary>
        /// <value>bool.  If true then ransomly selects a quote from Quotes property.</value>
        public bool RandomQuotes { get; set; }

        /// <summary>
        /// Array of quotes that can be displayed.
        /// </summary>
        /// <value>string[]</value>
        public string[] Quotes { get; set; }

        /// <summary>
        /// Encryption key, used to encrypt cookie values.
        /// </summary>
        /// <value>string</value>
        [SettingDefault("asldfjanpsa]3;la9e4823[2oer09oecrlc")]
        [SettingString(false, 20, 60)]
        public string EncryptionKey { get; set; }

        /// <summary>
        /// Login page that the user should be directed to if login is required.
        /// </summary>
        /// <value>string</value>
        [SettingDefault("/Login/")]
        [SettingUri(false, UriKind.RelativeOrAbsolute)]
        public string LoginPage { get; set; }

        /// <summary>
        /// Total number of quotes available.
        /// </summary>
        /// <returns>int</returns>
        public int Count()
        {
            if (Quotes == null)
                return 0;

            return Quotes.Length - 1;
        }

        /// <summary>
        /// Retrieves a random quote.
        /// </summary>
        /// <param name="index">Index of quote to retrieve.</param>
        /// <returns>string</returns>
        public string GetQuote(int index)
        {
            if (index < 0 || index > Count())
                index = 0;

            string Result;

            if (Quotes != null && index <= Count())
                Result = Quotes[index];
            else
                Result = "The page you were looking for could not be found\rPlease try navigating from the menu above.";

            Result = Result.Trim().Replace("\r", "</p><p>");

            return $"<p>{Result}</p>";
        }
    }
}
