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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockSaveData.cs
 *
 *  Purpose:  Mock ISaveData class
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
    public sealed class MockSaveData : ISaveData
    {
        #region Constructors

        public MockSaveData(bool saveDataResponse = true)
        {
            SaveDataResponse = saveDataResponse;
            SaveDataCalled = false;
        }

        #endregion Constructors

        #region ISaveData Methods

        public Boolean Save<T>(T data, in String location, in String name)
        {
            SaveDataCalled = true;
            return SaveDataResponse;
        }

        #endregion ISaveData Methods

        #region Properties

        public bool SaveDataCalled { get; private set; }

        public bool SaveDataResponse { get; set; }

        #endregion Properties
    }
}
