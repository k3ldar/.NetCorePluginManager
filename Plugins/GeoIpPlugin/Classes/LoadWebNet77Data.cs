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
 *  Product:  GeoIpPlugin
 *  
 *  File: LoadWebNet77Data.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;

using PluginManager.Abstractions;

using Shared;
using Shared.Classes;

using SharedPluginFeatures;

namespace GeoIp.Plugin
{
    /// <summary>
    /// Background thread used to load WebNet77 Geo Ip Data
    /// </summary>
    internal class LoadWebNet77Data : ThreadManager
    {
        #region Properties

        private readonly ILogger _logger;
        private readonly string _webNet77DataFile;
        private readonly string _webNetDownloadDataFile;
        private readonly bool _autoDownloadWebnet77Data;
        private readonly Uri _webnet77CsvUrl;
        private readonly int _downloadFrequency;

        #endregion Propertes

        #region Constructors

        public LoadWebNet77Data(ILogger logger, string webNet77DataFile, List<IpCity> ipRangeData,
            bool autoDownloadWebnet77Data, string webnet77CsvUrl, int downloadFrequency)
            : base(ipRangeData, new TimeSpan(24, 0, 0))
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (ipRangeData == null)
                throw new ArgumentNullException(nameof(ipRangeData));

            base.ContinueIfGlobalException = false;

            _webNet77DataFile = Path.Combine(webNet77DataFile, Constants.Webnet77CsvDataFileName);
            _webNetDownloadDataFile = Path.ChangeExtension(_webNet77DataFile, Constants.FileExtensionDat);
            _autoDownloadWebnet77Data = autoDownloadWebnet77Data;
            _downloadFrequency = Utilities.CheckMinMax(downloadFrequency, 1, 20);

            Uri.TryCreate(webnet77CsvUrl, UriKind.Absolute, out _webnet77CsvUrl);
        }

        #endregion Constructors

        #region Overridden Methods

        protected override bool Run(object parameters)
        {
            List<IpCity> rangeData = (List<IpCity>)parameters;
            rangeData.Clear();

            if (_autoDownloadWebnet77Data)
            {
                DownloadAndUpackWebnet77Data();
            }

            // if available, load Webnet77 data
            if (File.Exists(_webNet77DataFile))
            {
                using (StreamReader stream = new StreamReader(_webNet77DataFile))
                {
                    while (!stream.EndOfStream)
                    {
                        string line = stream.ReadLine();

                        if (line[0] == '#' || line[0] == ' ')
                            continue;

                        if (HasCancelled())
                            return false;

                        string[] parts = line.Split(',');

                        long startRange = Convert.ToInt64(parts[0].Replace("\"", ""));
                        long endRange = Convert.ToInt64(parts[1].Replace("\"", ""));
                        string country = parts[4].Replace("\"", "");
                        rangeData.Add(new IpCity(startRange, endRange, country));
                    }
                }
            }

            rangeData.Sort();


            return false;
        }

        #endregion Overridden Methods

        #region Private Members

        private void DownloadAndUpackWebnet77Data()
        {
            if (CanDownloadWebnet77Data())
            {
                string downloadFile = Path.ChangeExtension(_webNetDownloadDataFile, Constants.FileExtensionZip);

                if (DownloadWebnetData(downloadFile))
                {
                    ZipFile.ExtractToDirectory(downloadFile, Utilities.AddTrailingBackSlash(Path.GetDirectoryName(downloadFile)), true);
                }
            }
        }

        private bool DownloadWebnetData(string downloadFile)
        {
            bool Result = true;
            try
            {
#pragma warning disable SYSLIB0014 // Type or member is obsolete
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(_webnet77CsvUrl, downloadFile);
                }
#pragma warning restore SYSLIB0014 // Type or member is obsolete
            }
            catch (WebException err)
            {
                Result = false;
                _logger.AddToLog(PluginManager.LogLevel.Error, err);
            }
            finally
            {
                // you only get one chance at a time, if it fails try again in 24 hours, otherwise could get banned
                File.WriteAllText(_webNetDownloadDataFile, DateTime.UtcNow.Ticks.ToString());
            }

            return Result;
        }

        private bool CanDownloadWebnet77Data()
        {
            if (File.Exists(_webNetDownloadDataFile))
            {
                string fileContents = File.ReadAllText(_webNetDownloadDataFile);

                if (Int64.TryParse(fileContents, out long lastUpdateTime))
                {
                    TimeSpan span = DateTime.UtcNow - new DateTime(lastUpdateTime, DateTimeKind.Utc);

                    return span.TotalMinutes > _downloadFrequency * 1440;
                }
            }
            else
            {
				if (Directory.Exists(Path.GetDirectoryName(_webNetDownloadDataFile)))
					File.WriteAllText(_webNetDownloadDataFile, DateTime.UtcNow.Ticks.ToString());
            }

            return true;
        }

        #endregion Private Members
    }
}
