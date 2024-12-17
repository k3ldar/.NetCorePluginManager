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
 *  Product:  DynamicContent.Plugin
 *  
 *  File: DynamicContentController.cs
 *
 *  Purpose:  Dynamic Content Controller
 *
 *  Date        Name                Reason
 *  13/08/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using DynamicContent.Plugin.Model;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Middleware;
using Middleware.DynamicContent;
using Middleware.Interfaces;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;
using SharedPluginFeatures.DynamicContent;

using static SharedPluginFeatures.Constants;

#pragma warning disable CS1591

namespace DynamicContent.Plugin.Controllers
{
	/// <summary>
	/// Dynamic content controller allows for editing of dynamic content on a website.
	/// </summary>
	[DenySpider]
	public partial class DynamicContentController : BaseController
	{
		#region Private Members

		public const string Name = "DynamicContent";

		private const string ViewEditPage = "/Views/DynamicContent/EditPage.cshtml";

		private const string InvalidCacheId = "Invalid cache id";
		private const string InvalidCacheItem = "Invalid cache item";
		private const string InvalidContentPage = "Invalid template";
		private const string InvalidModelData = "Invalid Model Data";
		private const string InvalidControlId = "Invalid control id";
		private const string InvalidTemplate = "Invalid template";
		private const string InvalidTemplateNotFound = "Template not found";
		private const string InvalidDynamicPage = "Invalid dynamic page";
		private const string InvalidControl = "Invalid page control";
		private const string InvalidUniqueId = "Invalid unique id";

		private const string EditControlRow = "<li id=\"{0}\" class=\"col-edit-{2} ui-state-default editControl\">" +
				"<p class=\"ctlHeader\">{1}<span class=\"deleteBtn\" id=\"{0}\">X</span><span class=\"editBtn\" id=\"{0}\" data-cs=\"{5}\">{3}</span></p><div class=\"ctlContent\">{4}</div></li>";

		private readonly IDynamicContentProvider _dynamicContentProvider;
		private readonly IMemoryCache _memoryCache;
		private readonly INotificationService _notificationService;

		#endregion Private Members

		#region Constructors

		public DynamicContentController(IDynamicContentProvider dynamicContentProvider,
			IMemoryCache memoryCache,
			INotificationService notificationService)
		{
			_dynamicContentProvider = dynamicContentProvider ?? throw new ArgumentNullException(nameof(dynamicContentProvider));
			_memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
			_notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
		}

		#endregion Constructors

		#region Public Action Methods

		public static List<LookupListItem> GetHeightTypes()
		{
			return
			[
				new(1, DynamicContentHeightType.Automatic.ToString()),
				new(3, DynamicContentHeightType.Percentage.ToString()),
				new(4, DynamicContentHeightType.Pixels.ToString()),
			];
		}

		public static List<LookupListItem> GetWidthTypes()
		{
			return
			[
				new(1, DynamicContentWidthType.Columns.ToString()),
				new(2, DynamicContentWidthType.Percentage.ToString()),
				new(3, DynamicContentWidthType.Pixels.ToString()),
			];
		}

		[HttpGet]
		[Route("Page/{*path}")]
		public IActionResult Index(string path)
		{
			if (String.IsNullOrEmpty(path))
			{
				return StatusCode(HtmlResponseNotFound);
			}

			CacheItem cacheItem = PluginInitialisation.DynamicContentCache.Get(path.ToLower());

			if (cacheItem == null)
				return StatusCode(HtmlResponseNotFound);


			if (cacheItem.Value is not IDynamicContentPage dynamicContentPage)
				return StatusCode(HtmlResponseNotFound);

			return View("/Views/DynamicContent/Index.cshtml", GetDynamicContentPageModel(dynamicContentPage, false));
		}

		[HttpPost]
		public IActionResult SubmitData(string path, string data)
		{
			if (String.IsNullOrEmpty(path))
			{
				return StatusCode(HtmlResponseNotFound);
			}

			CacheItem cacheItem = PluginInitialisation.DynamicContentCache.Get(path);

			if (cacheItem == null)
				return StatusCode(HtmlResponseNotFound);

			if (cacheItem.Value is not IDynamicContentPage dynamicContentPage)
				return StatusCode(HtmlResponseNotFound);

			bool saveUserData = _dynamicContentProvider.SaveUserInput(data);

			return View("/Views/DynamicContent/Index.cshtml", GetDynamicContentPageModel(dynamicContentPage, saveUserData));
		}

		[LoggedIn]
		[HttpPost]
		[Authorize(Policy = PolicyNameContentEditor)]
		public IActionResult NewPage()
		{
			long newId = _dynamicContentProvider.CreateCustomPage();

			return RedirectToAction(nameof(EditPage), new { id = newId });
		}

		[LoggedIn]
		[HttpGet]
		[Breadcrumb(nameof(Languages.LanguageStrings.DynamicContent))]
		[Authorize(Policy = PolicyNameContentEditor)]
		public IActionResult GetCustomPages()
		{
			return View("/Views/DynamicContent/CustomPages.cshtml", GetCustomPagesModel());
		}

		[LoggedIn]
		[HttpGet]
		[Authorize(Policy = PolicyNameContentEditor)]
		[Breadcrumb(nameof(Languages.LanguageStrings.Edit))]
		public IActionResult EditPage(long id)
		{
			IDynamicContentPage dynamicContentPage = _dynamicContentProvider.GetCustomPage(id);

			if (dynamicContentPage == null)
				return RedirectToAction(nameof(GetCustomPages));

			string cacheId = $"{GetUserSession().InternalSessionID}-{dynamicContentPage.Id}";

			_memoryCache.GetExtendingCache().Add(cacheId, new CacheItem(cacheId, dynamicContentPage));

			return View(GetEditPageModel(cacheId, dynamicContentPage));
		}

		[LoggedIn]
		[HttpPost]
		[Authorize(Policy = PolicyNameContentEditor)]
		[Route("DynamicContent/EditPage")]
		public IActionResult SavePage(EditPageModel model)
		{
			if (model == null)
				return RedirectToAction(nameof(GetCustomPages));

			string cacheId = model.CacheId;

			CacheItem cachedPage = _memoryCache.GetExtendingCache().Get(cacheId);

			if (cachedPage == null)
				return RedirectToAction(nameof(GetCustomPages));

			if (cachedPage.Value is not IDynamicContentPage dynamicContentPage)
				return RedirectToAction(nameof(GetCustomPages));

			if (ModelState.IsValid)
			{
				if (_dynamicContentProvider.PageNameExists(dynamicContentPage.Id, model.Name))
					ModelState.AddModelError(nameof(model.Name), Languages.LanguageStrings.NameAlreadyExists);

				if (ModelState.IsValid)
				{
					dynamicContentPage.ActiveFrom = model.ActiveFrom;
					dynamicContentPage.ActiveTo = model.ActiveTo;
					dynamicContentPage.Name = model.Name;
					dynamicContentPage.RouteName = model.RouteName;
					dynamicContentPage.BackgroundColor = model.BackgroundColor;
					dynamicContentPage.BackgroundImage = model.BackgroundImage;

					if (!_dynamicContentProvider.Save(dynamicContentPage))
						ModelState.AddModelError(String.Empty, Languages.LanguageStrings.FailedToSavePage);

					if (ModelState.IsValid)
					{
						_notificationService.RaiseEvent(NotificationEventDynamicContentUpdated);
						return RedirectToAction(nameof(GetCustomPages));
					}
				}

				return View(ViewEditPage, GetEditPageModel(cacheId, dynamicContentPage));
			}

			return View(ViewEditPage, GetEditPageModel(cacheId, dynamicContentPage));
		}

		[LoggedIn]
		[HttpGet]
		[AjaxOnly]
		[Route("DynamicContent/GetContent/{*cacheId}")]
		public IActionResult GetContent(string cacheId)
		{
			if (String.IsNullOrEmpty(cacheId))
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidCacheId);

			CacheItem cacheItem = _memoryCache.GetExtendingCache().Get(cacheId);

			if (cacheItem == null)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidCacheItem);

			return new JsonResult(GetDynamicContentModel(cacheItem))
			{
				StatusCode = 200,
				ContentType = "application/json"
			};
		}

		[LoggedIn]
		[HttpPost]
		[AjaxOnly]
		[Route("DynamicContent/UpdatePosition")]
		public IActionResult UpdateControlPosition(UpdatePositionModel model)
		{
			if (model == null)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidModelData);

			if (String.IsNullOrEmpty(model.CacheId))
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidCacheId);

			if (String.IsNullOrEmpty(model.ControlId))
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidControlId);

			CacheItem cacheItem = _memoryCache.GetExtendingCache().Get(model.CacheId);

			if (cacheItem == null)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidCacheItem);

			if (cacheItem.Value is not DynamicContentPage dynamicContentPage)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidDynamicPage);

			if (dynamicContentPage.Content.Find(ctl => ctl.UniqueId.Equals(model.ControlId)) == null)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidControl);

			RepositionControls(dynamicContentPage, model.Controls);

			return new JsonResult(GetDynamicContentModel(cacheItem))
			{
				StatusCode = 200,
				ContentType = "application/json"
			};
		}

		[LoggedIn]
		[HttpGet]
		[AjaxOnly]
		[Route("DynamicContent/TemplateEditor/{cacheId}/{controlId}")]
		public IActionResult TemplateEditor(string cacheId, string controlId)
		{
			if (String.IsNullOrEmpty(cacheId))
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidCacheId);

			if (String.IsNullOrEmpty(controlId))
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidControlId);

			CacheItem cacheItem = _memoryCache.GetExtendingCache().Get(cacheId);

			if (cacheItem == null)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidCacheItem);

			if (cacheItem.Value is not DynamicContentPage dynamicContentPage)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidDynamicPage);

			DynamicContentTemplate control = dynamicContentPage.Content.Find(ctl => ctl.UniqueId.Equals(controlId));

			if (control == null)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidContentPage);

			return PartialView("/Views/DynamicContent/_TemplateEditor.cshtml", CreateEditTemplateModel(cacheId, control));
		}

		[HttpPost]
		[LoggedIn]
		[AjaxOnly]
		public IActionResult UpdateTemplate(EditTemplateModel model)
		{
			if (model == null)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidModelData);

			if (String.IsNullOrEmpty(model.CacheId))
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidCacheId);

			if (String.IsNullOrEmpty(model.UniqueId))
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidUniqueId);

			CacheItem cacheItem = _memoryCache.GetExtendingCache().Get(model.CacheId);

			if (cacheItem == null)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidCacheItem);

			if (cacheItem.Value is not DynamicContentPage dynamicContentPage)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidContentPage);

			DynamicContentTemplate control = dynamicContentPage.Content.Find(ctl => ctl.UniqueId.Equals(model.UniqueId));

			if (control == null)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidControlId);

			if (!ValidateWidth(model, out string widthError))
				return GenerateJsonErrorResponse(200, widthError);

			if (!ValidateHeight(model, out string heightError))
				return GenerateJsonErrorResponse(200, heightError);

			control.Height = model.Height;
			control.HeightType = model.HeightType;
			control.Width = model.Width;
			control.WidthType = model.WidthType;
			control.Data = model.Data ?? String.Empty;
			control.CssStyle = model.CssStyle ?? String.Empty;
			control.CssClassName = model.CssClassName ?? String.Empty;

			return GenerateJsonSuccessResponse();
		}

		[LoggedIn]
		[HttpGet]
		[Route("DynamicContent/Preview/{cacheId}")]
		public IActionResult Preview(string cacheId)
		{
			if (String.IsNullOrEmpty(cacheId))
				return StatusCode(HtmlResponseBadRequest);

			CacheItem cacheItem = _memoryCache.GetExtendingCache().Get(cacheId);

			if (cacheItem == null)
				return StatusCode(HtmlResponseBadRequest);

			if (cacheItem.Value is not IDynamicContentPage dynamicContentPage)
				return StatusCode(HtmlResponseBadRequest);

			return View("/Views/DynamicContent/Index.cshtml", GetDynamicContentPageModel(dynamicContentPage, false));
		}

		[HttpGet]
		[LoggedIn]
		[AjaxOnly]
		[Route("DynamicContent/DeleteControl/{cacheId}/{controlId}")]
		public IActionResult DeleteControl(string cacheId, string controlId)
		{
			if (String.IsNullOrEmpty(cacheId))
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidCacheId);

			if (String.IsNullOrEmpty(controlId))
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidControlId);

			CacheItem cacheItem = _memoryCache.GetExtendingCache().Get(cacheId);

			if (cacheItem == null)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidCacheItem);

			if (cacheItem.Value is not DynamicContentPage dynamicContentPage)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidContentPage);

			DynamicContentTemplate control = dynamicContentPage.Content.Find(ctl => ctl.UniqueId.Equals(controlId));

			if (control == null)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidControl);

			return PartialView("/Views/DynamicContent/_DeleteControl.cshtml", CreateDeleteControlModel(cacheId, control));
		}

		[LoggedIn]
		[HttpPost]
		[AjaxOnly]
		public IActionResult DeleteItem(DeleteControlModel model)
		{
			if (model == null)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidModelData);

			if (String.IsNullOrEmpty(model.CacheId))
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidCacheId);

			if (String.IsNullOrEmpty(model.ControlId))
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidControlId);

			CacheItem cacheItem = _memoryCache.GetExtendingCache().Get(model.CacheId);

			if (cacheItem == null)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidCacheItem);

			if (cacheItem.Value is not DynamicContentPage dynamicContentPage)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidContentPage);

			DynamicContentTemplate control = dynamicContentPage.Content.Find(ctl => ctl.UniqueId.Equals(model.ControlId));

			if (control == null)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidControl);

			dynamicContentPage.Content.Remove(control);

			return GenerateJsonSuccessResponse();
		}

		[LoggedIn]
		[HttpPost]
		[AjaxOnly]
		public IActionResult GetTemplates()
		{
			TemplatesModel model = new();

			foreach (DynamicContentTemplate template in _dynamicContentProvider.Templates().OrderBy(t => t.TemplateSortOrder))
			{
				model.Templates.Add(new TemplateModel(template.UniqueId,
					template.Name,
					$"/images/dynamiccontent/templates/{HtmlHelper.RouteFriendlyName(template.Name)}.png"));
			}

			return PartialView("/Views/DynamicContent/_Templates.cshtml", model);
		}

		[HttpPost]
		[LoggedIn]
		[AjaxOnly]
		[Route("DynamicContent/AddTemplate/")]
		public IActionResult AddTemplateToPage(AddControlModel model)
		{
			if (model == null)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidModelData);

			if (String.IsNullOrEmpty(model.CacheId))
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidCacheId);

			if (String.IsNullOrEmpty(model.TemplateId))
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidTemplate);

			CacheItem cacheItem = _memoryCache.GetExtendingCache().Get(model.CacheId);

			if (cacheItem == null)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidCacheItem);

			if (cacheItem.Value is not DynamicContentPage dynamicContentPage)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidContentPage);

			DynamicContentTemplate template = _dynamicContentProvider.Templates().Find(t => t.UniqueId.Equals(model.TemplateId));

			if (template == null)
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, InvalidTemplateNotFound);

			dynamicContentPage.AddContentTemplate(template, model.NextControl);

			return GenerateJsonSuccessResponse();
		}

		#endregion Public Action Methods

		#region Private Methods

		private static bool ValidateHeight(EditTemplateModel model, out string error)
		{
			error = String.Empty;

			if (model.HeightType == DynamicContentHeightType.Percentage && (model.Height < 1 || model.Height > 100))
			{
				error = String.Format(Languages.LanguageStrings.InvalidHeightBetween, 1, 100);
			}
			else if (model.HeightType == DynamicContentHeightType.Pixels && model.Height < 1)
			{
				error = String.Format(Languages.LanguageStrings.InvalidHeightMinimum, 1);
			}

			return String.IsNullOrEmpty(error);
		}

		private static bool ValidateWidth(EditTemplateModel model, out string error)
		{
			error = String.Empty;

			if (model.WidthType == DynamicContentWidthType.Columns && (model.Width < 1 || model.Width > 12))
			{
				error = String.Format(Languages.LanguageStrings.InvalidWidthBetween, 1, 12);
			}
			else if (model.WidthType == DynamicContentWidthType.Percentage && (model.Width < 1 || model.Width > 100))
			{
				error = String.Format(Languages.LanguageStrings.InvalidWidthBetween, 1, 100);
			}
			else if (model.WidthType == DynamicContentWidthType.Pixels && model.Width < 1)
			{
				error = String.Format(Languages.LanguageStrings.InvalidWidthMinimum, 1);
			}

			return String.IsNullOrEmpty(error);
		}

		private static EditTemplateModel CreateEditTemplateModel(string cacheId, DynamicContentTemplate control)
		{
			return new EditTemplateModel()
			{
				CacheId = cacheId,
				EditorInstructions = control.EditorInstructions,
				TemplateEditor = control.EditorAction,
				UniqueId = control.UniqueId,
				Name = control.Name,
				SortOrder = control.SortOrder,
				WidthType = control.WidthType,
				Width = control.Width,
				HeightType = control.HeightType,
				Height = control.Height,
				Data = control.Data,
				ActiveFrom = control.ActiveFrom,
				ActiveTo = control.ActiveTo,
				CssClassName = control.CssClassName,
				CssStyle = control.CssStyle,
			};
		}

		private static DeleteControlModel CreateDeleteControlModel(string cacheId, DynamicContentTemplate control)
		{
			return new DeleteControlModel(cacheId, control.UniqueId);
		}

		private static void RepositionControls(DynamicContentPage dynamicContentPage, string[] controls)
		{
			int missingPosition = dynamicContentPage.Content.Count;

			foreach (DynamicContentTemplate contentTemplate in dynamicContentPage.Content)
			{
				int newPosition = Array.IndexOf(controls, contentTemplate.UniqueId);

				if (newPosition == -1)
				{
					contentTemplate.SortOrder = missingPosition++;
				}
				else
				{
					contentTemplate.SortOrder = newPosition;
				}
			}
		}

		private static JsonResponseModel GetDynamicContentModel(CacheItem cacheItem)
		{
			IDynamicContentPage dynamicContentPage = cacheItem.Value as IDynamicContentPage;

			StringBuilder content = new(4096);
			foreach (DynamicContentTemplate template in dynamicContentPage.Content.OrderBy(pc => pc.SortOrder))
			{
				content.AppendFormat(EditControlRow,
					template.UniqueId,
					template.Name,
					template.ColumnCount,
					Languages.LanguageStrings.Edit,
					template.EditorContent(),
					template.EditorAction);
			}

			if (content.Length == 0)
				return new JsonResponseModel(true);
			else
				return new JsonResponseModel(content.ToString());
		}

		private CustomPagesModel GetCustomPagesModel()
		{
			return new CustomPagesModel(GetModelData(), _dynamicContentProvider.GetCustomPageList());
		}

		private EditPageModel GetEditPageModel(string cacheId, IDynamicContentPage dynamicContentPage)
		{
			EditPageModel Result = new(GetModelData(), cacheId, dynamicContentPage.Id, dynamicContentPage.Name,
				dynamicContentPage.RouteName, dynamicContentPage.ActiveFrom, dynamicContentPage.ActiveTo, dynamicContentPage.Content,
				dynamicContentPage.BackgroundColor, dynamicContentPage.BackgroundImage);

			Result.Breadcrumbs.Add(new BreadcrumbItem("Dynamic Content", $"/{Name}/{nameof(GetCustomPages)}", false));
			Result.Breadcrumbs.Add(new BreadcrumbItem($"{Languages.LanguageStrings.Edit} {dynamicContentPage.Name}", Result.Breadcrumbs[1].Route, Result.Breadcrumbs[1].HasParameters));
			Result.Breadcrumbs.RemoveAt(1);

			return Result;
		}

		private PageModel GetDynamicContentPageModel(IDynamicContentPage dynamicContentPage, bool hasDataSaved)
		{
			IEnumerable<DynamicContentTemplate> templates = dynamicContentPage.Content.OrderBy(pc => pc.SortOrder);

			StringBuilder content = new(4096);
			List<string> inputControlIds = [];

			foreach (DynamicContentTemplate template in templates)
			{
				content.Append(template.Content());

				if (template.TemplateType == DynamicContentTemplateType.Input &&
					GetInputId(template, out string inputId) &&
					!inputControlIds.Contains(inputId))
				{
					inputControlIds.Add(inputId);
				}
			}

			string pageCss = "";

			if (!String.IsNullOrEmpty(dynamicContentPage.BackgroundColor) || !String.IsNullOrEmpty(dynamicContentPage.BackgroundImage))
			{
				pageCss = "<style>body {";

				if (!String.IsNullOrEmpty(dynamicContentPage.BackgroundColor))
					pageCss += $"background-color: {dynamicContentPage.BackgroundColor}";

				if (!String.IsNullOrEmpty(dynamicContentPage.BackgroundImage))
					pageCss += $"background-image: url('{dynamicContentPage.BackgroundImage}');background-size: 100% 100%;";

				pageCss += "}</style>";
			}

			return new PageModel(GetModelData(), dynamicContentPage.Name, content.ToString(), pageCss, [.. inputControlIds], hasDataSaved);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool GetInputId(DynamicContentTemplate template, out string value)
		{
			FormTemplateEditorModel formModel = new(template.Data);

			value = formModel.ControlName;

			return !String.IsNullOrEmpty(value);
		}

		#endregion Private Methods
	}
}

#pragma warning restore CS1591