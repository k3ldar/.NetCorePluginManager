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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockSession.cs
 *
 *  Purpose:  Mock IProductProvider class
 *
 *  Date        Name                Reason
 *  14/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Middleware;
using Middleware.Products;

namespace AspNetCore.PluginManager.Tests.Shared
{
    public sealed class MockProductProvider : IProductProvider
    {
        public Product GetProduct(in int id)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetProducts(in int page, in int pageSize)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetProducts(in ProductGroup productGroup, in int page, in int pageSize)
        {
            throw new NotImplementedException();
        }

        public ProductGroup ProductGroupGet(in int id)
        {
            throw new NotImplementedException();
        }

        public List<ProductGroup> ProductGroupsGet()
        {
            throw new NotImplementedException();
        }
    }
}
