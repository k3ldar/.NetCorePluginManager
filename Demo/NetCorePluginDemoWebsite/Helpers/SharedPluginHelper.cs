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
 *  Product:  AspNetCore.PluginManager.DemoWebsite
 *  
 *  File: SharedPluginHelper.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.DemoWebsite.Helpers
{
	/// <summary>
	/// This class acts as a wrapper arund the elements you extend through plugin manager
	/// </summary>
	public sealed class SharedPluginHelper : ISharedPluginHelper
	{
		#region Constants

		private const string MainMenuCache = "Main Menu Cache";

		#endregion Constants

		#region Private Methods

		private readonly IMemoryCache _memoryCache;
		private readonly IPluginClassesService _pluginClassesService;

		#endregion Private Methods

		#region Constructors

		public SharedPluginHelper(IMemoryCache memoryCache, IPluginClassesService pluginClassesService)
		{
			_memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
			_pluginClassesService = pluginClassesService ?? throw new ArgumentNullException(nameof(pluginClassesService));
		}

		#endregion Constructors

		public List<MainMenuItem> BuildMainMenu()
		{
			CacheItem cache = _memoryCache.GetExtendingCache().Get(MainMenuCache);

			if (cache == null)
			{
				cache = new CacheItem(MainMenuCache, _pluginClassesService.GetPluginClasses<MainMenuItem>());
				_memoryCache.GetExtendingCache().Add(MainMenuCache, cache);
			}

			return (List<MainMenuItem>)cache.Value;
		}
	}
}
