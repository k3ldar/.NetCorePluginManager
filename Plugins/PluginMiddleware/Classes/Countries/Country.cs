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
 *  Product:  PluginMiddleware
 *  
 *  File: Country.cs
 *
 *  Purpose:  Country Details
 *
 *  Date        Name                Reason
 *  16/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace Middleware
{
    /// <summary>
    /// Country class designed to hold basic country data for viewing only.
    /// </summary>
    public sealed class Country
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Country()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of country.</param>
        /// <param name="code">Country code.</param>
        /// <param name="visible">Indicates whether the country is visible or not.</param>
        public Country(in string name, in string code, in bool visible)
        {
            Name = name;
            Code = code;
            Visible = visible;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Name of country.
        /// </summary>
        /// <value>string</value>
        public string Name { get; set; }

        /// <summary>
        /// Country code.
        /// </summary>
        /// <value>string</value>
        public string Code { get; set; }

        /// <summary>
        /// Indicates whether the country is visible or not.
        /// </summary>
        /// <value>bool</value>
        public bool Visible { get; set; }

        #endregion Properties
    }
}
