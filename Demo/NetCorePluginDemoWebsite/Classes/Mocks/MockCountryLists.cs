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
 *  Product:  Demo Website
 *  
 *  File: MockCountryLists.cs
 *
 *  Purpose:  Mock IAccountProvider for tesing purpose
 *
 *  Date        Name                Reason
 *  14/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Middleware;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
    [ExcludeFromCodeCoverage(Justification = "Code coverage not required for mock classes")]
    public class MockCountryLists : ICountryProvider
    {
        public Country CountryCreate(in string name, in string code, in bool visible)
        {
            return new Country(name, code, visible);
        }

        public bool CountryDelete(in Country country)
        {
            return false;
        }

        public bool CountryUpdate(in Country country)
        {
            return true;
        }

        public List<Country> GetAllCountries()
        {
            return new List<Country>()
            {
                new Country("USA", "US", true),
                new Country("England", "GB", true),
                new Country("Unknown", "UK", false),
            };
        }

        public List<Country> GetVisibleCountries()
        {
            return new List<Country>()
            {
                new Country("USA", "US", true),
                new Country("England", "GB", true),
            };
        }
    }
}
