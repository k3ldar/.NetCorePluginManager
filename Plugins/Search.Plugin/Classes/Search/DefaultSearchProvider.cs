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
 *  Product:  Search Plugin
 *  
 *  File: DefaultSearchProvider.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  12/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Middleware;
using Middleware.Search;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

namespace SearchPlugin.Classes.Search
{
	/// <summary>
	/// Default search provider to be used if no other search provider is registered
	/// </summary>
	public class DefaultSearchProvider : ISearchProvider
	{
		#region Private Members

		private readonly List<ISearchKeywordProvider> _searchProviders;

		#endregion Private Members

		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="pluginClassesService">IPluginClassesService instance</param>
		public DefaultSearchProvider(IPluginClassesService pluginClassesService)
		{
			if (pluginClassesService == null)
				throw new ArgumentNullException(nameof(pluginClassesService));

			_searchProviders = pluginClassesService.GetPluginClasses<ISearchKeywordProvider>();
		}

		#endregion Constructors

		#region ISearchProvider Methods

		/// <summary>
		/// Performs a keyword search
		/// </summary>
		/// <param name="keywordSearchOptions"></param>
		/// <returns></returns>
		public List<SearchResponseItem> KeywordSearch(in KeywordSearchOptions keywordSearchOptions)
		{
			return DefaultSearchThread.KeywordSearch(_searchProviders, keywordSearchOptions);
		}

		/// <summary>
		/// Retrieves all available search response types from all registered search providers
		/// </summary>
		/// <param name="quickSearch">Indicates whether its the providers from a quick search or normal search.</param>
		/// <returns>List&lt;string&gt;</returns>
		public List<string> SearchResponseTypes(in Boolean quickSearch)
		{
			List<string> Result = [];

			foreach (ISearchKeywordProvider provider in _searchProviders)
			{
				List<string> providerResults = provider.SearchResponseTypes(quickSearch);

				if (providerResults == null)
					continue;

				providerResults.ForEach(st => Result.Add(st));
			}

			return Result;
		}

		/// <summary>
		/// Retrieves a list of strings from all search providers that can optionally be used by the UI 
		/// to provide a paged or tabbed advance search option.
		/// </summary>
		/// <returns>Dictionary&lt;string, AdvancedSearchOptions&gt;</returns>
		public Dictionary<string, AdvancedSearchOptions> AdvancedSearch()
		{
			Dictionary<string, AdvancedSearchOptions> Result = [];

			foreach (ISearchKeywordProvider provider in _searchProviders)
			{
				Dictionary<string, AdvancedSearchOptions> advancedSearch = provider.AdvancedSearch();

				if (advancedSearch == null)
					continue;

				foreach (KeyValuePair<string, AdvancedSearchOptions> item in advancedSearch)
					Result.Add(item.Key, item.Value);
			}

			return Result;
		}

		/// <summary>
		/// Retrieve existing search results if they exist.
		/// </summary>
		/// <param name="searchId">Name of search</param>
		/// <returns>List&lt;SearchResponseItem&gt;</returns>
		public List<SearchResponseItem> GetSearchResults(in String searchId)
		{
			if (String.IsNullOrEmpty(searchId))
			{
				return null;
			}

			return DefaultSearchThread.RetrieveSearch(searchId);
		}

		#endregion ISearchProvider Methods

		#region Properties

		internal static Timings SearchTimings
		{
			get
			{
				return DefaultSearchThread.SearchTimings;
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Internal and used in other places")]
		internal CacheManager GetCacheManager
		{
			get
			{
				return DefaultSearchThread.SearchCache;
			}
		}

		#endregion Properties
	}
}
