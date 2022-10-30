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
 *  File: PaymentSuccessModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  30/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace ShoppingCartPlugin.Models
{
    public class PaymentSuccessModel : BaseModel
    {
        #region Constructors

        public PaymentSuccessModel(in BaseModelData modelData, int orderId)
            : base(modelData)
        {
            if (orderId == 0)
                throw new ArgumentOutOfRangeException(nameof(orderId));

            OrderId = orderId;
        }

        #endregion Constructors

        #region Properties

        public int OrderId { get; private set; }

        #endregion Properties
    }
}

#pragma warning restore CS1591