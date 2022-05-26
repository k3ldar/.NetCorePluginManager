using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginManager.DAL.TextFiles.Interfaces
{
    public interface ITextReaderWriter<T> : IDisposable
        where T : BaseTable
    {
        List<T> Read();

        /// <summary>
        /// Saves all records, this overwrites existing data
        /// </summary>
        /// <param name="records">List of records to save</param>
        void Save(List<T> records);

        /// <summary>
        /// Saves a specific record to disk, this is added to the list of records already saved
        /// </summary>
        /// <param name="record"></param>
        void Create(T record);

        /// <summary>
        /// Removes a specific record
        /// </summary>
        /// <param name="record"></param>
        void Delete(T record);

        /// <summary>
        /// Length of the data stored on disk
        /// </summary>
        public int DataLength { get; }

        /// <summary>
        /// Number of records stored on disk
        /// </summary>
        public int RecordCount { get; }

        /// <summary>
        /// Retrieves the next unique number in sequence
        /// </summary>
        /// <returns>long</returns>
        public long NextSequence();

        /// <summary>
        /// Resets the sequence to a specific number
        /// </summary>
        /// <param name="sequence"></param>
        public void ResetSequence(long sequence);
    }
}
