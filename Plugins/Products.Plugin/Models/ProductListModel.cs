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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
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

namespace ProductPlugin.Models
{
    /// <summary>
    /// Model for list of products
    /// </summary>
    public sealed class ProductListModel
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductListModel(int id, string sku, string name)
        {
            if (String.IsNullOrEmpty(sku))
                throw new ArgumentNullException(nameof(sku));

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Id = id;
            Sku = sku;
            Name = name;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Product Id
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Product Sku
        /// </summary>
        public string Sku { get; }

        /// <summary>
        /// Product Name
        /// </summary>
        public string Name { get; }

        #endregion Properties
    }
}
