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
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: MicrosoftDefenderVirusScanner.cs
 *
 *  Purpose:  Default virus scanner for windows OS 
 *
 *  Date        Name                Reason
 *  02/06/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics;
using System.IO;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

using plMgr = PluginManager;

namespace AspNetCore.PluginManager.Internal
{
    internal sealed class MicrosoftDefenderVirusScanner : IVirusScanner
    {
        #region Private Members

        private const string DefenderPath = "Microsoft\\Windows Defender\\Platform";
        private const string DefenderExe = "MpCmdRun.exe";
        private string _defenderProcess = null;
        private static readonly Timings _timingsVirusScan = new Timings();

        private readonly ILogger _logger;

        #endregion Private Members

        #region Constructors

        public MicrosoftDefenderVirusScanner(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Enabled = IsDefenderInstalled();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Indicates whether the virus scanner is enabled or not
        /// </summary>
        public bool Enabled { get; }

        internal static Timings ScanTimings
        {
            get
            {
                return _timingsVirusScan.Clone();
            }
        }

        #endregion Properties

        #region IVirusScanner Methods

        public void ScanDirectory(in string directory)
        {
            if (String.IsNullOrWhiteSpace(directory))
                return;

            if (Enabled)
            {
                if (!Directory.Exists(directory))
                {
                    _logger.AddToLog(plMgr.LogLevel.Warning, nameof(MicrosoftDefenderVirusScanner), $"Directory does not exist: {directory}");
                    return;
                }

                RunScan(directory);
            }
        }

        public void ScanFile(in string fileName)
        {
            if (String.IsNullOrWhiteSpace(fileName))
                return;

            if (!File.Exists(fileName))
                return;

            RunScan(fileName);
        }

        public void ScanFile(in string[] fileNames)
        {
            foreach (string file in fileNames)
            {
                ScanFile(file);
            }
        }

        #endregion IVirusScanner Methods

        #region Private Methods

        private void RunScan(string pathOrFile)
        {
            using (TimedLock timedLock = TimedLock.Lock(_timingsVirusScan))
            using (StopWatchTimer scanTimer = StopWatchTimer.Initialise(_timingsVirusScan))
            using (Process process = new Process())
            {
                process.StartInfo.FileName = _defenderProcess;
                process.StartInfo.Arguments = $"-Scan -ScanType 3 -File \"{pathOrFile}\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;

                try
                {
                    process.Start();

                    StreamReader reader = process.StandardOutput;
                    string contents = reader.ReadToEnd();
                    process.WaitForExit();
                    _logger.AddToLog(plMgr.LogLevel.Information, nameof(RunScan), contents);
                }
                catch (Exception err)
                {
                    _logger.AddToLog(plMgr.LogLevel.Error, err, $"{_defenderProcess} -Scan -ScanType 3 -File \"{pathOrFile}\"");
                }
            }
        }

        private bool IsDefenderInstalled()
        {
            string defenderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), DefenderPath);
            if (!Directory.Exists(defenderPath))
                return false;

            _defenderProcess = GetLatestDefenderFolder(defenderPath);

            if (_defenderProcess == null)
                return false;

            return true;
        }

        private static string GetLatestDefenderFolder(string defenderPath)
        {
            DateTime newest = DateTime.MinValue;
            string Result = null;
            string[] directories = Directory.GetDirectories(defenderPath);

            foreach (string directory in directories)
            {
                DirectoryInfo di = new DirectoryInfo(directory);

                if (di.CreationTime > newest && File.Exists(Path.Combine(directory, DefenderExe)))
                {
                    newest = di.CreationTime;
                    Result = Path.Combine(directory, DefenderExe);
                }
            }

            return Result;
        }

        #endregion Private Methods
    }
}
