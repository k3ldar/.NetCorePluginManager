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
 *  File: ProductGroupDataRow.cs
 *
 *  Purpose:  Table definition for product groups
 *
 *  Date        Name                Reason
 *  25/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
	[Table(Constants.TableNameProductGroups, CompressionType.None, CachingStrategy.Memory)]
	internal sealed class ProductGroupDataRow : TableRowDefinition
	{
		private string _description;
		private bool _showOnWebsite;
		private int _sortOrder;
		private string _tagLine;
		private string _url;

		[UniqueIndex]
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

		public bool ShowOnWebsite
		{
			get => _showOnWebsite;

			set
			{
				if (_showOnWebsite == value)
					return;

				_showOnWebsite = value;
				Update();
			}
		}

		public int SortOrder
		{
			get => _sortOrder;

			set
			{
				if (_sortOrder == value)
					return;

				_sortOrder = value;
				Update();
			}
		}

		public string TagLine
		{
			get => _tagLine;

			set
			{
				if (_tagLine == value)
					return;

				_tagLine = value;
				Update();
			}
		}

		public string Url
		{
			get => _url;

			set
			{
				if (_url == value)
					return;

				_url = value;
				Update();
			}
		}
	}
}
