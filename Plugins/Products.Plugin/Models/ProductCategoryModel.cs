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
 *  File: ProductCategoryModel.cs
 *
 *  Purpose:  Product Controller Category Model
 *
 *  Date        Name                Reason
 *  31/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

#pragma warning disable CS1591

namespace ProductPlugin.Models
{
    public class ProductCategoryModel : BaseProductModel
    {
        #region Constructors

        public ProductCategoryModel()
        {

        }

        public ProductCategoryModel(in int id, in string description)
        {
            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));

            Id = id;
            Description = description;
        }

        public ProductCategoryModel(in int id, in string description, in string url)
            : this(id, description)
        {
            if (String.IsNullOrEmpty(url))
            {
                Url = $"/Products/{RouteText(Description)}/{id}/";
            }
            else
            {
                Url = url;
            }
        }

        #endregion Constructors

        #region Properties

        public string Description { get; }

        public int Id { get; }

        public string Url { get; }

        #endregion Properties
    }
}

#pragma warning restore CS1591