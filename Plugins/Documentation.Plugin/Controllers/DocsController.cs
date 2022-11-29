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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
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
using System.Runtime.CompilerServices;
using System.Text;

using DocumentationPlugin.Classes;
using DocumentationPlugin.Models;

using Microsoft.AspNetCore.Mvc;

using PluginManager;
using PluginManager.Abstractions;

using Shared;
using Shared.Docs;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace DocumentationPlugin.Controllers
{
    [Subdomain(DocsController.Name)]
    public class DocsController : BaseController
    {
        #region Constants

        public const string Name = "docs";

        #endregion Constants

        #region Private Members

        private readonly IDocumentationService _documentationService;
        private readonly ILogger _logger;

        #endregion Private Members

        #region Constructors

        public DocsController(IDocumentationService documentationService, ILogger logger,
            ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _documentationService = documentationService ?? throw new ArgumentNullException(nameof(documentationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Constructors

        #region Public Action Methods

        [HttpGet]
        [Breadcrumb(nameof(Languages.LanguageStrings.Documentation))]
        public IActionResult Index()
        {
            IndexViewModel model = new IndexViewModel(GetModelData(),
                _documentationService.GetCustomData("Header", Languages.LanguageStrings.APIReference),
                _documentationService.GetCustomData("Description", Languages.LanguageStrings.InThisDocument));

            List<Document> documents = _documentationService.GetDocuments()
                .Where(d => d.DocumentType == DocumentType.Assembly || d.DocumentType == DocumentType.Custom)
                .OrderBy(o => o.SortOrder).ThenBy(o => o.Title)
                .ToList();

            foreach (Document doc in documents)
            {
                if (!model.AssemblyNames.ContainsKey(doc.Title))
                {
                    if (String.IsNullOrEmpty(doc.ShortDescription))
                        _logger.AddToLog(LogLevel.Information, $"No short description for document {doc.Title}");

                    model.AssemblyNames.Add(doc.Title, new DocumentationModule(doc.Title, doc.ShortDescription));
                }
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

            model.ShowShortDescription = false;

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

            model.ShowShortDescription = true;

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "I deem it to be valid in this context!")]
        private DocumentViewTypeViewModel BuildDocumentViewTypeModel(string className, string classType, string typeName, out Document selected)
        {
            selected = _documentationService.GetDocuments()
                .Where(d => d.DocumentType == DocumentType.Class)
                .ToList().FirstOrDefault(c => HtmlHelper.RouteFriendlyName(c.ClassName) == className);

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

            DocumentViewTypeViewModel model = new DocumentViewTypeViewModel(GetModelData(),
                selected.Title, data.ReferenceData);

            model.Assembly = HtmlHelper.RouteFriendlyName(selected.AssemblyName);
            model.Namespace = HtmlHelper.RouteFriendlyName(selected.NameSpaceName);
            model.ClassName = HtmlHelper.RouteFriendlyName(selected.ClassName);

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
                    return BuildConstructorViewModel(model, selected, typeName);

                case "Method":
                    return BuildMethodViewModel(model, selected, typeName);

                case "Property":
                    return BuildPropertyViewModel(model, selected, typeName);

                case "Field":
                    return BuildFieldViewModel(model, selected, typeName);

                default:
                    throw new InvalidOperationException(SharedPluginFeatures.Constants.InvalidTypeName);
            }
        }

        private DocumentViewTypeViewModel BuildConstructorViewModel(DocumentViewTypeViewModel model, Document selected, string name)
        {
            DocumentMethod constructor = selected.Constructors.FirstOrDefault(f => HtmlHelper.RouteFriendlyName(f.MethodName) == name);

            if (constructor == null)
                return null;

            model.TypeName = $"{ReturnUptoParam(constructor.MethodName)} Constructor";
            model.Returns = constructor.Returns;
            model.ShortDescription = constructor.ShortDescription;
            model.LongDescription = constructor.LongDescription;
            model.Summary = constructor.Summary;
            model.ExampleUseage = constructor.ExampleUseage;
            model.Parameters = constructor.Parameters;

            foreach (DocumentExample item in constructor.Examples)
            {
                model.ExampleUseage += $"<p>{item.Text}</p>";
            }

            foreach (DocumentException exception in constructor.Exception)
            {
                model.Exceptions += $"<p>{exception.ExceptionName}<br />{exception.Summary}";
            }

            model.Remarks = constructor.Remarks;

            return model;
        }

        private DocumentViewTypeViewModel BuildMethodViewModel(DocumentViewTypeViewModel model, Document selected, string name)
        {
            DocumentMethod method = selected.Methods.FirstOrDefault(f => HtmlHelper.RouteFriendlyName(f.MethodName) == name);

            if (method == null)
                return null;

            model.TypeName = $"{ReturnUptoParam(method.MethodName)} Method";
            model.Returns = method.Returns;
            model.ShortDescription = method.ShortDescription;
            model.LongDescription = method.LongDescription;
            model.Summary = method.Summary;
            model.ExampleUseage = method.ExampleUseage ?? String.Empty;
            model.Parameters = method.Parameters;

            foreach (DocumentExample item in method.Examples)
            {
                model.ExampleUseage += $"<p>{item.Text}</p>";
            }

            foreach (DocumentException exception in method.Exception)
            {
                model.Exceptions += $"<p>{exception.ExceptionName}<br />{exception.Summary}";
            }

            model.Remarks = method.Remarks;

            return model;
        }

        private DocumentViewTypeViewModel BuildPropertyViewModel(DocumentViewTypeViewModel model, Document selected, string name)
        {
            DocumentProperty property = selected.Properties.FirstOrDefault(f => HtmlHelper.RouteFriendlyName(f.PropertyName) == name);

            if (property == null)
                return null;

            model.TypeName = $"{property.PropertyName} Property";
            model.Value = property.Value;
            model.ShortDescription = property.ShortDescription;
            model.LongDescription = property.LongDescription;
            model.Summary = property.Summary;

            foreach (DocumentExample item in property.Examples)
            {
                model.ExampleUseage += $"<p>{item.Text}</p>";
            }

            foreach (DocumentException exception in property.Exception)
            {
                model.Exceptions += $"<p>{exception.ExceptionName}<br />{exception.Summary}";
            }

            model.Remarks = property.Remarks;

            return model;
        }

        private DocumentViewTypeViewModel BuildFieldViewModel(DocumentViewTypeViewModel model, Document selected, string name)
        {
            DocumentField field = selected.Fields.FirstOrDefault(f => HtmlHelper.RouteFriendlyName(f.FieldName) == name);

            if (field == null)
                return null;

            model.TypeName = field.FieldName;
            model.Value = field.Value;
            model.ShortDescription = field.ShortDescription;
            model.LongDescription = field.LongDescription;
            model.Summary = field.Summary;

            foreach (DocumentExample item in field.Examples)
            {
                model.ExampleUseage += $"<p>{item.Text}</p>";
            }

            foreach (DocumentException exception in field.Exception)
            {
                model.Exceptions += $"<p>{exception.ExceptionName}<br />{exception.Summary}";
            }

            model.Remarks = field.Remarks;

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
            {
                selected = documents.FirstOrDefault(d => HtmlHelper.RouteFriendlyName(d.Title) == documentName);
            }
            else
            {
                selected = _documentationService.GetDocuments()
                    .FirstOrDefault(d => HtmlHelper.RouteFriendlyName(d.AssemblyName).Equals(documentName, StringComparison.InvariantCultureIgnoreCase) &&
                                (
                                    HtmlHelper.RouteFriendlyName(d.Title).Equals(className, StringComparison.InvariantCultureIgnoreCase) ||
                                    HtmlHelper.RouteFriendlyName(d.ClassName).Equals(className, StringComparison.InvariantCultureIgnoreCase)
                                ));
            }

            if (selected == null)
                return null;

            DocumentData data = (DocumentData)selected.Tag;

            if (data.ReferenceData == null)
            {
                data.ReferenceData = GetAllReferences(selected, data, documents);
            }

            DocumentViewModel model = new DocumentViewModel(GetModelData(),
                selected.Title, selected.ShortDescription, selected.LongDescription, data.ReferenceData);

            if (selected.DocumentType != DocumentType.Custom && selected.DocumentType != DocumentType.Document)
            {
                model.Assembly = selected.AssemblyName;
                model.Namespace = selected.NameSpaceName;
            }

            if (data.SeeAlso != null && data.SeeAlso.Count > 0)
            {
                foreach (KeyValuePair<string, string> item in data.SeeAlso)
                    model.SeeAlso.Add(item.Key, item.Value);
            }

            if (selected.SeeAlso != null && selected.SeeAlso.Count > 0)
            {
                foreach (KeyValuePair<string, string> item in selected.SeeAlso)
                    model.SeeAlso.Add(item.Key, item.Value);
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

            foreach (DocumentException exception in selected.Exception)
            {
                model.Exceptions += $"<p>{exception.ExceptionName}<br />{exception.Summary}";
            }

            model.Remarks = selected.Remarks;

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

#pragma warning restore CS1591