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
 *  Copyright (c) 2019 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: KnowledgeBaseItem.cs
 *
 *  Purpose:  Knowledge Base Item
 *
 *  Date        Name                Reason
 *  24/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace Middleware.Helpdesk
{
    /// <summary>
    /// Represents a knowledgebase item that is part of a KnowledgeBaseGroup and used by IHelpdeskProvider within the HelpdeskPlugin module.
    /// </summary>
    public sealed class KnowledgeBaseItem
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Unique id of the knowledgebase item.</param>
        /// <param name="description">Description of the knowledgebase item.</param>
        /// <param name="viewCount">Number of times the knowledgebase item has been viewed.</param>
        /// <param name="content">Content for the knowledgebase item.</param>
        public KnowledgeBaseItem(in long id, in string description,
            in int viewCount, in string content)
        {
            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));

            if (String.IsNullOrEmpty(content))
                throw new ArgumentNullException(nameof(content));

            if (viewCount < 0)
                throw new ArgumentOutOfRangeException(nameof(viewCount));

            Id = id;
            Description = description;
            ViewCount = viewCount;
            Content = content;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Unique id of the knowledgebase item.
        /// </summary>
        /// <value>int</value>
        public long Id { get; private set; }

        /// <summary>
        /// Description of the knowledgebase item.
        /// </summary>
        /// <value>string</value>
        public string Description { get; private set; }

        /// <summary>
        /// Number of times the knowledgebase item has been viewed.
        /// </summary>
        /// <value>int</value>
        public int ViewCount { get; private set; }

        /// <summary>
        /// Content for the knowledgebase item.
        /// </summary>
        /// <value>string</value>
        public string Content { get; private set; }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Increases the view count the item by one.
        /// </summary>
        public void IncreaseViewCount()
        {
            ViewCount++;
        }

        #endregion Public Methods
    }
}
