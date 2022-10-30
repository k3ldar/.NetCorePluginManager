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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: Constants.cs
 *
 *  Purpose:  Shared Middleware Constant Values
 *
 *  Date        Name                Reason
 *  16/12/2018  Simon Carter        Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace Middleware
{
    /// <summary>
    /// Constant values used by PluginMiddleware
    /// </summary>
    public sealed class Constants
    {
        /// <summary>
        /// Captcha characters that will be used to generate captcha requests.
        /// </summary>
        /// <value>string</value>
        public const string CaptchaCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Cash on delivery payment provider.
        /// </summary>
        /// <value>string</value>
        public const string PaymentProviderCashOnDelivery = "Cash on Delivery";

        /// <summary>
        /// Click and collect payment provider.
        /// </summary>
        /// <value>string</value>
        public const string PaymentProviderClickAndCollect = "Click and Collect";

        /// <summary>
        /// Cheque payment provider.
        /// </summary>
        /// <value>string</value>
        public const string PaymentProviderCheque = "Cheque";

        /// <summary>
        /// Direct bank transfer payment provider.
        /// </summary>
        /// <value>string</value>
        public const string PaymentProviderDirectTransfer = "Direct Transfer";

        /// <summary>
        /// Payflow payment provider.
        /// </summary>
        /// <value>string</value>
        public const string PaymentProviderPayflow = "Payflow";

        /// <summary>
        /// Paypoint payment provider.
        /// </summary>
        /// <value>string</value>
        public const string PaymentProviderPaypoint = "Paypoint";

        /// <summary>
        /// Phone payment provider.
        /// </summary>
        /// <value>string</value>
        public const string PaymentProviderPhone = "Phone";

        /// <summary>
        /// SunTech24 payment provider.
        /// </summary>
        /// <value>string</value>
        public const string PaymentProviderSunTech24Payment = "SunTech24Payment";


        /// <summary>
        /// No payment providers configured.
        /// </summary>
        /// <value>string</value>
        public const string PaymentProviderNone = "There are no payment providers configured";

        /// <summary>
        /// Invalid payment provider.
        /// </summary>
        /// <value>string</value>
        public const string PaymentProviderNotFound = "Could not find payment provider";
    }
}
