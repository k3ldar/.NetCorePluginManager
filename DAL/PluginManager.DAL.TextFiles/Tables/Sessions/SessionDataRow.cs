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
 *  File: SessionDataRow.cs
 *
 *  Purpose:  Table definition for session data
 *
 *  Date        Name                Reason
 *  02/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
	[Table(Constants.DomainSessions, Constants.TableNameSession, CompressionType.Brotli, CachingStrategy.SlidingMemory, WriteStrategy.Lazy, SlidingMemoryTimeoutMilliseconds = 300)]
	internal class SessionDataRow : TableRowDefinition
	{
		private string _sessionId;
		private string _ipAddress;
		private string _hostName;
		private string _userAgent;
		private bool _isMobile;
		private bool _isBrowserMobile;
		private bool _mobileRedirect;
		private int _referralType;
		private string _initialReferrer;
		private bool _bounced;
		private bool _isBot;
		private string _countryCode;
		private long _userId;
		private string _mobileModel;
		private string _mobileManufacturer;
		private string _saleCurrency;
		private decimal _saleAmount;

		[UniqueIndex]
		public string SessionId
		{
			get => _sessionId;

			set
			{
				if (_sessionId == value)
					return;

				_sessionId = value;
				Update();
			}

		}

		public string IpAddress
		{
			get => _ipAddress;

			set
			{
				if (_ipAddress == value)
					return;

				_ipAddress = value;
				Update();
			}

		}

		public string HostName
		{
			get => _hostName;

			set
			{
				if (_hostName == value)
					return;

				_hostName = value;
				Update();
			}

		}

		public string UserAgent
		{
			get => _userAgent;

			set
			{
				if (_userAgent == value)
					return;

				_userAgent = value;
				Update();
			}

		}

		public bool IsMobile
		{
			get => _isMobile;

			set
			{
				if (_isMobile == value)
					return;

				_isMobile = value;
				Update();
			}

		}

		public bool IsBrowserMobile
		{
			get => _isBrowserMobile;

			set
			{
				if (_isBrowserMobile == value)
					return;

				_isBrowserMobile = value;
				Update();
			}

		}

		public bool MobileRedirect
		{
			get => _mobileRedirect;

			set
			{
				if (_mobileRedirect == value)
					return;

				_mobileRedirect = value;
				Update();
			}

		}

		public int ReferralType
		{
			get => _referralType;

			set
			{
				if (_referralType == value)
					return;

				_referralType = value;
				Update();
			}

		}

		public string InitialReferrer
		{
			get => _initialReferrer;

			set
			{
				if (_initialReferrer == value)
					return;

				_initialReferrer = value;
				Update();
			}

		}

		public bool Bounced
		{
			get => _bounced;

			set
			{
				if (_bounced == value)
					return;

				_bounced = value;
				Update();
			}

		}

		public bool IsBot
		{
			get => _isBot;

			set
			{
				if (_isBot == value)
					return;

				_isBot = value;
				Update();
			}

		}

		public string CountryCode
		{
			get => _countryCode;

			set
			{
				if (_countryCode == value)
					return;

				_countryCode = value;
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

		public string MobileManufacturer
		{
			get => _mobileManufacturer;

			set
			{
				if (_mobileManufacturer == value)
					return;

				_mobileManufacturer = value;
				Update();
			}

		}

		public string MobileModel
		{
			get => _mobileModel;

			set
			{
				if (_mobileModel == value)
					return;

				_mobileModel = value;
				Update();
			}

		}

		public string SaleCurrency
		{
			get => _saleCurrency;

			set
			{
				if (_saleCurrency == value)
					return;

				_saleCurrency = value;
				Update();
			}

		}

		public decimal SaleAmount
		{
			get => _saleAmount;

			set
			{
				if (_saleAmount == value)
					return;

				_saleAmount = value;
				Update();
			}

		}
	}
}
