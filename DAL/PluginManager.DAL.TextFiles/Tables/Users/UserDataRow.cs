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
 *  File: UserRowDataRow.cs
 *
 *  Purpose:  Table definition for user details
 *
 *  Date        Name                Reason
 *  31/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
	/// <summary>
	/// User table 
	/// </summary>
	[Table(Constants.TableNameUsers, CompressionType.Brotli, CachingStrategy.Memory)]
	internal sealed class UserDataRow : TableRowDefinition
	{
		#region Private Members

		string _email;
		string _firstName;
		string _surname;
		string _password;
		DateTime _passwordExpire;
		string _telephone;
		string _businessName;
		string _addressLine1;
		string _addressLine2;
		string _addressLine3;
		string _city;
		string _county;
		string _postcode;
		string _countryCode;
		string _unlockCode;
		bool _emailConfirmed;
		string _emailConfirmCode;
		bool _telephoneConfirmed;
		string _telephoneConfirmCode;
		bool _marketingEmail;
		bool _marketingPostal;
		bool _marketingSms;
		bool _marketingTelephone;

		#endregion Private Members

		public string Email
		{
			get
			{
				return _email;
			}

			set
			{
				if (_email == value)
					return;

				_email = value;
				Update();
			}
		}

		public string FirstName
		{
			get
			{
				return _firstName;
			}

			set
			{
				if (_firstName == value)
					return;

				_firstName = value;
				Update();
			}
		}

		public string Surname
		{
			get
			{
				return _surname;
			}

			set
			{
				if (_surname == value)
					return;

				_surname = value;
				Update();
			}
		}

		public string Password
		{
			get
			{
				return _password;
			}

			set
			{
				if (_password == value)
					return;

				_password = value;
				Update();
			}
		}

		public DateTime PasswordExpire
		{
			get
			{
				return _passwordExpire;
			}

			set
			{
				if (_passwordExpire == value)
					return;

				_passwordExpire = value;
				Update();
			}
		}

		public string Telephone
		{
			get
			{
				return _telephone;
			}

			set
			{
				if (_telephone == value)
					return;

				_telephone = value;
				Update();
			}
		}

		public string BusinessName
		{
			get
			{
				return _businessName;
			}

			set
			{
				if (_businessName == value)
					return;

				_businessName = value;
				Update();
			}
		}

		public string AddressLine1
		{
			get
			{
				return _addressLine1;
			}

			set
			{
				if (_addressLine1 == value)
					return;

				_addressLine1 = value;
				Update();
			}
		}

		public string AddressLine2
		{
			get
			{
				return _addressLine2;
			}

			set
			{
				if (_addressLine2 == value)
					return;

				_addressLine2 = value;
				Update();
			}
		}

		public string AddressLine3
		{
			get
			{
				return _addressLine3;
			}

			set
			{
				if (_addressLine3 == value)
					return;

				_addressLine3 = value;
				Update();
			}
		}

		public string City
		{
			get
			{
				return _city;
			}

			set
			{
				if (_city == value)
					return;

				_city = value;
				Update();
			}
		}

		public string County
		{
			get
			{
				return _county;
			}

			set
			{
				if (_county == value)
					return;

				_county = value;
				Update();
			}
		}

		public string Postcode
		{
			get
			{
				return _postcode;
			}

			set
			{
				if (_postcode == value)
					return;

				_postcode = value;
				Update();
			}
		}

		public string CountryCode
		{
			get
			{
				return _countryCode;
			}

			set
			{
				if (_countryCode == value)
					return;

				_countryCode = value;
				Update();
			}
		}

		public string UnlockCode
		{
			get
			{
				return _unlockCode;
			}

			set
			{
				if (_unlockCode == value)
					return;

				_unlockCode = value;
				Update();
			}
		}

		public bool EmailConfirmed
		{
			get
			{
				return _emailConfirmed;
			}

			set
			{
				if (_emailConfirmed == value)
					return;

				_emailConfirmed = value;
				Update();
			}
		}

		public string EmailConfirmCode
		{
			get
			{
				return _emailConfirmCode;
			}

			set
			{
				if (_emailConfirmCode == value)
					return;

				_emailConfirmCode = value;
				Update();
			}
		}

		public bool TelephoneConfirmed
		{
			get
			{
				return _telephoneConfirmed;
			}

			set
			{
				if (_telephoneConfirmed == value)
					return;

				_telephoneConfirmed = value;
				Update();
			}
		}

		public string TelephoneConfirmCode
		{
			get
			{
				return _telephoneConfirmCode;
			}

			set
			{
				if (_telephoneConfirmCode == value)
					return;

				_telephoneConfirmCode = value;
				Update();
			}
		}

		public bool MarketingEmail
		{
			get
			{
				return _marketingEmail;
			}

			set
			{
				if (_marketingEmail == value)
					return;

				_marketingEmail = value;
				Update();
			}
		}

		public bool MarketingPostal
		{
			get
			{
				return _marketingPostal;
			}

			set
			{
				if (_marketingPostal == value)
					return;

				_marketingPostal = value;
				Update();
			}
		}

		public bool MarketingSms
		{
			get
			{
				return _marketingSms;
			}

			set
			{
				if (_marketingSms == value)
					return;

				_marketingSms = value;
				Update();
			}
		}

		public bool MarketingTelephone
		{
			get
			{
				return _marketingTelephone;
			}

			set
			{
				if (_marketingTelephone == value)
					return;

				_marketingTelephone = value;
				Update();
			}
		}

		public string FullName => $"{FirstName} {Surname}";

		public bool Locked => !String.IsNullOrEmpty(UnlockCode);
	}
}
