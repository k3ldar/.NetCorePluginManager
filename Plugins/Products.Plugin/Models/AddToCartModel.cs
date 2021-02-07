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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Products.Plugin
 *  
 *  File: AddToCartModel.cs
 *
 *  Purpose:  Add to cart model
 *
 *  Date        Name                Reason
 *  02/02/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

#pragma warning disable CS1591

namespace ProductPlugin.Models
{
    public class AddToCartModel
    {
        #region Constructors

        public AddToCartModel()
        {
            Quantity = 1;
        }

        public AddToCartModel(in int id, in decimal retailPrice, in decimal discount, in uint stockAvailability)
            : this()
        {
            if (retailPrice <= 0)
                throw new ArgumentOutOfRangeException(nameof(retailPrice));

            if (discount < 0 || discount > 100)
                throw new ArgumentOutOfRangeException(nameof(discount));

            Id = id;
            RetailPrice = retailPrice;
            Discount = discount;
            StockAvailability = stockAvailability;
        }

        #endregion Constructors

        #region Properties

        public int Id { get; set; }

        public int Quantity { get; set; }

        public decimal RetailPrice { get; set; }

        public decimal Discount { get; set; }

        public uint StockAvailability { get; private set; }

        #endregion Properties
    }
}

#pragma warning restore CS1591