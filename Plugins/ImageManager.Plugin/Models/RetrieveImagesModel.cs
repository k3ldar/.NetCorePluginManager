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
 *  File: RetrieveImagesModel.cs
 *
 *  Purpose:  Model for retrieving subgroups and images for a given group
 *
 *  Date        Name                Reason
 *  16/06/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

#pragma warning disable CS1591

namespace ImageManager.Plugin.Models
{
	public sealed class RetrieveImagesModel
	{
		public RetrieveImagesModel(List<string> subgroups, List<string> images)
		{
			Subgroups = subgroups ?? throw new ArgumentNullException(nameof(subgroups));
			Images = images ?? throw new ArgumentNullException(nameof(images));
		}

		public List<string> Subgroups { get; set; }

		public List<string> Images { get; set; }
	}
}

#pragma warning restore CS1591