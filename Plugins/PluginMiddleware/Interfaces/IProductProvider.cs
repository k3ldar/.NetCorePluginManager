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
 *  Copyright (c) 2012 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: IProductProvider.cs
 *
 *  Purpose:  Provider provider
 *
 *  Date        Name                Reason
 *  31/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

using Middleware.Products;

namespace Middleware
{
    /// <summary>
    /// Product provider interface, used to query product data.  
    /// 
    /// This item must be implemented by the host application and made available via DI.
    /// </summary>
    public interface IProductProvider
    {
        #region Product Groups

        /// <summary>
        /// Request to retrieve all product groups that will be displayed on the website.
        /// </summary>
        /// <returns>List&lt;ProductGroup&gt;</returns>
        List<ProductGroup> ProductGroupsGet();

        /// <summary>
        /// Request to retrieve an individual product group based on its Id.
        /// </summary>
        /// <param name="id">Id of product group to retrieve.</param>
        /// <returns>ProductGroup</returns>
        ProductGroup ProductGroupGet(in int id);

        #endregion Product Groups

        #region Products

        /// <summary>
        /// Retrieves a group of products for display on the website.
        /// </summary>
        /// <param name="page">Page number of products to retrieve.</param>
        /// <param name="pageSize">Number of product items per page.</param>
        /// <returns>List&lt;Product&gt;</returns>
        List<Product> GetProducts(in int page, in int pageSize);

        /// <summary>
        /// Retrieves all products belonging to a product group.
        /// </summary>
        /// <param name="productGroup">ProductGroup instance.</param>
        /// <param name="page">Page number of products to retrieve.</param>
        /// <param name="pageSize">Number of product items per page.</param>
        /// <returns>List&lt;Product&gt;</returns>
        List<Product> GetProducts(in ProductGroup productGroup, in int page, in int pageSize);

        /// <summary>
        /// Retrieves an individual product based on its unique Id.
        /// </summary>
        /// <param name="id">Id of product to retrieve.</param>
        /// <returns>Product</returns>
        Product GetProduct(in int id);

        /// <summary>
        /// Retrieves the count of visible products
        /// </summary>
        int ProductCount { get; }

        #endregion Products
    }
}
