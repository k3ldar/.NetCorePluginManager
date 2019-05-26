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
 *  File: DefaultDocumentationService.cs
 *
 *  Purpose:  Provides default implementation of documentation service
 *
 *  Date        Name                Reason
 *  19/05/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

using Shared;
using Shared.Classes;
using Shared.Docs;

using SharedPluginFeatures;

namespace DocumentationPlugin.Classes
{
    internal sealed class DefaultDocumentationService : IDocumentationService
    {
        #region Private Members

        private static readonly object _lockObject = new object();
        private readonly DocumentationSettings _documentation;
        private readonly IMemoryCache _memoryCache;
        private readonly string _fileNameFile;
        private readonly string _xmlFilePath;
        private readonly string _customDataPath;

        #endregion Private Members

        #region Constructors

        public DefaultDocumentationService(ISettingsProvider settingsProvider, IMemoryCache memoryCache)
        {
           if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _documentation = settingsProvider.GetSettings<DocumentationSettings>(nameof(DocumentationSettings));
            _fileNameFile = Path.GetFullPath(Path.Combine(_documentation.Path, "Settings", "Files.dat"));
            _xmlFilePath = Path.GetFullPath(Path.Combine(_documentation.Path, "XmlFiles"));
            _customDataPath = Path.GetFullPath(Path.Combine(_documentation.Path, "Custom"));

            if (!Directory.Exists(_documentation.Path))
                Directory.CreateDirectory(_documentation.Path);

            if (!Directory.Exists(_xmlFilePath))
                Directory.CreateDirectory(_xmlFilePath);

            if (!Directory.Exists(_customDataPath))
                Directory.CreateDirectory(_customDataPath);
        }

        #endregion Constructors

        #region IDocumentationService Methods

        public List<string> GetDocumentationFiles()
        {
            CacheItem cache = _memoryCache.GetCache().Get(Constants.DocumentationFileCache);

            if (cache == null)
            {
                cache = new CacheItem(Constants.DocumentationFileCache, GetDocumentationFileNames());
                _memoryCache.GetCache().Add(Constants.DocumentationFileCache, cache);
            }

            return (List<string>)cache.Value;
        }

        public List<Document> GetDocuments()
        {
            using (TimedLock doclock = TimedLock.Lock(_lockObject))
            {
                CacheItem cache = _memoryCache.GetCache().Get(Constants.DocumentationListCache);

                if (cache == null)
                {
                    DocumentBuilder builder = new DocumentBuilder();
                    List<Document> documents = new List<Document>();

                    foreach (string file in GetDocumentationFileNames())
                        builder.LoadDocuments(documents, Path.Combine(_xmlFilePath, file));

                    foreach (Document doc in documents)
                    {
                        if (doc.DocumentType == DocumentType.Custom)
                            doc.AssemblyName = doc.Title;

                        ProcessDocument(doc);
                        BuildReferences(doc, documents);

                        if (String.IsNullOrEmpty(doc.ShortDescription))
                            doc.ShortDescription = doc.Summary;
                    }

                    BuildAllReferences(documents);

                    SetParentData(documents);

                    foreach (Document doc in documents)
                    {
                        UpdateClassReferences(doc, documents);
                    }

                    SetPreviousNext(documents);

                    cache = new CacheItem(Constants.DocumentationListCache, documents);
                    _memoryCache.GetCache().Add(Constants.DocumentationListCache, cache);
                }

                return (List<Document>)cache.Value;
            }
        }

        public string GetCustomData(in string name, in string defaultValue)
        {
            string fileName = Path.Combine(_customDataPath, name + ".dat");

            if (File.Exists(fileName))
            {
                return Utilities.FileRead(fileName, false);
            }
            else
            {
                string contents = defaultValue ?? String.Empty;

                if (!String.IsNullOrEmpty(contents))
                    Utilities.FileWrite(fileName, contents);
            }

            return defaultValue ?? String.Empty;
        }

        public int GetCustomSortOrder(in string name, in int defaultValue)
        {
            string fileName = Path.Combine(_customDataPath, name + ".dat");
            int Result = defaultValue;

            if (File.Exists(fileName))
            {
                Int32.TryParse(Utilities.FileRead(fileName, false), out Result);
            }
            else
            {
                 Utilities.FileWrite(fileName, defaultValue.ToString());
            }

            return Result;
        }

        public void ProcessDocument(in Document document)
        {
            string docName = $"{document.AssemblyName}\\";

            if (!String.IsNullOrEmpty(document.NameSpaceName))
                docName += $"{document.NameSpaceName}\\";

            switch (document.DocumentType)
            {
                case DocumentType.Class:
                    docName += document.ClassName + "\\";

                    foreach (DocumentField field in document.Fields)
                        ProcessDocumentField(field);

                    foreach (DocumentMethod method in document.Methods)
                        ProcessDocumentMethod(method);

                    foreach (DocumentProperty property in document.Properties)
                        ProcessDocumentProperty(property);

                    break;

                case DocumentType.Custom:
                    docName += document.Title + "\\";

                    break;
            }

            document.Summary = GetCustomData(docName + nameof(document.Summary), document.Summary);
            document.ShortDescription = GetCustomData(docName + nameof(document.ShortDescription), document.ShortDescription);
            document.LongDescription = GetCustomData(docName + nameof(document.LongDescription), document.LongDescription);
            document.Value = GetCustomData(docName + nameof(document.Value), document.Value);
            document.AcquisitionMethod = GetCustomData(docName + nameof(document.AcquisitionMethod), document.AcquisitionMethod);
            document.ClassName = GetCustomData(docName + nameof(document.ClassName), document.ClassName);
            document.Example = GetCustomData(docName + nameof(document.Example), document.Example);
            document.ExampleUseage = GetCustomData(docName + nameof(document.ExampleUseage), document.ExampleUseage);
            document.Remarks = GetCustomData(docName + nameof(document.Remarks), document.Remarks);
            document.Returns = GetCustomData(docName + nameof(document.Returns), document.Returns);
            document.Title = GetCustomData(docName + nameof(document.Title), document.Title);
            document.Value = GetCustomData(docName + nameof(document.Value), document.Value);
            document.SortOrder = GetCustomSortOrder(docName + nameof(document.SortOrder), document.SortOrder);

            if (String.IsNullOrEmpty(document.Title))
            {
                switch (document.DocumentType)
                {
                    case DocumentType.Assembly:
                        document.Title = document.AssemblyName;
                        break;
                    case DocumentType.Class:
                        document.Title = document.ClassName;
                        break;
                }
            }
        }

        public void ProcessDocumentField(in DocumentField field)
        {
            string docName = $"{field.AssemblyName}\\{field.NameSpaceName}\\{field.ClassName}\\Fields\\{field.FieldName}\\";
            field.Summary = GetCustomData(docName + nameof(field.Summary), field.Summary);
            field.Value = GetCustomData(docName + nameof(field.Value), field.Value);
            field.ShortDescription = GetCustomData(docName + nameof(field.ShortDescription), field.ShortDescription);
            field.LongDescription = GetCustomData(docName + nameof(field.LongDescription), field.LongDescription);
        }

        public void ProcessDocumentMethod(in DocumentMethod method)
        {
            string docName = $"{method.AssemblyName}\\{method.NameSpaceName}\\{method.ClassName}\\Methods\\{method.MethodName}\\";
            method.Summary = GetCustomData(docName + nameof(method.Summary), method.Summary);
            method.Returns = GetCustomData(docName + nameof(method.Returns), method.Returns);
            method.ExampleUseage = GetCustomData(docName + nameof(method.ExampleUseage), method.ExampleUseage);
            method.ShortDescription = GetCustomData(docName + nameof(method.ShortDescription), method.ShortDescription);
            method.LongDescription = GetCustomData(docName + nameof(method.LongDescription), method.LongDescription);
            method.MethodName = GetCustomData(docName + nameof(method.MethodName), FixMethodName(method.MethodName, String.Empty));

            foreach (DocumentMethodParameter param in method.Parameters)
                ProcessDocumentMethodParameter(param);
        }

        public void ProcessDocumentMethodParameter(in DocumentMethodParameter parameter)
        {
            string docName = $"{parameter.AssemblyName}\\{parameter.NameSpaceName}\\{parameter.ClassName}\\Methods\\{parameter.MethodName}\\Parameters\\{parameter.ParameterName}\\";
            parameter.Summary = GetCustomData(docName + nameof(parameter.Summary), parameter.Summary);
            parameter.ParameterName = GetCustomData(docName + nameof(parameter.ParameterName), parameter.ParameterName);
            parameter.Value = GetCustomData(docName + nameof(parameter.Value), parameter.Value);
            parameter.ShortDescription = GetCustomData(docName + nameof(parameter.ShortDescription), parameter.ShortDescription);
            parameter.LongDescription = GetCustomData(docName + nameof(parameter.LongDescription), parameter.LongDescription);
        }

        public void ProcessDocumentProperty(in DocumentProperty property)
        {
            string docName = $"{property.AssemblyName}\\{property.NameSpaceName}\\{property.ClassName}\\Properties\\{property.PropertyName}\\";
            property.Summary = GetCustomData(docName + nameof(property.Summary), property.Summary);
            property.PropertyName = GetCustomData(docName + nameof(property.PropertyName), property.PropertyName);
            property.Value = GetCustomData(docName + nameof(property.Value), property.Value);
            property.ShortDescription = GetCustomData(docName + nameof(property.ShortDescription), property.ShortDescription);
            property.LongDescription = GetCustomData(docName + nameof(property.LongDescription), property.LongDescription);
        }

        #endregion IDocumentationService Methods

        #region Private Methods

        private void SetPreviousNext(List<Document> documents)
        {
            List<Document> topLevel = documents.Where(d => d.DocumentType == DocumentType.Assembly ||
                d.DocumentType == DocumentType.Custom ||
                d.DocumentType == DocumentType.Document)
                .OrderBy(o => o.SortOrder).ThenBy(o => o.Title)
                .ToList();

            for (int i = 0; i < topLevel.Count; i++)
            {
                if (i == 0)
                {
                    DocumentData next = topLevel[i + 1].Tag as DocumentData;
                    next.PreviousDocument = topLevel[i];
                }
                else if (i == topLevel.Count -1)
                {
                    DocumentData previous = topLevel[i -1].Tag as DocumentData;
                    previous.NextDocument = topLevel[i];
                }
                else
                {
                    DocumentData next = topLevel[i + 1].Tag as DocumentData;
                    DocumentData previous = topLevel[i - 1].Tag as DocumentData;
                    next.PreviousDocument = topLevel[i];
                    previous.NextDocument = topLevel[i];
                }
            }
        }

        private string FixMethodName(string name, string newName)
        {
            if (!name.Contains('('))
                name += "()";

            if (name.StartsWith("#ctor") && !String.IsNullOrEmpty(newName))
                name = name.Replace("#ctor", newName);

            return name.Replace(",", ", ");
        }

        private void BuildReferences(Document document, in List<Document> documents)
        {
            DocumentData data = new DocumentData();

            //extract all html H references (H1, H2 etc) from long description for all documents, 
            // to be used as "in this document"
            BuildCustomReferences(document, data, documents);

            if (document.DocumentType == DocumentType.Assembly)
                BuildAssemblyReferences(document, data, documents);
            else if (document.DocumentType == DocumentType.Class)
                BuildClassReferences(document, data, documents);

            document.Tag = data;
        }

        private void SetParentData(in List<Document> documents)
        {
            foreach (Document docParent in documents.Where(d => d.DocumentType == DocumentType.Assembly).ToList())
            {
                DocumentData parentData = (DocumentData)docParent.Tag;

                // set parent for all classes
                foreach (Document doc in documents.Where(d => d.DocumentType == DocumentType.Class && d.AssemblyName == docParent.AssemblyName).ToList())
                {
                    DocumentData childData = (DocumentData)doc.Tag;
                    childData.Parent = parentData;
                }
            }
        }

        private void UpdateClassReferences(in Document document, in List<Document> documents)
        {
            DocumentData data = (DocumentData)document.Tag;
            string route = $"/docs/Document/{HtmlHelper.RouteFriendlyName(document.AssemblyName)}/{HtmlHelper.RouteFriendlyName(data.ShortClassName)}/";

            foreach (Document doc in documents)
            {
                if (doc == document || String.IsNullOrEmpty(data.FullClassName) || String.IsNullOrEmpty(data.ShortClassName))
                    continue;

                doc.ShortDescription = StringReplaceClassName(doc.ShortDescription, 
                    data.FullClassName, data.ShortClassName, route, document, doc);
                doc.LongDescription = StringReplaceClassName(doc.LongDescription, 
                    data.FullClassName, data.ShortClassName, route, document, doc);
                doc.Remarks = StringReplaceClassName(doc.Remarks, data.FullClassName, 
                    data.ShortClassName, route, document, doc);
                doc.Returns = StringReplaceClassName(doc.Returns, data.FullClassName, 
                    data.ShortClassName, route, document, doc);
                doc.Summary = StringReplaceClassName(doc.Summary, data.FullClassName, 
                    data.ShortClassName, route, document, doc);

                foreach (var item in doc.Constructors)
                {
                    item.MethodName = item.MethodName.Replace("#ctor", item.ClassName).Replace(",", ", ");
                    item.ExampleUseage = StringReplaceClassName(item.ExampleUseage, 
                        data.FullClassName, data.ShortClassName, route, document, doc);
                    item.LongDescription = StringReplaceClassName(item.LongDescription, 
                        data.FullClassName, data.ShortClassName, route, document, doc);
                    item.Returns = StringReplaceClassName(item.Returns, data.FullClassName, 
                        data.ShortClassName, route, document, doc);
                    item.ShortDescription = StringReplaceClassName(item.ShortDescription,
                        data.FullClassName, data.ShortClassName, route, document, doc);
                    item.Summary = StringReplaceClassName(item.Summary, data.FullClassName, 
                        data.ShortClassName, route, document, doc);

                    foreach (var param in item.Parameters)
                    {
                        param.ParameterName = StringReplaceClassName(param.ParameterName, 
                            data.FullClassName, data.ShortClassName, route, document, doc);
                        param.LongDescription = StringReplaceClassName(param.LongDescription, 
                            data.FullClassName, data.ShortClassName, route, document, doc);
                        param.ShortDescription = StringReplaceClassName(param.LongDescription, 
                            data.FullClassName, data.ShortClassName, route, document, doc);
                        param.Summary = StringReplaceClassName(param.Summary, data.FullClassName, 
                            data.ShortClassName, route, document, doc);
                        param.Value = StringReplaceClassName(param.Value, data.FullClassName, 
                            data.ShortClassName, route, document, doc);
                    }
                }

                foreach (var item in doc.Methods)
                {
                    item.ExampleUseage = StringReplaceClassName(item.ExampleUseage, 
                        data.FullClassName, data.ShortClassName, route, document, doc);
                    item.LongDescription = StringReplaceClassName(item.LongDescription, 
                        data.FullClassName, data.ShortClassName, route, document, doc);
                    item.Returns = StringReplaceClassName(item.Returns, data.FullClassName, 
                        data.ShortClassName, route, document, doc);
                    item.ShortDescription = StringReplaceClassName(item.ShortDescription, 
                        data.FullClassName, data.ShortClassName, route, document, doc);
                    item.Summary = StringReplaceClassName(item.Summary, data.FullClassName, 
                        data.ShortClassName, route, document, doc);

                    foreach (var param in item.Parameters)
                    {
                        param.LongDescription = StringReplaceClassName(param.LongDescription, 
                            data.FullClassName, data.ShortClassName, route, document, doc);
                        param.ShortDescription = StringReplaceClassName(param.LongDescription, 
                            data.FullClassName, data.ShortClassName, route, document, doc);
                        param.Summary = StringReplaceClassName(param.Summary, data.FullClassName, 
                            data.ShortClassName, route, document, doc);
                        param.Value = StringReplaceClassName(param.Value, data.FullClassName, 
                            data.ShortClassName, route, document, doc);
                    }
                }

                foreach (var item in doc.Properties)
                {
                    item.LongDescription = StringReplaceClassName(item.LongDescription, 
                        data.FullClassName, data.ShortClassName, route, document, doc);
                    item.ShortDescription = StringReplaceClassName(item.ShortDescription, 
                        data.FullClassName, data.ShortClassName, route, document, doc);
                    item.Summary = StringReplaceClassName(item.Summary, data.FullClassName, 
                        data.ShortClassName, route, document, doc);
                    item.Value = StringReplaceClassName(item.Value, data.FullClassName, 
                        data.ShortClassName, route, document, doc);
                }

                foreach (var value in doc.Fields)
                {
                    value.LongDescription = StringReplaceClassName(value.LongDescription, 
                        data.FullClassName, data.ShortClassName, route, document, doc);
                    value.ShortDescription = StringReplaceClassName(value.ShortDescription, 
                        data.FullClassName, data.ShortClassName, route, document, doc);
                    value.Summary = StringReplaceClassName(value.Summary, data.FullClassName, 
                        data.ShortClassName, route, document, doc);
                    value.Value = StringReplaceClassName(value.Value, data.FullClassName, 
                        data.ShortClassName, route, document, doc);
                }

                DocumentData docData = (DocumentData)doc.Tag;

                if (docData.SeeAlso.Count > 0 && !docData.Contains.ContainsKey("#seealso"))
                    docData.Contains.Add("#seealso", nameof(Languages.LanguageStrings.SeeAlso));
            }

            if (data.SeeAlso.Count > 0 && !data.Contains.ContainsKey("#seealso"))
                data.Contains.Add("#seealso", nameof(Languages.LanguageStrings.SeeAlso));
        }

        private string StringReplaceClassName(in string value, in string fullClassName, in string shortClassName, 
            in string route, in Document priDoc, in Document subDoc)
        {
            if (String.IsNullOrEmpty(value) || priDoc.SortOrder < 0)
                return value;

            string[] words = value.Split(new char[] { ' ' }, StringSplitOptions.None);
            bool xRefSeeAlso = false;

            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];

                if (word.Trim().Equals(fullClassName, StringComparison.InvariantCultureIgnoreCase))
                {
                    words[i] = $"<a href=\"{route}\">{fullClassName}</a>";

                    if (!xRefSeeAlso)
                        xRefSeeAlso = true;
                }
                else if (word.Equals(shortClassName, StringComparison.InvariantCultureIgnoreCase))
                {
                    words[i] = $"<a href=\"{route}\">{shortClassName}</a>";

                    if (!xRefSeeAlso)
                        xRefSeeAlso = true;
                }
                else if (word.Contains(fullClassName, StringComparison.InvariantCultureIgnoreCase))
                {
                    words[i] = word.Replace(fullClassName, $"<a href=\"{route}\">{fullClassName}</a>");

                    if (!xRefSeeAlso)
                        xRefSeeAlso = true;
                }
            }

            if (xRefSeeAlso)
            {
                DocumentData data1 = (DocumentData)priDoc.Tag;
                string route1 = $"/docs/Document/{HtmlHelper.RouteFriendlyName(priDoc.AssemblyName)}/{HtmlHelper.RouteFriendlyName(data1.ShortClassName)}/";

                DocumentData data2 = (DocumentData)subDoc.Tag;
                string route2 = $"/docs/Document/{HtmlHelper.RouteFriendlyName(subDoc.AssemblyName)}/{HtmlHelper.RouteFriendlyName(data2.ShortClassName)}/";

                if (!data1.SeeAlso.ContainsKey(route2))
                    data1.SeeAlso.Add(route2, subDoc.Title);

                if (!data2.SeeAlso.ContainsKey(route1))
                    data2.SeeAlso.Add(route1, priDoc.Title);
            }

            return String.Join(' ', words);
        }

        private void BuildAssemblyReferences(in Document document, in DocumentData data, in List<Document> documents)
        {
            foreach (Document doc in documents)
            {
                if (doc.AssemblyName == document.AssemblyName && doc.DocumentType == DocumentType.Class)
                {
                    string route = $"/docs/Document/{HtmlHelper.RouteFriendlyName(document.Title)}/{HtmlHelper.RouteFriendlyName(doc.Title)}/";
                    data.KeyNames.Add(route, doc.Title);
                }
            }
        }

        private void BuildClassReferences(in Document document, in DocumentData data, in List<Document> documents)
        {
            if (!String.IsNullOrEmpty(document.AcquisitionMethod))
                data.Contains.Add("#acquire", nameof(Languages.LanguageStrings.Acquisition));

            if (!String.IsNullOrEmpty(document.Returns))
                data.Contains.Add("#return", nameof(Languages.LanguageStrings.ReturnValue));

            if (!String.IsNullOrEmpty(document.Value))
                data.Contains.Add("#value", nameof(Languages.LanguageStrings.ApiValue));

            if (!String.IsNullOrEmpty(document.Example))
                data.Contains.Add("#example", nameof(Languages.LanguageStrings.Example));

            if (document.Constructors.Count > 0)
                data.Contains.Add("#constructors", nameof(Languages.LanguageStrings.Constructors));

            if (document.Properties.Count > 0)
                data.Contains.Add("#properties", nameof(Languages.LanguageStrings.Properties));

            if (document.Methods.Count > 0)
                data.Contains.Add("#methods", nameof(Languages.LanguageStrings.Methods));

            if (document.Fields.Count > 0)
                data.Contains.Add("#fields", nameof(Languages.LanguageStrings.Fields));

            if (!String.IsNullOrEmpty(document.Remarks))
                data.Contains.Add("#acquire", nameof(Languages.LanguageStrings.Remarks));

            data.ShortClassName = document.ClassName;
            data.FullClassName = $"{document.NameSpaceName}.{document.ClassName}";
        }

        private void BuildCustomReferences(in Document document, in DocumentData data, in List<Document> documents)
        {
            int nextHStart = document.LongDescription.IndexOf("<h");

            if (nextHStart == -1)
                return;

            int nextHEnd;

            while (nextHStart > -1)
            {
                nextHEnd = document.LongDescription.IndexOf("</h", nextHStart + 1);

                if (nextHEnd > nextHStart)
                {
                    string reference = document.LongDescription.Substring(nextHStart, nextHEnd - nextHStart);
                    string idName = HtmlHelper.RouteFriendlyName(reference.Substring(4).ToLower());

                    string hType = reference.Substring(0, 3);

                    string newhRef = $"{hType} id=\"{idName}\">" + reference.Substring(4);
                    document.LongDescription = document.LongDescription.Replace(reference, newhRef);

                    if (!data.Contains.ContainsKey(idName))
                        data.Contains.Add("#" + idName, reference.Substring(4));
                }
                else
                    break;

                nextHStart = document.LongDescription.IndexOf("<h", nextHEnd + 1);
            }
        }

        private void BuildAllReferences(in List<Document> documents)
        {
            StringBuilder allReferences = new StringBuilder("<ul>", 2048);

            foreach (Document selected in documents)
            {
                allReferences.Clear();
                DocumentData selectedData = (DocumentData)selected.Tag;

                foreach (Document doc in documents)
                {
                    DocumentData data = (DocumentData)doc.Tag;


                    if (doc == selected)
                    {
                        allReferences.Append("<ul>");

                        foreach (KeyValuePair<string, string> reference in data.KeyNames)
                        {
                            allReferences.Append($"<li><a href=\"{reference.Key}\">{reference.Value}</a></li>");
                        }

                        allReferences.Append("</ul>");
                    }
                }

                selectedData.AllReferences = allReferences.ToString();
            }
        }

        private List<string> GetDocumentationFileNames()
        {
            List<string> Result = new List<string>();

            if (File.Exists(_fileNameFile))
            {
                string[] fileNames = Utilities.FileRead(_fileNameFile, false).Split('\n');

                foreach (string s in fileNames)
                    Result.Add(s);
            }

            if (UpdateMissingFileNames(Result))
                SaveFileList(Result, _fileNameFile);

            return Result;
        }

        private bool UpdateMissingFileNames(in List<string> fileList)
        {
            bool Result = false;

            string[] existingFiles = Directory.GetFiles(_xmlFilePath, "*.xml");

            foreach (string file in existingFiles)
            {
                string fileName = Path.GetFileName(file);

                if (!fileList.Contains(fileName))
                {
                    fileList.Add(fileName);

                    if (!Result)
                        Result = true;
                }
            }

            return Result;
        }

        private void SaveFileList(in List<string> files, in string fileName)
        {
            Utilities.FileWrite(_fileNameFile, String.Join('\n', files.ToArray()));
        }

        #endregion Private Methods
    }
}
