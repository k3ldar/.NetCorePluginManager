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
 *  Product:  Documentation Plugin
 *  
 *  File: DocumentationController.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  19/05/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using Shared.Classes;
using Shared.Docs;

using static Shared.Utilities;
using Shared;

using SharedPluginFeatures;

using Middleware;
using static Middleware.Constants;

using DocumentationPlugin.Models;

namespace DocumentationPlugin.Controllers
{
    public class DocsController : BaseController
    {
        #region Constants

        public const string Name = "docs";

        #endregion Constants

        #region Private Members

        private readonly IDocumentationService _documentationService;

        #endregion Private Members

        #region Constructors

        public DocsController(IDocumentationService documentationService,
            ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _documentationService = documentationService ?? throw new ArgumentNullException(nameof(documentationService));
        }

        #endregion Constructors

        #region Public Action Methods

        [HttpGet]
        [Breadcrumb(nameof(Languages.LanguageStrings.Documentation))]
        public IActionResult Index()
        {
            IndexViewModel model = new IndexViewModel(GetBreadcrumbs(), GetCartSummary());

            List<Document> documents = _documentationService.GetDocuments()
                .Where(d => d.DocumentType == DocumentType.Assembly || d.DocumentType == DocumentType.Custom)
                .OrderBy(o => o.SortOrder).ThenBy(o => o.Title)
                .ToList();

            foreach (Document doc in documents)
            {
                if (!model.AssemblyNames.ContainsKey(doc.Title))
                    model.AssemblyNames.Add(doc.Title, doc.ShortDescription);
            }

            return View(model);
        }

        [HttpGet]
        [Breadcrumb(nameof(Document), Name, nameof(Index))]
        [Route("docs/Document/{documentName}/")]
        public IActionResult Document(string documentName)
        {
            Document document = _documentationService.GetDocuments()
                .Where(d => HtmlHelper.RouteFriendlyName(d.Title) == documentName)
                .FirstOrDefault();

            if (document == null)
                return RedirectToAction(nameof(Index));

            DocumentViewModel model = new DocumentViewModel(GetBreadcrumbs(), GetCartSummary(),
                document.Title, document.ShortDescription, document.LongDescription);

            if (model.Breadcrumbs.Count > 0)
            {
                BreadcrumbItem lastItem = model.Breadcrumbs[model.Breadcrumbs.Count - 1];
                BreadcrumbItem breadcrumb = new BreadcrumbItem(document.Title, lastItem.Route, lastItem.HasParameters);
                model.Breadcrumbs.Remove(lastItem);
                model.Breadcrumbs.Add(breadcrumb);
            }

            return View(model);
        }

        #endregion Public Action Methods

        #region Private Methods

        #endregion Private Methods
    }
}