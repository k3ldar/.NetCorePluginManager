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
 *  File: SessionStatsBaseData.cs
 *
 *  Purpose:  Base table properties for session statistics
 *
 *  Date        Name                Reason
 *  07/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
	internal class SessionStatsBaseData : TableRowDefinition
	{
		public SessionStatsBaseData()
		{
			_userAgents = [];
			_userAgents.Changed += ObservableItem_Changed;
			_countryData = [];
			_countryData.Changed += ObservableItem_Changed;
		}

		private uint _totalVisits;
		private uint _humanVisits;
		private uint _mobileVisits;
		private uint _botVisits;
		private uint _bounced;
		private uint _totalPages;
		private decimal _totalSales;
		private uint _conversions;
		private uint _mobileConversions;
		private uint _referUnknown;
		private uint _referDirect;
		private uint _referOrganic;
		private uint _referBing;
		private uint _referGoogle;
		private uint _referYahoo;
		private uint _referFacebook;
		private uint _referTwitter;
		private uint _referOther;
		private ObservableDictionary<string, uint> _userAgents;
		private ObservableDictionary<string, uint> _countryData;

		public uint TotalVisits
		{
			get => _totalVisits;

			set
			{
				if (value == _totalVisits)
					return;

				_totalVisits = value;
				Update();
			}
		}

		public uint HumanVisits
		{
			get => _humanVisits;

			set
			{
				if (value == _humanVisits)
					return;

				_humanVisits = value;
				Update();
			}
		}

		public uint MobileVisits
		{
			get => _mobileVisits;

			set
			{
				if (value == _mobileVisits)
					return;

				_mobileVisits = value;
				Update();
			}
		}

		public uint BotVisits
		{
			get => _botVisits;

			set
			{
				if (value == _botVisits)
					return;

				_botVisits = value;
				Update();
			}
		}

		public uint Bounced
		{
			get => _bounced;

			set
			{
				if (value == _bounced)
					return;

				_bounced = value;
				Update();
			}
		}

		public uint TotalPages
		{
			get => _totalPages;

			set
			{
				if (value == _totalPages)
					return;

				_totalPages = value;
				Update();
			}
		}

		public decimal TotalSales
		{
			get => _totalSales;

			set
			{
				if (value == _totalSales)
					return;

				_totalSales = value;
				Update();
			}
		}

		public uint Conversions
		{
			get => _conversions;

			set
			{
				if (value == _conversions)
					return;

				_conversions = value;
				Update();
			}
		}

		public uint MobileConversions
		{
			get => _mobileConversions;

			set
			{
				if (value == _mobileConversions)
					return;

				_mobileConversions = value;
				Update();
			}
		}

		public uint ReferUnknown
		{
			get => _referUnknown;

			set
			{
				if (value == _referUnknown)
					return;

				_referUnknown = value;
				Update();
			}
		}

		public uint ReferDirect
		{
			get => _referDirect;

			set
			{
				if (value == _referDirect)
					return;

				_referDirect = value;
				Update();
			}
		}

		public uint ReferOrganic
		{
			get => _referOrganic;

			set
			{
				if (value == _referOrganic)
					return;

				_referOrganic = value;
				Update();
			}
		}

		public uint ReferBing
		{
			get => _referBing;

			set
			{
				if (value == _referBing)
					return;

				_referBing = value;
				Update();
			}
		}

		public uint ReferGoogle
		{
			get => _referGoogle;

			set
			{
				if (value == _referGoogle)
					return;

				_referGoogle = value;
				Update();
			}
		}

		public uint ReferYahoo
		{
			get => _referYahoo;

			set
			{
				if (value == _referYahoo)
					return;

				_referYahoo = value;
				Update();
			}
		}

		public uint ReferFacebook
		{
			get => _referFacebook;

			set
			{
				if (value == _referFacebook)
					return;

				_referFacebook = value;
				Update();
			}
		}

		public uint ReferTwitter
		{
			get => _referTwitter;

			set
			{
				if (value == _referTwitter)
					return;

				_referTwitter = value;
				Update();
			}
		}

		public uint ReferOther
		{
			get => _referOther;

			set
			{
				if (value == _referOther)
					return;

				_referOther = value;
				Update();
			}
		}

		public ObservableDictionary<string, uint> UserAgents
		{
			get => _userAgents;

			set
			{
				if (_userAgents == value)
					return;

				if (value == null)
					return;

				_userAgents.Changed -= ObservableItem_Changed;
				_userAgents = value;
				_userAgents.Changed += ObservableItem_Changed;
				Update();
			}
		}

		public ObservableDictionary<string, uint> CountryData
		{
			get => _countryData;

			set
			{
				if (_countryData == value)
					return;

				if (value == null)
					return;

				_countryData.Changed -= ObservableItem_Changed;
				_countryData = value;
				_countryData.Changed += ObservableItem_Changed;
				Update();
			}
		}

		private void ObservableItem_Changed(object sender, EventArgs e)
		{
			Update();
		}
	}
}
