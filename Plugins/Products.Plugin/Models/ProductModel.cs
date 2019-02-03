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
 *  File: ProductModel.cs
 *
 *  Purpose:  Product Model
 *
 *  Date        Name                Reason
 *  31/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

using SharedPluginFeatures;

using Middleware.Products;

namespace ProductPlugin.Models
{
    public sealed class ProductModel : BaseProductModel
    {
        #region Constructors

        public ProductModel()
        {
        }

        public ProductModel(in int id, in string name, in string[] images, in int productGroupId, 
            in bool newProduct, in bool bestSeller, in decimal lowestPrice)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Id = id;
            ProductGroupId = productGroupId;
            Name = name;
            Description = String.Empty;
            Features = String.Empty;
            VideoLink = String.Empty;
            Images = images;
            NewProduct = newProduct;
            BestSeller = bestSeller;

            if (lowestPrice == 0)
                Price = Languages.LanguageStrings.Free;
            else
                Price = lowestPrice.ToString("C", System.Threading.Thread.CurrentThread.CurrentUICulture);
        }

        public ProductModel(in List<BreadcrumbItem> breadcrumbs, in IEnumerable<ProductCategoryModel> productGroups)
            : base (breadcrumbs, productGroups)
        {

        }

        public ProductModel(in List<BreadcrumbItem> breadcrumbs, in IEnumerable<ProductCategoryModel> productGroups, 
            in int id, in int productGroupId, in string name, in string description, in string features,
            in string videoLink, in string[] images, in decimal lowestPrice)
            : this (breadcrumbs, productGroups)
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

            if (lowestPrice == 0)
                Price = Languages.LanguageStrings.Free;
            else
                Price = lowestPrice.ToString("C", System.Threading.Thread.CurrentThread.CurrentUICulture);
        }

        #endregion Constructors

        #region Public Methods

        public string GetRouteName()
        {
            return RouteFriendlyName(Name);
        }

        public string GetVideoLink()
        {
            string Result = VideoLink;

            if (Result.ToLower().StartsWith("https://www.facebook.com/video") ||
                Result.ToLower().StartsWith("http://www.facebook.com/video"))
            {
                //its from facebook
                string fbReference = Result.Replace("video.php?v=", "v/");
                Result = String.Format("<object width=\"640\" height=\"390\" ><param name=\"allowfullscreen\" value=\"true\" /> " +
                    "<param name=\"allowscriptaccess\" value=\"always\" /> <param name=\"movie\" value=\"{0}\" /> " +
                    "<embed src=\"{0}\" type=\"application/x-shockwave-flash\" allowscriptaccess=\"always\" " +
                    "allowfullscreen=\"true\" width=\"640\" height=\"390\"></embed></object>", fbReference);
            }
            else if (!Result.ToLower().StartsWith("http"))
            {
                //assume a you tube link here
                Result = String.Format("<iframe width=\"640\" height=\"390\" src=\"https://www.youtube.com/embed/{0}\" frameborder=\"0\"></iframe>", Result);
            }

            return (Result);
        }

        public string[] FeatureList()
        {
            return Features.Split('\r', StringSplitOptions.RemoveEmptyEntries);
        }

        #endregion Public Methods

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

        public string Price { get; private set; }

        #endregion Properties
    }
}
