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
 *  File: Product.cs
 *
 *  Purpose:  Product 
 *
 *  Date        Name                Reason
 *  01/02/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace Middleware.Products
{
    public sealed class Product
    {
        #region Constructors

        public Product(in int id, in int productGroupId, in string name, in string description, in string features,
            in string videoLink, in string[] images, in decimal retailPrice, in string sku, in bool isDownload,
            in bool allowBackorder)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));

            Id = id;
            ProductGroupId = productGroupId;
            Name = name;
            Description = description;
            Features = features;
            VideoLink = videoLink;
            Images = images;
            RetailPrice = retailPrice;
            Sku = sku;
            IsDownload = isDownload;
            AllowBackorder = allowBackorder;
        }

        #endregion Constructors

        #region Properties

        public int Id { get; private set; }

        public int ProductGroupId { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public string Features { get; private set; }

        public string VideoLink { get; private set; }

        public string[] Images { get; private set; }

        public bool NewProduct { get; private set; }

        public bool BestSeller { get; private set; }

        public decimal RetailPrice { get; private set; }

        public string Sku { get; private set; }

        public bool IsDownload { get; private set; }

        public bool AllowBackorder { get; private set; }

        #endregion Properties
    }
}
