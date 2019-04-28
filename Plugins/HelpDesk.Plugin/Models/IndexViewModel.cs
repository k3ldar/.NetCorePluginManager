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
 *  File: IndexViewModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  11/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

using SharedPluginFeatures;

namespace HelpdeskPlugin.Models
{
    public class IndexViewModel : BaseModel
    {
        #region Construtors

        public IndexViewModel(in List<BreadcrumbItem> breadcrumbs, in ShoppingCartSummary cartSummary, 
            in bool showTickets, in bool showFaq, in bool showFeedback, in string growlMessage)
            : base (breadcrumbs, cartSummary)
        {
            ShowFaq = showFaq;
            ShowFeedback = showFeedback;
            ShowTickets = showTickets;
            GrowlMessage = growlMessage ?? string.Empty;
        }

        #endregion Construtors

        #region Properties

        public bool ShowTickets { get; private set; }

        public bool ShowFaq { get; private set; }

        public bool ShowFeedback { get; private set; }

        public string GrowlMessage { get; private set; }

        #endregion Properties
    }
}
