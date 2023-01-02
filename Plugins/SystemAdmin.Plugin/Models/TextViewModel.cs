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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
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

#pragma warning disable CS1591

namespace SystemAdmin.Plugin.Models
{
    public sealed class TextViewModel : BaseModel
    {
        #region Constructors

        public TextViewModel(in BaseModelData modelData,
            SystemAdminSubMenu subMenu)
            : base(modelData)
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
        }

        #endregion Constructors

        #region Public Properties

        public string Title { get; }

        public string Text { get; }

        #endregion Public Properties
    }
}

#pragma warning restore CS1591