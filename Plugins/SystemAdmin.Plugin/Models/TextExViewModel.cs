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
 *  Product:  SystemAdmin.Plugin
 *  
 *  File: TextExViewModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  03/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

namespace SystemAdmin.Plugin.Models
{
    public sealed class TextExViewModel : BaseModel
    {
        #region Private Members

        private readonly ISettingsProvider _settingsProvider;

        #endregion Private Members

        #region Constructors

        public TextExViewModel(in List<BreadcrumbItem> breadcrumbs, in ShoppingCartSummary cartSummary,
            in ISettingsProvider settingsProvider, in SystemAdminSubMenu subMenu)
            : base (breadcrumbs, cartSummary)
        {
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));

            if (subMenu == null)
                throw new ArgumentNullException(nameof(subMenu));

            Title = subMenu.Name();

            SystemAdminSettings settings = _settingsProvider.GetSettings<SystemAdminSettings>("SystemAdmin");

            if (settings.DisableFormattedText)
                Text = "Formatted Text is not enabed";
            else
                Text = subMenu.Data();
        }

        #endregion Constructors

        #region Public Properties

        public string Title { get; set; }

        public string Text { get; private set; }

        #endregion Public Properties
    }
}
