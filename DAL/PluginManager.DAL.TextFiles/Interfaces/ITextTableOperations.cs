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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: ITextReaderWriter.cs
 *
 *  Purpose:  ITextReaderWriter<T> for text based storage
 *
 *  Date        Name                Reason
 *  23/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace PluginManager.DAL.TextFiles
{
    public interface ITextTableOperations<T> : IDisposable
        where T : TableRowDefinition
    {
        /// <summary>
        /// Selects all rows
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<T> Select();

        /// <summary>
        /// Selects a single item based on unique id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Select(long id);

        /// <summary>
        /// Batch inserts multiple new records
        /// </summary>
        /// <param name="records">List of records to batch insert</param>
        void Insert(List<T> records);

        /// <summary>
        /// Inserts a single new record
        /// </summary>
        /// <param name="record">Record to insert</param>
        void Insert(T record);

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
        /// Retrieves the current sequence
        /// </summary>
        /// <value>long</value>
        long Sequence { get; }

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
        /// Resets the sequence to a specific number
        /// </summary>
        /// <param name="sequence"></param>
        void ResetSequence(long sequence);
    }
}
