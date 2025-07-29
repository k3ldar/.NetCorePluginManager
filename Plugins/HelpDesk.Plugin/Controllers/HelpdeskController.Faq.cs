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
 *  File: HelpdeskController.Faq.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using HelpdeskPlugin.Models;

using Microsoft.AspNetCore.Mvc;

using Middleware.Helpdesk;

using SharedPluginFeatures;

#pragma warning disable CS1591, IDE0060

namespace HelpdeskPlugin.Controllers
{
	public partial class HelpdeskController
	{
		#region Public Action Methods

		[Breadcrumb(nameof(Languages.LanguageStrings.FrequentlyAskedQuestions), Name, nameof(Index), HasParams = true)]
		public IActionResult FaQ()
		{
			if (!_settings.ShowFaq)
				return RedirectToAction(nameof(Index), Name);

			List<KnowledgeBaseGroup> groups = _helpdeskProvider.GetKnowledgebaseGroups(UserId(), null);

			return View(CreateFaqViewModel(groups, null));
		}

		[Route("/Helpdesk/Faq/{groupId}/{groupName}/")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Forms part of route name")]
		public IActionResult FaQ(int groupId, string groupName)
		{
			if (!_settings.ShowFaq)
				return RedirectToAction(nameof(Index), Name);

			KnowledgeBaseGroup activeGroup = _helpdeskProvider.GetKnowledgebaseGroup(UserId(), groupId);

			if (activeGroup == null)
				return RedirectToAction(nameof(FaQ), Name);

			List<KnowledgeBaseGroup> groups = _helpdeskProvider.GetKnowledgebaseGroups(UserId(), activeGroup);

			return View(CreateFaqViewModel(groups, activeGroup));
		}

		[HttpGet]
		[Route("/Helpdesk/FaQItem/{id}/{description}/")]
		[Breadcrumb(nameof(Languages.LanguageStrings.FrequentlyAskedQuestions), Name, nameof(FaQ))]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Forms part of route name")]
		public IActionResult FaQItem(int id, int description)
		{
			if (!_helpdeskProvider.GetKnowledgebaseItem(UserId(), id,
				out KnowledgeBaseItem item, out KnowledgeBaseGroup parentGroup))
			{
				return RedirectToAction(nameof(FaQ), Name);
			}

			return View(CreateFaQViewItemModel(parentGroup, item));
		}

		#endregion Public Action Methods

		#region Private Methods

		private FaqItemViewModel CreateFaQViewItemModel(KnowledgeBaseGroup parentGroup, KnowledgeBaseItem item)
		{
			_helpdeskProvider.KnowledgebaseView(item);

			List<BreadcrumbItem> crumbs = GetBreadcrumbs().Take(2).ToList();

			crumbs.Add(new BreadcrumbItem(Languages.LanguageStrings.FrequentlyAskedQuestions, $"/{Name}/{nameof(FaQ)}/", false));

			crumbs.Add(new BreadcrumbItem(parentGroup.Name,
				$"/{Name}/{nameof(FaQ)}/{parentGroup.Id}/{BaseModel.RouteFriendlyName(parentGroup.Name)}/", true));

			KnowledgeBaseGroup currGroup = parentGroup.Parent;

			while (currGroup != null)
			{
				crumbs.Insert(3, new BreadcrumbItem(currGroup.Name,
					$"/{Name}/{nameof(FaQ)}/{currGroup.Id}/{BaseModel.RouteFriendlyName(currGroup.Name)}/", true));
				currGroup = currGroup.Parent;
			}

			IBaseModelData modelData = GetModelData();
			modelData.ReplaceBreadcrumbs(crumbs);

			return new FaqItemViewModel(modelData,
				KnowledgeBaseToFaQGroup(parentGroup), item.Description, item.ViewCount,
				FormatTextForDisplay(item.Content));
		}

		private FaqGroupViewModel CreateFaqViewModel(List<KnowledgeBaseGroup> groups,
			in KnowledgeBaseGroup activeGroup)
		{
			List<FaqGroup> faqGroups = [];

			foreach (KnowledgeBaseGroup group in groups)
			{
				faqGroups.Add(KnowledgeBaseToFaQGroup(group));
			}

			List<BreadcrumbItem> crumbs = GetBreadcrumbs().Take(2).ToList();

			if (activeGroup != null)
			{
				crumbs.Add(new BreadcrumbItem(Languages.LanguageStrings.FrequentlyAskedQuestions, $"/{Name}/{nameof(FaQ)}/", false));

				crumbs.Add(new BreadcrumbItem(activeGroup.Name,
					$"/{Name}/{nameof(FaQ)}/{activeGroup.Id}/{BaseModel.RouteFriendlyName(activeGroup.Name)}/", true));

				KnowledgeBaseGroup currGroup = activeGroup.Parent;

				while (currGroup != null)
				{
					crumbs.Insert(3, new BreadcrumbItem(currGroup.Name,
						$"/{Name}/{nameof(FaQ)}/{currGroup.Id}/{BaseModel.RouteFriendlyName(currGroup.Name)}/", true));
					currGroup = currGroup.Parent;
				}
			}

			IBaseModelData modelData = GetModelData();
			modelData.ReplaceBreadcrumbs(crumbs);
			return new FaqGroupViewModel(modelData, faqGroups, KnowledgeBaseToFaQGroup(activeGroup));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private FaqGroup KnowledgeBaseToFaQGroup(KnowledgeBaseGroup group)
		{
			if (group == null)
				return null;

			List<FaqGroupItem> items = [];

			foreach (KnowledgeBaseItem item in group.Items)
			{
				items.Add(new FaqGroupItem(item.Id, item.Description, item.ViewCount, item.Content));
			}

			int subGroupCount = _helpdeskProvider.GetKnowledgebaseGroups(UserId(), group).Count;

			return new FaqGroup(group.Id, group.Name, group.Description, items, subGroupCount);
		}

		#endregion Private Methods
	}
}

#pragma warning restore CS1591, IDE0060