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
 *  File: FormListBoxTemplate.cs
 *
 *  Purpose:  Html form list box template for dynamic pages
 *
 *  Date        Name                Reason
 *  17/07/2021  Simon Carter        Initially Created
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
    public class FormListBoxTemplate : DynamicContentTemplate
    {
        #region Constructors

        public FormListBoxTemplate()
        {
            WidthType = DynamicContentWidthType.Columns;
            Width = 12;
            ActiveFrom = new DateTime(2020, 1, 1, 0, 0, 0);
            ActiveTo = new DateTime(2050, 12, 31, 23, 59, 59);
        }

        #endregion Constructors

        #region DynamicContentTemplate Properties

        public override string AssemblyQualifiedName => typeof(FormListBoxTemplate).AssemblyQualifiedName;

        public override string EditorAction
        {
            get
            {
                return $"/{Controllers.DynamicContentController.Name}/{nameof(Controllers.DynamicContentController.FormControlTemplateEditorListBox)}/";
            }
        }

        public override string EditorInstructions => String.Empty;

        public override string Name => LanguageStrings.TemplateNameFormListBox;

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

            return new FormListBoxTemplate()
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

            FormTemplateEditorModel formModel = new FormTemplateEditorModel(Data);
            string lblStyle = String.IsNullOrEmpty(formModel.LabelStyle) ? "" : $" style=\"{formModel.LabelStyle}\"";

            string disabled = isEditing ? " disabled" : "";
            string ctlStyle = String.IsNullOrEmpty(formModel.ControlStyle) ? "" : $" style=\"{formModel.ControlStyle}\"";

            if (!String.IsNullOrEmpty(formModel.LabelText))
            {
                Result.AppendFormat("<label for=\"{0}\"{1}>{2}</label>",
                    HtmlHelper.RouteFriendlyName(formModel.ControlName), lblStyle, formModel.LabelText);
            }

            if (formModel.AlignTop)
            {
                Result.Append("<br />");
            }

            Result.AppendFormat("<select name=\"{0}\" id=\"{0}\" onclick=\"updateUC();\" onfocusout=\"updateUC();\"{1}{2}>",
                HtmlHelper.RouteFriendlyName(formModel.ControlName), ctlStyle, disabled);
            bool first = true;

            foreach (string option in formModel.Options)
            {
                string optionText = option.Trim();
                string routeOption = HtmlHelper.RouteFriendlyName(optionText);

                if (first)
                {
                    Result.AppendFormat("<option value=\"{0}\" selected>{1}</option>",
                        routeOption, optionText, ctlStyle, disabled);
                    first = false;
                }
                else
                {
                    Result.AppendFormat("<option value=\"{0}\">{1}</option>",
                        routeOption, optionText, ctlStyle, disabled);
                }
            }

            Result.Append("</select></div>");

            HtmlEnd(Result);

            return Result.ToString();
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591