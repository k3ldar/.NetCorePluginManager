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
 *  File: PriceGroupProductCounts.cs
 *
 *  Purpose:  Product price information updates product counts
 *
 *  Date        Name                Reason
 *  30/03/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;

using Middleware;

using Shared.Classes;

using static SharedPluginFeatures.Constants;

namespace ProductPlugin.Classes
{
    internal class PriceGroupProductCounts : ThreadManager
    {
        #region Private Members

        private readonly IProductProvider _productProvider;

        #endregion Private Members

        #region Constructors

        public PriceGroupProductCounts(in IProductProvider productProvider, in List<ProductPriceInfo> productPriceInfo)
            : base(productPriceInfo, new TimeSpan())
        {
            if (productPriceInfo == null)
                throw new ArgumentNullException(nameof(productPriceInfo));

            _productProvider = productProvider ?? throw new ArgumentNullException(nameof(productProvider));

            base.ContinueIfGlobalException = false;
        }

        #endregion Constructors

        #region Overridden Methods

        protected override Boolean Run(object parameters)
        {
            List<ProductPriceInfo> productPriceInfo = (List<ProductPriceInfo>)parameters;

            foreach (ProductPriceInfo priceInfo in productPriceInfo)
            {
                int productPriceCount = _productProvider.GetProducts(1, MaximumProducts)
                    .Count(p => priceInfo.PriceMatch(p.RetailPrice));
                priceInfo.Text += $" ({productPriceCount})";
            }

            return false;
        }

        #endregion Overridden Methods
    }
}
