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
 *  File: ObservableDictionary.cs
 *
 *  Purpose:  Dictionary with changed event
 *
 *  Date        Name                Reason
 *  10/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SimpleDB
{
	/// <summary>
	/// Observable dictionary which notifies table of updated/new records
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	/// <typeparam name="TValue">Value type</typeparam>
	[Serializable]
	public sealed class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
	{
		/// <summary>
		/// Retrieves the item given the key
		/// </summary>
		/// <param name="key"></param>
		/// <returns>TValue</returns>
		new public TValue this[TKey key]
		{
			get
			{
				return base[key];
			}

			set
			{
				base[key] = value;
				Updated();
			}
		}

		/// <summary>
		/// Adds a new record to the dictionary
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		new public void Add(TKey key, TValue value)
		{
			base.Add(key, value);
			Updated();
		}

		/// <summary>
		/// Clears the dictionary of all records
		/// </summary>
		new public void Clear()
		{
			base.Clear();
			Updated();
		}

		/// <summary>
		/// Removes an item from the dictionary
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		new public bool Remove(TKey key)
		{
			bool Result = base.Remove(key);

			if (Result)
				Updated();

			return Result;
		}

		/// <summary>
		/// Attempts to add a new item to the dictionary
		/// </summary>
		/// <param name="key">items key</param>
		/// <param name="value">item to try add</param>
		/// <returns>bool</returns>
		new public bool TryAdd(TKey key, TValue value)
		{
			bool Result = base.TryAdd(key, value);

			if (Result)
				Updated();

			return Result;
		}

		private void Updated()
		{
			Changed?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Event raised when an item is changed/deleted from the dictionary
		/// </summary>
		public event EventHandler Changed;
	}
}
