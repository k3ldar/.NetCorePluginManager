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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
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

#pragma warning disable CS1591

namespace SystemAdmin.Plugin.Models
{
    public sealed class GridViewModel : BaseModel
    {
        #region Constructors

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Validating property of param so ok")]
        public GridViewModel(in BaseModelData modelData,
            SystemAdminSubMenu subMenu)
            : base(modelData)
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

            HeaderColumnCount = Headers.Length;

            Items = new List<string[]>();

            for (int i = 1; i < allLines.Length; i++)
            {
                string[] line = allLines[i].Split('|');

                if (line.Length > Headers.Length)
                    throw new InvalidOperationException("line column count much match header column count" +
                        $"\r\n\r\n{subMenu.Data()}");

                Items.Add(line);
            }
        }

        #endregion Constructors

        #region Public Properties

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "ok on this occasion")]
        public string[] Headers { get; set; }

        public List<string[]> Items { get; set; }

        public string Title { get; set; }

        public int HeaderColumnCount { get; private set; }

        #endregion Public Properties
    }
}

#pragma warning restore CS1591