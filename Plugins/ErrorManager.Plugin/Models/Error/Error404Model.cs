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
 *  Product:  Error Manager Plugin
 *  
 *  File: Error404Model.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  17/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

namespace ErrorManager.Plugin.Models.Error
{
    public sealed class Error404Model : BaseModel
    {
        #region Constructors

        public Error404Model()
        {

        }

        public Error404Model(in List<BreadcrumbItem> breadcrumbs, in ShoppingCartSummary cartSummary, 
            string title)
            : base (breadcrumbs, cartSummary)
        {
            if (String.IsNullOrEmpty(title))
                throw new ArgumentNullException(nameof(title));

            Title = title;
        }

        public Error404Model(in List<BreadcrumbItem> breadcrumbs, in ShoppingCartSummary cartSummary, 
            string title, string message, string image)
            : this(breadcrumbs, cartSummary, title)
        {
            if (String.IsNullOrEmpty(title))
                throw new ArgumentNullException(nameof(title));

            Title = title;
            Message = message;
            Image = image;
        }

        #endregion Constructors

        #region Properties

        public string Title { get; set; }

        public string Message { get; set; }

        public string Image { get; set; }

        #endregion Properties
    }
}
