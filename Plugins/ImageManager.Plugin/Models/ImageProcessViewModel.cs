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
 *  Product:  Image Manager Plugin
 *  
 *  File: ImageProcessViewModel.cs
 *
 *  Date        Name                Reason
 *  14/05/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;

namespace ImageManager.Plugin.Models
{
    /// <summary>
    /// View model for processing uploaded images
    /// </summary>
    public sealed class ImageProcessViewModel
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ImageProcessViewModel()
        {
        }

        /// <summary>
        /// Constructor accepting id of file upload
        /// </summary>
        /// <param name="fileUploadId">Id for the file upload</param>
        /// <exception cref="ArgumentNullException">Thrown if fileUploadId is null or an empty string</exception>
        public ImageProcessViewModel(string fileUploadId)
        {
            if (string.IsNullOrEmpty(fileUploadId))
                throw new ArgumentNullException(nameof(fileUploadId));

            FileUploadId = fileUploadId;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Unique file upload id
        /// </summary>
        /// <value>string</value>
        public string FileUploadId { get; set; }

        /// <summary>
        /// Additional data supplied by user
        /// </summary>
        /// <value>string</value>
        public string AdditionalData { get; set; }

        #endregion Properties
    }
}
