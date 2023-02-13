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

using Microsoft.AspNetCore.Authorization;
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
    [DenySpider]
    [LoggedIn]
    [Authorize(Policy = PolicyNameManageProducts)]
    public class ProductAdminController : BaseController
    {
        public const string Name = "ProductAdmin";

        #region Private Members

        private const string InvalidModel = "Invalid model";
        private const string ProductNotFound = "Invalid product";
        private const string ProductGroupNotFound = "Invalid product group";
		private const string ProductQuantityMustBeAtLeastOne = "Product quantity must be at least one";

        private readonly IProductProvider _productProvider;
		private readonly IStockProvider _stockProvider;
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
        public ProductAdminController(IProductProvider productProvider, ISettingsProvider settingsProvider, IStockProvider stockProvider, IMemoryCache memoryCache)
        {
            _productProvider = productProvider ?? throw new ArgumentNullException(nameof(productProvider));
			_stockProvider = stockProvider ?? throw new ArgumentNullException(nameof(stockProvider));

            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _settings = settingsProvider.GetSettings<ProductPluginSettings>(ProductController.Name);
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
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
            model.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.SystemAdmin, "/SystemAdmin/Index", false));
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

            EditProductModel model = CreateEditProductModel(product, pageNumber);

            return View(model);
        }

        [HttpGet]
        public IActionResult NewProduct()
        {
            return View("/Views/ProductAdmin/EditProduct.cshtml", CreateNewProductModel());
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
                model.NewProduct, model.BestSeller, model.RetailPrice, model.Sku, model.IsDownload, model.AllowBackorder,
				model.IsVisible, out string errorMessage))
            {
                ModelState.AddModelError(String.Empty, errorMessage);
            }

            if (!ModelState.IsValid)
            {
                return View("/Views/ProductAdmin/EditProduct.cshtml", CreateEditProductModel(model));
            }

            _memoryCache.GetShortCache().Clear();

            return RedirectToAction(nameof(Index), new { page = model.PageNumber });
        }

        [HttpGet]
        [Route("/ProductAdmin/ViewDeleteProduct/{productId}/")]
        [AjaxOnly]
        public IActionResult ViewDeleteProduct(int productId)
        {
            if (_productProvider.GetProduct(productId) == null)
                return GenerateJsonErrorResponse(HtmlResponseBadRequest, ProductNotFound);

            return PartialView("_ShowDeleteProduct", new ProductDeleteModel(productId));
        }

        [HttpPost]
        [AjaxOnly]
        public JsonResult DeleteProduct(ProductDeleteModel model)
        {
            if (model == null)
                return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidModel);

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

		[HttpGet]
		[Route("/ProductAdmin/ViewAddProductStock/{productId}/")]
		[AjaxOnly]
		public IActionResult ViewAddProductStock(int productId)
		{
			Product product = _productProvider.GetProduct(productId);

			if (product == null)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, ProductNotFound);

			return PartialView("_ShowAddProductStock", new ProductAddStockModel(productId, product.Name));
		}

		[HttpPost]
		[AjaxOnly]
		public JsonResult AddStockToProduct(ProductAddStockModel model)
		{
			if (model == null)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidModel);

			Product product = _productProvider.GetProduct(model.Id);

			if (product == null)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, ProductNotFound);

			if (model.Quantity < 1)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, ProductQuantityMustBeAtLeastOne);

			if (!_stockProvider.AddStockToProduct(product, model.Quantity, out string error))
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, error);

			_memoryCache.GetShortCache().Clear();

			return GenerateJsonSuccessResponse();
		}

		#region Product Groups

		[HttpGet]
        public IActionResult GroupIndex()
        {
            return View(CreateProductGroupListModel());
        }

        [HttpGet]
        [Route("/ProductAdmin/ViewDeleteProductGroup/{productGroupId}/")]
        [AjaxOnly]
        public IActionResult ViewDeleteProductGroup(int productGroupId)
        {
            if (_productProvider.ProductGroupGet(productGroupId) == null)
                return GenerateJsonErrorResponse(HtmlResponseBadRequest, ProductGroupNotFound);

            return PartialView("_ShowDeleteProductGroup", new ProductGroupDeleteModel(productGroupId));
        }

        [HttpPost]
        [AjaxOnly]
        public JsonResult DeleteProductGroup(ProductGroupDeleteModel model)
        {
            if (model == null)
                return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidModel);

            ProductGroup productGroup = _productProvider.ProductGroupGet(model.Id);

            if (productGroup == null)
                return GenerateJsonErrorResponse(HtmlResponseBadRequest, ProductGroupNotFound);


            if (String.IsNullOrEmpty(model.Confirmation) || !model.Confirmation.Equals("CONFIRM", StringComparison.InvariantCultureIgnoreCase))
            {
                return GenerateJsonErrorResponse(HtmlResponseBadRequest, LanguageStrings.ConfirmDeleteWord);
            }
            else if (!_productProvider.ProductGroupDelete(model.Id, out string errorMessage))
            {
                // product provider can have it's own rules and fail to delete at this point
                return GenerateJsonErrorResponse(HtmlResponseBadRequest, errorMessage);
            }

            _memoryCache.GetShortCache().Clear();

            return GenerateJsonSuccessResponse();
        }

        [HttpGet]
        public IActionResult EditProductGroup(int id)
        {
            ProductGroup productGroup = _productProvider.ProductGroupGet(id);

            if (productGroup == null)
                return RedirectToAction(nameof(GroupIndex));

            EditProductGroupModel model = CreateEditProductGroupModel(productGroup);

            return View(model);
        }

        [HttpGet]
        public IActionResult NewProductGroup()
        {
            return View("/Views/ProductAdmin/EditProductGroup.cshtml", CreateNewProductGroupModel());
        }

        [HttpPost]
        public IActionResult SaveProductGroup(EditProductGroupModel model)
        {
            if (model == null)
                return RedirectToAction(nameof(GroupIndex));

            if (String.IsNullOrEmpty(model.Description))
                ModelState.AddModelError(nameof(model.Description), LanguageStrings.AppErrorInvalidProductGroupDescription);

            // product provider can have it's own rules and fail to save at this point
            if (!_productProvider.ProductGroupSave(model.Id, model.Description, model.ShowOnWebsite,
                model.SortOrder, model.TagLine, model.Url, out string errorMessage))
            {
                ModelState.AddModelError(String.Empty, errorMessage);
            }

            if (!ModelState.IsValid)
            {
                return View("/Views/ProductAdmin/EditProductGroup.cshtml", CreateEditProductGroupModel(model));
            }

            _memoryCache.GetShortCache().Clear();

            return RedirectToAction(nameof(GroupIndex));
        }

        #endregion Product Groups

        #region Private Methods

        private EditProductGroupModel CreateNewProductGroupModel()
        {
            EditProductGroupModel result = new EditProductGroupModel(GetModelData());

            result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.SystemAdmin, "/SystemAdmin/Index", false));
            result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.AppProductGroups, "/ProductAdmin/GroupIndex/", false));
            result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.AppMenuNewProductGroup, $"/ProductAdmin/EditProductGroup/-1", false));

            return result;
        }

        private EditProductGroupModel CreateEditProductGroupModel(EditProductGroupModel productGroupModel)
        {
            EditProductGroupModel result = new EditProductGroupModel(GetModelData(), productGroupModel.Id, productGroupModel.Description,
                productGroupModel.ShowOnWebsite, productGroupModel.SortOrder, productGroupModel.TagLine, productGroupModel.Url);
            
            result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.SystemAdmin, "/SystemAdmin/Index", false));
            result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.AppProductGroups, "/ProductAdmin/GroupIndex/", false));
            result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.AppMenuEditProductGroup, $"/ProductAdmin/EditProductGroup/{productGroupModel.Id}", false));

            return result;
        }

        private EditProductGroupModel CreateEditProductGroupModel(ProductGroup productGroup)
        {
            EditProductGroupModel result = new EditProductGroupModel(GetModelData(), productGroup.Id, productGroup.Description,
                productGroup.ShowOnWebsite, productGroup.SortOrder, productGroup.TagLine, productGroup.Url);

            result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.SystemAdmin, "/SystemAdmin/Index", false));
            result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.AppProductGroups, "/ProductAdmin/GroupIndex/", false));
            result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.AppMenuEditProductGroup, $"/ProductAdmin/EditProductGroup/{productGroup.Id}", false));

            return result;
        }

        private ProductGroupListModel CreateProductGroupListModel()
        {
            ProductGroupListModel result = new ProductGroupListModel(GetModelData());

            List<ProductGroup> groups = _productProvider.ProductGroupsGet();

            groups.ForEach(g => result.Groups.Add(new LookupListItem(g.Id, g.Description)));

            result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.SystemAdmin, "/SystemAdmin/Index", false));
            result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.AppProductGroups, $"/ProductAdmin/{nameof(GroupIndex)}", false));

            return result;
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

        private EditProductModel CreateNewProductModel()
        {
            List<ProductGroup> allProductGroups = _productProvider.ProductGroupsGet();

            EditProductModel result = new EditProductModel(GetModelData())
            {
                ProductGroupId = allProductGroups[0].Id
            };

            allProductGroups.ForEach(pg => result.ProductGroups.Add(new LookupListItem(pg.Id, pg.Description)));

            result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.SystemAdmin, "/SystemAdmin/Index", false));
            result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.AppProductsAdministration, "/ProductAdmin/Index/", false));
            result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.AppMenuNewProduct, $"/ProductAdmin/EditProduct/-1", false));


            return result;
        }

        private EditProductModel CreateEditProductModel(Product product, int pageNumber)
        {
            List<LookupListItem> productGroups = new List<LookupListItem>();

            _productProvider.ProductGroupsGet().ForEach(pg => productGroups.Add(new LookupListItem(pg.Id, pg.Description)));

            EditProductModel result = new EditProductModel(GetModelData(), productGroups, product.Id, product.ProductGroupId,
                product.Name, product.Description, product.Features, product.VideoLink, product.NewProduct,
                product.BestSeller, product.RetailPrice, product.Sku, product.IsDownload, product.AllowBackorder,
                product.IsVisible, pageNumber);

            result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.SystemAdmin, "/SystemAdmin/Index", false));

            if (pageNumber > 1)
                result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.AppProductsAdministration, $"/ProductAdmin/Page/{pageNumber}/", false));
            else
                result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.AppProductsAdministration, "/ProductAdmin/Index/", false));

            result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.AppMenuEditProduct, $"/ProductAdmin/EditProduct/{product.Id}/{pageNumber}", false));


            return result;
        }

        private EditProductModel CreateEditProductModel(EditProductModel model)
        {
            List<LookupListItem> productGroups = new List<LookupListItem>();

            _productProvider.ProductGroupsGet().ForEach(pg => productGroups.Add(new LookupListItem(pg.Id, pg.Description)));

            EditProductModel result = new EditProductModel(GetModelData(), productGroups, model.Id, model.ProductGroupId,
                model.Name, model.Description, model.Features, model.VideoLink, model.NewProduct,
                model.BestSeller, model.RetailPrice, model.Sku, model.IsDownload, model.AllowBackorder,
				model.IsVisible, model.PageNumber);

            result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.SystemAdmin, "/SystemAdmin/Index", false));

            if (model.PageNumber > 1)
                result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.AppProductsAdministration, $"/ProductAdmin/Page/{model.PageNumber}/", false));
            else
                result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.AppProductsAdministration, "/ProductAdmin/Index/", false));

            result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.AppMenuEditProduct, $"/ProductAdmin/EditProduct/{model.ProductGroupId}/{model.PageNumber}", false));

            return result;
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591