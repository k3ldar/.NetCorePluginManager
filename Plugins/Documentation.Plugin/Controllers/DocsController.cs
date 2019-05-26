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
using System.Text;

using Microsoft.AspNetCore.Mvc;

using Shared.Classes;
using Shared.Docs;

using static Shared.Utilities;
using Shared;

using SharedPluginFeatures;

using Middleware;
using static Middleware.Constants;

using DocumentationPlugin.Classes;
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
            DocumentViewModel model = BuildDocumentViewModel(documentName, null, out Document selected);

            if (model == null)
                return RedirectToAction(nameof(Index));

            if (model.Breadcrumbs.Count > 0)
            {
                BreadcrumbItem lastItem = model.Breadcrumbs[model.Breadcrumbs.Count - 1];
                BreadcrumbItem breadcrumb = new BreadcrumbItem(selected.Title, lastItem.Route, lastItem.HasParameters);
                model.Breadcrumbs.Remove(lastItem);
                model.Breadcrumbs.Add(breadcrumb);
            }

            return View(model);
        }

        [HttpGet]
        [Route("docs/Document/{documentName}/{className}/")]
        public IActionResult Type(string documentName, string className)
        {
            DocumentViewModel model = BuildDocumentViewModel(documentName, className, out Document selected);

            if (model == null)
                return RedirectToAction(nameof(Index));

            if (model.Breadcrumbs.Count > 0)
            {
                BreadcrumbItem lastItem = model.Breadcrumbs[model.Breadcrumbs.Count - 1];
                BreadcrumbItem breadcrumb = new BreadcrumbItem(selected.Title, lastItem.Route, lastItem.HasParameters);
                model.Breadcrumbs.Remove(lastItem);
                model.Breadcrumbs.Add(breadcrumb);
            }

            return View(model);
        }

        #endregion Public Action Methods

        #region Private Methods

        private DocumentViewModel BuildDocumentViewModel(string documentName, string className, out Document selected)
        {
            List<Document> documents = _documentationService.GetDocuments()
                .Where(d => d.DocumentType == DocumentType.Assembly || 
                    d.DocumentType == DocumentType.Custom /*|| 
                    d.DocumentType == DocumentType.Class*/)
                .OrderBy(o => o.SortOrder).ThenBy(o => o.Title)
                .ToList();

            if (String.IsNullOrEmpty(className))
                selected = documents.Where(d => HtmlHelper.RouteFriendlyName(d.Title) == documentName).FirstOrDefault();
            else
                selected = _documentationService.GetDocuments().Where(d => HtmlHelper.RouteFriendlyName(d.AssemblyName) == documentName && 
                    HtmlHelper.RouteFriendlyName(d.Title) == className).FirstOrDefault();

            if (selected == null)
                return null;

            DocumentData data = (DocumentData)selected.Tag;

            if (data.ReferenceData == null)
            {
                StringBuilder allReferences = new StringBuilder("<ul>", 2048);

                foreach (Document doc in documents)
                {
                    allReferences.Append($"<li><a href=\"/docs/Document/{HtmlHelper.RouteFriendlyName(doc.Title)}/\">{doc.Title}</a>");

                    if (doc.Tag == data && (selected.DocumentType == DocumentType.Assembly || selected.DocumentType == DocumentType.Custom))
                    {
                        allReferences.Append(((DocumentData)selected.Tag).AllReferences);
                    }
                    else if (selected.DocumentType == DocumentType.Class && data.Parent != null && data.Parent == doc.Tag)
                    {
                        if (data.Parent == null)
                            allReferences.Append(data.AllReferences);
                        else
                            allReferences.Append(data.Parent.AllReferences);
                    }

                    allReferences.Append("</li>");
                }

                allReferences.Append("</ul>");

                data.ReferenceData = allReferences.ToString();
            }

            DocumentViewModel model = new DocumentViewModel(GetBreadcrumbs(), GetCartSummary(),
                selected.Title, selected.ShortDescription, selected.LongDescription, data.ReferenceData);

            if (selected.DocumentType != DocumentType.Custom && selected.DocumentType != DocumentType.Document)
            {
                model.Assembly = selected.AssemblyName;
                model.Namespace = selected.NameSpaceName;
            }

            if (data.SeeAlso != null && data.SeeAlso.Count > 0)
            {
                model.SeeAlso = data.SeeAlso;
            }

            if (data.Contains != null && data.Contains.Count > 0)
                model.Contains = data.Contains;

            if (selected.Constructors != null && selected.Constructors.Count > 0)
                model.Constructors = selected.Constructors;

            if (selected.Fields != null && selected.Fields.Count > 0)
                model.Fields = selected.Fields;

            if (selected.Methods != null && selected.Methods.Count > 0)
                model.Methods = selected.Methods;

            if (selected.Properties != null && selected.Properties.Count > 0)
                model.Properties = selected.Properties;

            model.TranslateStrings = selected.DocumentType != DocumentType.Custom &&
                selected.DocumentType != DocumentType.Document &&
                selected.DocumentType != DocumentType.Assembly;

            model.PreviousDocument = data.PreviousDocument == null ? String.Empty : data.PreviousDocument.Title;
            model.NextDocument = data.NextDocument == null ? String.Empty : data.NextDocument.Title;
            model.Example = selected.Example;

            return model;
        }

        #endregion Private Methods
    }
}