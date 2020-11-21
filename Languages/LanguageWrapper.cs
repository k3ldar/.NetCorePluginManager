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
using System.IO;

namespace Languages
{
    /// <summary>
    /// Static class which returns a list of all languages that are installed within the language pack.
    /// </summary>
    public static class LanguageWrapper
    {
        /// <summary>
        /// Gets all installed language files
        /// </summary>
        /// <param name="path">Path where search to begin</param>
        /// <returns>String array of installed languages</returns>
        public static string[] GetInstalledLanguages(string path)
        {
            string[] files = Directory.GetFiles(path, "Languages.resources*", SearchOption.AllDirectories);

            string[] Result = new string[files.Length + 1];
            Result[0] = "en-GB";

            for (int i = 1; i < Result.Length; i++)
            {
                string file = files[i - 1].Replace(path, String.Empty);
                file = file.Substring(0, file.LastIndexOf('\\'));
                Result[i] = file.Substring(file.LastIndexOf('\\') + 1);
            }

            return (Result);
        }
    }
}
