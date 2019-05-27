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
 *  Product:  Demo Website
 *  
 *  File: MockSeoProvider.cs
 *
 *  Purpose:  Implements a mock ISeoProvider interface
 *
 *  Date        Name                Reason
 *  12/05/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
    public sealed class MockSeoProvider : ISeoProvider
    {
        #region Private Members

        private readonly Dictionary<string, string> _descriptions;
        private readonly Dictionary<string, string> _authors;
        private readonly Dictionary<string, string> _titles;
        private readonly Dictionary<string, List<string>> _keywords;

        #endregion Private Members

        #region Constructors

        public MockSeoProvider ()
        {
            _descriptions = new Dictionary<string, string>();
            _authors = new Dictionary<string, string>();
            _titles = new Dictionary<string, string>();
            _keywords = new Dictionary<string, List<string>>();

            _titles.Add("/", "Plugin Manager Demo Home page");
            _titles.Add("/Home", "Plugin Manager Demo Home page");
            _titles.Add("/Services/Middleware", "Middleware Plugin Description");
            _titles.Add("/Services/Api", "Api Plugin Description");
            _titles.Add("/Services/DependencyInjection", "Dependency Injection Plugin Description");
            _titles.Add("/Services/Website", "Website Plugin Description");
            _titles.Add("/Services/Custom", "Custom Plugin Description");
        }

        #endregion Constructors

        #region ISeoProvider Methods

        public bool AddKeyword(in string route, in string keyword)
        {
            if (!_keywords.ContainsKey(route))
            {
                _keywords.Add(route, new List<string>() { keyword });
            }
            else
            {
                List<string> keywordList = _keywords[route];

                if (keywordList.Contains(keyword))
                    return false;

                keywordList.Add(keyword);
            }

            return true;
        }

        public bool AddKeywords(in string route, in List<string> keywords)
        {
            if (!_keywords.ContainsKey(route))
            {
                _keywords.Add(route, new List<string>());
            }

            List<string> keywordList = _keywords[route];

            foreach (string s in keywords)
                if (!keywordList.Contains(s))
                    keywordList.Add(s);

            return true;
        }

        public bool GetSeoDataForRoute(in string route, out string title, 
            out string description, out string author, out List<string> keywords)
        {
            if (!_authors.ContainsKey(route))
                _authors.Add(route, "Simon Carter");

            if (!_titles.ContainsKey(route))
                _titles.Add(route, "ASP Net Core Demonstration Website");

            if (!_keywords.ContainsKey(route))
                _keywords.Add(route, new List<string>() { "ASP", "Net", "Core", "Plugin", "Manager" });

            if (!_descriptions.ContainsKey(route))
                _descriptions.Add(route, "ASP Net Core Plugin Manager - demo website");

            author = _authors[route];
            description = _descriptions[route];
            title = _titles[route];
            keywords = _keywords[route];

            return true;
        }

        public bool RemoveKeyword(in string route, in string keyword)
        {
            return true;
        }

        public bool RemoveKeywords(in string route, in List<string> keywords)
        {
            return true;
        }

        public bool UpdateDescription(in string route, in string description)
        {
            return true;
        }

        public bool UpdateTitle(in string route, in string title)
        {
            return true;
        }

        public bool UpdateAuthor(in string route, in string author)
        {
            return true;
        }

        #endregion ISeoProvider Methods
    }
}
