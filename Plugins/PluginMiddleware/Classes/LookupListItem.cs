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
 *  Copyright (c) 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: LookupListItem.cs
 *
 *  Purpose:  Standard container for lookup item
 *
 *  Date        Name                Reason
 *  19/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace Middleware
{
    /// <summary>
    /// Standard lookup list item.  Contains an id and description and is used in a variety of places.
    /// </summary>
    public sealed class LookupListItem
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="id">Id of lookup item.</param>
        /// <param name="descirption">Description of lookup item.</param>
        public LookupListItem(in int id, in string descirption)
        {
            if (String.IsNullOrEmpty(descirption))
                throw new ArgumentNullException(nameof(descirption));

            Id = id;
            Description = descirption;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Unique id of lookup item.
        /// </summary>
        /// <value>int</value>
        public int Id { get; set; }

        /// <summary>
        /// Description of lookup item.
        /// </summary>
        /// <value>string</value>
        public string Description { get; set; }

        #endregion Properties
    }
}
