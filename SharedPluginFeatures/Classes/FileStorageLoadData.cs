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
 *  Copyright (c) 2012 - 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatues
 *  
 *  File: FileStorageLoadData.cs
 *
 *  Purpose:  Default implementation of File storage for ILoadData
 *
 *  Date        Name                Reason
 *  18/10/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.IO;

using Newtonsoft.Json;

using PluginManager.Abstractions;

using Shared.Classes;

#pragma warning disable CS1591, CA1303

namespace SharedPluginFeatures
{
    /// <summary>
    /// Default implementation of File storage for ILoadData
    /// </summary>
    public class FileStorageLoadData : ILoadData
    {
        private readonly object _lockObject = new object();
        private readonly ILogger _logger;
        private readonly string _rootPath;

        #region Constructors

        public FileStorageLoadData(ILogger logger, string rootPath)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (string.IsNullOrEmpty(rootPath))
                throw new ArgumentNullException(nameof(rootPath));

            if (!Directory.Exists(rootPath))
                throw new ArgumentException("Root path does not exists", nameof(rootPath));

            _rootPath = Path.Combine(rootPath, "Settings");
        }

        #endregion Constructors

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Catch all ok as returns default value allowing callers to determine if success without throwing exception")]
        public T Load<T>(in string location, in string name)
        {
            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                string basePath = Path.Combine(_rootPath, location);

                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                string dataFile = Path.Combine(basePath, name);

                if (!Path.HasExtension(dataFile))
                    dataFile += ".dat";

                if (File.Exists(dataFile))
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<T>(File.ReadAllText(dataFile));
                    }
                    catch (Exception err)
                    {
                        _logger.AddToLog(PluginManager.LogLevel.Error, err);
                    }

                }

                return default;
            }
        }
    }
}

#pragma warning restore CS1591, CA1303