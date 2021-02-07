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
 *  Copyright (c) 2012 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: IPaymentProvider.cs
 *
 *  Purpose:  User Account provider
 *
 *  Date        Name                Reason
 *  07/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Microsoft.AspNetCore.Http;

using Middleware.Accounts.Orders;

using Shared.Classes;

namespace Middleware
{
    /// <summary>
    /// PaymentProvider instance, this interface represents an individual payment provider against which
    /// a shopping cart could be checked out against.
    /// 
    /// This item must be implemented by the host application and made available via DI.
    /// </summary>
    public interface IPaymentProvider
    {
        /// <summary>
        /// Returns the supported currencies for the payment provider, seperated by ;
        /// 
        /// i.e.  GPB;USD  or GBP  or GBP;USD;EUR
        /// </summary>
        /// <returns>string</returns>
        string GetCurrencies();

        /// <summary>
        /// Test Execute Method for debugging purposes.
        /// </summary>
        /// <returns>bool.  True if the test passed.</returns>
        bool ExecuteTest(in NVPCodec codec);

        /// <summary>
        /// Execute method, completes the payment using the interface specified
        /// </summary>
        /// <param name="request">Current HttpRequest</param>
        /// <param name="order">Order who's payment attempt is being made against.</param>
        /// <param name="paymentStatus">Current PaymentStatus</param>
        /// <param name="userSession">UserSession for the user who is making the payment.</param>
        /// <param name="urlParameters">Url where the user is to be redirected to, so that the order can be completed.</param>
        /// <returns>bool.  True if the payment was successfully executed.  This could vary depending on the payment provider in question.</returns>
        bool Execute(in HttpRequest request, in Order order, in PaymentStatus paymentStatus,
            in UserSession userSession, out string urlParameters);

        /// <summary>
        /// Returns the name of the payment provider.
        /// </summary>
        /// <returns>string.  Unique name of payment provider.</returns>
        string Name();

        /// <summary>
        /// Determines wether the payment provider is active or not
        /// </summary>
        /// <returns>bool.  True if the payment provider is ok to use.</returns>
        bool Enabled();

        /// <summary>
        /// Unique Id for payment provider
        /// </summary>
        /// <returns>Guid</returns>
        Guid UniqueId();
    }
}
