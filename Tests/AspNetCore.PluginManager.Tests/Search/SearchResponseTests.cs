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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: SearchResponseTests.cs
 *
 *  Purpose:  Tests for Search Response
 *
 *  Date        Name                Reason
 *  03/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Search;

namespace AspNetCore.PluginManager.Tests.Search
{
    [TestClass]
    public class SearchResponseTests
    {
        [TestMethod]
        public void SearchResponseLoggedInValidSearchTerm()
        {
            SearchResponse response = new SearchResponse(true, "test");

            Assert.IsTrue(response.IsLoggedIn);
            Assert.AreEqual(response.SearchTerm, "test");
        }

        [TestMethod]
        public void SearchResponseLoggedOutValidSearchTerm()
        {
            SearchResponse response = new SearchResponse(false, "test");

            Assert.IsFalse(response.IsLoggedIn);
            Assert.AreEqual(response.SearchTerm, "test");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SearchResponseLoggedOutInvalidSearchTermNull()
        {
            SearchResponse response = new SearchResponse(false, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SearchResponseLoggedOutInvalidSearchTermEmptyString()
        {
            SearchResponse response = new SearchResponse(false, "");
        }
    }
}