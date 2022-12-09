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
 *  Product:  DynamicContent.Plugin
 *  
 *  File: FormCheckBoxTemplate.cs
 *
 *  Purpose:  Html form check box template for dynamic pages
 *
 *  Date        Name                Reason
 *  13/07/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Text;

using DynamicContent.Plugin.Model;

using Languages;

using SharedPluginFeatures;
using SharedPluginFeatures.DynamicContent;

#pragma warning disable CS1591

namespace DynamicContent.Plugin.Templates
{
    public class FormCheckBoxTemplate : DynamicContentTemplate
    {
        #region Constructors

        public FormCheckBoxTemplate()
        {
            WidthType = DynamicContentWidthType.Columns;
            Width = 12;
            ActiveFrom = new DateTime(2020, 1, 1, 0, 0, 0);
            ActiveTo = new DateTime(2050, 12, 31, 23, 59, 59);
        }

        #endregion Constructors

        #region DynamicContentTemplate Properties

        public override string AssemblyQualifiedName => typeof(FormCheckBoxTemplate).AssemblyQualifiedName;

        public override string EditorAction
        {
            get
            {
                return $"/{Controllers.DynamicContentController.Name}/{nameof(Controllers.DynamicContentController.FormControlTemplateEditorRightAlign)}/";
            }
        }

        public override string EditorInstructions => String.Empty;

        public override string Name => LanguageStrings.TemplateNameFormCheckBox;

        public override DynamicContentTemplateType TemplateType => DynamicContentTemplateType.Input;

        public override int TemplateSortOrder => DefaultFormTemplateSortOrder;

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

        public override string Data { get; set; }

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

            return new FormCheckBoxTemplate()
            {
                UniqueId = uniqueId
            };
        }

        #endregion DynamicContentTemplate Methods

        #region Private Methods

        private string GenerateContent(bool isEditing)
        {
            StringBuilder Result = new StringBuilder(512);

            HtmlStart(Result, isEditing);

            Result.AppendFormat("<div{0}", RetrieveCssClassAndStyle("form-group"));

            if (isEditing)
                Result.Append(" style=\"margin: 0 0 0 10px;min-height:32px;\"");

            Result.Append('>');

            FormTemplateEditorModel formModel = new FormTemplateEditorModel(Data, true);
            string lblStyle = String.IsNullOrEmpty(formModel.LabelStyle) ? "" : $" style=\"{formModel.LabelStyle}\"";

            if (formModel.AlignTop && !String.IsNullOrEmpty(formModel.LabelText))
            {
                Result.AppendFormat("<label for=\"{0}\" class=\"form-check-label\"{1}>{2}</label>", formModel.ControlName, lblStyle, formModel.LabelText);

                if (formModel.AlignTop)
                    Result.Append("<br />");
            }

            string disabled = isEditing ? " disabled" : "";
            string ctlStyle = String.IsNullOrEmpty(formModel.ControlStyle) ? "" : $" style=\"{formModel.ControlStyle}\"";

            Result.AppendFormat("<input type=\"checkbox\" class=\"form-check-input\" id=\"{0}\" onclick=\"updateUC();\" onfocusout=\"updateUC();\"{1}{2}>", formModel.ControlName, ctlStyle, disabled);

            if (!formModel.AlignTop && !String.IsNullOrEmpty(formModel.LabelText))
            {
                Result.AppendFormat("<label for=\"{0}\" class=\"form-check-label\"{1}>{2}</label>", formModel.ControlName, lblStyle, formModel.LabelText);
            }

            Result.Append("</div>");

            HtmlEnd(Result);

            return Result.ToString();
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591