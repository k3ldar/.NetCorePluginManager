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
 *  File: TableRowDefinition.cs
 *
 *  Purpose:  Table Row Definition for a table table
 *
 *  Date        Name                Reason
 *  25/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Text.Json.Serialization;

namespace SimpleDB
{
	/// <summary>
	/// Base class for all table row types
	/// </summary>
	public abstract class TableRowDefinition
	{
		private long _id;
		private long _created;
		private long _updated;

		/// <summary>
		/// Constructor
		/// </summary>
		protected TableRowDefinition()
		{
			_id = Int64.MinValue;
			_created = DateTime.UtcNow.Ticks;
			_updated = DateTime.UtcNow.Ticks;
		}

		/// <summary>
		/// Unique id of the record
		/// </summary>
		/// <value>long</value>
		[UniqueIndex(nameof(Id), IndexType.Ascending)]
		public long Id
		{
			get => _id;

			set
			{
				if (Immutable)
					throw new InvalidOperationException();

				_id = value;
			}
		}

		/// <summary>
		/// Date/time data was created
		/// </summary>
		/// <value>DateTime</value>
		[JsonIgnore]
		public DateTime Created => new(_created, DateTimeKind.Utc);

		/// <summary>
		/// Date/time data was created in ticks
		/// </summary>
		/// <value>long</value>
		public long CreatedTicks
		{
			get
			{
				return _created;
			}

			set
			{
				if (Immutable)
					throw new InvalidOperationException();

				_created = value;
			}
		}

		/// <summary>
		/// Date time data was last updated
		/// </summary>
		/// <value>DateTime</value>
		[JsonIgnore]
		public DateTime Updated => new(_updated, DateTimeKind.Utc);

		/// <summary>
		/// Ticks for when data was last updated
		/// </summary>
		public long UpdatedTicks
		{
			get
			{
				return _updated;
			}

			set
			{
				if (Immutable)
					throw new InvalidOperationException();

				_updated = value;
			}
		}

		/// <summary>
		/// Indicates the row is readonly and any updates will be ignored
		/// </summary>
		public bool ReadOnly { get; internal set; }

		/// <summary>
		/// Indicates whether the row has been marked for delete or not
		/// </summary>
		protected internal bool Immutable { get; internal set; } = false;

		/// <summary>
		/// Indicates the record has been loaded from storage
		/// </summary>
		[JsonIgnore]
		internal bool Loaded { get; set; }

		/// <summary>
		/// Indicates the record has been updated 
		/// </summary>
		[JsonIgnore]
		public bool HasChanged { get; internal set; }

		/// <summary>
		/// Update called to indicate the data has potentially been updated/changed
		/// </summary>
		/// <exception cref="InvalidOperationException"></exception>
		protected void Update()
		{
			if (!Loaded || HasChanged)
				return;

			if (ReadOnly)
				throw new InvalidOperationException("Record is readonly");

			_updated = DateTime.UtcNow.Ticks;
			HasChanged = true;
		}

		/// <summary>
		/// Indicates observable data has changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ObservableDataChanged(object sender, EventArgs e)
		{
			Update();
		}

	}
}
