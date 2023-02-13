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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: ResourceItemDataRow.cs
 *
 *  Purpose:  Table definition for resource items
 *
 *  Date        Name                Reason
 *  01/09/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
	[Table(Constants.TableDomainResources, Constants.TableNameResourceItems, WriteStrategy.Lazy)]
	internal class ResourceItemDataRow : TableRowDefinition
	{
		private long _categoryId;
		private int _resourceType;
		private long _userId;
		private string _userName;
		private string _name;
		private string _description;
		private string _value;
		private int _likes;
		private int _dislikes;
		private int _viewCount;
		private bool _approved;
		private ObservableList<string> _tags;

		public ResourceItemDataRow()
		{
			_tags = new();
			_tags.Changed += ObservableDataChanged;
		}

		[ForeignKey(Constants.TableNameResourceCateogories)]
		[UniqueIndex("Idx_CategoryName")]
		public long CategoryId
		{
			get => _categoryId;

			set
			{
				if (_categoryId == value)
					return;

				_categoryId = value;
				Update();
			}
		}

		public int ResourceType
		{
			get => _resourceType;

			set
			{
				if (_resourceType == value)
					return;

				_resourceType = value;
				Update();
			}
		}

		[ForeignKey(Constants.TableNameUsers, true)]
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

		public string UserName
		{
			get => _userName;

			set
			{
				if (_userName == value)
					return;

				_userName = value;
				Update();
			}
		}

		[UniqueIndex("Idx_CategoryName")]
		public string Name 
		{
			get => _name;

			set
			{
				if (_name == value)
					return;

				_name = value;
				Update();
			}
		}

		public string Description 
		{
			get => _description;

			set
			{
				if (_description == value)
					return;

				_description = value;
				Update();
			}
		}

		public string Value 
		{
			get => _value;

			set
			{
				if (_value == value)
					return;

				_value = value;
				Update();
			}
		}

		public int Likes 
		{
			get => _likes;

			set
			{
				if (_likes == value)
					return;

				_likes = value;
				Update();
			}
		}

		public int Dislikes
		{
			get => _dislikes;

			set
			{
				if (_dislikes == value)
					return;

				_dislikes = value;
				Update();
			}
		}

		public bool Approved
		{
			get => _approved;

			set
			{
				if (_approved == value)
					return;

				_approved = value;
				Update();
			}
		}

		public int ViewCount
		{
			get => _viewCount;

			set
			{
				if (_viewCount == value)
					return;

				_viewCount = value;
				Update();
			}
		}

		public ObservableList<string> Tags
		{
			get => _tags;

			set
			{
				if (value == _tags)
					return;

				if (_tags != null)
					_tags.Changed -= ObservableDataChanged;

				_tags = value;
				_tags.Changed += ObservableDataChanged;
				Update();
			}
		}
	}
}
