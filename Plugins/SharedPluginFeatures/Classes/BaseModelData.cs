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
 *  Product:  SharedPluginFeatures
 *  
 *  File: BaseModelData.cs
 *
 *  Purpose:  Contains basic data to be loaded into BaseModel for general use.
 *
 *  Date        Name                Reason
 *  08/06/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

namespace SharedPluginFeatures
{
    /// <summary>
    /// Generic model data that can be displayed on all web pages.
    /// </summary>
    public sealed class BaseModelData
    {
        #region Constructors

        internal BaseModelData(in List<BreadcrumbItem> breadcrumbs, 
            in ShoppingCartSummary cartSummary, in string seoTitle, 
            in string seoAuthor, in string seoDescription, 
            in string seoTags)
        {
            Breadcrumbs = breadcrumbs ?? new List<BreadcrumbItem>();
            CartSummary = cartSummary;
            SeoAuthor = seoAuthor ?? String.Empty;
            SeoDescription = seoDescription ?? String.Empty;
            SeoTitle = seoTitle ?? String.Empty;
            SeoTags = seoTags ?? String.Empty;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Breadcrumb items
        /// </summary>
        /// <value>List&lt;BreadcrumbItems&gt;</value>
        public List<BreadcrumbItem> Breadcrumbs { get; private set; }

        /// <summary>
        /// Shopping cart summary
        /// </summary>
        /// <value>ShoppingCartSummary</value>
        public ShoppingCartSummary CartSummary { get; private set; }

        /// <summary>
        /// Seo title for a web page.
        /// </summary>
        /// <value>string</value>
        public string SeoTitle { get; private set; }

        /// <summary>
        /// Seo description for a web page.
        /// </summary>
        /// <value>string</value>
        public string SeoDescription { get; private set; }

        /// <summary>
        /// Seo author for a web page.
        /// </summary>
        /// <value>string</value>
        public string SeoAuthor { get; private set; }

        /// <summary>
        /// Seo Tags for a web page.
        /// </summary>
        /// <value>string</value>
        public string SeoTags { get; private set; }

        #endregion Properties
    }
}
