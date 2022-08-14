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
 *  Copyright (c) 2019 - 2021 Simon Carter.  All Rights Reserved.
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
    /// <summary>
    /// Represents a knowledgebase group item.  Used with IHelpdeskProvider and HelpdeskPlugin module.
    /// </summary>
    public sealed class KnowledgeBaseGroup
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Unique id of the knowledgebase group.</param>
        /// <param name="name">Name of the knowledgebase group item.</param>
        /// <param name="description">Description for the group.</param>
        /// <param name="order">Sort order for the group item compared to other items.</param>
        /// <param name="viewCount">Number of times the group has been viewed.</param>
        /// <param name="parent">Knowledgebase group parent, or null if there is no parent.</param>
        /// <param name="items">List of KnowledgeBaseItem's that are within the group.</param>
        public KnowledgeBaseGroup(in long id, in string name, in string description, in int order,
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

        /// <summary>
        /// Unique id of the knowledgebase group.
        /// </summary>
        /// <value>long</value>
        public long Id { get; private set; }

        /// <summary>
        /// Name of the knowledgebase group item.
        /// </summary>
        /// <value>string</value>
        public string Name { get; private set; }

        /// <summary>
        /// Knowledgebase group parent, or null if there is no parent.
        /// </summary>
        /// <value>KnowledgeBaseGroup</value>
        public KnowledgeBaseGroup Parent { get; private set; }

        /// <summary>
        /// Description for the group.
        /// </summary>
        /// <value>string</value>
        public string Description { get; private set; }

        /// <summary>
        /// Number of times the group has been viewed.
        /// </summary>
        /// <value></value>
        public int ViewCount { get; private set; }

        /// <summary>
        /// Sort order for the group item compared to other items.
        /// </summary>
        /// <value>int</value>
        public int Order { get; private set; }

        /// <summary>
        /// List of KnowledgeBaseItem's that are within the group.
        /// </summary>
        /// <value>List&lt;KnowledgeBaseItem&gt;</value>
        public List<KnowledgeBaseItem> Items { get; private set; }

        #endregion Properties
    }
}
