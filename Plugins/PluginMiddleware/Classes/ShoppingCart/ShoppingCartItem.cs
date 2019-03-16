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
 *  Copyright (c) 2012 - 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: ShoppingCartItem.cs
 *
 *  Purpose:  User Account provider
 *
 *  Date        Name                Reason
 *  07/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace Middleware.ShoppingCart
{
    public sealed class ShoppingCartItem
    {
        #region Constructors

        private ShoppingCartItem()
        {
            Weight = 0;
            TaxRate = 0;
            CustomerReference = String.Empty;
        }

        public ShoppingCartItem(in int id, in decimal itemCount, in decimal itemCost, 
            in string description, in string sku, in string image, in bool isDownload)
            :this ()
        {
            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));

            if (String.IsNullOrEmpty(image))
                throw new ArgumentNullException(nameof(image));

            if (itemCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(itemCount));

            if (itemCost < 0)
                throw new ArgumentOutOfRangeException(nameof(itemCost));

            Id = id;
            ItemCount = itemCount;
            ItemCost = itemCost;
            Description = description;
            SKU = sku ?? String.Empty;
            Image = image;
            IsDownload = isDownload;
        }

        public ShoppingCartItem(in int id, in decimal itemCount, in decimal itemCost, in decimal taxRate,
            in string description, in string sku, in string image, in bool isDownload, in int weight,
            in string customerReference)
            : this(id, itemCount, itemCost, description, sku, image, isDownload)
        {
            if (taxRate < 0)
                throw new ArgumentOutOfRangeException(nameof(taxRate));

            if (weight < 0)
                throw new ArgumentOutOfRangeException(nameof(weight));

            TaxRate = taxRate;
            Weight = weight;
            CustomerReference = customerReference ?? String.Empty;
        }

        #endregion Constructors

        #region Properties

        public int Id { get; private set; }

        public decimal ItemCount { get; private set; }

        public decimal ItemCost { get; private set; }

        public decimal TaxRate { get; private set; }

        public string Description { get; private set; }

        public string SKU { get; private set; }

        public string Image { get; private set; }

        public bool IsDownload { get; private set; }

        public int Weight { get; private set; }

        public string CustomerReference { get; private set; }

        #endregion Properties
    }
}
