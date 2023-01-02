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
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TableAttribute : Attribute
    {
		private const int MinimumSlidingTimeoutMilliseconds = 1;
		private int _SlidingMemoryTimeoutMilliseconds;

		public TableAttribute(string tableName, 
            CompressionType compression = CompressionType.None,
            CachingStrategy cachingStrategy = CachingStrategy.None, 
            WriteStrategy writeStrategy = WriteStrategy.Forced)
            : this (String.Empty, tableName, compression, cachingStrategy, writeStrategy)
        {

        }

        public TableAttribute(string tableName,
            WriteStrategy writeStrategy)
            : this(String.Empty, tableName, CompressionType.None,
                  writeStrategy == WriteStrategy.Lazy ? CachingStrategy.Memory : CachingStrategy.None, writeStrategy)
        {

        }

        public TableAttribute(string domain,
            string tableName,
            WriteStrategy writeStrategy)
            : this(domain, tableName, CompressionType.None, 
                  writeStrategy == WriteStrategy.Lazy ? CachingStrategy.Memory : CachingStrategy.None, writeStrategy)
        {

        }

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
            CachingStrategy = writeStrategy == WriteStrategy.Lazy ? CachingStrategy.Memory : cachingStrategy;
            WriteStrategy = writeStrategy;
		}

        public string Domain { get; }

        public string TableName { get; }

        public CompressionType Compression { get; }

        public CachingStrategy CachingStrategy { get; }

        public WriteStrategy WriteStrategy { get; }

		public PageSize PageSize { get; set; } = PageSize.Size8192;

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
