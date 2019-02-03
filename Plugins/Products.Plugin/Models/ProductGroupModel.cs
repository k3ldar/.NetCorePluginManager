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
 *  File: ProductGroupModel.cs
 *
 *  Purpose:  Product Group Model
 *
 *  Date        Name                Reason
 *  01/02/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

namespace ProductPlugin.Models
{
    public class ProductGroupModel : BaseProductModel
    {
        #region Constructors

        public ProductGroupModel()
        {
        }

        public ProductGroupModel(in List<BreadcrumbItem> breadcrumbs, in IEnumerable<ProductCategoryModel> productGroups,
            in string description, in string tagLine)
            : base(breadcrumbs, productGroups)
        {
            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));

            if (String.IsNullOrEmpty(tagLine))
                throw new ArgumentNullException(nameof(tagLine));

            Products = new List<ProductCategoryProductModel>();
            Description = description;
            TagLine = tagLine;
        }


        #endregion Constructors

        #region Public Methods

        public string GetRouteDescription()
        {
            return RouteFriendlyName(Description);
        }

        #endregion Public Methods

        #region Properties

        public string Description { get; private set; }

        public string TagLine { get; private set; }

        public string Pagination { get; internal set; }

        public List<ProductCategoryProductModel> Products { get; private set; }

        #endregion Properties
    }
}
