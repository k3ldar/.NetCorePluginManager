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
 *  File: IPValidation.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  11/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
    [ExcludeFromCodeCoverage(Justification = "Code coverage not required, only intended as demonstration")]
    public class IPValidation : IIpValidation
    {
        #region IIpValidation Methods

        public void ConnectionAdd(in string ipAddress)
        {
			// required for interface, not used in demo mode
        }

        public bool ConnectionBan(in string ipAddress, in double hits, in ulong requests, in TimeSpan duration)
        {
            // don't ban for real on demo website
            return false;
        }

        public void ConnectionRemove(in string ipAddress, in double hits, in ulong requests, in TimeSpan duration)
        {
			// required for interface, not used in demo mode
		}

		public void ConnectionReport(in string ipAddress, in string queryString, in Enums.ValidateRequestResult validation)
        {
			// required for interface, not used in demo mode
		}

		#endregion IIpValidation Methods
	}
}
