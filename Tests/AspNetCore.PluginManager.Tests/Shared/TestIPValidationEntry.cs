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
 *  File: TestIPValidationEntry.cs
 *
 *  Purpose:  Mock IP Validation class
 *
 *  Date        Name                Reason
 *  23/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests
{
    public class TestIPValidationEntry
    {
        #region Private Members

        private string _ipAddress;

        #endregion Private Members

        public TestIPValidationEntry(in string ipAddress)
        {
            if (String.IsNullOrEmpty(ipAddress))
                throw new ArgumentNullException(nameof(ipAddress));

            _ipAddress = ipAddress;
        }

        public string IpAddress
        {
            get
            {
                return _ipAddress;
            }
        }

        public bool IsBanned { get; set; }


        public double Hits { get; set; }

        public ulong Requests { get; set; }

        public TimeSpan Duration { get; set; }

        public string QueryString { get; set; }

        public Enums.ValidateRequestResult ValidateRequestResult { get; set; }
    }
}
