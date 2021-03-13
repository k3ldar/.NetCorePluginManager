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
 *  File: MockLoadData.cs
 *
 *  Purpose:  Mock ILoadData class
 *
 *  Date        Name                Reason
 *  08/11/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests
{
    [ExcludeFromCodeCoverage]
    public sealed class MockLoadData : ILoadData
    {
        #region Constructors

        public MockLoadData()
        {
            LoadDataCalled = false;
        }

        #endregion Constructors

        #region ILoadData Methods

        public T Load<T>(in String location, in String name)
        {
            LoadDataCalled = true;
            return (T)Activator.CreateInstance(typeof(T));
        }

        #endregion ILoadData Methods

        #region Properties

        public bool LoadDataCalled { get; private set; }

        #endregion Properties
    }
}
