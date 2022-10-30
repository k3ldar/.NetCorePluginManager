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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockResponseCookies.cs
 *
 *  Purpose:  Mock IResponseCookies class
 *
 *  Date        Name                Reason
 *  20/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Http;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class MockResponseCookies : IResponseCookies
    {
        #region Private Members

        private Dictionary<string, MockResponseCookie> _cookies = new Dictionary<string, MockResponseCookie>();


        #endregion Private Members

        #region Constructors


        #endregion Constructors

        #region IResponseCookies Methods

        public void Append(String key, String value)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (_cookies.ContainsKey(key))
                _cookies[key] = new MockResponseCookie(key, value);
            else
                _cookies.Add(key, new MockResponseCookie(key, value));
        }

        public void Append(String key, String value, CookieOptions options)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (_cookies.ContainsKey(key))
                _cookies[key] = new MockResponseCookie(key, value, options);
            else
                _cookies.Add(key, new MockResponseCookie(key, value, options));
        }

        public void Delete(String key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (_cookies.ContainsKey(key))
                _cookies.Remove(key);
        }

        public void Delete(String key, CookieOptions options)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (_cookies.ContainsKey(key))
                _cookies.Remove(key);
        }

        #endregion IResponseCookies Methods

        #region Public Methods

        public MockResponseCookie Get(in string key)
        {
            return _cookies[key];
        }

        #endregion Public Methods
    }
}
