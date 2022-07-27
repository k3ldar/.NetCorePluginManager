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
 *  Product:  PluginManager.DAL.TextFiles.Tests
 *  
 *  File: MockTextTableOperations.cs
 *
 *  Purpose:  MockTextTableOperations for text based storage
 *
 *  Date        Name                Reason
 *  07/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

namespace PluginManager.DAL.TextFiles.Tests.Mocks
{
    internal class MockTextTableOperations<T> : ITextTableOperations<T>, ITextTable
        where T : TableRowDefinition
    {
        public int DataLength => throw new NotImplementedException();

        public int RecordCount => throw new NotImplementedException();

        public long PrimarySequence => throw new NotImplementedException();

        public long SecondarySequence => throw new NotImplementedException();

        public byte CompactPercent => throw new NotImplementedException();

        public string TableName => throw new NotImplementedException();

        public void Delete(List<T> records)
        {
            throw new NotImplementedException();
        }

        public void Delete(T record)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool IdExists(long id)
        {
            throw new NotImplementedException();
        }

        public bool IdIsInUse(string propertyName, long value)
        {
            throw new NotImplementedException();
        }

        public void Insert(List<T> records)
        {
            throw new NotImplementedException();
        }

        public void Insert(T record)
        {
            throw new NotImplementedException();
        }

        public void InsertOrUpdate(T record)
        {
            throw new NotImplementedException();
        }

        public long NextSequence()
        {
            throw new NotImplementedException();
        }

        public long NextSequence(long increment)
        {
            throw new NotImplementedException();
        }

        public long NextSecondarySequence(long increment)
        {
            throw new NotImplementedException();
        }

        public void ResetSequence(long primarySequence, long secondarySequence)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<T> Select()
        {
            throw new NotImplementedException();
        }

        public T Select(long id)
        {
            throw new NotImplementedException();
        }

        public void Truncate()
        {
            throw new NotImplementedException();
        }

        public void Update(List<T> records)
        {
            throw new NotImplementedException();
        }

        public void Update(T record)
        {
            throw new NotImplementedException();
        }

        public void Insert(List<T> records, TextTableInsertOptions insertOptions)
        {
            throw new NotImplementedException();
        }

        public void Insert(T records, TextTableInsertOptions insertOptions)
        {
            throw new NotImplementedException();
        }

        public bool IndexExists(string name, object value)
        {
            throw new NotImplementedException();
        }

        public void ForceWrite()
        {
            throw new NotImplementedException();
        }
    }
}
