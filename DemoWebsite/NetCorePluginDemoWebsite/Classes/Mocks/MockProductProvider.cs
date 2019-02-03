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
 *  Product:  Login Plugin
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
using System.Linq;

using Middleware;
using Middleware.Products;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
    public class MockProductProvider : IProductProvider
    {
        #region IProductProvider Members

        #region Product Groups

        public ProductGroup ProductGroupGet(int id)
        {
            return ProductGroupsGet().Where(pg => pg.Id == id).FirstOrDefault();
        }

        public IEnumerable<ProductGroup> ProductGroupsGet()
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
                new Product(1, 1, "Product A", "This is product a", "1 year guarantee", "aBdkiff", new string[] { "geoip" }, 0),
                new Product(2, 1, "Product B", "This is product b", "1 year guarantee", "aBdkiff", new string[] { "geoip" }, 0),
                new Product(3, 1, "Product C", "This is product c", "1 year guarantee", "aBdkiff", new string[] { "geoip" }, 1.99m),
                new Product(4, 2, "Product D", "This is product d", "1 year guarantee", "aBdkiff", new string[] { "geoip" }, 2.99m),
                new Product(5, 2, "Product E", "This is product e", "1 year guarantee", "aBdkiff", new string[] { "geoip" }, 0)
            };
        }

        public List<Product> GetProducts(ProductGroup productGroup, in int page, in int pageSize)
        {
            return GetProducts(page, pageSize).Where(p => p.ProductGroupId == productGroup.Id).ToList();
        }

        public Product GetProduct(int id)
        {
            return GetProducts(1, 10000).Where(p => p.Id == id).FirstOrDefault();
        }

        #endregion Products

        #endregion IProductProvider Members
    }
}
