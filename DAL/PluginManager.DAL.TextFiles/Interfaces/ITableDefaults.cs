namespace PluginManager.DAL.TextFiles
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
        /// Latest version of data row
        /// </summary>
        ushort Version { get; }

        /// <summary>
        /// Initial data that will be added when the table is first created and for each upgrade
        /// </summary>
        List<T> InitialData(ushort version);
    }
}
