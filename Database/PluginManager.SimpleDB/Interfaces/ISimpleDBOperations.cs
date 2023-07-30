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
 *  Product:  SimpleDB
 *  
 *  File: ISimpleDBOperations.cs
 *
 *  Purpose:  ISimpleDBOperations<T> interface used by consumers to perform
 *			  operations on a table
 *
 *  Date        Name                Reason
 *  23/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SimpleDB
{
    public interface ISimpleDBOperations<T> : IDisposable
        where T : TableRowDefinition
    {
        /// <summary>
        /// Selects all rows
        /// </summary>
        /// <returns>IReadOnlyList&lt;T&gt;</returns>
        IReadOnlyList<T> Select();

        /// <summary>
        /// Selects a single item based on unique id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>T</returns>
        T Select(long id);

		/// <summary>
		/// Selects a list of items based on the predicate selectFilter
		/// </summary>
		/// <param name="selectFilter"></param>
		/// <returns>IReadOnlyList&lt;T&gt;</returns>
		IReadOnlyList<T> Select(Func<T, bool> predicate);

        /// <summary>
        /// Batch inserts multiple new records, new primary key will be assigned
        /// </summary>
        /// <param name="records">List of records to batch insert</param>
        void Insert(List<T> records);

        /// <summary>
        /// Batch inserts multiple new records with insert options
        /// </summary>
        /// <param name="records"></param>
        /// <param name="insertOptions"></param>
        void Insert(List<T> records, InsertOptions insertOptions);

        /// <summary>
        /// Inserts a single new record, new primary key will be assigned to each record
        /// </summary>
        /// <param name="record">Record to insert</param>
        void Insert(T record);

        /// <summary>
        /// Inserts a single new record with insert options
        /// </summary>
        /// <param name="record"></param>
        /// <param name="insertOptions"></param>
        void Insert(T record, InsertOptions insertOptions);

        /// <summary>
        /// Removes a batch of records
        /// </summary>
        /// <param name="records"></param>
        void Delete(List<T> records);

        /// <summary>
        /// Removes a specific record
        /// </summary>
        /// <param name="record"></param>
        void Delete(T record);

        /// <summary>
        /// Removes all records after validating foreign keys don't exist etc
        /// </summary>
        void Truncate();

        /// <summary>
        /// Updates a batch of records
        /// </summary>
        /// <param name="records"></param>
        void Update(List<T> records);

        /// <summary>
        /// Updates a specific record
        /// </summary>
        /// <param name="record"></param>
        void Update(T record);

        /// <summary>
        /// Inserts a record if it does not exist already (based on id) or updates an existing record
        /// </summary>
        /// <param name="record"></param>
        void InsertOrUpdate(T record);

        /// <summary>
        /// Force writes data to disk, only has an effect if WriteStrategy is not Forced
        /// </summary>
        void ForceWrite();

        /// <summary>
        /// Length of the data stored on disk
        /// </summary>
        /// <value>int</value>
        public int DataLength { get; }

        /// <summary>
        /// Number of records stored on disk
        /// </summary>
        /// <value>int</value>
        public int RecordCount { get; }

        /// <summary>
        /// Retrieves the current primary sequence
        /// </summary>
        /// <value>long</value>
        long PrimarySequence { get; }

        /// <summary>
        /// Retrieves the current secondary sequence
        /// </summary>
        /// <value>long</value>
        long SecondarySequence { get; }

        byte CompactPercent { get; }

        /// <summary>
        /// Retrieves the next unique number in sequence
        /// </summary>
        /// <returns>long</returns>
        long NextSequence();

        /// <summary>
        /// Retrieves the next sequence, incremented by increment
        /// </summary>
        /// <param name="increment">, number to increment by</param>
        /// <returns>long</returns>
        long NextSequence(long increment);

        /// <summary>
        /// Retrieves the next secondary sequence, incremented by increment
        /// </summary>
        /// <param name="increment">, number to increment by</param>
        /// <returns>long</returns>
        long NextSecondarySequence(long increment);

        /// <summary>
        /// Resets the sequence to a specific number
        /// </summary>
        /// <param name="primarySequence"></param>
        /// <param name="secondarySequence"></param>
        void ResetSequence(long primarySequence, long secondarySequence);

        /// <summary>
        /// Indicates whether a record exists with a specific id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IdExists(long id);

        /// <summary>
        /// Indicates whether an index exists with a specific value
        /// </summary>
        /// <param name="name">Name of index</param>
        /// <param name="value">Index value</param>
        /// <returns></returns>
        bool IndexExists(string name, object value);

		/// <summary>
		/// Retrieves a table lock instance to be used for table level locking
		/// </summary>
		object TableLock { get; }
    }
}
