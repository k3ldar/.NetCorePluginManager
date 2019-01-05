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
 *  Product:  UserAccount.Plugin
 *  
 *  File: ViewDownloadViewModel.cs
 *
 *  Purpose: View a Download view model
 *
 *  Date        Name                Reason
 *  05/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Middleware.Downloads;

namespace UserAccount.Plugin.Models
{
    public class ViewDownloadViewItem
    {
        #region Constructors

        public ViewDownloadViewItem()
        {

        }

        public ViewDownloadViewItem(in int id, in string name, in string description,
            in string version, in string filename, in string icon, in string size)
        {
            Id = id;
            Name = name;
            Description = description;
            Version = version;
            Filename = filename;
            Icon = icon;
            Size = size;
        }

        #endregion Constructors

        #region Properties

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Version { get; set; }

        public string Filename { get; set; }

        public string Icon { get; set; }

        public string Size { get; set; }

        #endregion Properties
    }
}
