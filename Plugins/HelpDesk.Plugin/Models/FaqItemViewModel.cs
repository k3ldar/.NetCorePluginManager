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
 *  Product:  Helpdesk Plugin
 *  
 *  File: FaqItemViewModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  27/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

namespace HelpdeskPlugin.Models
{
    public class FaqItemViewModel : BaseModel
    {
        #region Constructors

        public FaqItemViewModel(in List<BreadcrumbItem> breadcrumbs, in ShoppingCartSummary cartSummary,
            in FaqGroup parentGroup, in string description, in int viewCount, in string content)
            : base(breadcrumbs, cartSummary)
        {
            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));

            if (String.IsNullOrEmpty(content))
                throw new ArgumentNullException(nameof(content));

            if (viewCount < 0)
                throw new ArgumentOutOfRangeException(nameof(viewCount));

            ParentGroup = parentGroup ?? throw new ArgumentNullException(nameof(parentGroup));
            Description = description;
            ViewCount = viewCount;
            Content = content;
        }

        #endregion Constructors

        #region Properties

        public FaqGroup ParentGroup { get; private set; }

        public string Description { get; private set; }

        public int ViewCount { get; private set; }

        public string Content { get; private set; }

        #endregion Properties
    }
}
