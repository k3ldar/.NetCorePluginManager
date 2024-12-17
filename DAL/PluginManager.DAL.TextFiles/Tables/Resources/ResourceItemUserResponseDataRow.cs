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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: ResourceItemUserResponseDataRow.cs
 *
 *  Purpose:  Table definition for user likes/dislikes
 *
 *  Date        Name                Reason
 *  27/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
	[Table(Constants.TableDomainResources, Constants.TableNameResourceResponse)]
	internal class ResourceItemUserResponseDataRow : TableRowDefinition
	{
		private long _userId;
		private long _resourceItemId;
		private bool _like;

		[ForeignKey(Constants.TableNameUsers)]
		public long UserId
		{
			get => _userId;

			set
			{
				if (_userId == value)
					return;

				_userId = value;
				Update();
			}
		}

		[ForeignKey(Constants.TableNameResourceItems)]
		public long ResourceItemId
		{
			get => _resourceItemId;

			set
			{
				if (_resourceItemId == value)
					return;

				_resourceItemId = value;
				Update();
			}
		}

		public bool Like
		{
			get => _like;

			set
			{
				if (_like == value)
					return;

				_like = value;
				Update();
			}
		}
	}
}
