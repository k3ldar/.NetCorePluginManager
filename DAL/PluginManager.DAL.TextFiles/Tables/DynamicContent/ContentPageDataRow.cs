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
 *  File: ContentPageDataRow.cs
 *
 *  Purpose:  Row definition for Table for dynamic content pages
 *
 *  Date        Name                Reason
 *  25/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
	[Table(Constants.DomainDynamicContent, Constants.TableNameContentPage, CompressionType.None, CachingStrategy.Memory, WriteStrategy.Forced)]
	internal class ContentPageDataRow : TableRowDefinition
	{
		private string _name;
		private long _activeFromTicks;
		private long _activeToTicks;
		private string _routeName;
		private string _backgroundColor;
		private string _backgroundImage;

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

		public long ActiveFromTicks
		{
			get => _activeFromTicks;

			set
			{
				if (_activeFromTicks == value)
					return;

				_activeFromTicks = value;
				Update();
			}
		}

		public long ActiveToticks
		{
			get => _activeToTicks;

			set
			{
				if (_activeToTicks == value)
					return;

				_activeToTicks = value;
				Update();
			}
		}

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

		public string BackgroundColor
		{
			get => _backgroundColor;

			set
			{
				if (_backgroundColor == value)
					return;

				_backgroundColor = value;
				Update();
			}
		}

		public string BackgroundImage
		{
			get => _backgroundImage;

			set
			{
				if (_backgroundImage == value)
					return;

				_backgroundImage = value;
				Update();
			}
		}
	}
}
