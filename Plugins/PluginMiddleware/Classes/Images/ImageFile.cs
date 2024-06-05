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
 *  Product:  PluginMiddleware
 *  
 *  File: ImageFile.cs
 *
 *  Purpose:  Contains details for an image file
 *
 *  Date        Name                Reason
 *  19/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.IO;

using Middleware.Interfaces;

namespace Middleware.Images
{
    /// <summary>
    /// Contains details for image files return by <see cref="IImageProvider"/>
    /// </summary>
    public sealed class ImageFile
    {
        #region Constructors

        /// <summary>
        /// Constructor used for valid existing disk based file
        /// </summary>
        /// <param name="uri">Uri for the image file</param>
        /// <param name="fileName">Path and name of file</param>
        /// <exception cref="ArgumentNullException">Thrown if uri is null</exception>
        /// <exception cref="ArgumentNullException">Thrown if fileName is null or empty</exception>
        /// <exception cref="ArgumentException">Thrown if fileName does not exist</exception>
        public ImageFile(Uri uri, string fileName)
        {
            ImageUri = uri ?? throw new ArgumentNullException(nameof(uri));

            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            if (!File.Exists(fileName))
                throw new FileNotFoundException(nameof(fileName));

            Name = Path.GetFileName(fileName);
            FileExtension = Path.GetExtension(fileName);
            FileInfo fileInfo = new(fileName);
            Size = fileInfo.Length;
            CreateDate = fileInfo.CreationTime;
            ModifiedDate = fileInfo.LastWriteTime;
        }

        /// <summary>
        /// Constructor used for non disk based file information
        /// </summary>
        /// <param name="uri">Uri for the image file</param>
        /// <param name="fileName">Name of file</param>
        /// <param name="fileExtension">File extension type</param>
        /// <param name="size">Size of file</param>
        /// <param name="createdDate">Date and time created</param>
        /// <param name="modifiedDate">Date and time last modified</param>
        /// <exception cref="ArgumentNullException">Thrown if uri is null</exception>
        /// <exception cref="ArgumentNullException">Thrown if fileName is null or empty</exception>
        /// <exception cref="ArgumentNullException">Thrown if fileExtension is null or empty</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if size is less than zero</exception>
        public ImageFile(Uri uri, string fileName, string fileExtension, long size, DateTime createdDate, DateTime modifiedDate)
        {
            ImageUri = uri ?? throw new ArgumentNullException(nameof(uri));

            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            if (String.IsNullOrEmpty(fileExtension))
                throw new ArgumentNullException(nameof(fileExtension));

            if (size < 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            Name = fileName;
            FileExtension = fileExtension;
            Size = size;
            CreateDate = createdDate;
            ModifiedDate = modifiedDate;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Name of image file
        /// </summary>
        /// <value>string</value>
        public string Name { get; }

        /// <summary>
        /// Extension type for file
        /// </summary>
        /// <value>string</value>
        public string FileExtension { get; }

        /// <summary>
        /// Size of file in bytes
        /// </summary>
        /// <value>long</value>
        public long Size { get; }

        /// <summary>
        /// Date/Time file created
        /// </summary>
        /// <value>DateTime</value>
        public DateTime CreateDate { get; }

        /// <summary>
        /// Date/Time file last modified
        /// </summary>
        /// <value>DateTime</value>
        public DateTime ModifiedDate { get; }

        /// <summary>
        /// Uri for image
        /// </summary>
        /// <value>string</value>
        public Uri ImageUri { get; }

        #endregion Properties
    }
}
