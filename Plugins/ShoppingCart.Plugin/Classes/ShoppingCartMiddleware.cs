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
 *  Copyright (c) 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Shopping Cart Plugin
 *  
 *  File: ShoppingCartMiddleware.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  11/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using SharedPluginFeatures;

using Shared.Classes;

using static Shared.Utilities;

namespace ShoppingCartPlugin
{
    public class ShoppingCartMiddleware : BaseMiddleware
    {
        #region Private Members

        private readonly RequestDelegate _next;
        private readonly IShoppingCartService _shoppingCartService;
        internal static Timings _timings = new Timings();

        #endregion Private Members

        #region Constructors

        public ShoppingCartMiddleware(RequestDelegate next, IShoppingCartService shoppingCartService)
        {
            _next = next;
            _shoppingCartService = shoppingCartService ?? throw new ArgumentNullException(nameof(shoppingCartService));

            IPluginHelperService pluginHelper = PluginClass.GetServiceProvider.GetService<IPluginHelperService>();

            if (pluginHelper == null || !pluginHelper.PluginLoaded(Constants.PluginNameUserSession, out int version))
                PluginClass.Logger.AddToLog(Enums.LogLevel.Error, 
                    new Exception(Constants.UserSessionServiceNotFound), 
                    nameof(ShoppingCartMiddleware));
        }

        #endregion Constructors

        #region Public Methods

        public async Task Invoke(HttpContext context)
        {
            using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timings))
            {
                UserSession userSession = GetUserSession(context);

                if (userSession != null)
                {
                    if (userSession.UserBasketId != 0 && !CookieExists(context, Constants.ShoppingCart))
                    {
                        CookieAdd(context, Constants.ShoppingCart,
                            Encrypt(userSession.UserBasketId.ToString(), _shoppingCartService.GetEncryptionKey()), 180);
                    }

                    if (userSession.UserBasketId == 0 && CookieExists(context, Constants.ShoppingCart))
                    {
                        try
                        {
                            string basketCookie = Decrypt(CookieValue(context, Constants.ShoppingCart, String.Empty), 
                                _shoppingCartService.GetEncryptionKey());

                            if (Int64.TryParse(basketCookie, out long result))
                                userSession.UserBasketId = result;
                            else
                                userSession.UserBasketId = 0;
                        }
                        catch (FormatException)
                        {
                            // problem decrypting the cookie so delete it
                            CookieDelete(context, Constants.ShoppingCart);
                        }
                    }

                    context.Items[Constants.BasketSummary] = GetBasketSummary(userSession.UserBasketId);
                }
            }

            await _next(context);
        }

        #endregion Public Methods

        #region Private Methods

        private ShoppingCartSummary GetBasketSummary(in long basketId)
        {
            if (basketId == 0)
                return new ShoppingCartSummary(0, 0, 0, System.Threading.Thread.CurrentThread.CurrentCulture);

            return _shoppingCartService.GetSummary(basketId);
        }

        #endregion Private Methods
    }
}
