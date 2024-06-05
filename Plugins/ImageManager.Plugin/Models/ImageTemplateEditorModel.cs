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
 *  Product:  DynamicContent.Plugin
 *  
 *  File: ImageTemplateEditorModel.cs
 *
 *  Purpose:  image template editor model
 *
 *  Date        Name                Reason
 *  12/06/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Middleware.Images;
using Middleware.Interfaces;

#pragma warning disable CS1591

namespace ImageManager.Plugin.Models
{
    public sealed class ImageTemplateEditorModel
    {
        #region Private Members

        private readonly IImageProvider _imageProvider;

        #endregion Private Members

        #region Constructors

        public ImageTemplateEditorModel(IImageProvider imageProvider, string data)
        {
            _imageProvider = imageProvider ?? throw new ArgumentNullException(nameof(imageProvider));
            Data = data ?? string.Empty;

            if (String.IsNullOrEmpty(data) || !data.StartsWith("/images/", StringComparison.InvariantCultureIgnoreCase))
            {
                CreateGroupListNoExistingData();
            }
            else
            {
                string[] parts = data.Split('/', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 4)
                {
                    ActiveGroup = parts[1];
                    ActiveSubgroup = parts[2];
                    ActiveFile = parts[3];
                }
                else if (parts.Length == 3)
                {
                    ActiveGroup = parts[1];
                    ActiveFile = parts[2];
                }

                if (!CreateSubgroupsAndImagesForData())
                    CreateGroupListNoExistingData();
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Image template data
        /// </summary>
        /// <value>string</value>
        public string Data { get; set; }

        public string[] Groups { get; private set; }

        public string[] Subgroups { get; private set; }

        public string[] Images { get; private set; }

        public string ActiveGroup { get; }

        public string ActiveSubgroup { get; }

        public string ActiveFile { get; }

        #endregion Properties

        #region Private Methods

        private bool CreateSubgroupsAndImagesForData()
        {
            if (!_imageProvider.GroupExists(ActiveGroup))
                return false;

            Dictionary<string, List<string>> allGroups = _imageProvider.Groups();

            List<string> groups = new();

            foreach (KeyValuePair<string, List<string>> item in allGroups)
            {
                groups.Add(item.Key);
            }

            Groups = groups.ToArray();
            Subgroups = allGroups[ActiveGroup].ToArray();
            SetImagesForCurrentGroupAndSubgroup();

            return true;
        }

        private void SetImagesForCurrentGroupAndSubgroup()
        {
            List<ImageFile> files = null;

            if (String.IsNullOrEmpty(ActiveGroup))
                files = new List<ImageFile>();
            else if (String.IsNullOrEmpty(ActiveSubgroup))
                files = _imageProvider.Images(ActiveGroup);
            else
                files = _imageProvider.Images(ActiveGroup, ActiveSubgroup);

            List<string> fileNames = new();
            files.ForEach(f => fileNames.Add(f.Name));

            Images = fileNames.ToArray();
        }

        private void CreateGroupListNoExistingData()
        {
            List<string> groups = new();
            List<string> subgroups = new();
            bool firstGroup = true;

            foreach (KeyValuePair<string, List<string>> item in _imageProvider.Groups())
            {
                groups.Add(item.Key);

                if (firstGroup)
                {
                    firstGroup = false;

                    foreach (string subgroup in item.Value)
                        subgroups.Add(subgroup);
                }
            }

            Groups = groups.ToArray();
            Subgroups = subgroups.ToArray();
            SetImagesForCurrentGroupAndSubgroup();
        }

        #endregion Private Methods
    }

}

#pragma warning restore CS1591