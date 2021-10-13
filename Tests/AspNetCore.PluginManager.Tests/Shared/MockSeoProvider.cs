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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockSeoProvider.cs
 *
 *  Purpose:  Mocks ISeoProvider for unit testing
 *
 *  Date        Name                Reason
 *  07/10/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class MockSeoProvider : ISeoProvider
    {
        private readonly Dictionary<string, MockSeoData> _seoData;

        public MockSeoProvider()
        {
            _seoData = new Dictionary<string, MockSeoData>();
        }

        public MockSeoProvider(Dictionary<string, MockSeoData> seoData)
        {
            _seoData = seoData ?? throw new ArgumentNullException(nameof(seoData));
        }

        public bool AddKeyword(in string route, in string keyword)
        {
            EnsureRouteExists(route);
            _seoData[route].Keywords.Add(keyword);
            return true;
        }

        public bool AddKeywords(in string route, in List<string> keyword)
        {
            EnsureRouteExists(route);
            string routeName = route;
            _seoData[route].Keywords.AddRange(keyword.Where(kw => !_seoData[routeName].Keywords.Contains(kw)).ToList());
            return true;
        }

        public bool GetSeoDataForRoute(in string route, out string title, out string metaDescription, out string author, out List<string> keywords)
        {

            if (_seoData.ContainsKey(route))
            {
                title = _seoData[route].Title;
                metaDescription = _seoData[route].MetaDescription;
                author = _seoData[route].Author;
                keywords = _seoData[route].Keywords;

                return true;
            }

            title = "No Title";
            metaDescription = "Meta Description";
            author = "Author";
            keywords = new List<string>();
            return false;
        }

        public bool RemoveKeyword(in string route, in string keyword)
        {
            EnsureRouteExists(route);
            _seoData[route].Keywords.Remove(keyword);
            return true;
        }

        public bool RemoveKeywords(in string route, in List<string> keyword)
        {
            EnsureRouteExists(route);

            foreach (string kword in keyword)
            {
                RemoveKeyword(route, kword);
            }
            
            return true;
        }

        public bool UpdateAuthor(in string route, in string author)
        {
            EnsureRouteExists(route);
            _seoData[route].Author = author;
            return true;
        }

        public bool UpdateDescription(in string route, in string description)
        {
            EnsureRouteExists(route);
            _seoData[route].MetaDescription = description;
            return true;

        }

        public bool UpdateTitle(in string route, in string title)
        {
            EnsureRouteExists(route);
            _seoData[route].Title = title;
            return true;
        }

        private void EnsureRouteExists(string route)
        {
            if (_seoData.ContainsKey(route))
                return;

            _seoData.Add(route, new MockSeoData());
            _seoData[route].Keywords = new List<string>();
        }
    }
}
