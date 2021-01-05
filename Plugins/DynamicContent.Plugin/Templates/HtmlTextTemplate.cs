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
 *  File: HtmlTextTemplate.cs
 *
 *  Purpose:  Html text template for dynamic pages
 *
 *  Date        Name                Reason
 *  22/11/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Text;

using Languages;

using SharedPluginFeatures;
using SharedPluginFeatures.DynamicContent;

namespace DynamicContent.Plugin.Templates
{
    public class HtmlTextTemplate : DynamicContentTemplate
    {
        #region Private Members

        private DynamicContentWidthType _widthType = DynamicContentWidthType.Columns;
        private int _width = 12;

        #endregion Private Members

        #region DynamicContentTemplate Properties

        public override string AssemblyQualifiedName => typeof(HtmlTextTemplate).AssemblyQualifiedName;

        public override string EditorAction
        {
            get
            {
                return $"/{Controllers.DynamicContentController.Name}/{nameof(Controllers.DynamicContentController.TextTemplateEditor)}/";
            }
        }

        public override string Name
        {
            get
            {
                return LanguageStrings.TemplateNameHtml;
            }
        }

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

        public override DynamicContentWidthType WidthType
        {
            get
            {
                return _widthType;
            }

            set
            {
                _widthType = value;
            }
        }

        public override int Width
        {
            get
            {
                return _width;
            }

            set
            {
                _width = value;
            }
        }

        public override string Data { get; set; }

        public override String Content()
        {
            StringBuilder Result = new StringBuilder(2048);

            HtmlStart(Result);

            Result.Append(Data);

            HtmlEnd(Result);

            return Result.ToString();
        }

        public override DateTime ActiveFrom { get; set; }

        public override DateTime ActiveTo { get; set; }

        #endregion DynamicContentTemplate Properties
    }
}
