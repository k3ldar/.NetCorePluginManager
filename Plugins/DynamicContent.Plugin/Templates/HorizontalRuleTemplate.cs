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
 *  File: HorizontalRuleTemplate.cs
 *
 *  Purpose:  Html hr template for dynamic pages
 *
 *  Date        Name                Reason
 *  08/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Text;

using Languages;

using SharedPluginFeatures;
using SharedPluginFeatures.DynamicContent;

#pragma warning disable CS1591

namespace DynamicContent.Plugin.Templates
{
    public class HorizontalRuleTemplate : DynamicContentTemplate
    {
        #region Constructors

        public HorizontalRuleTemplate()
        {
            WidthType = DynamicContentWidthType.Columns;
            Width = 12;
            ActiveFrom = new DateTime(2020, 1, 1, 0, 0, 0);
            ActiveTo = new DateTime(2050, 12, 31, 23, 59, 59);
        }

        #endregion Constructors

        #region DynamicContentTemplate Properties

        public override string AssemblyQualifiedName => typeof(HorizontalRuleTemplate).AssemblyQualifiedName;

        public override string EditorAction => String.Empty;

        public override string EditorInstructions => String.Empty;

        public override string Name => LanguageStrings.TemplateNameHorizontalRule;

        public override DynamicContentTemplateType TemplateType => DynamicContentTemplateType.Default;

        public override int TemplateSortOrder => 11000;

        public override Int32 SortOrder { get; set; }

        public override DynamicContentHeightType HeightType
        {
            get
            {
                return DynamicContentHeightType.Automatic;
            }

            set
            {
				// not used but required for interface
			}
		}

        public override int Height
        {
            get
            {
                return -1;
            }

            set
            {
				// not used but required for interface
			}
		}

        public override DynamicContentWidthType WidthType { get; set; }

        public override int Width { get; set; }

        public override string Data
        {
            get
            {
                StringBuilder Result = new StringBuilder(256);

                Result.AppendFormat("<hr{0} />", RetrieveCssClassAndStyle());

                return Result.ToString();
            }

            set
            {
				// not used but required for interface
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

            return new HorizontalRuleTemplate()
            {
                UniqueId = uniqueId
            };
        }

        #endregion DynamicContentTemplate Methods

        #region Private Methods

        private string GenerateContent(bool isEditing)
        {
            StringBuilder Result = new StringBuilder(256);

            HtmlStart(Result, isEditing);

            Result.Append(Data);

            HtmlEnd(Result);

            return Result.ToString();
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591