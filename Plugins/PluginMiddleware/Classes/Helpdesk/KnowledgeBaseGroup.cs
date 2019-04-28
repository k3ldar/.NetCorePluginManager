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
 *  File: KnowledgeBaseGroup.cs
 *
 *  Purpose:  Knowledge Base Group
 *
 *  Date        Name                Reason
 *  24/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

namespace Middleware.Helpdesk
{
    public sealed class KnowledgeBaseGroup
    {
        #region Constructors

        public KnowledgeBaseGroup(in int id, in string name, in string description, in int order, 
            in int viewCount, in KnowledgeBaseGroup parent, in List<KnowledgeBaseItem> items)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));

            if (viewCount < 0)
                throw new ArgumentOutOfRangeException(nameof(viewCount));

            Id = id;
            Name = name;
            Description = description;
            Order = order;
            ViewCount = viewCount;
            Parent = parent;
            Items = items ?? throw new ArgumentNullException(nameof(items));
        }

        #endregion Constructors

        #region Properties

        public int Id { get; private set; }

        public string Name { get; private set; }

        public KnowledgeBaseGroup Parent { get; private set; }

        public string Description { get; private set; }

        public int ViewCount { get; private set; }

        public int Order { get; private set; }

        public List<KnowledgeBaseItem> Items { get; private set; }

        #endregion Properties
    }
}
