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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: DownloadCategory.cs
 *
 *  Purpose:  Download Categories
 *
 *  Date        Name                Reason
 *  05/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

namespace Middleware.Downloads
{
    /// <summary>
    /// Available download categories including a list of DownloadItem's that are available for the category.
    /// </summary>
    public class DownloadCategory
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Unique id for category.</param>
        /// <param name="name">Name of download category.</param>
        /// <param name="downloads">List of all download items that are within the category.</param>
        public DownloadCategory(in long id, in string name, in List<DownloadItem> downloads)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Id = id;
            Name = name;
            Downloads = downloads ?? throw new ArgumentNullException(nameof(downloads));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Unique id for category.
        /// </summary>
        /// <value>long</value>
        public long Id { get; private set; }

        /// <summary>
        /// Name of download category.
        /// </summary>
        /// <value>string</value>
        public string Name { get; private set; }

        /// <summary>
        /// List of all download items that are within the category.
        /// </summary>
        /// <value>List&lt;DownloadItem&gt;</value>
        public List<DownloadItem> Downloads { get; private set; }

        #endregion Properties
    }
}
