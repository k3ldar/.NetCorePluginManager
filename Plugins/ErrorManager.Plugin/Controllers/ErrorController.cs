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
 *  Product:  Error Manager Plugin
 *  
 *  File: TempErrorManager.cs
 *
 *  Purpose:  Provides a container for IErrorManager when not used as a plugin
 *
 *  Date        Name                Reason
 *  17/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.IO;

using ErrorManager.Plugin.Models;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using PluginManager.Abstractions;

using SharedPluginFeatures;

using static Shared.Utilities;

#pragma warning disable CS1591

namespace ErrorManager.Plugin.Controllers
{
	/// <summary>
	/// Error Controller
	/// </summary>
	[DenySpider]
	public class ErrorController : BaseController
	{
		#region Private Members

#if NET_CORE_3_X || NET_5_ABOVE
		private readonly IWebHostEnvironment _hostingEnvironment;
#else
        private readonly IHostingEnvironment _hostingEnvironment;
#endif

		private readonly ISettingsProvider _settingsProvider;

		#endregion Private Members

		#region Constructors

#if NET_CORE_3_X || NET_5_ABOVE
		public ErrorController(IWebHostEnvironment hostingEnvironment, ISettingsProvider settingsProvider)
		{
			_hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
			_settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
		}
#else
        public ErrorController(IHostingEnvironment hostingEnvironment, ISettingsProvider settingsProvider)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
        }
#endif


		#endregion Constructors

		#region Public Action Methods

		[Breadcrumb(nameof(Languages.LanguageStrings.Error))]
		public IActionResult Index()
		{
			return View(new BaseModel(GetModelData()));
		}

		[Breadcrumb(nameof(Languages.LanguageStrings.MissingLink))]
		public IActionResult NotFound404()
		{
			Error404Model model;

			ErrorManagerSettings settings = _settingsProvider.GetSettings<ErrorManagerSettings>(nameof(ErrorManager));

			if (settings.RandomQuotes)
			{
				// grab a random quote
				Random rnd = new(Convert.ToInt32(DateTime.Now.ToString("Hmsffff")));
				int quote = rnd.Next(settings.Count());
				model = new Error404Model(GetModelData(),
					Languages.LanguageStrings.PageNotFound, settings.GetQuote(quote), GetImageFile(quote));
			}
			else
			{
				int index = 0;

				// sequential, save current state to cookie
				if (CookieExists("Error404"))
				{
					// get index from cookie
					string cookieValue = Decrypt(CookieValue("Error404"), settings.EncryptionKey);
					index = StrToInt(cookieValue, 0) + 1;
				}

				if (index < 0 || index > settings.Count())
					index = 0;

				CookieAdd("Error404", Encrypt(Convert.ToString(index), settings.EncryptionKey),
					Constants.SessionOnlyCookie, true);

				model = new Error404Model(GetModelData(),
					Languages.LanguageStrings.PageNotFound, settings.GetQuote(index), GetImageFile(index));
			}

			return View(model);
		}

		[Breadcrumb(nameof(Languages.LanguageStrings.HighVolume))]
		public IActionResult HighVolume()
		{
			return View(new BaseModel(GetModelData()));
		}

		[Breadcrumb(nameof(Languages.LanguageStrings.NotAcceptable))]
		public IActionResult NotAcceptable()
		{
			return View(new BaseModel(GetModelData()));
		}

		[Breadcrumb(nameof(Languages.LanguageStrings.AccessDenied))]
		public IActionResult AccessDenied()
		{
			return View(new BaseModel(GetModelData()));
		}

#if DEBUG
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1030:Use events where appropriate", Justification = "Debug Only")]
		public IActionResult Raise(string s)
		{
			if (String.IsNullOrEmpty(s))
				s = "Oopsies";

			throw new Exception(s);
		}
#endif
		#endregion Public Action Methods

		#region Private Methods

		private string GetImageFile(in int index)
		{
			string rootPath = $"{_hostingEnvironment.ContentRootPath}\\wwwroot\\images\\error\\";

			if (Directory.Exists(rootPath))
			{
				string[] errorImageFiles = Directory.GetFiles(rootPath);

				foreach (string file in errorImageFiles)
				{
					if (Path.GetFileName(file).StartsWith(index.ToString()))
						return $"/images/error/{Path.GetFileName(file)}";
				}

				if (System.IO.File.Exists(rootPath + "Default404.png"))
					return "/images/error/Default404.png";
			}

			return String.Empty;
		}

		#endregion Private Methods
	}
}

#pragma warning restore CS1591