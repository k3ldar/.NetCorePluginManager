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
	[Serializable]
	public sealed class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
	{
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

		new public void Add(TKey key, TValue value)
		{
			base.Add(key, value);
			Updated();
		}

		new public void Clear()
		{
			base.Clear();
			Updated();
		}

		new public bool Remove(TKey key)
		{
			bool Result = base.Remove(key);

			if (Result)
				Updated();

			return Result;
		}

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

		public event EventHandler Changed;
	}
}
