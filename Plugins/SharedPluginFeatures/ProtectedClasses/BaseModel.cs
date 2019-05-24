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
using System.Threading;

namespace SharedPluginFeatures
{
    public class BaseModel
    {
        #region Private Members

        private List<BreadcrumbItem> _breadcrumbs;

        #endregion Private Members

        #region Constructors

        public BaseModel()
        {

        }

        public BaseModel(in List<BreadcrumbItem> breadcrumbs, in ShoppingCartSummary cartSummary)
        {
            CartSummary = cartSummary ?? throw new ArgumentNullException(nameof(cartSummary));
            _breadcrumbs = breadcrumbs ?? throw new ArgumentNullException(nameof(breadcrumbs));
        }

        #endregion Constructors

        #region Properties

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

        public ShoppingCartSummary CartSummary { get; set; }

        #endregion Properties

        #region Public Static Methods

        public static string RouteFriendlyName(in string name)
        {
            return HtmlHelper.RouteFriendlyName(name);
        }

        #endregion Public Static Methods

        #region Public Methods

        public string RouteText(in string text)
        {
            return RouteFriendlyName(text);
        }

        public string BreadcrumbData()
        {
            return BreadcrumbData(false);
        }

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

        #endregion Public Methods
    }
}
