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
 *  File: InitialReferralsDataRow.cs
 *
 *  Purpose:  Table definition for initial referrals data
 *
 *  Date        Name                Reason
 *  02/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
	[Table(Constants.DomainSessions, Constants.TableNameInitialReferrals)]
	internal class InitialReferralsDataRow : TableRowDefinition
	{
		private string _hash;
		private string _url;
		private uint _usage;

		public string Hash
		{
			get => _hash;

			set
			{
				if (value == _hash)
					return;

				_hash = value;
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

		public uint Usage
		{
			get => _usage;

			set
			{
				if (value == _usage)
					return;

				_usage = value;
				Update();
			}
		}
	}
}
