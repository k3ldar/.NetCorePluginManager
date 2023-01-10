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
 *  Product:  BadEgg.Plugin
 *  
 *  File: IpManagement.cs
 *
 *  Purpose:  Manages black/white listed ip addresses
 *
 *  Date        Name                Reason
 *  11/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable CA1812

namespace BadEgg.Plugin
{
    internal class IpManagement : IIpManagement
    {
        #region IIpManagement Methods

        public void AddBlackListedIp(in string ipAddress)
        {
            if (String.IsNullOrEmpty(ipAddress))
                throw new ArgumentNullException(nameof(ipAddress));

            using (TimedLock lck = TimedLock.Lock(WebDefender.ValidateConnections.InternalIpAddressLock))
            {
                WebDefender.ValidateConnections.InternalIpAddressList[ipAddress] = true;
            }
        }

        public void AddWhiteListedIp(in string ipAddress)
        {
            if (String.IsNullOrEmpty(ipAddress))
                throw new ArgumentNullException(nameof(ipAddress));

            using (TimedLock lck = TimedLock.Lock(WebDefender.ValidateConnections.InternalIpAddressLock))
            {
                WebDefender.ValidateConnections.InternalIpAddressList[ipAddress] = false;
            }
        }

        public void ClearAllIpAddresses()
        {
            using (TimedLock lck = TimedLock.Lock(WebDefender.ValidateConnections.InternalIpAddressLock))
            {
                WebDefender.ValidateConnections.InternalIpAddressList.Clear();
            }
        }

        public void RemoveIpAddress(in string ipAddress)
        {
            if (String.IsNullOrEmpty(ipAddress))
                throw new ArgumentNullException(nameof(ipAddress));

            using (TimedLock lck = TimedLock.Lock(WebDefender.ValidateConnections.InternalIpAddressLock))
            {
				WebDefender.ValidateConnections.InternalIpAddressList.Remove(ipAddress);
			}
        }

        #endregion IIpManagement Methods
    }
}

#pragma warning restore CA1812