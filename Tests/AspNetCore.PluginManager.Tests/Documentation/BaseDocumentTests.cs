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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: BaseDocumentTests.cs
 *
 *  Purpose:  Base Document Tests
 *
 *  Date        Name                Reason
 *  12/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.IO;

using DocumentationPlugin.Classes;

using MemoryCache.Plugin;

using PluginManager.Abstractions;
using PluginManager.Internal;

using Shared.Docs;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Documentation
{
    public class BaseDocumentTests
    {
        #region private Members

        private ISettingsProvider _settingsProvider;
        private IMemoryCache _memoryCache;
        private DefaultDocumentationService _documentService;

        #endregion Private Members

        #region Protected Properties

        protected ISettingsProvider SettingsProvider
        {
            get
            {
                if (_settingsProvider == null)
                    throw new InvalidOperationException();

                return _settingsProvider;
            }
        }

        protected IMemoryCache MemoryCache
        {
            get
            {
                if (_memoryCache == null)
                    throw new InvalidOperationException();

                return _memoryCache;
            }
        }

        #endregion Protected Properties

        #region Protected Methods

        protected List<Document> GetDocuments()
        {
            if (_settingsProvider == null)
            {
                _settingsProvider = new DefaultSettingProvider(Directory.GetCurrentDirectory());
            }

            if (_memoryCache == null)
            {
                _memoryCache = new DefaultMemoryCache(_settingsProvider);
            }

            if (_documentService == null)
            {
                _documentService = new DefaultDocumentationService(_settingsProvider, _memoryCache);
            }

            return _documentService.GetDocuments();
        }

        #endregion Protected Methods
    }
}
