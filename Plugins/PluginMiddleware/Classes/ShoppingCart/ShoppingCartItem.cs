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
            Size = String.Empty;
        }

        public ShoppingCartItem(in int id, in decimal itemCount, in decimal itemCost, in string name,
            in string description, in string sku, in string[] images, in bool isDownload,
            in bool canBackOrder, in string size)
            :this ()
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));

            if (images == null)
                throw new ArgumentNullException(nameof(images));

            if (images.Length < 1)
                throw new ArgumentException(nameof(images));

            if (itemCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(itemCount));

            if (itemCost < 0)
                throw new ArgumentOutOfRangeException(nameof(itemCost));

            Id = id;
            ItemCount = itemCount;
            ItemCost = itemCost;
            Name = name;
            Description = description;
            SKU = sku ?? String.Empty;
            Images = images;
            IsDownload = isDownload;
            CanBackOrder = canBackOrder;
            Size = size ?? String.Empty;
        }

        public ShoppingCartItem(in int id, in decimal itemCount, in decimal itemCost, in decimal taxRate,
            in string name, in string description, in string sku, in string[] images, in bool isDownload,
            in int weight, in string customerReference, in bool canBackOrder, in string size)
            : this(id, itemCount, itemCost, name, description, sku, images, isDownload, canBackOrder, size)
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

        #region Public Methods

        public void UpdateCount(in int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count));

            ItemCount += count;
        }

        public void ResetCount(in int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count));

            ItemCount = count;
        }

        #endregion Public Methods

        #region Properties

        public int Id { get; private set; }

        public decimal ItemCount { get; private set; }

        public decimal ItemCost { get; private set; }

        public decimal TaxRate { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public string SKU { get; private set; }

        public string[] Images { get; private set; }

        public bool IsDownload { get; private set; }

        public int Weight { get; private set; }

        public string CustomerReference { get; private set; }

        public bool CanBackOrder { get; private set; }

        public string Size { get; private set; }

        #endregion Properties
    }
}
