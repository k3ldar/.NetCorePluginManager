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
 *  File: Enums.cs
 *
 *  Purpose:  Enums for text based storage
 *
 *  Date        Name                Reason
 *  23/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace PluginManager.DAL.TextFiles
{
    /// <summary>
    /// Type of compression to use when reading/writing data from disk
    /// </summary>
    public enum CompressionType : byte
    {
        /// <summary>
        /// Data is not compressed
        /// </summary>
        None = 0,

        /// <summary>
        /// Data is compressed using Brotli
        /// </summary>
        Brotli = 1
    }

    /// <summary>
    /// Cache strategy to use 
    /// </summary>
    public enum CachingStrategy : byte
    {
        /// <summary>
        /// Records are read from storage on demand
        /// </summary>
        None = 0,

        /// <summary>
        /// Records are held in memory to speed up retrieval
        /// </summary>
        Memory = 1,
    }

    /// <summary>
    /// Type of index
    /// </summary>
    public enum IndexType : byte
    {
        /// <summary>
        /// Index is ascending
        /// </summary>
        Ascending = 0,

        /// <summary>
        /// Index is descending
        /// </summary>
        Descending = 1,
    }

    /// <summary>
    /// Supported trigger types
    /// </summary>
    [Flags]
    public enum TriggerType : byte
    {
        BeforeInsert,

        AfterInsert,

        BeforeDelete,

        AfterDelete,

        BeforeUpdate,

        BeforeUpdateCompare,

        AfterUpdate,
    }
}
