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
 *  File: DynamicContentModel.cs
 *
 *  Purpose:  Dynamic content result model
 *
 *  Date        Name                Reason
 *  20/12/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace DynamicContent.Plugin.Model
{
	public sealed class PageModel : BaseModel
	{
		public PageModel(BaseModelData modelData, string path, string content, string css, string[] dynamicContentIds, bool hasDataSaved)
			: base(modelData)
		{
			if (String.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			Path = path;
			Content = content ?? throw new ArgumentNullException(nameof(content));
			PageCSS = css;
			DynamicContentIds = dynamicContentIds ?? [];
			HasDataSaved = hasDataSaved;
		}

		#region Properties

		public string Path { get; }

		public string Content { get; }

		public string PageCSS { get; }

		public string[] DynamicContentIds { get; }

		public bool HasInputControls => DynamicContentIds.Length > 0;

		public bool HasDataSaved { get; }

		#endregion Properties
	}
}

#pragma warning restore CS1591