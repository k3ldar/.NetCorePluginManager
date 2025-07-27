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
		/// <param name="modelData"><see cref="IBaseModelData"/> instance.</param>
		/// <param name="canManageImages"></param>
		/// <param name="selectedGroupName"></param>
		/// <param name="selectedSubgroupName"></param>
		/// <param name="selectedImageFile"></param>
		/// <param name="groups"></param>
		/// <param name="imageFiles"></param>
		/// <param name="fileUploadId">Unique id of file upload session.</param>
		/// <exception cref="ArgumentNullException">Thrown if modelData is null or invalid.</exception>
		/// <exception cref="ArgumentNullException">Thrown if fileUploadId is null or empty.</exception>
		public ProcessImagesViewModel(in IBaseModelData modelData,
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

		/// <summary>
		/// Name of subgroup to which the options apply
		/// </summary>
		/// <value>string</value>
		public string SubgroupName { get; set; }

		/// <summary>
		/// Indicates whether the subgroup will be shown or not
		/// </summary>
		/// <value>bool</value>
		public bool ShowSubgroup { get; set; }

		/// <summary>
		/// Name of additional data
		/// </summary>
		/// <value>string</value>
		public string AdditionalDataName { get; set; }

		/// <summary>
		/// Additional data supplied by user
		/// </summary>
		/// <value>string</value>
		public string AdditionalData { get; set; }

		/// <summary>
		/// Indicates that additional data is mandatory
		/// </summary>
		/// <value>bool</value>
		public bool AdditionalDataMandatory { get; set; }

		#endregion Properties
	}
}
