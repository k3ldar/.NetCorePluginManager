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
        /// Initial sequence provided for the table
        /// </summary>
        long InitialSequence { get; }
    }
}
