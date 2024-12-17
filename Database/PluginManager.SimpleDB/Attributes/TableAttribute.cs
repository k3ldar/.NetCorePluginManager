/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SimpleDB
 *  
 *  File: TableAttribute.cs
 *
 *  Purpose:  TableAttribute for SimpleDB
 *
 *  Date        Name                Reason
 *  23/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SimpleDB
{
	/// <summary>
	/// Defines a tables property
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class TableAttribute : Attribute
	{
		private const int MinimumSlidingTimeoutMilliseconds = 1;
		private int _SlidingMemoryTimeoutMilliseconds;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="tableName">Name of table</param>
		/// <param name="compression">Type of compression to use for table</param>
		/// <param name="cachingStrategy">Caching strategy in use for the table</param>
		/// <param name="writeStrategy">Write strategy</param>
		public TableAttribute(string tableName,
			CompressionType compression = CompressionType.None,
			CachingStrategy cachingStrategy = CachingStrategy.None,
			WriteStrategy writeStrategy = WriteStrategy.Forced)
			: this(String.Empty, tableName, compression, cachingStrategy, writeStrategy)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tableName">Name of table</param>
		/// <param name="writeStrategy">Write strategy</param>
		public TableAttribute(string tableName,
			WriteStrategy writeStrategy)
			: this(String.Empty, tableName, CompressionType.None,
				  writeStrategy == WriteStrategy.Lazy ? CachingStrategy.Memory : CachingStrategy.None, writeStrategy)
		{

		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="domain"></param>
		/// <param name="tableName">Name of table</param>
		/// <param name="writeStrategy">Write strategy</param>
		public TableAttribute(string domain,
			string tableName,
			WriteStrategy writeStrategy)
			: this(domain, tableName, CompressionType.None,
				  writeStrategy == WriteStrategy.Lazy ? CachingStrategy.Memory : CachingStrategy.None, writeStrategy)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="domain">Domain name for table</param>
		/// <param name="tableName">Name of table</param>
		/// <param name="compression">Type of compression to use for table</param>
		/// <param name="cachingStrategy">Caching strategy in use for the table</param>
		/// <param name="writeStrategy">Write strategy, default forced write</param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public TableAttribute(string domain,
			string tableName,
			CompressionType compression = CompressionType.None,
			CachingStrategy cachingStrategy = CachingStrategy.None,
			WriteStrategy writeStrategy = WriteStrategy.Forced)
		{
			if (domain == null)
				throw new ArgumentNullException(nameof(domain));

			if (String.IsNullOrEmpty(tableName))
				throw new ArgumentNullException(nameof(tableName));

			foreach (char c in Path.GetInvalidFileNameChars())
			{
				if (domain.Contains(c))
					throw new ArgumentException($"Tablename contains invalid character: {c}", nameof(tableName));
			}

			foreach (char c in Path.GetInvalidFileNameChars())
			{
				if (tableName.Contains(c))
					throw new ArgumentException($"Tablename contains invalid character: {c}", nameof(tableName));
			}

			Domain = domain;
			TableName = tableName;
			Compression = compression;
			CachingStrategy = writeStrategy == WriteStrategy.Lazy ? cachingStrategy : writeStrategy == WriteStrategy.Lazy ? CachingStrategy.Memory : cachingStrategy;
			WriteStrategy = writeStrategy;
		}

		/// <summary>
		/// Name of domain where table is located
		/// </summary>
		/// <value>string</value>
		public string Domain { get; }

		/// <summary>
		/// Name of table
		/// </summary>
		/// <value>string</value>
		public string TableName { get; }

		/// <summary>
		/// Type of compression to use, if any
		/// </summary>
		/// <value>CompressionType</value>

		public CompressionType Compression { get; }

		/// <summary>
		/// Caching strategy for table data
		/// </summary>
		/// <value>CachingStrategy</value>
		public CachingStrategy CachingStrategy { get; }

		/// <summary>
		/// Write strategy to use when saving data
		/// </summary>
		/// <value>WriteStrategy</value>
		public WriteStrategy WriteStrategy { get; }

		/// <summary>
		/// Page size, not currently used
		/// </summary>
		/// <value>PageSize</value>
		public PageSize PageSize { get; set; } = PageSize.Size8192;

		/// <summary>
		/// Sliding time out in ms, determines when the data is no longer required for data held in memory
		/// </summary>
		/// <value>int</value>
		public int SlidingMemoryTimeoutMilliseconds
		{
			get
			{
				return _SlidingMemoryTimeoutMilliseconds;
			}

			set
			{
				if (CachingStrategy != CachingStrategy.SlidingMemory)
					throw new InvalidOperationException($"Table: {TableName} - {nameof(SlidingMemoryTimeoutMilliseconds)} can only be used with {CachingStrategy.SlidingMemory} caching strategy");

				if (value < MinimumSlidingTimeoutMilliseconds)
					throw new ArgumentOutOfRangeException(nameof(value));

				_SlidingMemoryTimeoutMilliseconds = value;
			}
		}

		/// <summary>
		/// Sliding memory timeout
		/// </summary>
		/// <value>TimeSpan</value>
		public TimeSpan SlidingMemoryTimeout
		{
			get
			{
				if (CachingStrategy != CachingStrategy.SlidingMemory)
					throw new InvalidOperationException($"Table: {TableName} - {nameof(SlidingMemoryTimeoutMilliseconds)} can only be used with {CachingStrategy.SlidingMemory} caching strategy");

				return TimeSpan.FromMilliseconds(SlidingMemoryTimeoutMilliseconds);
			}
		}
	}
}
