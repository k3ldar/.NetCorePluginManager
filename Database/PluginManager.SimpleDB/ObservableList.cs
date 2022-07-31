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
 *  File: TextFilesList.cs
 *
 *  Purpose:  List with changed event
 *
 *  Date        Name                Reason
 *  26/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace PluginManager.SimpleDB
{
    public sealed class ObservableList<T> : List<T>
    {
        new public void Add(T item)
        {
            base.Add(item);
            Updated();
        }

        new public void AddRange(IEnumerable<T> collection)
        {
            base.AddRange(collection);
            Updated();
        }

        new public void Clear()
        {
            base.Clear();
            Updated();
        }

        new public void Insert(int index, T item)
        {
            base.Insert(index, item);
            Updated();
        }

        new public void InsertRange(int index, IEnumerable<T> collection)
        {
            base.InsertRange(index, collection);
            Updated();
        }

        new public bool Remove(T item)
        {
            if (base.Remove(item))
            {
                Updated();
                return true;
            }

            return false;
        }

        new public int RemoveAll(Predicate<T> match)
        {
            int result = base.RemoveAll(match);

            if (result > 0)
                Updated();

            return result;
        }

        new public void RemoveAt(int index)
        {
            base.RemoveAt(index);
            Updated();
        }

        new public void RemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
            Updated();
        }

        private void Updated()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Changed;
    }
}
