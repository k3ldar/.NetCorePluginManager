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
 *  Copyright (c) 2019 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Shopping Cart Plugin
 *  
 *  File: DirectTransfer.cs
 *
 *  Purpose:  Direct bank transfer payment provider
 *
 *  Date        Name                Reason
 *  24/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Microsoft.AspNetCore.Http;

using Middleware;
using Middleware.Accounts.Orders;

using PluginManager.Abstractions;

using Shared.Classes;

#pragma warning disable CS1591

namespace ShoppingCartPlugin.Classes.PaymentProviders
{
    /// <summary>
    /// Direct transfer payment provider.  This payment provider is used when a user orders online, but then makes a payment by directly transferring funds to a nominated bank account.
    /// 
    /// This class implements IPaymentProvider interface.
    /// </summary>
    public sealed class DirectTransfer : IPaymentProvider
    {
        #region Private Members

        private readonly PaymentProviderSettings _paymentProviderSettings;

        #endregion Private Members

        #region Constructors

        public DirectTransfer(ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _paymentProviderSettings = settingsProvider.GetSettings<PaymentProviderSettings>(nameof(DirectTransfer));
        }

        #endregion Constructors

        #region IPaymentProvider Methods

        public bool Execute(in HttpRequest request, in Order order, in PaymentStatus paymentStatus,
            in UserSession userSession, out string urlParameters)
        {
            urlParameters = $"/Cart/Success/{UniqueId()}/";
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
            return Middleware.Constants.PaymentProviderDirectTransfer;
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

#pragma warning restore CS1591