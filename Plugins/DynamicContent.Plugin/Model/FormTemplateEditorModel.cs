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
 *  File: FormTemplateEditorModel.cs
 *
 *  Purpose:  generic template editor for form input controls
 *
 *  Date        Name                Reason
 *  07/07/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace DynamicContent.Plugin.Model
{
    public sealed class FormTemplateEditorModel
    {
        #region Private Members

        private string _data;

        #endregion Private Members

        #region Constructors

        public FormTemplateEditorModel(string data, bool allowAlignRight)
        {
            UpdateTemplateEditorModel(data);

            if (allowAlignRight)
            {
                AlignLeftText = Languages.LanguageStrings.AppTextAlignmentRight;
                AutoWidth = false;
            }
            else
            {
                AlignLeftText = Languages.LanguageStrings.AppTextAlignmentLeft;
                AutoWidth = true;
            }
        }

        public FormTemplateEditorModel(string data)
            : this(data, false)
        {

        }

        #endregion Constructors

        #region Properties

        public string ControlName { get; private set; }

        public string LabelText { get; private set; }

        public string AlignLeftText { get; }

        public bool AlignTop { get; private set; }

        public string LabelStyle { get; private set; }

        public string ControlStyle { get; private set; }

        public string[] Options { get; private set; }

        public string Data
        {
            get
            {
                return _data;
            }

            set
            {
                UpdateTemplateEditorModel(value);
            }
        }

        public bool AutoWidth { get; }

        #endregion Properties

        #region Private Methods

        private void UpdateTemplateEditorModel(string data)
        {
            if (String.IsNullOrEmpty(data))
                data = Constants.PipeString;

            string[] parts = data.Split(Constants.PipeChar, StringSplitOptions.None);

            if (parts.Length > 0)
                ControlName = HtmlHelper.RouteFriendlyName(parts[0]);

            if (parts.Length > 1)
                LabelText = parts[1];
            else
                LabelText = String.Empty;

            if (parts.Length > 2)
                AlignTop = parts[2].Equals(Boolean.TrueString, StringComparison.InvariantCultureIgnoreCase);

            if (parts.Length > 3)
                LabelStyle = parts[3];
            else
                LabelStyle = String.Empty;

            if (parts.Length > 4)
                ControlStyle = parts[4];
            else
                ControlStyle = String.Empty;

            if (parts.Length > 5)
                Options = parts[5].Split(";", StringSplitOptions.RemoveEmptyEntries);
            else
                Options = new string[] { };

            _data = data;
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591