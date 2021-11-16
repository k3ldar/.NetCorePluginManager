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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Middleware;

using SharedPluginFeatures;

using SharedConstants = SharedPluginFeatures.Constants;

namespace ProductPlugin.Controllers
{
    /// <summary>
    /// Product Administration Controller
    /// </summary>
    public class ProductAdminController : BaseController
    {
        #region Private Members

        private readonly IProductProvider _productProvider;

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="productProvider">IProductProvider instance</param>
        public ProductAdminController(IProductProvider productProvider)
        {
            _productProvider = productProvider ?? throw new ArgumentNullException(nameof(productProvider));
        }

        #endregion Constructors

        //#region Product Group API

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
        [HttpGet]
        [Authorize(Roles = SharedConstants.PolicyNameApiAuthorization)]
        public IActionResult ProductGroupGet()
        {
            throw new NotImplementedException();
        }

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

        //#endregion Product Group API

    }
}
