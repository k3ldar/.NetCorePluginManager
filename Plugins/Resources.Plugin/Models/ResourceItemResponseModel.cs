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
 *  Product:  Resources.Plugin
 *  
 *  File: IdModel.cs
 *
 *  Purpose:  Model used for view that needs an id
 *
 *  Date        Name                Reason
 *  21/09/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
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
 *  Product:  Resources.Plugin
 *  
 *  File: ResourceItemResponseModel.cs
 *
 *  Purpose:  Model used for resource item statistics
 *
 *  Date        Name                Reason
 *  22/09/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
namespace Resources.Plugin.Models
{
	/// <summary>
	/// Resource item response model
	/// </summary>
	public class ResourceItemResponseModel
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id"></param>
		/// <param name="likes"></param>
		/// <param name="dislikes"></param>
		/// <param name="viewCount"></param>
		/// <param name="isEnabled"></param>
		public ResourceItemResponseModel(long id, int likes, int dislikes, int viewCount, bool isEnabled)
		{
			Id = id;
			Likes = likes;
			Dislikes = dislikes;
			ViewCount = viewCount;
			IsEnabled = isEnabled;
		}

		/// <summary>
		/// Id of resource
		/// </summary>
		public long Id { get; }

		/// <summary>
		/// Number of likes for resource
		/// </summary>
		public int Likes { get; }

		/// <summary>
		/// Number of dislikes for resource
		/// </summary>
		public int Dislikes { get; }

		/// <summary>
		/// Number of views
		/// </summary>
		public int ViewCount { get; }

		/// <summary>
		/// Is response enabled
		/// </summary>
		public bool IsEnabled { get; }
	}
}
