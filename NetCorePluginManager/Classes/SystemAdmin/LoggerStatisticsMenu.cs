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
 *  File: LoggerStatistics.cs
 *
 *  Purpose:  Stores n Log items for display in System Admin
 *
 *  Date        Name                Reason
 *  31/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections;
using System.Text;
using System.Threading;

using PluginManager;
using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace AspNetCore.PluginManager.Classes.SystemAdmin
{
	/// <summary>
	/// Returns a list of the last 100 log entries that can be viewed within SystemAdmin.Plugin.  
	/// 
	/// This class descends from SystemAdminSubMenu and ILogger
	/// </summary>
	public class LoggerStatisticsMenu : SystemAdminSubMenu, ILogger
	{
		#region Private Static Members

		private const int MaxQueueLength = 100;
		private static ILogger _logger;
		private static readonly object _lockObject = new();
		private static readonly Queue _queue = new(MaxQueueLength);

		#endregion Private Static Members

		#region Internal Static Methods

		internal static void SetLogger(in ILogger logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		internal static void ClearLogger()
		{
			_logger = null;
		}

		#endregion Internal Static Methods

		#region SystemAdminSubMenu Methods

		public override string Action()
		{
			return String.Empty;
		}

		public override string Area()
		{
			return String.Empty;
		}

		public override string Controller()
		{
			return String.Empty;
		}

		public override Enums.SystemAdminMenuType MenuType()
		{
			return Enums.SystemAdminMenuType.Grid;
		}

		/// <summary>
		/// Returns delimited data on current log data, this will only store the last 100 entries.
		/// </summary>
		/// <returns>string</returns>
		public override string Data()
		{
			StringBuilder Result = new("DateTime|Log Type|Message", MaxQueueLength * 100);
			object[] queueItems;

			using (TimedLock.Lock(_lockObject))
			{
				queueItems = _queue.ToArray();
			}

			for (int i = 0; i < queueItems.Length - 1; i++)
			{
				LoggerQueueItem item = (LoggerQueueItem)queueItems[i];

				Result.Append($"\r{item.Date.ToString(Thread.CurrentThread.CurrentUICulture)}|");
				Result.Append($"{item.Level.ToString()}|{item.Message}");
			}

			return Result.ToString();
		}

		public override string Name()
		{
			return "Logs";
		}

		public override string ParentMenuName()
		{
			return "System";
		}

		public override int SortOrder()
		{
			return 0;
		}

		public override string Image()
		{
			return String.Empty;
		}

		#endregion SystemAdminSubMenu Methods

		#region ILogger Methods

		public void AddToLog(in LogLevel logLevel, in string data)
		{
			AddToLog(logLevel, String.Empty, data);
		}

		public void AddToLog(in LogLevel logLevel, in Exception exception)
		{
			AddToLog(logLevel, exception, String.Empty);
		}

		public void AddToLog(in LogLevel logLevel, in Exception exception, string data)
		{
			AddToLog(logLevel, String.Empty, exception, data);
		}

		public void AddToLog(in LogLevel logLevel, in string moduleName, in string data)
		{
			if (String.IsNullOrEmpty(data))
				throw new ArgumentNullException(nameof(data));

			using (TimedLock.Lock(_lockObject))
			{
				LoggerQueueItem loggerQueueItem = new(logLevel, data);

				if (_queue.Count == MaxQueueLength)
					_queue.Dequeue();

				_queue.Enqueue(loggerQueueItem);
			}

			_logger?.AddToLog(logLevel, moduleName, data);
		}

		public void AddToLog(in LogLevel logLevel, in string moduleName, in Exception exception)
		{
			AddToLog(logLevel, moduleName, exception, String.Empty);
		}

		public void AddToLog(in LogLevel logLevel, in string moduleName, in Exception exception, string data)
		{
			if (exception == null)
				throw new ArgumentNullException(nameof(exception));

#if TRACE
			System.Diagnostics.Trace.WriteLine($"{logLevel} {exception.Message}\r\n{data}");
#endif

			string message = exception.Message.Replace("\r", " ");

			if (!String.IsNullOrEmpty(data))
			{
				message += $"\nData: {data}";
			}

			using (TimedLock.Lock(_lockObject))
			{
				LoggerQueueItem loggerQueueItem = new(logLevel, message);

				if (_queue.Count == MaxQueueLength)
					_queue.Dequeue();

				_queue.Enqueue(loggerQueueItem);
			}

			_logger?.AddToLog(logLevel, moduleName, exception, data);
		}

		#endregion ILogger Methods
	}
}

#pragma warning restore CS1591