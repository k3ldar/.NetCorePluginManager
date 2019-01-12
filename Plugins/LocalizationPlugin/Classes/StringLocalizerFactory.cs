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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Localization.Plugin
 *  
 *  File: StringLocalizerFactory.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  12/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Microsoft.Extensions.Localization;

using Shared.Classes;

namespace Localization.Plugin
{
    public class StringLocalizerFactory : IStringLocalizerFactory
    {
        #region Private Members

        private readonly CacheManager _cacheManager;

        #endregion Private Members

        #region Constructors

        public StringLocalizerFactory()
        {
            if (_cacheManager == null)
                _cacheManager = Initialisation.CultureCache;
        }

        #endregion Constructors

        #region IStringLocalizerFactory Methods

        public IStringLocalizer Create(Type resourceSource)
        {
            string cacheName = $"IStringLocalizer {System.Threading.Thread.CurrentThread.CurrentUICulture}";
            CacheItem cacheItem = _cacheManager.Get(cacheName);

            if (cacheItem == null)
            {
                cacheItem = new CacheItem(cacheName, new StringLocalizer(System.Threading.Thread.CurrentThread.CurrentUICulture));
                _cacheManager.Add(cacheName, cacheItem);
            }

            return (IStringLocalizer)cacheItem.Value;
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            string cacheName = $"IStringLocalizer {System.Threading.Thread.CurrentThread.CurrentUICulture}";
            CacheItem cacheItem = _cacheManager.Get(cacheName);

            if (cacheItem == null)
            {
                cacheItem = new CacheItem(cacheName, new StringLocalizer(System.Threading.Thread.CurrentThread.CurrentUICulture));
                _cacheManager.Add(cacheName, cacheItem);
            }

            return (IStringLocalizer)cacheItem.Value;
        }

        #endregion IStringLocalizerFactory Methods
    }
}
