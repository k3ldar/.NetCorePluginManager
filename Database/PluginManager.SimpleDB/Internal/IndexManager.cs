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
 *  File: IndexManager.cs
 *
 *  Purpose:  IndexManager for SimpleDB
 *
 *  Date        Name                Reason
 *  05/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using Shared.Classes;

using SharedPluginFeatures.Interfaces;

namespace SimpleDB.Internal
{
	/// <summary>
	/// This saves all index in memory and is rebuilt every time the file is loaded, this could
	/// prove very inneficient with lots of data, if that is the case look at converting the 
	/// internals of this class to disk i/o
	/// 
	/// Another potential saving if more than 20 records would be to change to a binary search
	/// </summary>
	internal sealed class IndexManager<T> : IIndexManager
	{
		private readonly List<T> _keys;
		private readonly object _lock = new();
		private bool _sortRequired = false;
		private readonly List<string> _propertyNames;

		public IndexManager(IndexType indexType, params string[] propertyNames)
		{
			if (propertyNames.Length == 0)
				throw new ArgumentOutOfRangeException(nameof(propertyNames));

			_keys = new List<T>();
			_propertyNames = propertyNames.ToList();
			IndexType = indexType;
		}

		public IndexType IndexType { get; }

		public List<string> PropertyNames => _propertyNames;

		public bool IsUpdating { get; private set; } = false;

		public bool Contains(object value)
		{
			using (TimedLock timedLock = TimedLock.Lock(_lock))
			{
				return _keys.BinarySearch((T)value) > -1;
			}
		}

		public void Add(List<object> items)
		{
			if (items == null)
				return;

			using (TimedLock timedLock = TimedLock.Lock(_lock))
			{
				foreach (T item in items)
				{
					if (!Contains(item))
					{
						_keys.Add(item);
					}
				}
			}

			if (!IsUpdating)
				Sort();
		}

		public void Add(object value)
		{
			using (TimedLock timedLock = TimedLock.Lock(_lock))
			{

				if (Contains((T)value))
					return;

				switch (IndexType)
				{
					case IndexType.Ascending:
						if (typeof(T).Equals(typeof(long)))
							_sortRequired = _keys.Count > 0 && Convert.ToInt64(value) < Convert.ToInt64(_keys[^1]);
						else if (typeof(T).Equals(typeof(int)))
							_sortRequired = _keys.Count > 0 && Convert.ToInt32(value) < Convert.ToInt32(_keys[^1]);
						else
							_sortRequired = true;

						_keys.Add((T)value);

						break;

					case IndexType.Descending:
						if (typeof(T).Equals(typeof(long)))
							_sortRequired = _keys.Count > 0 && Convert.ToInt64(value) > Convert.ToInt64(_keys[^1]);
						else if (typeof(T).Equals(typeof(int)))
							_sortRequired = _keys.Count > 0 && Convert.ToInt32(value) > Convert.ToInt32(_keys[^1]);
						else
							_sortRequired = true;

						_keys.Insert(0, (T)value);

						break;
				}
			}

			if (!IsUpdating)
				Sort();
		}

		public void Remove(object value)
		{
			if (Contains(value))
			{
				using (TimedLock timedLock = TimedLock.Lock(_lock))
				{
					_keys.Remove((T)value);
				}

				if (!IsUpdating)
					Sort();
			}
		}

		private void Sort()
		{
			if (!_sortRequired)
				return;

			using (TimedLock timedLock = TimedLock.Lock(_lock))
			{
				_keys.Sort();

				switch (IndexType)
				{
					case IndexType.Descending:
						_keys.Reverse();
						break;
				}
			}
		}

		public void BeginUpdate()
		{
			if (IsUpdating)
				throw new InvalidOperationException();

			IsUpdating = true;
		}

		public void EndUpdate()
		{
			if (!IsUpdating)
				throw new InvalidOperationException();

			IsUpdating = false;
			Sort();
		}
	}
}
