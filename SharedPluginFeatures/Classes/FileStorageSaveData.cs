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
 *  Copyright (c) 2012 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatues
 *  
 *  File: FileStorageSaveData.cs
 *
 *  Purpose:  Default implementation of File storage for ISaveData
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
    /// Default implementation of File storage for ISaveData
    /// </summary>
    public class FileStorageSaveData : ISaveData
    {
        #region Private Members

        private readonly object _lockObject = new object();
        private readonly ILogger _logger;
        private readonly string _rootPath;

        #endregion Private Members

        #region Constructors

        public FileStorageSaveData(ILogger logger, string rootPath)
        {
            if (string.IsNullOrEmpty(rootPath))
                throw new ArgumentNullException(nameof(rootPath));

            if (!Directory.Exists(rootPath))
                throw new ArgumentException("Root path does not exists", nameof(rootPath));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _rootPath = Path.Combine(rootPath, "Settings");
        }

        #endregion Constructors

        #region ISaveData Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Catch all ok as returns false allowing callers to determine if success without throwing exception")]
        public bool Save<T>(T data, in string location, in string name)
        {
            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                string basePath = Path.Combine(_rootPath, location);

                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                string dataFile = Path.Combine(basePath, name);

                if (!Path.HasExtension(dataFile))
                    dataFile += ".dat";

                string tempCopy = Path.ChangeExtension(dataFile, ".tmp");
                try
                {
                    if (File.Exists(dataFile))
                    {
#if NET_CORE_2_1 || NET_CORE_2_2 || NET_STANDARD
                        File.Move(dataFile, tempCopy);
#else
                        File.Move(dataFile, tempCopy, true);
#endif
                    }

                    File.WriteAllText(dataFile, JsonConvert.SerializeObject(data));

                    if (File.Exists(tempCopy))
                    {
                        File.Delete(tempCopy);
                    }

                    return true;
                }
                catch (Exception err)
                {
                    if (File.Exists(tempCopy))
                    {
#if NET_CORE_2_1 || NET_CORE_2_2 || NET_STANDARD
                        File.Move(tempCopy, dataFile);
#else
                        File.Move(tempCopy, dataFile, true);
#endif
                    }

                    _logger.AddToLog(PluginManager.LogLevel.Error, err);

                    return false;
                }
            }
        }

        #endregion ISaveData Methods
    }
}

#pragma warning restore CS1591, CA1303