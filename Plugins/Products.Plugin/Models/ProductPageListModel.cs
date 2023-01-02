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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Products.Plugin
 *  
 *  File: ProductPageListModel.cs
 *
 *  Purpose:  Product Page List Model
 *
 *  Date        Name                Reason
 *  09/12/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;

using Middleware.Products;

using SharedPluginFeatures;

namespace ProductPlugin.Models
{
    /// <summary>
    /// List of products for a page view
    /// </summary>
    public class ProductPageListModel : BaseModel
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductPageListModel(in BaseModelData modelData, List<Product> products, string pagination, int pageNumber)
            : base (modelData)
        {
            if (products == null)
                throw new ArgumentNullException(nameof(products));

            if (String.IsNullOrEmpty(pagination))
                throw new ArgumentNullException(nameof(pagination));

            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber));

            List<ProductListModel> items = new List<ProductListModel>();

            products.ForEach(p => items.Add(new ProductListModel(p.Id, p.Sku, p.Name)));

            Items = items;
            Pagination = pagination;
            PageNumber = pageNumber;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// List of items
        /// </summary>
        public IReadOnlyList<ProductListModel> Items { get; }

        /// <summary>
        /// Pagination for page views
        /// </summary>
        public string Pagination { get; }

        /// <summary>
        /// Current page number
        /// </summary>
        public int PageNumber { get; }

        #endregion Properties
    }
}
