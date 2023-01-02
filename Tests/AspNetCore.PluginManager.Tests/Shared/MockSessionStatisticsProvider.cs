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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockSessionStatisticsProvider.cs
 *
 *  Purpose:  Mock IActionDescriptorCollectionProvider class
 *
 *  Date        Name                Reason
 *  23/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Middleware;
using Middleware.SessionData;

namespace AspNetCore.PluginManager.Tests.Shared
{
	[ExcludeFromCodeCoverage]
	internal class MockSessionStatisticsProvider : ISessionStatisticsProvider
	{
		public List<SessionDaily> GetDailyData(bool isBot)
		{
			throw new NotImplementedException();
		}

		public List<SessionHourly> GetHourlyData(bool isBot)
		{
			throw new NotImplementedException();
		}

		public List<SessionMonthly> GetMonthlyData(bool isBot)
		{
			throw new NotImplementedException();
		}

		public List<SessionUserAgent> GetUserAgents()
		{
			throw new NotImplementedException();
		}

		public List<SessionWeekly> GetWeeklyData(bool isBot)
		{
			throw new NotImplementedException();
		}

		public List<SessionYearly> GetYearlyData(bool isBot)
		{
			throw new NotImplementedException();
		}
	}
}
