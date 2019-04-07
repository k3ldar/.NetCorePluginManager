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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Products.Plugin
 *  
 *  File: ProductController.cs
 *
 *  Purpose:  Product Controller
 *
 *  Date        Name                Reason
 *  31/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using ProductPlugin.Models;

using SharedPluginFeatures;

using Middleware;
using Middleware.Products;

using Languages;

namespace ProductPlugin.Controllers
{
    public class ProductController : BaseController
    {
        #region Private Members

        private readonly bool _hasShoppingCart;
        private readonly IProductProvider _productProvider;
        private readonly uint _productsPerPage;

        #endregion Private Members

        #region Constructors

        public ProductController(IProductProvider productProvider,
            ISettingsProvider settingsProvider, IPluginHelperService pluginHelper)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            ProductControllerSettings settings = settingsProvider.GetSettings<ProductControllerSettings>("Products");

            _productProvider = productProvider ?? throw new ArgumentNullException(nameof(productProvider));
            _productsPerPage = settings.ProductsPerPage;
            _hasShoppingCart = pluginHelper.PluginLoaded(SharedPluginFeatures.Constants.PluginNameShoppingCart, out int version);
        }

        #endregion Constructors

        #region Public Action Methods

        [HttpGet]
        public IActionResult Index(int? id)
        {
            return Index(String.Empty, id, 1);
        }

        [HttpGet]
        [Route("/Products/{groupName}/{id?}/Page/{page}/")]
        [Route("/Products/{groupName}/{id?}/")]
        public IActionResult Index(string groupName, int?id, int? page)
        {
            ProductGroup group = null;

            if (id.HasValue)
                group = _productProvider.ProductGroupsGet().Where(pg => pg.Id == id.Value).FirstOrDefault();

            if (group == null)
                group = _productProvider.ProductGroupsGet().FirstOrDefault();

            if (!page.HasValue || (page.HasValue && page.Value < 1))
                page = 1;

            List<Product> products = _productProvider.GetProducts(group, page.Value, (int)_productsPerPage);

            ProductGroupModel model = GetProductGroupModel(group, products, page.Value);

            return View(model);
        }

        [HttpGet]
        [Route("/Product/{id}/{productName}/")]
        public IActionResult Product(int id, string productName)
        {
            ProductModel model = GetProductModel(id);

            if (model == null)
                return RedirectToAction(nameof(Index));

            return View(model);
        }

        [HttpPost]
        public IActionResult AddToCart(AddToCartModel model)
        {
            Product product = _productProvider.GetProduct(model.Id);
            IShoppingCartProvider provider = (IShoppingCartProvider)HttpContext.RequestServices.GetService(typeof(IShoppingCartProvider));
            provider.AddToCart(GetUserSession(), GetCartSummary(), product, model.Quantity);

            return RedirectToAction("Product", "Product", new { id = model.Id, productName = BaseModel.RouteFriendlyName(product.Name) });
        }

        #endregion Public Action Methods

        #region Private Methods

        private ProductGroupModel GetProductGroupModel(in ProductGroup group, List<Product> products, in int page)
        {
            if (group == null)
                throw new ArgumentNullException(nameof(group));

            if (products == null)
                throw new ArgumentNullException(nameof(products));

            if (page < 1)
                throw new ArgumentOutOfRangeException(nameof(page));

            List<ProductCategoryModel> modelCategories = new List<ProductCategoryModel>();

            foreach (var item in _productProvider.ProductGroupsGet())
            {
                modelCategories.Add(new ProductCategoryModel(item.Id, item.Description, item.Url));
            }

            ProductGroupModel Result = new ProductGroupModel(GetBreadcrumbs(), GetCartSummary(), 
                modelCategories, group.Description, group.TagLine);

            foreach (Product product in products)
            {
                Result.Products.Add(new ProductCategoryProductModel(product.Id, product.Name, product.Images[0], 
                    group.Id, product.NewProduct, product.BestSeller, product.RetailPrice));
            }

            Result.Breadcrumbs.Clear();
            Result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.Home, "/", false));
            Result.Breadcrumbs.Add(new BreadcrumbItem(group.Description, $"/Products/{group.Id}/", false));

            Result.Pagination = BuildPagination(products.Count, (int)_productsPerPage, page, 
                $"/Products/{Result.RouteText(group.Description)}/{group.Id}/", "",
                LanguageStrings.Previous, LanguageStrings.Next);

            return Result;
        }

        private ProductModel GetProductModel(in int productId)
        {
            ProductModel Result = null;

            List<ProductCategoryModel> modelCategories = new List<ProductCategoryModel>();

            foreach (var item in _productProvider.ProductGroupsGet())
            {
                modelCategories.Add(new ProductCategoryModel(item.Id, item.Description, item.Url));
            }

            Product product = _productProvider.GetProduct(productId);

            if (product == null)
                return null;

            if (product != null)
            {
                if (_productProvider.ProductGroupGet(product.ProductGroupId) == null)
                    return null;

                Result = new ProductModel(GetBreadcrumbs(), GetCartSummary(), modelCategories, product.Id, product.ProductGroupId,
                    product.Name, product.Description, product.Features, product.VideoLink, product.Images, 
                    product.RetailPrice, _hasShoppingCart && product.RetailPrice > 0);
            }
            else
            {
                Result = new ProductModel(GetBreadcrumbs(), GetCartSummary(), modelCategories);
            }

            ProductGroup primaryProductGroup = _productProvider.ProductGroupGet(product.ProductGroupId);
            Result.Breadcrumbs.Clear();
            Result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.Home, "/", false));
            Result.Breadcrumbs.Add(new BreadcrumbItem(primaryProductGroup.Description, 
                $"/Products/{primaryProductGroup.Id}/", false));
            Result.Breadcrumbs.Add(new BreadcrumbItem(product.Name, 
                $"/Product/{product.Id}/{Result.RouteText(product.Name)}/", false));

            return Result;
        }

        #endregion Private Methods
    }
}
