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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: TestIPValidation.cs
 *
 *  Purpose:  Mock IIpValidation class
 *
 *  Date        Name                Reason
 *  23/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests
{
    [ExcludeFromCodeCoverage]
    public class TestIPValidation : IIpValidation
    {
        #region Private Members

        private readonly Dictionary<string, TestIPValidationEntry> _testIPValidationEntry;

        #endregion Private Members

        #region Constructors

        public TestIPValidation()
        {
            _testIPValidationEntry = new Dictionary<string, TestIPValidationEntry>();
        }

        #endregion Constuctors

        #region IIpValidation Methods

        public void ConnectionAdd(in string ipAddress)
        {
            _testIPValidationEntry.Add(ipAddress, new TestIPValidationEntry(ipAddress));
        }

        public bool ConnectionBan(in string ipAddress, in double hits, in ulong requests, in TimeSpan duration)
        {
            TestIPValidationEntry entry = _testIPValidationEntry[ipAddress];
            entry.IsBanned = true;
            entry.Hits = hits;
            entry.Requests = requests;
            entry.Duration = duration;

            return false;
        }

        public void ConnectionRemove(in string ipAddress, in double hits, in ulong requests, in TimeSpan duration)
        {
            _testIPValidationEntry.Remove(ipAddress);
        }

        public void ConnectionReport(in string ipAddress, in string queryString, in Enums.ValidateRequestResult validation)
        {
            TestIPValidationEntry entry = _testIPValidationEntry[ipAddress];
            entry.QueryString = queryString;
            entry.ValidateRequestResult = validation;
        }

        #endregion IIpValidation Methods
    }
}
