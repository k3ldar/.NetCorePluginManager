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
 *  File: FAQDataRow.cs
 *
 *  Purpose:  Table definition for faq's
 *
 *  Date        Name                Reason
 *  18/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
	[Table(Constants.DomainHelpdesk, Constants.TableNameFAQ, WriteStrategy.Lazy)]
	internal class FaqDataRow : TableRowDefinition
	{
		private string _name;
		private long _parent;
		private string _description;
		private int _order;
		private int _viewCount;

		public string Name
		{
			get
			{
				return _name;
			}

			set
			{
				if (_name == value)
					return;

				_name = value;
				Update();
			}
		}

		[ForeignKey(Constants.TableNameFAQ, ForeignKeyAttributes.DefaultValue)]
		public long Parent
		{
			get
			{
				return _parent;
			}

			set
			{
				if (_parent == value)
					return;

				_parent = value;
				Update();
			}
		}

		public string Description
		{
			get
			{
				return _description;
			}

			set
			{
				if (_description == value)
					return;

				_description = value;
				Update();
			}
		}

		public int Order
		{
			get
			{
				return _order;
			}

			set
			{
				if (_order == value)
					return;

				_order = value;
				Update();
			}
		}

		public int ViewCount
		{
			get
			{
				return _viewCount;
			}

			set
			{
				if (_viewCount == value)
					return;

				_viewCount = value;
				Update();
			}
		}
	}
}
