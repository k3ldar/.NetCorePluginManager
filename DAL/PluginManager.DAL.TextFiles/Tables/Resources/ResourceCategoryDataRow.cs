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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: ResourceCategoryDataRow.cs
 *
 *  Purpose:  Table definition for primary resources
 *
 *  Date        Name                Reason
 *  27/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
	[Table(Constants.TableDomainResources, Constants.TableNameResourceCateogories)]
	internal class ResourceCategoryDataRow : TableRowDefinition
	{
		private long _userId;
		private long _parentCategoryId;
		private string _name;
		private string _description;
		private string _foreColor;
		private string _backColor;
		private string _image;
		private string _routeName;
		private bool _isVisible;

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

		[ForeignKey(Constants.TableNameResourceCateogories, true)]
		public long ParentCategoryId
		{
			get => _parentCategoryId;

			set
			{
				if (value == _parentCategoryId)
					return;

				_parentCategoryId = value;
				Update();
			}
		}

		[UniqueIndex]
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

		public string ForeColor
		{
			get => _foreColor;

			set
			{
				if (_foreColor == value)
					return;

				_foreColor = value;
				Update();
			}
		}

		public string BackColor
		{
			get => _backColor;

			set
			{
				if (_backColor == value)
					return;

				_backColor = value;
				Update();
			}
		}

		public string Image
		{
			get => _image;

			set
			{
				if (_image == value)
					return;

				_image = value;
				Update();
			}
		}

		[UniqueIndex]
		public string RouteName
		{
			get => _routeName;

			set
			{
				if (_routeName == value)
					return;

				_routeName = value;
				Update();
			}
		}

		public bool IsVisible
		{
			get => _isVisible;

			set
			{
				if (_isVisible == value)
					return;

				_isVisible = value;
				Update();
			}
		}
	}
}
