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
 *  Product:  Download Plugin
 *  
 *  File: DownloadableItem.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  11/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DownloadPlugin.Models
{
    public sealed class DownloadableItem
    {
        #region Constructors

        public DownloadableItem(in int id, in string name, in string description, in string version, 
            in string filename, in string size)
        {
            Id = id;
            Name = name;
            Description = description;
            Version = version;
            Filename = filename;
            Size = size;
        }

        #endregion Constructors

        #region Properties

        public int Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public string Version { get; private set; }

        public string Filename { get; private set; }

        public string Icon
        {
            get
            {
                switch (Path.GetExtension(Filename).ToLower())
                {
                    case ".exe":
                        return "download.jpg";

                    case ".pdf":
                        return "pdffile.jpg";

                    case ".zip":
                        return "zipfile.jpg";

                    case ".xls":
                    case ".xlxs":
                        return "xlsfile.jpg";

                    default:
                        return "file.jpg";
                }

                throw new InvalidOperationException(nameof(Icon));
            }
        }

        public string Size { get; private set; }

        #endregion Properties
    }
}
