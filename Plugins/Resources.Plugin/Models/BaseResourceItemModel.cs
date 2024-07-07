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
 *  Product:  Resources.Plugin
 *  
 *  File: BaseResourceItemModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  30/10/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;
using System;

using SharedPluginFeatures;
using Middleware;

namespace Resources.Plugin.Models
{
	/// <summary>
	/// Base resource item model
	/// </summary>
	public class BaseResourceItemModel : BaseModel
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="modelData"></param>
		public BaseResourceItemModel(BaseModelData modelData)
			: base(modelData)
		{
		}

		/// <summary>
		/// Default Constructor
		/// </summary>
		public BaseResourceItemModel()
			: base()
		{

		}

		/// <summary>
		/// All resource item types
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Left as non static as used in belongs to a base class")]
		public List<NameIdModel> AllResourceTypes
		{
			get
			{
				List<NameIdModel> Result = [];

				foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
				{
					Result.Add(new NameIdModel((long)resourceType, resourceType.ToString()));
				}

				return Result;
			}
		}
	}
}
