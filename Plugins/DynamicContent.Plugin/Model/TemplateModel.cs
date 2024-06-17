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
 *  File: TemplateModel.cs
 *
 *  Purpose:  Dynamic content templates model
 *
 *  Date        Name                Reason
 *  31/12/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

#pragma warning disable CS1591

namespace DynamicContent.Plugin.Model
{
	public sealed class TemplateModel
	{
		#region Constructors

		public TemplateModel(string templateId, string templateName, string templateImage)
		{
			if (String.IsNullOrEmpty(templateId))
				throw new ArgumentNullException(nameof(templateId));

			if (String.IsNullOrEmpty(templateName))
				throw new ArgumentNullException(nameof(templateName));

			if (String.IsNullOrEmpty(templateImage))
				throw new ArgumentNullException(nameof(templateImage));

			TemplateId = templateId;
			TemplateName = templateName;
			TemplateImage = templateImage;
		}

		#endregion Constructors

		#region Properties

		public string TemplateId { get; }

		public string TemplateName { get; }

		public string TemplateImage { get; }

		#endregion Properties
	}
}

#pragma warning restore CS1591