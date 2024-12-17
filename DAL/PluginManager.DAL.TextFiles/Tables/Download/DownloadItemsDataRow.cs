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
 *  File: DownloadItemsDataRow.cs
 *
 *  Purpose:  Row definition for Table for download items
 *
 *  Date        Name                Reason
 *  25/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
	[Table(Constants.DomainDownloads, Constants.TableNameDownloadItems, WriteStrategy.Lazy)]
	internal class DownloadItemsDataRow : TableRowDefinition
	{
		private string _name;
		private string _description;
		private string _version;
		private string _filename;
		private string _icon;
		private string _size;
		private int _downloadCount;
		private long _categoryId;
		private long _userId;

		[ForeignKey(Constants.TableNameDownloadCategories)]
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

		[ForeignKey(Constants.TableNameUsers, ForeignKeyAttributes.DefaultValue)]
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

		public string Name
		{
			get => _name;

			set
			{
				if (value == _name)
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
				if (value == _description)
					return;

				_description = value;
				Update();
			}
		}

		public string Version
		{
			get => _version;

			set
			{
				if (value == _version)
					return;

				_version = value;
				Update();
			}
		}

		public string Filename
		{
			get => _filename;

			set
			{
				if (value == _filename)
					return;

				_filename = value;
				Update();
			}
		}

		public string Icon
		{
			get => _icon;

			set
			{
				if (value == _icon)
					return;

				_icon = value;
				Update();
			}
		}

		public string Size
		{
			get => _size;

			set
			{
				if (value == _size)
					return;

				_size = value;
				Update();
			}
		}

		public int DownloadCount
		{
			get => _downloadCount;

			set
			{
				if (value == _downloadCount)
					return;

				_downloadCount = value;
				Update();
			}
		}
	}
}
