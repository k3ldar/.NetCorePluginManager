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
 *  Product:  Helpdesk Plugin
 *  
 *  File: FaqGroup.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  27/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelpdeskPlugin.Models
{
    public sealed class FaqGroup
    {
        #region Constructors

        public FaqGroup(in int id, in string name, in string description, 
            in List<FaqGroupItem> items, in int subGroupCount)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));

            if (subGroupCount < 0)
                throw new ArgumentOutOfRangeException(nameof(subGroupCount));

            Id = id;
            Name = name;
            Description = description;
            Items = items ?? throw new ArgumentNullException(nameof(items));
            SubGroupCount = subGroupCount;
        }

        #endregion Constructors

        #region Properties

        public int Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public int SubGroupCount { get; private set; }

        public List<FaqGroupItem> Items { get; private set; }

        public bool HasItems
        {
            get
            {
                return Items.Count > 0;
            }
        }

        #endregion Properties

        #region Public Methods

        public List<FaqGroupItem> GetTopItems(int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count));

            return Items.OrderByDescending(o => o.ViewCount).Take(count).ToList();
        }

        #endregion Public Methods
    }
}
