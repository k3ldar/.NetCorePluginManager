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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: ProductGroup.cs
 *
 *  Purpose:  Product Group
 *
 *  Date        Name                Reason
 *  31/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace Middleware.Products
{
    /// <summary>
    /// Displays information for a product group within a website.
    /// </summary>
    public class ProductGroup
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Unique id of product group.</param>
        /// <param name="description">Description for Product Group.</param>
        /// <param name="showOnWebsite">Determines whether the product group is visible on the website or not.</param>
        /// <param name="sortOrder">Sort order in comparison to other product groups.</param>
        /// <param name="tagLine">Tag line displayed at the top of the page when thr group is shown.</param>
        /// <param name="url">Custom url to be redirected to if the group is selected.  Default route values apply if not set.</param>
        public ProductGroup(in int id, in string description, in bool showOnWebsite,
            in int sortOrder, in string tagLine, in string url)
        {
            if (string.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));

            Id = id;
            Description = description;
            ShowOnWebsite = showOnWebsite;
            SortOrder = sortOrder;
            TagLine = tagLine ?? String.Empty;
            Url = url ?? String.Empty;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Unique id of product group.
        /// </summary>
        /// <value>int</value>
        public int Id { get; }

        /// <summary>
        /// Description for Product Group.
        /// </summary>
        /// <value>string</value>
        public string Description { get; }

        /// <summary>
        /// Determines whether the product group is visible on the website or not.
        /// </summary>
        /// <value>bool.  If true the value is shown.</value>
        public bool ShowOnWebsite { get; }

        /// <summary>
        /// Sort order in comparison to other product groups.
        /// </summary>
        /// <value>int</value>
        public int SortOrder { get; }

        /// <summary>
        /// Tag line displayed at the top of the page when thr group is shown.
        /// </summary>
        /// <value>string</value>
        public string TagLine { get; }

        /// <summary>
        /// Custom url to be redirected to if the group is selected.  Default route values apply if not set.
        /// </summary>
        /// <value>string</value>
        public string Url { get; }

        #endregion Properties
    }
}
