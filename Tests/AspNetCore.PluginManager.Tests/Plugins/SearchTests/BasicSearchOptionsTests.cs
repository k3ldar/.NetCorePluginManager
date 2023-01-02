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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: BasicSearchOptionsTests.cs
 *
 *  Purpose:  Tests for basic search options
 *
 *  Date        Name                Reason
 *  03/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Search;

#pragma warning disable IDE0059

namespace AspNetCore.PluginManager.Tests.Plugins.SearchTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BasicSearchOptionsTests
    {
        [TestMethod]
        public void CreateBaseSearchOptionsLoggedInValidEmptySearchTerm()
        {
            BaseSearchOptions sut = new BaseSearchOptions(true, "");
			Assert.IsNotNull(sut);
		}

		[TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateBaseSearchOptionsLoggedOutInvalidSearchTerm()
        {
            BaseSearchOptions baseSearchOptions = new BaseSearchOptions(false, null);
        }

        [TestMethod]
        public void CreateBaseSearchOptionsLoggedIn()
        {
            BaseSearchOptions sut = new BaseSearchOptions(true, "anything");
			Assert.IsNotNull(sut);
		}

		[TestMethod]
        public void CreateBaseSearchOptionsLoggedOut()
        {
            BaseSearchOptions sut = new BaseSearchOptions(false, "anything");
			Assert.IsNotNull(sut);
		}
	}
}

#pragma warning restore IDE0059