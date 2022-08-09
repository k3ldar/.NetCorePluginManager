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
		private uint _totalVisits;
		private uint _humanVisits;
		private uint _mobileVisits;
		private uint _botVisits;
		private uint _bounced;
		private uint _totalPages;
		private decimal _totalSales;
		private uint _conversions;
		private uint _movileConversions;
		private uint _referrerUnknown;
		private uint _referDirect;
		private uint _referOrganic;
		private uint _referBing;
		private uint _referGoogle;
		private uint _referYahoo;
		private uint _referFacebook;
		private uint _referTwitter;
		private uint _referOther;
		private ObservableList<SessionUserAgent> _userAgents;
		private Dictionary<string, uint> _countryData;
	}
}
