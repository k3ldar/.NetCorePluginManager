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
 *  File: CashOnDelivery.cs
 *
 *  Purpose:  Cash on delivery payment provider
 *
 *  Date        Name                Reason
 *  24/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Microsoft.AspNetCore.Http;

using Middleware;
using Middleware.Accounts.Orders;

using SharedPluginFeatures;

using Shared.Classes;

using ShoppingCartPlugin.Classes.Paypoint;

namespace ShoppingCartPlugin.Classes.PaymentProviders
{
    /// <summary>
    /// Paypoint payment provider.  This payment provider is used when a user orders online and makes a payment using the online Paypoint payment service.
    /// 
    /// This class implements IPaymentProvider interface.
    /// </summary>
    public sealed class Paypoint : IPaymentProvider
    {
        #region Private Members

        private readonly PaypointSettings _paymentProviderSettings;

        #endregion Private Members

        #region Constructors

        public Paypoint(ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _paymentProviderSettings = settingsProvider.GetSettings<PaypointSettings>(nameof(Paypoint));
        }

        #endregion Constructors

        #region IPaymentProvider Methods

        public bool Execute(in HttpRequest request, in Order order, in PaymentStatus paymentStatus,
            in UserSession userSession, out string urlParameters)
        {
            urlParameters = $"/Cart/Failed/";

            if (order == null)
                throw new Exception("Invalid Order, can not find order during payment (Paypoint)");


            if (order.Total > 0.00m)
            {
                PaypointHelper vc = new PaypointHelper(order.Id.ToString(), order.Total, 
                    _paymentProviderSettings.Currencies.Split(';')[0], 
                    _paymentProviderSettings.MerchantId, _paymentProviderSettings.RemotePassword,
                    $"{request.Scheme}://{request.Host.Value}/Cart/Paypoint/");

                urlParameters = vc.GetURL();
                return true;
            }

            return false;
        }

        public bool ExecuteTest(in NVPCodec codec)
        {
            return false;
        }

        public string GetCurrencies()
        {
            return _paymentProviderSettings.Currencies;
        }

        public string Name()
        {
            return Middleware.Constants.PaymentProviderPaypoint;
        }

        public bool Enabled()
        {
            return _paymentProviderSettings.Enabled;
        }

        public Guid UniqueId()
        {
            return Guid.Parse(_paymentProviderSettings.UniqueId);
        }

        #endregion IPaymentProvider Methods
    }
}
