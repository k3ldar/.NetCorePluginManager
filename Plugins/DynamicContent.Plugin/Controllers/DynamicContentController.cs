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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
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
using System.Text;

using DynamicContent.Plugin.Model;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Middleware;
using Middleware.DynamicContent;

using Shared.Classes;

using SharedPluginFeatures;
using SharedPluginFeatures.DynamicContent;

namespace DynamicContent.Plugin.Controllers
{
    /// <summary>
    /// Dynamic content controller allows for editing of dynamic content on a website.
    /// </summary>
    [DenySpider]
    public partial class DynamicContentController : BaseController
    {
        public const string Name = "DynamicContent";

        #region Private Members

        private const string ControlRow = "<li id=\"{0}\" class=\"col-edit-{2} ui-state-default editControl\">" +
                "<p class=\"ctlHeader\">{1}<span class=\"deleteBtn\" id=\"{0}\">X</span><span class=\"editBtn\" id=\"{0}\" data-cs=\"{5}\">{3}</span></p><div class=\"ctlContent\">{4}</div></li>";

        private readonly IDynamicContentProvider _dynamicContentProvider;
        private readonly IMemoryCache _memoryCache;
        //private readonly IDynamicContentServices _dynamicContentServices;

        #endregion Private Members

        #region Constructors

        public DynamicContentController(IDynamicContentProvider dynamicContentProvider,
            IMemoryCache memoryCache
            //ISettingsProvider settingsProvider,
            //IPluginHelperService pluginHelper
            )
        {
            //if (settingsProvider == null)
            //    throw new ArgumentNullException(nameof(settingsProvider));

            //DynamicContentControllerSettings settings = settingsProvider
            //    .GetSettings<DynamicContentControllerSettings>(nameof(DynamicContentControllerSettings));
            _dynamicContentProvider = dynamicContentProvider ?? throw new ArgumentNullException(nameof(dynamicContentProvider));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            //_dynamicContentServices = dynamicContentServices ?? throw new ArgumentNullException(nameof(dynamicContentServices));
        }

        #endregion Constructors

        #region Public Action Methods

        public static List<LookupListItem> GetHeightTypes()
        {
            return new List<LookupListItem>()
            {
                new LookupListItem(1, DynamicContentHeightType.Automatic.ToString()),
                new LookupListItem(3, DynamicContentHeightType.Percentage.ToString()),
                new LookupListItem(4, DynamicContentHeightType.Pixels.ToString()),
            };
        }

        public static List<LookupListItem> GetWidthTypes()
        {
            return new List<LookupListItem>()
            {
                new LookupListItem(1, DynamicContentWidthType.Columns.ToString()),
                new LookupListItem(2, DynamicContentWidthType.Percentage.ToString()),
                new LookupListItem(3, DynamicContentWidthType.Pixels.ToString()),
            };
        }

        //[HttpGet]
        //[Route("Page/{*path}")]
        //public IActionResult Index(string path)
        //{
        //    return View(new BaseModel(GetModelData()));
        //}

        [LoggedIn]
        [HttpGet]
        [Breadcrumb(nameof(Languages.LanguageStrings.DynamicContent))]
        [Authorize(Policy = SharedPluginFeatures.Constants.PolicyNameContentEditor)]
        public IActionResult GetCustomPages()
        {
            return View("/Views/DynamicContent/CustomPages.cshtml", GetCustomPagesModel());
        }

        [LoggedIn]
        [HttpGet]
        [Authorize(Policy = SharedPluginFeatures.Constants.PolicyNameContentEditor)]
        [Breadcrumb(nameof(Languages.LanguageStrings.Edit))]
        public IActionResult EditPage(int id)
        {
            IDynamicContentPage pageContent = _dynamicContentProvider.GetCustomPage(id);

            if (pageContent == null)
                return RedirectToAction(nameof(GetCustomPages));

            string cacheId = $"{GetUserSession().InternalSessionID}-{pageContent.Id}";

            _memoryCache.GetExtendingCache().Add(cacheId, new CacheItem(cacheId, pageContent));

            return View(GetEditPageModel(cacheId, pageContent));
        }

        [LoggedIn]
        [HttpGet]
        [AjaxOnly]
        [Route("DynamicContent/GetContent/{*cacheId}")]
        public IActionResult GetContent(string cacheId)
        {
            if (String.IsNullOrEmpty(cacheId))
                return GenerateErrorResponse(400);

            CacheItem cacheItem = _memoryCache.GetExtendingCache().Get(cacheId);

            if (cacheItem == null)
                return GenerateErrorResponse(400);

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
                return GenerateErrorResponse(400);

            if (String.IsNullOrEmpty(model.CacheId))
                return GenerateErrorResponse(400);

            if (String.IsNullOrEmpty(model.ControlId))
                return GenerateErrorResponse(400);

            CacheItem cacheItem = _memoryCache.GetExtendingCache().Get(model.CacheId);

            if (cacheItem == null)
                return GenerateErrorResponse(400);

            DynamicContentPage dynamicContentPage = cacheItem.Value as DynamicContentPage;

            if (dynamicContentPage == null)
                return GenerateErrorResponse(400);

            if (dynamicContentPage.Content.Where(ctl => ctl.UniqueId.Equals(model.ControlId)).FirstOrDefault() == null)
                return GenerateErrorResponse(400);

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
                return GenerateErrorResponse(400);

            if (String.IsNullOrEmpty(controlId))
                return GenerateErrorResponse(400);

            CacheItem cacheItem = _memoryCache.GetExtendingCache().Get(cacheId);

            if (cacheItem == null)
                return GenerateErrorResponse(400);

            DynamicContentPage dynamicContentPage = cacheItem.Value as DynamicContentPage;

            if (dynamicContentPage == null)
                return GenerateErrorResponse(400);

            DynamicContentTemplate control = dynamicContentPage.Content.Where(ctl => ctl.UniqueId.Equals(controlId)).FirstOrDefault();

            if (control == null)
                return GenerateErrorResponse(400);

            return PartialView("/Views/DynamicContent/_TemplateEditor.cshtml", CreateEditTemplateModel(cacheId, control));
        }

        [HttpPost]
        [LoggedIn]
        [AjaxOnly]
        public IActionResult UpdateTemplate(EditTemplateModel model)
        {
            if (model == null)
                return GenerateErrorResponse(400);

            if (String.IsNullOrEmpty(model.CacheId))
                return GenerateErrorResponse(400);

            if (String.IsNullOrEmpty(model.UniqueId))
                return GenerateErrorResponse(400);

            CacheItem cacheItem = _memoryCache.GetExtendingCache().Get(model.CacheId);

            if (cacheItem == null)
                return GenerateErrorResponse(400);

            DynamicContentPage dynamicContentPage = cacheItem.Value as DynamicContentPage;

            if (dynamicContentPage == null)
                return GenerateErrorResponse(400);

            DynamicContentTemplate control = dynamicContentPage.Content.Where(ctl => ctl.UniqueId.Equals(model.UniqueId)).FirstOrDefault();

            if (control == null)
                return GenerateErrorResponse(400);

            if (!ValidateWidth(model, out string widthError))
                return GenerateErrorResponse(200, widthError);

            if (!ValidateHeight(model, out string heightError))
                return GenerateErrorResponse(200, heightError);

            control.Height = model.Height;
            control.HeightType = model.HeightType;
            control.Width = model.Width;
            control.WidthType = model.WidthType;
            control.Data = model.Data;

            return GenerateSuccessResponse();
        }

        [LoggedIn]
        [HttpGet]
        [Route("DynamicContent/Preview/{cacheId}")]
        public IActionResult Preview(string cacheId)
        {
            if (String.IsNullOrEmpty(cacheId))
                return StatusCode(400);


            CacheItem cacheItem = _memoryCache.GetExtendingCache().Get(cacheId);

            if (cacheItem == null)
                return StatusCode(400);

            IDynamicContentPage dynamicContentPage = cacheItem.Value as IDynamicContentPage;

            if (dynamicContentPage == null)
                return StatusCode(400);

            return View("/Views/DynamicContent/Index.cshtml", GetDynamicContentPageModel(dynamicContentPage, true));
        }

        [HttpGet]
        [LoggedIn]
        [AjaxOnly]
        [Route("DynamicContent/DeleteControl/{cacheId}/{controlId}")]
        public IActionResult DeleteControl(string cacheId, string controlId)
        {
            if (String.IsNullOrEmpty(cacheId))
                return GenerateErrorResponse(400);

            if (String.IsNullOrEmpty(controlId))
                return GenerateErrorResponse(400);

            CacheItem cacheItem = _memoryCache.GetExtendingCache().Get(cacheId);

            if (cacheItem == null)
                return GenerateErrorResponse(400);

            DynamicContentPage dynamicContentPage = cacheItem.Value as DynamicContentPage;

            if (dynamicContentPage == null)
                return GenerateErrorResponse(400);

            DynamicContentTemplate control = dynamicContentPage.Content.Where(ctl => ctl.UniqueId.Equals(controlId)).FirstOrDefault();

            if (control == null)
                return GenerateErrorResponse(400);

            return PartialView("/Views/DynamicContent/_DeleteControl.cshtml", CreateDeleteControlModel(cacheId, control));
        }

        [LoggedIn]
        [HttpPost]
        [AjaxOnly]
        public IActionResult DeleteItem(DeleteControlModel model)
        {
            if (model == null)
                return GenerateErrorResponse(400);

            if (String.IsNullOrEmpty(model.CacheId))
                return GenerateErrorResponse(400);

            if (String.IsNullOrEmpty(model.ControlId))
                return GenerateErrorResponse(400);

            CacheItem cacheItem = _memoryCache.GetExtendingCache().Get(model.CacheId);

            if (cacheItem == null)
                return GenerateErrorResponse(400);

            DynamicContentPage dynamicContentPage = cacheItem.Value as DynamicContentPage;

            if (dynamicContentPage == null)
                return GenerateErrorResponse(400);

            DynamicContentTemplate control = dynamicContentPage.Content.Where(ctl => ctl.UniqueId.Equals(model.ControlId)).FirstOrDefault();

            if (control == null)
                return GenerateErrorResponse(400);

            dynamicContentPage.Content.Remove(control);

            return GenerateSuccessResponse();
        }

        [LoggedIn]
        [HttpPost]
        [AjaxOnly]
        public IActionResult GetTemplates()
        {
            TemplatesModel model = new TemplatesModel();

            foreach (DynamicContentTemplate template in _dynamicContentProvider.Templates())
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
        [Route("DynamicContent/AddTemplate/{cacheId}/{templateId}")]
        public IActionResult AddTemplateToPage(string cacheId, string templateId)
        {
            if (String.IsNullOrEmpty(cacheId))
                return GenerateErrorResponse(400);

            if (String.IsNullOrEmpty(templateId))
                return GenerateErrorResponse(400);

            CacheItem cacheItem = _memoryCache.GetExtendingCache().Get(cacheId);

            if (cacheItem == null)
                return GenerateErrorResponse(400);

            DynamicContentPage dynamicContentPage = cacheItem.Value as DynamicContentPage;

            if (dynamicContentPage == null)
                return GenerateErrorResponse(400);

            DynamicContentTemplate control = dynamicContentPage.Content.Where(ctl => ctl.UniqueId.Equals(templateId)).FirstOrDefault();

            if (control == null)
                return GenerateErrorResponse(400);

            return PartialView("/Views/DynamicContent/_DeleteControl.cshtml", CreateDeleteControlModel(cacheId, control));
        }

        #endregion Public Action Methods

        #region Private Methods

        private bool ValidateHeight(EditTemplateModel model, out string error)
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

        private bool ValidateWidth(EditTemplateModel model, out string error)
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

        private EditTemplateModel CreateEditTemplateModel(string cacheId, DynamicContentTemplate control)
        {
            return new EditTemplateModel()
            {
                CacheId = cacheId,
                TemplateEditor = control.EditorAction,
                UniqueId = control.UniqueId,
                Name = control.Name,
                SortOrder = control.SortOrder,
                WidthType = control.WidthType,
                Width = control.Width,
                HeightType = control.HeightType,
                Height = control.Height,
                Data = control.Data,
                ActiveFrom = control.ActiveFrom
            };
        }

        private DeleteControlModel CreateDeleteControlModel(string cacheId, DynamicContentTemplate control)
        {
            return new DeleteControlModel(cacheId, control.UniqueId);
        }

        private void RepositionControls(DynamicContentPage dynamicContentPage, string[] controls)
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

        private DynamicContentModel GetDynamicContentModel(CacheItem cacheItem)
        {
            IDynamicContentPage pageContent = cacheItem.Value as IDynamicContentPage;

            StringBuilder content = new StringBuilder(4096);
            foreach (DynamicContentTemplate template in pageContent.Content.OrderBy(pc => pc.SortOrder))
            {
                content.AppendFormat(ControlRow,
                    template.UniqueId,
                    template.Name,
                    template.ColumnCount,
                    Languages.LanguageStrings.Edit,
                    template.Data,
                    template.EditorAction);
            }

            return new DynamicContentModel(content.ToString());
        }

        private JsonResult GenerateErrorResponse(int statusCode)
        {
            return new JsonResult(new DynamicContentModel())
            {
                ContentType = "application/json",
                StatusCode = statusCode
            };
        }

        private JsonResult GenerateErrorResponse(int statusCode, string message)
        {
            return new JsonResult(new DynamicContentModel() { Data = message })
            {
                ContentType = "application/json",
                StatusCode = statusCode
            };
        }

        private JsonResult GenerateSuccessResponse()
        {
            return new JsonResult(new DynamicContentModel(true))
            {
                ContentType = "application/json",
                StatusCode = 200
            };
        }

        private CustomPagesModel GetCustomPagesModel()
        {
            return new CustomPagesModel(GetModelData(), _dynamicContentProvider.GetActiveCustomPages());
        }

        private EditPageModel GetEditPageModel(string cacheId, IDynamicContentPage dynamicContentPage)
        {
            EditPageModel Result = new EditPageModel(GetModelData(), cacheId, dynamicContentPage.Id, dynamicContentPage.Name, dynamicContentPage.Content);

            Result.Breadcrumbs.Add(new BreadcrumbItem("Dynamic Content", $"/{Name}/{nameof(GetCustomPages)}", false));
            Result.Breadcrumbs.Add(new BreadcrumbItem($"{Languages.LanguageStrings.Edit} {dynamicContentPage.Name}", Result.Breadcrumbs[1].Route, Result.Breadcrumbs[1].HasParameters));
            Result.Breadcrumbs.RemoveAt(1);

            return Result;
        }

        private PageModel GetDynamicContentPageModel(IDynamicContentPage pageContent, bool ignoreDates)
        {
            IEnumerable<DynamicContentTemplate> templates = ignoreDates ?
                pageContent.Content.OrderBy(pc => pc.SortOrder) :
                pageContent.Content.Where(c => c.ActiveFrom >= DateTime.Now).OrderBy(o => o.SortOrder);

            StringBuilder content = new StringBuilder(4096);

            foreach (DynamicContentTemplate template in templates)
            {
                content.Append(template.Content());
            }

            return new PageModel(GetModelData(), content.ToString());
        }

        #endregion Private Methods
    }
}
