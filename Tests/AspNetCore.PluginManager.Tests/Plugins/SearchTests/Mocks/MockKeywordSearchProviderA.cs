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
 *  File: MockKeywordSearchProvider.cs
 *
 *  Purpose:  Mock keyword search provider
 *
 *  Date        Name                Reason
 *  03/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Middleware;
using Middleware.Search;

namespace AspNetCore.PluginManager.Tests.Search.Mocks
{
    [ExcludeFromCodeCoverage]
    public class MockKeywordSearchProviderA : ISearchKeywordProvider
    {
        public Dictionary<String, AdvancedSearchOptions> AdvancedSearch()
        {
            return null;
        }

        public List<SearchResponseItem> Search(in KeywordSearchOptions searchOptions)
        {
            if (searchOptions == null)
                throw new ArgumentNullException(nameof(searchOptions));

            if (searchOptions.ExactMatch && !searchOptions.QuickSearch)
                return new List<SearchResponseItem>();

            if (searchOptions.IsLoggedIn)
            {
                return new List<SearchResponseItem>()
                {
                    new SearchResponseItem("TestProviderA", "Modular Windows"),
                    new SearchResponseItem("TestProviderA", "Modular Table"),
                };
            }
            else
            {
                return new List<SearchResponseItem>()
                {
                    new SearchResponseItem("TestProviderA", "Modular Windows")
                };
            }
        }

        public Dictionary<string, string> SearchName()
        {
            return null;
        }

        public List<string> SearchResponseTypes(in Boolean quickSearch)
        {
            if (!quickSearch)
                return null;

            return new List<string>()
            {
                "TestProviderA"
            };
        }
    }
}
