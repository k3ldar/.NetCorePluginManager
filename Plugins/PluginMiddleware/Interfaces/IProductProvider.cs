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
 *  Copyright (c) 2012 - 2022 Simon Carter.  All Rights Reserved.
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

        /// <summary>
        /// Saves changes to a an existing product group or creates a new product group
        /// </summary>
        /// <param name="id">id of product group being saved if an existing product group, otherwise -1 for a new group</param>
        /// <param name="description">Description of product group</param>
        /// <param name="showOnWebsite">Indicates whether the group is shown on a website or not</param>
        /// <param name="sortOrder">Sort order for product group relative to other groups</param>
        /// <param name="tagLine">Tagline for product group, usually displayed at the top of the page, if set</param>
        /// <param name="url">Specific url for product group, if available</param>
        /// <param name="errorMessage">Error message when saving the group, if the result is false</param>
        /// <returns>bool</returns>
        bool ProductGroupSave(in int id, in string description, in bool showOnWebsite,
            in int sortOrder, in string tagLine, in string url, out string errorMessage);

        /// <summary>
        /// Deletes a product group
        /// </summary>
        /// <param name="id">Id of product group to be deleted</param>
        /// <param name="errorMessage">error message if delete fails</param>
        /// <returns>bool</returns>
        bool ProductGroupDelete(in int id, out string errorMessage);

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


		/// <summary>
		/// Retrieves the count of visible products within a group
		/// </summary>
		int ProductCountForGroup(ProductGroup productGroup);

		/// <summary>
		/// Saves changes to an existing product or creates a new product
		/// </summary>
		/// <param name="id">id of product being saved if an existing product, otherwise -1 for a new product</param>
		/// <param name="productGroupId">Primary product id</param>
		/// <param name="name">Product Name</param>
		/// <param name="description">Product Description</param>
		/// <param name="features">List of features for a product</param>
		/// <param name="videoLink">Video link for a product</param>
		/// <param name="newProduct">Indicates that this is a new product</param>
		/// <param name="bestSeller">Indicates that this is a best seller</param>
		/// <param name="retailPrice">Retail price (excluding taxes) for a product</param>
		/// <param name="sku">Product SKU</param>
		/// <param name="isDownload">Indicates whether it is a downloadable product or not</param>
		/// <param name="allowBackOrder">Indicates that the product is available for back order if not currently available.</param>
		/// <param name="isVisible">Indicates whether the product is visible or not</param>
		/// <param name="errorMessage">Error message when saving the product, if the result is false</param>
		/// <returns></returns>
		bool ProductSave(in int id, in int productGroupId, in string name, in string description,
            in string features, in string videoLink, in bool newProduct, in bool bestSeller, in decimal retailPrice, in string sku,
            in bool isDownload, in bool allowBackOrder, in bool isVisible, out string errorMessage);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        bool ProductDelete(in int id, out string errorMessage);

        #endregion Products
    }
}
