/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  DynamicContent.Plugin
 *  
 *  File: YoutubeVideo.cs
 *
 *  Purpose:  Template for YouTube videos
 *
 *  Date        Name                Reason
 *  19/06/2021  Simon Carter        Initially Created
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
	public class YouTubeVideoTemplate : DynamicContentTemplate
	{
		#region Private Members

		private string _youTubeId;
		private bool _autoPlay;

		#endregion Private Members

		#region Constructors

		public YouTubeVideoTemplate()
		{
			Width = 12;
			WidthType = DynamicContentWidthType.Columns;
			Height = 450;
			HeightType = DynamicContentHeightType.Pixels;
			ActiveFrom = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			ActiveTo = new DateTime(2050, 12, 31, 23, 59, 59, DateTimeKind.Utc);
			_autoPlay = true;
		}

		#endregion Constructors

		#region DynamicContentTemplate Properties

		public override String AssemblyQualifiedName => typeof(YouTubeVideoTemplate).AssemblyQualifiedName;

		public override String EditorAction
		{
			get
			{
				return $"/{Controllers.DynamicContentController.Name}/{nameof(Controllers.DynamicContentController.YouTubeTemplateEditor)}/";
			}
		}

		public override string EditorInstructions => String.Empty;

		public override String Name => LanguageStrings.TemplateNameYouTube;

		public override DynamicContentTemplateType TemplateType => DynamicContentTemplateType.Default;

		public override int TemplateSortOrder => 600;

		public override Int32 SortOrder { get; set; }

		public override DynamicContentWidthType WidthType { get; set; }

		public override Int32 Width { get; set; }

		public override DynamicContentHeightType HeightType { get; set; }

		public override Int32 Height { get; set; }

		public override String Data
		{
			get
			{
				return $"{_youTubeId}|{_autoPlay}";
			}

			set
			{
				string[] parts = value.Split('|', StringSplitOptions.RemoveEmptyEntries);

				if (parts.Length > 0)
					_youTubeId = parts[0];

				if (parts.Length > 1)
					_autoPlay = parts[1].Equals(Boolean.TrueString, StringComparison.InvariantCultureIgnoreCase);
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

			return new YouTubeVideoTemplate()
			{
				UniqueId = uniqueId
			};
		}

		#endregion DynamicContentTemplate Methods

		#region Private Methods

		private string GenerateContent(bool isEditing)
		{
			StringBuilder Result = new(1024);

			HtmlStart(Result, isEditing);

			if (String.IsNullOrEmpty(_youTubeId))
			{
				Result.Append("<p>Please enter a valid video Id</p>");
			}
			else
			{
				Result.AppendFormat("<iframe{0} ", RetrieveCssClassAndStyle());

				Result.Append("type=\"text/html\" width=\"100%\" height=\"100%\" src=\"https://www.youtube.com/embed/");

				Result.Append(_youTubeId);

				if (_autoPlay)
					Result.Append("?autoplay=1\"");
				else
					Result.Append('\"');

				Result.Append(" frameborder=\"0\"");

				if (_autoPlay)
					Result.Append(" allow=\"autoplay\"");

				Result.Append("></iframe>");
			}

			HtmlEnd(Result);

			return Result.ToString();
		}

		#endregion Private Methods
	}
}

#pragma warning restore CS1591