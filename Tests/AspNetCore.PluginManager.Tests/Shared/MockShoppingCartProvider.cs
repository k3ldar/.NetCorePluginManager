using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Middleware;
using Middleware.Accounts.Orders;
using Middleware.Products;
using Middleware.ShoppingCart;

using Shared.Classes;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Shared
{
    class MockShoppingCartProvider : IShoppingCartProvider
    {

        public long AddToCart(in UserSession userSession, in ShoppingCartSummary shoppingCart, in Product product, in int count)
        {
            return 0;    
        }

        public bool ConvertToOrder(in ShoppingCartSummary cartSummary, in long userId, out Order order)
        {
            throw new NotImplementedException();
        }

        public ShoppingCartDetail GetDetail(in long shoppingCartId)
        {
            throw new NotImplementedException();
        }

        public bool ValidateVoucher(in ShoppingCartSummary cartSummary, in string voucher, in long userId)
        {
            throw new NotImplementedException();
        }
    }
}
