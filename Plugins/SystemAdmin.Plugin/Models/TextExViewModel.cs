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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
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

using Microsoft.Extensions.DependencyInjection;

using SharedPluginFeatures;

namespace SystemAdmin.Plugin.Models
{
    public sealed class TextExViewModel : BaseCoreClass
    {
        #region Constructors

        public TextExViewModel(SystemAdminSubMenu subMenu)
        {
            if (subMenu == null)
                throw new ArgumentNullException(nameof(subMenu));

            Title = subMenu.Name();

            ISettingsProvider settingsProvider = (ISettingsProvider)Classes.PluginClass.GetServiceProvider.GetRequiredService<IPluginClassesService>();
            SystemAdminSettings settings = settingsProvider.GetSettings<SystemAdminSettings>("SystemAdmin");

            if (settings.DisableFormattedText)
                Text = "Formatted Text is not enabed";
            else
                Text = subMenu.Data();

            BreadCrumb = $"<ul><li><a href=\"/SystemAdmin/\">System Admin</a></li><li><a href=\"/SystemAdmin/Index/" +
                $"{subMenu.ParentMenu.UniqueId}\">{subMenu.ParentMenu.Name()}</a></li><li>{Title}</li></ul>";
        }

        #endregion Constructors

        #region Public Properties

        public string Title { get; set; }

        public string BreadCrumb { get; set; }

        public string Text { get; private set; }

        #endregion Public Properties
    }
}
