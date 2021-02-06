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
 *  Product:  PluginMiddleware
 *  
 *  File: ICountryProvider.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  16/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

namespace Middleware
{
    /// <summary>
    /// Provides country data used throughout the website.
    /// 
    /// This item must be implemented by the host application and made available via DI.
    /// </summary>
    public interface ICountryProvider
    {
        /// <summary>
        /// Retrieve all countries.
        /// </summary>
        /// <returns>List&lt;Country&gt;</returns>
        List<Country> GetAllCountries();

        /// <summary>
        /// Retrieve a list of all visible (available) countries.
        /// </summary>
        /// <returns>List&lt;Country&gt;</returns>
        List<Country> GetVisibleCountries();

        /// <summary>
        /// Update a country.
        /// </summary>
        /// <param name="country">Country to be updated.</param>
        /// <returns>bool.  True if the country was updated.</returns>
        bool CountryUpdate(in Country country);

        /// <summary>
        /// Deletes a country.
        /// </summary>
        /// <param name="country">Country to be deleted.</param>
        /// <returns>bool.  True if the country has been deleted.</returns>
        bool CountryDelete(in Country country);

        /// <summary>
        /// Create a country.
        /// </summary>
        /// <param name="name">Name of country.</param>
        /// <param name="code">Country code.</param>
        /// <param name="visible">bool.  Determines whether the country is visible or not.</param>
        /// <returns>Country</returns>
        Country CountryCreate(in string name, in string code, in bool visible);
    }
}
