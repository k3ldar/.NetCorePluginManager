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
 *  Product:  Resources.Plugin
 *  
 *  File: KeywordSearchProvider.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  30/10/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Threading;

using Microsoft.AspNetCore.Http;

using Middleware;
using Middleware.Classes.Search;
using Middleware.Resources;
using Middleware.Search;

#pragma warning disable CS1591

namespace Resources.Plugin.Classes
{
	public class KeywordSearchProvider : BaseSearchProvider, ISearchKeywordProvider
	{
		private const string SearchType = "ResourceDescription";
		private const string ResourceName = "ResourceName";
		private const string ResourceTag = "ResourceTag";
		private readonly IResourceProvider _provider;

		public KeywordSearchProvider(IResourceProvider resourceProvider)
		{
			_provider = resourceProvider ?? throw new ArgumentNullException(nameof(resourceProvider));
		}

		public Dictionary<string, AdvancedSearchOptions> AdvancedSearch()
		{
			return null;
		}

		public List<SearchResponseItem> Search(in KeywordSearchOptions searchOptions)
		{
			if (searchOptions == null)
				throw new ArgumentNullException(nameof(searchOptions));

			List<SearchResponseItem> Result = [];

			if (searchOptions.ExactMatch || searchOptions.QuickSearch)
			{
				ExactMatch(Result, searchOptions, _provider.RetrieveAllResourceItems());

				if (Result.Count < searchOptions.MaximumSearchResults)
					ExactMatch(Result, searchOptions, _provider.RetrieveAllCategories());
			}
			else
			{
				NonExactMatch(Result, searchOptions, _provider.RetrieveAllResourceItems());

				if (Result.Count < searchOptions.MaximumSearchResults)
					NonExactMatch(Result, searchOptions, _provider.RetrieveAllCategories());
			}

			return Result;
		}

		public List<string> SearchResponseTypes(in bool quickSearch)
		{
			List<string> Result =
			[
				"Name",
				"Tags"
			];

			if (!quickSearch)
			{
				Result.Add("Description");
			}

			return Result;
		}

		private static void ExactMatch(in List<SearchResponseItem> results, in KeywordSearchOptions searchOptions,
			List<ResourceItem> resources)
		{
			foreach (ResourceItem resource in resources)
			{
				if (results.Count >= searchOptions.MaximumSearchResults)
				{
					return;
				}

				int offset = resource.Name.IndexOf(searchOptions.SearchTerm, StringComparison.InvariantCultureIgnoreCase);

				if (offset > -1 && results.Count < searchOptions.MaximumSearchResults)
				{
					AddSearchResult(results, resource, ResourceName, offset);
					continue;
				}

				foreach (string tag in resource.Tags)
				{
					if (tag.IndexOf(searchOptions.SearchTerm, StringComparison.InvariantCultureIgnoreCase) > -1)
					{
						AddSearchResult(results, resource, ResourceTag, -1);
						break;
					}
				}

				if (!searchOptions.QuickSearch)
				{
					offset = resource.Description.IndexOf(searchOptions.SearchTerm, StringComparison.InvariantCultureIgnoreCase);

					if (offset > -1 && results.Count < searchOptions.MaximumSearchResults)
					{
						AddSearchResult(results, resource, SearchType, offset);
					}
				}
			}
		}

		private static void ExactMatch(in List<SearchResponseItem> results, in KeywordSearchOptions searchOptions,
			List<ResourceCategory> resources)
		{
			foreach (ResourceCategory resource in resources)
			{
				if (results.Count >= searchOptions.MaximumSearchResults)
				{
					return;
				}

				int offset = resource.Name.IndexOf(searchOptions.SearchTerm, StringComparison.InvariantCultureIgnoreCase);

				if (offset > -1 && results.Count < searchOptions.MaximumSearchResults)
				{
					AddSearchResult(results, resource, ResourceName, offset);
					continue;
				}

				if (!searchOptions.QuickSearch)
				{
					offset = resource.Description.IndexOf(searchOptions.SearchTerm, StringComparison.InvariantCultureIgnoreCase);

					if (offset > -1 && results.Count < searchOptions.MaximumSearchResults)
					{
						AddSearchResult(results, resource, SearchType, offset);
					}
				}
			}
		}

		private static void NonExactMatch(in List<SearchResponseItem> results, in KeywordSearchOptions searchOptions,
			in List<ResourceItem> resources)
		{
			string[] words = searchOptions.SearchTerm.Split(" ", StringSplitOptions.RemoveEmptyEntries);

			if (words.Length == 0)
				return;

			foreach (string word in words)
			{
				if (String.IsNullOrWhiteSpace(word) || ExcludeWordFromSearch(word))
					continue;

				foreach (ResourceItem resource in resources)
				{
					if (results.Count >= searchOptions.MaximumSearchResults)
					{
						return;
					}

					int offset = resource.Name.IndexOf(word, StringComparison.InvariantCultureIgnoreCase);

					if (offset > -1 && results.Count < searchOptions.MaximumSearchResults)
					{
						AddSearchResult(results, resource, ResourceName, offset);
						continue;
					}

					foreach (string tag in resource.Tags)
					{
						if (tag.IndexOf(word, StringComparison.InvariantCultureIgnoreCase) > -1)
						{
							AddSearchResult(results, resource, ResourceTag, -1);
							break;
						}
					}

					if (!searchOptions.QuickSearch)
					{
						offset = resource.Description.IndexOf(word, StringComparison.InvariantCultureIgnoreCase);

						if (offset > -1 && results.Count < searchOptions.MaximumSearchResults)
						{
							AddSearchResult(results, resource, SearchType, offset);
						}
					}
				}
			}
		}

		private static void NonExactMatch(in List<SearchResponseItem> results, in KeywordSearchOptions searchOptions,
			in List<ResourceCategory> resources)
		{
			string[] words = searchOptions.SearchTerm.Split(" ", StringSplitOptions.RemoveEmptyEntries);

			if (words.Length == 0)
				return;

			foreach (string word in words)
			{
				if (String.IsNullOrWhiteSpace(word) || ExcludeWordFromSearch(word))
					continue;

				foreach (ResourceCategory resource in resources)
				{
					if (results.Count >= searchOptions.MaximumSearchResults)
					{
						return;
					}

					int offset = resource.Name.IndexOf(word, StringComparison.InvariantCultureIgnoreCase);

					if (offset > -1 && results.Count < searchOptions.MaximumSearchResults)
					{
						AddSearchResult(results, resource, ResourceName, offset);
						continue;
					}

					if (!searchOptions.QuickSearch)
					{
						offset = resource.Description.IndexOf(word, StringComparison.InvariantCultureIgnoreCase);

						if (offset > -1 && results.Count < searchOptions.MaximumSearchResults)
						{
							AddSearchResult(results, resource, SearchType, offset);
						}
					}
				}
			}
		}

		private static void AddSearchResult(in List<SearchResponseItem> results, in ResourceItem resource, in string searchType, in int offset)
		{
			SearchResponseItem searchItem = new(searchType, resource.Name, offset,
				$"/{Controllers.ResourcesController.Name}/View/{resource.Id}/", resource.Name, null);

			searchItem.Properties.Add(nameof(resource.Id), resource.Id);

			results.Add(searchItem);
		}

		private static void AddSearchResult(in List<SearchResponseItem> results, in ResourceCategory resource, in string searchType, in int offset)
		{
			SearchResponseItem searchItem = new(searchType, resource.Name, offset,
				$"/{Controllers.ResourcesController.Name}/Category/{resource.Id}/{resource.Name}/", resource.Name, null);

			searchItem.Properties.Add(nameof(resource.Id), resource.Id);

			results.Add(searchItem);
		}
	}
}

#pragma warning restore CS1591