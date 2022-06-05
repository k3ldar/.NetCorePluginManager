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
    internal sealed class IndexManager : IBatchUpdate
    {
        private readonly List<long> _keys;
        private readonly object _lock = new object();
        private bool _sortRequired = false;

        public IndexManager(IndexType indexType)
        {
            _keys = new List<long>();
            IndexType = indexType;
        }

        public IndexType IndexType { get; }

        public bool IsUpdating { get; private set; } = false;

        public bool Contains(long id)
        {
            using (TimedLock timedLock = TimedLock.Lock(_lock))
            {
                return _keys.Contains(id);
            }
        }

        public void Add(List<long> items)
        {
            if (items == null)
                return;

            using (TimedLock timedLock = TimedLock.Lock(_lock))
            {
                foreach (long item in items)
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

        public void Add(long item)
        {
            if (!Contains(item))
            {
                using (TimedLock timedLock = TimedLock.Lock(_lock))
                {
                    switch (IndexType)
                    {
                        case IndexType.Ascending:
                            _sortRequired = _keys.Count > 0 && item < _keys[_keys.Count - 1];
                            _keys.Add(item);
                            break;

                        case IndexType.Descending:
                            _sortRequired = _keys.Count > 0 && item > _keys[_keys.Count - 1];
                            _keys.Insert(0, item);
                            break;
                    }
                }

                if (!IsUpdating)
                    Sort();
            }
        }

        public void Remove(long item)
        {
            if (Contains(item))
            {
                using (TimedLock timedLock = TimedLock.Lock(_lock))
                {
                    _keys.Remove(item);
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
                switch (IndexType)
                {
                    case IndexType.Descending:
                        _keys.Sort(new SortDescending());
                        break;

                    default:
                        _keys.Sort(new SortAscending());
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
