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
 *  Product:  Download Plugin
 *  
 *  File: DownloadModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  11/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

namespace DownloadPlugin.Models
{
    public class DownloadModel : BaseModel
    {
        #region Constructors

        public DownloadModel(in List<BreadcrumbItem> breadcrumbs, in ShoppingCartSummary cartSummary, 
            in string category, in List<DownloadableItem> downloads, in List<CategoriesModel> categories)
            : base (breadcrumbs, cartSummary)
        {
            if (String.IsNullOrEmpty(category))
                throw new ArgumentNullException(nameof(category));

            Category = category;
            Categories = categories ?? throw new ArgumentNullException(nameof(categories));
            Downloads = downloads ?? throw new ArgumentNullException(nameof(downloads));
        }

        #endregion Constructors

        #region Properties

        public string Category { get; private set; }

        public List<CategoriesModel> Categories { get; private set; }

        public List<DownloadableItem> Downloads { get; private set; }

        public string Pagination { get; internal set; }

        #endregion Properties
    }
}
