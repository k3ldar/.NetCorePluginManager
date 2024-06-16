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
 *  Product:  AspNetCore.PluginManager.DemoWebsite
 *  
 *  File: SessionHelper.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using Shared.Classes;

namespace AspNetCore.PluginManager.DemoWebsite.Helpers
{
	/// <summary>
	/// The purpose of this static class is to provide a conduit to integrating with the user session
	/// and obtaining geo ip data, if required
	/// </summary>
	[ExcludeFromCodeCoverage(Justification = "Code coverage not required only intended as demonstration")]
	public static class SessionHelper
	{
		public static void InitSessionHelper()
		{
			UserSessionManager.Instance.OnSessionCreated += UserSession_OnSessionCreated;
			UserSessionManager.Instance.OnSavePage += UserSession_OnSavePage;
			UserSessionManager.Instance.OnSessionClosing += UserSession_OnSessionClosing;
			UserSessionManager.Instance.OnSessionRetrieve += UserSession_OnSessionRetrieve;
			UserSessionManager.Instance.OnSessionSave += UserSession_OnSessionSave;
			UserSessionManager.Instance.IPAddressDetails += UserSession_IPAddressDetails;
		}

		private static void UserSession_IPAddressDetails(object sender, Shared.IpAddressArgs e)
		{
			// not used in mock
		}

		private static void UserSession_OnSessionSave(object sender, Shared.UserSessionArgs e)
		{
			// not used in mock
		}

		private static void UserSession_OnSessionRetrieve(object sender, Shared.UserSessionRequiredArgs e)
		{
			// not used in mock
		}

		private static void UserSession_OnSessionClosing(object sender, Shared.UserSessionArgs e)
		{
			// not used in mock
		}

		private static void UserSession_OnSavePage(object sender, Shared.UserPageViewArgs e)
		{
			// not used in mock
		}

		private static void UserSession_OnSessionCreated(object sender, Shared.UserSessionArgs e)
		{
			// not used in mock
		}
	}
}
