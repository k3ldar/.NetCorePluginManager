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
 *  File: BasketModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  16/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace ShoppingCartPlugin.Models
{
    public class BasketModel : BaseModel
    {
        #region Constructors

        public BasketModel(in BaseModelData modelData,
            in List<BasketItemModel> cartItems, in string discountCode, in bool requiresShipping,
            in bool loggedIn)
            : base(modelData)
        {
            CartItems = cartItems ?? throw new ArgumentNullException(nameof(cartItems));
            DiscountCode = discountCode ?? String.Empty;
            RequiresShipping = requiresShipping;
            LoggedIn = loggedIn;
        }

        #endregion Constructors

        #region Properties

        public List<BasketItemModel> CartItems { get; private set; }

        public string DiscountCode { get; private set; }

        public bool RequiresShipping { get; private set; }

        public bool LoggedIn { get; private set; }

        public string DiscountDescription
        {
            get
            {
                if (CartSummary.Discount > 0)
                {
                    if (String.IsNullOrEmpty(DiscountCode))
                        return $"{CartSummary.DiscountRate}%";
                    else
                        return $"({DiscountCode} {CartSummary.DiscountRate}%)";
                }

                return String.Empty;
            }
        }

        #endregion Properties
    }
}

#pragma warning restore CS1591