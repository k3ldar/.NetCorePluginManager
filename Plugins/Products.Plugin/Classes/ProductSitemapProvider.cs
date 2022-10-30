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
 *  Product:  Product Plugin
 *  
 *  File: ProductSitemapProvider.cs
 *
 *  Purpose:  Provides sitemap functionality for products
 *
 *  Date        Name                Reason
 *  27/07/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Middleware;
using Middleware.Products;

using SharedPluginFeatures;

namespace ProductPlugin.Classes
{
    /// <summary>
    /// Product sitemap provider, provides sitemap information for products
    /// </summary>
    public class ProductSitemapProvider : ISitemapProvider
    {
        #region Private Members

        private readonly IProductProvider _productProvider;

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="productProvider">IProductProvider instance</param>
        public ProductSitemapProvider(IProductProvider productProvider)
        {
            _productProvider = productProvider ?? throw new ArgumentNullException(nameof(productProvider));
        }

        #endregion Constructors

        /// <summary>
        /// Retrieve a list of all product items that will be included in the sitemap
        /// </summary>
        /// <returns>List&lt;ISitemapItem&gt;</returns>
        public List<SitemapItem> Items()
        {
            List<SitemapItem> Result = new List<SitemapItem>();


            foreach (ProductGroup group in _productProvider.ProductGroupsGet())
            {
                if (group.ShowOnWebsite)
                {
                    string groupUrl = String.IsNullOrEmpty(group.Url) ?
                        $"Products/{HtmlHelper.RouteFriendlyName(group.Description)}/{group.Id}/" :
                        group.Url;

                    Result.Add(new SitemapItem(new Uri(groupUrl, UriKind.RelativeOrAbsolute),
                        SitemapChangeFrequency.Daily, 1.0m));
                }
            }

            foreach (Product product in _productProvider.GetProducts(1, ushort.MaxValue))
            {
                Result.Add(new SitemapItem(
                    new Uri($"Product/{product.Id}/{HtmlHelper.RouteFriendlyName(product.Name)}/", UriKind.RelativeOrAbsolute),
                    SitemapChangeFrequency.Hourly));
            }

            return Result;
        }
    }
}
