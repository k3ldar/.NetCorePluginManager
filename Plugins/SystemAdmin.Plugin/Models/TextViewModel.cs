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
 *  File: TextViewModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  03/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Text;

using SharedPluginFeatures;

namespace SystemAdmin.Plugin.Models
{
    public sealed class TextViewModel
    {
        #region Constructors

        public TextViewModel(SystemAdminSubMenu subMenu)
        {
            if (subMenu == null)
                throw new ArgumentNullException(nameof(subMenu));

            Title = subMenu.Name();

            StringBuilder newData = new StringBuilder();

            string data = subMenu.Data();

            for (int i = 0; i < data.Length; i++)
            {
                char character = data[i];

                switch (character)
                {
                    case ' ':
                        newData.Append("&nbsp;");
                        break;
                    case '\t':
                        newData.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
                        break;
                    case '\n':
                        newData.Append("<br />");
                        break;
                    case '<':
                        newData.Append("&lt;");
                        break;
                    case '>':
                        newData.Append("&gt;");
                        break;
                    case '\r':
                        break;
                    default:
                        newData.Append(character);
                        break;
                }
            }


            Text = newData.ToString();

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
