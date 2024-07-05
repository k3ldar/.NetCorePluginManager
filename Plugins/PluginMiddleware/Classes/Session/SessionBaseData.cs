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
 *  Product:  Middleware.Plugin
 *  
 *  File: SessionBaseData.cs
 *
 *  Purpose:  Base session data
 *
 *  Date        Name                Reason
 *  03/09/2020  Simon Carter        Initially Created
 *  02/08/2022	Simon Carter		Moved to middleware
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System.Collections.Generic;

namespace Middleware.SessionData
{
	/// <summary>
	/// Base session data for Hourly, Daily, Weekly Monthly and Yearly
	/// </summary>
	public class SessionBaseData
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public SessionBaseData()
		{
			CountryData = [];
			UserAgents = [];
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Total number of visits
		/// </summary>
		/// <value>uint</value>
		public uint TotalVisits { get; set; }

		/// <summary>
		/// Total number of human visits
		/// </summary>
		/// <value>uint</value>
		public uint HumanVisits { get; set; }

		/// <summary>
		/// Total number of visits from a mobile device
		/// </summary>
		/// <value>uint</value>
		public uint MobileVisits { get; set; }

		/// <summary>
		/// Total number of visits from a Bot
		/// </summary>
		/// <value>uint</value>
		public uint BotVisits { get; set; }

		/// <summary>
		/// Total number of human visitors who have bounced
		/// </summary>
		/// <value>uint</value>
		public uint Bounced { get; set; }

		/// <summary>
		/// Total number of pages viewed
		/// </summary>
		public uint TotalPages { get; set; }

		/// <summary>
		/// Total cost of all sales
		/// </summary>
		/// <value>decimal</value>
		public decimal TotalSales { get; set; }

		/// <summary>
		/// Number of conversions from visits to sales
		/// </summary>
		/// <value>uint</value>
		public uint Conversions { get; set; }

		/// <summary>
		/// Number of daily conversions from visits to sales on mobile devices
		/// </summary>
		/// <value>uint</value>
		public uint MobileConversions { get; set; }

		/// <summary>
		/// Unknown referrer count
		/// </summary>
		/// <value>uint</value>
		public uint ReferrerUnknown { get; set; }

		/// <summary>
		/// Direct referrer count
		/// </summary>
		/// <value>uint</value>
		public uint ReferDirect { get; set; }

		/// <summary>
		/// Organic referrer count
		/// </summary>
		/// <value>uint</value>
		public uint ReferOrganic { get; set; }

		/// <summary>
		/// Bing referrer count
		/// </summary>
		/// <value>uint</value>
		public uint ReferBing { get; set; }

		/// <summary>
		/// Google referrer count
		/// </summary>
		/// <value>uint</value>
		public uint ReferGoogle { get; set; }

		/// <summary>
		/// Yahoo referrer count
		/// </summary>
		/// <value>uint</value>
		public uint ReferYahoo { get; set; }

		/// <summary>
		/// Facebook referrer count
		/// </summary>
		/// <value>uint</value>
		public uint ReferFacebook { get; set; }

		/// <summary>
		/// Twitter referrer count
		/// </summary>
		/// <value>uint</value>
		public uint ReferTwitter { get; set; }

		/// <summary>
		/// Other referrer count
		/// </summary>
		/// <value>uint</value>
		public uint ReferOther { get; set; }

		/// <summary>
		/// Counts by country for the specified period
		/// </summary>
		/// <value>Dictionary&lt;string, uint&gt;</value>
		public Dictionary<string, uint> CountryData { get; set; }

		/// <summary>
		/// Counts by user agent for the specified period
		/// </summary>
		/// <value>List&lt;string, uint&gt;</value>
		public List<SessionUserAgent> UserAgents { get; set; }

		#endregion Properties
	}
}
