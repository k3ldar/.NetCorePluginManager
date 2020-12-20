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

        #endregion DynamicContentTemplate Properties
    }
}
