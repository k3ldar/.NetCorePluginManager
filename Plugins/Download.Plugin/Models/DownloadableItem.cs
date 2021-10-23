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
 *  Copyright (c) 2019 - 2021 Simon Carter.  All Rights Reserved.
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

namespace DownloadPlugin.Models
{
    /// <summary>
    /// Represents a downloadable item
    /// </summary>
    public sealed class DownloadableItem
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Unique id of downloadable item.</param>
        /// <param name="name">Name of downloadable item.</param>
        /// <param name="description">Description of download item.</param>
        /// <param name="version">Version of download item.</param>
        /// <param name="filename">Filename.</param>
        /// <param name="size">Size of file.</param>
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

        /// <summary>
        /// Unique id of downloadable item.
        /// </summary>
        /// <value>int</value>
        public int Id { get; private set; }

        /// <summary>
        /// Name of downloadable item.
        /// </summary>
        /// <value>string</value>
        public string Name { get; private set; }

        /// <summary>
        /// Description of download item.
        /// </summary>
        /// <value>string</value>
        public string Description { get; private set; }

        /// <summary>
        /// Version of download item.
        /// </summary>
        /// <value>string</value>
        public string Version { get; private set; }

        /// <summary>
        /// Filename
        /// </summary>
        /// <value>string</value>
        public string Filename { get; private set; }

        /// <summary>
        /// Icon to be displayed.
        /// </summary>
        /// <value>string</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "I deem it to be valid in this context!")]
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

        /// <summary>
        /// Size of file.
        /// </summary>
        /// <value>string</value>
        public string Size { get; private set; }

        #endregion Properties
    }
}
