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
 *  Product:  Helpdesk Plugin
 *  
 *  File: HelpdeskSitemapProvider.cs
 *
 *  Purpose:  Provides sitemap functionality for Helpdesk items
 *
 *  Date        Name                Reason
 *  27/07/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using HelpdeskPlugin.Controllers;

using Middleware.Helpdesk;

using SharedPluginFeatures;

namespace HelpdeskPlugin.Classes
{
	/// <summary>
	/// Helpdesk sitemap provider, provides sitemap information for helpdesk items
	/// </summary>
	public class HelpdeskSitemapProvider : ISitemapProvider
	{
		#region Private Members

		private readonly IHelpdeskProvider _helpdeskProvider;

		#endregion Private Members

		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="helpdeskProvider">IHelpdeskProvider instance</param>
		public HelpdeskSitemapProvider(IHelpdeskProvider helpdeskProvider)
		{
			_helpdeskProvider = helpdeskProvider ?? throw new ArgumentNullException(nameof(helpdeskProvider));
		}

		#endregion Constructors

		/// <summary>
		/// Retrieve a list of all helpdesk items that will be included in the sitemap
		/// </summary>
		/// <returns>List&lt;ISitemapItem&gt;</returns>
		public List<SitemapItem> Items()
		{
			List<SitemapItem> Result =
			[
				new SitemapItem(
					new Uri($"{HelpdeskController.Name}/{nameof(HelpdeskController.Feedback)}", UriKind.RelativeOrAbsolute),
						SitemapChangeFrequency.Weekly),
			];

			List<KnowledgeBaseGroup> faqGroups = _helpdeskProvider.GetKnowledgebaseGroups(0, null);

			foreach (KnowledgeBaseGroup faqGroup in faqGroups)
			{
				AddFaqItem(Result, faqGroup);
			}

			return Result;
		}

		private void AddFaqItem(in List<SitemapItem> result, KnowledgeBaseGroup faqGroup)
		{
			Uri faqUrl = new($"{HelpdeskController.Name}/{nameof(HelpdeskController.FaQ)}/{faqGroup.Id}/{BaseModel.RouteFriendlyName(faqGroup.Name)}/",
				UriKind.RelativeOrAbsolute);

			result.Add(new SitemapItem(faqUrl, SitemapChangeFrequency.Daily));

			foreach (KnowledgeBaseGroup subGroup in _helpdeskProvider.GetKnowledgebaseGroups(0, faqGroup))
				AddFaqItem(result, subGroup);
		}
	}
}
