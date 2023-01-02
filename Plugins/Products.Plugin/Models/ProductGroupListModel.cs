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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Products.Plugin
 *  
 *  File: ProductGroupListModel.cs
 *
 *  Purpose:  Product Model
 *
 *  Date        Name                Reason
 *  13/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

using Middleware;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace ProductPlugin.Models
{
    /// <summary>
    /// View model for product group admin
    /// </summary>
    public sealed class ProductGroupListModel : BaseModel
    {
        public ProductGroupListModel(BaseModelData modelData)
            : base (modelData)
        {
            Groups = new List<LookupListItem>();
        }

        public List<LookupListItem> Groups { get; }
    }
}

#pragma warning restore CS1591