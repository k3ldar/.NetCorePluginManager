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
 *  File: IndexManager.cs
 *
 *  Purpose:  IndexManager for text based storage
 *
 *  Date        Name                Reason
 *  05/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System.Linq;

using Shared.Classes;

using SharedPluginFeatures.Interfaces;

namespace PluginManager.DAL.TextFiles.Internal
{
    /// <summary>
    /// This saves all index in memory and is rebuilt every time the file is loaded, this could
    /// prove very inneficient with lots of data, if that is the case look at converting the 
    /// internals of this class to disk i/o
    /// 
    /// Another potential saving if more than 20 records would be to change to a binary search
    /// </summary>
    internal sealed class IndexManager<T> : IIndexManager, IBatchUpdate
    {
        private readonly List<T> _keys;
        private readonly object _lock = new object();
        private bool _sortRequired = false;

        public IndexManager(IndexType indexType)
        {
            _keys = new List<T>();
            IndexType = indexType;
        }

        public IndexType IndexType { get; }

        public bool IsUpdating { get; private set; } = false;

        public bool Contains(object id)
        {
            using (TimedLock timedLock = TimedLock.Lock(_lock))
            {
                return _keys.Contains((T)id);
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

        public void Add(object item)
        {
            using (TimedLock timedLock = TimedLock.Lock(_lock))
            {
                switch (IndexType)
                {
                    case IndexType.Ascending:
                        if (typeof(T).Equals(typeof(long)))
                            _sortRequired = _keys.Count > 0 && Convert.ToInt64(item) < Convert.ToInt64(_keys[^1]);
                        else if (typeof(T).Equals(typeof(int)))
                            _sortRequired = _keys.Count > 0 && Convert.ToInt32(item) < Convert.ToInt32(_keys[^1]);
                        else
                            _sortRequired = true;

                        _keys.Add((T)item);

                        break;

                    case IndexType.Descending:
                        if (typeof(T).Equals(typeof(long)))
                            _sortRequired = _keys.Count > 0 && Convert.ToInt64(item) > Convert.ToInt64(_keys[^1]);
                        else if (typeof(T).Equals(typeof(int)))
                            _sortRequired = _keys.Count > 0 && Convert.ToInt32(item) > Convert.ToInt32(_keys[^1]);
                        else
                            _sortRequired = true;

                        _keys.Insert(0, (T)item);

                        break;
                }
            }

            if (!IsUpdating)
                Sort();
        }

        public void Remove(object item)
        {
            if (Contains(item))
            {
                using (TimedLock timedLock = TimedLock.Lock(_lock))
                {
                    _keys.Remove((T)item);
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
