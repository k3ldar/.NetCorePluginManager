﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  ErrorManager.Plugin
 *  
 *  File: ErrorManagerMiddleware.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  16/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace ErrorManager.Plugin
{
    /// <summary>
    /// Error manager middleware pipeline service.
    /// </summary>
    public sealed class ErrorManagerMiddleware : BaseMiddleware
    {
        #region Private Members

        private readonly RequestDelegate _next;
        private readonly IErrorManager _errorManager;
        private readonly string _loginPage;

        private static ErrorThreadManager _errorThreadManager;

        private static readonly object _lockObject = new object();
        private static readonly Dictionary<string, uint> _missingPageCount = new Dictionary<string, uint>();
        private static readonly CacheManager _errorCacheManager = new CacheManager("Error Manager", new TimeSpan(1, 0, 0), true, false);
        private static readonly Timings _timingsExceptions = new Timings();
        private static readonly Timings _timingsMissingPages = new Timings();

        #endregion Private Members

        #region Constructors

        public ErrorManagerMiddleware(RequestDelegate next, IErrorManager errorManager,
            ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _next = next ?? throw new ArgumentNullException(nameof(next));
            _errorManager = errorManager ?? throw new ArgumentNullException(nameof(errorManager));
            _errorThreadManager = new ErrorThreadManager(errorManager);

            if (!ThreadManager.Exists("Error Manager"))
                ThreadManager.ThreadStart(_errorThreadManager, "Error Manager", ThreadPriority.Lowest);

            _errorCacheManager.ItemRemoved += ErrorCacheManager_ItemRemoved;

            ErrorManagerSettings settings = settingsProvider.GetSettings<ErrorManagerSettings>("ErrorManager");

            _loginPage = settings.LoginPage;
        }

        #endregion Constructors

        #region Public Methods

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                if (!context.Response.HasStarted)
                {
                    switch (context.Response.StatusCode)
                    {
                        case 403:
                            ITempDataDictionary tempData = GetTempData(context);
                            tempData[Constants.ReturnUrl] = context.Request.Path;
                            context.Response.Redirect(_loginPage, false);
                            break;

                        case 404:
                            using (StopWatchTimer notFoundTimer = StopWatchTimer.Initialise(_timingsMissingPages))
                            {
                                context.Response.Redirect(GetMissingPage(context), false);
                                break;
                            }

                        case 406:
                            context.Response.Redirect("/Error/NotAcceptable");
                            break;

                        case 420:
                        case 429:
                            context.Response.Redirect("/Error/Highvolume", false);
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                using ( StopWatchTimer stopWatch = StopWatchTimer.Initialise(_timingsExceptions))
                {
                    if (ProcessException(context, exception))
                    {
                        context.Response.Redirect("/Error/Index/", false);
                    }
                    else
                    {
                        // this is only invoked if the error manager is the problem
                         throw;
                    }
                }
            }
        }

        #endregion Public Methods

        #region Internal Static Methods

        internal static Dictionary<string, uint> GetMissingPages()
        {
            using (TimedLock lck = TimedLock.Lock(_lockObject))
            {
                Dictionary<string, uint> Result = new Dictionary<string, uint>();

                foreach (KeyValuePair<string, uint> item in _missingPageCount)
                    Result.Add(item.Key, item.Value);

                return Result;
            }
        }

        internal static List<ErrorInformation> GetErrors()
        {
            List<CacheItem> cacheItems = _errorCacheManager.Items;

            List<ErrorInformation> Result = new List<ErrorInformation>();

            foreach (CacheItem item in cacheItems)
                Result.Add((ErrorInformation)item.Value);

            return Result;
        }

        internal static Timings GetMissingPageTimings()
        {
            return _timingsMissingPages;
        }

        internal static Timings GetErrorTimings()
        {
            return _timingsExceptions;
        }

        #endregion Internal Static Methods

        #region Private Methods

        private bool ProcessException(in HttpContext context, in Exception error)
        {
            string stackTrace = error.StackTrace == null ? String.Empty : error.StackTrace.ToString();

            if (!String.IsNullOrEmpty(stackTrace) && stackTrace.Contains('\r'))
            {
                stackTrace = stackTrace.Substring(0, stackTrace.IndexOf('\r')).Trim();
            }

            // has this error been logged before?
            string errorIdentifier = $"{error.Message} {stackTrace}";

            CacheItem cacheItem = _errorCacheManager.Get(errorIdentifier);

            if (cacheItem == null)
            {
                // not been logged before
                cacheItem = new CacheItem(errorIdentifier, new ErrorInformation(error, errorIdentifier));
                _errorCacheManager.Add(errorIdentifier, cacheItem);
            }

            ErrorInformation errorInformation = (ErrorInformation)cacheItem.Value;
            errorInformation.IncrementError();

            // only inform about the error if it's the first time!
            if (errorInformation.ErrorCount == 1)
                _errorThreadManager.AddError(errorInformation);

            // what if we are the problem?
            if (context.Request.Path.Value.StartsWith("/Error", StringComparison.CurrentCultureIgnoreCase)
#if DEBUG
                && !context.Request.Path.Value.Contains("/Error/Raise", StringComparison.CurrentCultureIgnoreCase)
#endif
                )
                return false;
            else
                return true;
        }

        private string GetMissingPage(in HttpContext context)
        {
            string path = RouteLowered(context);
            string replacePage = "/Error/NotFound404";

            using (TimedLock lck = TimedLock.Lock(_lockObject))
            {
                if (!_missingPageCount.ContainsKey(path))
                    _missingPageCount.Add(path, 0);

                _missingPageCount[path]++;

                // does the host have a replacement page for the missing page?
                if (_errorManager.MissingPage(path, ref replacePage))
                    if (!String.IsNullOrEmpty(replacePage))
                        return replacePage;
            }

            return "/Error/NotFound404";
        }

        private void ErrorCacheManager_ItemRemoved(object sender, Shared.CacheItemArgs e)
        {
            // when the error has expired, re-add it to the list of errors reported
            ErrorInformation errorInformation = (ErrorInformation)e.CachedItem.Value;
            errorInformation.Expired = true;
            _errorThreadManager.AddError(errorInformation);
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591