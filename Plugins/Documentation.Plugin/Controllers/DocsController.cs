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
using System.Runtime.CompilerServices;

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
                    model.AssemblyNames.Add(doc.Title, new DocumentationModule(doc.Title, doc.ShortDescription));
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

        [HttpGet]
        [Route("docs/Document/{className}/Type/{classType}/{typeName}")]
        public IActionResult ViewType(string className, string classType, string typeName)
        {
            DocumentViewTypeViewModel model = BuildDocumentViewTypeModel(className, classType, typeName, out Document selected);

            if (model == null)
                return RedirectToAction(nameof(Index));

            return View(model);
        }


        #endregion Public Action Methods

        #region Private Methods

        private DocumentViewTypeViewModel BuildDocumentViewTypeModel(string className, string classType, string typeName, out Document selected)
        {
            selected = _documentationService.GetDocuments()
                .Where(d => d.DocumentType == DocumentType.Class)
                .ToList().Where(c => HtmlHelper.RouteFriendlyName(c.ClassName) == className)
                .FirstOrDefault();

            if (selected == null)
                return null;

            DocumentData data = (DocumentData)selected.Tag;

            if (data.ReferenceData == null)
            {
                List<Document> documents = _documentationService.GetDocuments()
                    .Where(d => d.DocumentType == DocumentType.Assembly ||
                        d.DocumentType == DocumentType.Custom)
                    .OrderBy(o => o.SortOrder).ThenBy(o => o.Title)
                    .ToList();

                data.ReferenceData = GetAllReferences(selected, data, documents);
            }

            DocumentViewTypeViewModel model = new DocumentViewTypeViewModel(GetBreadcrumbs(), GetCartSummary(),
                selected.Title, data.ReferenceData);

            model.Assembly = selected.AssemblyName;
            model.Namespace = selected.NameSpaceName;
            model.ClassName = selected.ClassName;

            if (data.SeeAlso != null && data.SeeAlso.Count > 0)
            {
                model.SeeAlso = data.SeeAlso;
            }

            if (data.SeeAlso != null && data.SeeAlso.Count > 0)
                model.SeeAlso = data.SeeAlso;

            model.TranslateStrings = selected.DocumentType != DocumentType.Custom &&
                selected.DocumentType != DocumentType.Document &&
                selected.DocumentType != DocumentType.Assembly;

            switch (classType)
            {
                case "Constructor":
                    return BuildConstructorViewModel(model, selected, data, typeName);

                case "Method":
                    return BuildMethodViewModel(model, selected, data, typeName);

                case "Property":
                    return BuildPropertyViewModel(model, selected, data, typeName);

                case "Field":
                    return BuildFieldViewModel(model, selected, data, typeName);

                default:
                    throw new InvalidOperationException(SharedPluginFeatures.Constants.InvalidTypeName);
            }
        }

        private DocumentViewTypeViewModel BuildConstructorViewModel(DocumentViewTypeViewModel model, Document selected, DocumentData data, string name)
        {
            DocumentMethod constructor = selected.Constructors.Where(f => HtmlHelper.RouteFriendlyName(f.MethodName) == name).FirstOrDefault();

            if (constructor == null)
                return null;

            model.TypeName = $"{ReturnUptoParam(constructor.MethodName)} Constructor";
            model.Returns = constructor.Returns;
            model.ShortDescription = constructor.ShortDescription;
            model.LongDescription = constructor.LongDescription;
            model.Summary = constructor.Summary;
            model.ExampleUseage = constructor.ExampleUseage;
            model.Parameters = constructor.Parameters;

            return model;
        }

        private DocumentViewTypeViewModel BuildMethodViewModel(DocumentViewTypeViewModel model, Document selected, DocumentData data, string name)
        {
            DocumentMethod method = selected.Methods.Where(f => HtmlHelper.RouteFriendlyName(f.MethodName) == name).FirstOrDefault();

            if (method == null)
                return null;

            model.TypeName = $"{ReturnUptoParam(method.MethodName)} Method";
            model.Returns = method.Returns;
            model.ShortDescription = method.ShortDescription;
            model.LongDescription = method.LongDescription;
            model.Summary = method.Summary;
            model.ExampleUseage = method.ExampleUseage;
            model.Parameters = method.Parameters;

            return model;
        }

        private DocumentViewTypeViewModel BuildPropertyViewModel(DocumentViewTypeViewModel model, Document selected, DocumentData data, string name)
        {
            DocumentProperty property = selected.Properties.Where(f => HtmlHelper.RouteFriendlyName(f.PropertyName) == name).FirstOrDefault();

            if (property == null)
                return null;

            model.TypeName = $"{property.PropertyName} Property";
            model.Value = property.Value;
            model.ShortDescription = property.ShortDescription;
            model.LongDescription = property.LongDescription;
            model.Summary = property.Summary;

            return model;
        }

        private DocumentViewTypeViewModel BuildFieldViewModel(DocumentViewTypeViewModel model, Document selected, DocumentData data, string name)
        {
            DocumentField field = selected.Fields.Where(f => HtmlHelper.RouteFriendlyName(f.FieldName) == name).FirstOrDefault();

            if (field == null)
                return null;

            model.TypeName = field.FieldName;
            model.Value = field.Value;
            model.ShortDescription = field.ShortDescription;
            model.LongDescription = field.LongDescription;
            model.Summary = field.Summary;

            return model;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string ReturnUptoParam(string s)
        {
            int bracket = s.IndexOf("(");

            if (bracket == -1)
                return s;
            else
                return s.Substring(0, bracket);
        }

        private DocumentViewModel BuildDocumentViewModel(string documentName, string className, out Document selected)
        {
            List<Document> documents = _documentationService.GetDocuments()
                .Where(d => d.DocumentType == DocumentType.Assembly || 
                    d.DocumentType == DocumentType.Custom)
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
                data.ReferenceData = GetAllReferences(selected, data, documents);
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

        private string GetAllReferences(Document document, DocumentData data, List<Document> documents)
        {
            StringBuilder allReferences = new StringBuilder("<ul>", 2048);

            foreach (Document doc in documents)
            {
                allReferences.Append($"<li><a href=\"/docs/Document/{HtmlHelper.RouteFriendlyName(doc.Title)}/\">{doc.Title}</a>");

                if (doc.Tag == data && (document.DocumentType == DocumentType.Assembly || document.DocumentType == DocumentType.Custom))
                {
                    allReferences.Append(((DocumentData)document.Tag).AllReferences);
                }
                else if (document.DocumentType == DocumentType.Class && data.Parent != null && data.Parent == doc.Tag)
                {
                    if (data.Parent == null)
                        allReferences.Append(data.AllReferences);
                    else
                        allReferences.Append(data.Parent.AllReferences);
                }

                allReferences.Append("</li>");
            }

            allReferences.Append("</ul>");

            return allReferences.ToString();
        }

        #endregion Private Methods
    }
}