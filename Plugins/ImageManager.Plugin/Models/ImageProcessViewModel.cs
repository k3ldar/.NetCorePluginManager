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
 *  Product:  Image Manager Plugin
 *  
 *  File: ImageProcessViewModel.cs
 *
 *  Date        Name                Reason
 *  14/05/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

namespace ImageManager.Plugin.Models
{
    /// <summary>
    /// View model for processing uploaded images
    /// </summary>
    public sealed class ImageProcessViewModel : BaseModel
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="modelData"><see cref="BaseModelData"/> instance.</param>
        /// <param name="fileUploadId">Unique id of file upload session.</param>
        /// <exception cref="ArgumentNullException">Thrown if modelData is null or invalid.</exception>
        /// <exception cref="ArgumentNullException">Thrown if fileUploadId is null or empty.</exception>
        public ImageProcessViewModel(BaseModelData modelData, string fileUploadId)
            : base(modelData)
        {
            if (String.IsNullOrEmpty(fileUploadId))
                throw new ArgumentNullException(nameof(fileUploadId));

            FileUploadId = fileUploadId;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Unique file upload id
        /// </summary>
        /// <value>string</value>
        public string FileUploadId { get; }

        #endregion Properties
    }
}
