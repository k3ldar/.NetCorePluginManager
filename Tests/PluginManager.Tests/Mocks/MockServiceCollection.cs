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
 *  Product:  PluginManager.Tests
 *  
 *  File: MockServiceCollection.cs
 *
 *  Purpose:  Mock IServiceCollection for unit tests
 *
 *  Date        Name                Reason
 *  17/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;

namespace PluginManager.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class MockServiceCollection : IServiceCollection
    {
        private readonly List<ServiceDescriptor> _serviceDescriptors;

        public MockServiceCollection()
        {
            _serviceDescriptors = new List<ServiceDescriptor>();
        }

        public ServiceDescriptor this[int index] { get => _serviceDescriptors[index]; set => throw new NotImplementedException(); }

        public int Count => _serviceDescriptors.Count;

        public bool IsReadOnly => false;

        public void Add(ServiceDescriptor item)
        {
            _serviceDescriptors.Add(item);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(ServiceDescriptor item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            _serviceDescriptors.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            return _serviceDescriptors.GetEnumerator();
        }

        public int IndexOf(ServiceDescriptor item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, ServiceDescriptor item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(ServiceDescriptor item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
