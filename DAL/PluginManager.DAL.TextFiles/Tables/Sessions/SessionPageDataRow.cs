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
 *  File: SessionPageDataRow.cs
 *
 *  Purpose:  Table definition for page view data
 *
 *  Date        Name                Reason
 *  07/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
	[Table(Constants.TableNameSessionPages)]
	internal class SessionPageDataRow : TableRowDefinition
	{
		private long _sessionId;
		private string _url;
		private long _totalTime;
		private string _referrer;
		private bool _isPostBack;

		[ForeignKey(Constants.TableNameSession)]
		public long SessionId
		{
			get => _sessionId;

			set
			{
				if (value == _sessionId)
					return;

				_sessionId = value;
				Update();
			}
		}

		public string Url
		{
			get => _url;

			set
			{
				if (value == _url)
					return;

				_url = value;
				Update();
			}
		}

		public long TotalTime
		{
			get => _totalTime;

			set
			{
				if (value == _totalTime)
					return;

				_totalTime = value;
				Update();
			}
		}

		public string Referrer
		{
			get => _referrer;

			set
			{
				if (value == _referrer)
					return;

				_referrer = value;
				Update();
			}
		}

		public bool IsPostBack
		{
			get => _isPostBack;

			set
			{
				if (value == _isPostBack)
					return;

				_isPostBack = value;
				Update();
			}
		}
	}
}
