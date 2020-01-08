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
 *  Product:  SharedPluginFeatures
 *  
 *  File: BaseModel.cs
 *
 *  Purpose:  Base model class
 *
 *  Date        Name                Reason
 *  21/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedPluginFeatures
{
    /// <summary>
    /// Base model value containing data that can be displayed on every page.
    /// </summary>
    public class BaseModel
    {
        #region Private Members

        private List<BreadcrumbItem> _breadcrumbs;

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseModel()
        {

        }

        /// <summary>
        /// Constructor allowing developer to pass all generic base model data in one pass.
        /// </summary>
        /// <param name="modelData">BaseModelData</param>
        public BaseModel(in BaseModelData modelData)
        {
            if (modelData == null)
                throw new ArgumentNullException(nameof(modelData));

            Breadcrumbs = modelData.Breadcrumbs;
            CartSummary = modelData.CartSummary;
            SeoAuthor = modelData.SeoAuthor;
            SeoDescription = modelData.SeoDescription;
            SeoTags = modelData.SeoTags;
            SeoTitle = modelData.SeoTitle;
            CanManageSeoData = modelData.CanManageSeoData;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Set the list of BreadcrumbItem.
        /// </summary>
        /// <value>List&lt;BreadCrumbItem&gt;</value>
        public List<BreadcrumbItem> Breadcrumbs
        {
            get
            {
                return _breadcrumbs;
            }

            set
            {
                _breadcrumbs = value ?? throw new InvalidOperationException("Invalid Breadcrumbs value");
            }
        }

        /// <summary>
        /// ShoppingCartSummary instance.
        /// </summary>
        /// <value>ShoppingCartSummary</value>
        public ShoppingCartSummary CartSummary { get; set; }

        #endregion Properties

        #region Public Static Methods

        /// <summary>
        /// Converts a string to a route friendly name, removing all but alpha numeric charactes.
        /// </summary>
        /// <param name="text">Route text to be converted.</param>
        /// <returns>string</returns>
        public static string RouteFriendlyName(in string text)
        {
            return HtmlHelper.RouteFriendlyName(text);
        }

        #endregion Public Static Methods

        #region Public Methods

        /// <summary>
        /// Converts a string to a route friendly name, removing all but alpha numeric charactes.
        /// </summary>
        /// <param name="text">Route text to be converted.</param>
        /// <returns>string</returns>
        public string RouteText(in string text)
        {
            return RouteFriendlyName(text);
        }

        /// <summary>
        /// Retrieve a list of BreadcrumbItem object items converted to a ul/li list for display within a page.
        /// 
        /// The last item in the list will not have a link generated.
        /// </summary>
        /// <returns>string</returns>
        public string BreadcrumbData()
        {
            return BreadcrumbData(false);
        }

        /// <summary>
        ///Retrieve a list of BreadcrumbItem object items converted to a ul/li list for display within a page.
        /// </summary>
        /// <param name="lastItemLinked">bool.  If true the last item will have an a href link, otherwise it wont.</param>
        /// <returns>string</returns>
        public string BreadcrumbData(in bool lastItemLinked)
        {
            StringBuilder Result = new StringBuilder("<ul>", 4000);

            for (int i = 0; i < _breadcrumbs.Count; i++)
            {
                BreadcrumbItem item = _breadcrumbs[i];

                if (i == _breadcrumbs.Count - 1 && !lastItemLinked)
                    Result.Append($"<li>{item.Name}</li>");
                else
                    Result.Append($"<li><a href=\"{item.Route}\">{item.Name}</a></li>");
            }

            Result.Append("</ul>");

            return Result.ToString();
        }

        /// <summary>
        /// Contains Seo Author data to be displayed on a web page.
        /// </summary>
        /// <value>string</value>
        public string SeoAuthor { get; set; }

        /// <summary>
        /// Contains the Seo Title for a web page
        /// </summary>
        /// <value>string</value>
        public string SeoTitle { get; set; }

        /// <summary>
        /// Contains the Seo tags that will be inserted into a web page.
        /// </summary>
        /// <value>string</value>
        public string SeoTags { get; set; }

        /// <summary>
        /// Contains Seo description to be inserted into a web page.
        /// </summary>
        /// <value>string</value>
        public string SeoDescription { get; set; }

        /// <summary>
        /// Indicates the user can manage Seo data.
        /// </summary>
        public bool CanManageSeoData { get; private set; }

        #endregion Public Methods
    }
}
