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
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: MinificationThread.cs
 *
 *  Purpose:  Background thread that executes minification of files and prevents startup lag
 *
 *  Date        Name                Reason
 *  26/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using PluginManager;
using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;


namespace AspNetCore.PluginManager.Classes.Minify
{
	internal class MinificationThread : ThreadManager
	{
		#region Private Members

		private readonly ILogger _logger;
		private readonly IMinificationEngine _minificationEngine;

		#endregion Private Members

		#region Constructors

		public MinificationThread(in List<string> files, in ILogger logger, in IMinificationEngine minificationEngine)
			: base(files, new TimeSpan())
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_minificationEngine = minificationEngine ?? throw new ArgumentNullException(nameof(minificationEngine));
			HangTimeoutSpan = TimeSpan.FromMinutes(10);
		}

		#endregion Constructors

		#region Overridden Methods

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Not the end of the world if a file can't be minified, log it, move on in life!")]
		protected override Boolean Run(Object parameters)
		{
			int totalBytesSaved = 0;
			Timings minifyTimings = new();
			List<string> files = (List<string>)parameters;

			using (StopWatchTimer.Initialise(minifyTimings))
			{
				foreach (string file in files)
				{
					Timings fileTimings = new();
					using (StopWatchTimer.Initialise(fileTimings))
					{
						try
						{
							_logger.AddToLog(LogLevel.Information, nameof(MinificationThread), $"Minifying {file}");
							MinificationFileType fileType = GetFileType(file);
							byte[] fileData = File.ReadAllBytes(file);

							List<IMinifyResult> minifyResults = _minificationEngine.MinifyData(Encoding.UTF8, fileType, fileData, out byte[] minifiedResult);

							File.WriteAllBytes(file, minifiedResult);

							if (minifyResults.Count > 0)
								totalBytesSaved += minifyResults[0].StartLength - minifyResults[^1].EndLength;

							foreach (IMinifyResult minifyResult in minifyResults)
							{
								_logger.AddToLog(LogLevel.Information, nameof(MinificationThread),
									String.Format("{0}; Start Size: {1}; End Size: {2}; Time Taken: {3}",
									minifyResult.ProcessName, minifyResult.StartLength, minifyResult.EndLength, minifyResult.TimeTaken));
							}
						}
						catch (Exception minifyError)
						{
							_logger.AddToLog(LogLevel.Error, nameof(MinificationThread), minifyError, $"Error minifying {file}");
						}

					}

					_logger.AddToLog(LogLevel.Information, $"Total Time: {fileTimings.Fastest}ms; Minify {file}");
				}

			}

			_logger.AddToLog(LogLevel.Information, nameof(MinificationThread), $"Total minified bytes removed: {totalBytesSaved}; Total Time: {minifyTimings.Fastest}ms");

			return false;
		}

		#endregion Overridden Methods

		#region Private Methods

		private static MinificationFileType GetFileType(in string fileName)
		{
			switch (Path.GetExtension(fileName).ToLower())
			{
				case ".html":
					return MinificationFileType.Html;
				case ".htm":
					return MinificationFileType.Htm;
				case ".css":
					return MinificationFileType.CSS;
				case ".less":
					return MinificationFileType.Less;
				case ".js":
					return MinificationFileType.Js;
				case ".cshtml":
				case ".vbhtml":
					return MinificationFileType.Razor;
				case ".gif":
					return MinificationFileType.ImageGif;
				case ".jpg":
				case ".jpeg":
					return MinificationFileType.ImageJpeg;
				case ".png":
					return MinificationFileType.ImagePng;

				default:
					return MinificationFileType.Unknown;
			}
		}

		#endregion Private Methods
	}
}
