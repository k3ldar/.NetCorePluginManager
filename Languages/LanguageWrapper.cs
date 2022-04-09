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
 *  Product:  Languages
 *  
 *  File: LanguageWrapper.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  10/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Languages
{
    /// <summary>
    /// Static class which returns a list of all languages that are installed within the language pack.
    /// </summary>
    public static class LanguageWrapper
    {
        private const string DefaultLanguageCultureName = "en-GB";

        /// <summary>
        /// Gets all installed language files, default culture is first in the list
        /// </summary>
        /// <param name="path">Path where search to begin</param>
        /// <param name="defaultCulture">Default culture in use</param>
        /// <returns>String array of installed languages</returns>
        public static string[] GetInstalledLanguages(string path, CultureInfo defaultCulture)
        {
            string[] files = Directory.GetFiles(path, "Languages.resources*", SearchOption.AllDirectories);

            List<string> Result = new List<string>();
            Result.Add(defaultCulture.Name);

            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i].Replace(path, String.Empty);
                file = file.Substring(0, file.LastIndexOf('\\'));
                string language = file.Substring(file.LastIndexOf('\\') + 1);

                if (!Result.Contains(language))
                    Result.Add(language);
            }

            if (!Result.Contains(DefaultLanguageCultureName))
                Result.Add(DefaultLanguageCultureName);

            Result.Sort();

            int defaultCultureIndex = Result.IndexOf(defaultCulture.Name);

            if (defaultCultureIndex != 0)
            {
                Result.RemoveAt(defaultCultureIndex);
                Result.Insert(0, defaultCulture.Name);
            }

            return Result.ToArray();
        }

        /// <summary>
        /// Gets all installed language files, default culture is en-GB
        /// </summary>
        /// <param name="path">Path where search to begin</param>
        /// <returns>String array of installed languages</returns>
        public static string[] GetInstalledLanguages(string path)
        {
            return GetInstalledLanguages(path, new CultureInfo(DefaultLanguageCultureName));
        }
    }
}
