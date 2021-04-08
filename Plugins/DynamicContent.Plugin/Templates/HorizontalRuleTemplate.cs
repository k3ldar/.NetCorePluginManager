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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
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

namespace DynamicContent.Plugin.Templates
{
    public class HorizontalRuleTemplate : DynamicContentTemplate
    {
        #region Constructors

        public HorizontalRuleTemplate()
        {
            WidthType = DynamicContentWidthType.Columns;
            Width = 12;
            ActiveFrom = DateTime.MinValue;
            ActiveTo = DateTime.MaxValue;
        }

        #endregion Constructors

        #region DynamicContentTemplate Properties

        public override string AssemblyQualifiedName => typeof(HorizontalRuleTemplate).AssemblyQualifiedName;

        public override string EditorAction => String.Empty;

        public override string Name => LanguageStrings.TemplateNameHorizontalRule;

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

        public override string Data
        {
            get
            {
                return "<hr />";
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
            StringBuilder Result = new StringBuilder(256);

            HtmlStart(Result);

            Result.Append(Data);

            HtmlEnd(Result);

            return Result.ToString();
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
    }
}
