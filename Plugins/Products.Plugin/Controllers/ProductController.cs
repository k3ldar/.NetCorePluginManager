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

using Languages;

using Microsoft.AspNetCore.Mvc;

using Middleware;
using Middleware.Interfaces;
using Middleware.Products;

using PluginManager.Abstractions;

using ProductPlugin.Models;

using SharedPluginFeatures;

using static SharedPluginFeatures.Constants;

#pragma warning disable CS1591, IDE0060

namespace ProductPlugin.Controllers
{
    /// <summary>
    /// Product controller displays standard product information on a website.
    /// </summary>
    [Subdomain(ProductController.Name)]
    public partial class ProductController : BaseController
    {
        #region Private Members

        private const string InvalidModel = "Invalid Model";
        private const string InvalidProduct = "Invalid Product";

        private readonly bool _hasShoppingCart;
        private readonly IProductProvider _productProvider;
        private readonly ProductPluginSettings _settings;
        private readonly IStockProvider _stockProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly IImageProvider _imageProvider;
        private readonly IShoppingCartProvider _shoppingCartProvider;

        #endregion Private Members

        #region Constructors

        public ProductController(IProductProvider productProvider, ISettingsProvider settingsProvider,
            IPluginHelperService pluginHelper, IStockProvider stockProvider, IMemoryCache memoryCache,
            IImageProvider imageProvider, IShoppingCartProvider shoppingCartProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            if (pluginHelper == null)
                throw new ArgumentNullException(nameof(pluginHelper));

            _settings = settingsProvider.GetSettings<ProductPluginSettings>(Name);

            _productProvider = productProvider ?? throw new ArgumentNullException(nameof(productProvider));
            _stockProvider = stockProvider ?? throw new ArgumentNullException(nameof(stockProvider));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _hasShoppingCart = pluginHelper.PluginLoaded(SharedPluginFeatures.Constants.PluginNameShoppingCart, out _);
            _imageProvider = imageProvider ?? throw new ArgumentNullException(nameof(imageProvider));
            _shoppingCartProvider = shoppingCartProvider ?? throw new ArgumentNullException(nameof(shoppingCartProvider));
        }

        #endregion Constructors

        #region Constants

        public const string Name = "Product";

        #endregion Constants

        #region Public Action Methods

        [HttpGet]
        public IActionResult Index(int? id)
        {
            return Index(String.Empty, id, 1);
        }

        [HttpGet]
        [Route("/Products/{groupName}/{id?}/Page/{page}/")]
        [Route("/Products/{groupName}/{id?}/")]
        public IActionResult Index(string groupName, int? id, int? page)
        {
            ProductGroup group = null;

            if (id.HasValue)
                group = _productProvider.ProductGroupsGet().Where(pg => pg.Id == id.Value).FirstOrDefault();

            if (group == null)
                group = _productProvider.ProductGroupsGet().FirstOrDefault();

            if (group == null)
                return RedirectToAction(nameof(Index));

            if (!page.HasValue || page.Value < 1)
                page = 1;

            List<Product> products = _productProvider.GetProducts(group, page.Value, (int)_settings.ProductsPerPage);

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
        [DenySpider]
        public IActionResult AddToCart(AddToCartModel model)
        {
            if (model == null)
                return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidModel);

            Product product = _productProvider.GetProduct(model.Id);

            if (product == null)
                return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidProduct);

            _shoppingCartProvider.AddToCart(GetUserSession(), GetCartSummary(), product, model.Quantity);

            return RedirectToAction("Product", "Product", new { id = model.Id, productName = BaseModel.RouteFriendlyName(product.Name) });
        }

        #endregion Public Action Methods

        #region Private Methods

        private ProductGroupModel GetProductGroupModel(in ProductGroup group, List<Product> products, in int page)
        {
            List<ProductCategoryModel> modelCategories = new();

            foreach (ProductGroup item in _productProvider.ProductGroupsGet())
            {
                modelCategories.Add(new ProductCategoryModel(item.Id, item.Description, item.Url));
            }

            ProductGroupModel Result = new(GetModelData(),
                modelCategories, group.Description, group.TagLine);

            foreach (Product product in products)
            {
                Result.Products.Add(new ProductCategoryProductModel(product.Id, product.Name, product.Images[0],
                    group.Id, product.NewProduct, product.BestSeller, product.RetailPrice, product.Sku));
            }

            Result.Breadcrumbs.Clear();
            Result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.Home, "/", false));
            Result.Breadcrumbs.Add(new BreadcrumbItem(group.Description, $"/Products/{group.Id}/", false));

            Result.Pagination = BuildPagination(products.Count, (int)_settings.ProductsPerPage, page,
                $"/Products/{Result.RouteText(group.Description)}/{group.Id}/", "",
                LanguageStrings.Previous, LanguageStrings.Next);

            return Result;
        }

        private ProductModel GetProductModel(in int productId)
        {
            ProductModel Result;

            List<ProductCategoryModel> modelCategories = new();

            foreach (ProductGroup item in _productProvider.ProductGroupsGet())
            {
                modelCategories.Add(new ProductCategoryModel(item.Id, item.Description, item.Url));
            }

            Product product = _productProvider.GetProduct(productId);

            if (product == null)
                return null;

            _stockProvider.GetStockAvailability(product);

            if (_productProvider.ProductGroupGet(product.ProductGroupId) == null)
                return null;


            Result = new ProductModel(GetModelData(), modelCategories, product.Id, product.ProductGroupId,
                product.Name, product.Description, product.Features, product.VideoLink, GetImageNameArray(product),
                product.RetailPrice, product.Sku, product.NewProduct, product.BestSeller,
                _hasShoppingCart && product.RetailPrice > 0, product.StockAvailability);



            ProductGroup primaryProductGroup = _productProvider.ProductGroupGet(product.ProductGroupId);
            Result.Breadcrumbs.Clear();
            Result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.Home, "/", false));
            Result.Breadcrumbs.Add(new BreadcrumbItem(primaryProductGroup.Description,
                $"/Products/{primaryProductGroup.Id}/", false));
            Result.Breadcrumbs.Add(new BreadcrumbItem(product.Name,
                $"/Product/{product.Id}/{Result.RouteText(product.Name)}/", false));

            return Result;
        }

        private string[] GetImageNameArray(Product product)
        {
            List<Middleware.Images.ImageFile> images = _imageProvider.Images(ProductImageFolderName, product.Sku)
                    .Where(i => i.Name.Contains("_orig"))
                    .ToList();

            List<string> imageNames = new();
            images.ForEach(i => imageNames.Add(i.Name.Substring(0, i.Name.IndexOf("_orig"))));

            return imageNames.ToArray();
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591, IDE0060