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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Products.Plugin
 *  
 *  File: ImageUploadNotificationListener.cs
 *
 *  Purpose:  Product image upload listener
 *
 *  Date        Name                Reason
 *  30/05/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

using Middleware.Images;
using Middleware.Interfaces;

using PluginManager.Abstractions;

using SharedPluginFeatures;

#pragma warning disable CS1591, CA1416

namespace ProductPlugin.Classes
{
    public sealed class ImageUploadNotificationListener : INotificationListener
    {
        #region Private Members

        private readonly IImageProvider _imageProvider;
        private readonly ISettingsProvider _settingsProvider;

        #endregion Private Members

        #region Constructors

        public ImageUploadNotificationListener(IImageProvider imageProvider, ISettingsProvider settingsProvider)
        {
            _imageProvider = imageProvider ?? throw new ArgumentNullException(nameof(imageProvider));
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
        }

        #endregion Constructors

        #region INotificationListener Methods

        public bool EventRaised(in string eventId, in object param1, in object param2, ref object result)
        {
            bool Result = false;

            if (String.IsNullOrEmpty(eventId))
                return Result;

            if (eventId.Equals(Constants.NotificationEventImageUploaded))
            {
                List<string> errors = new List<string>();
                Result = ProcessImageUpload(param1 as CachedImageUpload, (string)param2, errors);
                result = Result ? errors : null;
            }
            else if (eventId.Equals(Constants.NotificationEventImageUploadOptions))
            {
                Result = ProcessImageOptions(param1 as IImageProcessOptions);
                result = Result ? param1 : null;
            }


            return Result;
        }

        public void EventRaised(in string eventId, in object param1, in object param2)
        {

        }

        public List<string> GetEvents()
        {
            return new List<string>()
            {
                Constants.NotificationEventImageUploaded,
                Constants.NotificationEventImageUploadOptions
            };
        }

        #endregion INotificationListener Methods

        #region Private Methods

        private bool ProcessImageUpload(CachedImageUpload cachedImageUpload, string additionalData, List<string> errors)
        {
            if (cachedImageUpload == null)
                return false;

            if (String.IsNullOrWhiteSpace(additionalData))
                return false;

            if (!cachedImageUpload.GroupName.Equals(Constants.ProductImageFolderName, StringComparison.InvariantCultureIgnoreCase))
                return false;

            if (cachedImageUpload.Files.Count == 0)
                return true;

            ProductPluginSettings settings = _settingsProvider.GetSettings<ProductPluginSettings>("Products");

            List<Size> newSizes = new List<Size>();

            if (settings.ResizeImages)
            {
                string[] newSizess = settings.ResizeWidths.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string newSize in newSizess)
                {
                    string[] parts = newSize.Split(new char[] { 'x' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length != 2)
                        continue;

                    if (Int32.TryParse(parts[0], out int newWidth) && newWidth > 0 && Int32.TryParse(parts[1], out int newHeight) && newHeight > 0)
                        newSizes.Add(new Size(newWidth, newHeight));
                }
            }

            Color backfillColor = GetColorFromHex(settings.ResizeBackfillColor);

            EnsureProductGroupCreated(additionalData);
            return CopyImagesToSubGroupAndVerify(cachedImageUpload.Files, newSizes, additionalData, errors, backfillColor);
        }

        private static Color GetColorFromHex(string resizeBackfillColor)
        {
            try
            {
                return ColorTranslator.FromHtml(resizeBackfillColor);
            }
            catch (Exception)
            {
                return Color.White;
            }
        }

        private bool CopyImagesToSubGroupAndVerify(List<string> files, List<Size> additionalSizes,
            string subgroupName, List<string> errors, Color backfillColor)
        {
            List<ImageFile> existingFiles = _imageProvider.Images(Constants.ProductImageFolderName, subgroupName);

            foreach (string file in files)
            {
                string newFileName = GetNextAutoGenerateFileName(existingFiles, subgroupName);

                try
                {
                    using (Image img = Image.FromFile(file))
                    {
                        byte[] imgBytes = ImageToByteArray(img);
                        _imageProvider.AddFile(Constants.ProductImageFolderName, subgroupName,
                            $"{newFileName}_orig{Path.GetExtension(file)}", imgBytes);

                        foreach (Size newSize in additionalSizes)
                        {
                            using (MemoryStream ms = new MemoryStream(imgBytes))
                            {
                                Image ImageToResize = Image.FromStream(ms);
                                byte[] resizedImageBytes = ImageToPngByteArray(ResizeImageToFixedSize(ImageToResize, newSize, backfillColor));
                                _imageProvider.AddFile(Constants.ProductImageFolderName, subgroupName,
                                    $"{newFileName}_{newSize.Width}.png", resizedImageBytes);
                            }
                        }
                    }

                }
                catch (Exception e)
                {
                    errors.Add($"Unable to verify {file} {e}");
                }

                existingFiles.Add(new ImageFile(new Uri("/", UriKind.Relative), newFileName, ".jpg", 10, DateTime.Now, DateTime.Now));
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte[] ImageToPngByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private static Image ResizeImageToFixedSize(Image imgPhoto, in Size size, Color fillColor)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent;

            float nPercentW = ((float)size.Width / (float)sourceWidth);
            float nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((size.Width - (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((size.Height - (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(size.Width, size.Height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(fillColor);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        private static string GetNextAutoGenerateFileName(List<ImageFile> files, string subgroupName)
        {
            int i = 0;
            bool isFree = false;
            string Result = null;
            do
            {
                i++;
                Result = $"{subgroupName}_{i}";
                isFree = files.Where(f => f.Name.StartsWith(Result)).FirstOrDefault() == null;
            } while (!isFree);

            return Result;
        }

        private void EnsureProductGroupCreated(string subgroupName)
        {
            if (!_imageProvider.SubgroupExists(Constants.ProductImageFolderName, subgroupName))
                _imageProvider.AddSubgroup(Constants.ProductImageFolderName, subgroupName);
        }

        private static bool ProcessImageOptions(IImageProcessOptions options)
        {
            if (options == null)
                return false;

            if (!options.GroupName.Equals(Constants.ProductImageFolderName, StringComparison.InvariantCultureIgnoreCase))
                return false;

            options.AdditionalDataMandatory = true;
            options.AdditionalDataName = Languages.LanguageStrings.AppStockSKU;
            options.ShowSubgroup = false;

            return true;
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591, CA1416