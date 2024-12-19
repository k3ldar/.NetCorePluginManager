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
 *  Copyright (c) 2024 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Cron.Plugin
 *  
 *  File: ICronJobProvider.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  09/08/2024  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

namespace Middleware
{
	/// <summary>
	/// Cron job data provider
	/// </summary>
	public interface ICronJobProvider
	{
		/// <summary>
		/// Date and time the job was last run
		/// </summary>
		/// <param name="cronJob">ICronJob instance</param>
		/// <returns></returns>
		DateTime GetLastRun(ICronJob cronJob);

		/// <summary>
		/// Sets the date and time the job was last run
		/// </summary>
		/// <param name="cronJob">ICronJob instance</param>
		/// <param name="lastRun">Date time last run</param>
		void SetLastRun(ICronJob cronJob, DateTime lastRun);
	}
}
