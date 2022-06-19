using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginManager.DAL.TextFiles.Interfaces
{
    public interface ITableDefaults<T>
        where T : TableRowDefinition
    {
        /// <summary>
        /// Initial primary sequence provided for the table
        /// </summary>
        long PrimarySequence { get; }

        /// <summary>
        /// Secondary sequence
        /// </summary>
        long SecondarySequence { get; }

        /// <summary>
        /// Initial data that will be added when the table is first created
        /// </summary>
        List<T> InitialData { get; }
    }
}
