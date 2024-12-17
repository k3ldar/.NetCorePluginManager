﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Product:  PluginMiddleware
 *  
 *  File: GenericTextTemplate.cs
 *
 *  Purpose:  Dynamic content page
 *
 *  Date        Name                Reason
 *  17/11/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Text;

using SharedPluginFeatures;
using SharedPluginFeatures.DynamicContent;

#pragma warning disable CS1591, S3237

namespace Middleware.Classes.DynamicContent
{
	/// <summary>
	/// Generic Text Template
	/// </summary>
	public sealed class GenericTextTemplate : DynamicContentTemplate
	{
		#region Constructors

		public GenericTextTemplate()
		{
			WidthType = DynamicContentWidthType.Columns;
			Width = 12;
			ActiveFrom = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			ActiveTo = new DateTime(2050, 12, 31, 23, 59, 59, DateTimeKind.Utc);
		}

		#endregion Constructors

		#region DynamicContentTemplate Properties

		public override string AssemblyQualifiedName => typeof(GenericTextTemplate).AssemblyQualifiedName;

		public override string EditorAction
		{
			get
			{
				return "/DynamicContent/TextTemplateEditor/";
			}
		}

		public override string EditorInstructions => String.Empty;

		public override string Name
		{
			get
			{
				return "TemplateNameGenericTextHtml";
			}
		}

		public override DynamicContentTemplateType TemplateType => DynamicContentTemplateType.Default;

		public override int TemplateSortOrder => 200;

		public override Int32 SortOrder { get; set; }

		public override DynamicContentHeightType HeightType
		{
			get
			{
				return DynamicContentHeightType.Automatic;
			}

			set
			{
				// required from interface but not used
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
				// required from interface but not used
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

			return new GenericTextTemplate()
			{
				UniqueId = uniqueId
			};
		}

		#endregion DynamicContentTemplate Methods

		#region Private Methods

		private string GenerateContent(bool isEditing)
		{
			StringBuilder Result = new(2048);

			HtmlStart(Result, isEditing);

			Result.Append(Data);

			HtmlEnd(Result);

			return Result.ToString();
		}

		#endregion Private Methods
	}
}

#pragma warning restore CS1591, S3237