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
 *  File: BatchUpdateDictionary.cs
 *
 *  Purpose:  Dictionary with batch update facility
 *
 *  Date        Name                Reason
 *  05/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SharedPluginFeatures.Interfaces;

namespace PluginManager.DAL.TextFiles.Internal
{
    internal sealed class BatchUpdateDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IBatchUpdate
        where TValue : IIndexManager, IBatchUpdate
    {
        public bool IsUpdating { get; private set; }

        public void BeginUpdate()
        {
            if (IsUpdating)
                throw new InvalidOperationException();

            foreach (KeyValuePair<TKey, TValue> kv in this)
                kv.Value.BeginUpdate();

            IsUpdating = true;
        }

        public void EndUpdate()
        {
            if (!IsUpdating)
                throw new InvalidOperationException();

            foreach (KeyValuePair<TKey, TValue> kv in this)
                kv.Value.EndUpdate();

            IsUpdating = false;
        }
    }
}
