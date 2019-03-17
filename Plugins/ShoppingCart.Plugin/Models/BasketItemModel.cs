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
 *  File: BasketItemModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  16/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SharedPluginFeatures;

namespace ShoppingCartPlugin.Models
{
    public class BasketItemModel : BaseModel
    {
        #region Constructors

        public BasketItemModel(in int productId, in string name, in string shortDescription, in string size, 
            in string sku, in decimal price, in int quantity, in string stock, in decimal subTotal, 
            in bool backOrder, in string image)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (price < 0)
                throw new ArgumentOutOfRangeException(nameof(price));

            if (quantity < 1)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            if (subTotal < 0)
                throw new ArgumentOutOfRangeException(nameof(subTotal));

            if (!String.IsNullOrEmpty(shortDescription) && shortDescription.Length > 50)
                throw new ArgumentException(nameof(shortDescription));

            ProductId = productId;
            BackOrder = backOrder;
            SubTotal = subTotal;
            Stock = stock ?? Languages.LanguageStrings.InStock;
            Quantity = quantity;
            Price = price;
            Name = name;
            Sku = sku ?? String.Empty;
            Size = size ?? String.Empty;
            Image = image + "_89.png";
            ShortDescription = shortDescription ?? String.Empty;
        }

        #endregion Constructors

        #region Properties

        public int ProductId { get; private set; }

        public bool BackOrder { get; private set; }

        public string Stock { get; private set; }

        public int Quantity { get; private set; }

        public decimal Price { get; private set; }

        public decimal SubTotal { get; private set; }

        public string Name { get; private set; }

        public string Sku { get; private set; }

        public string Size { get; private set; }

        public string Image { get; private set; }

        public string ShortDescription { get; private set; }

        public string Url
        {
            get
            {
                return $"/Product/{ProductId}/{RouteFriendlyName(Name)}/";
            }
        }

        #endregion Properties
    }
}
