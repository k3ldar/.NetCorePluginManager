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

        private readonly IProductProvider _productProvider;
        private readonly ProductPluginSettings _settings;

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="productProvider">IProductProvider instance</param>
        /// <param name="settingsProvider">ISettingsProvider instance</param>
        public ProductAdminController(IProductProvider productProvider, ISettingsProvider settingsProvider)
        {
            _productProvider = productProvider ?? throw new ArgumentNullException(nameof(productProvider));

            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _settings = settingsProvider.GetSettings<ProductPluginSettings>(ProductController.Name);

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
            return View(GetPageList(page.GetValueOrDefault(1)));
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult NewProduct()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult SaveProduct()
        {
            throw new NotImplementedException();
        }

        private ProductPageListModel GetPageList(int page)
        {
            int pageSize = (int)_settings.ProductsPerPage;

            List<Product> pageProducts = _productProvider.GetProducts(page, pageSize);

            string pagination = BuildPagination(_productProvider.ProductCount, (int)_settings.ProductsPerPage, page,
                $"/ProductAdmin/", "",
                LanguageStrings.Previous, LanguageStrings.Next);

            return new ProductPageListModel(GetModelData(), pageProducts, pagination);
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