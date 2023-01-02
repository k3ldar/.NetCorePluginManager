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
 *  Product:  SharedPluginFeatures
 *  
 *  File: IShoppingCartService.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  11/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SharedPluginFeatures
{
    /// <summary>
    /// This interface should be implemented by the host application or a type of middleware or
    /// business object layer and should be available to the ShoppingCart.Plugin via DI.
    /// </summary>
    public interface IShoppingCartService
    {
        /// <summary>
        /// Retrieves a summary for the specified cart id.
        /// </summary>
        /// <param name="shoppingCartId">ShoppingCartId, unique value identifying a customers shopping cart.</param>
        /// <returns>ShoppingCartSummary</returns>
        ShoppingCartSummary GetSummary(in long shoppingCartId);

        /// <summary>
        /// Retrieves the key that is used to encrypt and decrypt the shopping cart data held
        /// within a users cookie.
        /// </summary>
        /// <returns>string</returns>
        string GetEncryptionKey();
    }
}
