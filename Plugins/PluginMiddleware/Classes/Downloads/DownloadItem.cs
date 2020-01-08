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
 *  Product:  PluginMiddleware
 *  
 *  File: DownloadItem.cs
 *
 *  Purpose:  Downloadable file for a user
 *
 *  Date        Name                Reason
 *  04/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace Middleware.Downloads
{
    /// <summary>
    /// Represents a downloadable file that is used with IDownloadProvider and DownloadPlugin module.
    /// </summary>
    public sealed class DownloadItem
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public DownloadItem()
        {
            Icon = String.Empty;
            Size = String.Empty;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Unique id of download item.</param>
        /// <param name="name">Name of downloadable item.</param>
        /// <param name="description">Description of downloadable item.</param>
        /// <param name="version">Current version of downloadable item.</param>
        /// <param name="filename">Name of the physical file for the downlaod item.</param>
        public DownloadItem(in int id, in string name, in string description,
            in string version, in string filename)
            : this()
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));

            if (String.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));

            Id = id;
            Name = name;
            Description = description;
            Version = version ?? String.Empty;
            Filename = filename;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Unique id of download item.</param>
        /// <param name="name">Name of downloadable item.</param>
        /// <param name="description">Description of downloadable item.</param>
        /// <param name="version">Current version of downloadable item.</param>
        /// <param name="filename">Name of the physical file for the downlaod item.</param>
        /// <param name="icon">Icon used to represent the download item.</param>
        /// <param name="size">Size of the downloadable file.</param>
        public DownloadItem(in int id, in string name, in string description,
            in string version, in string filename, in string icon, in string size)
            : this(id, name, description, version, filename)
        {
            Icon = icon;
            Size = size;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Unique id of download item.
        /// </summary>
        /// <value>int</value>
        public int Id { get; private set; }

        /// <summary>
        /// Name of downloadable item.
        /// </summary>
        /// <value>string</value>
        public string Name { get; private set; }

        /// <summary>
        /// Description of downloadable item.
        /// </summary>
        /// <value>string</value>
        public string Description { get; private set; }

        /// <summary>
        /// Current version of downloadable item.
        /// </summary>
        /// <value>string</value>
        public string Version { get; private set; }

        /// <summary>
        /// Name of the physical file for the downlaod item.
        /// </summary>
        /// <value>string</value>
        public string Filename { get; private set; }

        /// <summary>
        /// Icon used to represent the download item.
        /// </summary>
        /// <value>string</value>
        public string Icon { get; set; }

        /// <summary>
        /// Size of the downloadable file.
        /// </summary>
        /// <value>string</value>
        public string Size { get; set; }

        #endregion Properties
    }
}
