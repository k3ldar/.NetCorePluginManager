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
 *  Product:  SharedPluginFeatures
 *  
 *  File: HtmlHelper.cs
 *
 *  Purpose: IHtmlHelper extension methods
 *
 *  Date        Name                Reason
 *  20/05/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Text;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace SharedPluginFeatures
{
    /// <summary>
    /// Html Helper class, contains extension methods and other methods for use within MVC application
    /// </summary>
    public static class HtmlHelper
    {
        /// <summary>
        /// Extension Method
        /// 
        /// Returns a route friendly name, one that will not require html encoding where spaces are separated by a dash and only alpha numeric characters are returned.
        /// </summary>
        /// <param name="_">IHtmlHelper instance that is being extended</param>
        /// <param name="s">Text to be transformed into a route friendly name</param>
        /// <returns>string.  Route friendly name.</returns>
        public static string RouteFriendlyName(this IHtmlHelper _, in string s)
        {
            return RouteFriendlyName(s);
        }

        /// <summary>
        /// Returns a route friendly name, one that will not require html encoding where spaces are separated by a dash and only alpha numeric characters are returned.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>string.  Route friendly name.</returns>
        public static string RouteFriendlyName(in string name)
        {
            if (string.IsNullOrEmpty(name))
                return name;

            StringBuilder Result = new StringBuilder(name.Length);
            char lastChar = ' ';

            foreach (char c in name)
            {
                if (c >= 65 && c <= 90)
                {
                    Result.Append(c);
                    lastChar = c;
                }
                else if (c >= 97 && c <= 122)
                {
                    Result.Append(c);
                    lastChar = c;
                }
                else if (c >= 48 && c <= 57)
                {
                    Result.Append(c);
                    lastChar = c;
                }
                else if (lastChar != Constants.Dash)
                {
                    Result.Append(Constants.Dash);
                    lastChar = Constants.Dash;
                }
            }

            if (Result[Result.Length - 1] == Constants.Dash)
                Result.Length = Result.Length - 1;

            return Result.ToString();
        }
    }
}
