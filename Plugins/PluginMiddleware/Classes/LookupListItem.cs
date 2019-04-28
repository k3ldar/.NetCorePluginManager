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
    public sealed class LookupListItem
    {
        #region Constructors

        public LookupListItem(in int id, in string descirption)
        {
            if (String.IsNullOrEmpty(descirption))
                throw new ArgumentNullException(nameof(descirption));

            Id = id;
            Description = descirption;
        }

        #endregion Constructors

        #region Properties

        public int Id { get; set; }

        public string Description { get; set; }

        #endregion Properties
    }
}
