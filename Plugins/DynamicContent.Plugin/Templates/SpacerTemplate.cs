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
 *  Product:  DynamicContent.Plugin
 *  
 *  File: SpacerTemplate.cs
 *
 *  Purpose:  Spacer template for dynamic pages
 *
 *  Date        Name                Reason
 *  25/12/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Text;

using Languages;

using SharedPluginFeatures;
using SharedPluginFeatures.DynamicContent;

namespace DynamicContent.Plugin.Templates
{
    public sealed class SpacerTemplate : DynamicContentTemplate
    {
        #region Constructors

        public SpacerTemplate()
        {
            Width = 2;
            WidthType = DynamicContentWidthType.Columns;
            Height = 200;
            HeightType = DynamicContentHeightType.Pixels;
            ActiveFrom = DateTime.MinValue;
            ActiveTo = DateTime.MaxValue;
        }

        #endregion Constructors

        #region DynamicContentTemplate Properties

        public override String AssemblyQualifiedName => typeof(SpacerTemplate).AssemblyQualifiedName;

        public override String EditorAction => String.Empty;

        public override string EditorInstructions => String.Empty;

        public override String Name => LanguageStrings.TemplateNameSpacer;

        public override Int32 SortOrder { get; set; }

        public override DynamicContentWidthType WidthType { get; set; }

        public override Int32 Width { get; set; }

        public override DynamicContentHeightType HeightType { get; set; }

        public override Int32 Height { get; set; }

        public override String Data
        {
            get
            {
                return String.Empty;
            }

            set
            {
            }
        }

        public override DateTime ActiveFrom { get; set; }

        public override DateTime ActiveTo { get; set; }

        #endregion DynamicContentTemplate Properties

        #region DynamicContentTemplate Methods

        public override String Content()
        {
            return GenerateContent(false);
        }

        public override string EditorContent()
        {
            return GenerateContent(true);
        }

        public override DynamicContentTemplate Clone(string uniqueId)
        {
            if (String.IsNullOrEmpty(uniqueId))
                uniqueId = Guid.NewGuid().ToString();

            return new SpacerTemplate()
            {
                UniqueId = uniqueId
            };
        }

        #endregion DynamicContentTemplate Methods

        #region Private Methods

        private string GenerateContent(bool isEditing)
        {
            StringBuilder Result = new StringBuilder(1024);

            HtmlStart(Result, isEditing);

            Result.Append("<p>&nbsp;</p>");

            HtmlEnd(Result);

            return Result.ToString();
        }

        #endregion Private Methods
    }
}
