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

using Shared;
using Shared.Classes;
using Shared.Docs;

using SharedPluginFeatures;

namespace LoginPlugin.Classes
{
    public sealed class DefaultDocumentationService : IDocumentationService
    {
        #region Private Members

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
            CacheItem cache = _memoryCache.GetCache().Get(Constants.DocumentationListCache);

            if (cache == null)
            {
                Shared.Docs.DocumentBuilder builder = new Shared.Docs.DocumentBuilder();
                List<Document> documents = new List<Document>();

                foreach (string file in GetDocumentationFileNames())
                    builder.LoadDocuments(documents, Path.Combine(_xmlFilePath, file));

                foreach (Document doc in documents)
                {
                    if (doc.DocumentType == DocumentType.Custom)
                        doc.AssemblyName = doc.Title;

                    ProcessDocument(doc);
                }

                cache = new CacheItem(Constants.DocumentationListCache, documents);
                _memoryCache.GetCache().Add(Constants.DocumentationListCache, cache);
            }

            return (List<Document>)cache.Value;
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
