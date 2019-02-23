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
            _breadcrumbs = new List<BreadcrumbItem>();
        }

        public BaseModel(in List<BreadcrumbItem> breadcrumbs)
        {
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

        #endregion Properties

        #region Protected Methods

        protected string RouteFriendlyName(in string name)
        {
            StringBuilder Result = new StringBuilder(name.Length);
            char lastChar = ' ';

            foreach (char c in name)
            {
                if (c >= 65 && c <= 90)
                {
                    Result.Append(c);
                    lastChar = c;
                }
                else if (c >= 97 && c <= 122)
                {
                    Result.Append(c);
                    lastChar = c;
                }
                else if (c >= 48 && c <= 57)
                {
                    Result.Append(c);
                    lastChar = c;
                }
                else if (lastChar != Constants.Dash)
                {
                    Result.Append(Constants.Dash);
                    lastChar = Constants.Dash;
                }
            }

            if (Result[Result.Length -1] == Constants.Dash)
                Result.Length = Result.Length - 1;

            return Result.ToString();
        }

        #endregion Protected Methods

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
