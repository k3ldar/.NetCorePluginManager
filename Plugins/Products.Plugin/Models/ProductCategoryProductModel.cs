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
 *  Product:  Products.Plugin
 *  
 *  File: ProductCategoryProductModel.cs
 *
 *  Purpose:  Product Category Product
 *
 *  Date        Name                Reason
 *  03/02/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace ProductPlugin.Models
{
    public class ProductCategoryProductModel : BaseProductModel
    {
        #region Constructors

        public ProductCategoryProductModel()
        {
        }

        public ProductCategoryProductModel(in int id, in string name, in string image, in int productGroupId,
            in bool newProduct, in bool bestSeller, in decimal lowestPrice)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Id = id;
            ProductGroupId = productGroupId;
            Name = name;
            Images = image;
            NewProduct = newProduct;
            BestSeller = bestSeller;

            if (lowestPrice == 0)
                Price = Languages.LanguageStrings.Free;
            else
                Price = $"{Languages.LanguageStrings.From} {lowestPrice.ToString("C", System.Threading.Thread.CurrentThread.CurrentUICulture)}";
        }

        #endregion Constructors

        #region Public Methods

        public string GetRouteName()
        {
            return RouteFriendlyName(Name);
        }

        #endregion Public Methods

        #region Properties

        public int Id { get; private set; }

        public int ProductGroupId { get; private set; }

        public string Name { get; private set; }

        public string Images { get; private set; }

        public bool NewProduct { get; private set; }

        public bool BestSeller { get; private set; }

        public string Price { get; private set; }

        #endregion Properties
    }
}
