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
 *  Product:  SeoPlugin
 *  
 *  File: SeoMiddleware.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  12/05/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using Shared.Classes;

using SharedPluginFeatures;

using static SharedPluginFeatures.Constants;

#pragma warning disable CS1591

namespace SeoPlugin
{
    /// <summary>
    /// Seo middleware class used to process Seo requests within the request pipeline.
    /// </summary>
    public sealed class SeoMiddleware : BaseMiddleware
    {
        #region Private Members

        private readonly IMemoryCache _memoryCache;
        private readonly ISeoProvider _seoProvider;
        private readonly RequestDelegate _next;
        internal static Timings _timings = new Timings();

        #endregion Private Members

        #region Constructors

        public SeoMiddleware(RequestDelegate next, IMemoryCache memoryCache, ISeoProvider seoProvider)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _seoProvider = seoProvider ?? throw new ArgumentNullException(nameof(seoProvider));

            _next = next;
        }

        #endregion Constructors

        #region Public Methods

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timings))
            {
                string fileExtension = RouteFileExtension(context);

                if (!String.IsNullOrEmpty(fileExtension) &&
                    StaticFileExtensions.Contains($"{fileExtension};"))
                {
                    await _next(context);
                    return;
                }

                string route = RouteLowered(context);

                if (route.Length > 1 && route[route.Length - 1] == Constants.ForwardSlashChar)
                    route = route.Substring(0, route.Length - 1);

                string cacheName = $"Seo Cache {route}";
                CacheItem cacheItem = _memoryCache.GetCache().Get(cacheName);

                if (cacheItem == null)
                {
                    _seoProvider.GetSeoDataForRoute(route, out string title, out string description,
                        out string author, out List<string> tags);
                    cacheItem = new CacheItem(cacheName, new SeoCacheItem(title, description, author, tags));
                    _memoryCache.GetCache().Add(cacheName, cacheItem);
                }

                SeoCacheItem seoCache = (SeoCacheItem)cacheItem.Value;
                context.Items[SeoMetaAuthor] = seoCache.Author;
                context.Items[SeoTitle] = seoCache.Title;
                context.Items[SeoMetaDescription] = seoCache.Description;
                context.Items[SeoMetaKeywords] = seoCache.Keywords;
            }

            await _next(context);
        }

        #endregion Public Methods
    }
}

#pragma warning restore CS1591