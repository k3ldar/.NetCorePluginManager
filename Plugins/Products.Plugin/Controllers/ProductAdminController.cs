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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Products.Plugin
 *  
 *  File: ProductAdminController.cs
 *
 *  Purpose:  Product Admin Controller
 *
 *  Date        Name                Reason
 *  31/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Languages;

using Microsoft.AspNetCore.Mvc;

using Middleware;
using Middleware.Products;

using PluginManager.Abstractions;

using ProductPlugin.Models;

using SharedPluginFeatures;

using static SharedPluginFeatures.Constants;

#pragma warning disable CS1591

namespace ProductPlugin.Controllers
{
    /// <summary>
    /// Product Administration Controller
    /// </summary>
    public class ProductAdminController : BaseController
    {
        public const string Name = "ProductAdmin";

        #region Private Members

        private const string InvalidProductModel = "model no good";
        private const string ProductNotFound = "Invalid product";

        private readonly IProductProvider _productProvider;
        private readonly ProductPluginSettings _settings;
        private readonly IMemoryCache _memoryCache;

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="productProvider">IProductProvider instance</param>
        /// <param name="settingsProvider">ISettingsProvider instance</param>
        /// <param name="memoryCache"></param>
        public ProductAdminController(IProductProvider productProvider, ISettingsProvider settingsProvider, IMemoryCache memoryCache)
        {
            _productProvider = productProvider ?? throw new ArgumentNullException(nameof(productProvider));

            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _settings = settingsProvider.GetSettings<ProductPluginSettings>(ProductController.Name);
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(_memoryCache));
        }

        #endregion Constructors

        /// <summary>
        /// Default controller entry point, provides a page of data for viewing
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/ProductAdmin/Page/{page}/")]
        [Route("/ProductAdmin/Index/")]
        public IActionResult Index(int? page)
        {
            ProductPageListModel model = GetPageList(page.GetValueOrDefault(1));
            model.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.AppProductsAdministration, "/ProductAdmin/Index", false));

            if (page.HasValue)
                model.Breadcrumbs.Add(new BreadcrumbItem($"{LanguageStrings.Page} {page.Value}", $"/ProductAdmin/Page/{page.Value}", false));

            return View(model);
        }

        [HttpGet]
        public IActionResult EditProduct(int id, int pageNumber)
        {
            Product product = _productProvider.GetProduct(id);

            if (product == null)
                return RedirectToAction(nameof(Index));

            EditProductModel model = CreateEditProductModel(product);

            if (pageNumber > 1)
                model.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.AppProductsAdministration, $"/ProductAdmin/Page/{pageNumber}/", false));
            else
                model.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.AppProductsAdministration, "/ProductAdmin/Index/", false));

            model.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.AppMenuEditProduct, $"/ProductAdmin/EditProduct/{id}/{pageNumber}", false));

            return View(model);
        }

        [HttpGet]
        public IActionResult NewProduct()
        {
            return View("/Views/ProductAdmin/EditProduct.aspx", new EditProductModel(GetModelData()));
        }

        [HttpPost]
        public IActionResult SaveProduct(EditProductModel model)
        {
            if (model == null)
                return RedirectToAction(nameof(Index));

            if (String.IsNullOrEmpty(model.Name))
                ModelState.AddModelError(nameof(model.Name), LanguageStrings.AppErrorInvalidProductName);

            if (String.IsNullOrEmpty(model.Description))
                ModelState.AddModelError(nameof(model.Description), LanguageStrings.AppErrorInvalidProductDescription);

            if (model.RetailPrice < 0)
                ModelState.AddModelError(nameof(model.RetailPrice), LanguageStrings.AppErrorInvalidProductPrice);

            if (String.IsNullOrEmpty(model.Sku))
                ModelState.AddModelError(nameof(model.Sku), LanguageStrings.AppErrorInvalidProductSku);

            if (_productProvider.ProductGroupGet(model.ProductGroupId) == null)
                ModelState.AddModelError(nameof(model.ProductGroupId), LanguageStrings.AppProductSaveNoPrimaryGroup);

            // product provider can have it's own rules and fail to save at this point
            if (!_productProvider.ProductSave(model.Id, model.ProductGroupId, model.Name, model.Description, model.Features, model.VideoLink,
                model.NewProduct, model.BestSeller, model.RetailPrice, model.Sku, model.IsDownload, model.AllowBackorder, out string errorMessage))
            {
                ModelState.AddModelError(String.Empty, errorMessage);
            }

            if (!ModelState.IsValid)
                return View("/Views/ProductAdmin/EditProduct.aspx", model);

            _memoryCache.GetShortCache().Clear();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("/ProductAdmin/ViewDeleteProduct/{productId}/")]
        public IActionResult ViewDeleteProduct(int productId)
        {
            if (_productProvider.GetProduct(productId) == null)
                return RedirectToAction(nameof(Index));

            return PartialView("_ShowDeleteProduct", new ProductDeleteModel(productId));
        }

        [HttpPost]
        [AjaxOnly]
        public JsonResult DeleteProduct(ProductDeleteModel model)
        {
            if (model == null)
                return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidProductModel);

            if (_productProvider.GetProduct(model.Id) == null)
                return GenerateJsonErrorResponse(HtmlResponseBadRequest, ProductNotFound);

            if (String.IsNullOrEmpty(model.Confirmation) || !model.Confirmation.Equals("CONFIRM", StringComparison.InvariantCultureIgnoreCase))
            {
                return GenerateJsonErrorResponse(HtmlResponseBadRequest, LanguageStrings.ConfirmDeleteWord);
            }
            else if (!_productProvider.ProductDelete(model.Id, out string errorMessage))
            {
                // product provider can have it's own rules and fail to delete at this point
                return GenerateJsonErrorResponse(HtmlResponseBadRequest, errorMessage);
            }

            _memoryCache.GetShortCache().Clear();

            return GenerateJsonSuccessResponse();
        }

        private ProductPageListModel GetPageList(int page)
        {
            int pageSize = (int)_settings.ProductsPerPage;

            List<Product> pageProducts = _productProvider.GetProducts(page, pageSize);

            string pagination = BuildPagination(_productProvider.ProductCount, (int)_settings.ProductsPerPage, page,
                $"/ProductAdmin/", "",
                LanguageStrings.Previous, LanguageStrings.Next);

            return new ProductPageListModel(GetModelData(), pageProducts, pagination, page);
        }

        private EditProductModel CreateEditProductModel(Product product)
        {
            List<LookupListItem> productGroups = new List<LookupListItem>();

            _productProvider.ProductGroupsGet().ForEach(pg => productGroups.Add(new LookupListItem(pg.Id, pg.Description)));

            return new EditProductModel(GetModelData(), productGroups, product.Id, product.ProductGroupId,
                product.Name, product.Description, product.Features, product.VideoLink, product.NewProduct,
                product.BestSeller, product.RetailPrice, product.Sku, product.IsDownload, product.AllowBackorder);
        }

        #region Product Group API

        ///// <summary>
        ///// Creates a ProductGroup item
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public IActionResult ProductGroupCreate(ProductGroupModel model)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Gets a ProductGroup item
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //[ApiAuthorization(SharedConstants.PolicyNameManageProducts)]
        //public IActionResult ProductGroupGet()
        //{
        //    throw new NotImplementedException();
        //}

        ///// <summary>
        ///// Updates a ProductGroup item
        ///// </summary>
        ///// <returns></returns>
        //[HttpPut]
        //public IActionResult ProductGroupUpdate()
        //{
        //    throw new NotImplementedException();
        //}

        ///// <summary>
        ///// Deletes a ProductGroup item
        ///// </summary>
        ///// <returns></returns>
        //[HttpDelete]
        //public IActionResult ProductGroupDelete()
        //{
        //    throw new NotImplementedException();
        //}

        //#endregion Product Group API

        //#region Product Group API

        ///// <summary>
        ///// Creates a ProductGroup item
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public IActionResult ProductCreate()
        //{
        //    throw new NotImplementedException();
        //}

        ///// <summary>
        ///// Gets a ProductGroup item
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public IActionResult ProductGet()
        //{
        //    throw new NotImplementedException();
        //}

        ///// <summary>
        ///// Updates a ProductGroup item
        ///// </summary>
        ///// <returns></returns>
        //[HttpPut]
        //public IActionResult ProductUpdate()
        //{
        //    throw new NotImplementedException();
        //}

        ///// <summary>
        ///// Deletes a ProductGroup item
        ///// </summary>
        ///// <returns></returns>
        //[HttpDelete]
        //public IActionResult ProductDelete()
        //{
        //    throw new NotImplementedException();
        //}

        #endregion Product Group API
    }
}

#pragma warning restore CS1591