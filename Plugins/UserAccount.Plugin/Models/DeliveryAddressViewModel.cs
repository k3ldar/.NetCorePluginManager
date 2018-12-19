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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  UserAccount.Plugin
 *  
 *  File: DeliveryAddressViewModel.cs
 *
 *  Purpose:  Delivery Address view model
 *
 *  Date        Name                Reason
 *  16/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Middleware;

namespace UserAccount.Plugin.Models
{
    public sealed class DeliveryAddressViewModel
    {
        #region Constructors

        public DeliveryAddressViewModel()
        {

        }

        public DeliveryAddressViewModel(List<DeliveryAddress> addresses)
        {
            Addresses = addresses ?? throw new ArgumentNullException(nameof(addresses));
        }

        public DeliveryAddressViewModel(List<DeliveryAddress> addresses, in string growlMessage)
        {
            Addresses = addresses ?? throw new ArgumentNullException(nameof(addresses));
            GrowlMessage = growlMessage ?? throw new ArgumentNullException(nameof(growlMessage));
        }

        #endregion Constructors

        #region Properties

        public List<DeliveryAddress> Addresses { get; set; }

        public string GrowlMessage { get; set; }

        #endregion Properties
    }
}
