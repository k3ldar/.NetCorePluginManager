using System;
using System.Collections.Generic;

using Middleware.Images;

using SharedPluginFeatures;

namespace ImageManager.Plugin.Models
{
    /// <summary>
    /// View model for processing images which have just been uploaded
    /// </summary>
    public sealed class ProcessImagesViewModel : ImagesViewModel
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="modelData"><see cref="BaseModelData"/> instance.</param>
        /// <param name="canManageImages"></param>
        /// <param name="selectedGroupName"></param>
        /// <param name="selectedSubgroupName"></param>
        /// <param name="selectedImageFile"></param>
        /// <param name="groups"></param>
        /// <param name="imageFiles"></param>
        /// <param name="fileUploadId">Unique id of file upload session.</param>
        /// <exception cref="ArgumentNullException">Thrown if modelData is null or invalid.</exception>
        /// <exception cref="ArgumentNullException">Thrown if fileUploadId is null or empty.</exception>
        public ProcessImagesViewModel(in BaseModelData modelData,
            bool canManageImages,
            string selectedGroupName,
            string selectedSubgroupName,
            ImageFile selectedImageFile,
            Dictionary<string,
            List<string>> groups,
            List<ImageFile> imageFiles,
            string fileUploadId)
            : base(modelData, canManageImages, selectedGroupName, selectedSubgroupName, selectedImageFile, groups, imageFiles)
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
