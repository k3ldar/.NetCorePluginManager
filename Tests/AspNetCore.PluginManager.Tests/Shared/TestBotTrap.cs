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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: TestBotTrap.cs
 *
 *  Purpose:  Mock ITestBotTrap class
 *
 *  Date        Name                Reason
 *  15/10/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.IO;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Shared
{
    public sealed class TestBotTrap : IBotTrap
    {
        private string _ipAddress;
        private string _userAgent;
        private readonly bool _throwException;

        public TestBotTrap()
        {
            _throwException = false;
        }

        public TestBotTrap(bool throwException)
        {
            _throwException = true;
        }

        public void OnTrapEntered(in String ipAddress, in string userAgent)
        {
            if (_throwException)
            {
                throw new IOException();
            }

            _ipAddress = ipAddress;
            _userAgent = userAgent;
        }

        public bool IpAddressTrapped(string ipAddress)
        {
            if (String.IsNullOrEmpty(_ipAddress))
                return false;

            return _ipAddress.Equals(ipAddress);
        }

        public bool UserAgentTrapped(string userAgent)
        {
            if (String.IsNullOrEmpty(userAgent))
                return false;

            return _userAgent.Equals(userAgent);
        }
    }
}
