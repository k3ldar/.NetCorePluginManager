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
 *  Copyright (c) 2012 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: ISessionStatisticsProvider.cs
 *
 *  Purpose:  Session statistics provider
 *
 *  Date        Name                Reason
 *  02/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

using Middleware.SessionData;

namespace Middleware
{
	/// <summary>
	/// Session statistics provider
	/// </summary>
	public interface ISessionStatisticsProvider
	{

		/// <summary>
		/// Retrieves Daily data
		/// </summary>
		/// <returns>List&lt;SessionDaily&gt;</returns>
		List<SessionDaily> GetDailyData(bool isBot);

		/// <summary>
		/// Retrieves Hourly data
		/// </summary>
		/// <returns>List&lt;SessionHourly&gt;</returns>
		List<SessionHourly> GetHourlyData(bool isBot);

		/// <summary>
		/// Retrieves weekly data
		/// </summary>
		/// <returns>List&lt;SessionWeekly&gt;</returns>
		List<SessionWeekly> GetWeeklyData(bool isBot);

		/// <summary>
		/// Retrieves Monthly data
		/// </summary>
		/// <returns>List&lt;SessionMonthly&gt;</returns>
		List<SessionMonthly> GetMonthlyData(bool isBot);

		/// <summary>
		/// Retrieves yearly data
		/// </summary>
		/// <returns>List&lt;SessionYearly&gt;</returns>
		List<SessionYearly> GetYearlyData(bool isBot);

		/// <summary>
		/// Retrieves user agent data
		/// </summary>
		/// <returns>List&lt;SessionUserAgent&gt;</returns>
		List<SessionUserAgent> GetUserAgents();
	}
}
