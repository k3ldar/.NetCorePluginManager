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
 *  File: GridViewModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  28/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using SharedPluginFeatures;

namespace SystemAdmin.Plugin.Models
{
    public sealed class GridViewModel
    {
        #region Constructors

        public GridViewModel(SystemAdminSubMenu subMenu)
        {
            if (subMenu == null)
                throw new ArgumentNullException(nameof(subMenu));

            Title = subMenu.Name();

            // data has to have the header on first row, each column seperated by a pipe |
            // the data is on all following lines and is also seperated by pipe |
            // each line is seperated with \r
            string[] allLines = subMenu.Data().Split('\r');

            // must have a header at the very least!
            if (allLines.Length == 0)
                throw new ArgumentNullException(nameof(subMenu.Data));

            Headers = allLines[0].Split('|');

            int columnCount = Headers.Length;

            Items = new List<string[]>();

            for (int i = 1; i < allLines.Length; i++)
            {
                string[] line = allLines[i].Split('|');

                if (line.Length != Headers.Length)
                    throw new InvalidOperationException("column count much match header column count");

                Items.Add(line);
            }

            BreadCrumb = $"<ul><li><a href=\"/SystemAdmin/\">System Admin</a></li><li><a href=\"/SystemAdmin/Index/" +
                $"{subMenu.ParentMenu.UniqueId}\">{subMenu.ParentMenu.Name()}</a></li><li>{Title}</li></ul>";
        }

        #endregion Constructors

        #region Public Properties

        public string[] Headers { get; set; }

        public List<string[]> Items { get; set; }

        public string Title { get; set; }

        public string BreadCrumb { get; set; }

        #endregion Public Properties
    }
}
