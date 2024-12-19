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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockApplicationOverrides.cs
 *
 *  Purpose:  Mock IApplicationOverride class
 *
 *  Date        Name                Reason
 *  25/07/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AppSettings;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class MockApplicationOverrides : IApplicationOverride
    {
        private readonly Dictionary<string, string> _overrides;

        public MockApplicationOverrides(Dictionary<string, string> overrides)
        {
            _overrides = overrides ?? throw new ArgumentNullException(nameof(overrides));
        }

        public MockApplicationOverrides()
            : this(new Dictionary<string, string>())
        {

        }

        public bool ExpandApplicationVariable(string variableName, ref object value)
        {
            if (_overrides.ContainsKey(variableName))
            {
                value = _overrides[variableName];
                return true;
            }

            return false;
        }
    }
}
