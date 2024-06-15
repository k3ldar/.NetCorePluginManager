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
 *  File: ObservableList.cs
 *
 *  Purpose:  List with changed event
 *
 *  Date        Name                Reason
 *  26/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SimpleDB
{
	/// <summary>
	/// Observabl list which notifies table of updated/new records
	/// </summary>
	/// <typeparam name="T">Item type</typeparam>
	public sealed class ObservableList<T> : List<T>
    {
		/// <summary>
		/// Adds a new record to the list
		/// </summary>
		/// <param name="item"></param>
        new public void Add(T item)
        {
            base.Add(item);
            Updated();
        }

		/// <summary>
		/// Adds a range of new items to the list
		/// </summary>
		/// <param name="collection"></param>
        new public void AddRange(IEnumerable<T> collection)
        {
            base.AddRange(collection);
            Updated();
        }

		/// <summary>
		/// Clears all list items
		/// </summary>
        new public void Clear()
        {
            base.Clear();
            Updated();
        }

		/// <summary>
		/// Inserts a new list item at the specified position
		/// </summary>
		/// <param name="index">Position of item</param>
		/// <param name="item">Item to be inserted</param>
        new public void Insert(int index, T item)
        {
            base.Insert(index, item);
            Updated();
        }

		/// <summary>
		/// Inserts a range of items at the specified index
		/// </summary>
		/// <param name="index">Position of item</param>
		/// <param name="collection">Collection of items to be inserted</param>
        new public void InsertRange(int index, IEnumerable<T> collection)
        {
            base.InsertRange(index, collection);
            Updated();
        }

		/// <summary>
		/// Removes the specified item from the list
		/// </summary>
		/// <param name="item">Item to be removed</param>
		/// <returns></returns>
		new public bool Remove(T item)
        {
            if (base.Remove(item))
            {
                Updated();
                return true;
            }

            return false;
        }

		/// <summary>
		/// Removes all items matching a predicate
		/// </summary>
		/// <param name="match">Predicate</param>
		/// <returns></returns>
        new public int RemoveAll(Predicate<T> match)
        {
            int result = base.RemoveAll(match);

            if (result > 0)
                Updated();

            return result;
        }

		/// <summary>
		/// Removes a specific item at the given index
		/// </summary>
		/// <param name="index">Index of item to be removed</param>
		new public void RemoveAt(int index)
        {
            base.RemoveAt(index);
            Updated();
        }

		/// <summary>
		/// Removes a range of items from the list
		/// </summary>
		/// <param name="index">Index of item to be removed</param>
		/// <param name="count">Number of items to remove</param>
        new public void RemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
            Updated();
        }

        private void Updated()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }

		/// <summary>
		/// Event raised when an item is inserted/removed from the list
		/// </summary>
        public event EventHandler Changed;
    }
}
