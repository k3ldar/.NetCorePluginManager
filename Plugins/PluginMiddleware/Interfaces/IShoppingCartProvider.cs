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
 *  Copyright (c) 2012 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: IShoppingCartProvider.cs
 *
 *  Purpose:  User Account provider
 *
 *  Date        Name                Reason
 *  07/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Middleware.Accounts.Orders;
using Middleware.Products;
using Middleware.ShoppingCart;

using Shared.Classes;

using SharedPluginFeatures;

namespace Middleware
{
    /// <summary>
    /// Shopping cart provider interface, used to query shopping cart data.  
    /// 
    /// This item must be implemented by the host application and made available via DI.
    /// </summary>
    public interface IShoppingCartProvider
    {
        /// <summary>
        /// Retrieves detailed information on a shopping cart.
        /// </summary>
        /// <param name="shoppingCartId">Unique shopping cart id</param>
        /// <returns>ShoppingCartDetail</returns>
        ShoppingCartDetail GetDetail(in long shoppingCartId);

        /// <summary>
        /// Adds an individual product to a shopping cart.
        /// </summary>
        /// <param name="userSession">UserSession for user adding the item to a cart.</param>
        /// <param name="shoppingCart">The shopping cart the item is being added to.</param>
        /// <param name="product">Product item to be added to a cart.</param>
        /// <param name="count">Number of items to add.</param>
        /// <returns>long.  Unique id of the item within the shopping cart.</returns>
        long AddToCart(in UserSession userSession, in ShoppingCartSummary shoppingCart,
            in Product product, in int count);

        /// <summary>
        /// Validates a shopping cart voucher to determine its validity.
        /// </summary>
        /// <param name="cartSummary">Shoping cart against which the voucher should be validated against.</param>
        /// <param name="voucher">Voucher code being validated.</param>
        /// <param name="userId">Id of the user attempting to validate the voucher.</param>
        /// <returns>bool.  True if the voucher is valid.</returns>
        bool ValidateVoucher(in ShoppingCartSummary cartSummary, in string voucher, in long userId);

        /// <summary>
        /// Converts a shopping cart to an order.
        /// </summary>
        /// <param name="cartSummary">ShoppingCartSummary item to be converted to an order.</param>
        /// <param name="userId">User id who is checking out the shopping cart.</param>
        /// <param name="order">out.  Converted order.</param>
        /// <returns>bool.  True if the shopping cart was successfully converted to an order.</returns>
        bool ConvertToOrder(in ShoppingCartSummary cartSummary, in long userId, out Order order);
    }
}
