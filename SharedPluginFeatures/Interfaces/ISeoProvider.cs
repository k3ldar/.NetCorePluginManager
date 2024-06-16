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
 *  Copyright (c) 2012 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatues
 *  
 *  File: ISeoProvider.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  12/05/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

namespace SharedPluginFeatures
{
	/// <summary>
	/// An instance of this interface should be managed by the host application and is used
	/// by the SeoPlugin module to retrieve Seo data that is placed into the request pipeline
	/// which can be used by an individual route to provide custom Seo data.
	/// 
	/// This allows applications to provide a single source for Seo data for the entire application.
	/// </summary>
	public interface ISeoProvider
	{
		/// <summary>
		/// Retrieves Seo data for a route.
		/// </summary>
		/// <param name="route">Route in lowercase.</param>
		/// <param name="title">out string.  Route title.</param>
		/// <param name="metaDescription">out string.  Route meta description.</param>
		/// <param name="author">out string.  Route author.</param>
		/// <param name="keywords">out List&lt;string&gt;.  Route keywords.</param>
		/// <returns></returns>
		bool GetSeoDataForRoute(in string route, out string title, out string metaDescription,
			out string author, out List<string> keywords);

		/// <summary>
		/// Notification to update the title for a specific route.
		/// </summary>
		/// <param name="route">Route which will be updated.</param>
		/// <param name="title">New title for route.</param>
		/// <returns>bool if route title updated, otherwise false.</returns>
		bool UpdateTitle(in string route, in string title);

		/// <summary>
		/// Update description
		/// </summary>
		/// <param name="route">Route which will be updated.</param>
		/// <param name="description">New description for the route.</param>
		/// <returns>bool if route description updated, otherwise false.</returns>
		bool UpdateDescription(in string route, in string description);

		/// <summary>
		/// Updates the author for a route.
		/// </summary>
		/// <param name="route">Route which will be updated.</param>
		/// <param name="author">New author for the route.</param>
		/// <returns>bool if route author updated, otherwise false.</returns>
		bool UpdateAuthor(in string route, in string author);

		/// <summary>
		/// Adds a keyword for the route.
		/// </summary>
		/// <param name="route">Route which will be updated.</param>
		/// <param name="keyword">Keyword to be added.</param>
		/// <returns>bool if route keyword is added, otherwise false.</returns>
		bool AddKeyword(in string route, in string keyword);

		/// <summary>
		/// Keyword to be removed.
		/// </summary>
		/// <param name="route">Route which will be updated.</param>
		/// <param name="keyword">Keyword to be removed.</param>
		/// <returns>bool if route keyword is removed, otherwise false.</returns>
		bool RemoveKeyword(in string route, in string keyword);

		/// <summary>
		/// Group of keywords to be added.
		/// </summary>
		/// <param name="route">Route which will be updated.</param>
		/// <param name="keywords">Keywords to be added.</param>
		/// <returns>bool if route keywords are removed, otherwise false.</returns>
		bool AddKeywords(in string route, in List<string> keywords);

		/// <summary>
		/// Group of keywords to be removed.
		/// </summary>
		/// <param name="route">Route which will be updated.</param>
		/// <param name="keywords">Keywords to be removed.</param>
		/// <returns>bool if route keywords are removed, otherwise false.</returns>
		bool RemoveKeywords(in string route, in List<string> keywords);
	}
}
