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
 *  File: ImageTemplate.cs
 *
 *  Purpose:  Image template for dynamic pages
 *
 *  Date        Name                Reason
 *  12/06/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Text;

using Languages;

using SharedPluginFeatures;
using SharedPluginFeatures.DynamicContent;

#pragma warning disable CS1591

namespace ImageManager.Plugin.Templates
{
    public sealed class ImageTemplate : DynamicContentTemplate
    {
        #region Constructors

        public ImageTemplate()
        {
            Width = 3;
            WidthType = DynamicContentWidthType.Columns;
            Height = 200;
            HeightType = DynamicContentHeightType.Pixels;
            ActiveFrom = new DateTime(2020, 1, 1, 0, 0, 0);
            ActiveTo = new DateTime(2050, 12, 31, 23, 59, 59);
            Data = String.Empty;
        }

        #endregion Constructors

        #region DynamicContentTemplate Properties

        public override string AssemblyQualifiedName => typeof(ImageTemplate).AssemblyQualifiedName;

        public override string EditorAction
        {
            get
            {
                return $"/{Controllers.ImageManagerController.Name}/{nameof(Controllers.ImageManagerController.ImageTemplateEditor)}/";
            }
        }

        public override string EditorInstructions => String.Empty;

        public override string Name => LanguageStrings.TemplateNameImage;

        public override DynamicContentTemplateType TemplateType => DynamicContentTemplateType.Default;

        public override int TemplateSortOrder => 400;

        public override int SortOrder { get; set; }

        public override DynamicContentWidthType WidthType { get; set; }

        public override int Width { get; set; }

        public override DynamicContentHeightType HeightType { get; set; }

        public override int Height { get; set; }

        public override string Data { get; set; }

        public override DateTime ActiveFrom { get; set; }

        public override DateTime ActiveTo { get; set; }

        #endregion DynamicContentTemplate Properties

        #region DynamicContentTemplate Methods

        public override DynamicContentTemplate Clone(string uniqueId)
        {
            if (String.IsNullOrEmpty(uniqueId))
                uniqueId = Guid.NewGuid().ToString();

            return new ImageTemplate()
            {
                UniqueId = uniqueId
            };
        }

        public override string Content()
        {
            return GenerateContent(false);
        }

        public override string EditorContent()
        {
            return GenerateContent(true);
        }

        #endregion DynamicContentTemplate Methods

        #region Private Methods

        private string GenerateContent(bool isEditing)
        {
            StringBuilder Result = new StringBuilder(2048);

            HtmlStart(Result, isEditing);

            if (String.IsNullOrEmpty(Data))
            {
                Result.Append("<p>Please select an image</p>");
            }
            else
            {
                Result.Append("<img src=\"");
                Result.Append(Data);
                Result.Append("\" alt=\"image\" style=\"max-height:100%;");

                if (isEditing)
                {
                    Result.Append("width:100%;\"");
                }
                else
                {
                    Result.Append(CssStyle);
                    Result.Append('\"');

                    if (!String.IsNullOrEmpty(CssClassName))
                    {
                        Result.AppendFormat(" class=\"{0}\"", CssClassName);
                    }
                }

                Result.Append('>');
            }

            HtmlEnd(Result);

            return Result.ToString();
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591