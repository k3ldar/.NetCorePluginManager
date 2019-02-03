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
 *  File: ProductGroup.cs
 *
 *  Purpose:  Product Group
 *
 *  Date        Name                Reason
 *  31/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Text;

namespace Middleware.Products
{
    public class ProductGroup
    {
        #region Constructors

        public ProductGroup(in int id, in string description, in string seoDescription, in bool showOnWebsite,
            in int sortOrder, in string tagLine, in string url)
        {
            Id = id;
            Description = description;
            SeoDescripton = seoDescription;
            ShowOnWebsite = showOnWebsite;
            SortOrder = sortOrder;
            TagLine = tagLine;
            Url = url;
        }

        #endregion Constructors

        #region Public Methods

        #endregion Public Methods

        #region Properties

        public int Id { get; private set; }

        public string Description { get; private set; }

        public string SeoDescripton { get; private set; }

        public bool ShowOnWebsite { get; private set; }

        public int SortOrder { get; private set; }

        public string TagLine { get; private set; }

        public string Url { get; private set; }

        #endregion Properties
    }
}
