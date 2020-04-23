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
 *  File: TestRequestCookieCollection.cs
 *
 *  Purpose:  Mock IRequestCookieCollection class
 *
 *  Date        Name                Reason
 *  20/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

namespace AspNetCore.PluginManager.Tests
{
    public class TestRequestCookieCollection : IRequestCookieCollection
    {
        #region Private Members

        private Dictionary<string, string> _cookies = new Dictionary<string, string>();

        #endregion Private Members

        #region Constructors

        public TestRequestCookieCollection()
        {

        }

        #endregion Constructors

        #region IRequestCookieCollection Methods

        public String this[String key]
        {
            get
            {
                return _cookies[key];
            }
        }

        public Int32 Count => throw new NotImplementedException();

        public ICollection<String> Keys => throw new NotImplementedException();

        public Boolean ContainsKey(String key)
        {
            return _cookies.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<String, String>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public Boolean TryGetValue(String key, out String value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion IRequestCookieCollection Methods

        #region PUblic Methods

        public void AddCookie(in string name, in string value)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            _cookies.Add(name, value ?? String.Empty);
        }

        #endregion Public Methods
    }
}
