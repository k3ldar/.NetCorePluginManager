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
 *  File: Enums.cs
 *
 *  Purpose:  Enums for SimpleDB
 *
 *  Date        Name                Reason
 *  23/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SimpleDB
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
    /// Write strategy to use
    /// </summary>
    public enum WriteStrategy : byte
    {
        /// <summary>
        /// Data is written immediately to disk
        /// </summary>
        Forced = 0,

        /// <summary>
        /// Data is written to disk at intervals or when TextTableOperation is disposed
        /// 
        /// In cases of a application/system crash, this could result in lost data
        /// </summary>
        Lazy = 1,
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

		/// <summary>
		/// Data is held in memory for a specified amount of time, when the timeout expires with no use, memory is released
		/// </summary>
		SlidingMemory = 2,
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
		/// <summary>
		/// No supported triggers
		/// </summary>
		None = 0,

		/// <summary>
		/// Before insert trigger supported
		/// </summary>
        BeforeInsert = 1,

		/// <summary>
		/// After insert trigger supported
		/// </summary>
        AfterInsert = 2,

		/// <summary>
		/// Before delete trigger supported
		/// </summary>
        BeforeDelete = 4,

		/// <summary>
		/// After delete trigger supported
		/// </summary>
        AfterDelete = 8,

		/// <summary>
		/// Before update trigger supported
		/// </summary>
        BeforeUpdate = 16,

		/// <summary>
		/// Before update comparing old record with new record trigger is supported
		/// </summary>
        BeforeUpdateCompare = 32,

		/// <summary>
		/// After update trigger supported
		/// </summary>
        AfterUpdate = 64,
    }

	/// <summary>
	/// Database page sizes
	/// </summary>
	public enum PageSize : int
	{
		/// <summary>
		/// Page size 4096 bytes long
		/// </summary>
		Size4096 = 4096,

		/// <summary>
		/// Page size 8192 bytes long
		/// </summary>
		Size8192 = 8192,

		/// <summary>
		/// Page size 16384 bytes long
		/// </summary>
		Size1634 = 16384,
	}

	/// <summary>
	/// Usage for foreign keys
	/// </summary>
	[Flags]
	public enum ForeignKeyUsage
	{
		/// <summary>
		/// No usage found
		/// </summary>
		None = 0,

		/// <summary>
		/// Foreign key has a reference
		/// </summary>
		Referenced = 1,

		/// <summary>
		/// Foreign key is allowed default value for type
		/// </summary>
		AllowDefault = 2,

		/// <summary>
		/// Cascade delete is supported by foreign key
		/// </summary>
		CascadeDelete = 4,
	}

	/// <summary>
	/// Foreign key attributes
	/// </summary>
	public enum ForeignKeyAttributes
	{
		/// <summary>
		/// No special attributes
		/// </summary>
		None,

		/// <summary>
		/// Value is set to default if foreign key deleted
		/// </summary>
		DefaultValue,

		/// <summary>
		/// Data is deleted if foreign key deleted
		/// </summary>
		CascadeDelete,
	}
}
