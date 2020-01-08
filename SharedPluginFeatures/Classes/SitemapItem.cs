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
 *  Product:  SharedPluginFeatures
 *  
 *  File: SitemapItem.cs
 *
 *  Purpose:  Class which allows plugins to provide sitemap item.
 *
 *  Date        Name                Reason
 *  26/07/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
    /// <summary>
    /// Interface for an individual sitemap item.
    /// </summary>
    public sealed class SitemapItem
    {
        #region Constructors

        /// <summary>
        /// Constructor for item with no priority or modification date
        /// </summary>
        /// <param name="location">Partial or full uri</param>
        /// <param name="changeFrequency">Change frequency</param>
        public SitemapItem(Uri location, SitemapChangeFrequency changeFrequency)
            : this(location, changeFrequency, null, null)
        {

        }

        /// <summary>
        /// Constructor for item with no priority
        /// </summary>
        /// <param name="location">Partial or full uri</param>
        /// <param name="changeFrequency">Change frequency</param>
        /// <param name="lastModified">Date and time last modified</param>
        public SitemapItem(Uri location, SitemapChangeFrequency changeFrequency, DateTime lastModified)
            : this(location, changeFrequency, null, lastModified)
        {

        }

        /// <summary>
        /// Constructor for item with no last modified date
        /// </summary>
        /// <param name="location">Partial or full uri</param>
        /// <param name="changeFrequency">Change frequency</param>
        /// <param name="priority">Priority for item compared to other items</param>
        public SitemapItem(Uri location, SitemapChangeFrequency changeFrequency, decimal priority)
            : this(location, changeFrequency, priority, null)
        {

        }

        /// <summary>
        /// Constructor for items with priority and last modified date
        /// </summary>
        /// <param name="location">Partial or full uri</param>
        /// <param name="changeFrequency">Change frequency</param>
        /// <param name="priority">Priority for item compared to other items</param>
        /// <param name="lastModified">Date and time last modified</param>
        public SitemapItem(Uri location, SitemapChangeFrequency changeFrequency, decimal? priority, DateTime? lastModified)
        {
            Location = location ?? throw new ArgumentNullException(nameof(location));
            ChangeFrequency = changeFrequency;

            if (priority.HasValue && (priority < 0.0m || priority > 1.0m))
                throw new ArgumentOutOfRangeException(nameof(priority));

            Priority = priority ?? 0.5m;
            LastModified = lastModified;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The url for the sitemap item, this can be a full or partial uri
        /// </summary>
        /// <value>Uri</value>
        public Uri Location { get; }

        /// <summary>
        /// Date/time the item was last modified, this can be null
        /// </summary>
        /// <value>DateTime?</value>
        public DateTime? LastModified { get; }

        /// <summary>
        /// The frequency at which the item is updated
        /// </summary>
        /// <value>SitemapChangeFrequency</value>
        public SitemapChangeFrequency ChangeFrequency { get; }

        /// <summary>
        /// The priority of this item in comparison to other sitemap items.
        /// 
        /// This value must be between 0.0 and 1.0, if null a default value of 0.5 will be used.
        /// </summary>
        public decimal? Priority { get; }

        #endregion Properties
    }
}
