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
 *  File: MockHeaderDictionary.cs
 *
 *  Purpose:  Mock IHeaderDictionary class
 *
 *  Date        Name                Reason
 *  20/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace AspNetCore.PluginManager.Tests
{
    [ExcludeFromCodeCoverage]
    public class MockHeaderDictionary : IHeaderDictionary
    {
        #region Private Members

        private readonly Dictionary<string, StringValues> _headerDictionary;

        #endregion Private Members

        #region Constructors

        public MockHeaderDictionary()
        {
            _headerDictionary = new Dictionary<string, StringValues>();
        }

        #endregion Constructors

        #region IHeaderDictionary Methods

        public StringValues this[String key]
        {
            get
            {
                return _headerDictionary[key];
            }

            set
            {
                if (ContainsKey(key))
                    Remove(key);

                Add(key, value);
            }
        }

        public Int64? ContentLength { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ICollection<String> Keys => throw new NotImplementedException();

        public ICollection<StringValues> Values => throw new NotImplementedException();

        public Int32 Count => throw new NotImplementedException();

        public Boolean IsReadOnly => throw new NotImplementedException();

        public void Add(String key, StringValues value)
        {
            _headerDictionary.Add(key, value);
        }

        public void Add(KeyValuePair<String, StringValues> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public Boolean Contains(KeyValuePair<String, StringValues> item)
        {
            throw new NotImplementedException();
        }

        public Boolean ContainsKey(String key)
        {
            return _headerDictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<String, StringValues>[] array, Int32 arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<String, StringValues>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public Boolean Remove(String key)
        {
            return _headerDictionary.Remove(key);
        }

        public Boolean Remove(KeyValuePair<String, StringValues> item)
        {
            throw new NotImplementedException();
        }

        public Boolean TryGetValue(String key, [MaybeNullWhen(false)] out StringValues value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion IHeaderDictionary Methods
    }
}
