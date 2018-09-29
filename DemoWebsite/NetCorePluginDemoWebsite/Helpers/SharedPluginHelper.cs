using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AspNetCore.PluginManager;
using Shared.Classes;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.DemoWebsite.Helpers
{
    /// <summary>
    /// This class acts as a wrapper arund the elements you extend through plugin manager
    /// </summary>
    public sealed class SharedPluginHelper : ISharedPluginHelper
    {
        #region Constants

        private const string MainMenuCache = "Main Menu Cache";

        #endregion Constants

        #region Private Methods

        private readonly IMemoryCache _memoryCache;

        #endregion Private Methods

        #region Constructors

        public SharedPluginHelper(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        #endregion Constructors

        public List<MainMenuItem> BuildMainMenu()
        {
            CacheItem cache = _memoryCache.GetCache().Get(MainMenuCache);

            if (cache == null)
            {
                cache = new CacheItem(MainMenuCache, PluginManagerService.GetPluginClasses<MainMenuItem>());
                _memoryCache.GetCache().Add(MainMenuCache, cache);
            }

            return ((List<MainMenuItem>)cache.Value);
        }
    }
}
