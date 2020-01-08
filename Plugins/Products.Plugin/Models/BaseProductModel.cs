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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Products.Plugin
 *  
 *  File: BaseProductModel.cs
 *
 *  Purpose:  Base Product Model
 *
 *  Date        Name                Reason
 *  02/02/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

namespace ProductPlugin.Models
{
    public class BaseProductModel : BaseModel
    {
        #region Constructors

        public BaseProductModel()
        {

        }

        public BaseProductModel(in BaseModelData modelData)
            : base(modelData)
        {

        }

        public BaseProductModel(in BaseModelData modelData,
            in IEnumerable<ProductCategoryModel> productGroups)
            : base(modelData)
        {
            ProductGroups = productGroups ?? throw new ArgumentNullException(nameof(productGroups));
        }

        #endregion Constructors

        #region Properties

        public IEnumerable<ProductCategoryModel> ProductGroups { get; private set; }

        #endregion Properties

    }
}
