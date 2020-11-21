﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  File: PaypointSettings.cs
 *
 *  Purpose:  Settings for paypoint provider
 *
 *  Date        Name                Reason
 *  24/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using AppSettings;

namespace ShoppingCartPlugin.Classes
{
    /// <summary>
    /// Settings related specifically to Paypoint payment provider.
    /// </summary>
    public sealed class PaypointSettings : PaymentProviderSettings
    {
        /// <summary>
        /// Merchant Id
        /// </summary>
        [SettingString(false, 1, 100)]
        public string MerchantId { get; set; }

        /// <summary>
        /// Remote password.
        /// </summary>
        [SettingString(false, 1, 100)]
        public string RemotePassword { get; set; }
    }
}
