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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
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
    public sealed class Constants
    {
        public const string CaptchaCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public const string PaymentProviderCashOnDelivery = "Cash on Delivery";
        public const string PaymentProviderClickAndCollect = "Click and Collect";
        public const string PaymentProviderCheque = "Cheque";
        public const string PaymentProviderDirectTransfer = "Direct Transfer";
        public const string PaymentProviderPayflow = "Payflow";
        public const string PaymentProviderPaypoint = "Paypoint";
        public const string PaymentProviderPhone = "Phone";
        public const string PaymentProviderSunTech24Payment = "SunTech24Payment";

        public const string PaymentProviderNone = "There are no payment providers configured";
        public const string PaymentProviderNotFound = "Could not find payment provider";
    }
}
