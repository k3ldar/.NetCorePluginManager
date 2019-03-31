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
 *  Copyright (c) 2012 - 2018 Simon Carter.  All Rights Reserved.
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

using Middleware.Accounts.Orders;

using Microsoft.AspNetCore.Http;

using Shared.Classes;

namespace Middleware
{
    public interface IPaymentProvider
    {
        /// <summary>
        /// Returns the supported currencies for the payment provider, seperated by ;
        /// 
        /// i.e.  GPB;USD  or GBP  or GBP;USD;EUR
        /// </summary>
        /// <returns>i.e.  GPB;USD  or GBP  or GBP;USD;EUR</returns>
        string GetCurrencies();

        /// <summary>
        /// Test Execute Method for debugging
        /// </summary>
        /// <returns>True if test passed, otherwise false</returns>
        bool ExecuteTest(in NVPCodec codec);

        /// <summary>
        /// Execute method, completes the payment using the interface specified
        /// </summary>
        /// <param name="order">Order being processed for payment</param>
        /// <param name="paymentStatus"></param>
        /// <param name="userSession"></param>
        /// <param name="context"></param>
        bool Execute(in HttpRequest request, in Order order, in PaymentStatus paymentStatus, 
            in UserSession userSession, out string urlParameters);

        /// <summary>
        /// Returns the name of the payment provider
        /// </summary>
        /// <returns></returns>
        string Name();

        /// <summary>
        /// Determines wether the payment provider is active or not
        /// </summary>
        /// <returns></returns>
        bool Enabled();

        /// <summary>
        /// Unique Id for payment provider
        /// </summary>
        /// <returns></returns>
        Guid UniqueId();
    }
}
