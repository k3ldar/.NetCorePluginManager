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
 *  Product:  PluginMiddleware
 *  
 *  File: ResourceBookmarkDataRow.cs
 *
 *  Purpose:  Table definition for user resource bookmarks
 *
 *  Date        Name                Reason
 *  25/10/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables.Resources
{
	[Table(Constants.TableDomainResources, Constants.TableNameResourceBookmarks, WriteStrategy.Lazy)]
	internal sealed class ResourceBookmarkDataRow : TableRowDefinition
	{
		private long _resourceId;

		[ForeignKey(Constants.TableNameResourceItems)]
		[UniqueIndex("Idx_BookmarkItem")]
		public long ResourceId
		{
			get => _resourceId;


			set
			{
				if (value == _resourceId)
					return;

				_resourceId = value;
				Update();
			}
		}

		private long _userId;

		[ForeignKey(Constants.TableNameUsers)]
		[UniqueIndex("Idx_BookmarkItem")]
		public long UserId
		{
			get => _userId;


			set
			{
				if (value == _userId)
					return;

				_userId = value;
				Update();
			}
		}
	}
}
