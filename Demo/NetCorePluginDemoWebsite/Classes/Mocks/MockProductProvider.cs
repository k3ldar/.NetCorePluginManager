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
 *  Product:  Demo Website
 *  
 *  File: MockProductProvider.cs
 *
 *  Purpose:  Mock IProductProvider for tesing purpose
 *
 *  Date        Name                Reason
 *  31/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Middleware;
using Middleware.Products;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
    [ExcludeFromCodeCoverage(Justification = "Code coverage not required for mock classes")]
    public class MockProductProvider : IProductProvider
    {
        #region IProductProvider Members

        #region Product Groups

        public ProductGroup ProductGroupGet(in int id)
        {
            int groupId = id;
            return ProductGroupsGet().Where(pg => pg.Id == groupId).FirstOrDefault();
        }

        public List<ProductGroup> ProductGroupsGet()
        {
            return new List<ProductGroup>()
            {
                new ProductGroup(1, "Main Products", "Main Products within our range", true, 1, "Checkout our main products", String.Empty),
                new ProductGroup(2, "Other Products", "Our other products", true, 2, "Checkout our other products", String.Empty)
            };
        }

        #endregion Product Groups

        #region Products

        public List<Product> GetProducts(in int page, in int pageSize)
        {
            return new List<Product>()
            {
                new Product(1, 1, "Product A & - &", "This is product a", "1 year guarantee", "", new string[] { "geoip" }, 0, "ProdA", false, false),
                new Product(2, 1, "Product B", "This is product b", "1 year guarantee", "", new string[] { "geoip" }, 0, "ProdB", true, false),
                new Product(3, 1, "Product C", "This is product c", "1 year guarantee", "E7Voso411Vs", new string[] { "geoip" }, 1.99m, "ProdC", true, true, false, true),
                new Product(4, 2, "Product D", "This is product d", "1 year guarantee", "", new string[] { "geoip" }, 22.99m, "ProdD", false, true, true, true),
                new Product(5, 2, "Product E", "This is product e", "1 year guarantee", "pCvZtjoRq1I", new string[] { "geoip" }, 0, "ProdE", false, false)
            };
        }

        public List<Product> GetProducts(in ProductGroup productGroup, in int page, in int pageSize)
        {
            ProductGroup prodGroup = productGroup;
            return GetProducts(page, pageSize).Where(p => p.ProductGroupId == prodGroup.Id).ToList();
        }

        public Product GetProduct(in int id)
        {
            int prodId = id;
            return GetProducts(1, 10000).Where(p => p.Id == prodId).FirstOrDefault();
        }

        #endregion Products

        #endregion IProductProvider Members
    }
}
