using System;
using System.Collections.Generic;
using System.Globalization;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;

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
