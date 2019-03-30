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
 *  File: Cheque.cs
 *
 *  Purpose:  Cheque payment provider
 *
 *  Date        Name                Reason
 *  24/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Middleware;
using Middleware.Accounts.Orders;

using SharedPluginFeatures;

using Shared.Classes;

namespace ShoppingCartPlugin.Classes.PaymentProviders
{
    public sealed class Cheque : IPaymentProvider
    {
        #region Private Members

        private readonly PaymentProviderSettings _paymentProviderSettings;

        #endregion Private Members

        #region Constructors

        public Cheque(in ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _paymentProviderSettings = settingsProvider.GetSettings<PaymentProviderSettings>(nameof(Cheque));
        }

        #endregion Constructors

        #region IPaymentProvider Methods

        public bool Execute(in Order order, in PaymentStatus paymentStatus, in UserSession userSession,
            out string urlParameters)
        {
            urlParameters = $"/Cart/Success/{nameof(Cheque)}/";
            return true;
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
            return Middleware.Constants.PaymentProviderCheque;
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
