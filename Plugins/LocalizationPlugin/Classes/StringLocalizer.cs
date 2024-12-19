/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Localization.Plugin
 *  
 *  File: StringLocalizer.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  12/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Threading;

using Microsoft.Extensions.Localization;

using PluginManager;
using PluginManager.Abstractions;

using SharedPluginFeatures;

namespace Localization.Plugin
{
	internal sealed class StringLocalizer : IStringLocalizer
	{
		#region Private Members

		private static readonly Timings _timings = new();

		private readonly ILogger _logger;
		private readonly List<ResourceManager> _resourceManagers = [];

		#endregion Private Members

		#region Constructors

		public StringLocalizer(ILogger logger, IPluginClassesService pluginClassesService)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));

			if (pluginClassesService == null)
				throw new ArgumentNullException(nameof(pluginClassesService));

			List<ILanguageFile> languageFiles = pluginClassesService.GetPluginClasses<ILanguageFile>();

			foreach (ILanguageFile languageFile in languageFiles)
			{
				_resourceManagers.Add(new ResourceManager(languageFile.Name, languageFile.Assembly));
			}
		}


		#endregion Constructors

		#region IStringLocalizer Methods

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "it's ok here, nothing to see, move along")]
		public LocalizedString this[string name]
		{
			get
			{
				using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timings))
				{
					try
					{
						string resourceName = RemoveNonAlphaNumericChars(name);

						foreach (ResourceManager resourceManager in _resourceManagers)
						{
							string locString = resourceManager.GetString(resourceName, Thread.CurrentThread.CurrentUICulture);

							if (String.IsNullOrEmpty(locString))
								continue;

							return new LocalizedString(name, locString);
						}

						return new LocalizedString(name, name);
					}
					catch (Exception error)
					{
						_logger.AddToLog(LogLevel.Error, nameof(StringLocalizer), error, name);
						return new LocalizedString(name, name);
					}
				}
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "it's ok here, nothing to see, move along")]
		public LocalizedString this[string name, params object[] arguments]
		{
			get
			{
				using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timings))
				{
					try
					{
						string resourceName = RemoveNonAlphaNumericChars(name);

						foreach (ResourceManager resourceManager in _resourceManagers)
						{
							string locString = resourceManager.GetString(resourceName, Thread.CurrentThread.CurrentUICulture);

							if (String.IsNullOrEmpty(locString))
								continue;

							return new LocalizedString(name, String.Format(locString, arguments));
						}

						return new LocalizedString(name, String.Format(resourceName, arguments));
					}
					catch (Exception error)
					{
						_logger.AddToLog(LogLevel.Error, nameof(StringLocalizer), error, name);
						return new LocalizedString(name, String.Format(name, arguments));
					}
				}
			}
		}

		public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
		{
			// required from interface, not used in this context
			throw new InvalidOperationException();
		}

		#endregion IStringLocalizer Methods

		private static string RemoveNonAlphaNumericChars(string name)
		{
			StringBuilder Result = new(name.Length);

			foreach (char c in name)
			{
				if (c >= 65 && c <= 90)
					Result.Append(c);
				else if (c >= 61 && c <= 122)
					Result.Append(c);
				else if (c >= 48 && c <= 57)
					Result.Append(c);
			}

			return Result.ToString();
		}

		#region Internal Properties

		internal static Timings LocalizationTimings
		{
			get
			{
				return _timings.Clone();
			}
		}

		#endregion Internal Properties
	}
}
